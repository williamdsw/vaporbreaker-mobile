using System;
using UnityEngine;
using UnityEngine.UI;

namespace Effects
{
    /// <summary>
    /// Animation effect with sprite array
    /// </summary>
    public class AnimationEffect : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Configuration")]
        [SerializeField] private Sprite[] spritesToAnimateList;
        [SerializeField] private bool randomFPS = false;
        [SerializeField] private float framesPerSecond = 10;

        // || Cached

        private Image image;
        private SpriteRenderer spriteRenderer;

        // || Config

        private readonly Vector2Int MIN_MAX_FPS = new Vector2Int(5, 30);

        private void Awake() => GetRequiredComponents();

        private void Start()
        {
            framesPerSecond = (randomFPS ? UnityEngine.Random.Range(MIN_MAX_FPS.x, MIN_MAX_FPS.y) : framesPerSecond);
        }

        private void FixedUpdate() => AnimateImage();

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                image = GetComponent<Image>();
                if (!image)
                {
                    spriteRenderer = GetComponent<SpriteRenderer>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Animates image or sprite rendered based on frames
        /// </summary>
        private void AnimateImage()
        {
            if (spritesToAnimateList.Length == 0) return;

            int index = (int)(Time.fixedTime * framesPerSecond) % spritesToAnimateList.Length;

            if (image)
            {
                image.sprite = spritesToAnimateList[index];
            }
            else
            {
                spriteRenderer.sprite = spritesToAnimateList[index];
            }
        }
    }
}