using System.Collections;
using UnityEngine;

public class Relic_Fly : MonoBehaviour
{
	public LineRenderer lr_WingL;

	public LineRenderer lr_WingR;

	public int nodeCount;

	public float rootHight;

	public float segmentLength;

	public float lerp;

	public float rotateSpeed;

	public float rotateHalfAngle;

	private Vector3[] wingPointsL;

	private Vector3[] wingPointsR;

	public bool HideBySettings => DataMgr.settingData.DisableRelicSkins.Contains(13);

	private float FinalZ
	{
		get
		{
			if (PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint.y > PlayerMgr.Inst.PlayerPoint.y)
			{
				return -0.3f;
			}
			return 0.3f;
		}
	}

	private void Start()
	{
		lr_WingL.positionCount = nodeCount;
		lr_WingR.positionCount = nodeCount;
		wingPointsL = new Vector3[nodeCount];
		wingPointsR = new Vector3[nodeCount];
		float finalZ = FinalZ;
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				wingPointsL[i] = base.transform.position + new Vector3(0f, rootHight, 0f);
				wingPointsR[i] = base.transform.position + new Vector3(0f, rootHight, 0f);
			}
			else
			{
				wingPointsL[i] = wingPointsL[i - 1] + Vector3.left * segmentLength;
				wingPointsR[i] = wingPointsR[i - 1] + Vector3.right * segmentLength;
			}
			lr_WingL.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(wingPointsL[i]), finalZ));
			lr_WingR.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(wingPointsR[i]), finalZ));
		}
	}

	private void Update()
	{
		lr_WingL.enabled = !HideBySettings;
		lr_WingR.enabled = !HideBySettings;
		if ((bool)PlayerMgr.Inst.ItemCtrller.potion_Petrifaction)
		{
			return;
		}
		Vector3 vector = new Vector3(0f, 0f, FinalZ);
		float num = Mathf.Sin(Time.timeSinceLevelLoad * rotateSpeed) * rotateHalfAngle;
		Vector3 dir = Tool2D.GetDir(Vector3.left, num);
		Vector3 dir2 = Tool2D.GetDir(Vector3.right, 0f - num);
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				wingPointsL[i] = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, rootHight, 0f);
				wingPointsR[i] = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, rootHight, 0f);
			}
			else
			{
				wingPointsL[i] = Vector3.Lerp(wingPointsL[i], wingPointsL[i - 1] + dir * segmentLength, lerp * Time.deltaTime);
				wingPointsR[i] = Vector3.Lerp(wingPointsR[i], wingPointsR[i - 1] + dir2 * segmentLength, lerp * Time.deltaTime);
			}
			lr_WingL.SetPosition(i, wingPointsL[i] + vector);
			lr_WingR.SetPosition(i, wingPointsR[i] + vector);
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < nodeCount; i++)
		{
			wingPointsL[i] += changeValue;
			wingPointsR[i] += changeValue;
		}
	}

	public void PointerToPlayer()
	{
		Vector3 vector = new Vector3(0f, 0f, FinalZ);
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				wingPointsL[i] = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, rootHight, 0f);
				wingPointsR[i] = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, rootHight, 0f);
			}
			else
			{
				wingPointsL[i] = wingPointsL[i - 1] + Vector3.left * segmentLength;
				wingPointsR[i] = wingPointsR[i - 1] + Vector3.right * segmentLength;
			}
			lr_WingL.SetPosition(i, wingPointsL[i] + vector);
			lr_WingR.SetPosition(i, wingPointsR[i] + vector);
		}
	}

	public void PointerToPlayerThrougPotionPetrifaction()
	{
		StartCoroutine(PointerToPlayerThrougPotionPetrifactionIE());
	}

	private IEnumerator PointerToPlayerThrougPotionPetrifactionIE()
	{
		yield return null;
		Vector3 vector = new Vector3(0f, 0f, 0.3f);
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				wingPointsL[i] = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, rootHight, 0f);
				wingPointsR[i] = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, rootHight, 0f);
			}
			else
			{
				wingPointsL[i] = wingPointsL[i - 1] + Vector3.left * segmentLength;
				wingPointsR[i] = wingPointsR[i - 1] + Vector3.right * segmentLength;
			}
			lr_WingL.SetPosition(i, wingPointsL[i] + vector);
			lr_WingR.SetPosition(i, wingPointsR[i] + vector);
		}
	}

	public void DestroySelf()
	{
		PlayerMgr.Inst.FlyUnregister();
		Object.Destroy(base.gameObject);
	}
}
