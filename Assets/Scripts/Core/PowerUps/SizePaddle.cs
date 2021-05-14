using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class SizePaddle : PowerUp
    {
        [Header("Additional Required Configuration")]
        [SerializeField] private bool toExpand = false;

        protected override void Apply() => Resize(toExpand);

        private void Resize(bool toExpand)
        {
            if (paddle != null)
            {
                paddle.DefinePaddleSize(toExpand);
                Vector2Int minMaxScore = new Vector2Int(toExpand ? 100 : 10000, toExpand ? 500 : 30000);
                GameSession.Instance.AddToStore(Random.Range(minMaxScore.x, minMaxScore.y));
            }
        }
    }
}