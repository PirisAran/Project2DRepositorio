using TecnocampusProjectII;
using UnityEngine;
using UnityEngine.U2D;

public class FakeSwamp: MonoBehaviour
{
    [SerializeField] float _distanceOfActivation;
    PlayerController _player;

    FakeSwampsManager _manager;
    SpriteShapeRenderer _spriteShapeRenderer;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _distanceOfActivation);
        Gizmos.color = Color.white;
    }

    private void Awake()
    {
        _spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();
    }
    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        _manager = FakeSwampsManager.GetFakeSwampsManager();
        _manager.AddToFakeSwampsList(this);
    }

    public bool CanGetActive()
    {
        return Vector2.Distance(_player.transform.position, transform.position) < _distanceOfActivation;
    }

    private void Update()
    {
        if (CanGetActive())
        {
            _manager.SetCurrentSwamp(this);
        }
    }

    public void SetActiveFakeSwampRender(bool v)
    {
        _spriteShapeRenderer.enabled = v;
    }

}
