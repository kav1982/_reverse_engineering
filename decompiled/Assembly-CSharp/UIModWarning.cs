using UnityEngine;
using UnityEngine.SceneManagement;

public class UIModWarning : MonoBehaviour
{
	public GameObject textGo;

	private bool _hasBepInEx;

	private void Awake()
	{
		_hasBepInEx = ProgramInfo.CheckBepInEx();
		textGo.SetActive(_hasBepInEx);
	}

	private void Update()
	{
		if (_hasBepInEx)
		{
			string text = SceneManager.GetActiveScene().name.ToLower();
			bool active = text == "init" || text == "entry" || text == "mainmenu";
			textGo.SetActive(active);
		}
	}
}
