using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAirStateManager : MonoBehaviour
{


    HeroData PlayerData;
    HeroControls PlayerControls;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GetComponentInParent<HeroData>();
        PlayerControls = GetComponentInParent<HeroControls>();
    }

    public void AirActions()
    {
        if (PlayerControls.Walk == true)
        {
            if (PlayerControls.WalkLeft)
            {
                AirMovement(Vector2.left);
            }
            else if (PlayerControls.WalkRight)
            {
                AirMovement(Vector2.right);
            }

        }
        else if (!PlayerData.FollowThroughAirMomentum)
        {
            PlayerData.RB2D.velocity = new Vector2(0, PlayerData.RB2D.velocity.y);
        }
        CoyoteJump();
        JumpGravity();

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

    public void AirMovement(Vector2 Direction)
    {
        // set initial speed
        if (PlayerData.MovementTimer == 0)
        {
            PlayerData.RB2D.AddForce(new Vector2(PlayerData.AirInitialSpeed * Direction.x, 0), ForceMode2D.Impulse);
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

            PlayerData.RB2D.AddForce(new Vector2(PlayerData.AirMovementAccel * Direction.x, 0), ForceMode2D.Impulse);
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

    public void CoyoteJump()
    {
        if (PlayerData.CoyoteTimer > 0)
        {
            PlayerData.CoyoteTimer -= Time.deltaTime;
            if (PlayerData.CoyoteTimer < 0)
            {
                PlayerData.CoyoteTimer = 0;
            }
            if (PlayerControls.KeyDownJump)
            {
                PlayerData.CoyoteTimer = 0;
                InitialAirState();
                JumpedAction();
            }
        }
    }

    public void JumpGravity()
    {
        // if we are falling set fall gravity
        if (PlayerData.RB2D.velocity.y < 0)
        {
            if (PlayerData.RB2D.velocity.y <= PlayerData.FallMaxSpeed * -1)
            {
                PlayerData.RB2D.velocity = new Vector2(PlayerData.RB2D.velocity.x, Vector2.down.y * PlayerData.FallMaxSpeed);
                PlayerData.RB2D.gravityScale = 0;
            }
            else
            {
                PlayerData.RB2D.gravityScale = PlayerData.DownGravity;
            }

            PlayerData.mAnimator.SetBool("Falling", true);
        }
        else
        {   // if we are ascending set jump gravity
            PlayerData.RB2D.gravityScale = PlayerData.SlowJumpGravity;
        }

        if (PlayerControls.KeyUpJump) // if we let go of the jump key.
        {
            return;
        }

        // if we're holding down the jump key set gravity to a small number so we go higher.
        if (PlayerData.JumpHeightTimer > 0)
        {
            PlayerData.JumpHeightTimer -= Time.deltaTime;
            if (PlayerData.JumpHeightTimer < 0)
            {
                PlayerData.JumpHeightTimer = 0;
            }
            if (PlayerControls.KeyJump)
            {
                PlayerData.RB2D.gravityScale = PlayerData.UpGravity;
            }
        }

    }

    public void JumpedAction()
    {
        PlayerData.JumpHeightTimer = PlayerData.JumpHeightTimerValue;
        PlayerControls.KeyDownJump = false;
        PlayerData.HasJumped = true;
        PlayerData.RB2D.velocity = new Vector2(PlayerData.RB2D.velocity.x, Vector2.up.y * PlayerData.JumpVelocity);
    }

}
