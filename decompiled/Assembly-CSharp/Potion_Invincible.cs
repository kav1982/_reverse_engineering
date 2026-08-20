using System.Linq;
using Unity.Entities;
using UnityEngine;

public class Potion_Invincible : LayerCorrect
{
	[Space(50f)]
	public float warningTime;

	public float twinkleInterval;

	public Color color_PlayerSR;

	private SpriteRenderer[] playerSprites;

	private float duration;

	private float durationTimer;

	private bool isUnregisterInvincible;

	private bool isWarning;

	private float twinkleIntervalTimer;

	private void Update()
	{
		durationTimer += PlayerMgr.Inst.PlayerDeltaTime;
		if (durationTimer >= duration)
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			UnitProperty_Dots componentData = entityManager.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
			componentData.InvincibleUnregister();
			entityManager.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
			foreach (SpriteRenderer item in playerSprites.Where((SpriteRenderer e) => e))
			{
				item.color = Color.white;
			}
			Object.Destroy(base.gameObject);
			return;
		}
		if (!isWarning && durationTimer >= duration - warningTime)
		{
			isWarning = true;
			PlayerMgr.Inst.PlayerPpt.ChangeColor(Color.white);
			tsf_Layer.gameObject.SetActive(value: false);
		}
		if (!isWarning)
		{
			return;
		}
		twinkleIntervalTimer += PlayerMgr.Inst.PlayerDeltaTime;
		if (!(twinkleIntervalTimer >= twinkleInterval))
		{
			return;
		}
		twinkleIntervalTimer = 0f;
		if (playerSprites[0].color == Color.white)
		{
			for (int i = 0; i < playerSprites.Length; i++)
			{
				playerSprites[i].color = color_PlayerSR;
			}
			tsf_Layer.gameObject.SetActive(value: true);
		}
		else
		{
			for (int j = 0; j < playerSprites.Length; j++)
			{
				playerSprites[j].color = Color.white;
			}
			tsf_Layer.gameObject.SetActive(value: false);
		}
	}

	public void Initialize(float duration)
	{
		this.duration = Mathf.Max(this.duration - durationTimer, duration);
		durationTimer = 0f;
		isWarning = false;
		if (!isUnregisterInvincible)
		{
			isUnregisterInvincible = true;
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			UnitProperty_Dots componentData = entityManager.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
			componentData.InvincibleRegister();
			entityManager.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
		}
		playerSprites = PlayerMgr.Inst.PlayerT.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < playerSprites.Length; i++)
		{
			playerSprites[i].color = color_PlayerSR;
		}
	}
}
