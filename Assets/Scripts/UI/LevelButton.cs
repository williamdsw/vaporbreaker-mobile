using MVC.Models;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Level Button Prefab
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(LayoutElement))]
    public class LevelButton : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Elements")]
        [SerializeField] private GameObject staticEffect;
        [SerializeField] private TextMeshProUGUI levelNumberText;

        // || Cached

        private Button button;

        private void Awake() => GetRequiredComponents();

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                button = GetComponent<Button>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set properties for this element
        /// </summary>
        /// <param name="index"> Index of Level </param>
        /// <param name="level"> Instance of Level </param>
        /// <param name="callback"> Callback to be called on click </param>
        public void Set(int index, Level level, Action<Level, int> callback)
        {
            try
            {
                levelNumberText.SetText((index + 1).ToString("000"));
                levelNumberText.gameObject.SetActive(level.IsUnlocked);
                staticEffect.SetActive(!level.IsUnlocked);
                button.interactable = level.IsUnlocked;
                button.onClick.AddListener(() => callback(level, index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}