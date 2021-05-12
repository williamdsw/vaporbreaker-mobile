using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Effects
{
    [RequireComponent(typeof(Animator))]
    public class FadeEffect : MonoBehaviour
    {
        // Config
        private int minimumSceneIndexToEvents = 3;

        // Cached
        private Animator animator;
        private Enumerators.GameStates newGameState;

        public static FadeEffect Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (SceneManagerController.GetActiveSceneIndex() >= minimumSceneIndexToEvents)
            {
                CreateFadeInEvents();
                CreateFadeOutEvents();
            }
        }

        private void CreateFadeInEvents()
        {
            AnimationClip fadeInClip = animator.runtimeAnimatorController.animationClips[0];
            fadeInClip.events = null;

            // First frame event
            AnimationEvent firstFrameEvent = new AnimationEvent();
            firstFrameEvent.intParameter = 2;
            firstFrameEvent.time = 0f;
            firstFrameEvent.functionName = "DefineGameState";
            fadeInClip.AddEvent(firstFrameEvent);

            fadeInClip = animator.runtimeAnimatorController.animationClips[0];

            // Last frame event
            AnimationEvent lastFrameEvent = new AnimationEvent();
            lastFrameEvent.intParameter = 0;
            lastFrameEvent.time = 1f;
            lastFrameEvent.functionName = "DefineGameState";
            fadeInClip.AddEvent(lastFrameEvent);
        }

        private void CreateFadeOutEvents()
        {
            AnimationClip fadeOutClip = animator.runtimeAnimatorController.animationClips[1];
            fadeOutClip.events = null;

            // First frame event
            AnimationEvent firstFrameEvent = new AnimationEvent();
            firstFrameEvent.intParameter = 2;
            firstFrameEvent.time = 0f;
            firstFrameEvent.functionName = "DefineGameState";
            fadeOutClip.AddEvent(firstFrameEvent);

            // Last frame event
            AnimationEvent lastFrameEvent = new AnimationEvent();
            lastFrameEvent.time = 1f;
            lastFrameEvent.functionName = "CallResetLevel";
            fadeOutClip.AddEvent(lastFrameEvent);
        }

        public void ResetFadeOutEventsToLevelMenu()
        {
            AnimationClip fadeOutClip = animator.runtimeAnimatorController.animationClips[1];
            fadeOutClip.events = null;

            // Last frame event
            AnimationEvent lastFrameEvent = new AnimationEvent();
            lastFrameEvent.time = 1f;
            lastFrameEvent.functionName = "CallLevelMenu";
            fadeOutClip.AddEvent(lastFrameEvent);
        }

        public void ResetAnimationFunctions()
        {
            if (!animator)
            {
                animator = this.GetComponent<Animator>();
            }

            AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in animationClips)
            {
                clip.events = null;
            }
        }

        public float GetFadeOutLength() => (animator ? animator.runtimeAnimatorController.animationClips[1].length : 0);

        public void FadeToLevel()
        {
            animator.Rebind();
            animator.SetTrigger("FadeOut");
        }

        public void CallResetLevel()
        {
            animator.Rebind();
            GameSession.Instance.ResetLevel();
        }

        public void CallLevelMenu()
        {
            animator.Rebind();
            GameSession.Instance.ResetGame(SceneManagerController.SelectLevelsSceneName);
        }

        public void DefineGameState(int gameStateInt)
        {
            switch (gameStateInt)
            {
                case 0: newGameState = Enumerators.GameStates.GAMEPLAY; break;
                case 1: newGameState = Enumerators.GameStates.PAUSE; break;
                case 2: newGameState = Enumerators.GameStates.TRANSITION; break;
                default: break;
            }

            GameSession.Instance.SetActualGameState(newGameState);
        }
    }
}