using PlayerLogger;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.UI;

public class SpecialObj215 : MonoBehaviour, IRoomCtrller, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public Animator anima;

	public float totalTime;

	public float breathTime;

	public float singleModeTime;

	public float spellRange;

	public float modeShootCD;

	public float modeShiftDistance;

	public float spellHeight;

	public float spellDuration;

	public float spellKnockback;

	public int spellDamage;

	public float spellSpeed;

	public MeshRenderer symbol;

	public Text text;

	public UnityEngine.BoxCollider thisCollider;

	private MiniObjPool miniPool;

	private RoomController belongRoom;

	private bool isEntered;

	private bool gameStart;

	private float shootCD;

	private float timer;

	private float shootTimer = -0.5f;

	private float modeRunTimer;

	private float breathingTimer;

	private float shiftingX;

	private float shiftingY;

	private int startForward = -1;

	private int gameMode;

	private Vector2 roomOffset;

	private string remainder;

	private SpellSpawnParams ssp;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		miniPool = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), base.transform.parent).GetComponent<MiniObjPool>();
		base.gameObject.name = 800201 + "(Clone)";
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90061);
		UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Knockback = spellKnockback;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.ApplyToSSP(ref ssp);
		roomOffset.x = (float)belongRoom.roomCfg.theme6Width / 2f + 1f;
		roomOffset.y = (float)belongRoom.roomCfg.theme6Height / 2f + 1f;
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	protected void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Update()
	{
		if (!gameStart)
		{
			return;
		}
		timer += Time.deltaTime;
		shootTimer += Time.deltaTime;
		modeRunTimer += Time.deltaTime;
		symbol.enabled = false;
		if (spellDuration >= totalTime - timer + 0.75f)
		{
			spellDuration -= Time.deltaTime;
		}
		if (modeRunTimer >= singleModeTime)
		{
			gameMode = 0;
		}
		if (gameMode == 0)
		{
			breathingTimer += Time.deltaTime;
			modeRunTimer = 0f;
			if (breathingTimer >= breathTime)
			{
				breathingTimer = 0f;
				startForward = -1;
				gameMode = 1;
			}
		}
		if (shootTimer >= shootCD)
		{
			shootTimer = 0f;
			if (gameMode == 1)
			{
				shootCD = modeShootCD;
				startForward = Random.Range(0, 4);
				float num = Random.Range(0f - roomOffset.x, roomOffset.x);
				float num2 = Random.Range(0f - roomOffset.y, roomOffset.y);
				shiftingX = Random.Range(0f - modeShiftDistance, modeShiftDistance);
				shiftingY = Random.Range(0f - modeShiftDistance, modeShiftDistance);
				Vector3 spawnPosition = new Vector3(0f, 0f, 0f);
				if (startForward == 0)
				{
					spawnPosition = new Vector3(belongRoom.CenterPoint.x + num, belongRoom.CenterPoint.y + roomOffset.y, 0f - spellHeight);
				}
				if (startForward == 1)
				{
					spawnPosition = new Vector3(belongRoom.CenterPoint.x + num, belongRoom.CenterPoint.y - roomOffset.y, 0f - spellHeight);
				}
				if (startForward == 2)
				{
					spawnPosition = new Vector3(belongRoom.CenterPoint.x + roomOffset.x, belongRoom.CenterPoint.y + num2, 0f - spellHeight);
				}
				if (startForward == 3)
				{
					spawnPosition = new Vector3(belongRoom.CenterPoint.y + roomOffset.y, belongRoom.CenterPoint.x - num, 0f - spellHeight);
				}
				float num3 = belongRoom.CenterPoint.x + shiftingX - spawnPosition.x;
				float num4 = belongRoom.CenterPoint.y + shiftingY - spawnPosition.y;
				Vector3 direction = new Vector3(num3 / Mathf.Sqrt(num3 * num3 + num4 * num4), num4 / Mathf.Sqrt(num3 * num3 + num4 * num4), 0f);
				UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.SpawnPosition = spawnPosition;
				sSPModifier.Direction = direction;
				sSPModifier.Duration = spellDuration;
				sSPModifier.ApplyToSSP(ref ssp);
				UnitDotsSyncSystem.ShootSpell(ssp);
			}
		}
		remainder = Mathf.Ceil(totalTime - timer).ToString();
		if (Mathf.Ceil(totalTime - timer) <= 0f)
		{
			remainder = "0";
		}
		if (timer >= 0.115f)
		{
			text.text = remainder;
		}
		if (timer >= totalTime + 1f)
		{
			belongRoom.AllAccessOpen();
			text.text = "";
			symbol.enabled = true;
			gameStart = false;
			miniPool.GetGO("Prefabs/EF/EF_Puzzle_Correct", base.transform.position, 2f);
			int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
			ItemInfo itemInfo = default(ItemInfo);
			itemInfo.type = ItemType.Spell;
			itemInfo.id = specialRoomSpell;
			ItemInfo info = itemInfo;
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, base.transform.position);
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
			SEMgr.Inst.puzzleSucceed.PlaySE();
		}
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongRoom = roomCtrller;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!isEntered && other == PlayerMgr.Inst.PlayerEtt)
		{
			isEntered = true;
			gameStart = true;
			anima.SetTrigger("On");
			belongRoom.AllAccessClose();
			SEMgr.Inst.puzzleClick.PlaySE();
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
