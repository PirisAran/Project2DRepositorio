using System;

public class DifficultyManager
{
	public static Action OnDifficultyChanged;

	public static DifficultyLevels _currentDifficultyLevel = DifficultyLevels.Normal;
	public enum DifficultyLevels
    {
		Normal,
		Hard
    }

	public static void SetDifficultyLevel(DifficultyLevels newDifficulty)
    {
		_currentDifficultyLevel = newDifficulty;
		OnDifficultyChanged?.Invoke();
    }
}
