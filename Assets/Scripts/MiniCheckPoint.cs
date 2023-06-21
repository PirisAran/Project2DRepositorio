using UnityEngine;

public class MiniCheckPoint: CheckPoint
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _emptySprite;
    private void Start()
    {
        if (DifficultyManager._currentDifficultyLevel == DifficultyManager.DifficultyLevels.Normal)
        {
            return;
        }

        gameObject.SetActive(false);
    }
    protected override void DoAnimation()
    {
        _spriteRenderer.sprite = _emptySprite;
    }
}
