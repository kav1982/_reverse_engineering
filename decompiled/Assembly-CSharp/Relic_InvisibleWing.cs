using System.Collections;
using UnityEngine;

public class Relic_InvisibleWing : MonoBehaviour
{
	public GameObject go_Active;

	public SpriteRenderer sr;

	public Sprite sprite_Fram1;

	public Sprite sprite_Fram2;

	public float framChanceInterval;

	private bool isFly;

	private RelicConfig relicCfg;

	private float framChanceIntervalTimer;

	public float ExtraMoveSpeed
	{
		get
		{
			if (isFly)
			{
				return (float)relicCfg.int1.result / 100f;
			}
			return 0f;
		}
	}

	public void Intialize(RelicConfig relicCfg)
	{
		this.relicCfg = relicCfg;
	}

	private void Start()
	{
		go_Active.SetActive(value: false);
	}

	private void Update()
	{
		if (go_Active.activeSelf && !PlayerMgr.Inst.ItemCtrller.potion_Petrifaction)
		{
			framChanceIntervalTimer += Time.deltaTime;
			if (framChanceIntervalTimer >= framChanceInterval)
			{
				framChanceIntervalTimer = 0f;
				sr.sprite = ((sr.sprite == sprite_Fram1) ? sprite_Fram2 : sprite_Fram1);
			}
			if (PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint.y > PlayerMgr.Inst.PlayerPoint.y)
			{
				go_Active.transform.position = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, 0f, -0.4f);
			}
			else
			{
				go_Active.transform.position = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, 0f, 0.4f);
			}
		}
	}

	public void PlayerTakeDamage()
	{
		if (!isFly)
		{
			isFly = true;
			PlayerMgr.Inst.FlyRegister();
			go_Active.SetActive(value: true);
			Update();
		}
	}

	public void EnterDoor()
	{
		if (isFly)
		{
			isFly = false;
			PlayerMgr.Inst.FlyUnregister();
			go_Active.SetActive(value: false);
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		if (go_Active.activeSelf)
		{
			Update();
		}
	}

	public void PointerToPlayer()
	{
		if (go_Active.activeSelf)
		{
			Update();
		}
	}

	public void PointerToPlayerThrougPotionPetrifaction()
	{
		if (go_Active.activeSelf)
		{
			StartCoroutine(PointerToPlayerThrougPotionPetrifactionIE());
		}
	}

	private IEnumerator PointerToPlayerThrougPotionPetrifactionIE()
	{
		yield return null;
		Update();
	}

	public void DestroySelf()
	{
		if (isFly)
		{
			PlayerMgr.Inst.FlyUnregister();
		}
		Object.Destroy(base.gameObject);
	}
}
