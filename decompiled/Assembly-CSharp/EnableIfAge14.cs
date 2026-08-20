using UnityEngine;

public class EnableIfAge14 : MonoBehaviour
{
	public bool Enable;

	public GameObject obj;

	private void Start()
	{
		if (obj == null)
		{
			obj = base.gameObject;
		}
		if (Enable)
		{
			if (GameMgr.IsHarmony_Static)
			{
				obj.SetActive(value: true);
			}
			else if (!GameMgr.IsHarmony_Static)
			{
				obj.SetActive(value: false);
			}
		}
		else if (GameMgr.IsHarmony_Static)
		{
			obj.SetActive(value: false);
		}
		else if (!GameMgr.IsHarmony_Static)
		{
			obj.SetActive(value: true);
		}
	}
}
