using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControls : MonoBehaviour
{

    HeroData PlayerData;
    public bool Walk = false;
    public bool WalkLeft = false;
    public bool WalkRight = false;
    public bool KeyUpLeft = false;
    public bool KeyUpRight = false;
    public bool KeyDownJump = false;
    public bool KeyJump = false;
    public bool KeyUpJump = false;
    public bool KeyDownDash = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GetComponentInChildren<HeroData>();
    }
    public void CheckPlayerInput()
    {
        // walking stuff
        bool InputLeft = Input.GetKey(KeyCode.A);
        bool InputRight = Input.GetKey(KeyCode.D);
        bool InputLeftUp = Input.GetKeyUp(KeyCode.A);
        bool InputRightUp = Input.GetKeyUp(KeyCode.D);

        Walk = InputLeft || InputRight;
        WalkLeft = InputLeft && !InputRight;
        WalkRight = InputRight && !InputLeft;

        if (!KeyUpLeft)
        {
            KeyUpLeft = InputLeftUp;
        }

        if (!KeyUpRight)
        {
            KeyUpRight = InputRightUp;
        }
        // end of walking stuff

        // Jumping Off the Ground
        bool InputJump = Input.GetKey(KeyCode.W);
        KeyJump = InputJump;

        bool InputDownJump = Input.GetKeyDown(KeyCode.W);
        if (!KeyDownJump && ((PlayerData.BufferGround && PlayerData.Velocity.y < 0) || PlayerData.OnGround || PlayerData.CoyoteTimer > 0))
        {
            KeyDownJump = InputDownJump;
        }

        // if we let go of the jump key while in the air
        bool InputJumpUp = Input.GetKeyUp(KeyCode.W);
        if (!KeyUpJump && !PlayerData.OnGround)
        {
            KeyUpJump = InputJumpUp;
        }


        // Wall Jump
        if (!KeyDownJump && (PlayerData.OnWallLeft || PlayerData.OnWallRight) && !PlayerData.JumpedOffWall)
        {
            KeyDownJump = InputDownJump;
        }

        bool InputDashDown = Input.GetKeyDown(KeyCode.X);
        
        if (InputDashDown)
        {
            KeyDownDash = true;
        }

    }
}
