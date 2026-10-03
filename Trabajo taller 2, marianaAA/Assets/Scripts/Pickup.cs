using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController jugador = other.GetComponent<PlayerController>();
        if (jugador == null) return;

        AplicarEfecto(jugador);
        Destroy(gameObject);
    }

    protected abstract void AplicarEfecto(PlayerController jugador);
}
