using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenBLive.Runtime.Data;
using UnityEngine;

[RequireComponent(typeof(BLiveConnect))]
public class BLiveMgr : MonoBehaviour
{
	private BLiveConnect _connect;

	private readonly Dictionary<BLiveCommandCacheType, Queue<BLiveCommand>> _commandQueue = new Dictionary<BLiveCommandCacheType, Queue<BLiveCommand>>();

	private float _lastSummonDanmakuTime;

	private float _lastCurseDanmakuTime;

	private float _lastRelicDanmakuTime;

	public static BLiveMgr Inst { get; private set; }

	public bool Connected => _connect.Connected;

	public int QueueCount => _commandQueue.Values.Sum((Queue<BLiveCommand> e) => e.Count);

	public bool HasLineError { get; private set; }

	private bool _isTestMode => ScriptableObjMgr.Inst.testCtrller.BLiveTestMode;

	private void Awake()
	{
		_connect = GetComponent<BLiveConnect>();
		Inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public async Task Connect(string code)
	{
		if (!Connected)
		{
			await _connect.LinkStart(code, this);
			HasLineError = false;
		}
	}

	public async Task Disconnect()
	{
		if (Connected)
		{
			await _connect.LinkEnd();
			HasLineError = false;
		}
	}

	public void AppendCommand(BLiveCommand command)
	{
		if (command.CanExecute())
		{
			command.Execute();
		}
		else if (command.paid)
		{
			BLiveCommandCacheType cacheType = command.CacheType;
			if (!_commandQueue.ContainsKey(cacheType))
			{
				_commandQueue.Add(cacheType, new Queue<BLiveCommand>());
			}
			Queue<BLiveCommand> queue = _commandQueue[cacheType];
			if (queue.Count < command.CacheCount)
			{
				queue.Enqueue(command);
			}
		}
	}

	private void Update()
	{
		foreach (Queue<BLiveCommand> value in _commandQueue.Values)
		{
			if (value.Count != 0 && value.Peek().CanExecute())
			{
				value.Dequeue().Execute();
			}
		}
	}

	public void OnDanmaku(Dm dm)
	{
		if (dm.msg == "怪物" && Time.unscaledTime - _lastSummonDanmakuTime > GetSummonEnemyInterval())
		{
			AppendCommand(new SummonEnemyCommand(paid: false, null));
			_lastSummonDanmakuTime = Time.unscaledTime;
		}
		if (dm.msg == "诅咒" && Time.unscaledTime - _lastCurseDanmakuTime > GetCurseInterval())
		{
			AppendCommand(new AddOrRemoveCurseFreeCommand(null));
			_lastCurseDanmakuTime = Time.unscaledTime;
		}
		if (dm.msg == "遗物" && Time.unscaledTime - _lastRelicDanmakuTime > GetRelicInterval())
		{
			AppendCommand(new AddRelicCommand(paid: false, null));
			_lastRelicDanmakuTime = Time.unscaledTime;
		}
		if (_isTestMode && dm.msg.StartsWith("礼物") && dm.msg.Length > 2)
		{
			string msg = dm.msg;
			string giftName = msg.Substring(2, msg.Length - 2);
			OnGift(new SendGift
			{
				userName = dm.userName,
				giftName = giftName,
				giftNum = 1L
			});
		}
		float GetCurseInterval()
		{
			if (!_isTestMode)
			{
				return 8f;
			}
			return -1f;
		}
		float GetRelicInterval()
		{
			if (!_isTestMode)
			{
				return 8f;
			}
			return -1f;
		}
		float GetSummonEnemyInterval()
		{
			if (_isTestMode)
			{
				return -1f;
			}
			return BattleMgr.Inst.CurrentStage switch
			{
				1 => 8f, 
				2 => 7f, 
				3 => 6f, 
				4 => 6f, 
				5 => 5f, 
				6 => 5f, 
				7 => 4f, 
				8 => 3f, 
				9 => 3f, 
				10 => 3f, 
				_ => 5f, 
			};
		}
	}

	public void OnGift(SendGift gift)
	{
		if (gift.giftName == "诅咒权杖")
		{
			for (int i = 0; i < gift.giftNum; i++)
			{
				AppendCommand(new AddCursePaymentCommand(gift.userName));
			}
		}
		if (gift.giftName == "圣华之水")
		{
			for (int j = 0; j < gift.giftNum; j++)
			{
				AppendCommand(new RemoveCursePaymentCommand(gift.userName));
			}
		}
		if (gift.giftName == "遗物魔盒")
		{
			for (int k = 0; k < gift.giftNum; k++)
			{
				AppendCommand(new AddRelicCommand(paid: true, gift.userName));
			}
		}
		if (gift.giftName == "魔物之卵")
		{
			for (int l = 0; l < gift.giftNum; l++)
			{
				AppendCommand(new SummonEnemyCommand(paid: true, gift.userName));
			}
		}
	}

	public void OnLike(Like like)
	{
		AppendCommand(new RecoverFromLikes(like.uname));
	}

	public void OnLinkError(string json)
	{
		HasLineError = true;
		Debug.LogError("BLive Error: " + json);
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && !_isTestMode && !TimeScaleMgr.Inst.GamePaused)
		{
			UIMgr.Inst.UIMenu?.ShowUIMenu();
		}
	}
}
