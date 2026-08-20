using System.Collections.Generic;
using UnityEngine;

public class Elite8_ChildChain : MonoBehaviour
{
	public enum ChainType
	{
		line,
		Triangle,
		squire
	}

	public GameObject chainPrefab;

	public bool usedByChains;

	public int childId;

	public UnitProperty Elite8ppt;

	public List<Elite8_SingleChain> chains;

	public List<Elite8_Child> children;

	public List<Vector3> ChildrenDirations;

	public List<int> childrenConnection;

	public float radius;

	public float wideRadius;

	public Vector3 rotateDiraction;

	public Vector3 diration;

	public float speed;

	public VariableFloat rotateSpeedRange;

	public bool useRandomRotate = true;

	public float rotateSpeed;

	public float radiusSpreadSpeed;

	public float radiusSpreadSlowSpeed;

	public float radiusNow = 0.1f;

	public MiniObjPool MiniPool;

	public float maxExistTime;

	public float existTimer;

	public int edges;

	public bool workOnce;

	public bool isJail;

	public bool stableDestory;

	public void Recycle()
	{
		if (stableDestory)
		{
			ChainsRecycle();
			return;
		}
		for (int i = 0; i < edges; i++)
		{
			if (children[i] != null && !children[i].myPpt.AlreadyDead)
			{
				children[i].DieWithinBorder();
			}
		}
	}

	public void ChildReportDeath(Elite8_Child reporter)
	{
		if (children.IndexOf(reporter) != -1)
		{
			children[children.IndexOf(reporter)] = null;
		}
	}

