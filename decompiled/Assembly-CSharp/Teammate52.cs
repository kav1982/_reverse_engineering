using System.Collections.Generic;
using UnityEngine;

public class Teammate52 : UnitBase
{
	public static List<Teammate52> autoWandList;

	[Header("FloatingWandParameter")]
	public static float wandBaseAngle;

	private static bool angleIncreasedInThisFrame;

	public Vector3 defaultAngle;

	[HideInInspector]
	public Wand targetWand;

	[HideInInspector]
	public List<SpellBase> currentCastingSpell = new List<SpellBase>();

	public float idleCheckEnemyInRangeTime;

	public Transform shootPosition;

	public SpriteRenderer wandSprite;

	public SpriteRenderer SpecialWandSprite;

	public Transform wandRotateTransform;

	public Transform wandCenterTransform;

	public float minRecoil;

	public float wandRotateSpeed;

	public float wandRotateRadius;

	public float wandRotateLerpSpeed;

	public float floatShiftHeight;

	public float floatBaseHeight;

	public float heightFloatSpeed;

	public float heightShiftDownScale;

	public float rotationAmplify;

	public float rotateFrequency;

	public float maxRotatingLerpSpeedRatio;

	public float backDistance;

	public GameObject visualObject;

	public float recheckInterval;

	private float hammerForceMoveDistance;

	private float floatHeightCounter;

	private float keepCastingTimer;

	private Vector3 lastFrameSelfPosition;

	[HideInInspector]
	public Vector3 lastFrameTargetDirection;

	[HideInInspector]
	public Vector3 lastFrameTargetPosition;

	private bool waitingForFullMana;

	public AutoWandData wandShootData;

	public float attackDistance { get; set; }

	public float ExtraSpeed { get; set; }

	public float sizeRatio { get; set; } = 1f;


	public int chargeCount => targetWand.ChargeStars.Count;

	public bool ignoreWall { get; set; }

