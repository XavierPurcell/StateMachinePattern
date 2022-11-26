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


public class Hero1 : MonoBehaviour
{
    [SerializeField]
    LayerMask LayerMask;

    Rigidbody2D RB2D;
    BoxCollider2D BC2D;

    HeroControls Controls;

    HeroData PlayerData;

    bool Walk = false;
    bool WalkLeft = false;
    bool WalkRight = false;
    bool KeyUpLeft = false;
    bool KeyUpRight = false;
    bool KeyDownJump = false;
    bool KeyJump = false;
    bool KeyUpJump = false;

    [SerializeField]
    private float GroundingHitBox = 0.02f;
    [SerializeField]
    private float BufferGroundHitBox = 0.75f;
    [SerializeField]
    private float CoyoteValue = 0.15f;

    float CoyoteTimer = 0f;
    bool BufferGround;

    // walking stuff
    [Header("Walking")]
    [SerializeField]
    private float MaxSpeed = 20f;
    [SerializeField]
    private float InitialSpeed = 0f; // 1 = 100% 
    [SerializeField]
    private float Acceleration = 1f; // 1 = 1 seconds to accelerate 2 = half a second

    float xTimer = 0f;
    // Value gets updated after we add force to character.
    float UpdatedSpeed = 0;
    Vector2 LastDirection = new(0, 0);

    // jump stuff
    [Header("Jumping and Air Movement")]
    [SerializeField]
    private float JumpVelocity = 50f; // Characters jumping power
    [SerializeField]
    private float JumpHeightTimerValue = 0.3f; // This controls the hold down jump key to jump higher
    [SerializeField]
    private float AirInitialSpeed = 1f; // When we move in the air we apply a initial speed
    [SerializeField]
    private float AirMovementAccel = 1f; // Air acceleration
    [SerializeField]
    private float UpGravity = 7f; // characters gravity when they are jumping
    [SerializeField]
    private float SlowJumpGravity = 30f; // characters gravity when their ascent needs to slow to a stop
    [SerializeField]
    private float DownGravity = 12f; // characters gravity when falling
    [SerializeField]
    float FallMaxSpeed = 20f;   // Most games have a max fall speed. Sometimes its terminal velocity, but we want to control our characters gravity for a number of reasons. So we use a custom value instead of terminal velocity. 


    bool HasJumped = false;
    private float JumpHeightTimer = 0;
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
    float DecelTimer = 0;

    // wall jumps


    [Header("Wall Jump")]
    [SerializeField]
    float WallCheckHitBox = 6f;
    [SerializeField]
    private float WallSlideDownSpeed = -8f; // Characters jumping power
    [SerializeField]
    private float FallenOffWallTimerValue = 0.2f;

    float FallenOffWallTimer;
    bool FallenOffWall;
    bool MountedOnWall = false;

    // Tests and Debugging
    [Header("Tests")]
    [SerializeField]
    bool DisableTestUI = false;
    [SerializeField]
    Text Timer;
    [SerializeField]
    Text TimerSpeed;
    [SerializeField]
    Text TestState;
    [SerializeField]
    Text TestMethod;

    string Method = "";


    // state control
    //private static bool OnWallLeft = false;
    private static bool OnWallRight = false;
    private static bool OnGround = false;
    private static bool InWater = false;
    private static bool OnIce = false;
    string State = "";


    //animations
    private Animator mAnimator;
    bool Flipped = false;
    SpriteRenderer ThisSprite;
    // Start is called before the first frame update
    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();
        BC2D = GetComponent<BoxCollider2D>();
        mAnimator = GetComponent<Animator>();
        ThisSprite = GetComponent<SpriteRenderer>();
        float randomNumber = Random.Range(0, 2);

