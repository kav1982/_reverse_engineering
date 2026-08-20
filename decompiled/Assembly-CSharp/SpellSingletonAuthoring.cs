using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class SpellSingletonAuthoring : MonoBehaviour
{
	public class SpellSingletonBaker : Baker<SpellSingletonAuthoring>
	{
		public override void Bake(SpellSingletonAuthoring authoring)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (GameObject prefab in authoring.Prefabs)
			{
				if (!hashSet.Add(prefab.name))
				{
					Debug.LogError("SpellSingleton中存在重复名称的预制体 " + prefab.name);
				}
			}
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			DynamicBuffer<SpellPrefabBuffer> dynamicBuffer = AddBuffer<SpellPrefabBuffer>(entity);
			foreach (GameObject item in authoring.Prefabs.Where((GameObject prefab) => prefab))
			{
				dynamicBuffer.Add(new SpellPrefabBuffer
				{
					Id = item.name,
					Prefab = GetEntity(item, TransformUsageFlags.Dynamic)
				});
			}
			DynamicBuffer<SpellEffectBuffer> dynamicBuffer2 = AddBuffer<SpellEffectBuffer>(entity);
			foreach (SpellEffectSettingsById spellEffect in authoring.SpellEffects)
			{
				SpellEffectSettings[] effects = spellEffect.Effects;
				foreach (SpellEffectSettings spellEffectSettings in effects)
				{
					dynamicBuffer2.Add(new SpellEffectBuffer
					{
						SpellId = spellEffect.SpellId,
						Effect = spellEffectSettings.Bake()
					});
				}
			}
			SpellSingletonTag component = default(SpellSingletonTag);
			AddComponent(entity, in component);
		}
	}

	public struct SpellPrefabBuffer : IBufferElementData
	{
		public FixedString64Bytes Id;

		public Entity Prefab;
	}

	[Serializable]
	public struct SpellEffectSettingsById
	{
		public int SpellId;

		public SpellEffectSettings[] Effects;
	}

	[Serializable]
	public struct SpellEffectSettings
	{
		public string Name;

		public LayerCorrectType Layer;

		public float DestroyDelay;

		public SpellEffectSystem.ScaleMode ScaleMode;

		public bool AutoCreate;

		public bool IgnoreColor;

		public bool ClearTrail;

		public bool ClearParticle;

		public bool UseLowFpsOptimize;

		public int MaxInPoolCount;

		public SpellEffect Bake()
		{
			SpellEffect result = default(SpellEffect);
			result.Layer = Layer;
			result.DestroyDelay = DestroyDelay;
			result.ScaleMode = ScaleMode;
			result.AutoCreate = AutoCreate;
			result.IgnoreColor = IgnoreColor;
			result.Name = Name;
			result.ClearTrail = ClearTrail;
			result.ClearParticle = ClearParticle;
			result.UseLowFpsOptimize = UseLowFpsOptimize;
			result.MaxInPoolCount = MaxInPoolCount;
			return result;
		}
	}

	public struct SpellEffectBuffer : IBufferElementData
	{
		public int SpellId;

		public SpellEffect Effect;
	}

	[FormerlySerializedAs("SpellPrefabs")]
	[Space(10f)]
	public List<GameObject> Prefabs;

	public List<SpellEffectSettingsById> SpellEffects;

	[HideInInspector]
	public int Editor_SelectedSpellId = 1001;
}
