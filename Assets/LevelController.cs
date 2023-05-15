using UnityEngine;
using UnityEngine.SceneManagement;

namespace TecnocampusProjectII
{
	public class LevelController : MonoBehaviour
	{
		public string m_NextLevel;

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.N))
			{
				Debug.Log("load next level "+m_NextLevel);
				SceneManager.LoadSceneAsync(m_NextLevel);
			}
		}
	}
}
