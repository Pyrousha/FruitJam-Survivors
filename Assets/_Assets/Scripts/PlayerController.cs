using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isGrappling;
    private float distToHook;

    private Vector2 dirFacing;
    public Vector2 DirFacing => dirFacing;

    private Vector2 dir;
    [SerializeField] private float startingGrapplingSpeed;
    [SerializeField] private float speedGainedPerSecGrappling;
    [SerializeField] private float grappleGravUp;
    [SerializeField] private float grappleGravDown;
    [SerializeField] private float grappleXInfluence;
    [Space(5)]
    [SerializeField] private float grappleDistChangePerSec;
    [Space(5)]
    [SerializeField] private float maxGrappleDist;
    private float currGrapplingSpeed;

    private PlayerAnimStateEnum currentAnimation;


    [Header("Self-References")]
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator anim;
    private InputHandler inputHandler;
    private Rigidbody2D rb;
    public Rigidbody2D RB => rb;

    [Header("External References")]
    [SerializeField] private GrappleHook grappleHook;

    //Animation states
    enum PlayerAnimStateEnum
    {
        Player_Idle,
        Player_Right
    }

    [Header("Parameters")]
    [SerializeField] private bool airControl;
    [SerializeField] private float accelSpeed_Ground;
    [SerializeField] private float accelSpeed_Air;
    [SerializeField] private float frictionSpeed_Ground;
    [SerializeField] private float frictionSpeed_Air;
    [SerializeField] private float maxWalkSpeed;
    [SerializeField] private float jumpPower;
    [Space(5)]
    [SerializeField] private float gravUp;
    [SerializeField] private float gravDown;
    [SerializeField] private float spaceReleaseGravMult;
    [Space(5)]
    [SerializeField] private LayerMask terrainLayer;

    //private float xSpeed = 0;
    private bool grounded = false;
    private float lastTimePressedJump = -100.0f;
    private float lastTimeGrounded = -100.0f;
    private const float epsilon = 0.05f;
    private const float jumpBuffer = 0.1f;
    private const float coyoteTime = 0.1f;

    void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            dirFacing = rb.velocity.normalized;
        }

        if (isGrappling)
        {
            //Make player the same distance from the hook while grappling
            transform.position = grappleHook.transform.position + (transform.position - grappleHook.transform.position).normalized * distToHook;

            //Axis perpendicular to the direction of player to grappling hook
            Vector2 axis = Vector2.Perpendicular(grappleHook.transform.position - transform.position);

            //Gravity on hook
            if (rb.velocity.y > 0)
                rb.velocity += Vector2.down * grappleGravUp;
            else
                rb.velocity += Vector2.down * grappleGravDown;

            //A/D influence
            rb.velocity += Vector2.right * inputHandler.Direction.x * grappleXInfluence;

            rb.velocity = Vector3.Project(rb.velocity, axis);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrappling)
        {
            currGrapplingSpeed += speedGainedPerSecGrappling * Time.deltaTime;

            if (!grounded)
            {

                float inputY = inputHandler.Direction.y;

                if (inputY > 0)
                {
                    //Reel in
                    distToHook = Mathf.Max(0, distToHook - inputY * grappleDistChangePerSec * Time.deltaTime);
                }
                else
                {
                    if (inputY < 0)
                    {
                        //Reel out
                        if (distToHook < maxGrappleDist)
                        {
                            distToHook = Mathf.Min(maxGrappleDist, distToHook - inputY * grappleDistChangePerSec * Time.deltaTime);
                        }
                    }
                }
            }

        }

        if (inputHandler.Grapple.down && (isGrappling == false))
        {
            //Start Grapple
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector2 grappleDir = mousePos - transform.position;
            grappleHook.Fire(grappleDir);
        }

        if (inputHandler.Grapple.up && isGrappling)
        {
            //End Grapple
            grappleHook.Retract();
        }

        //Ground checking
        bool lastGrounded = grounded;
        grounded = (rb.velocity.y < 1.0f) && Physics2D.BoxCast(col.bounds.center, col.bounds.size * 0.99f, 0f, Vector2.down, 0.1f, terrainLayer);

        if (isGrappling == false)
        {
            //Accelerate + Friction
            float inputX = inputHandler.Direction.x;
            float xSpeed = rb.velocity.x;

            if (!grounded && !airControl)
                inputX = 0;

            if (inputX < -epsilon) //Pressing left
            {
                if (xSpeed > -maxWalkSpeed)
                {
                    //Can still accelerate to the left (but do not exceed max)
                    if (grounded)
                        xSpeed = Mathf.Max(-maxWalkSpeed, xSpeed + accelSpeed_Ground * inputX * Time.deltaTime);
                    else
                        xSpeed = Mathf.Max(-maxWalkSpeed, xSpeed + accelSpeed_Air * inputX * Time.deltaTime);
                }
            }
            else
            {
                if (inputX > epsilon) //Pressing right
                {
                    if (xSpeed < maxWalkSpeed)
                    {
                        //Can still accelerate to the right (but do not exceed max)
                        if (grounded)
                            xSpeed = Mathf.Min(maxWalkSpeed, xSpeed + accelSpeed_Ground * inputX * Time.deltaTime);
                        else
                            xSpeed = Mathf.Min(maxWalkSpeed, xSpeed + accelSpeed_Air * inputX * Time.deltaTime);
                    }
                }
                else //pressing nothing
                {
                    //Get sign of current speed
                    float sign = 1;
                    if (xSpeed < 0)
                        sign = -1;

                    //Not pressing anything, subtract friction and update speed
                    float newSpeedMagnitude;
                    if (grounded)
                        newSpeedMagnitude = Mathf.Max(0, Mathf.Abs(xSpeed) - frictionSpeed_Ground * Time.deltaTime);
                    else
                        newSpeedMagnitude = Mathf.Max(0, Mathf.Abs(xSpeed) - frictionSpeed_Air * Time.deltaTime);
                    xSpeed = newSpeedMagnitude * sign;
                }
            }

            //Set velocity
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        }


        //Get last time grounded
        if ((lastGrounded == true) && (grounded == false))
            lastTimeGrounded = Time.time;

        //Jump - grounded
        if (inputHandler.Jump.down)
        {
            if (grounded || (Time.time - lastTimeGrounded <= coyoteTime))
                TryJump();
            else
                lastTimePressedJump = Time.time;
        }
        //Jump - buffered
        if (grounded)
        {
            if (Time.time - lastTimePressedJump <= jumpBuffer)
                TryJump();
        }


        //Gravity
        if (isGrappling == false)
        {
            if (inputHandler.Jump.holding && rb.velocity.y > 0)
                rb.velocity -= new Vector2(0, gravUp * Time.deltaTime);
            else
                rb.velocity -= new Vector2(0, gravDown * Time.deltaTime);
        }

        //Space release gravity
        if (inputHandler.Jump.up && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * spaceReleaseGravMult);
        }


        //Sprite flipping
        if (rb.velocity.x < -epsilon)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            if (rb.velocity.x > epsilon)
            {
                spriteRenderer.flipX = false;
            }
        }


        //Set animation states
        if (grounded)
        {
            if (Mathf.Abs(rb.velocity.x) > 0.25f)
                ChangeAnimationState(PlayerAnimStateEnum.Player_Right);
            else
                ChangeAnimationState(PlayerAnimStateEnum.Player_Idle);
        }
        else
        {
            ChangeAnimationState(PlayerAnimStateEnum.Player_Idle);
            // if (rb.velocity.y >= 0)
            //     ChangeAnimationState(PlayerAnimStateEnum.Player_Jump_Up);
            // else
            //     ChangeAnimationState(PlayerAnimStateEnum.Player_Jump_Down);
        }
    }

    public void SetIsGrappling(bool _isGrappling)
    {
        isGrappling = _isGrappling;

        if (isGrappling) //start grapple
        {
            //Vector2 axis = Vector2.Perpendicular(grappleHook.transform.position - transform.position);
            currGrapplingSpeed = rb.velocity.magnitude;

            distToHook = (transform.position - grappleHook.transform.position).magnitude;
        }
        else //end grapple
        {

        }
    }


    private void TryJump()
    {
        if (isGrappling)
            return;

        rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        grounded = false;
    }

    private void ChangeAnimationState(PlayerAnimStateEnum _newState)
    {
        //Stop same animation from interrupting itself
        if (currentAnimation == _newState)
            return;

        //Play new animation
        anim.Play(_newState.ToString());

        //Update current anim state var
        currentAnimation = _newState;
    }

    // void OnCollisionEnter2D(Collision2D _col)
    // {
    //     if (isGrappling)
    //         rb.velocity *= -1;
    // }
}
