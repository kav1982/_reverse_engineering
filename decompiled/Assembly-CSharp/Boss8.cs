using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss8 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Phase_1_Idle,
		Phase_2_Idle,
		ChangePhase
	}

	public bool canAttack;

	public float extraView;

	public List<int> actionList = new List<int>();

	public VariableFloat attackCD;

	public float attackCDTimer;

	[Header("头部移动")]
	public float headSpeed;

	public Transform head;

	public Transform body;

	public Transform neckControl;

	public Transform[] neckSegments;

	public bool headFollow;

	[Header("手部移动")]
	public Transform leftHand;

	public Transform leftArmRoot;

	public Transform leftArmControl;

	public Transform[] leftArmSegments;

	public Transform leftGunPivot;

	public GameObject[] rightHands;

	public Transform rightHand;

	public Transform rightArmRoot;

	public Transform rightArmControl;

	public Transform[] rightArmSegments;

	public Transform rightGunPivot;

	[Header("着火的子弹")]
	private SpellInitialParameter fireBullet = new SpellInitialParameter();

	public float fireSpellHeight;

	public float fireSpellSpeed;

	public float fireSpellDuration;

	public int fireSpellDamage;

	[Header("骷髅头子弹")]
	private SpellInitialParameter skullBullet = new SpellInitialParameter();

	public float skullSpellHeight;

	public float skullSpellSpeed;

	public float skullSpellDuration;

	public int skullSpellDamage;

	[Header("子弹人子弹")]
	private SpellInitialParameter kinBullet = new SpellInitialParameter();

	public float kinSpellHeight;

	public float kinSpellSpeed;

	public float kinSpellDuration;

	public int kinSpellDamage;

	[Header("跟踪骷髅弹")]
	public int skullAmount;

	public float skullOffsetAngle;

	[Header("横向吐息")]
	public bool breath;

	public VariableFloat horizontalOffset;

	public VariableFloat horizontalShootCD;

	public VariableFloat bulletPositionXOffset;

	public bool horizontalAttack;

	public bool headOnPosition;

	public Transform headParent;

	public Transform mouth;

	public float breathTimer;

	[Header("纵向吐息")]
	public VariableFloat verticalOffset;

	public VariableFloat verticalShootCD;

	public float breathLength;

	public float breathLengthTimer;

	[Header("反弹子弹人")]
	public int kinAmount;

	public float kinOffsetAngle;

	[Header("火箭弹")]
	public int circleAmount;

	public float rocketOffsetAngle;

	public Transform rocketTransform;

	[Header("爆炸")]
	public ShockParam shockParam;

	public float knockback;

	public float boomRadius;

	public int boomDamage;

	public float playerDamageRatio;

	public bool boom;

	[Header("DoubleCircle-两圈圆圈")]
	public int doubleCircleAmount;

	[Header("DoubleCircle-前后弧形")]
	public int frontArcAmount;

	public int behindArcAmount;

	[Header("直角轨迹")]
	public int rightAngleAmount;

	public Transform leftPivot;

	public Transform rightPivot;

	[Header("交叉弧形")]
	public int doubleArcAmount;

	[Header("UZI扫射")]
	public VariableFloat uziOffset;

	public VariableFloat uziShootCD;

	public bool isStrafing;

	[Header("扔匕首")]
	public Transform leftKnifePivot;

	public Transform rightKnifePivot;

	[Header("弹幕雨")]
	public Transform leftSide;

	public Transform rightSide;

	public int bulletHorizontalRainAmount;

	public int bulletVerticalRainAmount;

	public float bulletRainInterval;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("动画机")]
	public Animator armsAnima;

	public Animator headAnima;

	public Animator wingsAnima;

	public float x;

	public float y;

	public MonsterState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	public override void SingleInitialCallback()
	{
		fireBullet.spelldataConfig = SpellConfig.GetConfigCopy(90281);
		fireBullet.spelldataConfig.speed = fireSpellSpeed;
		fireBullet.spelldataConfig.duration = fireSpellDuration;
		fireBullet.spelldataConfig.damage = fireSpellDamage;
		fireBullet.ownerPpt = myPpt;
		skullBullet.spelldataConfig = SpellConfig.GetConfigCopy(90311);
		skullBullet.spelldataConfig.speed = skullSpellSpeed;
		skullBullet.spelldataConfig.duration = skullSpellDuration;
		skullBullet.spelldataConfig.damage = skullSpellDamage;
		skullBullet.ownerPpt = myPpt;
		kinBullet.spelldataConfig = SpellConfig.GetConfigCopy(90321);
		kinBullet.spelldataConfig.speed = kinSpellSpeed;
		kinBullet.spelldataConfig.duration = kinSpellDuration;
		kinBullet.spelldataConfig.damage = kinSpellDamage;
		kinBullet.ownerPpt = myPpt;
		kinBullet.shootSpellPreSpells.Add(30121);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		actionList.Clear();
		for (int i = 0; i < 10; i++)
		{
			actionList.Add(i);
		}
		attackCD.RandomResult();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		CamController.Inst.ClearExtraCameraFocusRequirement();
	}

	public override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.C))
		{
			StartCoroutine(SpawnBulletRainWithCurvePath());
		}
		BreathAttack();
		neckControl.position = new Vector3(head.position.x + (body.position.x - head.position.x) * 0.66f, head.position.y + 3.5f, neckControl.position.z);
		leftArmControl.position = new Vector3((leftHand.position.x + leftArmRoot.position.x) * x, leftArmRoot.position.y - y, leftArmControl.position.z);
		rightArmControl.position = new Vector3((rightHand.position.x + rightArmRoot.position.x) * x, rightArmRoot.position.y - y, rightArmControl.position.z);
		if (headFollow)
		{
			float num = ((PlayerMgr.Inst.PlayerPpt.transform.position.x < -3.8f) ? (-3.8f) : ((!(PlayerMgr.Inst.PlayerPpt.transform.position.x > 3.8f)) ? PlayerMgr.Inst.PlayerPpt.transform.position.x : 3.8f));
			head.localPosition = Vector3.MoveTowards(head.localPosition, Tool2D.IgnoreZPoint(new Vector3(num, 0f, -0.7f)), Time.deltaTime * headSpeed);
		}
		else if (horizontalAttack)
		{
			if (head.localPosition.x > -3.79f && !headOnPosition)
			{
				head.localPosition = Vector3.MoveTowards(head.localPosition, Tool2D.IgnoreZPoint(new Vector3(-3.8f, 0f, -0.7f)), Time.deltaTime * headSpeed * 2.2f);
			}
			else
			{
				headOnPosition = true;
				headAnima.Play("HorizontalAttack");
			}
		}
		else
		{
			head.localPosition = Vector3.MoveTowards(head.localPosition, Tool2D.IgnoreZPoint(new Vector3(-1f, 0f, -0.7f)), Time.deltaTime * headSpeed);
		}
		for (int i = 0; i < neckSegments.Length; i++)
		{
			float t = (float)(i + 1) / (float)(neckSegments.Length + 1);
			Vector3 vector = BezierQuadratic(head.position, neckControl.position, body.position, t);
			neckSegments[i].position = vector - new Vector3(0f, 0f, 0.01f * (float)i);
		}
		for (int j = 0; j < leftArmSegments.Length; j++)
		{
			float num2 = (float)(j + 1) / (float)(leftArmSegments.Length + 1);
			Vector3 vector2 = BezierQuadratic(leftHand.position, leftArmControl.position, leftArmRoot.position, num2);
			leftArmSegments[j].position = vector2 - new Vector3(0f, 0f, 0.05f * (float)j);
			if (j < leftArmSegments.Length - 1)
			{
				Vector3 v = BezierQuadratic(leftHand.position, leftArmControl.position, leftArmRoot.position, num2 + 0.1f) - vector2;
				leftArmSegments[j].right = Tool2D.IgnoreZPoint(v);
			}
			vector2 = BezierQuadratic(rightHand.position, rightArmControl.position, rightArmRoot.position, num2);
			rightArmSegments[j].position = vector2 - new Vector3(0f, 0f, 0.05f * (float)j);
			if (j < rightArmSegments.Length - 1)
			{
				Vector3 v2 = BezierQuadratic(rightHand.position, rightArmControl.position, rightArmRoot.position, num2 + 0.1f) - vector2;
				rightArmSegments[j].right = Tool2D.IgnoreZPoint(v2);
			}
		}
		if (base.IsLocked)
		{
			return;
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornIdle:
			_ = changedState;
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Phase_1_Idle;
				CameraFocusSizeData data = new CameraFocusSizeData(extraView, 1, 1000000f);
				CamController.Inst.AddNewCameraFocusRequirement(data);
			}
			break;
		case MonsterState.Phase_1_Idle:
		{
			_ = changedState;
			if (!canAttack)
			{
				break;
			}
			attackCDTimer += Time.deltaTime;
			if (!(attackCDTimer > attackCD.result))
			{
				break;
			}
			int num3 = Random.Range(0, 10);
			if (!actionList.Contains(num3))
			{
				break;
			}
			attackCDTimer = 0f;
			attackCD.RandomResult();
			canAttack = false;
			actionList.Remove(num3);
			switch (num3)
			{
			case 0:
				armsAnima.Play("DoubleCircle");
				break;
			case 1:
				headFollow = false;
				horizontalAttack = true;
				headOnPosition = false;
				break;
			case 2:
				headAnima.Play("VerticalAttack");
				horizontalAttack = false;
				breathLengthTimer = 0f;
				break;
			case 3:
				headFollow = false;
				horizontalAttack = false;
				boom = true;
				armsAnima.Play("Rocket");
				break;
			case 4:
				armsAnima.Play("UziStrafe");
				break;
			case 5:
				wingsAnima.Play("FallRightAngle");
				break;
			case 6:
				headAnima.Play("FollowSkull");
				break;
			case 7:
				armsAnima.Play("BounceKin");
				break;
			case 8:
				if (LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count == 1)
				{
					armsAnima.Play("ThrowKnife");
				}
				else
				{
					canAttack = true;
				}
				break;
			case 9:
				armsAnima.Play("DoubleArc");
				break;
			}
			if (actionList.Count == 0)
			{
				for (int k = 0; k < 10; k++)
				{
					actionList.Add(k);
				}
			}
			break;
		}
		case MonsterState.Phase_2_Idle:
			_ = changedState;
			break;
		case MonsterState.ChangePhase:
			_ = changedState;
			break;
		}
	}

	private Vector3 BezierQuadratic(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		float num = 1f - t;
		return num * num * p0 + 2f * num * t * p1 + t * t * p2;
	}

	public SpellBase CreateBullet(string bulletName, Vector3 shootDir, float speed, Vector3 position)
	{
		switch (bulletName)
		{
		case "Fire":
		{
			fireBullet.shootDirection = shootDir;
			fireBullet.spelldataConfig.speed = speed;
			SpellBase component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + fireBullet.spelldataConfig.prefab, new Vector3(0f, 0f, 0f - fireSpellHeight) + position).GetComponent<SpellBase>();
			component2.Initialize(fireBullet);
			return component2;
		}
		case "Bullet":
		{
			kinBullet.shootDirection = shootDir;
			kinBullet.spelldataConfig.speed = speed;
			SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + kinBullet.spelldataConfig.prefab, new Vector3(0f, 0f, 0f - kinSpellHeight) + position).GetComponent<SpellBase>();
			component.Initialize(kinBullet);
			return component;
		}
		case "Skull":
			ObjPoolMgr.Inst.GetGO("Prefabs/Units/500831", new Vector3(0f, 0f, 0f - skullSpellHeight) + position).GetComponent<Boss8_SkullBullet>().Init(shootDir);
			return null;
		default:
			Debug.LogError("没有这个子弹");
			return null;
		}
	}

	public void DoubleCircleAttack(int attackIndex)
	{
		SEMgr.Inst.monster12Land.PlaySE();
		switch (attackIndex)
		{
		case 0:
		{
			for (int num2 = 0; num2 < doubleCircleAmount; num2++)
			{
				CreateBullet("Fire", Tool2D.GetDir(Vector3.up, (float)num2 * 360f / (float)doubleCircleAmount + 180f / (float)doubleCircleAmount), fireSpellSpeed, rightGunPivot.position);
			}
			break;
		}
		case 1:
		{
			for (int l = 0; l < doubleCircleAmount; l++)
			{
				CreateBullet("Fire", Tool2D.GetDir(Vector3.up, (float)l * 360f / (float)doubleCircleAmount), fireSpellSpeed - 1f, rightGunPivot.position);
			}
			break;
		}
		case 2:
		{
			for (int n = 0; n < frontArcAmount; n++)
			{
				CreateBullet("Fire", Tool2D.GetDir(Tool2D.GetDir(Vector3.left, 10f), ((float)n + 0.5f) * 160f / (float)behindArcAmount), fireSpellSpeed, rightGunPivot.position);
			}
			break;
		}
		case 3:
		{
			for (int j = 0; j < behindArcAmount; j++)
			{
				CreateBullet("Fire", Tool2D.GetDir(Tool2D.GetDir(Vector3.left, 10f), ((float)j + 0.5f) * 160f / (float)behindArcAmount), fireSpellSpeed - 1f, rightGunPivot.position);
			}
			break;
		}
		case 4:
		{
			for (int num = 0; num < doubleCircleAmount; num++)
			{
				CreateBullet("Fire", Tool2D.GetDir(Vector3.up, (float)num * 360f / (float)doubleCircleAmount + 180f / (float)doubleCircleAmount), fireSpellSpeed, leftGunPivot.position);
			}
			break;
		}
		case 5:
		{
			for (int m = 0; m < doubleCircleAmount; m++)
			{
				CreateBullet("Fire", Tool2D.GetDir(Vector3.up, (float)m * 360f / (float)doubleCircleAmount), fireSpellSpeed - 1f, leftGunPivot.position);
			}
			break;
		}
		case 6:
		{
			for (int k = 0; k < frontArcAmount; k++)
			{
				CreateBullet("Fire", Tool2D.GetDir(Tool2D.GetDir(Vector3.left, 10f), ((float)k + 0.5f) * 160f / (float)behindArcAmount), fireSpellSpeed, leftGunPivot.position);
			}
			break;
		}
		case 7:
		{
			for (int i = 0; i < behindArcAmount; i++)
			{
				CreateBullet("Fire", Tool2D.GetDir(Tool2D.GetDir(Vector3.left, 10f), ((float)i + 0.5f) * 160f / (float)behindArcAmount), fireSpellSpeed - 1f, leftGunPivot.position);
			}
			break;
		}
		}
	}

	public void BreathAttack()
	{
		if (!breath)
		{
			return;
		}
		if (horizontalAttack)
		{
			breathTimer += Time.deltaTime;
			if (breathTimer > horizontalShootCD.RandomResult())
			{
				breathTimer = 0f;
				for (int i = 0; i < Random.Range(3, 6); i++)
				{
					CreateBullet("Fire", Tool2D.GetDir(-headParent.transform.up, horizontalOffset.RandomResult()), fireSpellSpeed + Random.Range(-1f, 1f), mouth.position + new Vector3(bulletPositionXOffset.RandomResult(), 0f, 0f));
				}
			}
			return;
		}
		breathTimer += Time.deltaTime;
		breathLengthTimer += Time.deltaTime;
		if (breathTimer > verticalShootCD.RandomResult())
		{
			breathTimer = 0f;
			for (int j = 0; j < Random.Range(3, 6); j++)
			{
				CreateBullet("Fire", Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, head.position - new Vector3(0f, 0.2f, 0f)), verticalOffset.RandomResult()), fireSpellSpeed + Random.Range(-1f, 1f), mouth.position + new Vector3(bulletPositionXOffset.RandomResult(), 0f, 0f));
			}
		}
		if (breathLengthTimer > breathLength)
		{
			breath = false;
			headAnima.Play("HeadIdle");
			canAttack = true;
			horizontalAttack = false;
		}
	}

	public void RocketAttack(float angleOffset)
	{
		for (int i = 0; i < circleAmount; i++)
		{
			fireBullet.shootDirection = Tool2D.GetDir(Vector3.left, (float)(-i * 180 / (circleAmount - 1)) + angleOffset);
			fireBullet.spelldataConfig.speed = fireSpellSpeed;
			ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + fireBullet.spelldataConfig.prefab, new Vector3(0f, 0f, 0f - fireSpellHeight) + rocketTransform.position).GetComponent<SpellBase>().Initialize(fireBullet);
		}
		if (boom)
		{
			ExplodeOnce(new Vector3(rocketTransform.position.x, rocketTransform.position.y, 0f));
			boom = false;
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster104_ExplosionSingle", explodePoint, 6f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 300f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.monster34Explosion.PlaySE();
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(explodePoint, boomRadius, "Monster", "Destructible", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness", "Player", "Teammate");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			if (collidersByTag[i].tag == "Spell" || collidersByTag[i].tag == "RollBall" || collidersByTag[i].tag == "Butterfly")
			{
				if (!collidersByTag[i].gameObject.activeInHierarchy)
				{
					continue;
				}
				SpellBase componentInParent = collidersByTag[i].GetComponentInParent<SpellBase>();
				if (componentInParent.spellCfg.abilityType != SpellAbilityType.FireBall)
				{
					if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
					{
						((Spell1002RollBall)componentInParent).TakeDamage(boomDamage);
					}
					else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
					{
						((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
					}
				}
			}
			else
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
				takeDamageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(collidersByTag[i].transform.position, explodePoint) * knockback;
				takeDamageInfo.playerTakeDamageRatio = playerDamageRatio;
				collidersByTag[i].GetComponent<UnitProperty>().TakeDamage(boomDamage, null, takeDamageInfo);
			}
		}
	}

	public void FallRightAngle()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < rightAngleAmount; i++)
		{
			SpellBase spellBase = CreateBullet("Fire", Vector3.down, fireSpellSpeed, mouth.position);
			spellBase.isThroughWall = true;
			list.Add(spellBase.gameObject);
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss8_BulletsMgr", base.transform.position - new Vector3(0f, 0f, 0.3f)).GetComponent<Boss8_BulletsMgr>().Initialized(list, leftPivot, rightPivot);
	}

	public void FollowSkull()
	{
		for (int i = 0; i < skullAmount; i++)
		{
			CreateBullet("Skull", Tool2D.GetDir(Tool2D.GetDir(Vector3.left, 10f), ((float)i + 0.5f) * 160f / (float)skullAmount), skullSpellSpeed, new Vector3(mouth.position.x, mouth.position.y, 0f));
		}
	}

	public void BounceKinAttack(int actionIndex)
	{
		SEMgr.Inst.monster12Land.PlaySE();
		switch (actionIndex)
		{
		case 0:
		{
			for (int l = 0; l < kinAmount; l++)
			{
				CreateBullet("Bullet", Tool2D.GetDir(Tool2D.GetDir(Vector3.up, 30f), ((float)l + 0.5f) * 85f / (float)kinAmount), kinSpellSpeed, rightGunPivot.position).rebounceTime = Random.Range(1, 3);
			}
			break;
		}
		case 1:
		{
			for (int j = 0; j < kinAmount; j++)
			{
				CreateBullet("Bullet", Tool2D.GetDir(Tool2D.GetDir(Vector3.left, 45f), ((float)j + 0.5f) * 90f / (float)kinAmount), kinSpellSpeed, rightGunPivot.position).rebounceTime = Random.Range(1, 3);
			}
			break;
		}
		case 2:
		{
			for (int k = 0; k < kinAmount; k++)
			{
				CreateBullet("Bullet", Tool2D.GetDir(Tool2D.GetDir(Vector3.left, 45f), ((float)k + 0.5f) * 90f / (float)kinAmount), kinSpellSpeed, leftGunPivot.position).rebounceTime = Random.Range(1, 3);
			}
			break;
		}
		case 3:
		{
			for (int i = 0; i < kinAmount; i++)
			{
				CreateBullet("Bullet", Tool2D.GetDir(Tool2D.GetDir(Vector3.up, -30f), ((float)i + 0.5f) * -85f / (float)kinAmount), kinSpellSpeed, leftGunPivot.position).rebounceTime = Random.Range(1, 3);
			}
			break;
		}
		}
	}

	public void GenerateKnife(int actionIndex)
	{
		if (actionIndex == 0)
		{
			GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/Units/500821", Tool2D.IgnoreZPoint(leftKnifePivot.position));
			gO.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
			gO.transform.localScale = new Vector3(-1f, 1f, 1f);
			gO.transform.position -= new Vector3(0.094f, 0f, 0f);
		}
		else
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Units/500821", Tool2D.IgnoreZPoint(rightKnifePivot.position)).transform.rotation = Quaternion.Euler(0f, 0f, 30f);
		}
	}

	public void DoubleArc()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		for (int i = 0; i < doubleArcAmount; i++)
		{
			CreateBullet("Fire", Tool2D.GetDir(Tool2D.GetDir(Vector3.left, 20f), ((float)i + 0.5f) * 140f / (float)doubleArcAmount), fireSpellSpeed, leftGunPivot.position);
		}
		for (int j = 0; j < doubleArcAmount; j++)
		{
			CreateBullet("Fire", Tool2D.GetDir(Tool2D.GetDir(Vector3.left, 20f), ((float)j + 0.5f) * 140f / (float)doubleArcAmount), fireSpellSpeed, rightGunPivot.position);
		}
	}

	public void SetArmsOutside()
	{
		Transform[] array = leftArmSegments;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		array = rightArmSegments;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		leftHand.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		rightHand.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
	}

	public void SetArmsNone()
	{
		Transform[] array = leftArmSegments;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
		}
		array = rightArmSegments;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
		}
		leftHand.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
		rightHand.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
	}

	private IEnumerator SpawnBulletRainWithCurvePath()
	{
		float distance = Tool2D.IgnoreZDistance(leftSide, rightSide);
		List<int> safePath = new List<int> { Random.Range(0, bulletHorizontalRainAmount) };
		for (int j = 1; j < bulletVerticalRainAmount; j++)
		{
			int value = safePath[j - 1] + (Random.Range(0, 2) * 2 - 1);
			value = Mathf.Clamp(value, 0, bulletHorizontalRainAmount - 1);
			safePath.Add(value);
		}
		for (int i = 0; i < bulletVerticalRainAmount; i++)
		{
			for (int k = 0; k < bulletHorizontalRainAmount; k++)
			{
				if (safePath[i] != k && safePath[i] != k + 1 && safePath[i] != k - 1)
				{
					CreateBullet("Fire", Vector3.down, fireSpellSpeed - 2.5f, leftSide.position + new Vector3((float)k * (distance / (float)(bulletHorizontalRainAmount - 1)) + Random.Range(-0.2f, 0.2f), 0f, 0f));
				}
			}
			yield return new WaitForSeconds(bulletRainInterval);
		}
	}
}
