using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateInGroup(typeof(SpellEffectSystemGroup))]
public class SpellEffectSystem : SystemBase
{
	private struct SyncData
	{
		public Entity Entity;

		public Transform Transform;

		public SpellEffect Settings;
	}

	public struct Require : IBufferElementData
	{
		public int SpellId;

		public Entity Entity;

		public FixedString32Bytes Color;

		public SpellEffect Settings;
	}

	public struct UnfollowingRequire : IBufferElementData
	{
		public int SpellId;

		public quaternion StartRotation;

		public float3 StartPosition;

		public FixedString32Bytes Color;

		public float Scale;

		public SpellEffect Settings;
	}

	public struct Destroy : IBufferElementData
	{
		public FixedString32Bytes Name;

		public Entity Entity;
	}

	public enum ScaleMode
	{
		Scale,
		Radius,
		Ignore
	}

	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		public ComponentLookup<SpellComponentData> __SpellComponentData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__SpellComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellComponentData>();
		}
	}

	private static SpellEffectSystem _inst;

	private readonly List<SyncData> _syncData = new List<SyncData>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_874131577_0;

	private EntityQuery __query_874131577_1;

	private EntityQuery __query_874131577_2;

	private EntityQuery __query_874131577_3;

	[Preserve]
	protected override void OnCreate()
	{
		_inst = this;
		base.EntityManager.CreateSingletonBuffer<Require>();
		base.EntityManager.CreateSingletonBuffer<Destroy>();
		base.EntityManager.CreateSingletonBuffer<UnfollowingRequire>();
	}

	private GameObject SpawnEffect(Require require)
	{
		if (!base.EntityManager.HasComponent<LocalTransform>(require.Entity))
		{
			return null;
		}
		string text = string.Format("{0}{1}/{2}_{3}", "Prefabs/Spell/", require.SpellId, require.SpellId, require.Settings.Name);
		if (!require.Settings.IgnoreColor)
		{
			text += $"_{require.Color}";
			if (GameMgr.IsChAge14_Static && require.Color == "Monster" && ABResources.Exists(text + "_H"))
			{
				text += "_H";
			}
		}
		if (!ABResources.Exists(text))
		{
			return null;
		}
		float3 position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, require.Entity).Position;
		GameObject gO = ObjPoolMgr.Inst.GetGO(text, Tool2D.GetLayerPoint(position, require.Settings.Layer), 0f, null, require.Settings.MaxInPoolCount, (require.Settings.MaxInPoolCount > 0) ? $"{require.SpellId}_{require.Settings.Name}" : null);
		if (gO == null)
		{
			return null;
		}
		if (require.Settings.ClearTrail)
		{
			TrailRenderer[] componentsInChildren = gO.GetComponentsInChildren<TrailRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Clear();
			}
			if (gO.TryGetComponent<TrailRenderer>(out var component))
			{
				component.Clear();
			}
		}
		if (require.Settings.ClearParticle)
		{
			ParticleSystem[] componentsInChildren2 = gO.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].Clear();
			}
			if (gO.TryGetComponent<ParticleSystem>(out var component2))
			{
				component2.Clear();
			}
		}
		return gO;
	}

	private void RecycleEffect(SyncData data)
	{
		if ((bool)data.Transform)
		{
			ObjPoolMgr.Inst.RecycleGO(data.Transform.gameObject, data.Settings.DestroyDelay);
		}
	}

	[Preserve]
	protected override void OnUpdate()
	{
		ProcessDestroyEffect();
		ProcessNewEffect();
		ProcessUnfollowingEffect();
		for (int num = _syncData.Count - 1; num >= 0; num--)
		{
			SyncData data = _syncData[num];
			if (!data.Transform)
			{
				_syncData.RemoveAt(num);
			}
			else if (!base.EntityManager.HasComponent<LocalTransform>(data.Entity))
			{
				_syncData.RemoveAt(num);
				RecycleEffect(data);
			}
			else
			{
				LocalTransform componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, data.Entity);
				data.Transform.position = Tool2D.GetLayerPoint(componentAfterCompletingDependency.Position, data.Settings.Layer);
				switch (data.Settings.ScaleMode)
				{
				case ScaleMode.Radius:
					data.Transform.localScale = Vector3.one * InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref base.CheckedStateRef, data.Entity).Radius.Calculate();
					break;
				case ScaleMode.Scale:
					data.Transform.localScale = Vector3.one * componentAfterCompletingDependency.Scale;
					break;
				}
			}
		}
	}

	private void ProcessNewEffect()
	{
		DynamicBuffer<Require> singletonBuffer = __query_874131577_0.GetSingletonBuffer<Require>();
		DynamicOptimizeData singleton = __query_874131577_1.GetSingleton<DynamicOptimizeData>();
		foreach (Require item in singletonBuffer)
		{
			if (item.Settings.UseLowFpsOptimize && UnityEngine.Random.Range(0f, 1f) >= singleton.PoolEffectShowRatio)
			{
				continue;
			}
			if (item.Entity == Entity.Null)
			{
				Debug.Log("生成了一个空的entity，id为" + item.SpellId);
				continue;
			}
			GameObject gameObject = SpawnEffect(item);
			if ((object)gameObject != null)
			{
				if (base.EntityManager.HasBuffer<SpellGameObjectEffectLink>(item.Entity))
				{
					base.EntityManager.GetBuffer<SpellGameObjectEffectLink>(item.Entity).Add(new SpellGameObjectEffectLink
					{
						EffectName = item.Settings.Name,
						GameObject = gameObject
					});
				}
				if (item.Settings.Name.Value == "Trail")
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref base.CheckedStateRef, item.Entity).ValueRW.TrailEffectGameObject = gameObject;
				}
				else if (item.Settings.Name.Value == "Spell")
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref base.CheckedStateRef, item.Entity).ValueRW.SpellEffectGameObject = gameObject;
				}
				_inst._syncData.Add(new SyncData
				{
					Entity = item.Entity,
					Transform = gameObject.transform,
					Settings = item.Settings
				});
			}
		}
		singletonBuffer.Clear();
	}

	private void ProcessUnfollowingEffect()
	{
		DynamicBuffer<UnfollowingRequire> singletonBuffer = __query_874131577_2.GetSingletonBuffer<UnfollowingRequire>();
		DynamicOptimizeData singleton = __query_874131577_1.GetSingleton<DynamicOptimizeData>();
		foreach (UnfollowingRequire item in singletonBuffer)
		{
			UnfollowingRequire current = item;
			if (current.Settings.UseLowFpsOptimize && UnityEngine.Random.Range(0f, 1f) >= singleton.PoolEffectShowRatio)
			{
				continue;
			}
			string text = string.Format("{0}{1}/{2}_{3}", "Prefabs/Spell/", current.SpellId, current.SpellId, current.Settings.Name);
			if (!current.Settings.IgnoreColor)
			{
				text += $"_{current.Color}";
				if (GameMgr.IsChAge14_Static && current.Color == "Monster" && ABResources.Exists(text + "_H"))
				{
					text += "_H";
				}
			}
			GameObject gO = ObjPoolMgr.Inst.GetGO(text, Tool2D.GetLayerPoint(current.StartPosition, current.Settings.Layer));
			if (current.Settings.DestroyDelay > 0.001f)
			{
				ObjPoolMgr.Inst.RecycleGO(gO, current.Settings.DestroyDelay);
			}
			gO.transform.rotation = current.StartRotation;
			gO.transform.localScale = Vector3.one * current.Scale;
			if (current.Settings.ClearTrail)
			{
				TrailRenderer[] componentsInChildren = gO.GetComponentsInChildren<TrailRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].Clear();
				}
			}
			if (current.Settings.ClearParticle)
			{
				ParticleSystem[] componentsInChildren2 = gO.GetComponentsInChildren<ParticleSystem>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].Clear();
				}
			}
		}
		singletonBuffer.Clear();
	}

	private void ProcessDestroyEffect()
	{
		DynamicBuffer<Destroy> singletonBuffer = __query_874131577_3.GetSingletonBuffer<Destroy>();
		foreach (Destroy item in singletonBuffer)
		{
			Destroy current = item;
			for (int num = _syncData.Count - 1; num >= 0; num--)
			{
				if (_syncData[num].Entity == current.Entity)
				{
					SyncData syncData = _syncData[num];
					if (syncData.Settings.Name == current.Name)
					{
						if ((bool)_syncData[num].Transform)
						{
							ObjPoolMgr.Inst.RecycleGO(_syncData[num].Transform.gameObject, _syncData[num].Settings.DestroyDelay);
						}
						_syncData.RemoveAt(num);
						break;
					}
				}
			}
		}
		singletonBuffer.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_874131577_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_874131577_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<UnfollowingRequire>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_874131577_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Destroy>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_874131577_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	protected override void OnCreateForCompiler()
	{
		base.OnCreateForCompiler();
		__AssignQueries(ref base.CheckedStateRef);
		__TypeHandle.__AssignHandles(ref base.CheckedStateRef);
	}

	[Preserve]
	public SpellEffectSystem()
	{
	}
}
