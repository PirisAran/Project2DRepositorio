using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UmbraAnimator : MonoBehaviour
{
    [SerializeField]
    Sprite CuteSprite;
    [SerializeField]
    Sprite ChillSprite;
    [SerializeField]
    Sprite ChasingSprite;

    UmbraFSM _umbraFSM;
    SpriteRenderer _spriteRenderer;

    [SerializeField]
    List<Transform> EyeLights;

    private void Awake()
    {
        _umbraFSM = GetComponent<UmbraFSM>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _umbraFSM.OnCuteState += OnCuteState;
        _umbraFSM.OnChillState += OnChillState;
        _umbraFSM.OnChasingState += OnChasingState;
    }

    private void OnDisable()
    {
        _umbraFSM.OnCuteState -= OnCuteState;
        _umbraFSM.OnChillState -= OnChillState;
        _umbraFSM.OnChasingState -= OnChasingState;
    }

    private void Update()
    {
        _spriteRenderer.flipX = _umbraFSM.PlayerOrientation.x < 0;

        foreach (Transform light in EyeLights)
        {
            light.localPosition = new Vector2(Mathf.Abs(light.localPosition.x) * (_spriteRenderer.flipX ? -1 : 1), light.localPosition.y);
        }
    }

    void ChangeSprite(Sprite nextSprite)
    {
        _spriteRenderer.sprite = nextSprite;
    }

    void OnCuteState()
    {
        ChangeSprite(CuteSprite);
    }

    void OnChillState()
    {
        ChangeSprite(ChillSprite);
    }

    void OnChasingState()
    {
        ChangeSprite(ChasingSprite);
    }
}
