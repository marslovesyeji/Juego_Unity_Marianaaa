using UnityEngine;
using Unity.Cinemachine; // Para Unity 6 / Cinemachine 3.x

public class ItemCollectible : MonoBehaviour
{
    public enum CollectibleType
    {
        DamageLight,   // Coleccionable Daño 1
        DamageHeavy,   // Coleccionable Daño 2
        HealLight,     // Coleccionable Vida 1
        HealHeavy      // Coleccionable Vida 2
    }

    [Header("Configuración del Coleccionable")]
    public CollectibleType type;

    [Header("Valores de Efecto")]
    [Tooltip("Cantidad de daño o cura")]
    public float amount = 1f;

    [Header("Cinemachine Impulse (Para Daño)")]
    public CinemachineImpulseSource impulseSource;
    public float impulseForceLight = 0.3f;
    public float impulseForceHeavy = 1.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                switch (type)
                {
                    case CollectibleType.DamageLight:
                        player.TakeDamage(amount);
                        TriggerCameraShake(impulseForceLight);
                        break;

                    case CollectibleType.DamageHeavy:
                        player.TakeDamage(amount);
                        TriggerCameraShake(impulseForceHeavy);
                        break;

                    case CollectibleType.HealLight:
                        player.Heal(amount);
                        break;

                    case CollectibleType.HealHeavy:
                        player.Heal(amount);
                        break;
                }
            }

            // Destruye el coleccionable tras ser recogido
            Destroy(gameObject);
        }
    }

    private void TriggerCameraShake(float force)
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulseWithVelocity(Random.insideUnitCircle.normalized * force);
        }
    }
}