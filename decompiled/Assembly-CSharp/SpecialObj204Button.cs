using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj204Button : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public Animator anima;

	public MeshRenderer mr;

	public Sprite[] sprite_Symbols_Original;

	public Sprite[] sprite_Symbols_H;

	public UnityEngine.BoxCollider thisCollider;

	private SpecialObj204 so106;

	private bool isReady;

	private bool isDown;

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

	public int Index { get; private set; }

	public bool IsOn { get; private set; }

	public int Type { get; private set; }

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (isReady && !isDown && other == PlayerMgr.Inst.PlayerEtt)
		{
			isDown = true;
			so106.ButtonOn(this);
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	public void Initialize(SpecialObj204 so106, int type)
	{
		this.so106 = so106;
		Type = type;
		mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Symbols[type].texture);
	}

	public void Start()
	{
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

	public void Show()
	{
		anima.SetTrigger("Show");
	}

	public void ShowOver()
	{
		anima.SetTrigger("ShowOver");
	}

	public void Ready()
	{
		anima.SetTrigger("Ready");
		isReady = true;
	}

	public void Idle()
	{
		anima.SetTrigger("Idle");
	}

	public void Down()
	{
		if (!isDown)
		{
			isDown = true;
			anima.SetTrigger("Down");
		}
	}

	public void DownRight()
	{
		anima.SetTrigger("DownRight");
	}

	public void DownWrong()
	{
		anima.SetTrigger("DownWrong");
	}

	public void Reset()
	{
		Idle();
		isReady = false;
		isDown = false;
	}

	public void DisableInteractive()
	{
		isDown = true;
	}
}
