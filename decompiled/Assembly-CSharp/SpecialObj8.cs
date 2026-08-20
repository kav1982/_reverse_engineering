using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObj8 : MonoBehaviour, IRoomCtrller
{
	public GameObject go_ColliderUpRight;

	public GameObject go_ColliderRightDown;

	public GameObject go_ColliderDownLeft;

	public GameObject go_ColliderLeftUp;

	public SpriteRenderer sr;

	public Transform tsf_Layer;

	public BoxCollider bc;

	[Header("Texture")]
	public SpriteArray[] themeSprite_CornerRightDown;

	public SpriteArray[] themeSprite_CornerUpRight;

	public SpriteArray[] themeSprite_Full;

	public SpriteArray[] themeSprite_LeftUpRight;

	public SpriteArray[] themeSprite_RightDown;

	public SpriteArray[] themeSprite_RightDownLeft;

	public SpriteArray[] themeSprite_UpRight;

	public SpriteArray[] themeSprite_UpRightDown;

	private void Start()
	{
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.SO8_Abyss);
		RoomController component = base.transform.parent.parent.GetComponent<RoomController>();
		if (component == null)
		{
			Debug.LogError("!");
		}
		RoomThemeType themeType = component.roomCfg.themeType;
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
			sr.sprite = themeSprite_UpRight[(int)themeType].RandomSprite();
			StartCoroutine(EnableCollider(go_ColliderUpRight));
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = themeSprite_RightDown[(int)themeType].RandomSprite();
			StartCoroutine(EnableCollider(go_ColliderRightDown));
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = themeSprite_RightDown[(int)themeType].RandomSprite();
			sr.transform.localScale = new Vector3(-1f, 1f, 1f);
			StartCoroutine(EnableCollider(go_ColliderDownLeft));
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr.sprite = themeSprite_UpRight[(int)themeType].RandomSprite();
			sr.transform.localScale = new Vector3(-1f, 1f, 1f);
			StartCoroutine(EnableCollider(go_ColliderLeftUp));
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr.sprite = themeSprite_LeftUpRight[(int)themeType].RandomSprite();
			StartCoroutine(EnableCollider(go_ColliderLeftUp, go_ColliderUpRight));
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = themeSprite_UpRightDown[(int)themeType].RandomSprite();
			StartCoroutine(EnableCollider(go_ColliderUpRight, go_ColliderRightDown));
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr.sprite = themeSprite_RightDownLeft[(int)themeType].RandomSprite();
			StartCoroutine(EnableCollider(go_ColliderRightDown, go_ColliderDownLeft));
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = themeSprite_UpRightDown[(int)themeType].RandomSprite();
			sr.transform.localScale = new Vector3(-1f, 1f, 1f);
			StartCoroutine(EnableCollider(go_ColliderDownLeft, go_ColliderLeftUp));
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			sr.sprite = themeSprite_Full[(int)themeType].RandomSprite();
			sr.transform.position += new Vector3(0f, 0f, -0.01f);
			if (flag2)
			{
				StartCoroutine(EnableCollider(go_ColliderUpRight));
			}
			else
			{
				sr.sprite = themeSprite_CornerUpRight[(int)themeType].RandomSprite();
			}
			if (flag4)
			{
				StartCoroutine(EnableCollider(go_ColliderRightDown));
			}
			else
			{
				sr.sprite = themeSprite_CornerRightDown[(int)themeType].RandomSprite();
			}
			if (flag6)
			{
				StartCoroutine(EnableCollider(go_ColliderDownLeft));
			}
			else
			{
				sr.sprite = themeSprite_CornerRightDown[(int)themeType].RandomSprite();
				sr.flipX = true;
			}
			if (flag8)
			{
				StartCoroutine(EnableCollider(go_ColliderLeftUp));
			}
			else
			{
				sr.sprite = themeSprite_CornerUpRight[(int)themeType].RandomSprite();
				sr.flipX = true;
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
		Object.Destroy(this);
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		roomCtrller.AbyssRegister(base.gameObject);
	}
}
