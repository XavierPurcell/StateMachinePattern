using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKeys : MonoBehaviour
{

    HeroData PlayerData;
    public bool Walk = false;
    public bool WalkLeft = false;
    public bool WalkRight = false;
    public bool KeyDownJump = false;
    public bool KeyJump = false;
    public bool KeyUpJump = false;
    public bool KeyDownDash = false;

    public bool KeyUpLeft = false;
    public bool KeyUpRight = false;

    // log all key presses

    // keypresseddown
    public bool KeyAInitialDown = false;
    public bool KeyDInitialDown = false;
    public bool KeyWInitialDown = false;
    public bool KeySInitialDown = false;
    //key currently down
    public bool KeyADown = false;
    public bool KeyDDown = false;
    public bool KeyWDown = false;
    public bool KeySDown = false;
    //key up
    public bool KeyAUp = false;
    public bool KeyDUp = false;
    public bool KeyWUp = false;
    public bool KeySUp = false;








    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GetComponentInChildren<HeroData>();
    }

    public void CheckInput()
    {
        // if key is currently down
        KeyADown = Input.GetKey(KeyCode.A);
        KeyDDown = Input.GetKey(KeyCode.D);
        KeySDown = Input.GetKey(KeyCode.S);
        KeyWDown = Input.GetKey(KeyCode.W);


        // get key down
        if (!KeyAInitialDown)
        {
            KeyAInitialDown = Input.GetKeyDown(KeyCode.A);
        }

        if (!KeyDInitialDown)
        {
            KeyDInitialDown = Input.GetKeyDown(KeyCode.D);
        }

        if (!KeySInitialDown)
        {
            KeySInitialDown = Input.GetKeyDown(KeyCode.S);
        }

        if (!KeyWInitialDown)
        {
            KeyWInitialDown = Input.GetKeyDown(KeyCode.W);
        }

        
        // let go of keys
        if (!KeyAUp)
        {
            KeyAUp = Input.GetKeyUp(KeyCode.A);
        }

        if (!KeyDUp)
        {
            KeyDUp = Input.GetKeyUp(KeyCode.D);
        }

        if (!KeySUp)
        {
            KeySUp = Input.GetKeyUp(KeyCode.S);
        }

        if (!KeyWUp)
        {
            KeyWUp = Input.GetKeyUp(KeyCode.W);
        }


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