	private void Start()
	{
		rotateDiraction = Tool2D.GetDir();
		if (useRandomRotate)
		{
			rotateSpeed = rotateSpeedRange.RandomResult();
		}
		for (int i = 0; i < edges; i++)
		{
			ChildrenDirations.Add(Tool2D.GetDir(rotateDiraction, 360 / edges * (i - 1)));
		}
		if (usedByChains)
		{
			MiniPool = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<MiniObjPool>();
			for (int j = 0; j < edges; j++)
			{
				Elite8_SingleChain component = Object.Instantiate(chainPrefab, base.transform.position, Quaternion.identity, base.transform).GetComponent<Elite8_SingleChain>();
				chains.Add(component);
			}
			return;
		}
		for (int k = 0; k < edges; k++)
		{
			Elite8_Child component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + childId, base.transform.position + new Vector3(0f, 200f, 0f)).GetComponent<Elite8_Child>();
			children.Add(component2);
			childrenConnection.Add(0);
			component2.group = this;
			component2.core = null;
			if (k > 0)
			{
				Elite8_SingleChain component3 = Object.Instantiate(chainPrefab, base.transform.position, Quaternion.identity, base.transform).GetComponent<Elite8_SingleChain>();
				chains.Add(component3);
				chains[k - 1].dummy = true;
				if (workOnce)
				{
					component3.workOnce = true;
				}
				component3.star1 = children[k - 1];
				childrenConnection[k - 1]++;
				component3.star2 = children[k];
				childrenConnection[k]++;
			}
		}
		if (edges > 2)
		{
			Elite8_SingleChain component4 = Object.Instantiate(chainPrefab, base.transform.position, Quaternion.identity, base.transform).GetComponent<Elite8_SingleChain>();
			chains.Add(component4);
			component4.dummy = true;
			if (workOnce)
			{
				component4.workOnce = true;
			}
			component4.damage = 10;
			component4.star1 = children[children.Count - 1];
			childrenConnection[children.Count - 1]++;
			component4.star2 = children[0];
			childrenConnection[0]++;
		}
		MiniPool = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<MiniObjPool>();
		if (isJail)
		{
			radiusNow = radius;
		}
	}

	public void OnEnable()
	{
		for (int i = 0; i < chains.Count; i++)
		{
			chains[i].gameObject.SetActive(value: true);
		}
		if (usedByChains)
		{
			radiusNow = 0f;
		}
	}

	public void ChainsRecycle()
	{
		for (int i = 0; i < edges; i++)
		{
			chains[i].StopChain();
		}
		existTimer = 0f;
	}

	private void Update()
	{
		if (usedByChains)
		{
			bool flag = true;
			for (int i = 0; i < edges; i++)
			{
				if (chains[i].gameObject.activeSelf)
				{
					flag = false;
				}
			}
			if (flag)
			{
				if (stableDestory)
				{
					Object.Destroy(base.gameObject);
				}
				else
				{
					rotateDiraction = Tool2D.GetDir();
					for (int j = 0; j < edges; j++)
					{
						ChildrenDirations[j] = Tool2D.GetDir(rotateDiraction, 360 / edges * (j - 1));
					}
					base.gameObject.SetActive(value: false);
				}
			}
			else if (stableDestory)
			{
				if (radiusNow < radius)
				{
					radiusNow += Time.deltaTime * radiusSpreadSpeed;
				}
				else if (radiusNow < wideRadius)
				{
					radiusNow += Time.deltaTime * radiusSpreadSlowSpeed;
				}
				existTimer += Time.deltaTime;
				if (existTimer > maxExistTime)
				{
					ChainsRecycle();
				}
			}
			else
			{
				radiusNow += Time.deltaTime * radiusSpreadSpeed;
			}
			base.transform.position += diration.normalized * speed * Time.deltaTime;
			for (int k = 0; k < edges; k++)
			{
				ChildrenDirations[k] = Tool2D.GetDir(ChildrenDirations[k], rotateSpeed / radiusNow * Time.deltaTime);
				chains[k].point1.position = base.transform.position + ChildrenDirations[k] * radiusNow;
				chains[k].point2.position = base.transform.position + ChildrenDirations[(k + 1 < edges) ? (k + 1) : 0] * radiusNow;
			}
			return;
		}
		base.transform.position += diration.normalized * speed * Time.deltaTime;
		bool flag2 = true;
		for (int l = 0; l < chains.Count; l++)
		{
			if (chains[l].gameObject.activeSelf)
			{
				flag2 = false;
			}
			int num = l + 1;
			if (num == chains.Count)
			{
				num = 0;
			}
			if ((children[l] == null || children[num] == null) && !chains[l].hasStopped)
			{
				chains[l].StopChain();
				childrenConnection[l]--;
				childrenConnection[num]--;
			}
			else if (!chains[l].hasStopped && chains[l].reportHurt)
			{
				chains[l].StopChain();
				childrenConnection[l]--;
				childrenConnection[num]--;
			}
		}
		existTimer += Time.deltaTime;
		for (int m = 0; m < edges; m++)
		{
			if (existTimer > maxExistTime && children[m] != null)
			{
				children[m].DieWithinBorder();
			}
			ChildrenDirations[m] = Tool2D.GetDir(ChildrenDirations[m], rotateSpeed / radiusNow * Time.deltaTime);
			if (childrenConnection[m] <= 0 && children[m] != null)
			{
				children[m].DieWithinBorder();
			}
			if (children[m] != null)
			{
				flag2 = false;
				children[m].transform.position = base.transform.position + ChildrenDirations[m] * radiusNow;
			}
			chains[m].point1.position = base.transform.position + ChildrenDirations[m] * radiusNow;
			chains[m].point2.position = base.transform.position + ChildrenDirations[(m + 1 < edges) ? (m + 1) : 0] * radiusNow;
		}
		if (flag2)
		{
			Object.Destroy(base.gameObject);
		}
		if (isJail)
		{
			if (radiusNow > 0.5f)
			{
				radiusNow -= Time.deltaTime * radiusSpreadSpeed;
				return;
			}
			for (int n = 0; n < edges; n++)
			{
				if (children[n] != null)
				{
					children[n].DieWithinBorder();
					children[n] = null;
				}
			}
		}
		else if (radiusNow < radius)
		{
			radiusNow += Time.deltaTime * radiusSpreadSpeed;
		}
		else if (radiusNow < wideRadius)
		{
			radiusNow += Time.deltaTime * radiusSpreadSlowSpeed;
		}
	}
}
