using System;

public class DifficultyManager
{
	public static Action OnDifficultyChanged;

	public static DifficultyLevels _currentDifficultyLevel = DifficultyLevels.Hard;
	public enum DifficultyLevels
    {
		Easy,
		Normal,
		Hard
    }

	public static void SetDifficultyLevel(DifficultyLevels newDifficulty)
    {
		_currentDifficultyLevel = newDifficulty;
		OnDifficultyChanged?.Invoke();
    }
}
