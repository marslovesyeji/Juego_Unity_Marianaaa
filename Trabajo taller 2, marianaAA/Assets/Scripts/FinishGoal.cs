using UnityEngine;

public class FinishGoal : MonoBehaviour
{
    private bool levelFinished = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta al personaje y asegura que la victoria solo se dispare una vez
        if (collision.CompareTag("Player") && !levelFinished)
        {
            levelFinished = true;

            // Muestra el panel de victoria a través del UIManager
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowVictory();
            }
        }
    }
}