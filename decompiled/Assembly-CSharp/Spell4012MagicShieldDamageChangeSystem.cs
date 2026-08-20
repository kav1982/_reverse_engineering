using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[UpdateAfter(typeof(TakeDamageInfoPreProcessSystem))]
[UpdateBefore(typeof(UnitPropertySystem))]
[CompilerGenerated]
public class Spell4012MagicShieldDamageChangeSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_653990383_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public BufferAccessor<TakeDamageInfo_Dots> item2_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<UnitProperty_Dots>(item1_IntPtr, index), item2_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<UnitProperty_Dots> item1_ComponentTypeHandle_RO;

			private BufferTypeHandle<TakeDamageInfo_Dots> item2_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<UnitProperty_Dots>(isReadOnly: true);
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<TakeDamageInfo_Dots>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<TakeDamageInfo_Dots>();
		}
	}

	private struct TypeHandle
	{
		public IFE_653990383_0.TypeHandle __IFE_653990383_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_653990383_0_TypeHandle = new IFE_653990383_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_653990383_0;

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>> item3 in IFE_653990383_0.Query(__query_653990383_0, __TypeHandle.__IFE_653990383_0_TypeHandle, ref base.CheckedStateRef))
		{
			item3.Deconstruct(out var item, out var item2, out var _);
			InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots> uncheckedRefRO = item;
			DynamicBuffer<TakeDamageInfo_Dots> dynamicBuffer = item2;
			if (uncheckedRefRO.ValueRO.unitCfg.unitType != 0 || uncheckedRefRO.ValueRO.IsInvincible)
			{
				continue;
			}
			for (int i = 0; i < dynamicBuffer.Length; i++)
			{
				TakeDamageInfo_Dots takeDamageInfo_Dots = dynamicBuffer[i];
				if (!takeDamageInfo_Dots.ignoreUmbrella && takeDamageInfo_Dots.attackerType != AttackerType.FromUI)
				{
					if (PlayerMgr.Inst.PlayerPpt.UmbrellaCtrl.maxBlockDamage < (int)dynamicBuffer[i].damage)
					{
						takeDamageInfo_Dots.damage -= PlayerMgr.Inst.PlayerPpt.UmbrellaCtrl.maxBlockDamage;
						takeDamageInfo_Dots.hitMagicShieldDamage += PlayerMgr.Inst.PlayerPpt.UmbrellaCtrl.maxBlockDamage;
						PlayerMgr.Inst.PlayerPpt.UmbrellaCtrl.damageBlockThisFrame += PlayerMgr.Inst.PlayerPpt.UmbrellaCtrl.maxBlockDamage;
						PlayerMgr.Inst.PlayerPpt.UmbrellaCtrl.maxBlockDamage = 0;
						dynamicBuffer.ElementAt(i) = takeDamageInfo_Dots;
						break;
					}
					PlayerMgr.Inst.PlayerPpt.UmbrellaCtrl.maxBlockDamage -= (int)dynamicBuffer[i].damage;
					PlayerMgr.Inst.PlayerPpt.UmbrellaCtrl.damageBlockThisFrame += (int)dynamicBuffer[i].damage;
					takeDamageInfo_Dots.hitMagicShieldDamage = takeDamageInfo_Dots.damage;
					takeDamageInfo_Dots.damage = 0f;
					dynamicBuffer.ElementAt(i) = takeDamageInfo_Dots;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<UnitProperty_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TakeDamageInfo_Dots>();
		__query_653990383_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell4012MagicShieldDamageChangeSystem()
	{
	}
}
