using Controllers.Core;
using Core;
using System;
using UnityEngine;
using Utilities;

namespace Effects
{
    /// <summary>
    /// Echo Effect for the Ball
    /// </summary>
    public class EchoEffect : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Configuration")]
        [SerializeField] private GameObject echoPrefab;

        // || Config

        private readonly float startTimeBetweenSpanws = 0.05f;

        // || State

        private float timeBetweenSpawns = 0;

        // || Cached

        private Ball ball;

        // || Properties

        public float TimeToSelfDestruct { get; set; } = 1f;

        private void Awake() => FindNeededReferences();

        private void Update() => SpawnEchoEffect();

        /// <summary>
        /// Find needed references
        /// </summary>
        private void FindNeededReferences()
        {
            try
            {
                if (tag.Equals(NamesTags.Tags.BallEcho))
                {
                    ball = transform.parent.GetComponent<Ball>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Spawn echo effect
        /// </summary>
        private void SpawnEchoEffect()
        {
            try
            {
                if (GameSessionController.Instance.ActualGameState != Enumerators.GameStates.GAMEPLAY) return;

                if (timeBetweenSpawns <= 0)
                {
                    GameObject echo = Instantiate(echoPrefab, transform.position, Quaternion.identity) as GameObject;
                    echo.transform.parent = GameSessionController.Instance.FindOrCreateObjectParent(NamesTags.Parents.Echos).transform;
                    if (tag.Equals(NamesTags.Tags.BallEcho) && ball)
                    {
                        echo.transform.localScale = ball.transform.localScale;
                        echo.transform.rotation = ball.transform.rotation;
                        SpriteRenderer spriteRenderer = echo.GetComponent<SpriteRenderer>();
                        spriteRenderer.color = ball.CurrentColor;
                        spriteRenderer.sprite = ball.Sprite;
                    }

                    Destroy(echo, TimeToSelfDestruct);
                    timeBetweenSpawns = startTimeBetweenSpanws;
                }
                else
                {
                    timeBetweenSpawns -= Time.deltaTime;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}