using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBaseState
{ 

    public HeroData PlayerData;
    public HeroKeys PlayerControls;
    
    public HeroBaseState()
    {
        PlayerData = GameObject.FindGameObjectWithTag("Test").GetComponentInChildren<HeroData>();
        PlayerControls = GameObject.FindGameObjectWithTag("Test").GetComponentInChildren<HeroKeys>();
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void RegularUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {


    }


}
