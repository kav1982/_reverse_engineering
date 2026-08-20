using UnityEngine;

public class UIDialogueBubble_Hard : MonoBehaviour
{
	public Canvas canvas;

	private Transform tsf_Speaker;

	private void Update()
	{
		base.transform.position = tsf_Speaker.position;
		base.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
	}

	public void Initialize(Transform tsf_Speaker)
	{
		this.tsf_Speaker = tsf_Speaker;
		canvas = GetComponent<Canvas>();
		canvas.worldCamera = CamController.Inst.cam_Main;
	}
}
