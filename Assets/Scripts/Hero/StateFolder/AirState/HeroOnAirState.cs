using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeroOnAirState : HeroBaseState
{

    public string ParentState = "AirState";

    //private HeroRunState HeroRunState = new HeroRunState();
    //private HeroRunState HeroRunState = new HeroRunState();

    public override void Enter()
    {
        base.Enter();
        PlayerData.mAnimator.SetBool("AirState", true);
    }

    public override void Exit()
    {
        base.Exit();

        PlayerData.mAnimator.SetBool("AirState", false);
        PlayerData.mAnimator.SetBool("Jump", false);
        PlayerData.mAnimator.SetBool("Falling", false);

        // Ground velocity set to how fast we were going in the air.
        PlayerData.RB2D.velocity = new Vector2(PlayerData.UpdatedSpeed, PlayerData.RB2D.velocity.y);
    }

    public override void RegularUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
       
    }

}
