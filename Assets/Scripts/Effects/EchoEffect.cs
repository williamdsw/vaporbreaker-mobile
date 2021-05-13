using Controllers.Core;
using Core;
using UnityEngine;
using Utilities;

namespace Effects
{
    public class EchoEffect : MonoBehaviour
    {
        // Params Config
        [SerializeField] private GameObject echoPrefab;
        private float startTimeBetweenSpanws = 0.05f;
        private float timeBetweenSpawns = 0;
        private float timeToSelfDestruct = 1f;

        // Cached
        private Ball ball;
        private Paddle paddle;

        private void Start() => DefineReferences();

        private void Update() => SpawnEchoEffect();

        public void SetTimeToSelfDestruct(float time) => timeToSelfDestruct = time;

        private void DefineReferences()
        {
            if (tag.Equals(NamesTags.BallEchoTag))
            {
                ball = transform.parent.GetComponent<Ball>();
            }
        }

        private void SpawnEchoEffect()
        {
            if (GameSession.Instance.GetActualGameState() != Enumerators.GameStates.GAMEPLAY) return;

            if (timeBetweenSpawns <= 0)
            {
                GameObject echo = Instantiate(echoPrefab, transform.position, Quaternion.identity) as GameObject;
                echo.transform.parent = GameSession.Instance.FindOrCreateObjectParent(NamesTags.EchosParentName).transform;
                if (tag == NamesTags.BallEchoTag && ball)
                {
                    echo.transform.localScale = ball.transform.localScale;
                    echo.transform.rotation = ball.transform.rotation;
                    SpriteRenderer spriteRenderer = echo.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = ball.GetBallColor();
                    spriteRenderer.sprite = ball.GetSprite();
                }

                Destroy(echo, timeToSelfDestruct);
                timeBetweenSpawns = startTimeBetweenSpanws;
            }
            else
            {
                timeBetweenSpawns -= Time.deltaTime;
            }
        }
    }
}