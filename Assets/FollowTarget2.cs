using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget2 : MonoBehaviour
{
    public float speed = 5f; // velocidad base del objeto
    public float acceleration = 2f; // aceleración del objeto
    public float maxSpeed = 10f; // velocidad máxima del objeto
    public float deceleration = 0.5f; // velocidad de deceleración del objeto cuando el mouse se mueve más allá del objeto

    private Vector3 targetPosition; // posición objetivo del objeto

    private void Update()
    {
        // obtener la posición del mouse en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // mantener la misma posición z que el objeto

        // calcular la distancia entre el objeto y la posición del mouse
        float distance = Vector3.Distance(transform.position, mousePosition);

        // calcular la velocidad actual del objeto
        float currentSpeed = speed + acceleration * distance;

        // limitar la velocidad máxima del objeto
        currentSpeed = Mathf.Clamp(currentSpeed, speed, maxSpeed);

        // mover el objeto hacia la posición del mouse con velocidad acelerada
        transform.position = Vector3.MoveTowards(transform.position, mousePosition, currentSpeed * Time.deltaTime);

        // calcular la dirección del movimiento del objeto
        Vector3 moveDirection = transform.position - targetPosition;

        // si el mouse se ha movido más allá del objeto, aplicar una fuerza de deceleración
        if (moveDirection.magnitude > distance)
        {
            float decelerationSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, decelerationSpeed * Time.deltaTime);
        }
        else
        {
            // actualizar la posición objetivo del objeto
            targetPosition = transform.position;
        }
    }
}
