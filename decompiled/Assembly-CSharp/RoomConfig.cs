using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public struct RoomConfig
{
	public class Initializer : DataMgr.ConfigInitializer<RoomConfig>
	{
		private string[] _jsonList;

		public override void ReadJson()
		{
			_jsonList = DataMgr.GetSplitConfigText<RoomConfig>(4);
		}

		public override void Parse()
		{
			List<Task> list = new List<Task>();
			List<RoomConfig>[] lists = new List<RoomConfig>[4];
			for (int i = 0; i < _jsonList.Length; i++)
			{
				int index = i;
				Task task = new Task(delegate
				{
					lists[index] = DataMgr.ParseJsonList<RoomConfig>(_jsonList[index]);
				});
				list.Add(task);
				task.Start();
			}
			Task.WaitAll(list.ToArray());
			_jsonList = null;
			List<RoomConfig> list2 = new List<RoomConfig>();
			List<RoomConfig>[] array = lists;
			foreach (List<RoomConfig> collection in array)
			{
				list2.AddRange(collection);
			}
			rawData = list2;
			ApplyResult(list2);
			base.IsDone = true;
		}

		public override void ApplyResult(List<RoomConfig> result)
		{
			list = result;
			dic = new Dictionary<int, RoomConfig>();
			for (int i = 0; i < list.Count; i++)
			{
				if (GameMgr.IsMobile_Static && (list[i].themeType == RoomThemeType.Theme6_Chapter3 || list[i].themeType == RoomThemeType.Theme22_Chapter3_Shortcut1) && list[i].theme6Height > 11)
				{
					RoomConfig value = list[i];
					value.theme6Height = 11;
					list[i] = value;
				}
				dic.Add(list[i].id, list[i]);
			}
			if (GameMgr.IsHarmony_Static && dic.ContainsKey(11003))
			{
				RoomConfig value2 = dic[11003];
				value2.belongStage = 999999;
				dic[11003] = value2;
			}
		}
	}

	public static Dictionary<int, RoomConfig> dic;

	public static Dictionary<int, List<List<Vector2Data>>> originAllTileList = new Dictionary<int, List<List<Vector2Data>>>();

	public static List<RoomConfig> list;

	public int id;

	public string name;

	public RoomType type;

	public RoomThemeType themeType;

	public int theme6Width;

	public int theme6Height;

	public int theme8Width;

	public int theme8Height;

	public int theme8CornerRadius;

	public int themeEmptyWidth;

	public int themeEmptyHeight;

	public int belongStage;

	public int leastCountToBorn;

	public float monsterBornInterval;

	public float camExtraLimitX;

	public float camExtraLimitYUp;

	public float camExtraLimitYDown;

	public string overrideThemeMusic;

	public Vector4Data overrideThemeColor;

	public bool isShortcut;

	public List<List<RoomObjData>> allObjList;

	public List<List<Vector2Data>> allTileList;

	public List<Vector2Data> boundarys;

	public List<Vector2Data> boundary2s;

	public Vector2Data accessUp;

	public Vector2Data accessRight;

	public Vector2Data accessDown;

	public Vector2Data accessLeft;

	public bool haveRO;

	public Vector2Data extraDoor;

	public int localMinX;

	public int localMaxX;

	public int localMinY;

	public int localMaxY;

	public int width;

	public int height;

	public bool isFinalRoom;

	public bool generateRO;

	public bool isFlipped;

	public static RoomConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
			return dic[111].Copy();
		}
		return dic[id].Copy();
	}

	public RoomConfig Copy()
	{
		RoomConfig result = this;
		result.allObjList = new List<List<RoomObjData>>();
		for (int i = 0; i < allObjList.Count; i++)
		{
			result.allObjList.Add(new List<RoomObjData>());
			for (int j = 0; j < allObjList[i].Count; j++)
			{
				result.allObjList[i].Add(allObjList[i][j].Copy());
			}
		}
		result.allTileList = new List<List<Vector2Data>>();
		for (int k = 0; k < allTileList.Count; k++)
		{
			result.allTileList.Add(new List<Vector2Data>());
			for (int l = 0; l < allTileList[k].Count; l++)
			{
				result.allTileList[k].Add(allTileList[k][l]);
			}
		}
		result.boundarys = new List<Vector2Data>();
		for (int m = 0; m < boundarys.Count; m++)
		{
			result.boundarys.Add(boundarys[m]);
		}
		result.boundary2s = new List<Vector2Data>();
		for (int n = 0; n < boundary2s.Count; n++)
		{
			result.boundary2s.Add(boundary2s[n]);
		}
		return result;
	}

	public int GetExtraDistance(FourDir dir)
	{
		switch (dir)
		{
		case FourDir.Up:
			if (accessUp == Vector2Data.Up1000)
			{
				Debug.LogError("RoomID:" + id + " not have accessUp");
				return 0;
			}
			if ((float)localMaxY > accessUp.y)
			{
				return (int)accessUp.y - localMaxY;
			}
			return 0;
		case FourDir.Right:
			if (accessRight == Vector2Data.Up1000)
			{
				Debug.LogError("RoomID:" + id + " not have accessRight");
				return 0;
			}
			if ((float)localMaxX > accessRight.x)
			{
				return (int)accessRight.x - localMaxX;
			}
			return 0;
		case FourDir.Down:
			if (accessDown == Vector2Data.Up1000)
			{
				Debug.LogError("RoomID:" + id + " not have accessDown");
				return 0;
			}
			if ((float)localMinY > accessDown.y)
			{
				return localMinY - (int)accessDown.y;
			}
			return 0;
		case FourDir.Left:
			if (accessLeft == Vector2Data.Up1000)
			{
				Debug.LogError("RoomID:" + id + " not have accessLeft");
				return 0;
			}
			if ((float)localMinX > accessLeft.x)
			{
				return localMinX - (int)accessLeft.x;
			}
			return 0;
		default:
			Debug.LogError(dir);
			return 0;
		}
	}

	public void ReverseX()
	{
		if (isFlipped)
		{
			Debug.LogError("房间已经被翻转过！本次操作将翻转过来");
			isFlipped = false;
		}
		else
		{
			isFlipped = true;
		}
		for (int i = 0; i < allObjList.Count; i++)
		{
			for (int j = 0; j < allObjList[i].Count; j++)
			{
				allObjList[i][j].point.x = 0f - allObjList[i][j].point.x;
				allObjList[i][j].offset.x = 0f - allObjList[i][j].offset.x;
			}
		}
		for (int k = 0; k < allTileList.Count; k++)
		{
			for (int l = 0; l < allTileList[k].Count; l++)
			{
				allTileList[k][l] = new Vector2Data(0f - allTileList[k][l].x, allTileList[k][l].y);
			}
		}
		for (int m = 0; m < boundarys.Count; m++)
		{
			boundarys[m] = new Vector2Data(0f - boundarys[m].x, boundarys[m].y);
		}
		for (int n = 0; n < boundary2s.Count; n++)
		{
			boundary2s[n] = new Vector2Data(0f - boundary2s[n].x, boundary2s[n].y);
		}
		Vector2Data vector2Data = new Vector2Data(0f - accessLeft.x, accessLeft.y);
		Vector2Data vector2Data2 = new Vector2Data(0f - accessRight.x, accessRight.y);
		accessRight = vector2Data;
		accessLeft = vector2Data2;
		Vector2Data vector2Data3 = new Vector2Data(0f - accessUp.x, accessUp.y);
		Vector2Data vector2Data4 = new Vector2Data(0f - accessDown.x, accessDown.y);
		Vector2Data vector2Data5 = new Vector2Data(0f - extraDoor.x, extraDoor.y);
		vector2Data3.x -= 1f;
		vector2Data4.x -= 1f;
		vector2Data5.x -= 1f;
		accessUp = vector2Data3;
		accessDown = vector2Data4;
		extraDoor = vector2Data5;
		int num = localMinX;
		localMinX = -localMaxX;
		localMaxX = -num;
	}

	public void LoadArchiveClear()
	{
		for (int i = 0; i < allObjList.Count; i++)
		{
			if (i == 0)
			{
				for (int num = allObjList[i].Count - 1; num >= 0; num--)
				{
					if (allObjList[i][num].objType == RoomObjType.Unit)
					{
						if (!UnitConfig.map[allObjList[i][num].id].loadArchiveCreate)
						{
							allObjList[i].RemoveAt(num);
						}
					}
					else if (allObjList[i][num].objType == RoomObjType.SpecialObj)
					{
						if (allObjList[i][num].id == 30101)
						{
							allObjList[i][num].extraData1 = 1f;
						}
						if (!SpecialObjConfig.dic[allObjList[i][num].id].loadArchiveCreate)
						{
							allObjList[i].RemoveAt(num);
						}
					}
					else
					{
						allObjList[i].RemoveAt(num);
					}
				}
			}
			else
			{
				allObjList[i].Clear();
			}
		}
	}

	public void ReviseData()
	{
		localMinX = 0;
		localMaxX = 0;
		localMinY = 0;
		localMaxY = 0;
		if (themeType == RoomThemeType.Theme6_Chapter3 || themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			localMinX = -theme6Width / 2;
			localMaxX = theme6Width / 2;
			localMinY = -theme6Height / 2;
			localMaxY = theme6Height / 2;
		}
		else if (themeType == RoomThemeType.Theme7_Chapter4_Store || themeType == RoomThemeType.Theme8_Chapter4 || themeType == RoomThemeType.Theme9_Chapter4_2 || themeType == RoomThemeType.Theme12_Chapter5_2 || themeType == RoomThemeType.Theme15_Chapter5_Boss || themeType == RoomThemeType.Theme21_Chapter5_Store || themeType == RoomThemeType.Theme30_EndlessBattle)
		{
			localMinX = -theme8Width / 2;
			localMaxX = theme8Width / 2;
			localMinY = -theme8Height / 2;
			localMaxY = theme8Height / 2;
		}
		else if (themeType == RoomThemeType.Theme14_CustomRectangleEmptyScene || themeType == RoomThemeType.theme26_Chapter1_Dave || themeType == RoomThemeType.theme27_Store_Dave || themeType == RoomThemeType.theme28_Chapter4Boss_Dave || themeType == RoomThemeType.theme29_Chapter5Boss_Dave)
		{
			localMinX = -themeEmptyWidth / 2;
			localMaxX = themeEmptyWidth / 2;
			localMinY = -themeEmptyHeight / 2;
			localMaxY = themeEmptyHeight / 2;
		}
		else
		{
			for (int i = 0; i < allTileList[0].Count; i++)
			{
				if (allTileList[0][i].x < (float)localMinX)
				{
					localMinX = (int)allTileList[0][i].x;
				}
				else if (allTileList[0][i].x > (float)localMaxX)
				{
					localMaxX = (int)allTileList[0][i].x;
				}
				if (allTileList[0][i].y < (float)localMinY)
				{
					localMinY = (int)allTileList[0][i].y;
				}
				else if (allTileList[0][i].y > (float)localMaxY)
				{
					localMaxY = (int)allTileList[0][i].y;
				}
			}
			width = localMaxX - localMinX + 1;
			height = localMaxY - localMinY + 1;
			boundarys.Clear();
			for (int j = localMinX - 1; j <= localMaxX + 1; j++)
			{
				for (int k = localMinY - 1; k <= localMaxY + 1; k++)
				{
					if (!allTileList[0].Contains(new Vector2Data(j, k)) && (allTileList[0].Contains(new Vector2Data(j, k + 1)) || allTileList[0].Contains(new Vector2Data(j + 1, k + 1)) || allTileList[0].Contains(new Vector2Data(j + 1, k)) || allTileList[0].Contains(new Vector2Data(j + 1, k - 1)) || allTileList[0].Contains(new Vector2Data(j, k - 1)) || allTileList[0].Contains(new Vector2Data(j - 1, k - 1)) || allTileList[0].Contains(new Vector2Data(j - 1, k)) || allTileList[0].Contains(new Vector2Data(j - 1, k + 1))))
					{
						boundarys.Add(new Vector2Data(j, k));
					}
				}
			}
			boundary2s.Clear();
			for (int l = 0; l < boundarys.Count; l++)
			{
				Vector2Data item = new Vector2Data(boundarys[l].x, boundarys[l].y + 1f);
				if (!allTileList[0].Contains(item) && !boundarys.Contains(item) && !boundary2s.Contains(item))
				{
					boundary2s.Add(item);
				}
				item = new Vector2Data(boundarys[l].x + 1f, boundarys[l].y + 1f);
				if (!allTileList[0].Contains(item) && !boundarys.Contains(item) && !boundary2s.Contains(item))
				{
					boundary2s.Add(item);
				}
				item = new Vector2Data(boundarys[l].x + 1f, boundarys[l].y);
				if (!allTileList[0].Contains(item) && !boundarys.Contains(item) && !boundary2s.Contains(item))
				{
					boundary2s.Add(item);
				}
				item = new Vector2Data(boundarys[l].x + 1f, boundarys[l].y - 1f);
				if (!allTileList[0].Contains(item) && !boundarys.Contains(item) && !boundary2s.Contains(item))
				{
					boundary2s.Add(item);
				}
				item = new Vector2Data(boundarys[l].x, boundarys[l].y - 1f);
				if (!allTileList[0].Contains(item) && !boundarys.Contains(item) && !boundary2s.Contains(item))
				{
					boundary2s.Add(item);
				}
				item = new Vector2Data(boundarys[l].x - 1f, boundarys[l].y - 1f);
				if (!allTileList[0].Contains(item) && !boundarys.Contains(item) && !boundary2s.Contains(item))
				{
					boundary2s.Add(item);
				}
				item = new Vector2Data(boundarys[l].x - 1f, boundarys[l].y + 1f);
				if (!allTileList[0].Contains(item) && !boundarys.Contains(item) && !boundary2s.Contains(item))
				{
					boundary2s.Add(item);
				}
				item = new Vector2Data(boundarys[l].x - 1f, boundarys[l].y + 1f);
				if (!allTileList[0].Contains(item) && !boundarys.Contains(item) && !boundary2s.Contains(item))
				{
					boundary2s.Add(item);
				}
			}
		}
		accessUp = Vector2Data.Up1000;
		accessRight = Vector2Data.Up1000;
		accessDown = Vector2Data.Up1000;
		accessLeft = Vector2Data.Up1000;
		haveRO = false;
		extraDoor = Vector2Data.Up1000;
		for (int m = 0; m < allObjList[0].Count; m++)
		{
			if (allObjList[0][m].objType == RoomObjType.SpecialObj)
			{
				switch (allObjList[0][m].id)
				{
				case 11:
					accessUp = allObjList[0][m].point + allObjList[0][m].offset;
					break;
				case 12:
					accessRight = allObjList[0][m].point + allObjList[0][m].offset;
					break;
				case 13:
					accessDown = allObjList[0][m].point + allObjList[0][m].offset;
					break;
				case 14:
					accessLeft = allObjList[0][m].point + allObjList[0][m].offset;
					break;
				case 21:
					haveRO = true;
					break;
				case 31:
					extraDoor = allObjList[0][m].point;
					break;
				}
			}
		}
	}

	public bool CheckDoorIsCorrect(Vector2Data accessPoint, FourDir dir)
	{
		switch (dir)
		{
		case FourDir.Up:
			if (allTileList[0].Contains(accessPoint + new Vector2Data(-2f, 0f)) && allTileList[0].Contains(accessPoint + new Vector2Data(-1f, 0f)) && allTileList[0].Contains(accessPoint + new Vector2Data(0f, 0f)) && allTileList[0].Contains(accessPoint + new Vector2Data(1f, 0f)) && allTileList[0].Contains(accessPoint + new Vector2Data(2f, 0f)) && allTileList[0].Contains(accessPoint + new Vector2Data(3f, 0f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(-2f, 1f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(-1f, 1f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(0f, 1f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(1f, 1f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(2f, 1f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(3f, 1f)))
			{
				return true;
			}
			return false;
		case FourDir.Right:
			if (allTileList[0].Contains(accessPoint + new Vector2Data(0f, 0f)) && allTileList[0].Contains(accessPoint + new Vector2Data(0f, 1f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(1f, 0f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(1f, 1f)))
			{
				return true;
			}
			return false;
		case FourDir.Down:
			if (allTileList[0].Contains(accessPoint + new Vector2Data(0f, 0f)) && allTileList[0].Contains(accessPoint + new Vector2Data(1f, 0f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(0f, -1f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(1f, -1f)))
			{
				return true;
			}
			return false;
		case FourDir.Left:
			if (allTileList[0].Contains(accessPoint + new Vector2Data(0f, 0f)) && allTileList[0].Contains(accessPoint + new Vector2Data(0f, 1f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(-1f, 0f)) && !allTileList[0].Contains(accessPoint + new Vector2Data(-1f, 1f)))
			{
				return true;
			}
			return false;
		default:
			Debug.LogError(dir);
			return false;
		}
	}

	public bool CheckExtraDoorIsCorrect(Vector2Data extraDoorPoint)
	{
		if (allTileList[0].Contains(extraDoorPoint + new Vector2Data(-1f, 0f)) && allTileList[0].Contains(extraDoorPoint + new Vector2Data(0f, 0f)) && allTileList[0].Contains(extraDoorPoint + new Vector2Data(1f, 0f)) && allTileList[0].Contains(extraDoorPoint + new Vector2Data(2f, 0f)) && !allTileList[0].Contains(extraDoorPoint + new Vector2Data(-1f, 1f)) && !allTileList[0].Contains(extraDoorPoint + new Vector2Data(0f, 1f)) && !allTileList[0].Contains(extraDoorPoint + new Vector2Data(1f, 1f)) && !allTileList[0].Contains(extraDoorPoint + new Vector2Data(2f, 1f)))
		{
			return true;
		}
		return false;
	}
}
