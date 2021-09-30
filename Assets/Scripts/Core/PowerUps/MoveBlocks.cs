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
                if (GameSession.Instance.CanMoveBlocks)
                {
                    GameSession.Instance.CanMoveBlocks = false;
                    GameSession.Instance.BlockDirection = Enumerators.Directions.None;
                }

                GameSession.Instance.CanMoveBlocks = true;
                GameSession.Instance.BlockDirection = direction;
                GameSession.Instance.AddToScore(UnityEngine.Random.Range(0, 1000));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}