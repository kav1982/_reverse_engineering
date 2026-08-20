using UnityEngine;

public class GOHarmonyKeep : MonoBehaviour
{
	public GameObject go_Normal;

	public GameObject go_H;

	private void Start()
	{
		if (GameMgr.IsHarmony_Static)
		{
			Object.Destroy(go_Normal);
		}
		else
		{
			Object.Destroy(go_H);
		}
	}
}
