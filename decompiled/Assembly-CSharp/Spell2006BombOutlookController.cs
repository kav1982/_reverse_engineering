using System;
using UnityEngine;

public class Spell2006BombOutlookController : MonoBehaviour
{
	public Transform Teammate1Trans;

	public Transform Teammate2Trans;

	public Transform Teammate3Trans;

	public Transform Teammate4Trans;

	public Transform Teammate5Trans;

	public Transform Teammate6Trans;

	public Transform Teammate7Trans;

	public Transform Teammate2NormalModeTrans;

	public Transform Teammate2SafeModeTrans;

	public GameObject[] t1ColorObj;

	public GameObject[] t2ColorObj;

	public GameObject[] t2sColorObj;

	public GameObject[] t3ColorObj;

	public GameObject[] t4ColorObj;

	public GameObject[] t5ColorObj;

	public GameObject[] t6ColorObj;

	public GameObject[] t7ColorObj;

	private void OnEnable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
		SetSafeMode();
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	public void SetSafeMode()
	{
		Teammate2NormalModeTrans.gameObject.SetActive(!DataMgr.settingData.SafeMode);
		Teammate2SafeModeTrans.gameObject.SetActive(DataMgr.settingData.SafeMode);
	}

	public void InitializeOutlookDots(TeammateType type, SpellColorType color)
	{
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(value: false);
		}
		switch (type)
		{
		case TeammateType.teammate1:
		{
			Teammate1Trans.gameObject.SetActive(value: true);
			GameObject[] array = t1ColorObj;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			switch (color)
			{
			case SpellColorType.Frozen:
				t1ColorObj[0].SetActive(value: true);
				break;
			case SpellColorType.Void:
				t1ColorObj[1].SetActive(value: true);
				break;
			case SpellColorType.Mucus:
				t1ColorObj[2].SetActive(value: true);
				break;
			case SpellColorType.Player:
				t1ColorObj[3].SetActive(value: true);
				break;
			case SpellColorType.Venom:
				t1ColorObj[4].SetActive(value: true);
				break;
			case SpellColorType.Fire:
				t1ColorObj[5].SetActive(value: true);
				break;
			case SpellColorType.Thunder:
				t1ColorObj[6].SetActive(value: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("color", color, null);
			}
			break;
		}
		case TeammateType.teammate2:
		{
			Teammate2Trans.gameObject.SetActive(value: true);
			GameObject[] array = t2ColorObj;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			array = t2sColorObj;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			switch (color)
			{
			case SpellColorType.Frozen:
				t2ColorObj[0].SetActive(value: true);
				t2sColorObj[0].SetActive(value: true);
				break;
			case SpellColorType.Void:
				t2ColorObj[2].SetActive(value: true);
				t2sColorObj[2].SetActive(value: true);
				break;
			case SpellColorType.Mucus:
				t2ColorObj[3].SetActive(value: true);
				t2sColorObj[3].SetActive(value: true);
				break;
			case SpellColorType.Player:
			case SpellColorType.Thunder:
				t2ColorObj[1].SetActive(value: true);
				t2sColorObj[1].SetActive(value: true);
				break;
			case SpellColorType.Venom:
				t2ColorObj[4].SetActive(value: true);
				t2sColorObj[4].SetActive(value: true);
				break;
			case SpellColorType.Fire:
				t2ColorObj[5].SetActive(value: true);
				t2sColorObj[5].SetActive(value: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("color", color, null);
			}
			break;
		}
		case TeammateType.teammate3:
		{
			Teammate3Trans.gameObject.SetActive(value: true);
			GameObject[] array = t3ColorObj;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			switch (color)
			{
			case SpellColorType.Frozen:
				t3ColorObj[4].SetActive(value: true);
				break;
			case SpellColorType.Void:
				t3ColorObj[1].SetActive(value: true);
				break;
			case SpellColorType.Mucus:
				t3ColorObj[3].SetActive(value: true);
				break;
			case SpellColorType.Player:
			case SpellColorType.Thunder:
				t3ColorObj[0].SetActive(value: true);
				break;
			case SpellColorType.Venom:
				t3ColorObj[2].SetActive(value: true);
				break;
			case SpellColorType.Fire:
				t3ColorObj[5].SetActive(value: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("color", color, null);
			}
			break;
		}
		case TeammateType.teammate4:
		{
			Teammate4Trans.gameObject.SetActive(value: true);
			GameObject[] array = t4ColorObj;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			switch (color)
			{
			case SpellColorType.Frozen:
				t4ColorObj[4].SetActive(value: true);
				break;
			case SpellColorType.Void:
				t4ColorObj[1].SetActive(value: true);
				break;
			case SpellColorType.Mucus:
				t4ColorObj[3].SetActive(value: true);
				break;
			case SpellColorType.Player:
			case SpellColorType.Thunder:
				t4ColorObj[0].SetActive(value: true);
				break;
			case SpellColorType.Venom:
				t4ColorObj[2].SetActive(value: true);
				break;
			case SpellColorType.Fire:
				t4ColorObj[5].SetActive(value: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("color", color, null);
			}
			break;
		}
		case TeammateType.teammate5:
		{
			Teammate5Trans.gameObject.SetActive(value: true);
			GameObject[] array = t5ColorObj;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			switch (color)
			{
			case SpellColorType.Frozen:
				t5ColorObj[4].SetActive(value: true);
				break;
			case SpellColorType.Void:
				t5ColorObj[1].SetActive(value: true);
				break;
			case SpellColorType.Mucus:
				t5ColorObj[3].SetActive(value: true);
				break;
			case SpellColorType.Player:
			case SpellColorType.Thunder:
				t5ColorObj[0].SetActive(value: true);
				break;
			case SpellColorType.Venom:
				t5ColorObj[2].SetActive(value: true);
				break;
			case SpellColorType.Fire:
				t5ColorObj[5].SetActive(value: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("color", color, null);
			}
			break;
		}
		case TeammateType.teammate6:
		{
			Teammate6Trans.gameObject.SetActive(value: true);
			GameObject[] array = t6ColorObj;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			switch (color)
			{
			case SpellColorType.Frozen:
				t6ColorObj[4].SetActive(value: true);
				break;
			case SpellColorType.Void:
				t6ColorObj[1].SetActive(value: true);
				break;
			case SpellColorType.Mucus:
				t6ColorObj[3].SetActive(value: true);
				break;
			case SpellColorType.Thunder:
				t6ColorObj[6].SetActive(value: true);
				break;
			case SpellColorType.Player:
				t6ColorObj[0].SetActive(value: true);
				break;
			case SpellColorType.Venom:
				t6ColorObj[2].SetActive(value: true);
				break;
			case SpellColorType.Fire:
				t6ColorObj[5].SetActive(value: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("color", color, null);
			}
			break;
		}
		case TeammateType.teammate7:
		{
			Teammate7Trans.gameObject.SetActive(value: true);
			GameObject[] array = t7ColorObj;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			switch (color)
			{
			case SpellColorType.Frozen:
				t7ColorObj[4].SetActive(value: true);
				break;
			case SpellColorType.Void:
				t7ColorObj[1].SetActive(value: true);
				break;
			case SpellColorType.Mucus:
				t7ColorObj[3].SetActive(value: true);
				break;
			case SpellColorType.Thunder:
				t7ColorObj[6].SetActive(value: true);
				break;
			case SpellColorType.Player:
				t7ColorObj[0].SetActive(value: true);
				break;
			case SpellColorType.Venom:
				t7ColorObj[2].SetActive(value: true);
				break;
			case SpellColorType.Fire:
				t7ColorObj[5].SetActive(value: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("color", color, null);
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}
}
