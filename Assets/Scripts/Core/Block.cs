using Controllers.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Block : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Configuration Parameters")]
        [SerializeField] private Sprite[] hitSprites;
        [SerializeField] private GameObject[] explosionPrefabs;
        [SerializeField] private GameObject particlesPrefab;
        [SerializeField] private PowerUp[] powerUpPrefabs;
        [SerializeField] private Color32 particlesColor;

        // || Const

        private readonly Vector2 minMaxPointsScore = new Vector2(1f, 1000f);

        // State variables
        private int timesHit = 0;
        private bool collidedWithBall = false;
        private bool lastCollision = false;
        [SerializeField] private bool canSpawnPowerUp = false;
        public Dictionary<string, int> listPowerUpIndexes = new Dictionary<string, int>();

        // Cached Components
        private SpriteRenderer spriteRenderer;

        public int MaxHits { private get; set; } = 0;
        public int StartMaxHits { get; set; } = 0;
        public bool CanSpawnPowerUp { private get; set; } = false;

        private void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

        private void Start()
        {
            CountBreakableBlocks();
            MaxHits = (hitSprites.Length + 1);
            StartMaxHits = MaxHits;

            if (powerUpPrefabs.Length != 0)
            {
                int index = 0;
                foreach (PowerUp powerUp in powerUpPrefabs)
                {
                    listPowerUpIndexes.Add(powerUp.name, index);
                    index++;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (!lastCollision)
                {
                    collidedWithBall = (other.gameObject.GetComponent<Ball>() != null);

                    if (CompareTag(NamesTags.BreakableBlockTag))
                    {
                        HandleHit();
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (!lastCollision)
                {
                    collidedWithBall = (other.gameObject.GetComponent<Ball>() != null);

                    if (CompareTag(NamesTags.BreakableBlockTag))
                    {
                        HandleHit();
                    }
                }
            }
        }

        private void CountBreakableBlocks()
        {
            if (CompareTag(NamesTags.BreakableBlockTag))
            {
                GameSession.Instance.CountBlocks();
            }
        }

        private void HandleHit()
        {
            timesHit++;

            if (timesHit >= MaxHits)
            {
                DestroyBlock();
                lastCollision = true;
            }
            else
            {
                ShowNextSprite();
            }
        }

        private void ShowNextSprite()
        {
            int spriteIndex = timesHit - 1;
            if (hitSprites[spriteIndex] != null)
            {
                spriteRenderer.sprite = hitSprites[spriteIndex];
            }

            AudioController.Instance.PlaySFX(AudioController.Instance.SlamSound, AudioController.Instance.MaxSFXVolume / 2f);
            SpawnDebris();
        }

        private void DestroyBlock()
        {
            if (collidedWithBall)
            {
                GameSession.Instance.AddToComboMultiplier();
            }

            int comboMultiplier = GameSession.Instance.ComboMultiplier;
            int score = (int)Random.Range(minMaxPointsScore.x, minMaxPointsScore.y);
            score *= MaxHits;

            if (collidedWithBall)
            {
                if (comboMultiplier > 1)
                {
                    score *= comboMultiplier;
                }
            }

            TriggerExplosion();
            SpawnDebris();
            GameSession.Instance.AddToStore(score);

            if (CanSpawnPowerUp)
            {
                SpawnPowerUp();
            }

            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
            GameSession.Instance.BlockDestroyed();

            if (BlockGrid.CheckPosition(transform.position))
            {
                BlockGrid.RedefineBlock(transform.position, null);
            }
        }

        private void TriggerExplosion()
        {
            if (explosionPrefabs.Length >= 1)
            {
                AudioController.Instance.PlaySoundAtPoint(AudioController.Instance.ExplosionSound, AudioController.Instance.MaxSFXVolume / 2f);
                int randomIndex = Random.Range(0, explosionPrefabs.Length);
                GameObject explosion = Instantiate(explosionPrefabs[randomIndex], transform.position, Quaternion.identity) as GameObject;
                explosion.transform.SetParent(GameSession.Instance.FindOrCreateObjectParent(NamesTags.ExplosionsParentName).transform);
                Animator animator = explosion.GetComponent<Animator>();
                float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
                Destroy(explosion, animationLength);
            }
        }

        private void SpawnDebris()
        {
            if (particlesPrefab)
            {
                // Instantiate and Destroy
                GameObject debris = Instantiate(particlesPrefab, transform.position, particlesPrefab.transform.rotation) as GameObject;
                debris.transform.SetParent(GameSession.Instance.FindOrCreateObjectParent(NamesTags.DebrisParentName).transform);

                // Color
                ParticleSystem debrisParticleSystem = debris.GetComponent<ParticleSystem>();
                var mainModule = debrisParticleSystem.main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(particlesColor);

                // Time to destroy
                ParticleSystem prefabParticleSystem = particlesPrefab.GetComponent<ParticleSystem>();
                float durationLength = (prefabParticleSystem.main.duration + prefabParticleSystem.main.startLifetime.constant);
                Destroy(debris, durationLength);
            }
        }

        private void SpawnPowerUp()
        {
            int randomIndex = CalculateIndexChance();
            GameObject powerUp = Instantiate(powerUpPrefabs[randomIndex].gameObject, this.transform.position, Quaternion.identity) as GameObject;
            powerUp.transform.SetParent(GameSession.Instance.FindOrCreateObjectParent(NamesTags.PowerUpsParentName).transform);
            AudioController.Instance.PlaySoundAtPoint(AudioController.Instance.ShowUpSound, AudioController.Instance.MaxSFXVolume);
        }

        private int CalculateIndexChance()
        {
            int index = 0;
            int chance = Random.Range(0, 100);
            if (chance >= 80)
            {
                string[] powerUps =
                {
                    Enumerators.PowerUpsNames.PowerUp_FireBall.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Shooter.ToString ()
                };

                index = listPowerUpIndexes[powerUps[Random.Range(0, powerUps.Length)]];
            }
            else if (chance >= 60 && chance < 80)
            {
                string[] powerUps =
                {
                    Enumerators.PowerUpsNames.PowerUp_All_Blocks_1_Hit.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Move_Blocks_Right.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Move_Blocks_Left.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Move_Blocks_Up.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Move_Blocks_Down.ToString (),
                };

                index = listPowerUpIndexes[powerUps[Random.Range(0, powerUps.Length)]];
            }
            else if (chance >= 50 && chance < 60)
            {
                index = listPowerUpIndexes[Enumerators.PowerUpsNames.PowerUp_Unbreakables_To_Breakables.ToString()];
            }
            else if (chance >= 0 && chance < 50)
            {
                string[] powerUps =
                {
                    Enumerators.PowerUpsNames.PowerUp_Ball_Bigger.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Ball_Faster.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Ball_Slower.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Ball_Smaller.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Duplicate_Ball.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Paddle_Expand.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Paddle_Shrink.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Reset_Ball.ToString (),
                    Enumerators.PowerUpsNames.PowerUp_Reset_Paddle.ToString ()
                };
                index = listPowerUpIndexes[powerUps[Random.Range(0, powerUps.Length)]];
            }

            return index;
        }
    }
}