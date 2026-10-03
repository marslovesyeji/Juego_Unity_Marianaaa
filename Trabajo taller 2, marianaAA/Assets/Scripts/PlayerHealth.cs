using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Salud")]
    public int maxHealth = 30; // Vida máxima fija en 30
    public int currentHealth;

    [Header("Fuerza del Empujón")]
    public float knockbackForceX = 7f;
    public float knockbackForceY = 4f;

    private Rigidbody2D rb;

    void Start()
    {
        // Al iniciar, la vida actual toma el valor máximo configurado (30)
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI(currentHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1); // O los puntos de daño que quieras que quite el enemigo

            if (rb != null)
            {
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(knockbackDirection.x * knockbackForceX, knockbackForceY), ForceMode2D.Impulse);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método opcional por si quieres curar al jugador más adelante respetando el límite de 30
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Nunca pasará de 30
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI(currentHealth);
        }
    }

    void Die()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver();
        }

        gameObject.SetActive(false);
    }
}