using System;
using UnityEngine;

public class SpecialObj302EndlessStore : MonoBehaviour, IRoomObjExtraData, IComparable
{
	public GameObject Model;

	public SpriteRenderer sr_Lock;

	private bool recordedToSpawner;

	public int index { get; private set; }

	private void OnEnable()
	{
	}

	private void Show()
	{
		Model.SetActive(value: true);
	}

	public void Hide()
	{
		Model.SetActive(value: false);
	}

	private void OnDisable()
	{
		EventMgr.EndlessStageStart = (Action)Delegate.Remove(EventMgr.EndlessStageStart, new Action(Hide));
		EventMgr.EndlessStageClear = (Action)Delegate.Remove(EventMgr.EndlessStageClear, new Action(Show));
	}

	private void Update()
	{
		if (!recordedToSpawner)
		{
			recordedToSpawner = true;
			SpecialObj301EndlessMonsterSpawner.Inst.storeBasePoints.Add(this);
		}
	}

	public void SetLock(bool locked)
	{
		sr_Lock.enabled = locked;
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		index = Mathf.CeilToInt(data1);
	}

	public int CompareTo(object obj)
	{
		return index.CompareTo((obj as SpecialObj302EndlessStore).index);
	}
}
