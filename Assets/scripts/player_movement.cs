using UnityEngine;

public class player_movement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;

    public float speed = 5f;
    public float jumpForce = 3f;
    public float attackMoveMultiplier = 0.3f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool grounded;
    private float horizontalInput;
    private bool jumpRequested;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Ground check
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Get input
        horizontalInput = Input.GetAxis("Horizontal");

        // Request jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumpRequested = true;
        }

        // Set animation parameters
        anim.SetInteger("AnimState", Mathf.Abs(horizontalInput) > 0.01f ? 1 : 0);
        anim.SetFloat("AirSpeedY", body.velocity.y);
        anim.SetBool("Grounded", grounded);

        // Flip sprite
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FixedUpdate()
    {
        bool isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        float currentSpeed = isAttacking ? speed * attackMoveMultiplier : speed;

        // Apply movement
        body.velocity = new Vector2(horizontalInput * currentSpeed, body.velocity.y);

        // Handle jump
        if (jumpRequested)
        {
            anim.SetTrigger("Jump");
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpRequested = false;
        }
    }
}
