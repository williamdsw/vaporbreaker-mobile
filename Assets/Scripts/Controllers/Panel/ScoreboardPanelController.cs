using Controllers.Core;
using MVC.BL;
using MVC.Enums;
using MVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Panel
{
    /// <summary>
    /// Controller for Scoreboard Panel
    /// </summary>
    public class ScoreboardPanelController : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Elements")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button backButton;

        [Header("Required Scoreboard Table Elements")]
        [SerializeField] private Transform tableBodyViewportContent;
        [SerializeField] private ScoreboardRow scoreboardRowPrefab;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TextMeshProUGUI scoreColumnLabel;
        [SerializeField] private TextMeshProUGUI timeScoreColumnLabel;
        [SerializeField] private TextMeshProUGUI comboColumnLabel;

        // || Cached

        private ScoreboardBL scoreboardBL;
        private TextMeshProUGUI backButtonText;

        // || Properties

        public static ScoreboardPanelController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            scoreboardBL = new ScoreboardBL();
            GetRequiredComponents();
            Translate();
            BindEventListeners();
        }

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                backButtonText = backButton.GetComponentInChildren<TextMeshProUGUI>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Translates the UI
        /// </summary>
        private void Translate()
        {
            try
            {
                titleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_scoreboard);
                scoreColumnLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_score);
                timeScoreColumnLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_time);
                comboColumnLabel.text = "Combo"; // !TODO
                backButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_back);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Bind event listeners to elements
        /// </summary>
        private void BindEventListeners()
        {
            try
            {
                backButton.onClick.AddListener(() =>
                {
                    panel.SetActive(false);
                    LevelDetailsPanelController.Instance.TogglePanel(true);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Start to render the scoreboard
        /// </summary>
        /// <param name="levelId"> Level Id </param>
        public void Show(long levelId)
        {
            Translate();
            StartCoroutine(ListScoreboard(levelId));
            panel.SetActive(true);
        }

        /// <summary>
        /// Fill table with items
        /// </summary>
        /// <param name="levelId"> Level Id </param>
        private IEnumerator ListScoreboard(long levelId)
        {
            yield return ClearTable();

            int index = 0;
            foreach (Scoreboard item in scoreboardBL.ListByLevel(levelId))
            {
                ScoreboardRow clone = Instantiate(scoreboardRowPrefab);
                clone.Set(index + 1, item.Score, item.TimeScore, item.BestCombo, (index % 2 == 1));
                clone.gameObject.transform.SetParent(tableBodyViewportContent);
                clone.gameObject.transform.SetAsLastSibling();

                RectTransform rectTransform = clone.GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.one;

                index++;
            }
        }

        /// <summary>
        /// Clear table items
        /// </summary>
        private IEnumerator ClearTable()
        {
            foreach (Transform child in tableBodyViewportContent)
            {
                Destroy(child.gameObject);
            }

            yield return new WaitUntil(() => tableBodyViewportContent.childCount == 0);
        }
    }
}