using System.Collections.Generic;
using UnityEngine;

public class Monster35_Barrier : UnitBase
{
	public Vector3 originRingPosition;

	public int damage;

	public float radius;

	public ParticleSystem dirt;

	public float lifeTime;

	private float lifeTimer;

	public List<LayerCorrect> allTsf = new List<LayerCorrect>();

	public SpriteRenderer dirtSprite;

	public SpriteRenderer dirtSpriteSmall;

	public Sprite[] dirtSprites = new Sprite[4];

	public SpriteRenderer sr_DirtFore;

	public SpriteRenderer sr_DirtBack;

	public SpriteRenderer sr_Spike;

	public Sprite spike_H;

	public override void EveryInitialCallback()
	{
		lifeTimer = 0f;
		base.Anima.Play("Monster35_BarrierAppear");
		myPpt.CanTouch = false;
		dirtSprite.sprite = dirtSprites[Random.Range(0, 4)];
		dirtSpriteSmall.sprite = dirtSprites[Random.Range(0, 4)];
		SetSingleFlip(dirtSprite, Random.value < 0.5f);
		myPpt.RemoveSRFromArray(sr_DirtFore);
		myPpt.RemoveSRFromArray(sr_DirtBack);
		if (GameMgr.IsHarmony_Static)
		{
			sr_Spike.sprite = spike_H;
		}
	}

	public override void Update()
	{
		lifeTimer += Time.deltaTime;
		if (lifeTimer > lifeTime)
		{
			base.Anima.Play("Monster35_BarrierDie");
		}
		base.Update();
	}

	public override void AnimaAction(string animaName)
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		switch (animaName)
		{
		case "Damage":
			componentData.CanTouch = true;
			dirt.Play();
			break;
		case "DamageDone":
			componentData.CanTouch = false;
			break;
		case "End":
			DotsAnnouncedDeath();
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
		SetComponentData(componentData);
	}
}
