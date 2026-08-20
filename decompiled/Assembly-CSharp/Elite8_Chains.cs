using System.Collections.Generic;
using UnityEngine;

public class Elite8_Chains : MonoBehaviour
{
	public enum ChainMode
	{
		Rest,
		Cross,
		Wave,
		Lines,
		Maze,
		Triangle
	}

	public GameObject chainPrefab;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public List<Elite8_SingleChain> allChains = new List<Elite8_SingleChain>();

	public List<Elite8_SingleChain> linesGroup = new List<Elite8_SingleChain>();

	public Elite8_SingleChain linesMover;

	public float linesMoveToBorderTime;

	private float linesDiration;

	private float linesMoverDiration;

	private int linesDoneTime;

	public int linesMaxDoneTime;

	[Header("波动组的全部锁链，为了让锁链回收连贯，")]
	[Header("请让锁链总数大于场上存在的最多锁链数")]
	public List<Elite8_SingleChain> waveGroup = new List<Elite8_SingleChain>();

	private List<Elite8_SingleChain> waveRightGroup = new List<Elite8_SingleChain>();

	private List<Elite8_SingleChain> waveLeftGroup = new List<Elite8_SingleChain>();

	private Elite8_SingleChain waveStay;

	private Elite8_SingleChain useableWave;

	public float waveMoveToBorderTime;

	public int waveMaxExistCount;

	private float waveModeTimer;

	private float waveLinesCounter;

	private int waveTowards;

	private int waveDoneTime;

	public int waveMaxDoneTime;

	private int waveSpawnTime;

	public List<Elite8_SingleChain> crossGroup = new List<Elite8_SingleChain>();

	public float crossModeToBorderTime;

	public Vector2 crossModeDiration;

	private int crossDoneTime;

	public int crossMaxDoneTime;

	public List<Elite8_SingleChain> MazeGroup = new List<Elite8_SingleChain>();

	public Elite8_SingleChain MazeMover;

	private int mazeDoneTime;

	public int mazeMaxDoneTime;

	public float mazeMoveToBorderTime;

	private float mazeTowards;

	public List<Elite8_ChildChain> triangleGroup = new List<Elite8_ChildChain>();

	private int triangleDoneTime;

	public int triangleMaxDoneTime;

	public float triangleTimer;

	public float triangleExistTime;

	public float triangleExpandSpeed;

	public float triangleSpawnInterval;

	public float triangleMoveSpeed;

	public float activeTimeLimit;

	private float activeTimer;

	public float restTimer;

	public bool hasUsed;

	public ChainMode state;

	private ChainMode preState;

	private ChainMode tempState;

	public MiniObjPool MiniPool { get; private set; }

