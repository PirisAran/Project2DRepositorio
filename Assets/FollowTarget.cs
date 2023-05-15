using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public float speed = 5f; // velocidad base del objeto
    public float acceleration = 2f; // aceleraci�n del objeto
    public float maxSpeed = 10f; // velocidad m�xima del objeto
    public float currentSpeed;

    [SerializeField]
    private Transform _circle;

    private Vector3 targetPosition; // posici�n objetivo del objeto
    private Vector3 dir => (targetPosition - transform.position).normalized;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (transform.position + dir * currentSpeed));

    }

    private void Update()
    {
        targetPosition = _circle.position;
        // obtener la posici�n del mouse en el mundo

        // calcular la distancia entre el objeto y la posici�n del mouse
        float distance = Vector3.Distance(transform.position, targetPosition);

        // calcular la velocidad actual del objeto
        currentSpeed = speed + acceleration * distance;

        // limitar la velocidad m�xima del objeto
        currentSpeed = Mathf.Clamp(currentSpeed, speed, maxSpeed);

        // mover el objeto hacia la posici�n del mouse con velocidad acelerada
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }
}
