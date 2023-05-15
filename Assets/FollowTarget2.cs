using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget2 : MonoBehaviour
{
    public float speed = 5f; // velocidad base del objeto
    public float acceleration = 2f; // aceleraci�n del objeto
    public float maxSpeed = 10f; // velocidad m�xima del objeto
    public float deceleration = 0.5f; // velocidad de deceleraci�n del objeto cuando el mouse se mueve m�s all� del objeto

    private Vector3 targetPosition; // posici�n objetivo del objeto

    private void Update()
    {
        // obtener la posici�n del mouse en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // mantener la misma posici�n z que el objeto

        // calcular la distancia entre el objeto y la posici�n del mouse
        float distance = Vector3.Distance(transform.position, mousePosition);

        // calcular la velocidad actual del objeto
        float currentSpeed = speed + acceleration * distance;

        // limitar la velocidad m�xima del objeto
        currentSpeed = Mathf.Clamp(currentSpeed, speed, maxSpeed);

        // mover el objeto hacia la posici�n del mouse con velocidad acelerada
        transform.position = Vector3.MoveTowards(transform.position, mousePosition, currentSpeed * Time.deltaTime);

        // calcular la direcci�n del movimiento del objeto
        Vector3 moveDirection = transform.position - targetPosition;

        // si el mouse se ha movido m�s all� del objeto, aplicar una fuerza de deceleraci�n
        if (moveDirection.magnitude > distance)
        {
            float decelerationSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, decelerationSpeed * Time.deltaTime);
        }
        else
        {
            // actualizar la posici�n objetivo del objeto
            targetPosition = transform.position;
        }
    }
}
