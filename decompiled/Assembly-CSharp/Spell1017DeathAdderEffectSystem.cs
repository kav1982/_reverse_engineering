using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SpellEffectSystemGroup))]
[CompilerGenerated]
[UpdateAfter(typeof(SpellEffectSystem))]
[BurstCompile]
public struct Spell1017DeathAdderEffectSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1462804979_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1017DeathAdderEffectData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1017DeathAdderEffectData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1017DeathAdderEffectData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1017DeathAdderEffectData> item1_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1017DeathAdderEffectData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1017DeathAdderEffectData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1017DeathAdderEffectData>> Current => _resolvedChunk.Get(_currentEntityIndex);

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Enumerator(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
			{
				if (!entityQuery.IsEmptyIgnoreFilter)
				{
					CompleteDependencies(ref state);
					typeHandle.Update(ref state);
				}
				_entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
				_currentEntityIndex = -1;
				_endEntityIndex = -1;
				_typeHandle = typeHandle;
				_resolvedChunk = default(ResolvedChunk);
			}

			public void Dispose()
			{
				_entityQueryEnumerator.Dispose();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				_currentEntityIndex++;
				if (_currentEntityIndex >= _endEntityIndex)
				{
					if (_entityQueryEnumerator.MoveNextEntityRange(out var movedToNewChunk, out var chunk, out var entityStartIndex, out var entityEndIndex))
					{
						if (movedToNewChunk)
						{
							_resolvedChunk = _typeHandle.Resolve(chunk);
						}
						_currentEntityIndex = entityStartIndex;
						_endEntityIndex = entityEndIndex;
						return true;
					}
					return false;
				}
				return true;
			}

			public Enumerator GetEnumerator()
			{
				return this;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator Query(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
		{
			return new Enumerator(entityQuery, typeHandle, ref state);
		}

		public static void CompleteDependencies(ref SystemState state)
		{
			state.EntityManager.CompleteDependencyBeforeRW<Spell1017DeathAdderEffectData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1462804979_0.TypeHandle __IFE_1462804979_0_TypeHandle;

		[ReadOnly]
		public BufferLookup<SpellGameObjectEffectLink> __SpellGameObjectEffectLink_RO_BufferLookup;

		public BufferLookup<SpellGameObjectEffectLink> __SpellGameObjectEffectLink_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1462804979_0_TypeHandle = new IFE_1462804979_0.TypeHandle(ref state);
			__SpellGameObjectEffectLink_RO_BufferLookup = state.GetBufferLookup<SpellGameObjectEffectLink>(isReadOnly: true);
			__SpellGameObjectEffectLink_RW_BufferLookup = state.GetBufferLookup<SpellGameObjectEffectLink>();
		}
	}

	private static readonly int ID_Dissolve = Shader.PropertyToID("_DissolveProcess");

	private const string FX_Explode = "Explode";

	private const string FX_Chain = "Chain";

	private const string FX_Charge = "Charge";

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1462804979_0;

	private EntityQuery __query_1462804979_1;

	private EntityQuery __query_1462804979_2;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell1017DeathAdderEffectData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_1462804979_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		SpellSingleton singleton = __query_1462804979_2.GetSingleton<SpellSingleton>();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1017DeathAdderEffectData>> item2 in IFE_1462804979_0.Query(__query_1462804979_0, __TypeHandle.__IFE_1462804979_0_TypeHandle, ref state))
		{
			item2.Deconstruct(out var item, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell1017DeathAdderEffectData> uncheckedRefRW = item;
			Entity entity2 = entity;
			if (!InternalCompilerInterface.HasBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RO_BufferLookup, ref state, entity2) || Time.timeScale == 0f)
			{
				continue;
			}
			float deltaTime = state.WorldUnmanaged.Time.DeltaTime;
			uncheckedRefRW.ValueRW.EffectExistTimer += deltaTime;
			DynamicBuffer<SpellGameObjectEffectLink> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2);
			TryInitExplode(uncheckedRefRW, bufferAfterCompletingDependency);
			TryInitChain(uncheckedRefRW, bufferAfterCompletingDependency);
			TryInitCharge(uncheckedRefRW, bufferAfterCompletingDependency);
			UpdateBezierShift(uncheckedRefRW, deltaTime);
			UpdateDissolveAndDrawLine(uncheckedRefRW, deltaTime);
			UpdateExplosionSpeed(uncheckedRefRW, deltaTime);
			if (!uncheckedRefRW.ValueRO.GroundEffectCreated)
			{
				uncheckedRefRW.ValueRW.GroundEffectCreated = true;
				FixedString32Bytes effectName = "Ground";
				if (singleton.TryGetSpellEffectEntity(1017, in effectName, uncheckedRefRW.ValueRO.ColorType, out var entity3))
				{
					Entity e = entityCommandBuffer.Instantiate(entity3);
					float3 layerPosition = DTool.GetLayerPosition(in uncheckedRefRW.ValueRO.BoomPosition, LayerCorrectType.GroundEffectLow);
					entityCommandBuffer.SetComponent(e, LocalTransform.FromPositionRotationScale(uncheckedRefRW.ValueRO.BoomPosition + layerPosition + new float3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.1f), quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360)), uncheckedRefRW.ValueRO.GroundScale * 2f));
					entityCommandBuffer.SetComponent(e, new Spell1017DeathAdderTraceProgress
					{
						Progress = 1f
					});
					entityCommandBuffer.SetComponent(e, new Spell1017DeathAdderTraceTransparency
					{
						Transparency = 1f
					});
				}
			}
			if (uncheckedRefRW.ValueRW.EffectExistTimer > 2f)
			{
				entityCommandBuffer.DestroyEntity(entity2);
			}
		}
	}

	private void TryInitExplode(RefRW<Spell1017DeathAdderEffectData> d, DynamicBuffer<SpellGameObjectEffectLink> links)
	{
		if (!d.ValueRO.ExplosionParticleCtrl && TryGetLinkEffect("Explode", links, out var linkedObject))
		{
			ParticleGroupPlayController component = linkedObject.GetComponent<ParticleGroupPlayController>();
			if ((bool)component)
			{
				d.ValueRW.ExplosionParticleCtrl = component;
			}
		}
	}

	private void TryInitCharge(RefRW<Spell1017DeathAdderEffectData> d, DynamicBuffer<SpellGameObjectEffectLink> links)
	{
		if (!d.ValueRO.ChargeParticleCtrl && TryGetLinkEffect("Charge", links, out var linkedObject))
		{
			ParticleGroupPlayController component = linkedObject.GetComponent<ParticleGroupPlayController>();
			if ((bool)component)
			{
				d.ValueRW.ChargeParticleCtrl = component;
			}
		}
	}

	private void TryInitChain(RefRW<Spell1017DeathAdderEffectData> d, DynamicBuffer<SpellGameObjectEffectLink> links)
	{
		if ((bool)d.ValueRO.ChainEffect || !TryGetLinkEffect("Chain", links, out var linkedObject))
		{
			return;
		}
		d.ValueRW.ChainEffect = linkedObject;
		Transform transform = linkedObject.transform.Find("Line");
		Transform transform2 = linkedObject.transform.Find("Shadow");
		if (!transform || !transform2)
		{
			return;
		}
		GameObject gameObject = transform.gameObject;
		GameObject gameObject2 = transform2.gameObject;
		if ((bool)gameObject && (bool)gameObject2)
		{
			LineRenderer component = gameObject.GetComponent<LineRenderer>();
			LineRenderer component2 = gameObject2.GetComponent<LineRenderer>();
			if ((bool)component && (bool)component2)
			{
				d.ValueRW.LineRenderer = component;
				d.ValueRW.ShadowLineRenderer = component2;
				float num2 = (component.startWidth = (component.endWidth = d.ValueRO.LineWidth));
				Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
				d.ValueRW.LerpPos1ShiftDirection = new Vector3(insideUnitSphere.x, insideUnitSphere.y, 0f - Mathf.Abs(insideUnitSphere.z));
				insideUnitSphere = UnityEngine.Random.insideUnitSphere;
				d.ValueRW.LerpPos2ShiftDirection = new Vector3(insideUnitSphere.x, insideUnitSphere.y, 0f - Mathf.Abs(insideUnitSphere.z));
				float num3 = Mathf.Min(6f, Vector3.Distance((Vector3)d.ValueRO.BeginPosition, (Vector3)d.ValueRO.BoomPosition));
				float3 @float = d.ValueRO.BoomPosition - d.ValueRO.BeginPosition;
				d.ValueRW.LerpPos1 = (Vector3)(d.ValueRO.BeginPosition + @float * UnityEngine.Random.Range(0.2f, 0.8f)) + UnityEngine.Random.insideUnitSphere.normalized * 0.1f * num3;
				d.ValueRW.LerpPos2 = (Vector3)(d.ValueRO.BeginPosition + @float * UnityEngine.Random.Range(0.2f, 0.8f)) + UnityEngine.Random.insideUnitSphere.normalized * 0.2f * num3;
				d.ValueRW.ExplosionSpeed = 1f;
			}
		}
	}

	private bool TryGetLinkEffect(FixedString32Bytes name, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, out GameObject linkedObject)
	{
		for (int i = 0; i < linkBuffer.Length; i++)
		{
			SpellGameObjectEffectLink spellGameObjectEffectLink = linkBuffer[i];
			if (spellGameObjectEffectLink.EffectName == name)
			{
				linkedObject = linkBuffer[i].GameObject.Value;
				linkBuffer.RemoveAt(i);
				return true;
			}
		}
		linkedObject = null;
		return false;
	}

	private void UpdateBezierShift(RefRW<Spell1017DeathAdderEffectData> d, float dt)
	{
		if (!(d.ValueRW.EffectExistTimer >= 0.5f))
		{
			float num = (0.5f - d.ValueRW.EffectExistTimer) / 0.5f;
			d.ValueRW.LerpPos1 += d.ValueRW.LerpPos1ShiftDirection * num * num * 0.8f;
			d.ValueRW.LerpPos2 += d.ValueRW.LerpPos2ShiftDirection * num * num * 0.8f;
		}
	}

	private void UpdateDissolveAndDrawLine(RefRW<Spell1017DeathAdderEffectData> d, float deltaTime)
	{
		if (!d.ValueRO.LineRenderer || !d.ValueRO.ShadowLineRenderer || !d.ValueRO.LineRenderer.Value || !d.ValueRO.ShadowLineRenderer.Value)
		{
			return;
		}
		LineRenderer value = d.ValueRW.LineRenderer.Value;
		LineRenderer value2 = d.ValueRW.ShadowLineRenderer.Value;
		Material material = value.material;
		Material material2 = value2.material;
		if ((bool)material && (bool)material2)
		{
			if (d.ValueRW.EffectExistTimer < 0.3f)
			{
				material.SetFloat(ID_Dissolve, -1f);
				material2.SetFloat(ID_Dissolve, -1f);
				d.ValueRW.DissolveProcess = -1f;
			}
			else
			{
				material.SetFloat(ID_Dissolve, d.ValueRW.DissolveProcess);
				material2.SetFloat(ID_Dissolve, d.ValueRW.DissolveProcess);
				d.ValueRW.DissolveProcess += deltaTime * 5f;
			}
		}
		if (value.positionCount < 2)
		{
			value.positionCount = 2;
		}
		if (value2.positionCount < 2)
		{
			value2.positionCount = 2;
		}
		if (d.ValueRO.Type == SpellSpecialMovementType.Rotation)
		{
			SetAroundCirclePoints(value, value2, d.ValueRO.CenterPoint, d.ValueRO.RandomAngle, d.ValueRO.BaseHeight, d, d.ValueRO.AroundRadius);
		}
		else
		{
			SetCurvePointsPosition(value, value2, d.ValueRO.LerpPos1, d.ValueRO.LerpPos2, d);
		}
	}

	private void SetCurvePointsPosition(LineRenderer line, LineRenderer shadowLineRenderer, Vector3 lerpPos1, Vector3 lerpPos2, RefRW<Spell1017DeathAdderEffectData> chainData)
	{
		if ((bool)line && (bool)shadowLineRenderer && line.positionCount >= 2)
		{
			for (int i = 0; i < line.positionCount; i++)
			{
				float t = (float)i / ((float)line.positionCount - 1f);
				Vector3 vector = GeneralTool.CubicBezierCurve(chainData.ValueRO.BeginPosition, lerpPos1, lerpPos2, chainData.ValueRO.BoomPosition, t);
				line.SetPosition(i, (float3)Tool2D.GetLayerPoint(vector));
				shadowLineRenderer.SetPosition(i, Tool2D.IgnoreZPoint(vector, 1.05f));
			}
		}
	}

	private void SetAroundCirclePoints(LineRenderer line, LineRenderer shadowLineRenderer, float3 centerPoint, float initialDegree, float baseHeight, RefRW<Spell1017DeathAdderEffectData> chainData, float radius)
	{
		if (!line || !shadowLineRenderer || line.positionCount < 2)
		{
			return;
		}
		centerPoint = new float3(centerPoint.x, centerPoint.y, 0f);
		for (int i = 0; i < line.positionCount; i++)
		{
			Vector3 vector = (Vector3)centerPoint + Tool2D.GetDir(initialDegree + 360f / (float)(line.positionCount - 2) * (float)i) * radius;
			Vector3 rootPoint = vector + new Vector3(0f, 0f, baseHeight);
			if (chainData.ValueRO.IsFallSpell)
			{
				rootPoint.z *= (float)i / (float)line.positionCount;
			}
			line.SetPosition(i, Tool2D.GetLayerPoint(rootPoint));
			shadowLineRenderer.SetPosition(i, Tool2D.IgnoreZPoint(vector, 1.05f));
		}
	}

	private void UpdateExplosionSpeed(RefRW<Spell1017DeathAdderEffectData> d, float dt)
	{
		if ((bool)d.ValueRO.ExplosionParticleCtrl && (bool)d.ValueRO.ChargeParticleCtrl)
		{
			d.ValueRO.ExplosionParticleCtrl.Value.transform.position = d.ValueRO.BoomPosition;
			d.ValueRO.ChargeParticleCtrl.Value.transform.position = d.ValueRO.BoomPosition;
			if (!(d.ValueRW.EffectExistTimer <= 0.05f))
			{
				float end = ((d.ValueRW.EffectExistTimer < 0.05f + d.ValueRO.HoverDuration) ? 0f : 1f);
				d.ValueRW.ExplosionSpeed = DTool.Lerp(d.ValueRW.ExplosionSpeed, end, dt * 10f);
				d.ValueRO.ExplosionParticleCtrl.Value.SetSimulationSpeed(d.ValueRW.ExplosionSpeed);
				d.ValueRO.ChargeParticleCtrl.Value.SetSimulationSpeed(d.ValueRW.ExplosionSpeed);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1017DeathAdderEffectData>();
		__query_1462804979_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1462804979_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1462804979_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((Spell1017DeathAdderEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1017DeathAdderEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1017DeathAdderEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
