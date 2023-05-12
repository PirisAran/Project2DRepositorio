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
        // Obtiene la posici�n actual del objeto padre
        Vector3 parentPos = parentTransform.position;

        // Calcula la posici�n sim�trica en el eje Y para el objeto hijo
        Vector3 symPos = new Vector3(parentPos.x, -parentPos.y, parentPos.z);

        // Actualiza la posici�n del objeto hijo
        ps.transform.position = symPos;
    }
}
