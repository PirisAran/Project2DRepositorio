using UnityEngine;

public class SpeedBoostersBoss: MonoBehaviour
{
    Vector3 _dir;

    [SerializeField] float _boostSpeed;

    Vector2 _velocityValue;

    private void OnDrawGizmos()
    {
    }

    private void Awake()
    {
        _dir = transform.right;

        GetComponent<SpriteRenderer>().enabled = false;

        _velocityValue = _boostSpeed * _dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BossBehaviour>())
        {
            BossBehaviour.ModifyVelocity(_velocityValue);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BossBehaviour>())
        {
            BossBehaviour.ModifyVelocity(-_velocityValue);
        }
    }
}
