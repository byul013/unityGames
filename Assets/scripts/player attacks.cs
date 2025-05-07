using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private int attackIndex = 0;
    private float lastAttackTime;
    public float comboResetTime = 1f;

    public AttackHitbox attackHitbox; // <-- reference to your attack hitbox script

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Reset combo if too much time has passed
        if (Time.time - lastAttackTime > comboResetTime)
        {
            attackIndex = 0;
        }

        // Attack input
        if (Input.GetKeyDown(KeyCode.K))
        {
            lastAttackTime = Time.time;

            switch (attackIndex)
            {
                case 0:
                    anim.SetTrigger("Attack1");
                    break;
                case 1:
                    anim.SetTrigger("Attack2");
                    break;
                case 2:
                    anim.SetTrigger("Attack3");
                    break;
            }

            // Perform the attack logic (damage enemies)
            if (attackHitbox != null)
            {
                attackHitbox.PerformAttack();
            }
            else
            {
                Debug.LogWarning("AttackHitbox not assigned in PlayerAttack script.");
            }

            attackIndex = (attackIndex + 1) % 3;
        }
    }
}