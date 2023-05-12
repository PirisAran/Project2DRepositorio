using UnityEngine;

public class ParticlePositionUpdater : MonoBehaviour
{
    public Transform parentTransform;
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        // Obtiene la posición actual del objeto padre
        Vector3 parentPos = parentTransform.position;

        // Calcula la posición simétrica en el eje Y para el objeto hijo
        Vector3 symPos = new Vector3(parentPos.x, -parentPos.y, parentPos.z);

        // Actualiza la posición del objeto hijo
        ps.transform.position = symPos;
    }
}
