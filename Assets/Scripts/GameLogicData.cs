using System.Collections.Generic;
using UnityEngine;

namespace TecnocampusProjectII
{
	[CreateAssetMenu(fileName = "GameLogicData", menuName = "TecnocampusProjectII/GameLogicData", order = 1)]
	public class GameLogicData : ScriptableObject
	{
		//GameData info
		public List<string> m_Levels;
	}
}
