using System.Collections.Generic;
using UnityEngine;

public class Elite8_Star : MonoBehaviour
{
	public int damage;

	public float lifeTime;

	public float lifeTimer;

	public float destoryTime;

	public Vector3 diration;

	public float speed;

	public Rigidbody Rigid;

	public Collider Collider;

	public SpriteRenderer mainImage;

	public ParticleSystem trailParticle;

	public ParticleSystem explodeParticle;

	public Monster26 master;

	private bool startDisappear;

	public List<ParticleSystem> allParticles = new List<ParticleSystem>();

	public VariableFloat rotateSpeedRange;

	private float rotateSpeed;

	public float gravity = -10f;

	public float CurrentUpSpeed = 3f;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private string[] triggerLayers = new string[8] { "Destructible", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness", "Player", "Teammate" };

	private void Start()
	{
		rotateSpeedRange.RandomResult();
		rotateSpeed = rotateSpeedRange.result;
		rotateSpeed *= ((Random.Range(0, 2) == 0) ? 1 : (-1));
		Rigid.linearVelocity = diration * speed;
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
	}

	private void Update()
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
		if (!(base.transform.position.z > 0f))
		{
			return;
		}
		base.transform.position = Tool2D.IgnoreZPoint(base.transform);
		CurrentUpSpeed *= -1f;
		if (master != null && !startDisappear)
		{
			master.BoneBlast(base.transform.position);
			for (int i = 0; i < allParticles.Count; i++)
			{
				allParticles[i].Play();
			}
		}
		lifeTimer += Time.deltaTime;
		if (lifeTimer > lifeTime && !startDisappear)
		{
			startDisappear = true;
			mainImage.enabled = false;
			for (int j = 0; j < allParticles.Count; j++)
			{
				allParticles[j].Play();
			}
			Collider.enabled = false;
		}
		if (lifeTimer > destoryTime)
		{
			Object.Destroy(base.gameObject);
		}
		if (!startDisappear)
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

	private void OnTriggerEnter(Collider other)
	{
		bool flag = false;
		for (int i = 0; i < triggerLayers.Length; i++)
		{
			if (triggerLayers[i] == other.tag)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		if (other.tag == "Spell" || other.tag == "RollBall" || other.tag == "Butterfly")
		{
			if (!other.gameObject.activeInHierarchy)
			{
				return;
			}
			SpellBase componentInParent = other.GetComponentInParent<SpellBase>();
			if (componentInParent.spellCfg.abilityType != SpellAbilityType.FireBall)
			{
				if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
				{
					((Spell1002RollBall)componentInParent).TakeDamage(damage);
				}
				else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
				{
					((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
				}
			}
		}
		else if (other.CompareTag("Player"))
		{
			if (other.IsPlayerTrigger())
			{
				DoDamage();
			}
		}
		else
		{
			DoDamage();
		}
		void DoDamage()
		{
			other.GetComponent<UnitProperty>().TakeDamage(damage, null, new TakeDamageInfo
			{
				knockbackForce = Vector3.zero
			});
			explodeParticle.Play();
		}
	}
}
