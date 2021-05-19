using Controllers.Core;
using MVC.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Menu
{
    public class ConfirmBoxController : MonoBehaviour
    {
        [Header("Main Elements")]
        [SerializeField] private GameObject configurationPanel;
        [SerializeField] private GameObject selectLevelsPanel;
        [SerializeField] private GameObject confirmBox;
        [SerializeField] private Button noButton;
        [SerializeField] private Button yesButton;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TextMeshProUGUI messageText;

        // || Cached

        private TextMeshProUGUI noButtonText;
        private TextMeshProUGUI yesButtonText;

        private void Awake()
        {
            noButtonText = noButton.GetComponentInChildren<TextMeshProUGUI>();
            yesButtonText = yesButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            TranslateLabels();
            BindClickEvents();
        }

        private void TranslateLabels()
        {
            titleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.resetprogress_areyousure);
            messageText.text = LocalizationController.Instance.GetWord(LocalizationFields.resetprogress_warning);
            noButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_no);
            yesButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_yes);
        }

        private void BindClickEvents()
        {
            // Closes the panel
            noButton.onClick.AddListener(() =>
            {
                confirmBox.SetActive(false);
                configurationPanel.SetActive(true);
            });

            // Reset progress...
            yesButton.onClick.AddListener(() =>
            {
                // Resets all progress and updates buttons
                SelectLevelsController.Instance.ResetProgress();
                StartCoroutine(SelectLevelsController.Instance.ClearLevelButtons());
                confirmBox.SetActive(false);
                selectLevelsPanel.SetActive(true);
            });
        }
    }
}