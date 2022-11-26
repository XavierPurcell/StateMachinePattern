using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroOnWallStateManager : MonoBehaviour
{
    HeroData PlayerData;
    HeroControls PlayerControls;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GetComponentInParent<HeroData>();
        PlayerControls = GetComponentInParent<HeroControls>();
    }

    public void WallActions(Vector2 Direction)
    {
        if (!PlayerData.OnWallLeft && !PlayerData.OnWallRight)
        {
            PlayerData.MountedOnWall = false;
            return;
        }

        if (PlayerData.OnWallLeft && !PlayerControls.WalkLeft)
        {
            PlayerData.MountedOnWall = false;
            return;
        }

        if (PlayerData.OnWallRight && !PlayerControls.WalkRight)
        {
            PlayerData.MountedOnWall = false;
            return;
        }

        if (PlayerControls.KeyDownJump)
        {
            WallJump(Direction);
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
