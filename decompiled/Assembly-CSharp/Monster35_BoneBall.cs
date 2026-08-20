using UnityEngine;

public class Monster35_BoneBall : LayerCorrect
{
	[Space(50f)]
	public float horizontalSpeed;

	public float gravity;

	public float upSpeed;

	public float upSpeedDistanceFixer;

	public Rigidbody rigid;

	public float destroyDelayTime;

	public Transform tsf_Rotate;

	public float rotateSpeed;

	public GameObject go_GroundEF;

	public Shadow shadow;

	public Monster35 master;

	private Vector3 landPoint;

	private float destroyDelayTimer;

	private bool isLand;

	private float currentZSpeed;

	public int spellDamage;

	public float spellGravity;

	public float spellCount;

	public VariableFloat spellSpeed;

	public VariableFloat spellUpSpeed;

	private SpellSpawnParams ssp;

	[Header("和谐模式")]
	public SpriteRenderer sr_Bone;

	public SpriteRenderer sr_Border;

	public Sprite sprite_H;

	public ParticleSystem ps;

	public ParticleSystem psH;

	public ParticleSystem ps_Drop;

	public ParticleSystem ps_DropH;

	private void Start()
	{
		float num = Tool2D.IgnoreZDistance(base.transform.position, landPoint);
		upSpeed += num * upSpeedDistanceFixer;
		horizontalSpeed = GeneralTool.CannonSpeed(upSpeed, 0f - base.transform.position.z, gravity, num);
		rigid.linearVelocity = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position) * horizontalSpeed;
		currentZSpeed = upSpeed;
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90021);
		UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.CurrentFallSpeed = 0f - upSpeed;
		sSPModifier.Gravity = 0f - gravity;
		sSPModifier.Shooter = master.myPpt.myEntity;
		sSPModifier.ReboundCount = 100;
		sSPModifier.ApplyToSSP(ref ssp);
		if (GameMgr.IsMobile_Static)
		{
			spellCount *= 0.6f;
		}
	}

	private void Update()
	{
		if (isLand)
		{
			destroyDelayTimer += Time.deltaTime;
			if (destroyDelayTimer > destroyDelayTime)
			{
				Object.Destroy(base.gameObject);
			}
			return;
		}
		currentZSpeed += gravity * Time.deltaTime;
		base.transform.position -= new Vector3(0f, 0f, currentZSpeed) * Time.deltaTime;
		tsf_Rotate.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
		if (base.transform.position.z > 0f)
		{
			SEMgr.Inst.monster26BoneLand.PlaySE();
			BoneBlast(base.transform.position);
			isLand = true;
			rigid.linearVelocity = Vector3.zero;
			tsf_Rotate.gameObject.SetActive(value: false);
			go_GroundEF.SetActive(value: true);
			shadow.Hide();
		}
	}

	public void Iniaitlize(Vector3 landPoint, Monster35 master)
	{
		this.landPoint = landPoint;
		this.master = master;
		if (GameMgr.IsHarmony_Static)
		{
			sr_Bone.sprite = sprite_H;
			sr_Border.sprite = sprite_H;
			sr_Border.material.color = Color.magenta;
			ps = psH;
			ps_Drop = ps_DropH;
		}
		else
		{
			sr_Border.material.color = Color.red;
		}
		ps.Play();
	}

	public void BoneBlast(Vector3 point)
	{
		ps_Drop.Play();
		UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		for (int i = 0; (float)i < spellCount; i++)
		{
			Vector3 normalized = Tool2D.GetDir().normalized;
			Vector3 v = point + normalized * Random.Range(0f, 1f);
			sSPModifier.Speed = spellSpeed.RandomResult();
			sSPModifier.CurrentFallSpeed = 0f - spellUpSpeed.RandomResult();
			sSPModifier.Direction = normalized;
			sSPModifier.SpawnPosition = Tool2D.IgnoreZPoint(v, -0.1f);
			sSPModifier.ApplyToSSP(ref ssp);
			UnitDotsSyncSystem.ShootSpell(ssp);
		}
	}
}
