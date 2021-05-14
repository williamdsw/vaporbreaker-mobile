using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class Shooter : PowerUp
    {
        [Header("Required Configuration")]
        [SerializeField] private Core.Shooter shooterPrefab;

        protected override void Apply()
        {
            // Finds and cancel case have one already
            Core.Shooter shooter = FindObjectOfType<Core.Shooter>();
            if (shooter) return;

            shooter = Instantiate(shooterPrefab, paddle.transform.position, Quaternion.identity) as Core.Shooter;
            shooter.transform.parent = paddle.transform;
            GameSession.Instance.AddToStore(Random.Range(100, 500));
        }
    }
}