        if (randomNumber == 1)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = 300;
        }

        if (DisableTestUI)
        {
            Timer.enabled = false;
            TimerSpeed.enabled = false;
            TestState.enabled = false;
            TestMethod.enabled = false;
        }

        Controls = GetComponentInChildren<HeroControls>();
        PlayerData = GetComponentInChildren<HeroData>();

    }
    void Update()
    {
        // get keys
        CheckPlayerInput();
    
    }
    private void FixedUpdate()
    {
        RayCasts();
        FlipPlayer();
        StateController();
        MethodController();
        UpdateData();
        Tests();
    }

    private void UpdateData()
    {
        PlayerData.Velocity = RB2D.velocity;
        PlayerData.PlayerState = State;
        PlayerData.PlayerMethod = Method;
    }

    private void CheckPlayerInput()
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
        if (!KeyDownJump && ((BufferGround && RB2D.velocity.y < 0) || OnGround || CoyoteTimer > 0))
        {
            KeyDownJump = InputDownJump;
        }

        // if we let go of the jump key while in the air
        bool InputJumpUp = Input.GetKeyUp(KeyCode.W);
        if (!KeyUpJump && !OnGround)
        {
            KeyUpJump = InputJumpUp;
        }


        // Wall Jump
        if (!KeyDownJump && (PlayerData.OnWallLeft || OnWallRight) && !FallenOffWall)
        {
            KeyDownJump = InputDownJump;    
        }
    }
    private void RayCasts()
    {
        //wall jump
        PlayerData.OnWallLeft = WallAttachedCheckLeft();
        OnWallRight = WallAttachedCheckRight();
        // Check Ground
        OnGround = IsGrounded(GroundingHitBox);
        BufferGround = IsGrounded(BufferGroundHitBox);
    }
    private void FlipPlayer()
    {
        if (RB2D.velocity.x > 0.2)
        {
            Flipped = false;
        }
        else if (RB2D.velocity.x < -0.2)
        {
            Flipped = true;
        }

        ThisSprite.flipX = Flipped;
    }
    private void StateController()
    {
        // on the ground
        if (State != "OnGround" && OnGround == true)
        {
            InitialGroundState();
        }
        else if (State != "PlayerData.OnWallLeft" && PlayerData.OnWallLeft && WalkLeft && RB2D.velocity.y <= 0 && !OnGround)
        {
            InitialWallState("PlayerData.OnWallLeft");
        }
        else if (State != "OnWallRight" && OnWallRight && WalkRight && RB2D.velocity.y <= 0 && !OnGround)
        {
            InitialWallState("OnWallRight");
        }// in the air
        else if (State != "Air" && OnGround == false && !MountedOnWall)
        {
            InitialAirState();
        }
    }
    private void MethodController()
    {
        if (OnGround)
        {
            Method = "OnGroundMethod";
            GroundedActions();
        }
        else if (State == "PlayerData.OnWallLeft")
        {
            Method = "OnWallLeftMethod";
            WallActions(Vector2.right);

        }
        else if (State == "OnWallRight")
        {
            Method = "OnWallRightMethod";
            WallActions(Vector2.left);

        }
        else if (FallenOffWall)
        {
            Method = "FallenOffWallMethod";
            FallingOffWall();
        }
        else if (InWater)
        {

        }
        else if (OnIce)
        {

        }
        else // if all states are false we are in the air
        {
            Method = "AirMethod";
            AirActions();
        }
    }
    private bool IsGrounded(float HitBoxSize)
    {
        float ExtraHeightText = (BC2D.bounds.extents.y * 2) * HitBoxSize;
        RaycastHit2D Hit = Physics2D.BoxCast(BC2D.bounds.center, BC2D.bounds.size, 0f, Vector2.down, ExtraHeightText, LayerMask);
        Color RayColour;
        if (Hit.collider != null)
        {
            RayColour = Color.green;
        }
        else
        {
            RayColour = Color.red;
        }

        Debug.DrawRay(BC2D.bounds.center + new Vector3(BC2D.bounds.extents.x, 0), Vector2.down * (BC2D.bounds.extents.y + ExtraHeightText), RayColour);
        Debug.DrawRay(BC2D.bounds.center - new Vector3(BC2D.bounds.extents.x, 0), Vector2.down * (BC2D.bounds.extents.y + ExtraHeightText), RayColour);
        Debug.DrawRay(BC2D.bounds.center - new Vector3(BC2D.bounds.extents.x, BC2D.bounds.extents.y + ExtraHeightText), Vector2.right * (BC2D.bounds.extents.x * 2), RayColour);


        return Hit.collider != null;
    }
    private bool WallAttachedCheckLeft()
    {
        float ExtraHeightText = (BC2D.bounds.extents.x * 2) * 0.01f;
        RaycastHit2D Hit = Physics2D.BoxCast(BC2D.bounds.center, new Vector2(BC2D.bounds.size.x, BC2D.bounds.size.y / WallCheckHitBox), 0f, Vector2.left, ExtraHeightText, LayerMask);
        Color RayColour;
        if (Hit.collider != null)
        {
            RayColour = Color.green;
        }
        else
        {
            RayColour = Color.blue;
        }

        Debug.DrawRay(BC2D.bounds.center + new Vector3((BC2D.bounds.extents.x) * -1, BC2D.bounds.extents.y / WallCheckHitBox), Vector2.left * new Vector2(ExtraHeightText, 0), RayColour);
        Debug.DrawRay(BC2D.bounds.center - new Vector3(BC2D.bounds.extents.x, BC2D.bounds.extents.y / 6), Vector2.left * (ExtraHeightText), RayColour);
        // vertical line
        Debug.DrawRay(BC2D.bounds.center + new Vector3((BC2D.bounds.extents.x + ExtraHeightText) * -1, BC2D.bounds.extents.y / WallCheckHitBox), Vector2.down * new Vector2(BC2D.bounds.extents.x, BC2D.bounds.extents.y / WallCheckHitBox) * 2, RayColour);


        return Hit.collider != null;
    }
    private bool WallAttachedCheckRight()
    {
        float ExtraHeightText = (BC2D.bounds.extents.x * 2) * 0.01f;
        RaycastHit2D Hit = Physics2D.BoxCast(BC2D.bounds.center, new Vector2(BC2D.bounds.size.x, BC2D.bounds.size.y / WallCheckHitBox), 0f, Vector2.right, ExtraHeightText, LayerMask);
        Color RayColour;
        if (Hit.collider != null)
        {
            RayColour = Color.green;
        }
        else
        {
            RayColour = Color.blue;
        }

        Debug.DrawRay(BC2D.bounds.center + new Vector3((BC2D.bounds.extents.x), BC2D.bounds.extents.y / WallCheckHitBox), Vector2.right * new Vector2(ExtraHeightText, 0), RayColour);
        Debug.DrawRay(BC2D.bounds.center + new Vector3(BC2D.bounds.extents.x, (BC2D.bounds.extents.y * -1) / WallCheckHitBox), Vector2.right * (ExtraHeightText), RayColour);
        // vertical line
        Debug.DrawRay(BC2D.bounds.center + new Vector3((BC2D.bounds.extents.x + ExtraHeightText), BC2D.bounds.extents.y / WallCheckHitBox), Vector2.down * new Vector2(BC2D.bounds.extents.x, BC2D.bounds.extents.y / WallCheckHitBox) * 2, RayColour);


        return Hit.collider != null;
    }
    private void InitialGroundState()
    {
        State = "OnGround";
        //Ground Variables
        OnGroundMovementReset();
        // Wall/Jump Variables
        OffWallJumpVariablesReset();
        // Ground velocity set to how fast we were going in the air.
        RB2D.velocity = new Vector2(UpdatedSpeed, RB2D.velocity.y);

        mAnimator.SetBool("Jump", false);
        mAnimator.SetBool("Falling", false);
    }
    private void InitialWallState(string Direction)
    {
        State = Direction;
        OnGroundMovementReset();
        OnWallJumpVariablesReset();
        mAnimator.SetBool("WallGrab", true);
    }
    private void InitialAirState()
    {
        State = "Air";
        if (HasJumped == false) // if we're in the air and we havent HasJumped we have the option of coyote jumping
        {
            CoyoteTimer = CoyoteValue;
        }
        InAirVariableReset();
        mAnimator.SetBool("Jump", true);
        mAnimator.SetBool("Falling", false);
        mAnimator.SetBool("Run", false);
        mAnimator.SetBool("WallGrab", false);
    }
    private void GroundedActions()
    {
        if (KeyDownJump)
        {
            JumpedAction();
        }

        if (Walk == true)
        {
            mAnimator.SetBool("Run", true);
            if (WalkLeft)
            {

                if (SlideEnabled)
                {
                    SlideRun(Vector2.left);
                }
                else
                {
                    Run(Vector2.left);
                }

                LastDirection = Vector2.left;
            }
            if (WalkRight)
            {
                if (SlideEnabled)
                {
                    SlideRun(Vector2.right);
                }
                else
                {
                    Run(Vector2.right);
                }

                LastDirection = Vector2.right;
            }

        }
        else if (KeyUpLeft || KeyUpRight)
        {
            if (KeyUpLeft)
            {
                KeyUpLeft = false;
            }

            if (KeyUpRight)
            {
                KeyUpRight = false;
            }

            if (SlideEnabled)
            {
                Decel();
            }
            else
            {
                RB2D.velocity = new Vector2(0, RB2D.velocity.y);
                mAnimator.SetBool("Run", false);
            }
            xTimer = 0;
        }
        else
        {

            if (SlideEnabled)
            {
                Decel();
            }
            else
            {
                RB2D.velocity = new Vector2(0, RB2D.velocity.y);
                mAnimator.SetBool("Run", false);
            }

        }
    }
    private void AirActions()
    {
        if (Walk == true)
        {
            if (WalkLeft)
            {
                AirMovement(Vector2.left);
            }
            else if (WalkRight)
            {
                AirMovement(Vector2.right);
            }

        }
        else if (!FollowThroughAirMomentum)
        {
            RB2D.velocity = new Vector2(0, RB2D.velocity.y);
        }
        CoyoteJump();
        JumpGravity();

    }
    private void Tests()
    {
        if (!DisableTestUI)
        {
            TimerSpeed.text = RB2D.velocity.x.ToString();
            Timer.text = (xTimer).ToString();
            TestState.text = State;
            TestMethod.text = Method;
        }
    }
    private void UpdateNeededVelocityTimersAndValues()
    {
        DecelTimer = 0;
        UpdatedSpeed = RB2D.velocity.x;
    }
    private void Run(Vector2 Direction)
    {

        // animations
        //mAnimator.SetTrigger("Run");

        // change direction
        if (LastDirection != Direction)
        {
            xTimer = 0;
            RB2D.velocity = new Vector2(0, RB2D.velocity.y);
        }

        if (RB2D.velocity.x == 0)
        {
            xTimer = 0;
        }
        // set initial speed
        if (xTimer == 0)
        {
            float InitialForceToAdd = InitialSpeed - RB2D.velocity.x;

            if (RB2D.velocity.x < 0)
            {
                InitialForceToAdd = InitialSpeed + RB2D.velocity.x;
            }

            if (RB2D.velocity.x > InitialSpeed || RB2D.velocity.x < InitialSpeed * -1)
            {
                InitialForceToAdd = Acceleration;
            }

            RB2D.AddForce(new Vector2(InitialForceToAdd * Direction.x, 0), ForceMode2D.Impulse);
            xTimer += Time.deltaTime;

            if (xTimer > 1)
            {
                xTimer = 1;
            }
        }
        else
        {
            xTimer += Time.deltaTime;

            if (xTimer > 1)
            {
                xTimer = 1;
            }

            RB2D.AddForce(new Vector2(Acceleration * Direction.x, 0), ForceMode2D.Impulse);
        }


        float CheckSpeed = RB2D.velocity.x;

        if (CheckSpeed > MaxSpeed)
        {
            RB2D.velocity = new Vector2(MaxSpeed, RB2D.velocity.y);
        }
        else if (CheckSpeed < MaxSpeed * -1)
        {
            RB2D.velocity = new Vector2(MaxSpeed * -1, RB2D.velocity.y);
        }

        UpdateNeededVelocityTimersAndValues();


    }
    private void SlideRun(Vector2 Direction)
    {
        // turn right
        if (Direction.x > 0 && RB2D.velocity.x < 0)
        {
            RB2D.AddForce(new Vector2(DecelTurnSpeed * Direction.x, 0), ForceMode2D.Impulse);
        }
        //turn left
        else if (Direction.x < 0 && RB2D.velocity.x > 0)
        {
            RB2D.AddForce(new Vector2(DecelTurnSpeed * Direction.x, 0), ForceMode2D.Impulse);
        }// else apply normal force
        else
        {
            RB2D.AddForce(new Vector2(Acceleration * Direction.x, 0), ForceMode2D.Impulse);
        }


        // do we want initial speed? Doesn't really work with a full slide implementation. Need Partial Slide that is only enabled on turns
        /*
            float InitialForceToAdd = InitialSpeed - RB2D.velocity.x;

                    if (RB2D.velocity.x < 0)
                    {
                        InitialForceToAdd = InitialSpeed + RB2D.velocity.x;
                    }

                    if (InitialForceToAdd > InitialSpeed)
                    {
                        InitialForceToAdd = 0;
                    }

                    RB2D.AddForce(new Vector2(InitialForceToAdd * Direction.x, 0), ForceMode2D.Impulse);
        */

        float CheckSpeed = RB2D.velocity.x;

        if (CheckSpeed > MaxSpeed)
        {
            RB2D.velocity = new Vector2(MaxSpeed, RB2D.velocity.y);
        }
        else if (CheckSpeed < MaxSpeed * -1)
        {
            RB2D.velocity = new Vector2(MaxSpeed * -1, RB2D.velocity.y);
        }
        UpdateNeededVelocityTimersAndValues();
    }
    private void AirMovement(Vector2 Direction)
    {
        // set initial speed
        if (xTimer == 0)
        {
            RB2D.AddForce(new Vector2(AirInitialSpeed * Direction.x, 0), ForceMode2D.Impulse);
            xTimer += Time.deltaTime;

            if (xTimer > 1)
            {
                xTimer = 1;
            }
        }
        else
        {
            xTimer += Time.deltaTime;

            if (xTimer > 1)
            {
                xTimer = 1;
            }

            RB2D.AddForce(new Vector2(AirMovementAccel * Direction.x, 0), ForceMode2D.Impulse);
        }

        float CheckSpeed = RB2D.velocity.x;

        if (CheckSpeed > MaxSpeed)
        {
            RB2D.velocity = new Vector2(MaxSpeed, RB2D.velocity.y);
        }
        else if (CheckSpeed < MaxSpeed * -1)
        {
            RB2D.velocity = new Vector2(MaxSpeed * -1, RB2D.velocity.y);
        }

        UpdateNeededVelocityTimersAndValues();
    }
    private void JumpedAction()
    {
        JumpHeightTimer = JumpHeightTimerValue;
        KeyDownJump = false;
        HasJumped = true;
        RB2D.velocity = new Vector2(RB2D.velocity.x, Vector2.up.y * JumpVelocity);
    }
    private void JumpGravity()
    {
        // if we are falling set fall gravity
        if (RB2D.velocity.y < 0)
        {
            if (RB2D.velocity.y <= FallMaxSpeed * -1)
            {
                RB2D.velocity = new Vector2(RB2D.velocity.x, Vector2.down.y * FallMaxSpeed);
                RB2D.gravityScale = 0;
            }
            else
            {
                RB2D.gravityScale = DownGravity;
            }

            mAnimator.SetBool("Falling", true);
        }
        else
        {   // if we are ascending set jump gravity
            RB2D.gravityScale = SlowJumpGravity;
        }

        if (KeyUpJump) // if we let go of the jump key.
        {
            return;
        }

        // if we're holding down the jump key set gravity to a small number so we go higher.
        if (JumpHeightTimer > 0)
        {
            JumpHeightTimer -= Time.deltaTime;
            if (JumpHeightTimer < 0)
            {
                JumpHeightTimer = 0;
            }
            if (KeyJump)
            {
                RB2D.gravityScale = UpGravity;
            }
        }

    }
    private void CoyoteJump()
    {
        if (CoyoteTimer > 0)
        {
            CoyoteTimer -= Time.deltaTime;
            if (CoyoteTimer < 0)
            {
                CoyoteTimer = 0;
            }
            if (KeyDownJump)
            {
                CoyoteTimer = 0;
                InitialAirState();
                JumpedAction();
            }
        }
    }
    private void Decel()
    {
        if (RB2D.velocity.x == 0)
        {
            return;
        }

        DecelTimer += Time.deltaTime * Decceleration;

        if (DecelTimer > 1)
        {
            DecelTimer = 1;

        }

        float Speed = Mathf.Lerp(UpdatedSpeed, 0, DecelTimer);
        RB2D.velocity = new Vector2(Speed, RB2D.velocity.y);
    }
    private void WallActions(Vector2 Direction)
    {
        if (!PlayerData.OnWallLeft && !OnWallRight)
        {
            MountedOnWall = false;
            return;
        }

        if (PlayerData.OnWallLeft && !WalkLeft)
        {
            MountedOnWall = false;
            return;
        }

        if (OnWallRight && !WalkRight)
        {
            MountedOnWall = false;
            return;
        }

        if (KeyDownJump)
        {
            WallJump(Direction);
        }
        else
        {
            RB2D.gravityScale = 0;
            RB2D.velocity = new Vector2(0, WallSlideDownSpeed);
        }
      
    }
    private void WallJump(Vector2 Direction)
    {
        RB2D.gravityScale = UpGravity;
        RB2D.velocity = new Vector2(Direction.x * JumpVelocity/2, Vector2.up.y * JumpVelocity/1.5f);
        MountedOnWall = false;
        FallenOffWall = true;
        FallenOffWallTimer = FallenOffWallTimerValue;
    }
    private void OnGroundMovementReset()
    {
        // key up values set to default
        KeyUpLeft = false;
        KeyUpRight = false;
        // jump keys set to default values
        HasJumped = false;
        KeyUpJump = false;
        KeyUpLeft = false;
        KeyUpRight = false;
        KeyJump = false;
        KeyDownJump = false;

        // reset timers
        xTimer = 0;
        JumpHeightTimer = 0;
        CoyoteTimer = 0;
        DecelTimer = 0;
        RB2D.gravityScale = DownGravity;
    }
    private void OnWallJumpVariablesReset()
    {
        HasJumped = true;
        MountedOnWall = true;
        FallenOffWall = false;
    }
    private void OffWallJumpVariablesReset()
    {
        MountedOnWall = false;
        FallenOffWall = false;
    }
    private void InAirVariableReset()
    {
        xTimer = 0;
        DecelTimer = 0;
        RB2D.gravityScale = UpGravity;
    }
    private void FallingOffWall()
    {
        KeyDownJump = false;
        if (FallenOffWallTimer > 0)
        {
            FallenOffWallTimer -= Time.deltaTime;

            if (FallenOffWallTimer < 0)
            {
                FallenOffWallTimer = 0;
                FallenOffWall = false;
            }
        }
    }

}




// in order to add a new action we do the following

// In Fixed update we need two add two things

// an initialState check activation
// Add a place to call actions once we're on said state

// we need to create the intialstate method and all the variables it needs
// we need to create a  action method that does the doing

// we need to log key presses for our said doing method

// furthermore we may(defs will) need to add new variables to other methods such as reset WallJumps when we touch the ground.
