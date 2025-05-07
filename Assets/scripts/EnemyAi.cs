using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public int damage = 10;

    public bool isDead = false; // ← Added flag

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private float lastAttackTime;
    private Vector3 originalScale;

    private bool isFacingLeft = true;
    private float nextFlipTime = 0f;
    private float flipDelay = 1f;

    private int currentAnimState = -1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (isDead || player == null) return; // ← Stop all behavior if dead

        float distance = Vector2.Distance(transform.position, player.position);
        bool isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        if (!isAttacking)
        {
            if (distance > attackRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
                SetAnimState(2); // Run
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                SetAnimState(0); // Idle

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    AttackPlayer();
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        HandleFlipWithDelay();
    }

    void SetAnimState(int state)
    {
        if (state == currentAnimState) return;
        currentAnimState = state;
        anim.SetInteger("AnimState", state);
    }

    void AttackPlayer()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Enemy attacks!");
    }

    void HandleFlipWithDelay()
    {
        if (Time.time < nextFlipTime) return;

        float xDiff = player.position.x - transform.position.x;

        if (xDiff < -0.1f && !isFacingLeft)
        {
            Flip(true);
            nextFlipTime = Time.time + flipDelay;
        }
        else if (xDiff > 0.1f && isFacingLeft)
        {
            Flip(false);
            nextFlipTime = Time.time + flipDelay;
        }
    }

    void Flip(bool faceLeft)
    {
        isFacingLeft = faceLeft;
        float newX = faceLeft ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x);
        transform.localScale = new Vector3(newX, originalScale.y, originalScale.z);
    }
}
