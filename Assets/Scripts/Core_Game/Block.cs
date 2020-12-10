using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Block : MonoBehaviour
{
    // Configuration
    private float minPointsScore = 1f;
    private float maxPointsScore = 1000f;
    [SerializeField] private Sprite[] hitSprites;
    [SerializeField] private GameObject[] explosionPrefabs;
    [SerializeField] private GameObject particlesPrefab;
    [SerializeField] private PowerUp[] powerUpPrefabs;
    [SerializeField] private Color32 particlesColor;

    // State variables
    private int maxHits = 0;
    private int timesHit = 0;
    private int startMaxHits = 0;
    private bool collidedWithBall = false;
    private bool lastCollision = false;
    [SerializeField] private bool canSpawnPowerUp = false;
    public Dictionary<string, int> listPowerUpIndexes = new Dictionary<string, int>();

    // Cached Components
    private SpriteRenderer spriteRenderer;

    public int GetStartMaxHits()
    {
        return this.startMaxHits;
    }

    public void SetCanSpawnPowerUp(bool canSpawnPowerUpBlock)
    {
        this.canSpawnPowerUp = canSpawnPowerUpBlock;
    }

    public void SetMaxHits(int maxHits)
    {
        this.maxHits = maxHits;
    }

    public void SetStartMaxHits(int startMaxHits)
    {
        this.startMaxHits = startMaxHits;
    }

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        CountBreakableBlocks();
        maxHits = (hitSprites.Length + 1);
        startMaxHits = maxHits;

        if (powerUpPrefabs.Length != 0)
        {
            // Fill list
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
        if (!GameSession.Instance) return;

        if (GameSession.Instance.GetActualGameState() == Enumerators.GameStates.GAMEPLAY)
        {
            if (!lastCollision)
            {
                collidedWithBall = (other.gameObject.GetComponent<Ball>() != null);

                if (CompareTag(NamesTags.GetBreakableBlockTag()))
                {
                    HandleHit();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameSession.Instance) return;

        if (GameSession.Instance.GetActualGameState() == Enumerators.GameStates.GAMEPLAY)
        {
            if (!lastCollision)
            {
                // Verifies the ball
                collidedWithBall = (other.gameObject.GetComponent<Ball>() != null);

                if (CompareTag(NamesTags.GetBreakableBlockTag()))
                {
                    HandleHit();
                }
            }
        }
    }

    private void CountBreakableBlocks()
    {
        if (CompareTag(NamesTags.GetBreakableBlockTag()))
        {
            GameSession.Instance.CountBlocks();
        }
    }

    private void HandleHit()
    {
        timesHit++;

        if (timesHit >= maxHits)
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
        // Check and cancels
        if (!AudioController.Instance) return;

        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            spriteRenderer.sprite = hitSprites[spriteIndex];
        }

        // SFX & VFX
        AudioController.Instance.PlaySFX(AudioController.Instance.SlamSound, AudioController.Instance.GetMaxSFXVolume() / 2f);
        SpawnDebris();
    }

    // Do a lot of SFX and VFX stuff, update GameSession and destroys itself
    private void DestroyBlock()
    {
        // Add to combo only it's the ball
        if (collidedWithBall)
        {
            GameSession.Instance.AddToComboMultiplier();
        }

        // Calculates score
        int comboMultiplier = GameSession.Instance.GetComboMultiplier();
        int score = (int) Random.Range(minPointsScore, maxPointsScore);
        score *= maxHits;

        // Multiply score only it's the ball
        if (collidedWithBall)
        {
            if (comboMultiplier > 1)
            {
                score *= comboMultiplier;
            }
        }

        // VFX
        TriggerExplosion();
        SpawnDebris();
        GameSession.Instance.AddToStore(score);

        // Spawn powerups
        if (canSpawnPowerUp)
        {
            SpawnPowerUp();
        }

        StartCoroutine(DestroyCoroutine());
    }

    // Destroys object and update Game Session
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
        GameSession.Instance.BlockDestroyed();

        if (BlockGrid.CheckPosition(this.transform.position))
        {
            BlockGrid.RedefineBlock(this.transform.position, null);
        }
    }

    // Shows a random animated explosion
    private void TriggerExplosion()
    {
        // Check and cancels
        if (!AudioController.Instance || !GameSession.Instance) return;

        if (explosionPrefabs.Length >= 1)
        {
            AudioController.Instance.PlaySoundAtPoint(AudioController.Instance.ExplosionSound, AudioController.Instance.GetMaxSFXVolume() / 2f);
            int randomIndex = Random.Range(0, explosionPrefabs.Length);
            GameObject explosion = Instantiate(explosionPrefabs[randomIndex], this.transform.position, Quaternion.identity) as GameObject;
            explosion.transform.SetParent(GameSession.Instance.FindOrCreateObjectParent(NamesTags.GetExplosionsParentName()).transform);
            Animator animator = explosion.GetComponent<Animator>();
            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(explosion, animationLength);
        }
    }

    // Shows random number of debris
    private void SpawnDebris()
    {
        // Check and cancels
        if (!GameSession.Instance) return;

        if (particlesPrefab)
        {
            // Instantiate and Destroy
            GameObject debris = Instantiate(particlesPrefab, this.transform.position, particlesPrefab.transform.rotation) as GameObject;
            debris.transform.SetParent(GameSession.Instance.FindOrCreateObjectParent(NamesTags.GetDebrisParentName()).transform);

            // Color
            ParticleSystem debrisParticleSystem = debris.GetComponent<ParticleSystem>();
            var mainModule = debrisParticleSystem.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(particlesColor);

            // Time to destroy
            ParticleSystem prefabParticleSystem = particlesPrefab.GetComponent<ParticleSystem>();
            float durationLength = prefabParticleSystem.main.duration + prefabParticleSystem.main.startLifetime.constant;
            Destroy(debris, durationLength);
        }
    }

    // Instantiate a Power Up
    private void SpawnPowerUp()
    {
        // Check and cancels
        if (!AudioController.Instance || !GameSession.Instance) return;

        int randomIndex = CalculateIndexChance();
        GameObject powerUp = Instantiate(powerUpPrefabs[randomIndex].gameObject, this.transform.position, Quaternion.identity) as GameObject;
        powerUp.transform.SetParent(GameSession.Instance.FindOrCreateObjectParent(NamesTags.GetPowerUpsParentName()).transform);
        AudioController.Instance.PlaySoundAtPoint(AudioController.Instance.ShowUpSound, AudioController.Instance.GetMaxSFXVolume());
    }

    // Calculate chance percent and possibly decide when have other options... 
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