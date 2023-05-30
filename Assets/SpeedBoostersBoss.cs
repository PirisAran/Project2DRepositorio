using UnityEngine;

public class SpeedBoostersBoss: MonoBehaviour
{
    public Vector3 Dir => _dir;
    Vector3 _dir;

    public float BoostSpeed => _boostSpeed;
    [SerializeField] float _boostSpeed;

    private void OnDrawGizmos()
    {
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
        }
    }
}
