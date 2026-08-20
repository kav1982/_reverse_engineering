using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class UIRelic_Huang : MonoBehaviour
{
	private enum UIState
	{
		Idle,
		Hiding,
		Recover,
		Fade
	}

	public CanvasGroup cg;

	public Image image_Fill;

	public Image image_Atleast1;

	public Text text_Count;

	public Animator anima_Cooldown;

	public float fadeTime;

	[Header("Jump")]
	public float jumpMiddlePointHeight;

	public float jumpLerpSpeed;

	public AnimationCurve jumpLerpCurve;

	public AnimationCurve jumpRotateCurve;

	public ShockParam jumpShock;

	public float jumpKnockBack;

	public float jumpXOffset;

	public float jumpYOffset;

	public float jumpFinishInvincibleTime;

	[Header("JumpTimeScale")]
	public float jumpTimeScale;

	public float jumpTimeScaleDuration;

	public float jumpTimeScaleFadeSpeed;

	private Relic_Huang relic_Huang;

	private UIState uiState;

	private float cooldownTimer = 999999f;

	private float fadeTimer;

	private int maxSkillCount = 1;

	private int currentSkillCount = 1;

	private string countText = "";

	private bool isJump;

	private Vector3 jumpStartPoint;

	private Vector3 jumpMiddlePoint;

	private Vector3 jumpMiddlePoint2;

	private Vector3 jumpTargetPoint;

	private float jumpLerp;

	private NativeList<Entity> targetEttList = new NativeList<Entity>(Allocator.Persistent);

	private static HashSet<string> targetTags = new HashSet<string> { "Monster", "Destructible", "RollBall", "Butterfly", "Brittleness" };

	private EntityManager ettMgr;

	private RelicConfig RelicCfg => relic_Huang.RelicCfg;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void Update()
	{
		UpdateAtleast1Image();
		if (isJump)
		{
			jumpLerp += jumpLerpSpeed * PlayerMgr.Inst.PlayerDeltaTime;
			Vector3 playerPoint = GeneralTool.FreeBezierCurve(jumpLerpCurve.Evaluate(jumpLerp), jumpStartPoint, jumpMiddlePoint, jumpMiddlePoint2, jumpTargetPoint);
			PlayerMgr.Inst.SetPlayerPoint(playerPoint);
			float num = jumpRotateCurve.Evaluate(jumpLerp);
			if (jumpStartPoint.x < jumpTargetPoint.x)
			{
				num = 0f - num;
			}
			relic_Huang.tsf_JumpRotate.rotation = Quaternion.Euler(0f, 0f, num);
			if (jumpLerp >= 1f)
			{
				jumpLerp = 0f;
				isJump = false;
				PlayerMgr.Inst.SetPlayerPoint(Tool2D.IgnoreZPoint(PlayerMgr.Inst.PlayerT));
				relic_Huang.tsf_JumpRotate.rotation = Quaternion.identity;
				relic_Huang.AnimaBigSitOnGround();
				PlayerMgr.Inst.InvincibleUnregister();
				PlayerMgr.Inst.FlyUnregister();
				PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
				PlayerMgr.Inst.PlayerCtrller.OpenPlayerCollider();
				SEMgr.Inst.relic_Huang_BigSit.PlaySE();
				CamController.Inst.SetShock(jumpShock);
				float num2 = RelicCfg.float1.result * (1f + PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: false));
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				UnitDotsSyncSystem.GetAttackableEntitiesInRange(PlayerMgr.Inst.PlayerPoint, num2, UnitType.Player, containsBrittleness: true, ref targetEttList);
				TakeDamageInfo_Dots damageInfo = TakeDamageInfo_Dots.NewInfo(PlayerMgr.Inst.PlayerEtt);
				foreach (Entity targetEtt2 in targetEttList)
				{
					Entity targetEtt = targetEtt2;
					if (ettMgr.HasComponent<UnitProperty_Dots>(targetEtt))
					{
						UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(targetEtt);
						LocalTransform componentData2 = ettMgr.GetComponentData<LocalTransform>(targetEtt);
						if (componentData.unitCfg.unitType == UnitType.Brittleness)
						{
							damageInfo.damage = componentData.unitCfg.maxHP;
						}
						else if (componentData.unitCfg.unitType == UnitType.Boss || componentData.unitCfg.unitType == UnitType.Elite)
						{
							damageInfo.damage = componentData.unitCfg.maxHP * ((float)RelicCfg.int2.result / 100f);
						}
						else
						{
							damageInfo.damage = componentData.unitCfg.maxHP * ((float)RelicCfg.int1.result / 100f);
						}
						if (componentData.unitCfg.id == 10501)
						{
							damageInfo.damage = 100f;
						}
						else if (componentData.unitCfg.id == 300901 || componentData.unitCfg.id == 300921)
						{
							if (flag2)
							{
								continue;
							}
							damageInfo.damage = (int)(componentData.unitCfg.maxHP * ((float)RelicCfg.int2.result / 100f));
							flag2 = true;
						}
						else if (componentData.unitCfg.id == 500621 || componentData.unitCfg.id == 500622)
						{
							if (flag3)
							{
								continue;
							}
							damageInfo.damage = (int)(componentData.unitCfg.maxHP * ((float)RelicCfg.int2.result / 100f));
							flag3 = true;
						}
						else if (componentData.unitCfg.id == 501001 || componentData.unitCfg.id == 501021)
						{
							if (flag4)
							{
								continue;
							}
							damageInfo.damage = (int)(componentData.unitCfg.maxHP * ((float)RelicCfg.int2.result / 100f));
							flag4 = true;
						}
						else if (componentData.unitCfg.id == 505001 || componentData.unitCfg.id == 505021)
						{
							if (flag5)
							{
								continue;
							}
							damageInfo.damage = (int)(componentData.unitCfg.maxHP * ((float)RelicCfg.int2.result / 100f));
							flag5 = true;
						}
						else if (componentData.unitCfg.id == 100321)
						{
							damageInfo.damage = (int)(UnitConfig.map[100301].maxHP * ((float)RelicCfg.int1.result / 300f));
						}
						damageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(componentData2.Position, base.transform.position) * jumpKnockBack;
						damageInfo.extraCriticalChance = PlayerMgr.Inst.ExtraCriticalRatio;
						damageInfo.isPercentageDamage = true;
						if (componentData.unitCfg.unitType == UnitType.Monster || componentData.unitCfg.unitType == UnitType.Elite || componentData.unitCfg.unitType == UnitType.Boss)
						{
							if (componentData.voidExplosionData.InstantKillRatio > 0f)
							{
								float num3 = componentData.voidExplosionData.InstantKillRatio;
								if (componentData.unitCfg.unitType == UnitType.Elite || componentData.unitCfg.unitType == UnitType.Boss)
								{
									num3 *= 0.5f;
								}
								if (componentData.unitCfg.maxHP > 0f && (componentData.unitCfg.currentHP - damageInfo.damage) / componentData.unitCfg.maxHP <= num3)
								{
									flag = true;
								}
							}
							if (componentData.unitCfg.currentHP <= damageInfo.damage)
							{
								flag = true;
							}
						}
					}
					UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, ettMgr);
				}
				if (flag)
				{
					MobileMgr.inst.SkillPunch();
					cooldownTimer = RelicCfg.float2.result;
				}
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Relic_Huang", Tool2D.GetLayerPoint(PlayerMgr.Inst.PlayerT.position), Vector3.one * num2, 2f);
				if (PlayerMgr.Inst.ItemCtrller.potion_Invincible == null)
				{
					PlayerMgr.Inst.ItemCtrller.potion_Invincible = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_Invincible"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Invincible>();
				}
				PlayerMgr.Inst.ItemCtrller.potion_Invincible.Initialize(jumpFinishInvincibleTime);
				PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
				StartCoroutine(NonInteractiveUnregister());
			}
		}
		switch (uiState)
		{
		case UIState.Idle:
			if (PlayerMgr.Inst.PlayerCtrller.CanMotion && currentSkillCount > 0 && ControlMgr.Inst.isSprintPressed())
			{
				BigSit();
			}
			break;
		case UIState.Hiding:
			if (PlayerMgr.Inst.PlayerCtrller.CanMotion && currentSkillCount > 0 && ControlMgr.Inst.isSprintPressed())
			{
				BigSit();
			}
			break;
		case UIState.Recover:
		{
			if (currentSkillCount > 0 && ControlMgr.Inst.isSprintPressed() && BigSit())
			{
				break;
			}
			float num4 = RelicCfg.float2.result;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceSkillCD != null)
			{
				num4 *= 1f - (float)PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceSkillCD.int1.result / 100f;
			}
			cooldownTimer += PlayerMgr.Inst.PlayerDeltaTime;
			image_Fill.fillAmount = cooldownTimer / num4;
			if (GameMgr.IsMobile_Static)
			{
				MobileMgr.inst.UpdateSkillCD(1f - image_Fill.fillAmount, GetCountText(), currentSkillCount > 0);
			}
			if (image_Fill.fillAmount >= 1f)
			{
				currentSkillCount = Mathf.Min(maxSkillCount, currentSkillCount + 1);
				UpdateCountText();
				MobileMgr.inst.SkillPunch();
				anima_Cooldown.Play("Cooldown", 0, 0f);
				SEMgr.Inst.uiRelic_Huang_Cooldown.PlaySE();
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
			if (!PlayerMgr.Inst.PlayerCtrller.CanMotion || currentSkillCount <= 0 || !ControlMgr.Inst.isSprintPressed() || !BigSit())
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

	private bool BigSit()
	{
		if (currentSkillCount <= 0)
		{
			return false;
		}
		TimeScaleMgr.Inst.AddNewTimeScaleModifyRequest(jumpTimeScale, jumpTimeScaleDuration, jumpTimeScaleFadeSpeed, TimeScaleMgr.ManagerState.Progress);
		jumpStartPoint = PlayerMgr.Inst.PlayerPoint;
		jumpTargetPoint = PlayerMgr.Inst.PlayerCtrller.shotWorldPointWithOutLerp;
		jumpTargetPoint += new Vector3((jumpTargetPoint.x > jumpStartPoint.x) ? (0f - jumpXOffset) : jumpXOffset, jumpYOffset, 0f);
		jumpTargetPoint = Tool2D.GetNavMeshPointIngoreZ(jumpTargetPoint);
		jumpMiddlePoint = (jumpStartPoint + jumpTargetPoint) / 2f + new Vector3(0f, 0f, 0f - jumpMiddlePointHeight);
		jumpMiddlePoint2 = jumpTargetPoint + new Vector3(0f, 0f, 0f - jumpMiddlePointHeight);
		PlayerMgr.Inst.InvincibleRegister();
		PlayerMgr.Inst.FlyRegister();
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
		PlayerMgr.Inst.PlayerCtrller.ClosePlayerCollider();
		SEMgr.Inst.relic_Huang_JumpStart.PlaySE();
		isJump = true;
		relic_Huang.AnimaBigSitJump();
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
		PlayerMgr.Inst.PlayerCtrller.SetVisiable();
		return true;
	}

	private IEnumerator NonInteractiveUnregister()
	{
		yield return new WaitForSecondsRealtime(jumpFinishInvincibleTime);
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
	}

	public void Initialize(Relic_Huang relic_Huang)
	{
		this.relic_Huang = relic_Huang;
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
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateSkillCD(0f, GetCountText(), interactable: true);
		}
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
		if (targetEttList.IsCreated)
		{
			targetEttList.Dispose();
		}
		Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (targetEttList.IsCreated)
		{
			targetEttList.Dispose();
		}
	}
}
