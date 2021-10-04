using Controllers.Core;
using System;
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
        private readonly float maxForceXY = 1000f;

        // || State

        private float angleToIncrement = 0f;
        private int canRotateChance;
        private float moveSpeed = 0f;

        // || Cached

        private Rigidbody2D rigidBody2D;
        protected Paddle paddle;

        private void Awake()
        {
            GetRequiredComponents();

            angleToIncrement = UnityEngine.Random.Range(minMaxAngle.x, minMaxAngle.y);
            canRotateChance = UnityEngine.Random.Range(minMaxRotateChance.x, minMaxRotateChance.y);

            AddRandomForce();
        }

        private void Start() => paddle = FindObjectOfType<Paddle>();

        private void Update()
        {
            if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (canRotateChance >= 50)
                {
                    RotateObject();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                paddle = other.collider.GetComponent<Paddle>();
                if (paddle)
                {
                    DealCollisionWithPaddle();
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
                rigidBody2D = GetComponent<Rigidbody2D>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add random force to this object
        /// </summary>
        private void AddRandomForce()
        {
            float randomX = UnityEngine.Random.Range(minForceXY.x, maxForceXY);
            float randomY = UnityEngine.Random.Range(minForceXY.y, maxForceXY);
            Vector2 randomForce = new Vector2(randomX, randomY);
            moveSpeed = UnityEngine.Random.Range(minMaxMoveSpeed.x, minMaxMoveSpeed.y + 1);
            randomForce *= (Time.fixedDeltaTime * moveSpeed);
            rigidBody2D.AddForce(randomForce);
        }

        /// <summary>
        /// Rotate this object
        /// </summary>
        private void RotateObject()
        {
            if (angleToIncrement != 0)
            {
                Vector3 eulerAngles = transform.rotation.eulerAngles;
                eulerAngles.y += angleToIncrement;
                transform.rotation = Quaternion.Euler(eulerAngles);
            }
        }

        /// <summary>
        /// Deals collisiton with paddle
        /// </summary>
        private void DealCollisionWithPaddle()
        {
            Destroy(gameObject);
            AudioController.Instance.PlaySFX(AudioController.Instance.PowerUpSound, AudioController.Instance.MaxSFXVolume);
            Apply();
        }

        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected abstract void Apply();
    }
}