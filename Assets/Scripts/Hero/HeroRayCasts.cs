using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRayCasts : MonoBehaviour
{

    HeroData PlayerData;

    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GetComponentInChildren<HeroData>();
    }

    public void RayCasts()
    {
        //wall jump check
        PlayerData.OnWallLeft = WallAttachedCheckLeft();
        PlayerData.OnWallRight = WallAttachedCheckRight();
        // Check Ground
        PlayerData.OnGround = IsGrounded(PlayerData.GroundingHitBox);
        PlayerData.BufferGround = IsGrounded(PlayerData.BufferGroundHitBox);
    }

    private bool IsGrounded(float HitBoxSize)
    {
        float ExtraHeightText = (PlayerData.BC2D.bounds.extents.y * 2) * HitBoxSize;
        RaycastHit2D Hit = Physics2D.BoxCast(PlayerData.BC2D.bounds.center, PlayerData.BC2D.bounds.size, 0f, Vector2.down, ExtraHeightText, PlayerData.LayerMask);
        Color RayColour;
        if (Hit.collider != null)
        {
            RayColour = Color.green;
        }
        else
        {
            RayColour = Color.red;
        }

        Debug.DrawRay(PlayerData.BC2D.bounds.center + new Vector3(PlayerData.BC2D.bounds.extents.x, 0), Vector2.down * (PlayerData.BC2D.bounds.extents.y + ExtraHeightText), RayColour);
        Debug.DrawRay(PlayerData.BC2D.bounds.center - new Vector3(PlayerData.BC2D.bounds.extents.x, 0), Vector2.down * (PlayerData.BC2D.bounds.extents.y + ExtraHeightText), RayColour);
        Debug.DrawRay(PlayerData.BC2D.bounds.center - new Vector3(PlayerData.BC2D.bounds.extents.x, PlayerData.BC2D.bounds.extents.y + ExtraHeightText), Vector2.right * (PlayerData.BC2D.bounds.extents.x * 2), RayColour);


        return Hit.collider != null;
    }
    private bool WallAttachedCheckLeft()
    {
        float ExtraHeightText = (PlayerData.BC2D.bounds.extents.x * 2) * 0.01f;
        RaycastHit2D Hit = Physics2D.BoxCast(PlayerData.BC2D.bounds.center, new Vector2(PlayerData.BC2D.bounds.size.x, PlayerData.BC2D.bounds.size.y / PlayerData.WallCheckHitBox), 0f, Vector2.left, ExtraHeightText, PlayerData.LayerMask);
        Color RayColour;
        if (Hit.collider != null)
        {
            RayColour = Color.green;
        }
        else
        {
            RayColour = Color.blue;
        }

        Debug.DrawRay(PlayerData.BC2D.bounds.center + new Vector3((PlayerData.BC2D.bounds.extents.x) * -1, PlayerData.BC2D.bounds.extents.y / PlayerData.WallCheckHitBox), Vector2.left * new Vector2(ExtraHeightText, 0), RayColour);
        Debug.DrawRay(PlayerData.BC2D.bounds.center - new Vector3(PlayerData.BC2D.bounds.extents.x, PlayerData.BC2D.bounds.extents.y / 6), Vector2.left * (ExtraHeightText), RayColour);
        // vertical line
        Debug.DrawRay(PlayerData.BC2D.bounds.center + new Vector3((PlayerData.BC2D.bounds.extents.x + ExtraHeightText) * -1, PlayerData.BC2D.bounds.extents.y / PlayerData.WallCheckHitBox), Vector2.down * new Vector2(PlayerData.BC2D.bounds.extents.x, PlayerData.BC2D.bounds.extents.y / PlayerData.WallCheckHitBox) * 2, RayColour);


        return Hit.collider != null;
    }
    private bool WallAttachedCheckRight()
    {
        float ExtraHeightText = (PlayerData.BC2D.bounds.extents.x * 2) * 0.01f;
        RaycastHit2D Hit = Physics2D.BoxCast(PlayerData.BC2D.bounds.center, new Vector2(PlayerData.BC2D.bounds.size.x, PlayerData.BC2D.bounds.size.y / PlayerData.WallCheckHitBox), 0f, Vector2.right, ExtraHeightText, PlayerData.LayerMask);
        Color RayColour;
        if (Hit.collider != null)
        {
            RayColour = Color.green;
        }
        else
        {
            RayColour = Color.blue;
        }

        Debug.DrawRay(PlayerData.BC2D.bounds.center + new Vector3((PlayerData.BC2D.bounds.extents.x), PlayerData.BC2D.bounds.extents.y / PlayerData.WallCheckHitBox), Vector2.right * new Vector2(ExtraHeightText, 0), RayColour);
        Debug.DrawRay(PlayerData.BC2D.bounds.center + new Vector3(PlayerData.BC2D.bounds.extents.x, (PlayerData.BC2D.bounds.extents.y * -1) / PlayerData.WallCheckHitBox), Vector2.right * (ExtraHeightText), RayColour);
        // vertical line
        Debug.DrawRay(PlayerData.BC2D.bounds.center + new Vector3((PlayerData.BC2D.bounds.extents.x + ExtraHeightText), PlayerData.BC2D.bounds.extents.y / PlayerData.WallCheckHitBox), Vector2.down * new Vector2(PlayerData.BC2D.bounds.extents.x, PlayerData.BC2D.bounds.extents.y / PlayerData.WallCheckHitBox) * 2, RayColour);


        return Hit.collider != null;
    }
}
