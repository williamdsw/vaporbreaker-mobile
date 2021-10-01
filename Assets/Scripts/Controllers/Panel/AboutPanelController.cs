using Controllers.Core;
using MVC.Enums;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Panel
{
    /// <summary>
    /// Controller for About Panel
    /// </summary>
    public class AboutPanelController : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Elements")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button backButton;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TextMeshProUGUI copyrightLabel;

        // || Properties

        public static AboutPanelController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            Translate();
            BindEventListeners();
        }

        /// <summary>
        /// Translates the UI
        /// </summary>
        private void Translate()
        {
            try
            {
                titleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_about);
                copyrightLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.about_rights);
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
                    TogglePanel(false);
                    ConfigurationPanelController.Instance.TogglePanel(true);
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