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

[CompilerGenerated]
[UpdateAfter(typeof(SpellEffectSystem))]
[UpdateInGroup(typeof(SpellEffectSystemGroup))]
[BurstCompile]
internal struct Spell1024GiantBubbleEffectSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1826742325_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1024GiantBubbleData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1024GiantBubbleData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1024GiantBubbleData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpellConfigComponentData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1024GiantBubbleData> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item3_ComponentTypeHandle_RO;

			private ComponentTypeHandle<SpellComponentData> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1024GiantBubbleData>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1024GiantBubbleData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1024GiantBubbleData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1024GiantBubbleData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1826742325_0.TypeHandle __IFE_1826742325_0_TypeHandle;

		public BufferLookup<SpellGameObjectEffectLink> __SpellGameObjectEffectLink_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1826742325_0_TypeHandle = new IFE_1826742325_0.TypeHandle(ref state);
			__SpellGameObjectEffectLink_RW_BufferLookup = state.GetBufferLookup<SpellGameObjectEffectLink>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00006C0F_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00006C0F_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00006C0F_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnCreate_0024BurstManaged(self, state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1826742325_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Spell1024GiantBubbleData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1024GiantBubbleData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item5 in IFE_1826742325_0.Query(__query_1826742325_0, __TypeHandle.__IFE_1826742325_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<Spell1024GiantBubbleData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData> uncheckedRefRO = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW3 = item4;
			Entity entity2 = entity;
			if (!uncheckedRefRW2.ValueRO.EffectSpell.Value)
			{
				if (TryGetLinkEffect("EffectRange", InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2), out var linkedObject))
				{
					uncheckedRefRW2.ValueRW.EffectRange.Value = linkedObject;
				}
				if (TryGetLinkEffect("Spell", InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2), out var linkedObject2))
				{
					uncheckedRefRW2.ValueRW.EffectSpell.Value = linkedObject2;
				}
				if ((bool)uncheckedRefRW2.ValueRO.EffectRange.Value)
				{
					uncheckedRefRW2.ValueRW.EffectRange.Value.transform.localScale = Vector3.one * uncheckedRefRW.ValueRO.Radius.Calculate() * uncheckedRefRW2.ValueRW.EffectRangeInitScale;
				}
				if ((bool)uncheckedRefRW2.ValueRO.EffectSpell.Value)
				{
					uncheckedRefRW2.ValueRW.EffectSpell.Value.transform.localScale = Vector3.one * uncheckedRefRW.ValueRO.Radius.Calculate() * uncheckedRefRW2.ValueRW.EffectSpellInitScale;
				}
			}
			if ((bool)uncheckedRefRW2.ValueRO.EffectSpell.Value && !uncheckedRefRO.ValueRO.IsFallSpell)
			{
				uncheckedRefRW2.ValueRW.EffectSpell.Value.transform.position += new Vector3(0f, 0.6f, 0f);
			}
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, uncheckedRefRW3.ValueRW.SpellEffectEntity) && !uncheckedRefRO.ValueRO.IsFallSpell)
			{
				RefRW<EffectsCollectorData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state, uncheckedRefRW3.ValueRW.SpellEffectEntity);
				RefRW<LocalTransform> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentRWAfterCompletingDependency.ValueRW.Effect1);
				float3 position = new float3(0f, 0.55f / math.max(0.5f, uncheckedRefRW2.ValueRW.EffectSpellInitScale * uncheckedRefRW.ValueRO.Radius.Calculate()), 0f);
				componentRWAfterCompletingDependency2.ValueRW.Position = position;
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentRWAfterCompletingDependency.ValueRW.Effect2))
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentRWAfterCompletingDependency.ValueRW.Effect2).ValueRW.Position = position;
				}
			}
			if (!uncheckedRefRW2.ValueRO.IsCollapse)
			{
				float num = 0.08f;
				uncheckedRefRW2.ValueRW.EffectRangeInitScale += num * state.WorldUnmanaged.Time.DeltaTime;
				uncheckedRefRW2.ValueRW.EffectSpellInitScale += num * state.WorldUnmanaged.Time.DeltaTime;
				if ((bool)uncheckedRefRW2.ValueRO.EffectRange.Value)
				{
					uncheckedRefRW2.ValueRW.EffectRange.Value.transform.localScale = Vector3.one * uncheckedRefRW2.ValueRW.EffectRangeInitScale * uncheckedRefRW.ValueRO.Radius.Calculate();
				}
				if ((bool)uncheckedRefRW2.ValueRO.EffectSpell.Value)
				{
					uncheckedRefRW2.ValueRW.EffectSpell.Value.transform.localScale = Vector3.one * uncheckedRefRW2.ValueRW.EffectSpellInitScale * uncheckedRefRW.ValueRO.Radius.Calculate();
				}
			}
			else if ((bool)uncheckedRefRW2.ValueRO.EffectSpell.Value)
			{
				uncheckedRefRW2.ValueRW.EffectSpellInitScale -= 1.33f * state.WorldUnmanaged.Time.DeltaTime;
				if (uncheckedRefRW2.ValueRW.EffectSpellInitScale <= 0f)
				{
					uncheckedRefRW2.ValueRW.EffectSpellInitScale = 0f;
				}
				uncheckedRefRW2.ValueRW.EffectSpell.Value.transform.localScale = Vector3.one * uncheckedRefRW2.ValueRW.EffectSpellInitScale * uncheckedRefRW.ValueRO.Radius.Calculate();
			}
			if (!(uncheckedRefRW2.ValueRO.CollapseTimer >= 0.8f))
			{
				continue;
			}
			float num2 = 1f + uncheckedRefRW.ValueRO.DurationTimer * 0.1f;
			if (uncheckedRefRW.ValueRO.DurationTimer >= 6.5f)
			{
				if (!uncheckedRefRW2.ValueRO.EffectRainGround.Value)
				{
					if (TryGetLinkEffect("EndRainGround", InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2), out var linkedObject3))
					{
						uncheckedRefRW2.ValueRW.EffectRainGround.Value = linkedObject3;
					}
					if (TryGetLinkEffect("EndRain", InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2), out var linkedObject4))
					{
						uncheckedRefRW2.ValueRW.EffectRain.Value = linkedObject4;
					}
				}
				if ((bool)uncheckedRefRW2.ValueRO.EffectRainGround.Value)
				{
					uncheckedRefRW2.ValueRW.ParticleRainGround = uncheckedRefRW2.ValueRO.EffectRainGround.Value.transform.Find("Rain2").GetComponent<ParticleSystem>();
					ParticleSystem.ShapeModule shape = uncheckedRefRW2.ValueRO.ParticleRainGround.Value.shape;
					shape.radius = uncheckedRefRW.ValueRO.Radius.Calculate() * num2;
					uncheckedRefRW2.ValueRW.ParticleRain = uncheckedRefRW2.ValueRO.EffectRain.Value.transform.Find("Rain1").GetComponent<ParticleSystem>();
					ParticleSystem.ShapeModule shape2 = uncheckedRefRW2.ValueRO.ParticleRain.Value.shape;
					shape2.radius = uncheckedRefRW.ValueRO.Radius.Calculate() * num2;
					uncheckedRefRW2.ValueRW.ParticleRain = uncheckedRefRW2.ValueRO.EffectRainGround.Value.transform.Find("Rain3").GetComponent<ParticleSystem>();
					shape = uncheckedRefRW2.ValueRO.ParticleRain.Value.shape;
					shape.radius = uncheckedRefRW.ValueRO.Radius.Calculate() * num2;
				}
			}
			if ((bool)uncheckedRefRW2.ValueRO.EffectRange.Value)
			{
				uncheckedRefRW2.ValueRW.EffectRange.Value.transform.localScale = new Vector3(0f, 0f, 0f);
			}
			if ((bool)uncheckedRefRW2.ValueRO.EffectSpell.Value)
			{
				uncheckedRefRW2.ValueRW.EffectSpell.Value.transform.localScale = new Vector3(0f, 0f, 0f);
			}
		}
	}

	private bool TryGetLinkEffect(FixedString32Bytes name, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, out GameObject linkedObject)
	{
		foreach (SpellGameObjectEffectLink item in linkBuffer)
		{
			SpellGameObjectEffectLink current = item;
			if (current.EffectName == name)
			{
				UnityObjectRef<GameObject> gameObject = current.GameObject;
				linkedObject = gameObject.Value;
				return true;
			}
		}
		linkedObject = null;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1024GiantBubbleData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_1826742325_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00006C0F_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1024GiantBubbleEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1024GiantBubbleEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1024GiantBubbleEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
