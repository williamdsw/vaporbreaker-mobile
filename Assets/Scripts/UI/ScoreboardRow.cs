using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class ScoreboardRow : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Columns")]
        [SerializeField] private TextMeshProUGUI orderColumnValue;
        [SerializeField] private TextMeshProUGUI scoreColumnValue;
        [SerializeField] private TextMeshProUGUI timeScoreColumnValue;
        [SerializeField] private TextMeshProUGUI bestComboColumnValue;
        [SerializeField] private Color32 oddColor;
        [SerializeField] private Color32 evenColor;

        // || Cached

        private Image image;

        private void Awake() => GetRequiredComponents();

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                image = GetComponent<Image>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Set(int order, long score, long timeScore, long bestCombo, bool isOdd)
        {
            try
            {
                orderColumnValue.text = order.ToString();
                scoreColumnValue.text = Formatter.FormatToCurrency(score);
                timeScoreColumnValue.text = Formatter.GetEllapsedTimeInHours((int)timeScore);
                bestComboColumnValue.text = bestCombo.ToString();
                image.color = (isOdd ? oddColor : evenColor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}