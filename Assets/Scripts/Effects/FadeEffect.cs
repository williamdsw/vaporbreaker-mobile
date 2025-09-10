using Controllers.Core;
using System;
using UnityEngine;
using Utilities;

namespace Effects
{
    /// <summary>
    /// Fade In | Fade Out Effect
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class FadeEffect : MonoBehaviour
    {
        private class FunctionNames
        {
            public static string CallLevelMenu => "CallLevelMenu";
            public static string CallResetLevel => "CallResetLevel";
            public static string DefineGameState => "DefineGameState";
        }

        // || Config

        private readonly int SCENE_TO_APPLY_EVENTS = 4;

        // || Cached

        private Animator animator;

        // || Properties

        public static FadeEffect Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            GetRequiredComponents();
        }

        private void Start()
        {
            if (SceneManagerController.GetActiveSceneIndex() == SCENE_TO_APPLY_EVENTS)
            {
                CreateFadeInEvents();
                CreateFadeOutEvents();
            }
        }

        /// <summary>
        /// Get required components
        /// </summary>
        public void GetRequiredComponents()
        {
            try
            {
                animator = GetComponent<Animator>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create events to be called on Fade In
        /// </summary>
        private void CreateFadeInEvents()
        {
            try
            {
                AnimationClip fadeInClip = animator.runtimeAnimatorController.animationClips[0];
                fadeInClip.events = new AnimationEvent[0];

                // First frame event
                AnimationEvent firstFrameEvent = new AnimationEvent();
                firstFrameEvent.intParameter = 2;
                firstFrameEvent.time = 0f;
                firstFrameEvent.functionName = FunctionNames.DefineGameState;
                fadeInClip.AddEvent(firstFrameEvent);

                fadeInClip = animator.runtimeAnimatorController.animationClips[0];

                // Last frame event
                AnimationEvent lastFrameEvent = new AnimationEvent();
                lastFrameEvent.intParameter = 0;
                lastFrameEvent.time = 1f;
                lastFrameEvent.functionName = FunctionNames.DefineGameState;
                fadeInClip.AddEvent(lastFrameEvent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create events to be called on Fade Out
        /// </summary>
        private void CreateFadeOutEvents()
        {
            try
            {
                AnimationClip fadeOutClip = animator.runtimeAnimatorController.animationClips[1];
                fadeOutClip.events = new AnimationEvent[0];

                // First frame event
                AnimationEvent firstFrameEvent = new AnimationEvent();
                firstFrameEvent.intParameter = 2;
                firstFrameEvent.time = 0f;
                firstFrameEvent.functionName = FunctionNames.DefineGameState;
                fadeOutClip.AddEvent(firstFrameEvent);

                // Last frame event
                AnimationEvent lastFrameEvent = new AnimationEvent();
                lastFrameEvent.time = 1f;
                lastFrameEvent.functionName = FunctionNames.CallResetLevel;
                fadeOutClip.AddEvent(lastFrameEvent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reset fade out events to level menu
        /// </summary>
        public void ResetFadeOutEventsToLevelMenu()
        {
            try
            {
                AnimationClip fadeOutClip = animator.runtimeAnimatorController.animationClips[1];
                fadeOutClip.events = new AnimationEvent[0];

                // Last frame event
                AnimationEvent lastFrameEvent = new AnimationEvent();
                lastFrameEvent.time = 1f;
                lastFrameEvent.functionName = FunctionNames.CallLevelMenu;
                fadeOutClip.AddEvent(lastFrameEvent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reset all animation functions
        /// </summary>
        public void ResetAnimationFunctions()
        {
            if (!animator)
            {
                GetRequiredComponents();
            }

            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                clip.events = new AnimationEvent[0];
            }
        }

        /// <summary>
        /// Get fade out animation length
        /// </summary>
        /// <returns> Fade Out animation length </returns>
        public float GetFadeOutLength() => (animator ? animator.runtimeAnimatorController.animationClips[1].length : 0f);

        /// <summary>
        /// Fade in / out to level
        /// </summary>
        public void FadeToLevel()
        {
            animator.Rebind();
            animator.SetTrigger(NamesTags.AnimatorTriggers.FadeOut);
        }

        /// <summary>
        /// Call animation to reset level
        /// </summary>
        public void CallResetLevel()
        {
            animator.Rebind();
            GameSessionController.Instance.ResetLevel();
        }

        /// <summary>
        /// Call animation to level menu
        /// </summary>
        public void CallLevelMenu()
        {
            animator.Rebind();
            GameSessionController.Instance.GotoScene(SceneManagerController.SceneNames.SelectLevels);
        }

        /// <summary>
        /// Define new game state on fade in / fade out
        /// </summary>
        /// <param name="gameStateInt"> Next game state </param>
        public void DefineGameState(int gameStateInt)
        {
            switch (gameStateInt)
            {
                case 0: GameSessionController.Instance.ActualGameState = Enumerators.GameStates.GAMEPLAY; break;
                case 1: GameSessionController.Instance.ActualGameState = Enumerators.GameStates.PAUSE; break;
                case 2: GameSessionController.Instance.ActualGameState = Enumerators.GameStates.TRANSITION; break;
                default: break;
            }
        }
    }
}