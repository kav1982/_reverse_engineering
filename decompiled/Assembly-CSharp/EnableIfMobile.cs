using UnityEngine;

public class EnableIfMobile : MonoBehaviour
{
	public bool Enable;

	public GameObject obj;

	private void Start()
	{
		if (Enable)
		{
			obj.SetActive(GameMgr.IsMobile_Static);
		}
		else
		{
			obj.SetActive(!GameMgr.IsMobile_Static);
		}
	}
}