	private void Start()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		base.transform.position = roomCenterPoint;
		MiniPool = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<MiniObjPool>();
		for (int i = 0; i < allChains.Count; i++)
		{
			allChains[i].gameObject.SetActive(value: false);
		}
		for (int j = 0; j < triangleGroup.Count; j++)
		{
			triangleGroup[j].maxExistTime = triangleExistTime;
			triangleGroup[j].radiusSpreadSpeed = triangleExpandSpeed;
			triangleGroup[j].transform.position = base.transform.position;
			triangleGroup[j].gameObject.SetActive(value: false);
		}
		if (GameMgr.IsMobile_Static)
		{
			crossModeToBorderTime *= 1.1f;
			waveMoveToBorderTime *= 1.25f;
			triangleExpandSpeed *= 0.8f;
			triangleSpawnInterval *= 1.25f;
			triangleMaxDoneTime--;
			linesMoveToBorderTime *= 1.25f;
		}
		state = ChainMode.Rest;
		tempState = ChainMode.Rest;
	}

	private void Update()
	{
		preState = tempState;
		tempState = state;
		bool flag = preState != state;
		if (state != 0)
		{
			restTimer = 0f;
			activeTimer += Time.deltaTime;
			if (waveDoneTime >= waveMaxDoneTime)
			{
				state = ChainMode.Rest;
				waveDoneTime = 0;
			}
			if (linesDoneTime >= linesMaxDoneTime)
			{
				state = ChainMode.Rest;
				linesDoneTime = 0;
			}
			if (crossDoneTime >= crossMaxDoneTime)
			{
				state = ChainMode.Rest;
				crossDoneTime = 0;
			}
			if (mazeDoneTime >= mazeMaxDoneTime)
			{
				state = ChainMode.Rest;
				mazeDoneTime = 0;
			}
		}
		else
		{
			restTimer += Time.deltaTime;
		}
		switch (state)
		{
		case ChainMode.Rest:
		{
			if (!flag)
			{
				break;
			}
			for (int num5 = 0; num5 < allChains.Count; num5++)
			{
				if (allChains[num5].gameObject.activeSelf)
				{
					allChains[num5].StopChain();
				}
			}
			for (int num6 = 0; num6 < triangleGroup.Count; num6++)
			{
				if (triangleGroup[num6].gameObject.activeSelf)
				{
					triangleGroup[num6].ChainsRecycle();
				}
			}
			break;
		}
		case ChainMode.Cross:
			if (flag)
			{
				SEMgr.Inst.elite8Attack.PlaySE();
				crossModeDiration.x = ((Random.Range(0, 2) != 0) ? 1 : (-1));
				crossModeDiration.y = ((Random.Range(0, 2) != 0) ? 1 : (-1));
				crossDoneTime = 0;
				for (int n = 0; n < crossGroup.Count; n++)
				{
					crossGroup[n].gameObject.SetActive(value: true);
				}
				crossGroup[0].transform.position = crossModeDiration.x * new Vector3(roomWidth / 2f, 0f, 0f) + roomCenterPoint;
				crossGroup[0].point1.position = crossGroup[0].transform.position - new Vector3(0f, roomHeight / 2f, 0f);
				crossGroup[0].point2.position = crossGroup[0].transform.position + new Vector3(0f, roomHeight / 2f, 0f);
				crossGroup[1].transform.position = crossModeDiration.x * new Vector3(0f, roomHeight / 2f, 0f) + roomCenterPoint;
				crossGroup[1].point1.position = crossGroup[1].transform.position - new Vector3(roomWidth / 2f, 0f, 0f);
				crossGroup[1].point2.position = crossGroup[1].transform.position + new Vector3(roomWidth / 2f, 0f, 0f);
				activeTimer = -0.5f;
			}
			if (crossGroup[1].transform.position.y > roomHeight / 2f + roomCenterPoint.y || crossGroup[1].transform.position.y < (0f - roomHeight) / 2f + roomCenterPoint.y)
			{
				crossGroup[1].StopChain();
			}
			else if (activeTimer > 0f)
			{
				crossGroup[1].transform.position -= crossModeDiration.x * new Vector3(0f, roomHeight / crossModeToBorderTime, 0f) * Time.deltaTime;
			}
			if (crossGroup[0].transform.position.x > roomWidth / 2f + roomCenterPoint.x || crossGroup[0].transform.position.x < (0f - roomWidth) / 2f + roomCenterPoint.x)
			{
				crossGroup[0].StopChain();
			}
			else if (activeTimer > 0f)
			{
				crossGroup[0].transform.position -= crossModeDiration.x * new Vector3(roomWidth / crossModeToBorderTime, 0f, 0f) * Time.deltaTime;
			}
			if (crossGroup[0].gameObject.activeSelf || crossGroup[1].gameObject.activeSelf)
			{
				break;
			}
			SEMgr.Inst.elite8Attack.PlaySE();
			crossModeDiration.x = ((Random.Range(0, 2) != 0) ? 1 : (-1));
			crossModeDiration.y = ((Random.Range(0, 2) != 0) ? 1 : (-1));
			crossDoneTime++;
			if (crossDoneTime < crossMaxDoneTime)
			{
				for (int num3 = 0; num3 < crossGroup.Count; num3++)
				{
					crossGroup[num3].gameObject.SetActive(value: true);
				}
				crossGroup[0].transform.position = crossModeDiration.x * new Vector3(roomWidth / 2f, 0f, 0f) + roomCenterPoint;
				crossGroup[0].point1.position = crossGroup[0].transform.position - new Vector3(0f, roomHeight / 2f, 0f);
				crossGroup[0].point2.position = crossGroup[0].transform.position + new Vector3(0f, roomHeight / 2f, 0f);
				crossGroup[1].transform.position = crossModeDiration.x * new Vector3(0f, roomHeight / 2f, 0f) + roomCenterPoint;
				crossGroup[1].point1.position = crossGroup[1].transform.position - new Vector3(roomWidth / 2f, 0f, 0f);
				crossGroup[1].point2.position = crossGroup[1].transform.position + new Vector3(roomWidth / 2f, 0f, 0f);
				activeTimer = -0.5f;
			}
			break;
		case ChainMode.Lines:
			if (flag)
			{
				SEMgr.Inst.elite8Attack.PlaySE();
				linesDiration = ((Random.Range(0, 2) != 0) ? 1 : (-1));
				linesMoverDiration = ((Random.Range(0, 2) != 0) ? 1 : (-1));
				linesDoneTime = -1;
				for (int k = 0; k < linesGroup.Count; k++)
				{
					linesGroup[k].gameObject.SetActive(value: true);
					if (linesDiration == -1f)
					{
						linesGroup[k].point1.position = roomCenterPoint - new Vector3(roomWidth / 2f, (0f - roomHeight) / 2f, 0f) + new Vector3((float)(k + 1) * roomWidth / (float)linesGroup.Count, 0f, 0f);
						linesGroup[k].point2.position = roomCenterPoint - new Vector3(roomWidth / 2f, roomHeight / 2f, 0f) + new Vector3((float)k * roomWidth / (float)linesGroup.Count, 0f, 0f) - new Vector3(0f, 0.5f, 0f);
					}
					else
					{
						linesGroup[k].point1.position = roomCenterPoint - new Vector3(roomWidth / 2f, (0f - roomHeight) / 2f, 0f) + new Vector3((float)k * roomWidth / (float)linesGroup.Count, 0f, 0f);
						linesGroup[k].point2.position = roomCenterPoint - new Vector3(roomWidth / 2f, roomHeight / 2f, 0f) + new Vector3((float)(k + 1) * roomWidth / (float)linesGroup.Count, 0f, 0f) - new Vector3(0f, 0.5f, 0f);
					}
				}
				linesMover.gameObject.SetActive(value: true);
				linesMover.transform.position = roomCenterPoint - new Vector3(0f, linesMoverDiration * roomHeight / 2f, 0f) - new Vector3(0f, 0.5f, 0f);
				linesMover.point1.position = roomCenterPoint - new Vector3(roomWidth / 2f, linesMoverDiration * roomHeight / 2f, 0f);
				linesMover.point2.position = roomCenterPoint - new Vector3((0f - roomWidth) / 2f, linesMoverDiration * roomHeight / 2f, 0f);
			}
			if (linesMover.transform.position.y < roomCenterPoint.y - roomHeight / 2f - 0.5f && linesMoverDiration > 0f)
			{
				linesDoneTime++;
				if (linesDoneTime < linesMaxDoneTime)
				{
					SEMgr.Inst.elite8Attack.PlaySE();
					linesMover.transform.position = roomCenterPoint + new Vector3(0f, roomHeight / 2f, 0f) - new Vector3(0f, 0.5f, 0f);
				}
			}
			else if (linesMover.transform.position.y > roomCenterPoint.y + roomHeight / 2f - 0.5f && linesMoverDiration < 0f)
			{
				linesDoneTime++;
				if (linesDoneTime < linesMaxDoneTime)
				{
					SEMgr.Inst.elite8Attack.PlaySE();
					linesMover.transform.position = roomCenterPoint - new Vector3(0f, roomHeight / 2f, 0f) - new Vector3(0f, 0.5f, 0f);
				}
			}
			else
			{
				linesMover.transform.position -= new Vector3(0f, 1f, 0f) * (linesMoverDiration * roomHeight / linesMoveToBorderTime) * Time.deltaTime;
			}
			break;
		case ChainMode.Wave:
		{
			if (flag)
			{
				SEMgr.Inst.elite8Attack.PlaySE();
				if (Random.Range(0, 2) == 0)
				{
					waveTowards = 1;
				}
				else
				{
					waveTowards = -1;
				}
				waveLinesCounter = 0f;
				waveModeTimer = 0f;
				waveLeftGroup.Clear();
				waveRightGroup.Clear();
				waveStay = null;
				waveDoneTime = 0;
				waveSpawnTime = 0;
				for (int l = 0; l < waveGroup.Count; l++)
				{
					waveGroup[l].gameObject.SetActive(value: false);
					waveGroup[l].transform.position = roomCenterPoint;
					waveGroup[l].point1.position = roomCenterPoint + new Vector3(0f, roomHeight / 2f + 0.5f, 0f);
					waveGroup[l].point2.position = roomCenterPoint - new Vector3(0f, roomHeight / 2f + 0.5f, 0f);
				}
			}
			waveModeTimer += Time.deltaTime;
			if ((double)waveModeTimer / ((double)(waveMoveToBorderTime / (float)waveMaxExistCount) / 1.2) > (double)waveLinesCounter && !(waveStay != null))
			{
				useableWave = null;
				for (int m = 0; m < waveGroup.Count; m++)
				{
					if (!waveGroup[m].gameObject.activeSelf)
					{
						useableWave = waveGroup[m];
						break;
					}
				}
				if (useableWave != null && waveSpawnTime < waveMaxDoneTime)
				{
					SEMgr.Inst.elite8Attack.PlaySE();
					waveSpawnTime++;
					useableWave.transform.position = roomCenterPoint;
					waveStay = useableWave;
					waveStay.gameObject.SetActive(value: true);
					waveLinesCounter += 1f;
					waveTowards = -waveTowards;
				}
			}
			if (waveStay != null && waveStay.chainCanHurt)
			{
				if (waveTowards > 0)
				{
					waveRightGroup.Add(waveStay);
					waveStay = null;
				}
				else
				{
					waveLeftGroup.Add(waveStay);
					waveStay = null;
				}
			}
			for (int num = waveRightGroup.Count - 1; num >= 0; num--)
			{
				waveRightGroup[num].transform.position += new Vector3((roomWidth + 1f) / 2f / waveMoveToBorderTime * Time.deltaTime, 0f, 0f);
				if (waveRightGroup[num].transform.position.x > roomCenterPoint.x + (roomWidth + 1f) / 2f)
				{
					waveRightGroup[num].StopChain();
					waveRightGroup.RemoveAt(num);
					waveDoneTime++;
				}
			}
			for (int num2 = waveLeftGroup.Count - 1; num2 >= 0; num2--)
			{
				waveLeftGroup[num2].transform.position -= new Vector3((roomWidth + 1f) / 2f / waveMoveToBorderTime * Time.deltaTime, 0f, 0f);
				if (waveLeftGroup[num2].transform.position.x < roomCenterPoint.x - (roomWidth + 1f) / 2f)
				{
					waveLeftGroup[num2].StopChain();
					waveLeftGroup.RemoveAt(num2);
					waveDoneTime++;
				}
			}
			break;
		}
		case ChainMode.Maze:
			if (flag)
			{
				SEMgr.Inst.elite8Attack.PlaySE();
				bool flag2 = Random.Range(0, 2) < 1;
				mazeTowards = ((Random.Range(0, 2) >= 1) ? 1 : (-1));
				for (int num4 = 0; num4 < MazeGroup.Count; num4++)
				{
					MazeGroup[num4].point1.localPosition = new Vector3(0f, roomHeight * 0.3f + 0.5f, 0f);
					MazeGroup[num4].point2.localPosition = -new Vector3(0f, roomHeight * 0.3f + 0.5f, 0f);
					if (flag2)
					{
						MazeGroup[num4].transform.position = new Vector3(roomWidth / (float)(MazeGroup.Count - 1) * (float)num4 + roomCenterPoint.x - roomWidth / 2f, roomHeight * 0.2f * (float)(-1 + num4 % 2 * 2) + roomCenterPoint.y, 0f);
					}
					else
					{
						MazeGroup[num4].transform.position = new Vector3(roomWidth / (float)(MazeGroup.Count - 1) * (float)num4 + roomCenterPoint.x - roomWidth / 2f, roomHeight * 0.2f * (float)(-1 + (num4 + 1) % 2 * 2) + roomCenterPoint.y, 0f);
					}
					if (num4 == 0)
					{
						MazeGroup[num4].transform.position += new Vector3(0.5f, 0f, 0f);
					}
					if (num4 == 4)
					{
						MazeGroup[num4].transform.position -= new Vector3(0.5f, 0f, 0f);
					}
					MazeGroup[num4].gameObject.SetActive(value: true);
				}
				MazeMover.transform.position = roomCenterPoint + new Vector3(roomWidth * mazeTowards / 2f, 0f, 0f);
				MazeMover.point1.localPosition = new Vector3(0f, roomHeight / 2f + 1f, 0f);
				MazeMover.point2.localPosition = -new Vector3(0f, roomHeight / 2f + 1f, 0f);
				MazeMover.gameObject.SetActive(value: true);
			}
			MazeMover.transform.position += -new Vector3(roomWidth / mazeMoveToBorderTime, 0f, 0f) * Time.deltaTime * mazeTowards;
			if (Mathf.Abs(MazeMover.transform.position.x - roomCenterPoint.x) > roomWidth / 2f + 1f)
			{
				SEMgr.Inst.elite8Attack.PlaySE();
				MazeMover.transform.position = roomCenterPoint + new Vector3(roomWidth * mazeTowards / 2f, 0f, 0f);
				mazeDoneTime++;
			}
			break;
		case ChainMode.Triangle:
			if (flag)
			{
				SEMgr.Inst.elite8Attack.PlaySE();
				for (int i = 0; i < triangleGroup.Count; i++)
				{
					if (triangleGroup[i].gameObject.activeSelf)
					{
						triangleGroup[i].ChainsRecycle();
					}
				}
				triangleGroup[0].transform.position = base.transform.position;
				triangleGroup[0].diration = new Vector3((Random.Range(0, 2) <= 0) ? 1 : (-1), 0f, 0f);
				triangleGroup[0].speed = triangleMoveSpeed;
				triangleGroup[0].gameObject.SetActive(value: true);
				triangleTimer = 0f;
				triangleDoneTime = 1;
			}
			triangleTimer += Time.deltaTime;
			if (triangleTimer > triangleSpawnInterval * (float)triangleDoneTime && triangleMaxDoneTime > triangleDoneTime - 1)
			{
				SEMgr.Inst.elite8Attack.PlaySE();
				triangleDoneTime++;
				for (int j = 0; j < triangleGroup.Count; j++)
				{
					if (!triangleGroup[j].gameObject.activeSelf)
					{
						triangleGroup[j].transform.position = base.transform.position;
						triangleGroup[j].gameObject.SetActive(value: true);
						triangleGroup[j].diration = new Vector3((Random.Range(0, 2) <= 0) ? 1 : (-1), 0f, 0f);
						triangleGroup[j].speed = triangleMoveSpeed;
						break;
					}
				}
			}
			if (triangleTimer > triangleSpawnInterval * (float)(triangleMaxDoneTime - 1) + triangleExistTime)
			{
				state = ChainMode.Rest;
				triangleDoneTime = 0;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}
}
