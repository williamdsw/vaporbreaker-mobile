using Controllers.Core;
using MVC.Enums;
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
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI creditsText;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TextMeshProUGUI copyrightLabel;

        private void Start()
        {
            TranslateLabels();
            BindClickEvents();
        }

        private void TranslateLabels()
        {
            titleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_about);
            copyrightLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.about_rights);
            creditsText.text = LocalizationController.Instance.GetWord(LocalizationFields.messages_credits);
        }

        private void BindClickEvents()
        {
            backButton.onClick.AddListener(() =>
            {
                if (SelectLevelsController.Instance.ActualGameState != Enumerators.GameStates.GAMEPLAY) return;

                aboutPanel.SetActive(false);
                configurationPanel.SetActive(true);
            });
        }
    }
}