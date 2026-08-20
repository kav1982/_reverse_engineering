using System.Collections.Generic;
using UnityEngine;

public class Monster36_ToxicFog : MonoBehaviour
{
	public float fartInterval;

	private float fartTimer;

	public int damage;

	public float radius;

	public int venomStack;

	public float effectDuration;

	public float hurtTime;

	private float existTimer;

	public VariableFloat speed;

	private float trueSpeed;

	public float speedLerp;

	public Vector3 moveDir;

	public Vector3 inheritSpeed;

	public float inheritSpeedValue;

	private float trueInheritSpeed;

	private void OnEnable()
	{
		fartTimer = 0f;
		existTimer = 0f;
		trueSpeed = speed.RandomResult();
		trueInheritSpeed = inheritSpeedValue;
	}

	private void Start()
	{
	}

	private void Update()
	{
		trueInheritSpeed = Mathf.Lerp(inheritSpeedValue, 0f, speedLerp * Time.deltaTime);
		trueSpeed = Mathf.Lerp(trueSpeed, 0f, speedLerp * Time.deltaTime);
		base.transform.position += trueSpeed * Time.deltaTime * moveDir;
		base.transform.position += inheritSpeed * trueInheritSpeed * Time.deltaTime;
		existTimer += Time.deltaTime;
		if (existTimer > hurtTime)
		{
			return;
		}
		fartTimer += Time.deltaTime;
		if (!(fartTimer >= fartInterval))
		{
			return;
		}
		fartTimer = 0f;
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, radius, "Player", "Teammate", "Destructible", "Spell", "RollBall", "Butterfly", "Brittleness");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			if (collidersByTag[i].tag == "Spell" || collidersByTag[i].tag == "RollBall" || collidersByTag[i].tag == "Butterfly")
			{
				SpellBase componentInParent = collidersByTag[i].GetComponentInParent<SpellBase>();
				if (!componentInParent.IsSameCamp(UnitType.Monster))
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
			else
			{
				UnitProperty component = collidersByTag[i].GetComponent<UnitProperty>();
				component.TakeDamage(damage, AttackerType.NothingSpecial);
				component.SetVenom(effectDuration, venomStack);
			}
		}
	}
}
