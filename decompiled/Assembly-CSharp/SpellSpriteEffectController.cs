using SpriteEffectSystem;
using UnityEngine;

public class SpellSpriteEffectController : SpriteEffectController
{
	public SpriteEffectAnima OnOverTrigger;

	public SpriteEffectAnima OnOverSplitTrigger;

	public SpriteEffectAnima OnMoveTrigger;

	private static SpellSpriteEffectController _inst;

	public static SpellSpriteEffectController Inst
	{
		get
		{
			if (_inst == null)
			{
				_inst = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/SpellSpriteEffectController")).GetComponent<SpellSpriteEffectController>();
			}
			return _inst;
		}
	}

	public void PlayEffectIgnoreSpellBase(SpriteEffectAnima anima, EffectPlayParam param)
	{
		PlayEffect(anima, param);
	}

	public void PlayOnOverTriggerEffect(Vector3 position, Vector3 rotation)
	{
		PlayEffect(OnOverTrigger, new EffectPlayParam
		{
			Position = position,
			Rotation = Quaternion.LookRotation(rotation) * Quaternion.Euler(0f, -90f, 0f),
			FilpY = (Random.Range(0, 2) == 0),
			Color = new Color(1f, 1f, 1f, DataMgr.settingData.SpellTransparent)
		});
	}

	public void PlayOnOverSplitTriggerEffect(Vector3 position)
	{
		PlayEffect(OnOverSplitTrigger, new EffectPlayParam
		{
			Position = position,
			Rotation = Quaternion.Euler(0f, 0f, Random.Range(-360f, 360f)),
			Scale = Vector3.one * Random.Range(0.9f, 0.8f),
			Color = new Color(1f, 1f, 1f, DataMgr.settingData.SpellTransparent)
		});
	}

	public void PlayOnMoveTriggerEffect(Vector3 position, Vector3 rotation)
	{
		PlayEffect(OnMoveTrigger, new EffectPlayParam
		{
			Position = position,
			Rotation = Quaternion.LookRotation(rotation) * Quaternion.Euler(0f, -90f, 0f),
			FilpY = (Random.Range(0, 2) == 0),
			Scale = new Vector3(0.5f, 0.5f, 1f),
			Color = new Color(1f, 1f, 1f, DataMgr.settingData.SpellTransparent)
		});
	}
}
