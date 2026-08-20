using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpecialObj223LevelMaker : MonoBehaviour
{
	public SpecialObj223GameDataSO SpecialObj223GameDataSO;

	public SpecialObj223GameData specialObj223GameData;

	public GameObject background;

	public GameObject carPfbVerticle;

	public GameObject carPfbHorizontal;

	public GameObject blocksPrefab;

	public GameObject tile;

	public GameObject root;

	private Vector2 point1;

	private Vector2 point2;

	private Vector2 point1Acc;

	private Vector2 point2Acc;

	private List<GameObject> tiles = new List<GameObject>();

	private List<GameObject> cars = new List<GameObject>();

	private List<GameObject> blocks = new List<GameObject>();

	public Dropdown dropdown;

	public Text text;

	public int currentID;

	public InputField width;

	public InputField height;

	private void Start()
	{
		if (SpecialObj223GameDataSO.testID != -1)
		{
			currentID = SpecialObj223GameDataSO.testID;
		}
		else if (SpecialObj223GameDataSO.levels.Count == 0)
		{
			currentID = 0;
			SpecialObj223GameData item = new SpecialObj223GameData
			{
				width = 10,
				height = 10
			};
			SpecialObj223GameDataSO.levels.Add(item);
		}
		specialObj223GameData = SpecialObj223GameDataSO.levels[currentID].Copy();
		ReLoad();
	}

	public void TestThis()
	{
		SpecialObj223GameDataSO.testID = currentID;
		_ = Application.isEditor;
	}

	public void DeleteLevel()
	{
		SpecialObj223GameDataSO.levels.RemoveAt(currentID);
		currentID--;
		if (SpecialObj223GameDataSO.levels.Count == 0)
		{
			currentID = 0;
			SpecialObj223GameData item = new SpecialObj223GameData
			{
				width = 10,
				height = 10
			};
			SpecialObj223GameDataSO.levels.Add(item);
		}
		specialObj223GameData = SpecialObj223GameDataSO.levels[currentID].Copy();
		ReLoad();
	}

	public void CreateLevel()
	{
		SpecialObj223GameData specialObj223GameData = new SpecialObj223GameData();
		specialObj223GameData.width = int.Parse(width.text);
		specialObj223GameData.height = int.Parse(height.text);
		currentID++;
		SpecialObj223GameDataSO.levels.Insert(currentID, specialObj223GameData);
		this.specialObj223GameData = SpecialObj223GameDataSO.levels[currentID].Copy();
		ReLoad();
	}

	public void NextLevel()
	{
		if (currentID < SpecialObj223GameDataSO.levels.Count - 1)
		{
			currentID++;
		}
		specialObj223GameData = SpecialObj223GameDataSO.levels[currentID].Copy();
		ReLoad();
	}

	public void PreviousLevel()
	{
		if (currentID > 0)
		{
			currentID--;
		}
		specialObj223GameData = SpecialObj223GameDataSO.levels[currentID].Copy();
		ReLoad();
	}

	public void SaveLevel()
	{
		SpecialObj223GameDataSO.levels[currentID] = specialObj223GameData.Copy();
		_ = Application.isEditor;
	}

	public void ReLoad()
	{
		specialObj223GameData = SpecialObj223GameDataSO.levels[currentID].Copy();
		tiles.Clear();
		cars.Clear();
		root.transform.DestroyAllChild();
		background.transform.localScale = new Vector3(specialObj223GameData.width, specialObj223GameData.height, 0f);
		for (int i = 0; i < specialObj223GameData.width; i++)
		{
			for (int j = 0; j < specialObj223GameData.height; j++)
			{
				GameObject gameObject = Object.Instantiate(tile, root.transform, worldPositionStays: true);
				gameObject.name = "Tile";
				gameObject.SetActive(value: true);
				gameObject.transform.localPosition = new Vector3(i, j, 0f) + new Vector3(0.5f, 0.5f, 0f);
				tiles.Add(gameObject);
			}
		}
		for (int k = 0; k < specialObj223GameData.specialObjCarPieces.Count; k++)
		{
			SpecialObj223GameData.SpecialObjCarPiece specialObjCarPiece = specialObj223GameData.specialObjCarPieces[k];
			GameObject gameObject2 = null;
			gameObject2 = ((!(specialObjCarPiece.direction == Vector2.up) && !(specialObjCarPiece.direction == Vector2.down)) ? Object.Instantiate(carPfbHorizontal, root.transform, worldPositionStays: true) : Object.Instantiate(carPfbVerticle, root.transform, worldPositionStays: true));
			gameObject2.name = "Car";
			gameObject2.SetActive(value: true);
			gameObject2.transform.localScale = new Vector3(specialObjCarPiece.position2.x - specialObjCarPiece.position1.x, specialObjCarPiece.position2.y - specialObjCarPiece.position1.y, 1f) + new Vector3(1f, 1f, 1f);
			gameObject2.transform.localPosition = ((specialObjCarPiece.position2 + specialObjCarPiece.position1) / 2f).GetVector3() + new Vector3(0.5f, 0.5f, -0.1f);
			gameObject2.GetComponent<SpecialObj223LevelMakerCarBlockMono>().specialObjCar = specialObjCarPiece;
			cars.Add(gameObject2);
		}
		for (int l = 0; l < specialObj223GameData.blocks.Count; l++)
		{
			SpecialObj223GameData.SpecialObjBlock specialObjBlock = specialObj223GameData.blocks[l];
			GameObject gameObject3 = Object.Instantiate(blocksPrefab, root.transform, worldPositionStays: true);
			gameObject3.name = "Blocks";
			gameObject3.SetActive(value: true);
			gameObject3.transform.localScale = new Vector3(specialObjBlock.position2.x - specialObjBlock.position1.x, specialObjBlock.position2.y - specialObjBlock.position1.y, 1f) + new Vector3(1f, 1f, 1f);
			gameObject3.transform.localPosition = ((specialObjBlock.position2 + specialObjBlock.position1) / 2f).GetVector3() + new Vector3(0.5f, 0.5f, -0.1f);
			gameObject3.GetComponent<SpecialObj223LevelMakerCarBlockMono>().SpecialObjBlock = specialObjBlock;
			blocks.Add(gameObject3);
		}
	}

	private void Update()
	{
		text.text = currentID + 1 + "/" + SpecialObj223GameDataSO.levels.Count;
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.name == "Tile")
			{
				point1 = raycastHit2D.collider.transform.localPosition + new Vector3(-0.5f, -0.5f, 0f);
				point1Acc = Input.mousePosition;
			}
		}
		else if (Input.GetMouseButtonUp(1))
		{
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (raycastHit2D2.collider != null)
			{
				if (raycastHit2D2.collider.gameObject.name == "Tile")
				{
					point2 = raycastHit2D2.collider.transform.localPosition + new Vector3(-0.5f, -0.5f, 0f);
					point2Acc = Input.mousePosition;
					Vector2 vector = point2Acc - point1Acc;
					Vector2 vector2 = default(Vector2);
					vector2 = ((Mathf.Abs(vector.x) > Mathf.Abs(vector.y)) ? ((!(vector.x > 0f)) ? Vector2.left : Vector2.right) : ((!(vector.y > 0f)) ? Vector2.down : Vector2.up));
					if (dropdown.value == 0)
					{
						specialObj223GameData.specialObjCarPieces.Add(new SpecialObj223GameData.SpecialObjCarPiece(point1, point2, vector2));
					}
					else if (dropdown.value == 1)
					{
						specialObj223GameData.blocks.Add(new SpecialObj223GameData.SpecialObjBlock(point1, point2));
					}
				}
				else if (raycastHit2D2.collider.gameObject.name == "Car")
				{
					if (dropdown.value == 2)
					{
						specialObj223GameData.DeleteAt(raycastHit2D2.collider.gameObject.GetComponent<SpecialObj223LevelMakerCarBlockMono>().specialObjCar.position1);
					}
				}
				else if (raycastHit2D2.collider.gameObject.name == "Blocks" && dropdown.value == 2)
				{
					specialObj223GameData.DeleteAt(raycastHit2D2.collider.gameObject.GetComponent<SpecialObj223LevelMakerCarBlockMono>().SpecialObjBlock.position1);
				}
				SaveLevel();
				ReLoad();
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D raycastHit2D3 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (raycastHit2D3.collider != null && (bool)raycastHit2D3.collider.gameObject.GetComponent<SpecialObj223LevelMakerCarBlockMono>())
			{
				SpecialObj223GameData.SpecialObjCarPiece specialObjCar = raycastHit2D3.collider.gameObject.GetComponent<SpecialObj223LevelMakerCarBlockMono>().specialObjCar;
				specialObj223GameData.CheckClick(ref raycastHit2D3.collider.gameObject.GetComponent<SpecialObj223LevelMakerCarBlockMono>().specialObjCar);
				raycastHit2D3.collider.transform.DOLocalMove(((specialObjCar.position2 + specialObjCar.position1) / 2f).GetVector3() + new Vector3(0.5f, 0.5f, -0.1f), 0.5f);
			}
		}
		foreach (GameObject tile in tiles)
		{
			tile.GetComponent<SpriteRenderer>().color = Color.white;
		}
		RaycastHit2D raycastHit2D4 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (raycastHit2D4.collider != null && raycastHit2D4.collider.gameObject.name == "Tile")
		{
			raycastHit2D4.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		}
	}
}
