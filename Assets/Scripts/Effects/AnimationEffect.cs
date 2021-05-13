using UnityEngine;
using UnityEngine.UI;

namespace Effects
{
    public class AnimationEffect : MonoBehaviour
    {
        // Config
        [SerializeField] private Sprite[] spritesToAnimateList;
        [SerializeField] private bool randomFPS = false;
        [SerializeField] private float framesPerSecond = 10;

        // Cached Components
        private Image image;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            image = GetComponent<Image>();
            if (!image)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Start() => framesPerSecond = (randomFPS ? Random.Range(5, 30) : framesPerSecond);

        private void FixedUpdate() => AnimateImage();

        private void AnimateImage()
        {
            if (spritesToAnimateList.Length == 0) return;

            int index = (int)(Time.fixedTime * framesPerSecond) % spritesToAnimateList.Length;

            if (image)
            {
                image.sprite = spritesToAnimateList[index];
            }
            else if (spriteRenderer)
            {
                spriteRenderer.sprite = spritesToAnimateList[index];
            }
        }
    }
}