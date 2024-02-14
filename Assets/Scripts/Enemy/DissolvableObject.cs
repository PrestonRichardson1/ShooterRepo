using UnityEngine;

public class DissolvableObject : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    //public GameObject dissolveEffectPrefab;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Instantiate dissolve effect
        //Instantiate(dissolveEffectPrefab, transform.position, transform.rotation);

        // Destroy the object
        Destroy(gameObject);
    }
}
