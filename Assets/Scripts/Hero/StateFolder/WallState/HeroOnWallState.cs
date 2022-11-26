using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeroOnWallState : HeroBaseState
{

    public string ParentState = "WallState";

    public override void Enter()
    {
        base.Enter();
        PlayerData.mAnimator.SetBool("WallState", true);
    }

    public override void Exit()
    {
        base.Exit();
        PlayerData.mAnimator.SetBool("WallState", false);
    }

    public override void RegularUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
       
    }

}
