using UnityEngine;

[RequireComponent (typeof (Animator)) ]
public class FadeEffect : MonoBehaviour
{
    // Config
    private int minimumSceneIndexToEvents = 3;

    // Cached
    private Animator animator;
    private Enumerators.GameStates newGameState;

    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOUR

    private void Awake () 
    {
        // Find Component
        animator = this.GetComponent<Animator>();
    }

    private void Start () 
    {
        if (SceneManagerController.GetActiveSceneIndex () >= minimumSceneIndexToEvents)
        {
            CreateFadeInEvents ();
            CreateFadeOutEvents ();
        }
    }

    //--------------------------------------------------------------------------------//
    // HELPER FUNCTIONS

    private void CreateFadeInEvents ()
    {
        // Check and cancels
        if (!animator) { return; }

        AnimationClip fadeInClip = animator.runtimeAnimatorController.animationClips[0];
        fadeInClip.events = null;

        // First frame event
        AnimationEvent firstFrameEvent = new AnimationEvent ();
        firstFrameEvent.intParameter = 2;
        firstFrameEvent.time = 0f;
        firstFrameEvent.functionName = "DefineGameState";
        fadeInClip.AddEvent (firstFrameEvent);

        fadeInClip = animator.runtimeAnimatorController.animationClips[0];

        // Last frame event
        AnimationEvent lastFrameEvent = new AnimationEvent ();
        lastFrameEvent.intParameter = 0;
        lastFrameEvent.time = 1f;
        lastFrameEvent.functionName = "DefineGameState";
        fadeInClip.AddEvent (lastFrameEvent);
    }

    private void CreateFadeOutEvents ()
    {
        // Check and cancels
        if (!animator) { return; }

        AnimationClip fadeOutClip = animator.runtimeAnimatorController.animationClips[1];
        fadeOutClip.events = null;

        // First frame event
        AnimationEvent firstFrameEvent = new AnimationEvent ();
        firstFrameEvent.intParameter = 2;
        firstFrameEvent.time = 0f;
        firstFrameEvent.functionName = "DefineGameState";
        fadeOutClip.AddEvent (firstFrameEvent);

        // Last frame event
        AnimationEvent lastFrameEvent = new AnimationEvent ();
        lastFrameEvent.time = 1f;
        lastFrameEvent.functionName = "CallResetLevel";
        fadeOutClip.AddEvent (lastFrameEvent);
    }

    public void ResetFadeOutEventsToLevelMenu ()
    {
        // Check and cancels
        if (!animator) { return; }

        AnimationClip fadeOutClip = animator.runtimeAnimatorController.animationClips[1];
        fadeOutClip.events = null;

        // Last frame event
        AnimationEvent lastFrameEvent = new AnimationEvent ();
        lastFrameEvent.time = 1f;
        lastFrameEvent.functionName = "CallLevelMenu";
        fadeOutClip.AddEvent (lastFrameEvent);
    }

    public void ResetAnimationFunctions ()
    {
        if (!animator) { animator = this.GetComponent<Animator>(); }
        
        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips) { clip.events = null; }
    }

    public float GetFadeOutLength ()
    {
        // Check and cancels
        if (!animator) { return 0; }

        return animator.runtimeAnimatorController.animationClips[1].length;
    }

    public void FadeToLevel ()
    {
        // Check and cancels
        if (!animator) { return; }
        animator.Rebind ();
        animator.SetTrigger ("FadeOut");
    }

    public void CallResetLevel ()
    {
        // Cancels
        if (!GameSession.Instance || !animator) { return; }
        animator.Rebind ();
        GameSession.Instance.ResetLevel ();
    }

    public void CallLevelMenu ()
    {
        // Cancels
        if (!GameSession.Instance || !animator) { return; }
        animator.Rebind ();
        GameSession.Instance.ResetGame (SceneManagerController.SelectLevelsSceneName);
    }

    public void DefineGameState (int gameStateInt)
    {
        // Cancels
        if (!GameSession.Instance) { return; }

        if (gameStateInt == 0) { newGameState = Enumerators.GameStates.GAMEPLAY; }
        else if (gameStateInt == 1) { newGameState = Enumerators.GameStates.PAUSE; }
        else if (gameStateInt == 2) { newGameState = Enumerators.GameStates.TRANSITION; }

        GameSession.Instance.SetActualGameState (newGameState);
    }
}