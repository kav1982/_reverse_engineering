using System;
using Spine;
using UnityEngine;

public class Relic_AddMoveSpeed : MonoBehaviour
{
	private enum LegState
	{
		Idle,
		Back,
		Out
	}

	public GameObject go_Active;

	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public int totalNodeCount;

	public float rootHight;

	public float flexSpeedRatio;

	[Header("Important")]
	public VariableFloat outDistance;

	public float correctExtraDistance;

	public float tailEndOffset;

	public float tailLength;

	public float randomDirThreshold;

	public float middlePoint1Height;

	public float middlePoint2Height;

	[Header("safe mode")]
	public GameObject safemodeVehicle;

	public float wheelHorizontalOffset;

	public GameObject frontWheel;

	public GameObject backWheel;

	public Animator anima;

	public float wheelRootHeight;

	public SpriteRenderer wheel1;

	public SpriteRenderer wheel2;

	private LegState[] states;

	private LineRenderer[] lr_Legs;

	private LineRenderer[] lr_Shadows;

	private float[] correctDistances;

	private Vector3[] moveToEndPoints;

	private Vector3[] endPointOffsets;

	private float[] currentEndPointLerps;

	private Slot slotUpperLegL;

	private Slot slotUpperLegR;

	private Slot slotLowerLegL;

	private Slot slotLowerLegR;

	private Slot slotShoesL;

	private Slot slotShoesR;

	private Slot slotShoesL2;

	private Slot slotShoesR2;

	public RelicConfig RelicCfg { get; private set; }

	public bool HideBySettings => DataMgr.settingData.DisableRelicSkins.Contains(RelicCfg.id);

	public void Initialize(RelicConfig relicCfg)
	{
		RelicCfg = relicCfg;
		if (slotUpperLegL == null)
		{
			slotUpperLegL = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("tui_l");
			slotUpperLegR = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("tui_r");
			slotLowerLegL = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("xiaotui_l");
			slotLowerLegR = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("xiaotui_r");
			slotShoesL = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("xie_l");
			slotShoesR = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("xie_r");
			slotShoesL2 = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("bilibili_xie_l");
			slotShoesR2 = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("bilibili_xie_r");
		}
		if (lr_Legs != null)
		{
			for (int num = lr_Legs.Length - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(lr_Legs[num].gameObject);
				UnityEngine.Object.Destroy(lr_Shadows[num].gameObject);
			}
		}
		lr_Leg.gameObject.SetActive(value: true);
		lr_Shadow.gameObject.SetActive(value: true);
		lr_Leg.positionCount = totalNodeCount;
		lr_Shadow.positionCount = totalNodeCount;
		lr_Legs = new LineRenderer[relicCfg.int1.result];
		lr_Shadows = new LineRenderer[relicCfg.int1.result];
		states = new LegState[relicCfg.int1.result];
		correctDistances = new float[relicCfg.int1.result];
		moveToEndPoints = new Vector3[relicCfg.int1.result];
		endPointOffsets = new Vector3[relicCfg.int1.result];
		currentEndPointLerps = new float[relicCfg.int1.result];
		for (int i = 0; i < relicCfg.int1.result; i++)
		{
			lr_Legs[i] = UnityEngine.Object.Instantiate(lr_Leg, go_Active.transform);
			lr_Shadows[i] = UnityEngine.Object.Instantiate(lr_Shadow, go_Active.transform);
			states[i] = LegState.Out;
			correctDistances[i] = outDistance.RandomResult();
			moveToEndPoints[i] = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * correctDistances[i];
			endPointOffsets[i] = Tool2D.GetDir() * UnityEngine.Random.Range(0f, tailEndOffset);
		}
		lr_Leg.gameObject.SetActive(value: false);
		lr_Shadow.gameObject.SetActive(value: false);
		Update();
		UpdateDisplay();
	}

