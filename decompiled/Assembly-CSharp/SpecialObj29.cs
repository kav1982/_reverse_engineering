using System;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj29 : LayerCorrect, IRoomObjExtraData, ITrap, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	public static int so29ConcurrenceCounter;

	[Space(50f)]
	public Transform tsf_SR2;

	public Transform tsf_LayerPointer;

	public Transform tsf_LayerPointerParticle;

	public Transform tsf_LayerPointerShadow;

	public float pointerHeight;

	public UnityEngine.Collider thisCollider;

	[Header("Scale")]
	public Transform tsf_Scale;

	public float pointerLength;

	[Header("Rotate")]
	public Transform tsf_PointerRotate;

	public Transform tsf_PointerParticleRotate;

	public float rotateSpeed;

	[Header("Damage")]
	public LayerMask attackLayer;

	public float damageInterval;

	public float damageWidth;

	public int damageForPlayer;

	[Header("Dead")]
	public Transform tsf_SR;

	public ParticleSystem PSSmoke;

	public Transform tsf_PSDead;

	[Header("Other")]
	public AudioSource as_Loop;

	public ParticleSystem ps;

	public Gradient mobileColorGradient;

	public Sprite mobileSprite;

	public SpriteRenderer sr;

	private float currentDir;

	private float finalRotateSpeed;

	private float damageIntervalTimer;

	private bool isDestroy;

	public Entity thisEntity { get; set; }

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		if (GameMgr.IsChAge14_Static)
		{
			sr.sprite = mobileSprite;
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = ps.colorOverLifetime;
			colorOverLifetime.color = new ParticleSystem.MinMaxGradient(mobileColorGradient);
		}
		so29ConcurrenceCounter++;
		if (so29ConcurrenceCounter == 1)
		{
			as_Loop.Play();
		}
		else
		{
			as_Loop.Stop();
		}
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 256u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(256u);
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		so29ConcurrenceCounter--;
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void SoundVolumeChange()
	{
		as_Loop.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		tsf_LayerPointer.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - pointerHeight));
		tsf_LayerPointerParticle.transform.position = tsf_LayerPointer.position + new Vector3(0f, 0f, 1f) * 0.01f;
		tsf_LayerPointerShadow.position = Tool2D.IgnoreZPoint(base.transform, 1.05f);
		tsf_Scale.localScale = Vector3.one * pointerLength;
		finalRotateSpeed = ((UnityEngine.Random.Range(0, 2) == 0) ? rotateSpeed : (0f - rotateSpeed));
	}

	private void Update()
	{
		if (isDestroy)
		{
			return;
		}
		currentDir += finalRotateSpeed * Time.deltaTime;
		tsf_PointerRotate.localRotation = Quaternion.Euler(0f, 0f, currentDir);
		tsf_PointerParticleRotate.localRotation = Quaternion.Euler(0f, 0f, currentDir);
		tsf_LayerPointerShadow.localRotation = Quaternion.Euler(0f, 0f, currentDir);
		damageIntervalTimer += Time.deltaTime;
		if (!(damageIntervalTimer >= damageInterval))
		{
			return;
		}
		damageIntervalTimer = 0f;
		UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(base.transform.position, Tool2D.GetDir(currentDir), damageWidth, pointerLength, GameConst.Filter_MonsterAoeUndiffer);
		if (array.Length != 0)
		{
			SEMgr.Inst.so29Hit.PlaySE();
		}
		for (int i = 0; i < array.Length; i++)
		{
			UnitDotsSyncSystem.RayCastHitResult rayCastHitResult = array[i];
			Entity entity = rayCastHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 8388608u:
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damageForPlayer, out var _);
				break;
			}
			case 512u:
			case 2048u:
			case 4096u:
			case 8192u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(rayCastHitResult.entity, out var result))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.isTrapDamage = true;
					info.damage = damageForPlayer;
					info.teammateTakeDamageRatio = 3f;
					if (!result.unitCfg.IsSameCamp(UnitType.Player))
					{
						info.damage *= 15f;
					}
					UnitDotsSyncSystem.AddTakeDamageRequest(rayCastHitResult.entity, info);
					CreateHitEF(rayCastHitResult.point);
				}
				break;
			}
			}
		}
	}

	private void CreateHitEF(Vector3 createPoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_SO29Hit", createPoint, 1f);
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 > 0f)
		{
			pointerLength = data1;
		}
	}

	public void SetTrapInvalid()
	{
		if (!isDestroy)
		{
			isDestroy = true;
			tsf_SR2.localPosition = Vector3.zero;
			tsf_LayerPointerShadow.gameObject.SetActive(value: false);
			tsf_SR.gameObject.SetActive(value: false);
			tsf_PSDead.gameObject.SetActive(value: true);
			PSSmoke.Stop();
			SEMgr.Inst.so29Dead.PlaySE();
			as_Loop.enabled = false;
		}
	}
}
