using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private float initialZ; // Almacenar la posición Z inicial de la cámara

    // Definir los límites de la cámara
    public float leftLimit = -10f;
    public float rightLimit = 10f;

    void Start()
    {
        // Guardar la posición Z inicial de la cámara para mantenerla constante
        initialZ = transform.position.z;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        // Asegurar que la posición Z de la cámara no cambie
        desiredPosition.z = initialZ;

        // Aplicar límites a la posición deseada
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, leftLimit, rightLimit);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}