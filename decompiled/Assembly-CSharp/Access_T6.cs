using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Access_T6 : AccessBase
{
	[Space(50f)]
	public GameObject go_Highlight;

	public SkeletonAnimation sAnima;

	public SphereCollider sc;

	public TriggerIn triggerIn;

	[Header("Rune")]
	public SpriteRenderer sr_Rune;

	public VariableFloat intensity;

	public float intensitySpeed;

	[Header("BossBlood")]
	public Light2D light2d;

	public Color color_LightColor;

	public SpriteRenderer sr_Portal;

	public Material mat_BossPortal;

	private void Update()
	{
		float t = Mathf.Sin(Time.time * intensitySpeed) / 2f + 0.5f;
		sr_Rune.material.SetColor("_Color", Color.white * Mathf.Lerp(intensity.value1, intensity.value2, t));
	}

	private void TriggerEnter(Collider other)
	{
		if (other.IsPlayerTrigger())
		{
			LevelMgr.Inst.PlayerEnterAccess(base.Dir);
		}
		else if (other.tag == "Item")
		{
			switch (base.Dir)
			{
			case FourDir.Up:
				other.transform.position = base.transform.position + new Vector3(0f, -1f);
				break;
			case FourDir.Right:
				other.transform.position = base.transform.position + new Vector3(-1f, 0f);
				break;
			case FourDir.Down:
				other.transform.position = base.transform.position + new Vector3(0f, 1f);
				break;
			case FourDir.Left:
				other.transform.position = base.transform.position + new Vector3(1f, 0f);
				break;
			default:
				Debug.LogError(base.Dir);
				break;
			}
		}
	}

	private void SAnimaEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string @string = e.String;
		if (!(@string == "OpenFinish"))
		{
			if (!(@string == "CloseFinish"))
			{
				Debug.LogError(e.String);
			}
			return;
		}
		sc.enabled = false;
		if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
		{
			triggerIn.gameObject.SetActive(value: true);
		}
	}

	public override void Initialize(RoomController roomCtrller, AccessType type, FourDir dir)
	{
		base.Initialize(roomCtrller, type, dir);
		triggerIn.Initialize(TriggerEnter);
		sAnima.AnimationState.Event += SAnimaEvent;
		MonoBehaviour.print(base.AccessType);
		switch (base.AccessType)
		{
		case AccessType.Normal:
			base.IsConditionComplete = true;
			sAnima.skeleton.SetSkin("T6_Access_NotNeedKey");
			sAnima.skeleton.SetSlotsToSetupPose();
			break;
		case AccessType.Key:
			base.IsConditionComplete = false;
			break;
		default:
			Debug.LogError(base.AccessType);
			break;
		}
		if (roomCtrller.roomCfg.type == RoomType.Boss || roomCtrller.roomCfg.type == RoomType.BloodRelic)
		{
			light2d.color = color_LightColor;
			sr_Rune.color = color_LightColor;
			sr_Portal.material = mat_BossPortal;
		}
	}

	public override void Open()
	{
		base.gameObject.tag = "Untagged";
		if (base.AccessType == AccessType.Key)
		{
			if (base.IsConditionComplete)
			{
				sAnima.AnimationState.SetAnimation(0, "Open", loop: false);
				triggerIn.gameObject.SetActive(value: false);
				AccessOpenSE();
				tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.T6Door);
			}
			else
			{
				base.gameObject.tag = "InteractiveObj";
			}
		}
		else
		{
			sAnima.AnimationState.SetAnimation(0, "Open", loop: false);
			triggerIn.gameObject.SetActive(value: false);
			AccessOpenSE();
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.T6Door);
		}
	}

	public override void OpenDirect()
	{
		base.gameObject.tag = "InteractiveObj";
		if (base.AccessType == AccessType.Key)
		{
			if (base.IsConditionComplete)
			{
				sAnima.AnimationState.SetAnimation(0, "OpenDirect", loop: false);
				sc.enabled = false;
				if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
				{
					triggerIn.gameObject.SetActive(value: true);
				}
				tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.T6Door);
			}
		}
		else
		{
			sAnima.AnimationState.SetAnimation(0, "OpenDirect", loop: false);
			sc.enabled = false;
			if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
			{
				triggerIn.gameObject.SetActive(value: true);
			}
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.T6Door);
		}
	}

	public override void Close()
	{
		base.gameObject.tag = "Untagged";
		triggerIn.gameObject.SetActive(value: false);
		sc.enabled = true;
		if (base.IsConditionComplete)
		{
			sAnima.AnimationState.SetAnimation(0, "Close", loop: false);
		}
		else
		{
			sAnima.AnimationState.SetAnimation(0, "CloseDirect", loop: false);
		}
		AccessOpenSE();
	}

	public override void CloseDirect()
	{
		base.gameObject.tag = "Untagged";
		sAnima.AnimationState.SetAnimation(0, "CloseDirect", loop: false);
		triggerIn.gameObject.SetActive(value: false);
		sc.enabled = true;
	}

	public override void ConditionComplete()
	{
		base.ConditionComplete();
		if (base.gameObject.tag == "InteractiveObj")
		{
			Open();
		}
	}

	public override void Select()
	{
		go_Highlight.SetActive(value: true);
	}

	public override void Unselect()
	{
		go_Highlight.SetActive(value: false);
	}

	public override void Interact()
	{
		switch (base.AccessType)
		{
		case AccessType.Key:
			if (PlayerMgr.Inst.IsKeyEnough())
			{
				PlayerMgr.Inst.ChangeKey(-PlayerMgr.Inst.NeedKeyCount(), TextFloatQueueType.DirectFloat);
				ConditionComplete();
			}
			break;
		default:
			Debug.LogError(base.AccessType);
			break;
		case AccessType.Normal:
			break;
		}
	}
}
