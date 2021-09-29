using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Effects
{
    public class FlashTextEffect : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Configuration")]
        [SerializeField] private float timeToFlick = 0.01f;
        [SerializeField] private bool isLooping = true;

        // || Cached

        private TextMeshPro textMeshPro;
        private TextMeshProUGUI textMeshProUGUI;

        // || Properties

        public float TimeToFlick { get => timeToFlick; set => timeToFlick = value; }

        private void Awake() => GetRequiredComponents();

        private void Start() => StartCoroutine(Flash());

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                textMeshPro = GetComponent<TextMeshPro>();
                if (!textMeshPro)
                {
                    textMeshPro = GetComponentInChildren<TextMeshPro>();
                }

                if (!textMeshPro)
                {
                    textMeshProUGUI = GetComponent<TextMeshProUGUI>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Flashes current text
        /// </summary>
        private IEnumerator Flash()
        {
            while (isLooping)
            {
                if (textMeshPro)
                {
                    string text = textMeshPro.text;
                    if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                    {
                        yield return null;
                    }
                }
                else if (textMeshProUGUI)
                {
                    string text = textMeshProUGUI.text;
                    if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                    {
                        yield return null;
                    }
                }

                // Color
                Color color = (textMeshPro ? textMeshPro.color : textMeshProUGUI.color);
                color.a = (color.a == 1f ? 0f : 1f);

                if (textMeshPro)
                {
                    textMeshPro.color = color;
                }
                else if (textMeshProUGUI)
                {
                    textMeshProUGUI.color = color;
                }

                yield return new WaitForSeconds(TimeToFlick);
                yield return null;
            }
        }
    }
}