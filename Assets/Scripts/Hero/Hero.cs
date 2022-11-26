using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




// in order to add a method we do the following


// Add the variables that you need to PlayerData. 
// Add new Controls to HeroControls.
// Add new RayCasts to raycasts if needed.
//Add a new class that does the things you want to do. It should inheriet from a State. If you need a new State make one. The new State needs to inherit from BaseState.
// Add a new trigger to call your new class in HeroStateManager.


// EXAMPLE

// We Have basestate
// We have InAirState that inherits from basestate
//We have AirState that inherits from InAirState
// In our Animation controller we have a substate to represent InAirState with accompanying animations.



public class Hero : MonoBehaviour
{
    [Header("Layer For RayCasting")]
    [SerializeField]
    LayerMask LayerMask;

    Rigidbody2D RB2D;
    BoxCollider2D BC2D;

    [SerializeField]
    public List<GameObject> DashSFX = new();

    HeroKeys PlayerControls;
    HeroData PlayerData;
    HeroUI HeroUI;
    HeroRayCasts HeroRayCasts;
    HeroStateManager PlayerStateManager;

    public TrailRenderer TR;

    [SerializeField]
    private float GroundingHitBox = 0.02f;
    [SerializeField]
    private float BufferGroundHitBox = 0.75f;
    [SerializeField]
    private float CoyoteValue = 0.15f;

    // walking stuff
    // walking stuff
    [Header("Walking")]
    [SerializeField]
    public float MaxSpeed = 20f;
    [SerializeField]
    public float InitialSpeed = 0f; // 1 = 100% 
    [SerializeField]
    private float Acceleration = 1f; // 1 = 1 seconds to accelerate 2 = half a second

    // jump stuff
    [Header("Jumping and Air Movement")]
    [SerializeField]
    private float JumpVelocity = 50f; // Characters jumping power
    [SerializeField]
    private float JumpHeightTimerValue = 0.3f; // This controls the hold down jump key to jump higher
    [SerializeField]
    private float AirInitialSpeed = 1f; // When we move in the air we apply a initial speed
    [SerializeField]
    private float AirMovementAccel = 1f; // Air PlayerData.Acceleration
    [SerializeField]
    private float UpGravity = 7f; // characters gravity when they are jumping
    [SerializeField]
    private float SlowJumpGravity = 30f; // characters gravity when their ascent needs to slow to a stop
    [SerializeField]
    private float DownGravity = 12f; // characters gravity when falling
    [SerializeField]
    float FallMaxSpeed = 20f;   // Most games have a max fall speed. Sometimes its terminal velocity, but we want to control our characters gravity for a number of reasons. So we use a custom value instead of terminal velocity. 

    [Header("Sliding")]
    //sliding the character via its momentum
    [SerializeField]
    float Decceleration = 1f; // higher = faster stop
    [SerializeField]
    float DecelTurnSpeed = 2f; // higher = faster turn
    #pragma warning disable 0414
    [SerializeField]
    float StopSlowSlidingValue = 0f; // currently isnt used
    #pragma warning restore 0414
    [SerializeField]
    private bool FollowThroughAirMomentum = false; // True = super mario bros | false = Celeste.

    [SerializeField]
    bool SlideEnabled = true;

    // wall jumps
    [Header("Wall Jump")]
    [SerializeField]
    float WallCheckHitBox = 6f;
    [SerializeField]
    private float WallSlideDownSpeed = -8f; // Characters jumping power
    [SerializeField]
    private float JumpedOffWallTimerValue = 0.2f;

    // Start is called before the first frame update

    void Start()
    {
        // get components
        RB2D = GetComponent<Rigidbody2D>();
        BC2D = GetComponent<BoxCollider2D>();
        ThisSprite = GetComponent<SpriteRenderer>();
        PlayerControls = GetComponentInChildren<HeroKeys>();
        PlayerData = GetComponentInChildren<HeroData>();
        HeroUI = GetComponentInChildren<HeroUI>();
        HeroRayCasts = GetComponentInChildren<HeroRayCasts>();
        PlayerStateManager = new HeroStateManager();

        //assign things
        PlayerData.BC2D = BC2D;
        PlayerData.RB2D = RB2D;
        PlayerData.mAnimator = GetComponent<Animator>();
        TR = GetComponent<TrailRenderer>();
        PlayerData.TR = TR;


        //dash
        PlayerData.DashSFX = DashSFX;

        //
        UpdateData();

    }
    void Update()
    {
        // get keys
        //PlayerControls.CheckPlayerInput();
        PlayerControls.CheckInput();
    }
    private void FixedUpdate()
    {
        HeroRayCasts.RayCasts();
        FlipPlayer();
        PlayerStateManager.HeroStateController();
        PlayerStateManager.HeroSubStateControllerPhysics();
        UpdateData();
        HeroUI.Tests();


        // stateMachine();
        // check state
        // run state methods
    }
    private void UpdateData()
    {
        PlayerData.Velocity = PlayerData.RB2D.velocity;
        PlayerData.LayerMask = LayerMask;
        PlayerData.BufferGroundHitBox = BufferGroundHitBox;
        PlayerData.GroundingHitBox = GroundingHitBox;
        PlayerData.WallCheckHitBox = WallCheckHitBox;
        PlayerData.UpGravity = UpGravity;
        PlayerData.CoyoteValue = CoyoteValue;
        PlayerData.MaxSpeed = MaxSpeed;
        PlayerData.AirMovementAccel = AirMovementAccel;
        PlayerData.AirInitialSpeed = AirInitialSpeed;
        PlayerData.JumpHeightTimerValue = JumpHeightTimerValue;
        PlayerData.JumpVelocity = JumpVelocity;
        PlayerData.FallMaxSpeed = FallMaxSpeed;
        PlayerData.SlowJumpGravity = SlowJumpGravity;
        PlayerData.DownGravity = DownGravity;
        PlayerData.FollowThroughAirMomentum = FollowThroughAirMomentum;
        PlayerData.WallSlideDownSpeed = WallSlideDownSpeed;
        PlayerData.JumpedOffWallTimerValue = JumpedOffWallTimerValue;
        PlayerData.Decceleration = Decceleration;
        PlayerData.Acceleration = Acceleration;
        PlayerData.InitialSpeed = InitialSpeed;
        PlayerData.DecelTurnSpeed = DecelTurnSpeed;
        PlayerData.SlideEnabled = SlideEnabled;

        //Dashing
        PlayerData.CurrentSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        PlayerData.SpriteFlippedX = Flipped;
    }
 

    // dirty methods here

    //animations
    bool Flipped = false;
    SpriteRenderer ThisSprite;
    private void FlipPlayer()
    {

        if (PlayerData.RB2D.velocity.x > 0.2)
        {
            Flipped = false;
        }
        else if (PlayerData.RB2D.velocity.x < -0.2)
        {
            Flipped = true;
        }

        ThisSprite.flipX = Flipped;
    }

}


