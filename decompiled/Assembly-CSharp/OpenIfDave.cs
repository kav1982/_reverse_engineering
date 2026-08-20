using UnityEngine;

public class OpenIfDave : MonoBehaviour
{
	public bool hideIfDave;

	public GameObject obj;

	private void Awake()
	{
		obj.SetActive(hideIfDave ? (!DataMgr.selectedWorldData.IsDave) : DataMgr.selectedWorldData.IsDave);
	}
}
