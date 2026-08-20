using System;
using System.Collections.Generic;
using DG.Tweening;
using PlayerLogger;
using UnityEngine;

public class SpecialObj223 : LayerCorrect, IRoomCtrller
{
	public GameObject BG;

	public GameObject root;

	public GameObject carPrefab;

	public GameObject blockPrefab;

	private RoomController roomCtrller;

	public SpecialObj223GameDataSO levelSO;

	private SpecialObj223GameData currentLevelData;

	private List<GameObject> tiles = new List<GameObject>();

	private List<GameObject> cars = new List<GameObject>();

	private List<GameObject> blocks = new List<GameObject>();

	public void Start()
	{
		if (levelSO.testID != -1)
		{
			currentLevelData = levelSO.levels[levelSO.testID].Copy();
		}
		else
		{
			currentLevelData = levelSO.levels[UnityEngine.Random.Range(0, levelSO.levels.Count)].Copy();
		}
		LoadLevel();
	}

	public void LoadLevel()
	{
		tiles.Clear();
		cars.Clear();
		root.transform.localPosition = new Vector3((float)currentLevelData.width * -0.5f, (float)currentLevelData.height * -0.5f, 0f);
		root.transform.DestroyAllChild();
		BG.transform.localScale = new Vector3(currentLevelData.width, currentLevelData.height, 1f);
		foreach (SpecialObj223GameData.SpecialObjCarPiece specialObjCarPiece in currentLevelData.specialObjCarPieces)
		{
			GameObject gameObject = null;
			gameObject = UnityEngine.Object.Instantiate(carPrefab, root.transform, worldPositionStays: true);
			gameObject.name = "Car";
			gameObject.SetActive(value: true);
			gameObject.GetComponent<BoxCollider>().size = new Vector3(specialObjCarPiece.position2.x - specialObjCarPiece.position1.x, specialObjCarPiece.position2.y - specialObjCarPiece.position1.y, 1f) + new Vector3(1f, 1f, 1f);
			for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
			{
				if (specialObjCarPiece.direction == Vector2.up || specialObjCarPiece.direction == Vector2.down)
				{
					gameObject.transform.GetChild(0).transform.GetChild(i).rotation = Quaternion.Euler(0f, 0f, 90f);
					gameObject.transform.GetChild(0).transform.GetChild(i).GetComponent<SpriteRenderer>().size = new Vector3(specialObjCarPiece.position2.y - specialObjCarPiece.position1.y, specialObjCarPiece.position2.x - specialObjCarPiece.position1.x, 1f) + new Vector3(1f, 1f, 1f);
				}
				else
				{
					gameObject.transform.GetChild(0).transform.GetChild(i).GetComponent<SpriteRenderer>().size = new Vector3(specialObjCarPiece.position2.x - specialObjCarPiece.position1.x, specialObjCarPiece.position2.y - specialObjCarPiece.position1.y, 1f) + new Vector3(1f, 1f, 1f);
				}
			}
			gameObject.transform.localPosition = ((specialObjCarPiece.position2 + specialObjCarPiece.position1) / 2f).GetVector3() + new Vector3(0.5f, 0.5f, -0.1f);
			SpecialObj223CarBlocksMono component = gameObject.GetComponent<SpecialObj223CarBlocksMono>();
			component.level = this;
			component.specialObjCar = specialObjCarPiece;
			component.SpecialObjBlock = null;
			cars.Add(gameObject);
		}
		foreach (SpecialObj223GameData.SpecialObjBlock block in currentLevelData.blocks)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(blockPrefab, root.transform, worldPositionStays: true);
			gameObject2.name = "Blocks";
			gameObject2.SetActive(value: true);
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localPosition = ((block.position2 + block.position1) / 2f).GetVector3() + new Vector3(0.5f, 0.5f, -0.1f);
			SpecialObj223CarBlocksMono component2 = gameObject2.GetComponent<SpecialObj223CarBlocksMono>();
			component2.level = this;
			component2.SpecialObjBlock = block;
			component2.specialObjCar = null;
			blocks.Add(gameObject2);
		}
	}

	private int GetRandomAngle()
	{
		int[] array = new int[4] { 0, 90, 180, 270 };
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	public void TriggerCar(SpecialObj223CarBlocksMono part)
	{
		SpecialObj223GameData.SpecialObjCarPiece specialObjCarPiece = part.specialObjCar;
		currentLevelData.CheckClick(ref specialObjCarPiece);
		part.transform.DOLocalMove(((specialObjCarPiece.position2 + specialObjCarPiece.position1) / 2f).GetVector3() + new Vector3(0.5f, 0.5f, -0.1f), 0.3f);
		if (!currentLevelData.CheckOutLevel(specialObjCarPiece.position1, specialObjCarPiece.position2))
		{
			return;
		}
		currentLevelData.RemoveAt(specialObjCarPiece.position1);
		cars.Remove(part.gameObject);
		SEMgr.Inst.so223BoardMove.PlaySE();
		part.cantInteract = true;
		Action<float> actionFade = delegate(float time)
		{
			SpriteRenderer[] componentsInChildren = part.gameObject.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].DOFade(0f, time);
			}
		};
		DOTween.Sequence().Append(DOVirtual.DelayedCall(0.3f, delegate
		{
			actionFade(0.3f);
		})).Append(DOVirtual.DelayedCall(0.35f, delegate
		{
			UnityEngine.Object.Destroy(part.gameObject);
		}));
		if (cars.Count == 0)
		{
			int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
			SEMgr.Inst.puzzleSucceed.PlaySE();
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", PlayerMgr.Inst.PlayerCtrller.transform.position, 2f);
			ItemInfo itemInfo = default(ItemInfo);
			itemInfo.type = ItemType.Spell;
			itemInfo.id = specialRoomSpell;
			ItemInfo info = itemInfo;
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, base.transform.position);
		}
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		this.roomCtrller = roomCtrller;
	}
}
