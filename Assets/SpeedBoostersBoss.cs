using UnityEngine;

public class SpeedBoostersBoss: MonoBehaviour
{
    public Vector3 Dir => _dir;
    Vector3 _dir;

    public float BoostSpeed => _boostSpeed;
    [SerializeField] float _boostSpeed;
    private bool _inTrigger;

    private void OnDrawGizmos()
    {
        if (!_inTrigger) return;

        Debug.Log("gizmos");
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(transform.position, 5100);
    }

    private void Awake()
    {
        _dir = transform.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BossBehaviour>())
        {
            BossBehaviour.SetCurrentBoost(this);
            _inTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BossBehaviour>())
        {
            BossBehaviour.SetCurrentBoost(null);
            _inTrigger = false;
        }

    }
}
