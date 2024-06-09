using OpenAI;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public ChatGPT chatGPTScript; // Referencia al script ChatGPT en el Canvas
    [SerializeField] private GameObject canvasObject; // Referencia al objeto Canvas
    [SerializeField] private GameObject sign; // Referencia al objeto Canvas sign
    public bool NPC2 = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto con el que colisionamos tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            chatGPTScript.SendReply(); // Llama al método SendReply en el script ChatGPT
            Debug.Log("Trigger with Player");

            // Activa el objeto Canvas
            if (canvasObject != null)
            {
                canvasObject.SetActive(true);
            }

            // Desactiva el Collider2D para evitar futuras colisiones/triggers
            GetComponent<Collider2D>().enabled = false;

            if (NPC2 == true)
            {
                sign.SetActive(true);
            }
        }


    }
}