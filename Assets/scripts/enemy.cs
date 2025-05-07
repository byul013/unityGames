using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private Animator anim;
    public int health = 100;
    public float deathDelay = 2f; // seconds to wait before destroying the object

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component not found on this enemy!");
        }
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hurt");
        health -= damage;
        Debug.Log("Enemy took " + damage + " damage. Remaining health: " + health);

        if (health <= 0)
        {
            anim.SetTrigger("Death");
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Disable AI behavior
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.isDead = true;
        }

        // Play animation, then destroy
        StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2f); // or however long the death animation is
        Destroy(gameObject);
    }
}
