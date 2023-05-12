using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        // Obtiene el movimiento del usuario en el eje X
        float horizontalInput = Input.GetAxis("Horizontal");

        // Mueve el objeto padre en el eje X
        transform.position += Vector3.right * horizontalInput * speed * Time.deltaTime;

        // Obtiene el movimiento del usuario en el eje y
        float verticalInput = Input.GetAxis("Vertical");

        // Mueve el objeto padre en el eje X
        transform.position += Vector3.up * verticalInput * speed * Time.deltaTime;
    }
}
