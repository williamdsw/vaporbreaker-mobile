using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    private Ball[] balls;
    private FadeEffect fadeEffect;

    private void Awake()
    {
        edgeCollider = this.GetComponent<EdgeCollider2D>();
    }

    private void Start()
    {
        fadeEffect = FindObjectOfType<FadeEffect>();
        DefineColliderPoints();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check and cancels
        if (!GameSession.Instance || !AudioController.Instance) return;

        if (GameSession.Instance.GetActualGameState() == Enumerators.GameStates.GAMEPLAY)
        {
            if (other.gameObject.CompareTag(NamesTags.GetBallTag()))
            {
                DealWithBallCollision(other.gameObject);
            }
            else if (other.gameObject.CompareTag(NamesTags.GetPowerUpTag()))
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
        GameSession.Instance.SetActualGameState(Enumerators.GameStates.TRANSITION);
        float soundLength = AudioController.Instance.GetClipLength(AudioController.Instance.BoomSound);
        AudioController.Instance.PlaySFX(AudioController.Instance.BoomSound, 0.8f);
        yield return new WaitForSecondsRealtime(soundLength);
        fadeEffect.FadeToLevel();
    }
}