using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroJumpedOffWallStateManager : MonoBehaviour
{
    // Start is called before the first frame update
    HeroData PlayerData;
    HeroControls PlayerControls;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GetComponentInParent<HeroData>();
        PlayerControls = GetComponentInParent<HeroControls>();
    }

    public void JumpedOffWallMethod()
    {
        PlayerControls.KeyDownJump = false;
        if (PlayerData.JumpedOffWallTimer > 0)
        {
            PlayerData.JumpedOffWallTimer -= Time.deltaTime;

            if (PlayerData.JumpedOffWallTimer < 0)
            {
                PlayerData.JumpedOffWallTimer = 0;
                PlayerData.JumpedOffWall = false;
            }
        }
    }
}
