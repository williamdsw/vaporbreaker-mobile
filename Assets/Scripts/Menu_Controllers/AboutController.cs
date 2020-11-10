using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AboutController : MonoBehaviour
{
    [Header ("Main Elements")]
    [SerializeField] private GameObject configurationPanel;
    [SerializeField] private GameObject aboutPanel;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI creditsText;

    [Header ("Labels to Translate")]
    [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI> ();

    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOUR

    private void Start () 
    {
        TranslateLabels ();
        BindClickEvents ();
        LoadCredits ();
    }

    //--------------------------------------------------------------------------------//
    // HELPER FUNCTIONS

    // Translate labels based on choosed language
    private void TranslateLabels ()
    {
        // CANCELS
        if (!LocalizationController.Instance) { return; }
        
        List<string> labels = new List<string> ();
        foreach (string label in LocalizationController.Instance.GetAboutLabels ()) { labels.Add (label); }
        if (labels.Count == 0 || uiLabels.Count == 0) { return; }
        for (int index = 0; index < labels.Count; index++) { uiLabels[index].SetText (labels[index]); }
    }

    private void BindClickEvents ()
    {
        // Cancels
        if (!quitButton) { return; }

        // Close the panel
        quitButton.onClick.AddListener (() =>
        {
            // Cancels
            if (!SelectLevelsController.Instance) { return; }
            if (SelectLevelsController.Instance.GetActualGameState () != Enumerators.GameStates.GAMEPLAY) { return; }
            if (!aboutPanel || !configurationPanel) { return; }

            aboutPanel.SetActive (false);
            configurationPanel.SetActive (true);
        });
    }

    private void LoadCredits ()
    {
        // Cancel
        if (!creditsText) { return; }

        string creditsRaw = FileManager.LoadAsset (FileManager.GetOtherFolderPath (), FileManager.GetCreditsPath ());
        if (string.IsNullOrEmpty (creditsRaw) || string.IsNullOrWhiteSpace (creditsRaw)) { return; }
        creditsText.SetText (creditsRaw);
    }
}