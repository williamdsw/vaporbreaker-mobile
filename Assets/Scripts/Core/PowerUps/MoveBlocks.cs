using Controllers.Core;
using System;
using UnityEngine;
using Utilities;

namespace Core.PowerUps
{
    public class MoveBlocks : PowerUp
    {
        [Header("Additional Required Configuration")]
        [SerializeField] private Enumerators.Directions direction = Enumerators.Directions.None;

        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected override void Apply() => Move(direction);

        /// <summary>
        /// Move block to desired direction
        /// </summary>
        /// <param name="direction"> Desired direction </param>
        private void Move(Enumerators.Directions direction)
        {
            try
            {
                if (GameSessionController.Instance.CanMoveBlocks)
                {
                    GameSessionController.Instance.CanMoveBlocks = false;
                    GameSessionController.Instance.BlockDirection = Enumerators.Directions.None;
                }

                GameSessionController.Instance.CanMoveBlocks = true;
                GameSessionController.Instance.BlockDirection = direction;
                GameSessionController.Instance.AddToScore(UnityEngine.Random.Range(0, 1000));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}