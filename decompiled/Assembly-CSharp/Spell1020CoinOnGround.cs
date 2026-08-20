using UnityEngine;

public class Spell1020CoinOnGround : InteractiveObj
{
	public int[] coinTheshhold;

	public Sprite[] coinsSprite;

	public SpriteRenderer currentSprite;

	[Header("财富药水借用效果")]
	public Animator anima;

	public SphereCollider sc_Self;

	public int CoinCount { get; private set; }

	public void Initialize(int coinCount, Vector3 scale, bool isFromPotion = false)
	{
		PlayerMgr.Inst.manaCoinList.Add(this);
		CoinCount = coinCount;
		for (int i = 0; i < coinTheshhold.Length; i++)
		{
			if (CoinCount > coinTheshhold[i])
			{
				currentSprite.sprite = coinsSprite[i];
				break;
			}
		}
		sc_Self.enabled = !isFromPotion;
		if (isFromPotion)
		{
			anima.Play("Slow");
		}
	}

	public override void Interact()
	{
		PlayerMgr.Inst.manaCoinList.Remove(this);
		ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + CoinCount, UITextFloatType.GetCoin, base.transform.position);
		PlayerMgr.Inst.ChangeCoin(CoinCount);
		SEMgr.Inst.itemPick_Coin.PlaySE();
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_ItemPickup", base.transform.position, 1f);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	private void _OnGroundSE()
	{
		sc_Self.enabled = true;
		SEMgr.Inst.itemDropBase.PlaySE();
	}

	private void OnDisable()
	{
		PlayerMgr.Inst.manaCoinList.Remove(this);
	}
}
