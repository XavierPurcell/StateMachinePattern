using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDefaultGroundedState : HeroOnGroundState
{

    public override void Enter()
    {
        if (PlayerData.ParentState != ParentState)
        {
            base.Enter(); // run the containers ground state
            PlayerData.ParentState = ParentState;

        }
    }

    public override void Exit()
    {
        //exit code
        // leave grounding exit code
        base.Exit();
    }

    public override void RegularUpdate()
    {
        base.RegularUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        GroundedActions();
    }


    public void GroundedActions()
    {
        if (PlayerControls.KeyWInitialDown)
        {
            JumpedAction();
        }

        if (PlayerControls.KeyDDown || PlayerControls.KeyADown == true)
        {
            PlayerData.mAnimator.SetBool("Run", true);
            if (PlayerControls.KeyADown)
            {

                if (PlayerData.SlideEnabled)
                {
                    SlideRun(Vector2.left);
                }
                else
                {
                    Run(Vector2.left);
                }

                PlayerData.LastDirection = Vector2.left;
            }
            if (PlayerControls.KeyDDown)
            {
                if (PlayerData.SlideEnabled)
                {
                    SlideRun(Vector2.right);
                }
                else
                {
                    Run(Vector2.right);
                }

                PlayerData.LastDirection = Vector2.right;
            }

        }
        else if (PlayerControls.KeyAUp || PlayerControls.KeyDUp)
        {
            if (PlayerControls.KeyAUp)
            {
                PlayerControls.KeyAUp = false;
            }

            if (PlayerControls.KeyDUp)
            {
                PlayerControls.KeyDUp = false;
            }

            if (PlayerData.SlideEnabled)
            {
                Decel();
            }
            else
            {
                PlayerData.RB2D.velocity = new Vector2(0, PlayerData.RB2D.velocity.y);
                PlayerData.mAnimator.SetBool("Run", false);
            }
            PlayerData.MovementTimer = 0;
        }
        else
        {

            if (PlayerData.SlideEnabled)
            {
                Decel();
            }
            else
            {
                PlayerData.RB2D.velocity = new Vector2(0, PlayerData.RB2D.velocity.y);
                PlayerData.mAnimator.SetBool("Run", false);
            }

        }
    }


    public void Decel()
    {
        if (PlayerData.RB2D.velocity.x == 0)
        {
            return;
        }

        PlayerData.DecelTimer += Time.deltaTime * PlayerData.Decceleration;

        if (PlayerData.DecelTimer > 1)
        {
            PlayerData.DecelTimer = 1;

        }

        float Speed = Mathf.Lerp(PlayerData.UpdatedSpeed, 0, PlayerData.DecelTimer);
        PlayerData.RB2D.velocity = new Vector2(Speed, PlayerData.RB2D.velocity.y);
    }

    public void Run(Vector2 Direction)
    {

        // animations
        //PlayerData.mAnimator.SetTrigger("Run");

        // change direction
        if (PlayerData.LastDirection != Direction)
        {
            PlayerData.MovementTimer = 0;
            PlayerData.RB2D.velocity = new Vector2(0, PlayerData.RB2D.velocity.y);
        }

        if (PlayerData.RB2D.velocity.x == 0)
        {
            PlayerData.MovementTimer = 0;
        }
        // set initial speed
        if (PlayerData.MovementTimer == 0)
        {
            float InitialForceToAdd = PlayerData.InitialSpeed - PlayerData.RB2D.velocity.x;

            if (PlayerData.RB2D.velocity.x < 0)
            {
                InitialForceToAdd = PlayerData.InitialSpeed + PlayerData.RB2D.velocity.x;
            }

            if (PlayerData.RB2D.velocity.x > PlayerData.InitialSpeed || PlayerData.RB2D.velocity.x < PlayerData.InitialSpeed * -1)
            {
                InitialForceToAdd = PlayerData.Acceleration;
            }

            PlayerData.RB2D.AddForce(new Vector2(InitialForceToAdd * Direction.x, 0), ForceMode2D.Impulse);
            PlayerData.MovementTimer += Time.deltaTime;

            if (PlayerData.MovementTimer > 1)
            {
                PlayerData.MovementTimer = 1;
            }
        }
        else
        {
            PlayerData.MovementTimer += Time.deltaTime;

            if (PlayerData.MovementTimer > 1)
            {
                PlayerData.MovementTimer = 1;
            }

            PlayerData.RB2D.AddForce(new Vector2(PlayerData.Acceleration * Direction.x, 0), ForceMode2D.Impulse);
        }


        float CheckSpeed = PlayerData.RB2D.velocity.x;

        if (CheckSpeed > PlayerData.MaxSpeed)
        {
            PlayerData.RB2D.velocity = new Vector2(PlayerData.MaxSpeed, PlayerData.RB2D.velocity.y);
        }
        else if (CheckSpeed < PlayerData.MaxSpeed * -1)
        {
            PlayerData.RB2D.velocity = new Vector2(PlayerData.MaxSpeed * -1, PlayerData.RB2D.velocity.y);
        }

        PlayerData.DecelTimer = 0;
        PlayerData.UpdatedSpeed = PlayerData.RB2D.velocity.x;
    }

    public void SlideRun(Vector2 Direction)
    {
        // turn right
        if (Direction.x > 0 && PlayerData.RB2D.velocity.x < 0)
        {
            PlayerData.RB2D.AddForce(new Vector2(PlayerData.DecelTurnSpeed * Direction.x, 0), ForceMode2D.Impulse);
        }
        //turn left
        else if (Direction.x < 0 && PlayerData.RB2D.velocity.x > 0)
        {
            PlayerData.RB2D.AddForce(new Vector2(PlayerData.DecelTurnSpeed * Direction.x, 0), ForceMode2D.Impulse);
        }// else apply normal force
        else
        {
            PlayerData.RB2D.AddForce(new Vector2(PlayerData.Acceleration * Direction.x, 0), ForceMode2D.Impulse);
        }


        // do we want initial speed? Doesn't really work with a full slide implementation. Need Partial Slide that is only enabled on turns
        /*
            float InitialForceToAdd = PlayerData.InitialSpeed - PlayerData.RB2D.velocity.x;

                    if (PlayerData.RB2D.velocity.x < 0)
                    {
                        InitialForceToAdd = PlayerData.InitialSpeed + PlayerData.RB2D.velocity.x;
                    }

                    if (InitialForceToAdd > PlayerData.InitialSpeed)
                    {
                        InitialForceToAdd = 0;
                    }

                    PlayerData.RB2D.AddForce(new Vector2(InitialForceToAdd * Direction.x, 0), ForceMode2D.Impulse);
        */

        float CheckSpeed = PlayerData.RB2D.velocity.x;

        if (CheckSpeed > PlayerData.MaxSpeed)
        {
            PlayerData.RB2D.velocity = new Vector2(PlayerData.MaxSpeed, PlayerData.RB2D.velocity.y);
        }
        else if (CheckSpeed < PlayerData.MaxSpeed * -1)
        {
            PlayerData.RB2D.velocity = new Vector2(PlayerData.MaxSpeed * -1, PlayerData.RB2D.velocity.y);
        }
        PlayerData.DecelTimer = 0;
        PlayerData.UpdatedSpeed = PlayerData.RB2D.velocity.x;
    }

    public void JumpedAction()
    {
        PlayerControls.KeyWInitialDown = false;
        PlayerData.JumpHeightTimer = PlayerData.JumpHeightTimerValue;
        PlayerData.HasJumped = true;
        PlayerData.RB2D.velocity = new Vector2(PlayerData.RB2D.velocity.x, Vector2.up.y * PlayerData.JumpVelocity);
    }
}

