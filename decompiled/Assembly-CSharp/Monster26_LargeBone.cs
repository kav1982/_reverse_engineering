using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Monster26_LargeBone : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public int damage;

	public float lifeTime;

	public float lifeTimer;

	public float destoryTime;

	public Vector3 diration;

	public float speed;

	public Rigidbody rigid;

	public UnityEngine.CapsuleCollider thisCollider;

	public Monster26 master;

	public Entity masterEntity;

	private bool affect_InAbyss;

	private Vector3 affect_AbyssPoint;

	private bool startDisappear;

	public float fallInCliffDisntace;

	public ParticleSystem dropParticles;

	public ParticleSystem dropParticles_H;

	public VariableFloat rotateSpeedRange;

	private float rotateSpeed;

	public float gravity = -10f;

	public float CurrentUpSpeed = 3f;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public Shadow thisShadow;

	[Header("\ufffd\ufffdгģʽ")]
	public SpriteRenderer sr_Bone;

	public SpriteRenderer sr_Border;

	public Sprite sprite_boneH;

	public ParticleSystem trailParticle_H;

	public ParticleSystem trailParticle;

	public Shadow thisShadow_H;

	private string[] triggerLayers = new string[8] { "Destructible", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness", "Player", "Teammate" };

	private List<UnitDotsSyncSystem.DistanceHitResult> abyssCheckResult = new List<UnitDotsSyncSystem.DistanceHitResult>();

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		rigid = GetComponent<Rigidbody>();
		thisCollider = GetComponent<UnityEngine.CapsuleCollider>();
		thisShadow = GetComponent<Shadow>();
		thisShadow.ShadowGO.transform.parent = GetComponent<LayerCorrect>().tsf_Layer;
		rigid.linearVelocity = diration * speed;
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
	}

	public override void OnEnable()
	{
		base.transform.localScale = Vector3.one;
		startDisappear = false;
		affect_InAbyss = false;
		lifeTimer = 0f;
		rotateSpeedRange.RandomResult();
		rotateSpeed = rotateSpeedRange.result;
		rotateSpeed *= ((Random.Range(0, 2) == 0) ? 1 : (-1));
		rigid.linearVelocity = diration * speed;
		if (GameMgr.IsHarmony_Static)
		{
			sr_Bone.sprite = sprite_boneH;
			sr_Border.sprite = sprite_boneH;
			sr_Border.material.color = Color.magenta;
			thisShadow.CreateShadow();
			thisShadow.Hide();
			dropParticles = dropParticles_H;
			trailParticle.Stop();
		}
		else
		{
			sr_Border.material.color = Color.red;
			thisShadow_H.CreateShadow();
			thisShadow_H.Hide();
			trailParticle_H.Stop();
		}
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 4608u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (!affect_InAbyss)
		{
			if (gravity != 0f)
			{
				CurrentUpSpeed += gravity * Time.deltaTime;
			}
			if (CurrentUpSpeed != 0f)
			{
				base.transform.position -= new Vector3(0f, 0f, CurrentUpSpeed) * Time.deltaTime;
			}
			base.transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
			thisShadow.ShadowGO.transform.localEulerAngles = Vector3.zero;
		}
		if (affect_InAbyss)
		{
			if (base.transform.position != affect_AbyssPoint)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, affect_AbyssPoint, 4f * Time.deltaTime);
			}
			float num = base.transform.localScale.x - Time.deltaTime;
			if (num < 0f)
			{
				Recycle();
			}
			else
			{
				base.transform.localScale = Vector3.one * num;
			}
		}
		if (base.transform.position.z > 0f && CurrentUpSpeed < 0f)
		{
			if (!affect_InAbyss)
			{
				abyssCheckResult.Clear();
				CollisionFilter collisionFilter = default(CollisionFilter);
				collisionFilter.CollidesWith = 1024u;
				collisionFilter.BelongsTo = uint.MaxValue;
				collisionFilter.GroupIndex = 0;
				CollisionFilter filter = collisionFilter;
				UnitDotsSyncSystem.GetCollidersInRange(Tool2D.IgnoreZPoint(base.transform), thisCollider.radius * base.transform.localScale.x, filter, abyssCheckResult);
				if (abyssCheckResult.Count > 0)
				{
					affect_InAbyss = true;
					affect_AbyssPoint = Tool2D.IgnoreZPoint(abyssCheckResult[0].point);
					rigid.linearVelocity = Vector3.zero;
				}
			}
			if ((Tool2D.GetNavMeshPointIngoreZ(base.transform.position, 8) - base.transform.position).sqrMagnitude > fallInCliffDisntace * fallInCliffDisntace)
			{
				affect_InAbyss = true;
				affect_AbyssPoint = Tool2D.IgnoreZPoint(base.transform.position);
				rigid.linearVelocity = Vector3.zero;
			}
			if (!affect_InAbyss)
			{
				SEMgr.Inst.monster26BoneLand.PlaySE();
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				CurrentUpSpeed *= -1f;
				if (master != null && !startDisappear)
				{
					master.BoneBlast(base.transform.position);
					dropParticles.Play();
				}
			}
		}
		lifeTimer += Time.deltaTime;
		if (lifeTimer > lifeTime && !startDisappear)
		{
			startDisappear = true;
			dropParticles.Play();
			thisCollider.enabled = false;
		}
		if (startDisappear)
		{
			float num2 = base.transform.localScale.x - Time.deltaTime;
			if (num2 < 0f)
			{
				Recycle();
			}
			else
			{
				base.transform.localScale = Vector3.one * num2;
			}
		}
		if (!startDisappear && (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1))
		{
			if (base.transform.position.x > roomCenterPoint.x + roomWidth / 2f)
			{
				lifeTimer = lifeTime;
			}
			else if (base.transform.position.x < roomCenterPoint.x - roomWidth / 2f)
			{
				lifeTimer = lifeTime;
			}
			if (base.transform.position.y > roomCenterPoint.y + roomHeight / 2f)
			{
				lifeTimer = lifeTime;
			}
			else if (base.transform.position.y < roomCenterPoint.y - roomHeight / 2f)
			{
				lifeTimer = lifeTime;
			}
		}
	}

	private void Recycle()
	{
		base.gameObject.SetActive(value: false);
		lifeTimer = 0f;
		base.transform.localScale = Vector3.one;
		startDisappear = false;
		affect_InAbyss = false;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		if (layer == 512 || layer == 2097152)
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
			info.damage = damage;
			info.teammateTakeDamageRatio = 2f;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
