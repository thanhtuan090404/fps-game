using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " còn " + health + " HP");

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " bị tiêu diệt!");
        Destroy(gameObject);
    }
}