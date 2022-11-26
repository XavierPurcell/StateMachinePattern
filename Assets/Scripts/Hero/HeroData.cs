using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroData : MonoBehaviour
{

    public Vector2 Velocity;
    public string PlayerState;
    public string PlayerSubState;
    public string ParentState;
    public bool PlayerStateLocked;
    public bool PlayerSubStateLocked;
    public string PlayerMethod;

    //
    public Sprite CurrentSprite;
    public bool SpriteFlippedX;
    public TrailRenderer TR;

    //states
    public bool OnWallLeft = false;
    public bool OnWallRight = false;
    public bool OnGround = false;
    public bool InWater = false;
    public bool OnIce = false;
    public bool MountedOnWall = false;

    // state specific variables
    public bool JumpedOffWall;
    public bool HasJumped = false;
    public float UpdatedSpeed = 0;
    public Vector2 LastDirection;
    //dash
    public bool CanDash = true;
    public bool CurrentlyDashing = false;
    public List<GameObject> DashSFX;
    public Sprite DashSprite;

    //timers
    public float JumpHeightTimer = 0;
    public float CoyoteTimer = 0f;
    public float JumpedOffWallTimer = 0f;
    public float MovementTimer = 0f;
    public float DecelTimer = 0f;
    public float JumpedOffWallTimerValue = 0f;

    //raycasts
    public bool BufferGround;

    // values to be updated
    //Colliders etc
    public LayerMask LayerMask;
    public Rigidbody2D RB2D;
    public BoxCollider2D BC2D;
    public float GroundingHitBox = 0.02f;
    public float BufferGroundHitBox = 0.75f;

    // walking
    public float MaxSpeed;
    public float Acceleration;
    public float InitialSpeed;
    //Jumping
    public float JumpHeightTimerValue;
    public float JumpVelocity;
    public float AirMovementAccel;
    public float AirInitialSpeed;
    public float SlowJumpGravity;
    public float FallMaxSpeed;
    public float DownGravity;
    public float CoyoteValue;
    public float UpGravity;
    //sliding
    public float Decceleration;
    public float DecelTurnSpeed;
    public bool FollowThroughAirMomentum;
    public bool SlideEnabled;
    //Wall Jump
    public float WallCheckHitBox;
    public float WallSlideDownSpeed;


    //animation holder
    public Animator mAnimator;


    // Drag this in
    public Hero Parent;

}
