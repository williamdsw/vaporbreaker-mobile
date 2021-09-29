using Controllers.Core;
using MVC.BL;
using MVC.Enums;
using MVC.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Menu
{
    public class ScoreboardController : MonoBehaviour
    {
        // Config
        [Header("Required UI Elements")]
        [SerializeField] private GameObject levelDetailsPanel;
        [SerializeField] private GameObject scoreboardPanel;
        [SerializeField] private Button backButton;

        [Header("Required Scoreboard Table Elements")]
        [SerializeField] private Transform tableContent;
        [SerializeField] private GameObject rowPrefab;
        [SerializeField] private GameObject columnPrefab;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TextMeshProUGUI scoreColumnLabel;
        [SerializeField] private TextMeshProUGUI timeScoreColumnLabel;

        // || Cached

        private ScoreboardBL scoreboardBL;
        private TextMeshProUGUI backButtonText;

        // || Properties

        public static ScoreboardController Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            scoreboardBL = new ScoreboardBL();
            backButtonText = backButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            TranslateLabels();
            BindClickEvents();
        }

        private void TranslateLabels()
        {
            titleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_scoreboard);
            scoreColumnLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_score);
            timeScoreColumnLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_time);
            backButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_back);
        }

        private void BindClickEvents()
        {
            backButton.onClick.AddListener(() =>
            {
                Toggle(false);
                levelDetailsPanel.SetActive(true);
            });
        }

        private void Toggle(bool show) => scoreboardPanel.SetActive(show);

        public void RenderScoreboard(long levelId) => StartCoroutine(FillTable(levelId));

        private IEnumerator ClearTable()
        {
            foreach (Transform child in tableContent)
            {
                Destroy(child.gameObject);
            }

            yield return new WaitUntil(() => tableContent.childCount == 0);
        }

        private IEnumerator FillTable(long levelId)
        {
            yield return StartCoroutine(ClearTable());
            Toggle(true);

            List<Scoreboard> scoreboards = scoreboardBL.ListByLevel(levelId);
            int ranking = 1;
            foreach (Scoreboard item in scoreboards)
            {
                GameObject row = Instantiate(rowPrefab);
                row.transform.SetParent(tableContent);
                row.transform.SetAsLastSibling();

                RectTransform rectTransform = row.GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.one;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 120);

                CreateColumn(row.transform, ranking.ToString(), 200);
                CreateColumn(row.transform, Formatter.FormatToCurrency(item.Score), 400);
                CreateColumn(row.transform, Formatter.GetEllapsedTimeInHours(item.TimeScore), 500);

                ranking++;
            }
        }

        private GameObject CreateColumn(Transform rowParent, string value, int width)
        {
            GameObject column = Instantiate(columnPrefab);
            column.transform.SetParent(rowParent);
            column.transform.SetAsLastSibling();

            TextMeshProUGUI textMeshPro = column.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.text = value;

            RectTransform rectTransform = column.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);

            return gameObject;
        }
    }
}