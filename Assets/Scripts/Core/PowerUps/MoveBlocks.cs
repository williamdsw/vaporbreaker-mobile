using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class MoveBlocks : PowerUp
    {
        [Header("Additional Required Configuration")]
        [SerializeField] private string direction = string.Empty;

        protected override void Apply() => Move(direction);

        private void Move(string direction)
        {
            if (GameSession.Instance.GetCanMoveBlocks())
            {
                GameSession.Instance.SetCanMoveBlocks(false);
                GameSession.Instance.SetBlockDirection(string.Empty);
            }

            GameSession.Instance.SetCanMoveBlocks(true);
            GameSession.Instance.SetBlockDirection(direction);
            GameSession.Instance.AddToStore(Random.Range(0, 1000));
        }
    }
}