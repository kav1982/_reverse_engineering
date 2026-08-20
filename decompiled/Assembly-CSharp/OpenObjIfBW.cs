using UnityEngine;

public class OpenObjIfBW : MonoBehaviour
{
	public bool hideIfBW;

	public GameObject obj;

	private void Awake()
	{
		obj.SetActive(hideIfBW ? (!ScriptableObjMgr.Inst.testCtrller.isBW) : ScriptableObjMgr.Inst.testCtrller.isBW);
	}
}
