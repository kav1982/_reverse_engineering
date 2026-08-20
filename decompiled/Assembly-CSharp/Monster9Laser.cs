using UnityEngine;
using UnityEngine.VFX;

public class Monster9Laser : MonoBehaviour
{
	private enum LaserState
	{
		Stop,
		Warning,
		Attack
	}

	[Header("Warning")]
	public LineRenderer lr_Laser_Warning;

	public LineRenderer lr_LaserShadow_Warning;

	public Transform tsf_Node1_Warning;

	public Transform tsf_Node2_Warning;

	[Header("Attack")]
	public LineRenderer lr_Laser;

	public LineRenderer lr_LaserShadow;

	public Transform tsf_Node1;

	public Transform tsf_Node2;

	[Header("Bubble")]
	public VisualEffect ve_Bubble;

	public int bubbleCountPerMeter;

	private LaserState state;

	private void Update()
	{
		ve_Bubble.transform.position = Vector3.zero;
	}

	public void OnEnable()
	{
		ve_Bubble.gameObject.SetActive(value: false);
	}

	public void SetWarning(Vector3 point1, Vector3 point2)
	{
		if (state != LaserState.Warning)
		{
			state = LaserState.Warning;
			lr_Laser_Warning.gameObject.SetActive(value: true);
			lr_LaserShadow_Warning.gameObject.SetActive(value: true);
			tsf_Node1_Warning.gameObject.SetActive(value: true);
			tsf_Node2_Warning.gameObject.SetActive(value: true);
			lr_Laser.gameObject.SetActive(value: false);
			lr_LaserShadow.gameObject.SetActive(value: false);
			tsf_Node1.gameObject.SetActive(value: false);
			tsf_Node2.gameObject.SetActive(value: false);
		}
		lr_Laser_Warning.SetPosition(0, Tool2D.GetLayerPoint(point1));
		lr_Laser_Warning.SetPosition(1, Tool2D.GetLayerPoint(point2));
		lr_LaserShadow_Warning.SetPosition(0, Tool2D.IgnoreZPoint(point1, 1.05f));
		lr_LaserShadow_Warning.SetPosition(1, Tool2D.IgnoreZPoint(point2, 1.05f));
		tsf_Node1_Warning.position = Tool2D.GetLayerPoint(point1);
		tsf_Node2_Warning.position = Tool2D.GetLayerPoint(point2);
	}

	public void SetLaser(Vector3 point1, Vector3 point2)
	{
		if (state != LaserState.Attack)
		{
			state = LaserState.Attack;
			lr_Laser_Warning.gameObject.SetActive(value: false);
			lr_LaserShadow_Warning.gameObject.SetActive(value: false);
			tsf_Node1_Warning.gameObject.SetActive(value: false);
			tsf_Node2_Warning.gameObject.SetActive(value: false);
			lr_Laser.gameObject.SetActive(value: true);
			lr_LaserShadow.gameObject.SetActive(value: true);
			tsf_Node1.gameObject.SetActive(value: true);
			tsf_Node2.gameObject.SetActive(value: true);
		}
		lr_Laser.SetPosition(0, Tool2D.GetLayerPoint(point1));
		lr_Laser.SetPosition(1, Tool2D.GetLayerPoint(point2));
		lr_LaserShadow.SetPosition(0, Tool2D.IgnoreZPoint(point1, 1.05f));
		lr_LaserShadow.SetPosition(1, Tool2D.IgnoreZPoint(point2, 1.05f));
		tsf_Node1.position = Tool2D.GetLayerPoint(point1);
		tsf_Node2.position = Tool2D.GetLayerPoint(point2);
	}

	public void Stop()
	{
		lr_Laser_Warning.gameObject.SetActive(value: false);
		lr_LaserShadow_Warning.gameObject.SetActive(value: false);
		tsf_Node1_Warning.gameObject.SetActive(value: false);
		tsf_Node2_Warning.gameObject.SetActive(value: false);
		lr_Laser.gameObject.SetActive(value: false);
		lr_LaserShadow.gameObject.SetActive(value: false);
		tsf_Node1.gameObject.SetActive(value: false);
		tsf_Node2.gameObject.SetActive(value: false);
		ve_Bubble.gameObject.SetActive(value: false);
		if (state == LaserState.Attack && ve_Bubble != null)
		{
			ve_Bubble.SetFloat("Count", Tool2D.IgnoreZDistance(lr_Laser.GetPosition(0), lr_Laser.GetPosition(1)) * (float)bubbleCountPerMeter);
			ve_Bubble.SetVector3("Position0", lr_Laser.GetPosition(0));
			ve_Bubble.SetVector3("Position1", lr_Laser.GetPosition(1));
			if (!GameMgr.IsMobile_Static)
			{
				ve_Bubble.gameObject.SetActive(value: true);
			}
		}
		state = LaserState.Stop;
	}

	public void StopImmediately()
	{
		state = LaserState.Stop;
		lr_Laser_Warning.gameObject.SetActive(value: false);
		lr_LaserShadow_Warning.gameObject.SetActive(value: false);
		tsf_Node1_Warning.gameObject.SetActive(value: false);
		tsf_Node2_Warning.gameObject.SetActive(value: false);
		lr_Laser.gameObject.SetActive(value: false);
		lr_LaserShadow.gameObject.SetActive(value: false);
		tsf_Node1.gameObject.SetActive(value: false);
		tsf_Node2.gameObject.SetActive(value: false);
		ve_Bubble.gameObject.SetActive(value: false);
	}
}
