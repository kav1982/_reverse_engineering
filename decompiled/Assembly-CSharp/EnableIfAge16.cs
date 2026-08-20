using UnityEngine;

public class EnableIfAge16 : MonoBehaviour
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
			if (GameMgr.IsChAge14_Static)
			{
				obj.SetActive(value: true);
			}
			else
			{
				obj.SetActive(value: false);
			}
		}
		else if (GameMgr.IsChAge14_Static)
		{
			obj.SetActive(value: false);
		}
		else
		{
			obj.SetActive(value: true);
		}
	}
}
