using Controllers.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Menu
{
    public class AboutController : MonoBehaviour
    {
        [Header("Main Elements")]
        [SerializeField] private GameObject configurationPanel;
        [SerializeField] private GameObject aboutPanel;
        [SerializeField] private Button quitButton;
        [SerializeField] private TextMeshProUGUI creditsText;

        [Header("Labels to Translate")]
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        private void Start()
        {
            TranslateLabels();
            BindClickEvents();
            LoadCredits();
        }

        // Translate labels based on choosed language
        private void TranslateLabels()
        {
            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetAboutLabels())
            {
                labels.Add(label);
            }

            if (labels.Count == 0 || uiLabels.Count == 0) return;
            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }
        }

        private void BindClickEvents()
        {
            quitButton.onClick.AddListener(() =>
            {
                if (SelectLevelsController.Instance.ActualGameState != Enumerators.GameStates.GAMEPLAY) return;

                aboutPanel.SetActive(false);
                configurationPanel.SetActive(true);
            });
        }

        private void LoadCredits()
        {
            StartCoroutine(FileManager.LoadAssetAsync(FileManager.OtherFolderPath, FileManager.CreditsPath,
            content =>
            {
                if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content)) return;
                creditsText.SetText(content);
            }));
        }
    }
}