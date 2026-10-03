using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float direction;
    public float speed = 5f;
    public Rigidbody2D rb;

    [SerializeField] private float jumpForce = 7f;

    [Header("Detección de suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    public bool canJump;

    [Header("Vida")]
    public float health;
    [SerializeField] private float maxHealth = 3f; // Cambiado a 3 para coincidir con las vidas de la UI

    [Header("Knockback")]
    public float hitTime = 0.2f;
    public float hitForceX = 7f;
    public float hitForceY = 4f;
    public bool hitFromRight;
    private float hitTimer;
    private bool isHit;

    [Header("Animación")]
    [SerializeField] private Animator animator;
    public event Action<float, float> OnHealthChanged;

    void Start()
    {
        health = maxHealth;
        OnHealthChanged?.Invoke(health, maxHealth);

        // Actualizar la UI al iniciar
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI((int)health);
        }
    }

    void Update()
    {
        if (groundCheck != null)
        {
            canJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        if (isHit)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0f) isHit = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }

        UpdateAnimationAndFacing();
    }

    private void UpdateAnimationAndFacing()
    {
        if (!isHit && direction > 0.01f)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (!isHit && direction < -0.01f)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        if (animator != null)
            animator.SetBool("isRunning", !isHit && Mathf.Abs(direction) > 0.01f);
    }

    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && canJump && !isHit)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(float damage, Vector2 force, float time, bool fromRight)
    {
        hitFromRight = fromRight;
        hitForceX = force.x;
        hitForceY = force.y;
        hitTime = time;

        ChangeHealth(-damage);

        if (time > 0f)
        {
            float pushX = hitFromRight ? -hitForceX : hitForceX;
            rb.linearVelocity = new Vector2(pushX, hitForceY);
            isHit = true;
            hitTimer = hitTime;
        }

        if (animator != null) animator.SetTrigger("Hit");
    }

    public void TakeDamage(float damage)
    {
        ChangeHealth(-damage);
        if (animator != null) animator.SetTrigger("Hit");
    }

    public void Heal(float amount)
    {
        ChangeHealth(amount);
    }

    private void ChangeHealth(float amount)
    {
        health = Mathf.Clamp(health + amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(health, maxHealth);

        // 1. Notificar a la interfaz de usuario (UIManager)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI((int)health);
        }

        // 2. Verificar si el personaje murió
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Muestra la pantalla de Game Over desde el UIManager
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver();
        }
    }

    public void AumentarVelocidad(float cantidad)
    {
        speed += cantidad;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detección de impacto con el enemigo
        if (collision.gameObject.CompareTag("Enemy") && !isHit)
        {
            // Determina si el golpe viene desde la derecha
            bool fromRight = collision.transform.position.x > transform.position.x;

            // Usa tu sistema nativo de Knockback y Daño (Resta 1 de vida)
            TakeDamage(1f, new Vector2(hitForceX, hitForceY), hitTime, fromRight);
        }
    }
}