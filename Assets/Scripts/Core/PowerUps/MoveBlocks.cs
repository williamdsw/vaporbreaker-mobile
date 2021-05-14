using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Core.PowerUps
{
    public class MoveBlocks : PowerUp
    {
        [Header("Additional Required Configuration")]
        [SerializeField] private Enumerators.Directions direction = Enumerators.Directions.None;

        protected override void Apply() => Move(direction);

        private void Move(Enumerators.Directions direction)
        {
            if (GameSession.Instance.CanMoveBlocks)
            {
                GameSession.Instance.CanMoveBlocks = false;
                GameSession.Instance.BlockDirection = Enumerators.Directions.None;
            }

            GameSession.Instance.CanMoveBlocks = true;
            GameSession.Instance.BlockDirection = direction;
            GameSession.Instance.AddToStore(Random.Range(0, 1000));
        }
    }
}