	public override void Update()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (!angleIncreasedInThisFrame)
		{
			wandBaseAngle += wandRotateSpeed * Time.deltaTime;
			if (wandBaseAngle <= -360f)
			{
				wandBaseAngle += 360f;
			}
			angleIncreasedInThisFrame = true;
		}
		base.Update();
	}

	protected override void CreateSummonEffect()
	{
	}

	public override float GetSummonUnitRealMoveSpeed()
	{
		float num = base.GetSummonUnitRealMoveSpeed();
		if (hammerForceMoveDistance > 0f)
		{
			num *= 1f + targetWand.passiveOwnerSpeedUpRatio;
		}
		return num * (1f + ExtraSpeed / 10f * 2f);
	}

	private void LateUpdate()
	{
		angleIncreasedInThisFrame = false;
		wandCenterTransform.localPosition = GetFloatingHeight();
		GetAttackDistance();
		CheckCurrentShootGroupIgnoreWallState();
		CheckWatingState();
		if (wandShootData != null)
		{
			wandShootData.shootTransform = shootPosition;
			wandShootData.targetPosition = lastFrameTargetPosition;
			wandShootData.shootDirection = wandRotateTransform.transform.right;
		}
		myPpt.tsf_Layer.localPosition = new Vector3(0f, 0f, myPpt.tsf_Layer.localPosition.z);
	}

	private void CheckCurrentShootGroupIgnoreWallState()
	{
		SpellShootGroup currentShootGroup = targetWand.currentShootGroup;
		if (currentShootGroup == null)
		{
			ignoreWall = false;
		}
		ignoreWall = SpellGroupAttackDistanceCalculator.SpellGroupWillIgnoreWall(currentShootGroup, base.SummonerSpellBase.shooterWand);
	}

	private void OnDisable()
	{
		autoWandList.Remove(this);
	}

	public override void EveryInitialCallback()
	{
		floatHeightCounter = 0f;
		waitingForFullMana = false;
		keepCastingTimer = 0f;
		lastFrameTargetDirection = Vector3.zero;
		lastFrameTargetPosition = Vector3.zero;
		lastFrameSelfPosition = Vector3.zero;
		attackDistance = 0f;
		hammerForceMoveDistance = 0f;
		if (autoWandList == null)
		{
			autoWandList = new List<Teammate52>();
		}
		autoWandList.Add(this);
		base.transform.eulerAngles = defaultAngle;
		navAreaMask = 32;
	}

	public override void SummonsThrough()
	{
		base.SummonsThrough();
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
	}

	private Vector3 GetShootPoint()
	{
		return Tool2D.IgnoreZPoint(wandRotateTransform.transform.position + wandRotateTransform.transform.right * 0.25f) - new Vector3(0f, 0.25f, 0f);
	}

	private void CheckWatingState()
	{
		if (targetWand != null && targetWand.WandCfg != null && targetWand.gameObject.activeInHierarchy && waitingForFullMana && targetWand.CurrentMP >= targetWand.MaxMP)
		{
			waitingForFullMana = false;
		}
	}

	public AutoWandData InitialWandShootData()
	{
		wandShootData = new AutoWandData();
		wandShootData.wandObject = base.gameObject;
		wandShootData.wandObjectScript = this;
		wandShootData.shootTransform = shootPosition;
		wandShootData.shootPosition = GetShootPoint() - default(Vector3);
		wandShootData.currentPosition = base.transform.position + GetFloatingHeight();
		wandShootData.shootDirection = ((lastFrameTargetDirection == Vector3.zero) ? PlayerMgr.Inst.PlayerDir : wandRotateTransform.transform.right);
		wandShootData.wandPpt = myPpt;
		wandShootData.currentPosition = wandCenterTransform.transform.right;
		return wandShootData;
	}

	public Vector3 GetFloatingHeight()
	{
		Vector3 zero = Vector3.zero;
		if (keepCastingTimer >= 0f)
		{
			zero += new Vector3(0f, Mathf.Sin(floatHeightCounter) * floatShiftHeight, 0f);
		}
		else
		{
			zero += new Vector3(0f, Mathf.Sin(floatHeightCounter) * floatShiftHeight / 2f, 0f);
		}
		if (zero.y > 0f)
		{
			zero.y *= heightShiftDownScale;
		}
		zero.y += floatBaseHeight;
		return zero;
	}

	private void GetAttackDistance()
	{
		float num = 100f;
		float num2 = attackDistance;
		SpellShootGroup currentShootGroup = targetWand.currentShootGroup;
		if (currentShootGroup != null && !targetWand.passiveRandomPosShoot)
		{
			num = SpellGroupAttackDistanceCalculator.GetMinAttackDistance(currentShootGroup, targetWand, getMaxDistance: true);
			if (SpellGroupAttackDistanceCalculator.GetShootGroupMovementType(currentShootGroup, targetWand) != SpellSpecialMovementType.Rotation)
			{
				num *= 0.9f;
			}
			if (currentShootGroup.Shoots.Length == 0)
			{
				attackDistance = num2;
			}
			else
			{
				attackDistance = num;
			}
		}
		else
		{
			attackDistance = num;
		}
	}

	public void SetWandDate(Wand data)
	{
		targetWand = data;
		wandSprite.gameObject.SetActive(value: true);
		SpecialWandSprite.gameObject.SetActive(value: false);
		wandSprite.sprite = ABResources.LoadAsset<Sprite>(data.WandCfg.GetIconPath());
		if (data.WandCfg != null)
		{
			WandAbility specialAbility = data.WandCfg.specialAbility;
			if (specialAbility == WandAbility.LongWand || specialAbility == WandAbility.LongWandAndSpellBreaker || GameConstManaged.SpecialLongWandIdList.Contains(data.WandCfg.id))
			{
				wandSprite.gameObject.SetActive(value: false);
				SpecialWandSprite.gameObject.SetActive(value: true);
				SpecialWandSprite.sprite = ABResources.LoadAsset<Sprite>(data.WandCfg.GetIconPath() + "L");
			}
		}
	}
}
