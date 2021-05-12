using Controllers.Core;
using System.Collections.Generic;
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
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        private void Start()
        {
            TranslateLabels();
            BindClickEvents();
        }

        private void TranslateLabels()
        {
            if (!LocalizationController.Instance) return;

            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetConfirmBoxLabels())
            {
                labels.Add(label);
            }

            if (labels.Count == 0 || uiLabels.Count == 0 || labels.Count != uiLabels.Count) return;
            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }
        }

        private void BindClickEvents()
        {
            // Cancels
            if (!noButton || !yesButton) return;

            // Closes the panel
            noButton.onClick.AddListener(() =>
            {
                if (!configurationPanel || !confirmBox) return;
                confirmBox.SetActive(false);
                configurationPanel.SetActive(true);
            });

            // Reset progress...
            yesButton.onClick.AddListener(() =>
            {
                // Checks and cancels
                if (!selectLevelsPanel || !confirmBox) return;
                if (!SelectLevelsController.Instance) return;

                // Resets all progress and updates buttons
                SelectLevelsController.Instance.ResetProgress();
                SelectLevelsController.Instance.CleanLevelButtons();
                confirmBox.SetActive(false);
                selectLevelsPanel.SetActive(true);
            });
        }
    }
}