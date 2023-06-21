public class MiniCheckPoint: CheckPoint
{
    private void Start()
    {
        if (DifficultyManager._currentDifficultyLevel == DifficultyManager.DifficultyLevels.Easy)
        {
            return;
        }

        gameObject.SetActive(false);
    }
    protected override void DoAnimation()
    {
        //Nothing
    }
}
