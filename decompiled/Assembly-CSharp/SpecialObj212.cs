using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class SpecialObj212 : LayerCorrect, IRoomCtrller, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	private enum State
	{
		Idle,
		ShowReward,
		Swap,
		SwapWait,
		Finish
	}

	[Space(50f)]
	public Vector3[] bookOffsets;

	public Vector3[] so9Offsets;

	public GameObject pfb_Book;

	public Animator anima;

	public float showRewardTime;

	public UnityEngine.BoxCollider thisCollider;

	[Header("Swap")]
	public float[] swapWaitTimes;

	public float[] swapSpeeds;

	public Vector3 swapMiddlePointOffset;

	private State state;

	private SpecialObj212Book[] books;

	private SpecialObj9[] so9s;

	private RoomController belongCtrller;

	private bool isEnterd;

	private float showRewardTimer;

	private int swapTimer;

	private float swapLerpValue;

	private Vector3[] bookSwapBeforePoints;

	private Vector3[] bookSwapToPoints;

	private Vector3[] bookSwapMiddlePoints;

	private float swapWaitTimer;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		if (belongCtrller.roomCfg.isFlipped)
		{
			for (int i = 0; i < bookOffsets.Length; i++)
			{
				bookOffsets[i].x = 0f - bookOffsets[i].x;
			}
			for (int j = 0; j < so9Offsets.Length; j++)
			{
				so9Offsets[j].x = 0f - so9Offsets[j].x;
			}
		}
		int num = Random.Range(0, bookOffsets.Length);
		books = new SpecialObj212Book[bookOffsets.Length];
		bookSwapBeforePoints = new Vector3[bookOffsets.Length];
		bookSwapToPoints = new Vector3[bookOffsets.Length];
		bookSwapMiddlePoints = new Vector3[bookOffsets.Length];
		for (int k = 0; k < bookOffsets.Length; k++)
		{
			books[k] = Object.Instantiate(pfb_Book, base.transform.position + bookOffsets[k], Quaternion.identity, base.transform.parent).GetComponent<SpecialObj212Book>();
			if (k == num)
			{
				books[k].Initialize(SO212BookType.Reward, belongCtrller);
			}
			else
			{
				books[k].Initialize(SO212BookType.Curse, belongCtrller);
			}
		}
		so9s = new SpecialObj9[so9Offsets.Length];
		for (int l = 0; l < so9Offsets.Length; l++)
		{
			so9s[l] = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/SpecialObjs/" + 901), base.transform.position + so9Offsets[l], Quaternion.identity, base.transform.parent).GetComponent<SpecialObj9>();
		}
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	public void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		switch (state)
		{
		case State.ShowReward:
			showRewardTimer += Time.deltaTime;
			if (showRewardTimer >= showRewardTime)
			{
				state = State.Swap;
				PrepareSwap();
			}
			break;
		case State.Swap:
		{
			swapLerpValue = Mathf.MoveTowards(swapLerpValue, 1f, swapSpeeds[swapTimer] * Time.deltaTime);
			for (int j = 0; j < bookOffsets.Length; j++)
			{
				if (bookSwapBeforePoints[j] != bookSwapToPoints[j])
				{
					books[j].transform.position = GeneralTool.QuadraticBezierCurve(bookSwapBeforePoints[j], bookSwapMiddlePoints[j], bookSwapToPoints[j], swapLerpValue);
					if (UnitDotsSyncSystem.EntityIsValid(books[j].thisEntity))
					{
						LocalTransform componentData = UnitDotsSyncSystem.GetComponentData<LocalTransform>(books[j].thisEntity);
						componentData.Position = books[j].transform.position;
						UnitDotsSyncSystem.SetComponentData(componentData, books[j].thisEntity);
					}
				}
			}
			if (swapLerpValue == 1f)
			{
				swapLerpValue = 0f;
				state = State.SwapWait;
			}
			break;
		}
		case State.SwapWait:
			swapWaitTimer += Time.deltaTime;
			if (!(swapWaitTimer >= swapWaitTimes[swapTimer]))
			{
				break;
			}
			swapWaitTimer = 0f;
			if (swapTimer == swapSpeeds.Length - 1)
			{
				state = State.Finish;
				for (int i = 0; i < so9s.Length; i++)
				{
					so9s[i].SetTrapInvalid();
				}
			}
			else
			{
				swapTimer++;
				state = State.Swap;
				PrepareSwap();
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case State.Idle:
		case State.Finish:
			break;
		}
	}

	private void PrepareSwap()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < bookOffsets.Length; i++)
		{
			list.Add(i);
		}
		int num = 0;
		bool flag;
		do
		{
			num++;
			if (num > 100)
			{
				Debug.Log("循环达到100次");
				list = new List<int> { 2, 1, 0 };
				break;
			}
			list.Upset();
			flag = false;
			for (int j = 0; j < bookOffsets.Length; j++)
			{
				if (list[j] != j)
				{
					flag = true;
					break;
				}
			}
		}
		while (!flag);
		for (int k = 0; k < bookOffsets.Length; k++)
		{
			bookSwapBeforePoints[k] = books[k].transform.position;
			bookSwapToPoints[k] = books[list[k]].transform.position;
			if (bookSwapBeforePoints[k].x > bookSwapToPoints[k].x)
			{
				bookSwapMiddlePoints[k] = (bookSwapBeforePoints[k] + bookSwapToPoints[k]) / 2f - swapMiddlePointOffset;
			}
			else
			{
				bookSwapMiddlePoints[k] = (bookSwapBeforePoints[k] + bookSwapToPoints[k]) / 2f + swapMiddlePointOffset;
			}
		}
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!isEnterd && other == PlayerMgr.Inst.PlayerEtt)
		{
			isEnterd = true;
			anima.SetTrigger("Down");
			state = State.ShowReward;
			for (int i = 0; i < books.Length; i++)
			{
				books[i].ShowReward();
			}
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
