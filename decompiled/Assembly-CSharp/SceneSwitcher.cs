using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			if (SceneManager.GetActiveScene().buildIndex == 0)
			{
				SceneManager.LoadScene(1);
			}
			else
			{
				SceneManager.LoadScene(0);
			}
		}
	}
}
