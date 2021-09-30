using Controllers.Core;
using Effects;
using System;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Core
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class DeathZone : MonoBehaviour
    {
        // || Cached

        private EdgeCollider2D edgeCollider;

        private void Awake() => GetRequiredComponents();

        private void Start() => DefineColliderPoints();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (other.gameObject.CompareTag(NamesTags.Tags.Ball))
                {
                    DealWithBallCollision(other.gameObject);
                }
                else if (other.gameObject.CompareTag(NamesTags.Tags.PowerUp))
                {
                    Destroy(other.gameObject);
                }
            }
        }

        /// <summary>
        /// Get required components
        /// </summary>
        public void GetRequiredComponents()
        {
            try
            {
                edgeCollider = GetComponent<EdgeCollider2D>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set collider points with Screen size
        /// </summary>
        private void DefineColliderPoints()
        {
            try
            {
                // Size Vectors
                Vector2 screenHeight = new Vector2(0, Screen.height);
                Vector2 screenSize = new Vector2(Screen.width, Screen.height);
                Vector2 screenWidth = new Vector2(Screen.width, 0);

                // Converts
                Vector2 lowerLeftCorner = Camera.main.ScreenToWorldPoint(Vector2.zero);
                Vector2 upperLeftCorner = Camera.main.ScreenToWorldPoint(screenHeight);
                Vector2 upperRightCorner = Camera.main.ScreenToWorldPoint(screenSize);
                Vector2 lowerRightCorner = Camera.main.ScreenToWorldPoint(screenWidth);
                Vector2 padding = new Vector2(2, 2);

                edgeCollider.points = new Vector2[]
                {
                    lowerLeftCorner - padding,
                    new Vector2(upperLeftCorner.x - padding.x, upperLeftCorner.y + padding.y),
                    upperRightCorner + padding,
                    new Vector2(lowerRightCorner.x + padding.x, lowerRightCorner.y - padding.y),
                    lowerLeftCorner - padding,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deal with ball collision
        /// </summary>
        /// <param name="otherBall"> Other ball to compare </param>
        private void DealWithBallCollision(GameObject otherBall)
        {
            try
            {
                if (FindObjectsOfType<Ball>().Length == 1)
                {
                    StartCoroutine(WaitToReset());
                }
                else
                {
                    AudioController.Instance.PlaySFX(AudioController.Instance.BoomSound, AudioController.Instance.MaxSFXVolume / 2);
                    GameSessionController.Instance.CurrentNumberOfBalls--;
                }

                Destroy(otherBall);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Wait to reset the game
        /// </summary>
        private IEnumerator WaitToReset()
        {
            GameSessionController.Instance.ActualGameState = Enumerators.GameStates.TRANSITION;
            AudioController.Instance.PlaySFX(AudioController.Instance.BoomSound, AudioController.Instance.MaxSFXVolume);
            yield return new WaitForSecondsRealtime(AudioController.Instance.GetClipLength(AudioController.Instance.BoomSound));
            FadeEffect.Instance.FadeToLevel();
        }
    }
}