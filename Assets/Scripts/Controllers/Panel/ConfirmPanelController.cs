using Controllers.Core;
using MVC.Enums;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Panel
{
    /// <summary>
    /// Controller for Confirm Panel
    /// </summary>
    public class ConfirmPanelController : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Elements")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button noButton;
        [SerializeField] private Button yesButton;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TextMeshProUGUI messageText;

        // || Cached

        private TextMeshProUGUI noButtonLabel;
        private TextMeshProUGUI yesButtonLabel;

        // || Properties

        public static ConfirmPanelController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
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
                noButtonLabel = noButton.GetComponentInChildren<TextMeshProUGUI>();
                yesButtonLabel = yesButton.GetComponentInChildren<TextMeshProUGUI>();
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
                titleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.resetprogress_areyousure);
                messageText.text = LocalizationController.Instance.GetWord(LocalizationFields.resetprogress_warning);
                noButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_no);
                yesButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_yes);
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
                noButton.onClick.AddListener(() =>
                {
                    TogglePanel(false);
                    ConfigurationPanelController.Instance.TogglePanel(true);
                });

                yesButton.onClick.AddListener(() =>
                {
                    SelectLevelsPanelController.Instance.ResetProgress();
                    StartCoroutine(SelectLevelsPanelController.Instance.ClearLevels());
                    TogglePanel(false);
                    SelectLevelsPanelController.Instance.TogglePanel(true);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Show or hide the panel
        /// </summary>
        /// <param name="toShow"> Is to show the panel ? </param>
        public void TogglePanel(bool toShow)
        {
            panel.SetActive(toShow);

            if (toShow)
            {
                Translate();
            }
        }
    }
}