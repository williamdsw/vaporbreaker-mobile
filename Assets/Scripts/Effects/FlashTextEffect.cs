using System.Collections;
using UnityEngine;
using TMPro;

public class FlashTextEffect : MonoBehaviour
{
    // Config params
    [SerializeField] private float timeToFlick = 0.01f;
    [SerializeField] private bool isLooping = true;

    // Cached
    private Color color;
    private TextMeshPro textMeshPro;
    private TextMeshProUGUI textMeshProUGUI;

    //--------------------------------------------------------------------------------//
    // GETTERS

    public void SetTimeToFlick (float timeToFlick) { this.timeToFlick = timeToFlick; }

    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOURS

    private void Awake () 
    {
        // Parent or children
        textMeshPro = this.GetComponent<TextMeshPro>();
        if (!textMeshPro) { textMeshPro = this.GetComponentInChildren<TextMeshPro>(); }

        // For UGUI
        if (!textMeshPro) { textMeshProUGUI = this.GetComponent<TextMeshProUGUI>(); }
    }

    private void Start ()
    {
        StartCoroutine (Flash ());
    }

    //--------------------------------------------------------------------------------//
    // COROUTINES

    // Flashes the alpha of text color
    private IEnumerator Flash ()
    {
        while (isLooping)
        {
            // Cancels
            if (textMeshPro && string.IsNullOrEmpty (textMeshPro.text)) { yield return null; }
            else if (textMeshProUGUI && string.IsNullOrEmpty (textMeshProUGUI.text)) { yield return null; }

            // Color
            Color color = (textMeshPro ? textMeshPro.color : textMeshProUGUI.color);
            color.a = (color.a == 1f ? 0f : 1f );

            if (textMeshPro) { textMeshPro.color = color; }
            else { textMeshProUGUI.color = color; }
            
            yield return new WaitForSeconds (timeToFlick);
            yield return null;
        }
    }
}