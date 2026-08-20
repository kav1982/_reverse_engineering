using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj201Button : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public Animator anima;

	public MeshRenderer mr_Symbol;

	public Sprite[] sprite_Symbols_Original;

	public Sprite[] sprite_Symbols_H;

	public UnityEngine.BoxCollider thisCollider;

	private SpecialObj201 so103;

	private bool isDown;

	private bool isDownDelay;

	private int order;

	private Sprite[] sprite_Symbols
	{
		get
		{
			if (GameMgr.IsHarmony_Static)
			{
				return sprite_Symbols_H;
			}
			return sprite_Symbols_Original;
		}
	}

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	public void Initialize(SpecialObj201 so103, int order, int symbolIndex)
	{
		this.so103 = so103;
		this.order = order;
		mr_Symbol.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Symbols[symbolIndex].texture);
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Wrong()
	{
		if (isDown)
		{
			anima.SetTrigger("DownWrongDirect");
			return;
		}
		isDown = true;
		anima.SetTrigger("Down");
	}

	public void Idle()
	{
		isDownDelay = true;
		anima.SetTrigger("Idle");
	}

	private void Update()
	{
		if (isDownDelay)
		{
			isDown = false;
			isDownDelay = false;
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (isDown || other != PlayerMgr.Inst.PlayerEtt)
		{
			return;
		}
		isDown = true;
		if (so103.currentOrder == order - 1)
		{
			so103.currentOrder = order;
			anima.SetTrigger("DownRight");
			if (order == 4)
			{
				so103.AllCorrect();
				SEMgr.Inst.puzzleSucceed.PlaySE();
			}
			else
			{
				SEMgr.Inst.puzzleClick.PlaySE();
			}
		}
		else
		{
			anima.SetTrigger("DownWrong");
			so103.StepWrong(this);
			SEMgr.Inst.puzzleFail.PlaySE();
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
