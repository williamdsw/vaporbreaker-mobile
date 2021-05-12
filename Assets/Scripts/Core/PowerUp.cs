﻿using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Core
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PowerUp : MonoBehaviour
    {
        // Config
        [SerializeField] private Shooter shooterPrefab;

        // Rotation
        private float angleToIncrement = 0f;
        private int canRotateChance;

        // Speed
        private float moveSpeed = 0f;
        private float minMoveSpeed = 10f;
        private float maxMoveSpeed = 30f;

        //Force 
        private float minForceX = -1000f;
        private float maxForceX = 1000f;
        private float minForceY = 0f;
        private float maxForceY = 1000f;

        // State
        private int currentPowerUpIndex = 0;
        private string[] powerUpNames =
        {
        Enumerators.PowerUpsNames.PowerUp_All_Blocks_1_Hit.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Ball_Bigger.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Ball_Faster.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Ball_Slower.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Ball_Smaller.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Duplicate_Ball.ToString (),
        Enumerators.PowerUpsNames.PowerUp_FireBall.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Move_Blocks_Right.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Move_Blocks_Left.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Move_Blocks_Up.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Move_Blocks_Down.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Paddle_Expand.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Paddle_Shrink.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Reset_Ball.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Reset_Paddle.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Shooter.ToString (),
        Enumerators.PowerUpsNames.PowerUp_Unbreakables_To_Breakables.ToString (),
    };

        // Cached Components
        private Rigidbody2D rigidBody2D;

        // Cached Others
        private Paddle paddle;

        private void Awake()
        {
            rigidBody2D = this.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            paddle = FindObjectOfType<Paddle>();

            // Random values
            angleToIncrement = Random.Range(0f, 16f);
            canRotateChance = Random.Range(0, 100);

            AddRandomForce();
        }

        private void Update()
        {
            // Cancels
            if (!GameSession.Instance) return;

            // Rotates... or not
            if (GameSession.Instance.GetActualGameState() == Enumerators.GameStates.GAMEPLAY)
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

            if (GameSession.Instance.GetActualGameState() == Enumerators.GameStates.GAMEPLAY)
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
            float randomX = Random.Range(minForceX, maxForceX);
            float randomY = Random.Range(minForceY, maxForceY);
            Vector2 randomForce = new Vector2(randomX, randomY);
            moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed + 1);
            randomForce *= Time.deltaTime * moveSpeed;
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
            Destroy(this.gameObject);
            AudioController.Instance.PlaySoundAtPoint(AudioController.Instance.PowerUpSound, AudioController.Instance.MaxSFXVolume);
            string name = this.gameObject.name;
            name = name.Replace("(Clone)", "");
            ApplyPowerUpEffect(name);
            AddScoreToGameSession(name);
        }

        public void StopPowerUp()
        {
            if (!rigidBody2D) return;
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.gravityScale = 0;
            canRotateChance = 0;
            angleToIncrement = 0;
        }

        // Applies power up effect to gameplay
        private void ApplyPowerUpEffect(string powerUpName)
        {
            switch (powerUpName)
            {
                case "PowerUp_All_Blocks_1_Hit":
                    {
                        MakeAllBlocksOneHit();
                        currentPowerUpIndex = 0;
                        break;
                    }

                case "PowerUp_Ball_Bigger":
                    {
                        DefineBallsSize(true);
                        currentPowerUpIndex = 1;
                        break;
                    }

                case "PowerUp_Ball_Smaller":
                    {
                        DefineBallsSize(false);
                        currentPowerUpIndex = 2;
                        break;
                    }

                case "PowerUp_Ball_Faster":
                    {
                        DefineBallsSpeed(true);
                        currentPowerUpIndex = 3;
                        break;
                    }

                case "PowerUp_Ball_Slower":
                    {
                        DefineBallsSpeed(false);
                        currentPowerUpIndex = 4;
                        break;
                    }

                case "PowerUp_Duplicate_Ball":
                    {
                        DuplicateBalls();
                        currentPowerUpIndex = 5;
                        break;
                    }

                case "PowerUp_FireBall":
                    {
                        DefineFireBalls();
                        currentPowerUpIndex = 6;
                        break;
                    }

                case "PowerUp_Move_Blocks_Right":
                    {
                        DefineMoveBlocks("Right");
                        currentPowerUpIndex = 7;
                        break;
                    }

                case "PowerUp_Move_Blocks_Left":
                    {
                        DefineMoveBlocks("Left");
                        currentPowerUpIndex = 8;
                        break;
                    }

                case "PowerUp_Move_Blocks_Up":
                    {
                        DefineMoveBlocks("Up");
                        currentPowerUpIndex = 9;
                        break;
                    }

                case "PowerUp_Move_Blocks_Down":
                    {
                        DefineMoveBlocks("Down");
                        currentPowerUpIndex = 10;
                        break;
                    }

                case "PowerUp_Paddle_Expand":
                    {
                        if (paddle)
                        {
                            paddle.DefinePaddleSize(true);
                            currentPowerUpIndex = 11;
                        }
                        break;
                    }

                case "PowerUp_Paddle_Shrink":
                    {
                        if (paddle)
                        {
                            paddle.DefinePaddleSize(false);
                            currentPowerUpIndex = 12;
                        }
                        break;
                    }

                case "PowerUp_Reset_Ball":
                    {
                        ResetBalls();
                        currentPowerUpIndex = 13;
                        break;
                    }

                case "PowerUp_Reset_Paddle":
                    {
                        if (paddle)
                        {
                            paddle.ResetPaddle();
                            currentPowerUpIndex = 14;
                        }
                        break;
                    }

                case "PowerUp_Shooter":
                    {
                        CreateShooter();
                        currentPowerUpIndex = 15;
                        break;
                    }

                case "PowerUp_Unbreakables_To_Breakables":
                    {
                        MakeUnbreakableToBreakable();
                        currentPowerUpIndex = 16;
                        break;
                    }

                default: break;
            }
        }

        // Applies random score...
        private void AddScoreToGameSession(string powerUpName)
        {
            switch (powerUpName)
            {
                case "PowerUp_All_Blocks_1_Hit": { GameSession.Instance.AddToStore(Random.Range(0, 1000)); break; }
                case "PowerUp_Ball_Bigger": { GameSession.Instance.AddToStore(Random.Range(0, 1000)); break; }
                case "PowerUp_Ball_Smaller": { GameSession.Instance.AddToStore(Random.Range(1000, 5000)); break; }
                case "PowerUp_Ball_Faster": { GameSession.Instance.AddToStore(Random.Range(5000, 10000)); break; }
                case "PowerUp_Ball_Slower": { GameSession.Instance.AddToStore(Random.Range(100, 1000)); break; }
                case "PowerUp_Duplicate_Ball": { GameSession.Instance.AddToStore(Random.Range(500, 2500)); break; }
                case "PowerUp_FireBall": { GameSession.Instance.AddToStore(Random.Range(-10000, 10000)); break; }
                case "PowerUp_Move_Blocks_Right": { GameSession.Instance.AddToStore(Random.Range(0, 1000)); break; }
                case "PowerUp_Move_Blocks_Left": { GameSession.Instance.AddToStore(Random.Range(0, 1000)); break; }
                case "PowerUp_Move_Blocks_Up": { GameSession.Instance.AddToStore(Random.Range(0, 1000)); break; }
                case "PowerUp_Move_Blocks_Down": { GameSession.Instance.AddToStore(Random.Range(0, 1000)); break; }
                case "PowerUp_Paddle_Expand": { GameSession.Instance.AddToStore(Random.Range(100, 500)); break; }
                case "PowerUp_Paddle_Shrink": { GameSession.Instance.AddToStore(Random.Range(10000, 30000)); break; }
                case "PowerUp_Reset_Ball": { GameSession.Instance.AddToStore(Random.Range(100, 1000)); break; }
                case "PowerUp_Reset_Paddle": { GameSession.Instance.AddToStore(Random.Range(100, 1000)); break; }
                case "PowerUp_Shooter": { GameSession.Instance.AddToStore(Random.Range(100, 500)); break; }
                case "PowerUp_Unbreakables_To_Breakables": { GameSession.Instance.AddToStore(Random.Range(100, 500)); break; }
                default: break;
            }
        }

        private void MakeAllBlocksOneHit()
        {
            Block[] blocks = FindObjectsOfType<Block>();

            if (blocks.Length != 0)
            {
                foreach (Block block in blocks)
                {
                    block.SetMaxHits(1);
                    block.SetStartMaxHits(1);
                }
            }
        }

        private void DefineBallsSize(bool makeBigger)
        {
            Ball[] balls = FindObjectsOfType<Ball>();
            if (balls.Length != 0)
            {
                foreach (Ball ball in balls)
                {
                    Vector3 newLocalScale = ball.transform.localScale;
                    if (makeBigger)
                    {
                        if (newLocalScale.x < ball.GetMaxBallLocalScale())
                        {
                            newLocalScale *= 2f;
                        }
                    }
                    else
                    {
                        if (newLocalScale.x > ball.GetMinBallLocalScale())
                        {
                            newLocalScale /= 2f;
                        }
                    }

                    ball.transform.localScale = newLocalScale;
                }
            }
        }

        private void DefineBallsSpeed(bool moveFaster)
        {
            Ball[] balls = FindObjectsOfType<Ball>();
            if (balls.Length != 0)
            {
                foreach (Ball ball in balls)
                {
                    Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
                    float moveSpeed = ball.GetMoveSpeed();
                    float ballRotationDegree = ball.GetBallRotationDegree();
                    if (moveFaster)
                    {
                        if (moveSpeed < ball.GetMaxMoveSpeed())
                        {
                            moveSpeed += 100f;
                        }

                        if (ballRotationDegree < ball.GetMaxBallRotationDegree())
                        {
                            ballRotationDegree *= 2;
                        }
                    }
                    else
                    {
                        if (moveSpeed > ball.GetMinMoveSpeed())
                        {
                            moveSpeed -= 100f;
                        }

                        if (ballRotationDegree > ball.GetMinBallRotationDegree())
                        {
                            ballRotationDegree /= 2;
                        }
                    }

                    ball.SetMoveSpeed(moveSpeed);
                    ball.SetBallRotationDegree(ballRotationDegree);
                    ballRB.velocity = (ballRB.velocity.normalized * Time.deltaTime * moveSpeed);
                }
            }
        }

        private void ResetBalls()
        {
            Ball[] balls = FindObjectsOfType<Ball>();
            if (balls.Length != 0)
            {
                foreach (Ball ball in balls)
                {
                    // Local Scale
                    ball.transform.localScale = Vector3.one;

                    // Movement
                    Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
                    float defaultSpeed = ball.GetDefaultSpeed();
                    ball.SetMoveSpeed(defaultSpeed);
                    ballRB.velocity = (ballRB.velocity.normalized * Time.deltaTime * defaultSpeed);
                }
            }
        }


        // Duplicates que quantity of current balls in the reversed direction of each ball
        private void DuplicateBalls()
        {
            // Checks and Cancel
            if (!GameSession.Instance) return;
            if (GameSession.Instance.CurrentNumberOfBalls >= GameSession.Instance.GetMaxNumberOfBalls()) return;

            Ball[] balls = FindObjectsOfType<Ball>();

            if (balls.Length != 0)
            {
                foreach (Ball ball in balls)
                {
                    Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
                    Ball newBall = Instantiate(ball, ball.transform.position, Quaternion.identity) as Ball;
                    Rigidbody2D newBallRB = newBall.GetComponent<Rigidbody2D>();
                    newBallRB.velocity = (ballRB.velocity.normalized * -1 * Time.deltaTime * ball.GetMoveSpeed());
                    newBall.SetMoveSpeed(ball.GetMoveSpeed());
                    if (ball.GetIsBallOnFire())
                    {
                        newBall.SetIsBallOnFire(true);
                        newBall.ChangeBallSprite(true);
                    }
                }
            }
        }

        public void DefineFireBalls()
        {
            // Cancels
            if (!GameSession.Instance) return;
            GameSession.Instance.MakeFireBalls();
        }

        private void DefineMoveBlocks(string direction)
        {
            if (!GameSession.Instance) return;

            // Check if already is moving
            if (GameSession.Instance.GetCanMoveBlocks())
            {
                GameSession.Instance.SetCanMoveBlocks(false);
                GameSession.Instance.SetBlockDirection(string.Empty);
            }

            GameSession.Instance.SetCanMoveBlocks(true);
            GameSession.Instance.SetBlockDirection(direction);
        }

        // Verify and creates the shooter
        private void CreateShooter()
        {
            // Finds and cancel case have one already
            Shooter shooter = FindObjectOfType<Shooter>();
            if (shooter) return;

            shooter = Instantiate(shooterPrefab, paddle.transform.position, Quaternion.identity) as Shooter;
            shooter.transform.parent = paddle.transform;
        }

        private void MakeUnbreakableToBreakable()
        {
            if (GameObject.FindGameObjectsWithTag(NamesTags.UnbreakableBlockTag).Length != 0)
            {
                GameObject[] unbreakables = GameObject.FindGameObjectsWithTag(NamesTags.UnbreakableBlockTag);
                foreach (GameObject unbreakable in unbreakables)
                {
                    unbreakable.tag = NamesTags.BreakableBlockTag;
                    GameSession.Instance.CountBlocks();
                    unbreakable.GetComponent<Animator>().enabled = false;

                    for (int i = 0; i < unbreakable.transform.childCount; i++)
                    {
                        GameObject child = unbreakable.transform.GetChild(i).gameObject;
                        Destroy(child);
                    }
                }
            }
        }
    }
}