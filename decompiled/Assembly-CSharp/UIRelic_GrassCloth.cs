using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UIRelic_GrassCloth : MonoBehaviour
{
	private enum UIState
	{
		Idle,
		Hiding,
		Recover,
		Fade
	}

	public CanvasGroup cg;

	public Image image_Atleast1;

	public Image image_Fill;

	public Text text_Count;

	public Animator anima_Cooldown;

	public float fadeTime;

	private UIState uiState;

	private float cooldownTimer = 999999f;

	private float fadeTimer;

	private int maxSkillCount = 1;

	private int currentSkillCount = 1;

	private string countText = "";

	private Entity _listenerEntity;

	private EntityQuery _listenerQuery;

	private EntityQuery _bladeQuery;

	public RelicConfig RelicCfg { get; private set; }

	private void Update()
	{
		UpdateAtleast1Image();
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateSkillCD(1f - image_Fill.fillAmount, GetCountText(), currentSkillCount > 0);
		}
		switch (uiState)
		{
		case UIState.Idle:
			if (currentSkillCount > 0 && ControlMgr.Inst.isSprintPressed())
			{
				SwordBack();
			}
			break;
		case UIState.Hiding:
			if (currentSkillCount > 0 && ControlMgr.Inst.isSprintPressed())
			{
				SwordBack();
			}
			break;
		case UIState.Recover:
		{
			if (currentSkillCount > 0 && ControlMgr.Inst.isSprintPressed() && SwordBack())
			{
				break;
			}
			float num = RelicCfg.float1.result;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceSkillCD != null)
			{
				num *= 1f - (float)PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceSkillCD.int1.result / 100f;
			}
			cooldownTimer += PlayerMgr.Inst.PlayerDeltaTime;
			image_Fill.fillAmount = cooldownTimer / num;
			if (image_Fill.fillAmount >= 1f)
			{
				currentSkillCount = Mathf.Min(maxSkillCount, currentSkillCount + 1);
				UpdateCountText();
				anima_Cooldown.Play("Cooldown", 0, 0f);
				SEMgr.Inst.spell4019PullReady.PlaySE();
				MobileMgr.inst.SkillPunch();
				if (currentSkillCount >= maxSkillCount)
				{
					image_Fill.fillAmount = 1f;
					image_Fill.gameObject.SetActive(value: false);
					uiState = UIState.Idle;
				}
				else
				{
					cooldownTimer = 0f;
					image_Fill.fillAmount = 0f;
					image_Fill.gameObject.SetActive(value: true);
				}
			}
			break;
		}
		case UIState.Fade:
			if (currentSkillCount <= 0 || !ControlMgr.Inst.isSprintPressed() || !SwordBack())
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

	private bool SwordBack()
	{
		if (currentSkillCount <= 0)
		{
			return false;
		}
		if (Time.timeScale <= 0f)
		{
			return false;
		}
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		NativeArray<Spell4019BiAnBladeData> nativeArray = _bladeQuery.ToComponentDataArray<Spell4019BiAnBladeData>(Allocator.Temp);
		bool flag = false;
		for (int i = 0; i < nativeArray.Length; i++)
		{
			if (nativeArray[i].CanReturn)
			{
				flag = true;
				break;
			}
		}
		nativeArray.Dispose();
		if (!flag)
		{
			return false;
		}
		if (!_listenerQuery.IsEmptyIgnoreFilter)
		{
			_listenerEntity = _listenerQuery.GetSingletonEntity();
		}
		entityManager.GetBuffer<BladeShootListenerData>(_listenerEntity).Add(new BladeShootListenerData
		{
			EventType = 1
		});
		SEMgr.Inst.PlaySE("SE_Spell4019SlowTIme");
		TimeScaleMgr.Inst.AddNewTimeScaleModifyRequest(0.05f, 0.18f, 5f, TimeScaleMgr.ManagerState.Progress, 0.05f, 1f, 0f, affectSEPitch: false);
		CamController.Inst.AddNewCameraFocusRequirement(new CameraFocusSizeData(-0.3f, 1, 0.16f, 0.02f, 0.18f));
		bool num = currentSkillCount >= maxSkillCount;
		currentSkillCount--;
		UpdateCountText();
		uiState = UIState.Recover;
		if (num)
		{
			cooldownTimer = 0f;
			image_Fill.fillAmount = 0f;
			image_Fill.gameObject.SetActive(value: true);
		}
		cg.alpha = 1f;
		return true;
	}

	public void Initialize(RelicConfig relicCfg)
	{
		RelicCfg = relicCfg;
		UpdateCount();
	}

	public void UpdateCount()
	{
		maxSkillCount = 1;
		maxSkillCount += ((PlayerMgr.Inst.ItemCtrller.relicCfg_ExtraSkillUsage != null) ? PlayerMgr.Inst.ItemCtrller.relicCfg_ExtraSkillUsage.int1.result : 0);
		currentSkillCount = maxSkillCount;
		image_Fill.fillAmount = 1f;
		image_Fill.gameObject.SetActive(value: false);
		cooldownTimer = 999999f;
		uiState = UIState.Idle;
		UpdateCountText();
		UpdateAtleast1Image();
	}

	public void FullFill()
	{
		image_Fill.fillAmount = 1f;
		image_Fill.gameObject.SetActive(value: false);
		currentSkillCount = maxSkillCount;
		cooldownTimer = 999999f;
		uiState = UIState.Idle;
		UpdateCountText();
		UpdateAtleast1Image();
		MobileMgr.inst.UpdateSkillCD(0f, GetCountText(), interactable: true);
	}

	private string GetCountText()
	{
		return countText;
	}

	private void UpdateCountText()
	{
		countText = ((maxSkillCount > 1) ? (currentSkillCount + " / " + maxSkillCount) : "");
		if (text_Count != null)
		{
			text_Count.text = countText;
		}
	}

	private void UpdateAtleast1Image()
	{
		if (image_Atleast1 != null)
		{
			image_Atleast1.gameObject.SetActive(currentSkillCount > 0);
		}
	}

	public void DestroySelf()
	{
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			wand.ClearAutoSpell(typeof(Spell4019BiAnBladeData));
		}
		Object.Destroy(base.gameObject);
	}

	private void Awake()
	{
		_listenerQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<BladeShootListenerData>());
		_bladeQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Spell4019BiAnBladeData>());
	}
}
