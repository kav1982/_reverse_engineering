using System.Collections.Generic;
using UnityEngine;

public class SpecialObjPool : MonoBehaviour
{
	[Header("Prefabs")]
	public GameObject pfb_PoolNull;

	public GameObject pfb_PoolRight;

	public GameObject pfb_PoolLeft;

	public GameObject pfb_PoolUp;

	public GameObject pfb_PoolDown;

	public GameObject pfb_PoolUpRight;

	public GameObject pfb_PoolRightDown;

	public GameObject pfb_PoolDownLeft;

	public GameObject pfb_PoolLeftUp;

	public GameObject pfb_PoolLeftRight;

	public GameObject pfb_PoolUpDown;

	public GameObject pfb_PoolLeftUpRight;

	public GameObject pfb_PoolUpRightDown;

	public GameObject pfb_PoolRightDownLeft;

	public GameObject pfb_PoolDownLeftUp;

	public GameObject pfb_PoolFull;

	[Range(0f, 1f)]
	[Header("Ornament")]
	public float ornamentChance;

	public GameObject[] pfb_Ornaments;

	[Header("Other")]
	public GameObject go_Defualt;

	public GameObject go_UpRight;

	public GameObject go_RightDown;

	public GameObject go_DownLeft;

	public GameObject go_LeftUp;

	public Transform tsf_Transform;

	private void Start()
	{
		Object.Destroy(go_Defualt);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, 1f, "Pool");
		Vector3 vector = Tool2D.IgnoreZPoint(base.transform.position, 1.06f);
		GameObject gameObject = null;
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			if (collidersByTag[i].transform.parent.localPosition == base.transform.localPosition + new Vector3(0f, 0f, 1f))
			{
				flag = true;
			}
			else if (collidersByTag[i].transform.parent.localPosition == base.transform.localPosition + new Vector3(1f, 0f, 1f))
			{
				flag2 = true;
			}
			else if (collidersByTag[i].transform.parent.localPosition == base.transform.localPosition + new Vector3(1f, 0f, 0f))
			{
				flag3 = true;
			}
			else if (collidersByTag[i].transform.parent.localPosition == base.transform.localPosition + new Vector3(1f, 0f, -1f))
			{
				flag4 = true;
			}
			else if (collidersByTag[i].transform.parent.localPosition == base.transform.localPosition + new Vector3(0f, 0f, -1f))
			{
				flag5 = true;
			}
			else if (collidersByTag[i].transform.parent.localPosition == base.transform.localPosition + new Vector3(-1f, 0f, -1f))
			{
				flag6 = true;
			}
			else if (collidersByTag[i].transform.parent.localPosition == base.transform.localPosition + new Vector3(-1f, 0f, 0f))
			{
				flag7 = true;
			}
			else if (collidersByTag[i].transform.parent.localPosition == base.transform.localPosition + new Vector3(-1f, 0f, 1f))
			{
				flag8 = true;
			}
		}
		if (!flag && !flag3 && !flag5 && !flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolNull, tsf_Transform);
		}
		else if (flag && !flag3 && !flag5 && !flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolUp, tsf_Transform);
		}
		else if (!flag && flag3 && !flag5 && !flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolRight, tsf_Transform);
		}
		else if (!flag && !flag3 && flag5 && !flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolDown, tsf_Transform);
		}
		else if (!flag && !flag3 && !flag5 && flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolLeft, tsf_Transform);
		}
		else if (flag && flag3 && !flag5 && !flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolUpRight, tsf_Transform);
			if (flag2)
			{
				go_UpRight.SetActive(value: true);
			}
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolRightDown, tsf_Transform);
			if (flag4)
			{
				go_RightDown.SetActive(value: true);
			}
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolDownLeft, tsf_Transform);
			if (flag6)
			{
				go_DownLeft.SetActive(value: true);
			}
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolLeftUp, tsf_Transform);
			if (flag8)
			{
				go_LeftUp.SetActive(value: true);
			}
		}
		else if (!flag && flag3 && !flag5 && flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolLeftRight, tsf_Transform);
		}
		else if (flag && !flag3 && flag5 && !flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolUpDown, tsf_Transform);
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolLeftUpRight, tsf_Transform);
			if (flag8)
			{
				go_LeftUp.SetActive(value: true);
			}
			if (flag2)
			{
				go_UpRight.SetActive(value: true);
			}
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolUpRightDown, tsf_Transform);
			if (flag2)
			{
				go_UpRight.SetActive(value: true);
			}
			if (flag4)
			{
				go_RightDown.SetActive(value: true);
			}
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolRightDownLeft, tsf_Transform);
			if (flag4)
			{
				go_RightDown.SetActive(value: true);
			}
			if (flag6)
			{
				go_DownLeft.SetActive(value: true);
			}
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolDownLeftUp, tsf_Transform);
			if (flag6)
			{
				go_DownLeft.SetActive(value: true);
			}
			if (flag8)
			{
				go_LeftUp.SetActive(value: true);
			}
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			gameObject = Object.Instantiate(pfb_PoolFull, tsf_Transform);
			if (flag2)
			{
				go_UpRight.SetActive(value: true);
			}
			if (flag4)
			{
				go_RightDown.SetActive(value: true);
			}
			if (flag6)
			{
				go_DownLeft.SetActive(value: true);
			}
			if (flag8)
			{
				go_LeftUp.SetActive(value: true);
			}
		}
		gameObject.transform.position = vector;
		gameObject.transform.localRotation = Quaternion.identity;
		if (Random.value <= ornamentChance)
		{
			Object.Instantiate(pfb_Ornaments[Random.Range(0, pfb_Ornaments.Length)], vector + new Vector3(Random.Range(-0.2f, 0.2f), 0f, Random.Range(-0.2f, 0.1f)), Tool2D.GetRotation());
		}
	}
}
