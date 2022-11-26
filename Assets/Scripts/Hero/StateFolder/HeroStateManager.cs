using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateManager : HeroBaseState
{

    // states

    // could split this into a substate system to make it look cleaner, but I swear this is easier to keep track of until your character control is insane in size.

    HeroBaseState CurrentState = new HeroDefaultGroundedState();
    readonly HeroBaseState Grounded = new HeroDefaultGroundedState();
    readonly HeroBaseState GroundedAttack = new HeroDefaultGroundedState();
    readonly HeroBaseState AirState = new HeroAirState();
    readonly HeroBaseState WallState = new HeroWallState();
    public void HeroStateController()
    {

        if (PlayerData.OnGround)
        {
            // bunch of grounded Methods
            if (CurrentState != Grounded)
            {
                ChangeState(Grounded);
            }
            else if (CurrentState != GroundedAttack)
            {
                //example of how it could look
                //ChangeState(GroundedAttack);
            }
            else if (PlayerData.FallMaxSpeed != 3423434f)
            {
                //example of how it could look
            }
            else if (PlayerData.FallMaxSpeed != 3423434f)
            {
                //example of how it could look
            }
            else
            {
                //example of how it could look
            }
        }
        else if (PlayerData.OnWallLeft && PlayerData.Velocity.y < 0 && PlayerControls.KeyADown || PlayerData.OnWallRight && PlayerData.Velocity.y < 0 && PlayerControls.KeyDDown)
        {
            if (CurrentState != WallState)
            {
                ChangeState(WallState);
            }
        }
        else
        {
            if (CurrentState != AirState)
            {
                ChangeState(AirState);
            }
        }

    }

    public void HeroSubStateControllerUpdate()
    {
        CurrentState.RegularUpdate();
    }

    public void HeroSubStateControllerPhysics()
    {
        CurrentState.PhysicsUpdate();
    }

    private void InitialState()
    {
        CurrentState = new HeroBaseState();
    }

    private void ChangeState(HeroBaseState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

}
