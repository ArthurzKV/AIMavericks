using UnityEngine;
using UnityEngine.SceneManagement; // Añade este espacio de nombres

public class Logic : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Animator animator; // Referencia al Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obtiene la referencia al Animator
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        rb.velocity = movement * speed;

        // Actualiza el estado del Animator basado en el movimiento
        if (movement.magnitude > 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Flip basado en la dirección del movimiento
        if (moveHorizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            Flip();
        }
    }

    void Update() // Método Update para verificar la entrada del jugador
    {
        if (Input.GetKeyDown(KeyCode.R)) // Verifica si se presionó la tecla R
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}