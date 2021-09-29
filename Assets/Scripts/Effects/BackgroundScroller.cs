using System;
using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Effects
{
    public class BackgroundScroller : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Configuration")]
        [SerializeField] private bool randomMaterial;
        [SerializeField] private bool randomMovementSpeeds;
        [SerializeField] private float xMovementSpeed = 0.1f;
        [SerializeField] private float yMovementSpeed = 0.1f;
        [SerializeField] private Material[] listOfMaterials;

        // || CONFIG

        private const float TEXTURE_OFFSET_VALUE = 0.2f;

        // || Cached

        private Material material;
        private Renderer myRenderer;
        private Vector2 offset;

        private void Awake()
        {
            GetRequiredComponents();
        }

        private void Start()
        {
            // Chooses random material
            if (randomMaterial)
            {
                if (listOfMaterials.Length == 0) return;
                int index = UnityEngine.Random.Range(0, listOfMaterials.Length);
                myRenderer.material = listOfMaterials[index];
            }

            // Chooses movement speed
            if (randomMovementSpeeds)
            {
                xMovementSpeed = UnityEngine.Random.Range(-TEXTURE_OFFSET_VALUE, TEXTURE_OFFSET_VALUE);
                yMovementSpeed = UnityEngine.Random.Range(-TEXTURE_OFFSET_VALUE, TEXTURE_OFFSET_VALUE);
            }

            material = myRenderer.material;
            offset = new Vector2(xMovementSpeed, yMovementSpeed);
        }

        private void FixedUpdate() => material.mainTextureOffset += (offset * Time.fixedDeltaTime);

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                myRenderer = GetComponent<Renderer>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}