using System.Collections.Generic;
using UnityEngine;

public abstract class SpellChargeEffectBase : MonoBehaviour
{
	public Transform AttachTarget;

	public bool IsSkipHolding;

	public int SpellId;

	public SpellColorType? ColorType;

	private bool firstFrameDone;

	protected readonly Dictionary<string, GameObject> Effects = new Dictionary<string, GameObject>();

	protected virtual void OnDisable()
	{
		foreach (KeyValuePair<string, GameObject> effect in Effects)
		{
			effect.Deconstruct(out var _, out var value);
			GameObject go = value;
			ObjPoolMgr.Inst.RecycleGO(go);
		}
		Effects.Clear();
		ColorType = null;
		firstFrameDone = false;
	}

	protected virtual void Update()
	{
		if (!firstFrameDone)
		{
			firstFrameDone = true;
			OnFirstFrame();
		}
		base.transform.position = AttachTarget.position;
		foreach (KeyValuePair<string, GameObject> effect in Effects)
		{
			effect.Deconstruct(out var _, out var value);
			value.transform.position = AttachTarget.position;
		}
	}

	protected virtual void OnFirstFrame()
	{
	}

	protected virtual string GetRelationPrefabPath(string type)
	{
		string text = string.Format("{0}{1}/{2}_{3}", "Prefabs/Spell/", SpellId, SpellId, type);
		if (ColorType.HasValue)
		{
			string text2 = text;
			SpellColorType? colorType = ColorType;
			text = text2 + "_" + colorType.ToString();
		}
		return text;
	}

	protected virtual GameObject CreateEffect(string type)
	{
		if (Effects.ContainsKey(type))
		{
			Debug.LogError("已经存在蓄力特效 " + type + " 了");
			return null;
		}
		string relationPrefabPath = GetRelationPrefabPath(type);
		GameObject gO = ObjPoolMgr.Inst.GetGO(relationPrefabPath, base.transform.position);
		Effects.Add(type, gO);
		return gO;
	}

	protected virtual void RemoveEffect(string type, float delay)
	{
		GameObject gameObject = Effects[type];
		if (gameObject.TryGetComponent<ParticleSystem>(out var component))
		{
			component.Stop(withChildren: true);
		}
		ObjPoolMgr.Inst.RecycleGO(gameObject, delay);
		Effects.Remove(type);
	}

	public abstract void ChangeStage(int stage);

	public abstract void Release();
}
