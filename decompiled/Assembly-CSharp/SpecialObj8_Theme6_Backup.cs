using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObj8_Theme6_Backup : MonoBehaviour, IRoomCtrller
{
	public GameObject go_ColliderUpRight;

	public GameObject go_ColliderRightDown;

	public GameObject go_ColliderDownLeft;

	public GameObject go_ColliderLeftUp;

	public SpriteRenderer sr;

	public SpriteRenderer sr_Water;

	public Transform tsf_Tsf;

	public BoxCollider bc;

	[Header("sprite")]
	public Sprite[] sprite_Corner_RD;

	public Sprite[] sprite_Corner_UR;

	public Sprite[] sprite_Full;

	public Sprite[] sprite_LUR;

	public Sprite[] sprite_RD;

	public Sprite[] sprite_RDL;

	public Sprite[] sprite_UR;

	public Sprite[] sprite_URD;

	[Header("Water")]
	public Sprite sprite_Water_Corner_RD;

	public Sprite sprite_Water_Corner_UR;

	public Sprite sprite_Water_Full;

	public Sprite sprite_Water_LUR;

	public Sprite sprite_Water_RD;

	public Sprite sprite_Water_RDL;

	public Sprite sprite_Water_UR;

	public Sprite sprite_Water_URD;

	private void Start()
	{
		tsf_Tsf.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.SO8_Abyss);
		RoomController component = base.transform.parent.parent.GetComponent<RoomController>();
		if (component == null)
		{
			Debug.LogError("!");
		}
		_ = component.roomCfg;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, 1f, "Abyss");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			if (collidersByTag[i].transform.position == base.transform.position + new Vector3(0f, 1f))
			{
				flag = true;
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(1f, 1f))
			{
				flag2 = true;
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(1f, 0f))
			{
				flag3 = true;
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(1f, -1f))
			{
				flag4 = true;
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(0f, -1f))
			{
				flag5 = true;
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(-1f, -1f))
			{
				flag6 = true;
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(-1f, 0f))
			{
				flag7 = true;
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(-1f, 1f))
			{
				flag8 = true;
			}
		}
		if (flag && flag3 && !flag5 && !flag7)
		{
			sr.sprite = sprite_UR[Random.Range(0, sprite_UR.Length)];
			sr_Water.sprite = sprite_Water_UR;
			StartCoroutine(EnableCollider(go_ColliderUpRight));
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_RD[Random.Range(0, sprite_RD.Length)];
			sr_Water.sprite = sprite_Water_RD;
			StartCoroutine(EnableCollider(go_ColliderRightDown));
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_RD[Random.Range(0, sprite_RD.Length)];
			sr_Water.sprite = sprite_Water_RD;
			sr.flipX = true;
			sr_Water.flipX = true;
			StartCoroutine(EnableCollider(go_ColliderDownLeft));
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_UR[Random.Range(0, sprite_UR.Length)];
			sr_Water.sprite = sprite_Water_UR;
			sr.flipX = true;
			sr_Water.flipX = true;
			StartCoroutine(EnableCollider(go_ColliderLeftUp));
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_LUR[Random.Range(0, sprite_LUR.Length)];
			sr_Water.sprite = sprite_Water_LUR;
			StartCoroutine(EnableCollider(go_ColliderLeftUp, go_ColliderUpRight));
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_URD[Random.Range(0, sprite_URD.Length)];
			sr_Water.sprite = sprite_Water_URD;
			StartCoroutine(EnableCollider(go_ColliderUpRight, go_ColliderRightDown));
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_RDL[Random.Range(0, sprite_RDL.Length)];
			sr_Water.sprite = sprite_Water_RDL;
			StartCoroutine(EnableCollider(go_ColliderRightDown, go_ColliderDownLeft));
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_URD[Random.Range(0, sprite_URD.Length)];
			sr_Water.sprite = sprite_Water_URD;
			sr.flipX = true;
			sr_Water.flipX = true;
			StartCoroutine(EnableCollider(go_ColliderDownLeft, go_ColliderLeftUp));
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_Full[Random.Range(0, sprite_Full.Length)];
			sr.transform.position += new Vector3(0f, 0f, -0.01f);
			sr_Water.sprite = sprite_Water_Full;
			if (flag2)
			{
				StartCoroutine(EnableCollider(go_ColliderUpRight));
			}
			else
			{
				sr.sprite = sprite_Corner_UR[Random.Range(0, sprite_Corner_UR.Length)];
				sr_Water.sprite = sprite_Water_Corner_UR;
			}
			if (flag4)
			{
				StartCoroutine(EnableCollider(go_ColliderRightDown));
			}
			else
			{
				sr.sprite = sprite_Corner_RD[Random.Range(0, sprite_Corner_RD.Length)];
				sr_Water.sprite = sprite_Water_Corner_RD;
			}
			if (flag6)
			{
				StartCoroutine(EnableCollider(go_ColliderDownLeft));
			}
			else
			{
				sr.sprite = sprite_Corner_RD[Random.Range(0, sprite_Corner_RD.Length)];
				sr_Water.sprite = sprite_Water_Corner_RD;
				sr.flipX = true;
				sr_Water.flipX = true;
			}
			if (flag8)
			{
				StartCoroutine(EnableCollider(go_ColliderLeftUp));
			}
			else
			{
				sr.sprite = sprite_Corner_UR[Random.Range(0, sprite_Corner_UR.Length)];
				sr_Water.sprite = sprite_Water_Corner_UR;
				sr.flipX = true;
				sr_Water.flipX = true;
			}
		}
		Object.Destroy(bc);
	}

	private IEnumerator EnableCollider(params GameObject[] colliderGO)
	{
		yield return null;
		for (int i = 0; i < colliderGO.Length; i++)
		{
			colliderGO[i].SetActive(value: true);
		}
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		roomCtrller.AbyssRegister(base.gameObject);
	}
}
