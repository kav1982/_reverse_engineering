using System.Collections.Generic;
using UnityEngine;

public class Curse_Recall : MonoBehaviour
{
	private enum CurseState
	{
		Idle,
		Recording,
		Backing
	}

	public Transform tsf_Original;

	public VariableFloat recallInterval;

	public float recordThreshold;

	public int backSpeedRatio;

	private CurseState state;

	private List<GameObject> go_WayPoints = new List<GameObject>();

	private CurseConfig curseCfg;

	private float timer;

	private List<Vector3> playerPoints = new List<Vector3>();

	private MiniObjPool miniPool;

	private void Update()
	{
		switch (state)
		{
		case CurseState.Idle:
			if (LevelMgr.Inst.CurrentRoomCfg.type != RoomType.Puzzle)
			{
				timer += Time.deltaTime;
				if (timer >= recallInterval.result)
				{
					timer = 0f;
					state = CurseState.Recording;
					tsf_Original.gameObject.SetActive(value: true);
					tsf_Original.position = Tool2D.GetLayerPoint(PlayerMgr.Inst.PlayerPoint, LayerCorrectType.GroundEffect);
					playerPoints.Add(PlayerMgr.Inst.PlayerPoint);
				}
			}
			break;
		case CurseState.Recording:
			if (PlayerMgr.Inst.PlayerPoint != playerPoints[playerPoints.Count - 1])
			{
				playerPoints.Add(PlayerMgr.Inst.PlayerPoint);
			}
			if (go_WayPoints.Count == 0)
			{
				if (Tool2D.IgnoreZDistanceSqr(PlayerMgr.Inst.PlayerPoint, tsf_Original.position) > recordThreshold * recordThreshold)
				{
					go_WayPoints.Add(miniPool.GetGO("Prefabs/Item/Curse_Recall_WayPoint", PlayerMgr.Inst.PlayerPoint));
					SEMgr.Inst.curse_Recall.PlaySE();
				}
			}
			else if (Tool2D.IgnoreZDistanceSqr(PlayerMgr.Inst.PlayerPoint, go_WayPoints[go_WayPoints.Count - 1].transform.position) > recordThreshold * recordThreshold)
			{
				go_WayPoints.Add(miniPool.GetGO("Prefabs/Item/Curse_Recall_WayPoint", PlayerMgr.Inst.PlayerPoint));
				SEMgr.Inst.curse_Recall.PlaySE();
			}
			timer += Time.deltaTime;
			if (timer >= curseCfg.float1.result)
			{
				timer = 0f;
				state = CurseState.Backing;
			}
			break;
		case CurseState.Backing:
		{
			for (int i = 0; i < backSpeedRatio; i++)
			{
				if (playerPoints.Count > 0)
				{
					PlayerMgr.Inst.SetPlayerPoint(playerPoints[playerPoints.Count - 1]);
					playerPoints.RemoveAt(playerPoints.Count - 1);
					if (go_WayPoints.Count > 0 && PlayerMgr.Inst.PlayerPoint == go_WayPoints[go_WayPoints.Count - 1].transform.position)
					{
						go_WayPoints[go_WayPoints.Count - 1].SetActive(value: false);
						go_WayPoints.RemoveAt(go_WayPoints.Count - 1);
						SEMgr.Inst.curse_Recall.PlaySE();
					}
				}
			}
			if (go_WayPoints.Count == 0)
			{
				PlayerMgr.Inst.SetPlayerPoint(PlayerMgr.Inst.PlayerPoint.IgnoreZ());
				ResetState();
				SEMgr.Inst.curse_Recall.PlaySE();
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void Initialize(CurseConfig curseCfg)
	{
		this.curseCfg = curseCfg;
		recallInterval.RandomResult();
		if (miniPool == null)
		{
			miniPool = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool")).GetComponent<MiniObjPool>();
		}
	}

	public void ResetState()
	{
		state = CurseState.Idle;
		for (int i = 0; i < go_WayPoints.Count; i++)
		{
			go_WayPoints[i].SetActive(value: false);
		}
		playerPoints.Clear();
		go_WayPoints.Clear();
		tsf_Original.gameObject.SetActive(value: false);
		timer = 0f;
	}

	public void DestroySelf()
	{
		Object.Destroy(miniPool.gameObject);
		Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (miniPool != null)
		{
			Object.Destroy(miniPool.gameObject);
		}
	}
}
