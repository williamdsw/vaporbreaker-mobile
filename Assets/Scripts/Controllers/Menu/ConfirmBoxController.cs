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
            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetConfirmBoxLabels())
            {
                labels.Add(label);
            }

            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }
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