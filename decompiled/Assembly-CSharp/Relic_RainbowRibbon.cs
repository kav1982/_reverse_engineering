using UnityEngine;

public class Relic_RainbowRibbon : MonoBehaviour
{
	public LineRenderer lr_Ribbon;

	public int nodeCount;

	public float segmentLength;

	public float lerp;

	public float rootHight;

	public float faceLeftRightAngleOffset;

	public float aaa;

	private Vector3[] nodePoints;

	private Vector3[] nodePoints2;

	private Vector3 faceRightDir;

	private Vector3 faceLeftDir;

	public bool HideBySettings => DataMgr.settingData.DisableRelicSkins.Contains(72);

	private UnitProperty PlayerPpt => PlayerMgr.Inst.PlayerPpt;

	private void Start()
	{
		lr_Ribbon.positionCount = nodeCount;
		nodePoints = new Vector3[nodeCount];
		nodePoints2 = new Vector3[nodeCount];
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = PlayerPpt.transform.position + new Vector3(0f, 0f, 0f - rootHight);
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] + Vector3.left * segmentLength;
			}
			lr_Ribbon.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position.z));
		}
		faceRightDir = Tool2D.GetDir(180f - faceLeftRightAngleOffset);
		faceLeftDir = Tool2D.GetDir(180f + faceLeftRightAngleOffset);
	}

	private void Update()
	{
		lr_Ribbon.enabled = !HideBySettings;
		Vector3 vector = ((PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint.x > PlayerMgr.Inst.PlayerPoint.x) ? faceRightDir : faceLeftDir);
		float num = ((PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint.y > PlayerMgr.Inst.PlayerPoint.y) ? (-0.1f) : 0.1f);
		for (int i = 0; i < nodeCount; i++)
		{
			switch (i)
			{
			case 0:
				nodePoints[i] = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, rootHight, 0f);
				break;
			case 1:
				nodePoints[i] = nodePoints[i - 1] + vector * segmentLength;
				break;
			default:
			{
				Vector3 b = nodePoints[i - 1] - nodePoints[i - 2];
				Vector3 vector2 = Vector3.Lerp(nodePoints[i] - nodePoints[i - 1], b, lerp);
				Vector3 target = nodePoints[i - 1] + vector2.normalized * segmentLength;
				nodePoints[i] = Vector3.SmoothDamp(nodePoints[i], target, ref nodePoints2[i], aaa);
				break;
			}
			}
			Vector3 position = nodePoints[i];
			position.z = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position.z + num;
			lr_Ribbon.SetPosition(i, position);
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < nodeCount; i++)
		{
			nodePoints[i] += changeValue;
		}
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
