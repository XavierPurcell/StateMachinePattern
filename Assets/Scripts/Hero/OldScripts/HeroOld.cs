using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Can create a lot of different Platformer games movement with the following code. Things we are missing/need to be worked on.

// Super Mario bros features
// On above X speed start sliding, seems to happen in super mario bros, they have a very very slight turn on full speed.
// Stop sliding on a certain Speed.

// Celeste features to add
// Add Dash feature see Celeste.
// Add Wall Dash -> doing now
// Add Climb Feature


public class HeroOld : MonoBehaviour
{
    [Header("Layer For RayCasting")]
    [SerializeField]
    LayerMask LayerMask;

    Rigidbody2D RB2D;
    BoxCollider2D BC2D;

    [SerializeField]
    public List<GameObject> DashSFX = new();

    HeroControls PlayerControls;
    HeroData PlayerData;
    HeroUI HeroUI;
    HeroRayCasts HeroRayCasts;
    HeroMethodManager PlayerMethodManager;
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

        float randomNumber = Random.Range(0, 2);

        if (randomNumber == 1)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = 300;
        }

        // get components
        RB2D = GetComponent<Rigidbody2D>();
        BC2D = GetComponent<BoxCollider2D>();
        ThisSprite = GetComponent<SpriteRenderer>();
        PlayerControls = GetComponentInChildren<HeroControls>();
        PlayerData = GetComponentInChildren<HeroData>();
        HeroUI = GetComponentInChildren<HeroUI>();
        HeroRayCasts = GetComponentInChildren<HeroRayCasts>();
        PlayerMethodManager = GetComponentInChildren<HeroMethodManager>();
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
        PlayerControls.CheckPlayerInput();
    }
    private void FixedUpdate()
    {
        HeroRayCasts.RayCasts();
        FlipPlayer();
        PlayerMethodManager.StateController();
        PlayerMethodManager.MethodController();
        UpdateData();
        HeroUI.Tests();

        //PlayerStateManager.HeroStateController();
        //PlayerStateManager.HeroSubStateControllerPhysics();
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




// in order to add a method we do the following


// Create New Class

// In Method Manager Import Said Class

// Add new State for said class that runs when appropriate

// Add Methods that get called when state gets updated

// we need to create the intialstate PlayerData.PlayerMethod and all the variables it needs
// we need to create a  action PlayerData.PlayerMethod that does the doing

// we need to log key presses for our said doing PlayerData.PlayerMethod

// furthermore we may(defs will) need to add new variables to other methods such as reset WallJumps when we touch the ground.
