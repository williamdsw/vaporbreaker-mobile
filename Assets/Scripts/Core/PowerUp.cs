using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class PowerUp : MonoBehaviour
    {
        // || Config

        private readonly Vector2 minMaxAngle = new Vector2(0f, 16f);
        private readonly Vector2Int minMaxRotateChance = new Vector2Int(0, 100);
        private readonly Vector2 minMaxMoveSpeed = new Vector2(10f, 30f);
        private readonly Vector2 minForceXY = new Vector2(-1000f, 0f);
        private readonly Vector2 maxForceXY = new Vector2(1000f, 1000f);

        // || State
        private float angleToIncrement = 0f;
        private int canRotateChance;

        // Speed
        private float moveSpeed = 0f;

        // State
        private int currentPowerUpIndex = 0;

        // || Cached
        private Rigidbody2D rigidBody2D;
        protected Paddle paddle;

        private void Awake() => rigidBody2D = GetComponent<Rigidbody2D>();

        private void Start()
        {
            paddle = FindObjectOfType<Paddle>();

            // Random values
            angleToIncrement = Random.Range(minMaxAngle.x, minMaxAngle.y);
            canRotateChance = Random.Range(minMaxRotateChance.x, minMaxRotateChance.y);

            AddRandomForce();
        }

        private void Update()
        {
            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (canRotateChance >= 50)
                {
                    RotateObject();
                }
            }
        }

        // Collision with paddle
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!GameSession.Instance || !AudioController.Instance) return;

            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                Paddle paddle = other.collider.GetComponent<Paddle>();
                if (paddle)
                {
                    DealCollisionWithPaddle();
                }
            }
        }

        private void AddRandomForce()
        {
            float randomX = Random.Range(minForceXY.x, maxForceXY.x);
            float randomY = Random.Range(minForceXY.y, maxForceXY.y);
            Vector2 randomForce = new Vector2(randomX, randomY);
            moveSpeed = Random.Range(minMaxMoveSpeed.x, minMaxMoveSpeed.y + 1);
            randomForce *= (Time.deltaTime * moveSpeed);
            rigidBody2D.AddForce(randomForce);
        }

        private void RotateObject()
        {
            if (angleToIncrement != 0)
            {
                Vector3 eulerAngles = transform.rotation.eulerAngles;
                eulerAngles.y += angleToIncrement;
                transform.rotation = Quaternion.Euler(eulerAngles);
            }
        }

        private void DealCollisionWithPaddle()
        {
            Destroy(gameObject);
            AudioController.Instance.PlaySoundAtPoint(AudioController.Instance.PowerUpSound, AudioController.Instance.MaxSFXVolume);
            Apply();
        }

        public void StopPowerUp()
        {
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.gravityScale = 0;
            canRotateChance = 0;
            angleToIncrement = 0;
        }

        protected abstract void Apply();

    }
}