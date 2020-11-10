using UnityEngine;
using UnityEngine.EventSystems;

public class ReleaseBall : MonoBehaviour, IPointerUpHandler
{
    //--------------------------------------------------------------------------------//
    // IPOINTERUPHANDLER

    public virtual void OnPointerUp (PointerEventData pointerEventData)
    {
        // Cancels
        if (!GameSession.Instance) { return; }

        if (GameSession.Instance.GetActualGameState () == Enumerators.GameStates.GAMEPLAY)
        {
            if (!GameSession.Instance.GetHasStarted ())
            {
                // Launches the ball
                Ball ball = FindObjectOfType<Ball>();
                if (ball) { ball.LaunchBall (); }
            }
            else 
            {
                // Shooting if has shooter
                Shooter shooter = FindObjectOfType<Shooter>();
                if (shooter) { shooter.Shoot (); }
            }
        }
    }
}