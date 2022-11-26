using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroMethodManager : MonoBehaviour
{

    HeroData PlayerData;
    HeroControls PlayerControls;


    // state managers
    HeroAirStateManager _HeroAirStateManager;
    HeroJumpedOffWallStateManager _HeroJumpedOffWallStateManager;
    HeroOnWallStateManager _HeroOnWallStateManager;
    HeroOnGroundStateManager _HeroOnGroundStateManager;
    HeroDashStateManager _HeroDashStateManager;

    //states

    void Start()
    {
        PlayerControls = GetComponentInChildren<HeroControls>();
        PlayerData = GetComponentInChildren<HeroData>();

        // states
        _HeroAirStateManager = GetComponentInChildren<HeroAirStateManager>();
        _HeroJumpedOffWallStateManager = GetComponentInChildren<HeroJumpedOffWallStateManager>();
        _HeroOnWallStateManager = GetComponentInChildren<HeroOnWallStateManager>();
        _HeroOnGroundStateManager = GetComponentInChildren<HeroOnGroundStateManager>();
        _HeroDashStateManager = GetComponentInChildren<HeroDashStateManager>();

    }

    // states


   
    public void HeroStateController()
    {
         // on the ground
        if (PlayerData.PlayerState != "PlayerData.OnGround" && PlayerData.OnGround == true)
        {
            InitialGroundState();
        }
        else if (PlayerData.PlayerState != "PlayerData.OnWallLeft" && PlayerData.OnWallLeft && PlayerControls.WalkLeft && PlayerData.RB2D.velocity.y <= 0 && !PlayerData.OnGround)
        {
            InitialWallState("PlayerData.OnWallLeft");
        }
        else if (PlayerData.PlayerState != "PlayerData.OnWallRight" && PlayerData.OnWallRight && PlayerControls.WalkRight && PlayerData.RB2D.velocity.y <= 0 && !PlayerData.OnGround)
        {
            InitialWallState("PlayerData.OnWallRight");
        }// in the air
        else if (PlayerData.PlayerState != "Air" && PlayerData.OnGround == false && !PlayerData.MountedOnWall)
        {
            InitialAirState();
        }
    }

    public void MethodController()
    {

        if (PlayerData.CurrentlyDashing)
        {
            return;
        }


        if (PlayerData.OnGround)
        {
            PlayerData.PlayerMethod = "OnGroundMethod";
            _HeroOnGroundStateManager.GroundedActions();
        }
        else if (PlayerData.PlayerState == "PlayerData.OnWallLeft")
        {
            PlayerData.PlayerMethod = "OnWallLeftMethod";
            _HeroOnWallStateManager.WallActions(Vector2.right);

        }
        else if (PlayerData.PlayerState == "PlayerData.OnWallRight")
        {
            PlayerData.PlayerMethod = "OnWallRightMethod";
            _HeroOnWallStateManager.WallActions(Vector2.left);

        }
        else if (PlayerData.JumpedOffWall)
        {
            PlayerData.PlayerMethod = "JumpedOffWallMethod";
            _HeroJumpedOffWallStateManager.JumpedOffWallMethod();
        }
        else if (PlayerData.InWater)
        {

        }
        else if (PlayerData.OnIce)
        {

        }
        else // if all states are false we are in the air
        {
            PlayerData.PlayerMethod = "AirMethod";
            _HeroAirStateManager.AirActions();
        }

        if (PlayerControls.KeyDownDash)
        {
            PlayerData.CurrentlyDashing = true;
            PlayerControls.KeyDownDash = false;
            _HeroDashStateManager.DashingMethod();

            //do the dash method
        }

    }




    public void StateController()
    {

        // on the ground
        if (PlayerData.PlayerState != "PlayerData.OnGround" && PlayerData.OnGround == true)
        {
            InitialGroundState();
        }
        else if (PlayerData.PlayerState != "PlayerData.OnWallLeft" && PlayerData.OnWallLeft && PlayerControls.WalkLeft && PlayerData.RB2D.velocity.y <= 0 && !PlayerData.OnGround)
        {
            InitialWallState("PlayerData.OnWallLeft");
        }
        else if (PlayerData.PlayerState != "PlayerData.OnWallRight" && PlayerData.OnWallRight && PlayerControls.WalkRight && PlayerData.RB2D.velocity.y <= 0 && !PlayerData.OnGround)
        {
            InitialWallState("PlayerData.OnWallRight");
        }// in the air
        else if (PlayerData.PlayerState != "Air" && PlayerData.OnGround == false && !PlayerData.MountedOnWall)
        {
            InitialAirState();
        }
    }



    public void InAirVariableReset()
    {
        PlayerData.MovementTimer = 0;
        PlayerData.DecelTimer = 0;
        PlayerData.RB2D.gravityScale = PlayerData.UpGravity;
    }
    public void InitialAirState()
    {
        PlayerData.PlayerState = "Air";
        if (PlayerData.HasJumped == false) // if we're in the air and we havent PlayerData.HasJumped we have the option of coyote jumping
        {
            PlayerData.CoyoteTimer = PlayerData.CoyoteValue;
        }
        InAirVariableReset();
        PlayerData.mAnimator.SetBool("Jump", true);
        PlayerData.mAnimator.SetBool("Falling", false);
        PlayerData.mAnimator.SetBool("Run", false);
        PlayerData.mAnimator.SetBool("WallGrab", false);
    }

    private void InitialGroundState()
    {
        PlayerData.PlayerState = "PlayerData.OnGround";
        //Ground Variables
        OnGroundMovementReset();
        // Wall/Jump Variables
        OffWallJumpVariablesReset();
        // Ground velocity set to how fast we were going in the air.
        PlayerData.RB2D.velocity = new Vector2(PlayerData.UpdatedSpeed, PlayerData.RB2D.velocity.y);

        PlayerData.mAnimator.SetBool("Jump", false);
        PlayerData.mAnimator.SetBool("Falling", false);
    }
    private void InitialWallState(string Direction)
    {
        PlayerData.PlayerState = Direction;
        OnGroundMovementReset();
        OnWallJumpVariablesReset();
        PlayerData.mAnimator.SetBool("WallGrab", true);
    }
    private void OnGroundMovementReset()
    {
        // key up values set to default
        PlayerControls.KeyUpLeft = false;
        PlayerControls.KeyUpRight = false;
        // jump keys set to default values
        PlayerData.HasJumped = false;
        PlayerControls.KeyUpJump = false;
        PlayerControls.KeyUpLeft = false;
        PlayerControls.KeyUpRight = false;
        PlayerControls.KeyJump = false;
        PlayerControls.KeyDownJump = false;

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


}
