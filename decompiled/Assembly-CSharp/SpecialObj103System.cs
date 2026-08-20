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

[BurstCompile]
[CompilerGenerated]
internal struct SpecialObj103System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_257857185_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj103>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, LocalTransform> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj103>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, LocalTransform>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpecialObj103>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<IRoomCtrller_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpecialObj103> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<IRoomCtrller_Dots> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpecialObj103>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<IRoomCtrller_Dots>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj103>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, LocalTransform>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj103>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, LocalTransform> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpecialObj103>();
			state.EntityManager.CompleteDependencyBeforeRW<IRoomCtrller_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_257857185_0.TypeHandle __IFE_257857185_0_TypeHandle;

		public ComponentLookup<Item> __Item_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_257857185_0_TypeHandle = new IFE_257857185_0.TypeHandle(ref state);
			__Item_RW_ComponentLookup = state.GetComponentLookup<Item>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00005DAE_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00005DAE_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00005DAE_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_257857185_0;

	private EntityQuery __query_257857185_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SpecialObj103>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_257857185_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj103>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, LocalTransform> item6 in IFE_257857185_0.Query(__query_257857185_0, __TypeHandle.__IFE_257857185_0_TypeHandle, ref state))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpecialObj103> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots> uncheckedRefRW2 = item2;
			LocalTransform localTransform = item3;
			Entity e = entity;
			if (uncheckedRefRW.ValueRW.waitFrame < 2)
			{
				uncheckedRefRW.ValueRW.waitFrame++;
			}
			else
			{
				if (!uncheckedRefRW2.ValueRW.onRoomEnter)
				{
					continue;
				}
				uncheckedRefRW2.ValueRW.onRoomEnter = false;
				switch (uncheckedRefRW.ValueRW.type)
				{
				case SO103Type.Store:
				{
					List<ItemInfo> store = OutputMgr.GetStore();
					int num = ((UnityEngine.Random.Range(0, 3) == 0) ? UnityEngine.Random.Range(0, store.Count) : (-1));
					List<int> list = null;
					if (PlayerMgr.Inst.ItemCtrller.curseCfg_NoCargo != null)
					{
						list = new List<int>();
						int num2 = 0;
						while (list.Count < PlayerMgr.Inst.ItemCtrller.curseCfg_NoCargo.int1.result)
						{
							int item4 = UnityEngine.Random.Range(0, store.Count);
							if (!list.Contains(item4))
							{
								list.Add(item4);
							}
							num2++;
							if (num2 >= 100)
							{
								Debug.LogError("诅咒《懒散小店》为什么进入死循环");
								break;
							}
						}
					}
					List<int> list2 = null;
					if (PlayerMgr.Inst.ItemCtrller.relicCfg_FreeGods != null)
					{
						list2 = new List<int>();
						int num3 = 0;
						while (list2.Count < PlayerMgr.Inst.ItemCtrller.relicCfg_FreeGods.int1.result)
						{
							int item5 = UnityEngine.Random.Range(0, store.Count);
							if (!list2.Contains(item5))
							{
								if (list != null && list.Contains(item5))
								{
									continue;
								}
								list2.Add(item5);
							}
							num3++;
							if (num3 >= 100)
							{
								Debug.LogError("遗物《黄色印记》商品免费 为什么进入死循环");
								break;
							}
						}
					}
					for (int j = 0; j < store.Count; j++)
					{
						Entity @null = Entity.Null;
						if (j < 4)
						{
							float x3 = (-1.5f + (float)j) * uncheckedRefRW.ValueRW.spaceX;
							@null = QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, store[j], localTransform.Position + new float3(x3, uncheckedRefRW.ValueRW.spaceY / 2f, 0f), isStore: true);
						}
						else
						{
							float x4 = (float)(-5 + j) * uncheckedRefRW.ValueRW.spaceX;
							@null = QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, store[j], localTransform.Position + new float3(x4, (0f - uncheckedRefRW.ValueRW.spaceY) / 2f, 0f), isStore: true);
						}
						if (j == num)
						{
							InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref state, @null).ValueRW.SetPriceFactor(0.7f);
						}
						if (list != null && list.Contains(j))
						{
							RefRW<Item> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref state, @null);
							componentRWAfterCompletingDependency.ValueRW.BackPool();
							componentRWAfterCompletingDependency.ValueRW.Pickup();
						}
						if (list2 != null && list2.Contains(j))
						{
							InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref state, @null).ValueRW.SetPriceFactor(0f);
						}
					}
					break;
				}
				case SO103Type.Potion:
				{
					List<ItemInfo> storePotion = OutputMgr.GetStorePotion();
					for (int i = 0; i < storePotion.Count; i++)
					{
						if (i < storePotion.Count / 2)
						{
							float x = (float)(-(storePotion.Count / 2 - 1)) / 2f * uncheckedRefRW.ValueRW.spaceX + (float)i * uncheckedRefRW.ValueRW.spaceX;
							QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, storePotion[i], localTransform.Position + new float3(x, uncheckedRefRW.ValueRW.spaceY / 2f, 0f), isStore: true);
						}
						else
						{
							float x2 = (float)(-(storePotion.Count / 2 - 1)) / 2f * uncheckedRefRW.ValueRW.spaceX + (float)(i - storePotion.Count / 2) * uncheckedRefRW.ValueRW.spaceX;
							QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, storePotion[i], localTransform.Position + new float3(x2, (0f - uncheckedRefRW.ValueRW.spaceY) / 2f, 0f), isStore: true);
						}
					}
					break;
				}
				default:
					Debug.LogError(uncheckedRefRW.ValueRW.type);
					break;
				}
				entityCommandBuffer.DestroyEntity(e);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj103>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<IRoomCtrller_Dots>();
		__query_257857185_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_257857185_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00005DAE_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpecialObj103System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpecialObj103System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpecialObj103System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
