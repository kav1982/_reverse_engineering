using UnityEngine;

public class HardFinishNPCLaser : MonoBehaviour
{
	public Transform tsf_Node1;

	public Transform tsf_Node2;

	public LineRenderer lr_Laser;

	public LineRenderer lr_Shadow;

	public float shadowOffsetY;

	private void OnEnable()
	{
		lr_Laser.SetPosition(0, tsf_Node1.position);
		lr_Laser.SetPosition(1, tsf_Node2.position);
		lr_Shadow.SetPosition(0, tsf_Node1.position + new Vector3(0f, shadowOffsetY, 0f));
		lr_Shadow.SetPosition(1, tsf_Node2.position + new Vector3(0f, shadowOffsetY, 0f));
	}
}
