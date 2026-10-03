using UnityEngine;

public class HazardZone : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // O pon 99 si quieres que muera instantáneamente

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Busca el componente de vida en el jugador
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
