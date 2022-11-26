using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWallState : HeroOnWallState
{

    public override void Enter()
    {
        if (PlayerData.ParentState != ParentState)
        {
            base.Enter(); // run the containers ground state
            PlayerData.ParentState = ParentState;
        }
        InitialWallState();
    }


    private void InitialWallState()
    {
        OnGroundMovementReset();
        OnWallJumpVariablesReset();
        PlayerData.mAnimator.SetBool("WallGrab", true);
    }
    private void OnGroundMovementReset()
    {
        // key up values set to default
        PlayerControls.KeyAUp = false;
        PlayerControls.KeyDUp = false;
        // jump keys set to default values
        PlayerData.HasJumped = false;
        PlayerControls.KeyWUp = false;
        PlayerControls.KeyWInitialDown = false;


        // reset timers
        PlayerData.MovementTimer = 0;
        PlayerData.JumpHeightTimer = 0;
        PlayerData.CoyoteTimer = 0;
        PlayerData.DecelTimer = 0;
        PlayerData.RB2D.gravityScale = PlayerData.DownGravity;
    }
    private void OnWallJumpVariablesReset()
    {
        PlayerData.HasJumped = true;
        PlayerData.MountedOnWall = true;
        PlayerData.JumpedOffWall = false;
    }
    private void OffWallJumpVariablesReset()
    {
        PlayerData.MountedOnWall = false;
        PlayerData.JumpedOffWall = false;
    }


    public override void Exit()
    {
        base.Exit();
    }

    public override void RegularUpdate()
    {
        base.RegularUpdate();

    }

    public override void PhysicsUpdate()
    {
        base.Enter();



        Vector2 Direction = Vector2.right;

        if (PlayerData.OnWallRight)
        {
            Direction = Vector2.left;
        }

        WallActions(Direction);
    }


    public void WallActions(Vector2 Direction)
    {
        if (!PlayerData.OnWallLeft && !PlayerData.OnWallRight)
        {
            PlayerData.MountedOnWall = false;
            return;
        }

        if (PlayerData.OnWallLeft && !PlayerControls.KeyADown)
        {
            PlayerData.MountedOnWall = false;
            return;
        }

        if (PlayerData.OnWallRight && !PlayerControls.KeyDDown)
        {
            PlayerData.MountedOnWall = false;
            return;
        }

        if (PlayerControls.KeyWInitialDown)
        {
            WallJump(Direction);
            PlayerData.mAnimator.SetBool("Jump", true);
            PlayerData.mAnimator.SetBool("Falling", false);
            PlayerData.mAnimator.SetBool("WallGrab", false);
        }
        else
        {
            PlayerData.RB2D.gravityScale = 0;
            PlayerData.RB2D.velocity = new Vector2(0, PlayerData.WallSlideDownSpeed);
        }

    }

    private void WallJump(Vector2 Direction)
    {
        PlayerData.RB2D.gravityScale = PlayerData.UpGravity;
        PlayerData.RB2D.velocity = new Vector2(Direction.x * PlayerData.JumpVelocity / 2, Vector2.up.y * PlayerData.JumpVelocity / 1.5f);
        PlayerData.MountedOnWall = false;
        PlayerData.JumpedOffWall = true;
        PlayerData.JumpedOffWallTimer = PlayerData.JumpedOffWallTimerValue;
    }



}
