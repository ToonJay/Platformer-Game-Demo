using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [Header("Run")]
    [SerializeField] private float moveSpeed;
    [Header("Dash")]
    [SerializeField] float dashForce;
    [SerializeField] float dashTime;
    private float dashTimer;
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    private float jumpTimer;
    [Header("WallJump")]
    [SerializeField] float wallSlidingSpeed;
    [SerializeField] float xWallForce;
    [SerializeField] float yWallForce;
    [SerializeField] float wallJumpTime;
    private float wallJumpTimer;
    [Header("Hit")]
    [SerializeField] float invinTime;
    private float invinTimer;
    [SerializeField] float hitForceX;
    [SerializeField] float hitForceY;
    [SerializeField] float hitTime;
    private float hitTimer;
    [Header("Other")]
    [SerializeField] float levelLoadDelay = 1f;
    public ParticleSystem dashLeftParticles;
    public ParticleSystem dashRightParticles;

    //input Variables
    private float inputX;
    private float wallJumpInputX;

    //Components
    private Rigidbody2D myRB;
    private BoxCollider2D myFeet;
    private PolygonCollider2D myBody;
    private Animator myAnim;
    private SpriteRenderer mySpriRen;
    private AudioPlayer audioPlayer;
    
    //states
    private bool isGrounded;
    private bool isJumping;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isAlive = true;

    
    void Start() {
        myRB = GetComponent<Rigidbody2D>();
        myFeet = GetComponent<BoxCollider2D>();
        myBody = GetComponent<PolygonCollider2D>();
        myAnim = GetComponent<Animator>();
        mySpriRen = GetComponent<SpriteRenderer>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void FixedUpdate() {
        PlayerDamage();
        if (!isAlive) {
            return;
        }
        if (hitTimer <= 0) {
            Movement();
        }
        Animation();
    }
    public void Movement () {
        isGrounded = myFeet.IsTouchingLayers(LayerMask.GetMask("Platforms")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Death")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Spawners"));
        isTouchingWall = myBody.IsTouchingLayers(LayerMask.GetMask("Platforms")) || myBody.IsTouchingLayers(LayerMask.GetMask("Death")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Spawners"));
        isWallSliding = isTouchingWall && !isGrounded && inputX != 0;

        //Regular Movement (running)
        if (isWallSliding) {
            myRB.velocity = new Vector2(myRB.velocity.x, Mathf.Clamp(myRB.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        if (!isWallJumping && !isDashing) {
            myRB.velocity = new Vector2(inputX * moveSpeed, Mathf.Clamp(myRB.velocity.y, -jumpForce + 2.3544f, float.MaxValue));
        }
        //Regular Jumping
        if (isJumping && jumpTimer > 0) {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpForce);
            jumpTimer -= Time.deltaTime;
        } else if (isGrounded || isTouchingWall) {
            isJumping = false;
            jumpTimer = jumpTime;
        }
        //Wall Jumping
        if (!(isWallJumping && wallJumpTimer > 0)) {
            isWallJumping = false;
            wallJumpTimer = wallJumpTime;
        } else {
            myRB.velocity = new Vector2(xWallForce * wallJumpInputX, yWallForce);
            wallJumpTimer -= Time.deltaTime;
        }
        //Dashing
        if (isDashing && dashTimer > 0) {
            myRB.velocity = new Vector2(dashForce * transform.localScale.x, myRB.velocity.y);
            myRB.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            dashTimer -= Time.deltaTime;
            canDash = false;
            if (transform.localScale == Vector3.one) {
                var em = dashRightParticles.emission;
                em.enabled = true;
            } else {
                var em = dashLeftParticles.emission;
                em.enabled = true;
            }
        } else {
            SetDashToFalse();
        }
        if (isGrounded) {
            canDash = true;
        }
    }
    public void Animation () {
        //animation checks
        Flip();
        RunAnimation();
        JumpAnimation();
        DashAnimation();
    }
    public void Move(InputAction.CallbackContext context) {
        if (!isAlive) {
            return;
        }
        inputX = Mathf.Round(context.ReadValue<Vector2>().x);
    }
    public void Dash(InputAction.CallbackContext context) {
        if (!isAlive || hitTimer > 0) {
            return;
        }
        //As the button is pressed, if not dashing and can dash, initiate Dash
        if (!isDashing && canDash && context.started) {
            audioPlayer.PlayDashClip();
            isDashing = true;
            canDash = false;
        }
        //Release button to stop mid-dash
        if (isDashing && dashTimer > 0 && context.canceled) {
            SetDashToFalse();
        }
    }
    public void Jump(InputAction.CallbackContext context) {
        if (!isAlive || hitTimer > 0) {
            return;
        }
        //If grounded, and as the button is pressed, initiate Jump
        if (isGrounded && context.started) {
            audioPlayer.PlayJumpClip();
            isJumping = true;
            SetDashToFalse();
        }
        //If Wall Sliding, stop mid-dash (if dashing) and initiate Wall Jump
        //Also enables dashing
        if (isWallSliding && context.started) {
            audioPlayer.PlayJumpClip();
            SetDashToFalse();
            isWallJumping = true;
            canDash = true;
            wallJumpInputX = -inputX;
        }
        //Release button to stop mid-Jump (for regular and wall) 
        if (context.canceled && myRB.velocity.y > 0) {
            isJumping = false;
            isWallJumping = false;
            myRB.velocity = new Vector2(myRB.velocity.x, 0);
        }
        
    }
    void JumpAnimation() {
        myAnim.SetBool("isJumping",  (isDashing || myRB.velocity.y > 0) && !isGrounded);
        myAnim.SetBool("isFalling",  myRB.velocity.y < 0 && !isGrounded);
    }
    void RunAnimation() {
        myAnim.SetBool("isRunning", Mathf.Abs(myRB.velocity.x) > 0 && isGrounded);
    }

    void DashAnimation() {
        myAnim.SetBool("isDashing", isDashing);
    }
    void Flip() {
        if (!isDashing) {
            if (isWallJumping) {
                if (wallJumpInputX < 0f) {
                    transform.localScale = Vector3.one;
                }
                else if (wallJumpInputX > 0f) {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else {
                if (inputX > 0f) {
                    transform.localScale = Vector3.one;
                }
                else if (inputX < 0f) {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
        }
    }
    void SetDashToFalse() {
        isDashing = false;
        dashTimer = dashTime;
        myRB.constraints = ~RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        var em = dashRightParticles.emission;
        em.enabled = false;
        em = dashLeftParticles.emission;
        em.enabled = false;

    }
    void PlayerDamage() {
        invinTimer -= Time.deltaTime;
        hitTimer -= Time.deltaTime;
        if (myBody.IsTouchingLayers(LayerMask.GetMask("Death")) && invinTimer <= 0) {
            Die();
        }
        if (myBody.IsTouchingLayers(LayerMask.GetMask("Enemies")) && invinTimer <= 0) {
            TakeDamage();
        }
        if (invinTimer % 0.1 >= 0.05 && isAlive && hitTimer <= 0) {
            mySpriRen.enabled = false;
        }
        else {
            mySpriRen.enabled = true;
        }
        myAnim.SetBool("isHit", hitTimer > 0);
    }
    void TakeDamage() {
        audioPlayer.PlayHitClip();
        FindObjectOfType<GameSession>().ProcessPlayerDamage();
        invinTimer = invinTime;
        hitTimer = hitTime;
        if (FindObjectOfType<GameSession>().playerHealth <= 0) {
            Die();
        } else if (hitTimer > 0) {
            SetDashToFalse();
            canDash = true;
            isWallJumping = false;
            isJumping = false;
            if (isGrounded) {
                myRB.velocity = new Vector2(-hitForceX * transform.localScale.x, hitForceY);
            } else {
                myRB.velocity = new Vector2(-hitForceX * transform.localScale.x, myRB.velocity.y);
                myRB.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
    void Die () {
        audioPlayer.PlayDieClip();
        isAlive = false;
        myBody.enabled = false;
        myRB.constraints = RigidbodyConstraints2D.FreezeAll;
        myAnim.SetTrigger("Dying");
        StartCoroutine(Death());
    }
    IEnumerator Death() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Exit") {
            myBody.enabled = false;
        }
    }

}
