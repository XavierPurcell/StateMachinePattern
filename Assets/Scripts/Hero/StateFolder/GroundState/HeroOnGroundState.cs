using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeroOnGroundState : HeroBaseState
{

    public string ParentState = "GroundState";

    //private HeroRunState HeroRunState = new HeroRunState();
    //private HeroRunState HeroRunState = new HeroRunState();

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("HereOnGroundPlayEnter");
        OnGroundMovementReset();
        PlayerData.mAnimator.SetBool("GroundState", true);
    }

    public override void Exit()
    {
        PlayerData.mAnimator.SetBool("GroundState", false);
        base.Exit();
    }

    public override void RegularUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
       
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

        //reset other state variables 
        PlayerData.MountedOnWall = false;
        PlayerData.JumpedOffWall = false;
    }


}
