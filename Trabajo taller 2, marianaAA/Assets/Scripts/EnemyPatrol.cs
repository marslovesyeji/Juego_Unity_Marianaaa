using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Puntos de Patrullaje")]
    public Transform pointA;
    public Transform pointB;

    [Header("Parámetros de Movimiento")]
    public float speed = 2f;
    private Transform currentTarget;

    [Header("Componentes")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (pointB != null)
        {
            currentTarget = pointB;
        }
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        // Mueve al enemigo hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Si se está moviendo, activa la animación de caminar
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        // Al llegar a un punto, cambia de dirección
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (currentTarget == pointB)
            {
                currentTarget = pointA;
                Flip();
            }
            else
            {
                currentTarget = pointB;
                Flip();
            }
        }
    }

    void Flip()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        else
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca contra el Jugador, dispara la animación de ataque
        if (collision.gameObject.CompareTag("Player"))
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.15f);
            Gizmos.DrawSphere(pointB.position, 0.15f);
        }
    }
}