	private void OnEnable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(UpdateDisplay));
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(UpdateDisplay));
	}

	private void Update()
	{
		if (PlayerMgr.Inst.PlayerPpt.tsf_Layer.gameObject.activeSelf != go_Active.activeSelf)
		{
			if (!go_Active.activeSelf)
			{
				for (int i = 0; i < lr_Legs.Length; i++)
				{
					states[i] = LegState.Idle;
					currentEndPointLerps[i] = 1f;
					correctDistances[i] = outDistance.RandomResult();
					moveToEndPoints[i] = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * correctDistances[i];
				}
			}
			go_Active.SetActive(PlayerMgr.Inst.PlayerPpt.tsf_Layer.gameObject.activeSelf);
		}
		if (!PlayerMgr.Inst.PlayerPpt.tsf_Layer.gameObject.activeSelf)
		{
			return;
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			if (go_Active.activeSelf)
			{
				go_Active.SetActive(value: false);
			}
			return;
		}
		if (DataMgr.settingData.SafeMode)
		{
			float num = 1f;
			Vector3 vector = PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint - PlayerMgr.Inst.PlayerCtrller.transform.position;
			num = ((PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint.y > PlayerMgr.Inst.PlayerCtrller.transform.position.y) ? ((!(vector.x > 0f)) ? (-1f) : 1f) : ((!(vector.x > 0f)) ? 1f : (-1f)));
			bool flipX = num > 0f;
			wheel1.flipX = flipX;
			wheel2.flipX = flipX;
			frontWheel.transform.localPosition = new Vector3(num * wheelHorizontalOffset, frontWheel.transform.localPosition.y, frontWheel.transform.localPosition.z);
			backWheel.transform.localPosition = new Vector3((0f - num) * wheelHorizontalOffset, backWheel.transform.localPosition.y, backWheel.transform.localPosition.z);
			if (vector.x * PlayerMgr.Inst.PlayerCtrller.CurrentMotion.x > 0f)
			{
				anima.SetFloat("speed", num * (0f - PlayerMgr.Inst.PlayerCtrller.CurrentMotion.magnitude) / PlayerMgr.Inst.PlayerCtrller.myPpt.MoveSpeed);
			}
			else if (vector.x * PlayerMgr.Inst.PlayerCtrller.CurrentMotion.x < 0f)
			{
				anima.SetFloat("speed", num * (0f - PlayerMgr.Inst.PlayerCtrller.CurrentMotion.magnitude) / PlayerMgr.Inst.PlayerCtrller.myPpt.MoveSpeed);
			}
			else
			{
				anima.SetFloat("speed", num * (0f - PlayerMgr.Inst.PlayerCtrller.CurrentMotion.magnitude) / PlayerMgr.Inst.PlayerCtrller.myPpt.MoveSpeed);
			}
			safemodeVehicle.transform.localScale = Vector3.one * PlayerMgr.Inst.PlayerT.localScale.x;
			Vector3 v = PlayerMgr.Inst.PlayerPoint + (PlayerMgr.Inst.PlayerPpt.Tsf_BeHit.localPosition + new Vector3(0f, 0f - wheelRootHeight, 0f)) * PlayerMgr.Inst.PlayerT.localScale.x;
			safemodeVehicle.transform.position = Tool2D.IgnoreZPoint(v, PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position.z);
			return;
		}
		if (slotUpperLegL.A != 0f && !HideBySettings)
		{
			slotUpperLegL.A = 0f;
			slotUpperLegR.A = 0f;
			slotLowerLegL.A = 0f;
			slotLowerLegR.A = 0f;
			slotShoesL.A = 0f;
			slotShoesR.A = 0f;
			slotShoesL2.A = 0f;
			slotShoesR2.A = 0f;
		}
		for (int j = 0; j < lr_Legs.Length; j++)
		{
			switch (states[j])
			{
			case LegState.Idle:
				if ((PlayerMgr.Inst.PlayerPoint - moveToEndPoints[j]).sqrMagnitude > (correctDistances[j] + correctExtraDistance) * (correctDistances[j] + correctExtraDistance))
				{
					states[j] = LegState.Back;
				}
				break;
			case LegState.Back:
				currentEndPointLerps[j] = Mathf.MoveTowards(currentEndPointLerps[j], 0f, Mathf.Abs(flexSpeedRatio * PlayerMgr.Inst.PlayerPpt.MoveSpeed) * Time.deltaTime);
				if (currentEndPointLerps[j] == 0f)
				{
					states[j] = LegState.Out;
					correctDistances[j] = outDistance.RandomResult();
					float sqrMagnitude = PlayerMgr.Inst.PlayerCtrller.CurrentMotion.sqrMagnitude;
					float sqrMagnitude2 = PlayerMgr.Inst.PlayerCtrller.rigid.linearVelocity.sqrMagnitude;
					float num2 = randomDirThreshold * randomDirThreshold;
					if (sqrMagnitude < num2 && sqrMagnitude2 < num2)
					{
						moveToEndPoints[j] = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * correctDistances[j];
					}
					else if (sqrMagnitude > sqrMagnitude2)
					{
						moveToEndPoints[j] = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir(PlayerMgr.Inst.PlayerCtrller.CurrentMotion.normalized, UnityEngine.Random.Range(-60f, 60f)) * correctDistances[j];
					}
					else
					{
						moveToEndPoints[j] = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir(PlayerMgr.Inst.PlayerCtrller.rigid.linearVelocity.normalized, UnityEngine.Random.Range(-60f, 60f)) * correctDistances[j];
					}
					if (Physics.Raycast(PlayerMgr.Inst.PlayerPoint, moveToEndPoints[j] - base.transform.position, out var hitInfo, 100f, LayerMask.GetMask("Wall", "Abyss", "Cliff")) && (PlayerMgr.Inst.PlayerPoint - hitInfo.point).sqrMagnitude < (PlayerMgr.Inst.PlayerPoint - moveToEndPoints[j]).sqrMagnitude)
					{
						moveToEndPoints[j] = Tool2D.IgnoreZPoint(hitInfo.point);
					}
				}
				break;
			case LegState.Out:
				currentEndPointLerps[j] = Mathf.MoveTowards(currentEndPointLerps[j], 1f, Mathf.Abs(flexSpeedRatio * PlayerMgr.Inst.PlayerPpt.MoveSpeed) * Time.deltaTime);
				if (currentEndPointLerps[j] == 1f)
				{
					states[j] = LegState.Idle;
				}
				break;
			default:
				Debug.LogError(states[j]);
				break;
			}
			Vector3 vector2 = PlayerMgr.Inst.PlayerPoint + (PlayerMgr.Inst.PlayerPpt.Tsf_BeHit.localPosition + new Vector3(0f, 0f, 0f - rootHight)) * PlayerMgr.Inst.PlayerT.localScale.x;
			Vector3 v2 = vector2 + new Vector3(0f, 0f, 0f - middlePoint1Height) * PlayerMgr.Inst.PlayerT.localScale.x;
			Vector3 v3 = moveToEndPoints[j] + new Vector3(0f, 0f, 0f - middlePoint2Height) * PlayerMgr.Inst.PlayerT.localScale.x;
			for (int k = 0; k < totalNodeCount; k++)
			{
				float num3 = (float)k / ((float)totalNodeCount - 1f) * currentEndPointLerps[j];
				float num4 = correctDistances[j] / (correctDistances[j] + tailLength);
				Vector3 zero = Vector3.zero;
				if (num3 <= num4)
				{
					float t = (correctDistances[j] + tailLength) * num3 / correctDistances[j];
					zero = GeneralTool.CubicBezierCurve(vector2, v2, v3, moveToEndPoints[j] + endPointOffsets[j], t);
				}
				else
				{
					float t2 = ((correctDistances[j] + tailLength) * num3 - correctDistances[j]) / tailLength;
					zero = Vector3.Lerp(moveToEndPoints[j] + endPointOffsets[j], moveToEndPoints[j] + endPointOffsets[j] + endPointOffsets[j].normalized * tailLength, t2);
				}
				lr_Legs[j].SetPosition(k, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(zero), PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position.z + 0.1f));
				lr_Shadows[j].SetPosition(k, Tool2D.IgnoreZPoint(zero, 1.05f));
			}
		}
	}

	private void SetPlayerLegAndShoesAlpha(float alpha)
	{
		slotUpperLegL.A = alpha;
		slotUpperLegR.A = alpha;
		slotLowerLegL.A = alpha;
		slotLowerLegR.A = alpha;
		slotShoesL.A = alpha;
		slotShoesR.A = alpha;
		slotShoesL2.A = alpha;
		slotShoesR2.A = alpha;
	}

	public void UpdateDisplay()
	{
		if (HideBySettings)
		{
			LineRenderer[] array = lr_Legs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			array = lr_Shadows;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			safemodeVehicle.SetActive(value: false);
			SetPlayerLegAndShoesAlpha(1f);
		}
		else if (DataMgr.settingData.SafeMode)
		{
			LineRenderer[] array = lr_Legs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			array = lr_Shadows;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			safemodeVehicle.SetActive(value: true);
			SetPlayerLegAndShoesAlpha(1f);
		}
		else
		{
			LineRenderer[] array = lr_Legs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
			array = lr_Shadows;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
			safemodeVehicle.SetActive(value: false);
			SetPlayerLegAndShoesAlpha(0f);
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < lr_Legs.Length; i++)
		{
			moveToEndPoints[i] += changeValue;
		}
	}

	public void PointerToPlayer()
	{
		for (int i = 0; i < lr_Legs.Length; i++)
		{
			states[i] = LegState.Idle;
			currentEndPointLerps[i] = 1f;
			correctDistances[i] = outDistance.RandomResult();
			moveToEndPoints[i] = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * correctDistances[i];
		}
		Update();
	}

	public void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		slotUpperLegL.A = 1f;
		slotUpperLegR.A = 1f;
		slotLowerLegL.A = 1f;
		slotLowerLegR.A = 1f;
		slotShoesL.A = 1f;
		slotShoesR.A = 1f;
		slotShoesL2.A = 1f;
		slotShoesR2.A = 1f;
	}
}
