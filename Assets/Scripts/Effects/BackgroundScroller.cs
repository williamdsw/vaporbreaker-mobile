using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Effects
{
    public class BackgroundScroller : MonoBehaviour
    {
        // Config
        [SerializeField] private bool randomMaterial;
        [SerializeField] private bool randomMovementSpeeds;
        [SerializeField] private float xMovementSpeed = 0.1f;
        [SerializeField] private float yMovementSpeed = 0.1f;
        [SerializeField] private Material[] listOfMaterials;

        // Const
        private const float TEXTURE_OFFSET_VALUE = 0.2f;

        // State
        private bool canOffsetTexture = false;

        // Cached
        private Material material;
        private Renderer myRenderer;
        private Vector2 offset;

        private void Awake() => myRenderer = GetComponent<Renderer>();

        private void Start()
        {
            // Chooses random material
            if (randomMaterial)
            {
                if (listOfMaterials.Length == 0) return;
                int index = Random.Range(0, listOfMaterials.Length);
                myRenderer.material = listOfMaterials[index];
            }

            // Chooses movement speed
            if (randomMovementSpeeds)
            {
                xMovementSpeed = Random.Range(-TEXTURE_OFFSET_VALUE, TEXTURE_OFFSET_VALUE);
                yMovementSpeed = Random.Range(-TEXTURE_OFFSET_VALUE, TEXTURE_OFFSET_VALUE);
            }

            material = myRenderer.material;
            offset = new Vector2(xMovementSpeed, yMovementSpeed);
            canOffsetTexture = (material.name.Contains("Grid"));
        }

        private void FixedUpdate()
        {
            if (GameSession.Instance.GetActualGameState() != Enumerators.GameStates.GAMEPLAY) return;
            if (canOffsetTexture)
            {
                material.mainTextureOffset += (offset * Time.fixedDeltaTime);
            }
        }
    }
}