using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRelic_LightArmor : MonoBehaviour
{
	private enum UIState
	{
		Idle,
		Hiding,
		Recover,
		Fade
	}

	public CanvasGroup cg;

	public RectTransform icon_template;

	public Animator anima_Cooldown;

	public float outlineSpace;

	public Color color_FillCooling;

	public Color color_FillComplete;

	public float fadeTime;

	public GameObject go_Trail;

	[Header("Sprint")]
	public float sprintDuration;

	public float sprintSpeed;

	private UIState uiState;

	private List<float> timers = new List<float>();

	private List<Image> image_Fills = new List<Image>();

	private float fadeTimer;

	private int waitSprintIndex;

	private bool isPlayerSprint;

	private float sprintDurationTimer;

	public RelicConfig RelicCfg { get; private set; }

	public Vector3 SprintForce { get; private set; }

	private void Update()
	{
		if (isPlayerSprint)
		{
			sprintDurationTimer += PlayerMgr.Inst.PlayerDeltaTime;
			if (sprintDurationTimer >= sprintDuration)
			{
				StopSprint();
			}
		}
		bool flag = (GameMgr.IsMobile_Static ? ControlMgr.Inst.IsActiveSkillKeyUp() : ControlMgr.Inst.isSprintPressed());
		switch (uiState)
		{
		case UIState.Idle:
			if (!isPlayerSprint && PlayerMgr.Inst.PlayerCtrller.CanMotion && flag)
			{
				Sprint();
			}
			break;
		case UIState.Hiding:
			if (!isPlayerSprint && PlayerMgr.Inst.PlayerCtrller.CanMotion && flag)
			{
				Sprint();
			}
			break;
		case UIState.Recover:
		{
			if (!isPlayerSprint && PlayerMgr.Inst.PlayerCtrller.CanMotion && waitSprintIndex > -1 && flag && Sprint())
			{
				break;
			}
			float num = RelicCfg.float1.result;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceSkillCD != null)
			{
				num *= 1f - (float)PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceSkillCD.int1.result / 100f;
			}
			timers[waitSprintIndex + 1] += PlayerMgr.Inst.PlayerDeltaTime;
			image_Fills[waitSprintIndex + 1].fillAmount = timers[waitSprintIndex + 1] / num;
			if (GameMgr.IsMobile_Static)
			{
				MobileMgr.inst.UpdateSkillCD(1f - image_Fills[waitSprintIndex + 1].fillAmount, (waitSprintIndex + 1).ToString(), waitSprintIndex + 1 != 0);
			}
			if (!(image_Fills[waitSprintIndex + 1].fillAmount >= 1f))
			{
				break;
			}
			image_Fills[waitSprintIndex + 1].fillAmount = 1f;
			image_Fills[waitSprintIndex + 1].color = color_FillComplete;
			anima_Cooldown.transform.position = image_Fills[waitSprintIndex + 1].transform.position;
			anima_Cooldown.Play("Cooldown", 0, 0f);
			SEMgr.Inst.uiRelic_LightArmor_Cooldown.PlaySE();
			waitSprintIndex++;
			if (waitSprintIndex == image_Fills.Count - 1)
			{
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.UpdateSkillCD(0f, (waitSprintIndex + 1).ToString(), interactable: true);
					MobileMgr.inst.SkillPunch();
				}
				uiState = UIState.Idle;
			}
			break;
		}
		case UIState.Fade:
			if (!(PlayerMgr.Inst.PlayerCtrller.CanMotion && flag) || !Sprint())
			{
				fadeTimer += PlayerMgr.Inst.PlayerDeltaTime;
				cg.alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeTime);
				if (fadeTimer >= fadeTime)
				{
					fadeTimer = 0f;
					uiState = UIState.Hiding;
				}
			}
			break;
		default:
			Debug.LogError(uiState);
			break;
		}
	}

	private bool Sprint()
	{
		if (waitSprintIndex == -1 || TopUI.inst.uI_AimSkill.skillCancle)
		{
			TopUI.inst.uI_AimSkill.skillCancle = false;
			return false;
		}
		uiState = UIState.Recover;
		cg.alpha = 1f;
		fadeTimer = 0f;
		if (waitSprintIndex < image_Fills.Count - 1)
		{
			image_Fills[waitSprintIndex].fillAmount = image_Fills[waitSprintIndex + 1].fillAmount;
			image_Fills[waitSprintIndex].color = color_FillCooling;
			timers[waitSprintIndex] = timers[waitSprintIndex + 1];
			image_Fills[waitSprintIndex + 1].fillAmount = 0f;
			timers[waitSprintIndex + 1] = 0f;
		}
		else
		{
			image_Fills[waitSprintIndex].fillAmount = 0f;
			image_Fills[waitSprintIndex].color = color_FillCooling;
			timers[waitSprintIndex] = 0f;
		}
		waitSprintIndex--;
		isPlayerSprint = true;
		if (TopUI.inst.uI_AimSkill.finalDistance > 0.5f)
		{
			SprintForce = TopUI.inst.uI_AimSkill.aimDir.normalized * sprintSpeed;
		}
		else
		{
			SprintForce = PlayerMgr.Inst.PlayerCtrller.CurrentMoveDir * sprintSpeed;
		}
		PlayerMgr.Inst.InvincibleRegister();
		PlayerMgr.Inst.FlyRegister();
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
		PlayerMgr.Inst.PlayerCtrller.SetVisiable();
		go_Trail.SetActive(value: true);
		SEMgr.Inst.sprint.PlaySE();
		if ((bool)PlayerMgr.Inst.SelectedWand)
		{
			PlayerMgr.Inst.SelectedWand.FreeShoot();
		}
		return true;
	}

	public void Initialize(RelicConfig relicCfg)
	{
		RelicCfg = relicCfg;
		go_Trail.transform.SetParent(PlayerMgr.Inst.PlayerPpt.tsf_Layer);
		go_Trail.transform.localScale = Vector3.one;
		go_Trail.transform.localPosition = Vector3.zero;
		go_Trail.gameObject.SetActive(value: false);
		UpdateCount();
	}

	public void UpdateCount()
	{
		uiState = UIState.Idle;
		for (int i = 0; i < icon_template.parent.childCount; i++)
		{
			Transform child = icon_template.parent.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				Object.Destroy(child.gameObject);
			}
		}
		image_Fills.Clear();
		timers.Clear();
		int result = RelicCfg.int2.result;
		result += ((PlayerMgr.Inst.ItemCtrller.relicCfg_ExtraSkillUsage != null) ? PlayerMgr.Inst.ItemCtrller.relicCfg_ExtraSkillUsage.int1.result : 0);
		for (int j = 0; j < result; j++)
		{
			GameObject obj = Object.Instantiate(icon_template.gameObject, icon_template.parent);
			obj.gameObject.SetActive(value: true);
			obj.GetComponent<RectTransform>().anchoredPosition = icon_template.anchoredPosition + new Vector2(outlineSpace * (float)j, 0f);
			Image component = obj.transform.GetChild(0).GetComponent<Image>();
			component.fillAmount = 1f;
			component.color = color_FillComplete;
			image_Fills.Add(component);
			timers.Add(99f);
		}
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateSkillCD(0f, result.ToString(), interactable: true);
		}
		waitSprintIndex = result - 1;
	}

	public void FullFill()
	{
		for (int i = 0; i < image_Fills.Count; i++)
		{
			image_Fills[i].fillAmount = 1f;
			image_Fills[i].color = color_FillComplete;
			timers[i] = 99f;
		}
		waitSprintIndex = image_Fills.Count - 1;
		uiState = UIState.Idle;
		MobileMgr.inst.UpdateSkillCD(0f, RelicCfg.int2.result.ToString(), interactable: true);
	}

	public void StopSprint()
	{
		if (isPlayerSprint)
		{
			sprintDurationTimer = 0f;
			isPlayerSprint = false;
			SprintForce = Vector3.zero;
			PlayerMgr.Inst.InvincibleUnregister();
			PlayerMgr.Inst.FlyUnregister();
			PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
			go_Trail.SetActive(value: false);
		}
	}

	public void DestroySelf()
	{
		Object.Destroy(go_Trail);
		Object.Destroy(base.gameObject);
	}
}
