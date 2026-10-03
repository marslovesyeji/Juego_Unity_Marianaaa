using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private float multiplicadorVelocidad = 0.5f; // 0.5 = más lento, 1.5 = más rápido

    private float velocidadOriginal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController jugador = other.GetComponent<PlayerController>();
        if (jugador == null) return;

        velocidadOriginal = jugador.speed;
        jugador.speed = velocidadOriginal * multiplicadorVelocidad;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController jugador = other.GetComponent<PlayerController>();
        if (jugador == null) return;

        jugador.speed = velocidadOriginal;
    }
}