using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public float speed = 5f; // velocidad base del objeto
    public float acceleration = 2f; // aceleración del objeto
    public float maxSpeed = 10f; // velocidad máxima del objeto
    public float currentSpeed;

    [SerializeField]
    private Transform _circle;

    private Vector3 targetPosition; // posición objetivo del objeto
    private Vector3 dir => (targetPosition - transform.position).normalized;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (transform.position + dir * currentSpeed));

    }

    private void Update()
    {
        targetPosition = _circle.position;
        // obtener la posición del mouse en el mundo

        // calcular la distancia entre el objeto y la posición del mouse
        float distance = Vector3.Distance(transform.position, targetPosition);

        // calcular la velocidad actual del objeto
        currentSpeed = speed + acceleration * distance;

        // limitar la velocidad máxima del objeto
        currentSpeed = Mathf.Clamp(currentSpeed, speed, maxSpeed);

        // mover el objeto hacia la posición del mouse con velocidad acelerada
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }
}
