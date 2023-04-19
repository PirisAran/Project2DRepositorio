using UnityEngine;

public class SymmetricalMovement : MonoBehaviour
{

    public Vector3 startingPoint;  // Punto de partida relativo al cual se aplica el movimiento simetrico

    public Transform parentObject;  

    private Vector3 originalOffset;  

    void Start()
    {
        // calculo del vector de desplazamiento original al padre
        originalOffset = transform.position - startingPoint;
    }

    void Update()
    {
        //vector de desplazamiento del objeto padre en este frame
        Vector3 parentOffset = parentObject.position - startingPoint;

        //vector simetrico de desplazamiento para el hijo
        Vector3 childOffset = new Vector3(-parentOffset.x, -parentOffset.y, -parentOffset.z);

        //se aplica simetria al hijo
        transform.position = startingPoint + childOffset + originalOffset;
    }
}
