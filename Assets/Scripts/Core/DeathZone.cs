using Controllers.Core;
using Effects;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Core
{
    public class DeathZone : MonoBehaviour
    {
        // || Cached References
        private EdgeCollider2D edgeCollider;
        private Ball[] balls;

        private void Awake() => edgeCollider = GetComponent<EdgeCollider2D>();

        private void Start() => DefineColliderPoints();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
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


        // Defines the collider points based on size of screen
        private void DefineColliderPoints()
        {
            Camera mainCamera = Camera.main;
            Vector2 lowerLeftCorner = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            Vector2 lowerRightCorner = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            lowerLeftCorner.x = Mathf.FloorToInt(lowerLeftCorner.x) * 2;
            lowerRightCorner.x = Mathf.CeilToInt(lowerRightCorner.x) * 2;
            edgeCollider.points = new Vector2[] { lowerLeftCorner, lowerRightCorner };
        }

        // Deals with collision with ball depending on how much balls are on screen
        private void DealWithBallCollision(GameObject otherBall)
        {
            balls = FindObjectsOfType<Ball>();

            if (balls.Length == 1)
            {
                StartCoroutine(WaitToReset());
            }
            else
            {
                AudioController.Instance.PlaySFX(AudioController.Instance.BoomSound, 0.3f);
                GameSession.Instance.CurrentNumberOfBalls--;
            }

            Destroy(otherBall);
        }

        // Plays SFX and wait to call fade out
        private IEnumerator WaitToReset()
        {
            GameSession.Instance.ActualGameState = Enumerators.GameStates.TRANSITION;
            float soundLength = AudioController.Instance.GetClipLength(AudioController.Instance.BoomSound);
            AudioController.Instance.PlaySFX(AudioController.Instance.BoomSound, 0.8f);
            yield return new WaitForSecondsRealtime(soundLength);
            FadeEffect.Instance.FadeToLevel();
        }
    }
}