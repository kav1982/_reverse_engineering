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
[BurstCompile]
[UpdateInGroup(typeof(SceneGroup))]
public struct Boundary_T11System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_877289916_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public ManagedComponentAccessor<BoundaryT2RoomCtrller> item3_ManagedComponentAccessor;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Boundary_T11_Dots>, InternalCompilerInterface.UncheckedRefRO<BoundaryBase_Dots>, BoundaryT2RoomCtrller, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Boundary_T11_Dots>, InternalCompilerInterface.UncheckedRefRO<BoundaryBase_Dots>, BoundaryT2RoomCtrller, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Boundary_T11_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<BoundaryBase_Dots>(item2_IntPtr, index), item3_ManagedComponentAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			public EntityManager _entityManager;

			private ComponentTypeHandle<Boundary_T11_Dots> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<BoundaryBase_Dots> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<BoundaryT2RoomCtrller> item3_ManagedComponentTypeHandle_RO;

			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				_entityManager = systemState.EntityManager;
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Boundary_T11_Dots>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<BoundaryBase_Dots>(isReadOnly: true);
				item3_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<BoundaryT2RoomCtrller>(isReadOnly: false);
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ManagedComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor(ref item3_ManagedComponentTypeHandle_RO, _entityManager);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Boundary_T11_Dots>, InternalCompilerInterface.UncheckedRefRO<BoundaryBase_Dots>, BoundaryT2RoomCtrller, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Boundary_T11_Dots>, InternalCompilerInterface.UncheckedRefRO<BoundaryBase_Dots>, BoundaryT2RoomCtrller, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Boundary_T11_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<BoundaryBase_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<BoundaryT2RoomCtrller>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_877289916_0.TypeHandle __IFE_877289916_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_877289916_0_TypeHandle = new IFE_877289916_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000058A8_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000058A8_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000058A8_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_877289916_0;

	private EntityQuery __query_877289916_1;

	private EntityQuery __query_877289916_2;

	private EntityQuery __query_877289916_3;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<Boundary_T11_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer ecb = __query_877289916_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		RefRW<GlobalRandom> singletonRW = __query_877289916_2.GetSingletonRW<GlobalRandom>();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Boundary_T11_Dots>, InternalCompilerInterface.UncheckedRefRO<BoundaryBase_Dots>, BoundaryT2RoomCtrller, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> item5 in IFE_877289916_0.Query(__query_877289916_0, __TypeHandle.__IFE_877289916_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Boundary_T11_Dots> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<BoundaryBase_Dots> uncheckedRefRO = item2;
			BoundaryT2RoomCtrller boundaryT2RoomCtrller = item3;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW2 = item4;
			Entity e = entity;
			uncheckedRefRW2.ValueRW.Position = uncheckedRefRO.ValueRO.roomPosition + uncheckedRefRO.ValueRO.selfPosition.GetFloat3();
			float3 layerPosition = DTool.GetLayerPosition(in uncheckedRefRW2.ValueRW.Position, LayerCorrectType.BoundaryAO);
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRO.ett_LayerAO).ValueRW.Position = layerPosition + new float3(0f, 0f, DTool.Random(ref singletonRW.ValueRW.random, -0.0049f, 0.0049f));
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRO.ett_LayerWall).ValueRW.Position = DTool.GetLayerPosition(in uncheckedRefRW2.ValueRW.Position, LayerCorrectType.Coordinate);
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRO.ett_LayerTile).ValueRW.Position = DTool.GetLayerPosition(in uncheckedRefRW2.ValueRW.Position, LayerCorrectType.Tile0);
			ref readonly BoundaryBase_Dots valueRO = ref uncheckedRefRO.ValueRO;
			Vector2Data offset = new Vector2Data(0f, 1f);
			bool flag = DTool.BoundaryCheck(in valueRO, in offset);
			ref readonly BoundaryBase_Dots valueRO2 = ref uncheckedRefRO.ValueRO;
			offset = new Vector2Data(1f, 1f);
			bool flag2 = DTool.BoundaryCheck(in valueRO2, in offset);
			ref readonly BoundaryBase_Dots valueRO3 = ref uncheckedRefRO.ValueRO;
			offset = new Vector2Data(1f, 0f);
			bool flag3 = DTool.BoundaryCheck(in valueRO3, in offset);
			ref readonly BoundaryBase_Dots valueRO4 = ref uncheckedRefRO.ValueRO;
			offset = new Vector2Data(1f, -1f);
			bool flag4 = DTool.BoundaryCheck(in valueRO4, in offset);
			ref readonly BoundaryBase_Dots valueRO5 = ref uncheckedRefRO.ValueRO;
			offset = new Vector2Data(0f, -1f);
			bool flag5 = DTool.BoundaryCheck(in valueRO5, in offset);
			ref readonly BoundaryBase_Dots valueRO6 = ref uncheckedRefRO.ValueRO;
			offset = new Vector2Data(-1f, -1f);
			bool flag6 = DTool.BoundaryCheck(in valueRO6, in offset);
			ref readonly BoundaryBase_Dots valueRO7 = ref uncheckedRefRO.ValueRO;
			offset = new Vector2Data(-1f, 0f);
			bool flag7 = DTool.BoundaryCheck(in valueRO7, in offset);
			ref readonly BoundaryBase_Dots valueRO8 = ref uncheckedRefRO.ValueRO;
			offset = new Vector2Data(-1f, 1f);
			bool flag8 = DTool.BoundaryCheck(in valueRO8, in offset);
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			bool flag13 = false;
			bool flag14 = false;
			bool flag15 = false;
			bool flag16 = false;
			bool flag17 = false;
			bool flag18 = false;
			bool flag19 = false;
			bool flag20 = false;
			if (flag && flag3 && !flag5 && !flag7)
			{
				if (boundaryT2RoomCtrller.accessPositionR == uncheckedRefRO.ValueRO.selfPosition + new Vector2Data(-1f, -2f))
				{
					Entity e2 = ecb.Instantiate(__query_877289916_3.GetSingletonBuffer<SceneEttBED>()[0].ett_Boundary);
					ecb.SetComponent(e2, uncheckedRefRO.ValueRO);
					ecb.DestroyEntity(e);
					continue;
				}
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavActionTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(180f), boundaryT2RoomCtrller.roomCtrller.tsf_Action);
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGroundTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(180f), boundaryT2RoomCtrller.roomCtrller.tsf_Ground);
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFlyTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(180f), boundaryT2RoomCtrller.roomCtrller.tsf_Fly);
				RandomDetail(uncheckedRefRW.ValueRO.detailChance, uncheckedRefRO.ValueRO, singletonRW, ecb, uncheckedRefRW.ValueRO.ett_Detail_UR, flip: false);
				flag12 = true;
				flag19 = true;
				uncheckedRefRW2.ValueRW.Position += new float3(0f, 0f, 0.0049f);
				Entity e3 = ecb.Instantiate(uncheckedRefRW.ValueRO.ett_Collider_Big);
				ecb.SetComponent(e3, new LocalTransform
				{
					Position = uncheckedRefRW2.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (!flag && flag3 && flag5 && !flag7)
			{
				if (boundaryT2RoomCtrller.accessPositionR == uncheckedRefRO.ValueRO.selfPosition + new Vector2Data(-1f, 1f))
				{
					Entity e4 = ecb.Instantiate(__query_877289916_3.GetSingletonBuffer<SceneEttBED>()[0].ett_Boundary);
					ecb.SetComponent(e4, uncheckedRefRO.ValueRO);
					ecb.DestroyEntity(e);
					continue;
				}
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavActionTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(90f), boundaryT2RoomCtrller.roomCtrller.tsf_Action);
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGroundTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(90f), boundaryT2RoomCtrller.roomCtrller.tsf_Ground);
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFlyTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(90f), boundaryT2RoomCtrller.roomCtrller.tsf_Fly);
				RandomDetail(uncheckedRefRW.ValueRO.detailChance, uncheckedRefRO.ValueRO, singletonRW, ecb, uncheckedRefRW.ValueRO.ett_Detail_RD, flip: false);
				flag10 = true;
				flag17 = true;
				uncheckedRefRW2.ValueRW.Position += new float3(0f, 0f, -0.0049f);
				Entity e5 = ecb.Instantiate(uncheckedRefRW.ValueRO.ett_Collider_Big);
				ecb.SetComponent(e5, new LocalTransform
				{
					Position = uncheckedRefRW2.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, 4.712389f),
					Scale = 1f
				});
			}
			else if (!flag && !flag3 && flag5 && flag7)
			{
				if (boundaryT2RoomCtrller.accessPositionL == uncheckedRefRO.ValueRO.selfPosition + new Vector2Data(1f, 1f))
				{
					Entity e6 = ecb.Instantiate(__query_877289916_3.GetSingletonBuffer<SceneEttBED>()[0].ett_Boundary);
					ecb.SetComponent(e6, uncheckedRefRO.ValueRO);
					ecb.DestroyEntity(e);
					continue;
				}
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavActionTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(0f), boundaryT2RoomCtrller.roomCtrller.tsf_Action);
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGroundTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(0f), boundaryT2RoomCtrller.roomCtrller.tsf_Ground);
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFlyTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(0f), boundaryT2RoomCtrller.roomCtrller.tsf_Fly);
				RandomDetail(uncheckedRefRW.ValueRO.detailChance, uncheckedRefRO.ValueRO, singletonRW, ecb, uncheckedRefRW.ValueRO.ett_Detail_RD, flip: true);
				flag10 = true;
				flag17 = true;
				uncheckedRefRW2.ValueRW.Position += new float3(0f, 0f, -0.0049f);
				ecb.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
				Entity e7 = ecb.Instantiate(uncheckedRefRW.ValueRO.ett_Collider_Big);
				ecb.SetComponent(e7, new LocalTransform
				{
					Position = uncheckedRefRW2.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, MathF.PI),
					Scale = 1f
				});
			}
			else if (flag && !flag3 && !flag5 && flag7)
			{
				if (boundaryT2RoomCtrller.accessPositionL == uncheckedRefRO.ValueRO.selfPosition + new Vector2Data(1f, -2f))
				{
					Entity e8 = ecb.Instantiate(__query_877289916_3.GetSingletonBuffer<SceneEttBED>()[0].ett_Boundary);
					ecb.SetComponent(e8, uncheckedRefRO.ValueRO);
					ecb.DestroyEntity(e);
					continue;
				}
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavActionTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(270f), boundaryT2RoomCtrller.roomCtrller.tsf_Action);
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGroundTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(270f), boundaryT2RoomCtrller.roomCtrller.tsf_Ground);
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFlyTriangle"), uncheckedRefRW2.ValueRO.Position, Tool2D.GetRotation(270f), boundaryT2RoomCtrller.roomCtrller.tsf_Fly);
				RandomDetail(uncheckedRefRW.ValueRO.detailChance, uncheckedRefRO.ValueRO, singletonRW, ecb, uncheckedRefRW.ValueRO.ett_Detail_UR, flip: true);
				flag12 = true;
				flag19 = true;
				uncheckedRefRW2.ValueRW.Position += new float3(0f, 0f, 0.0049f);
				ecb.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
				Entity e9 = ecb.Instantiate(uncheckedRefRW.ValueRO.ett_Collider_Big);
				ecb.SetComponent(e9, new LocalTransform
				{
					Position = uncheckedRefRW2.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, MathF.PI / 2f),
					Scale = 1f
				});
			}
			else if (flag && flag3 && !flag5 && flag7)
			{
				ref readonly BoundaryBase_Dots valueRO9 = ref uncheckedRefRO.ValueRO;
				offset = new Vector2Data(-1f, -1f);
				if (!DTool.BoundaryCheckOnly1(in valueRO9, in offset))
				{
					ref readonly BoundaryBase_Dots valueRO10 = ref uncheckedRefRO.ValueRO;
					Vector2Data offset2 = new Vector2Data(1f, -1f);
					if (!DTool.BoundaryCheckOnly1(in valueRO10, in offset2))
					{
						ref readonly BoundaryBase_Dots valueRO11 = ref uncheckedRefRO.ValueRO;
						Vector2Data offset3 = new Vector2Data(-2f, 0f);
						if (DTool.BoundaryCheckOnly1(in valueRO11, in offset3))
						{
							ref readonly BoundaryBase_Dots valueRO12 = ref uncheckedRefRO.ValueRO;
							Vector2Data offset4 = new Vector2Data(2f, 0f);
							if (DTool.BoundaryCheckOnly1(in valueRO12, in offset4))
							{
								RandomDetail(uncheckedRefRW.ValueRO.detailChance, uncheckedRefRO.ValueRO, singletonRW, ecb, uncheckedRefRW.ValueRO.ett_Detail_LUR, flip: false);
							}
						}
					}
				}
				flag9 = true;
				flag16 = true;
				Entity e10 = ecb.Instantiate(uncheckedRefRW.ValueRO.ett_Collider_Full);
				ecb.SetComponent(e10, new LocalTransform
				{
					Position = uncheckedRefRW2.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (flag && flag3 && flag5 && !flag7)
			{
				ref readonly BoundaryBase_Dots valueRO13 = ref uncheckedRefRO.ValueRO;
				offset = new Vector2Data(-1f, -1f);
				if (!DTool.BoundaryCheckOnly1(in valueRO13, in offset))
				{
					ref readonly BoundaryBase_Dots valueRO14 = ref uncheckedRefRO.ValueRO;
					Vector2Data offset2 = new Vector2Data(-1f, 1f);
					if (!DTool.BoundaryCheckOnly1(in valueRO14, in offset2))
					{
						ref readonly BoundaryBase_Dots valueRO15 = ref uncheckedRefRO.ValueRO;
						Vector2Data offset3 = new Vector2Data(0f, -2f);
						if (DTool.BoundaryCheckOnly1(in valueRO15, in offset3))
						{
							ref readonly BoundaryBase_Dots valueRO16 = ref uncheckedRefRO.ValueRO;
							Vector2Data offset4 = new Vector2Data(0f, 2f);
							if (DTool.BoundaryCheckOnly1(in valueRO16, in offset4))
							{
								RandomDetail(uncheckedRefRW.ValueRO.detailChance, uncheckedRefRO.ValueRO, singletonRW, ecb, uncheckedRefRW.ValueRO.ett_Detail_URD, flip: false);
							}
						}
					}
				}
				flag13 = true;
				flag20 = true;
				Entity e11 = ecb.Instantiate(uncheckedRefRW.ValueRO.ett_Collider_Full);
				ecb.SetComponent(e11, new LocalTransform
				{
					Position = uncheckedRefRW2.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (!flag && flag3 && flag5 && flag7)
			{
				ref readonly BoundaryBase_Dots valueRO17 = ref uncheckedRefRO.ValueRO;
				offset = new Vector2Data(-1f, 1f);
				if (!DTool.BoundaryCheckOnly1(in valueRO17, in offset))
				{
					ref readonly BoundaryBase_Dots valueRO18 = ref uncheckedRefRO.ValueRO;
					Vector2Data offset2 = new Vector2Data(1f, 1f);
					if (!DTool.BoundaryCheckOnly1(in valueRO18, in offset2))
					{
						ref readonly BoundaryBase_Dots valueRO19 = ref uncheckedRefRO.ValueRO;
						Vector2Data offset3 = new Vector2Data(-2f, 0f);
						if (DTool.BoundaryCheckOnly1(in valueRO19, in offset3))
						{
							ref readonly BoundaryBase_Dots valueRO20 = ref uncheckedRefRO.ValueRO;
							Vector2Data offset4 = new Vector2Data(2f, 0f);
							if (DTool.BoundaryCheckOnly1(in valueRO20, in offset4))
							{
								RandomDetail(uncheckedRefRW.ValueRO.detailChance, uncheckedRefRO.ValueRO, singletonRW, ecb, uncheckedRefRW.ValueRO.ett_Detail_RDL, flip: false);
							}
						}
					}
				}
				flag11 = true;
				flag18 = true;
				uncheckedRefRW2.ValueRW.Position += new float3(0f, 0f, -0.0049f);
				Entity e12 = ecb.Instantiate(uncheckedRefRW.ValueRO.ett_Collider_Full);
				ecb.SetComponent(e12, new LocalTransform
				{
					Position = uncheckedRefRW2.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (flag && !flag3 && flag5 && flag7)
			{
				ref readonly BoundaryBase_Dots valueRO21 = ref uncheckedRefRO.ValueRO;
				offset = new Vector2Data(1f, 1f);
				if (!DTool.BoundaryCheckOnly1(in valueRO21, in offset))
				{
					ref readonly BoundaryBase_Dots valueRO22 = ref uncheckedRefRO.ValueRO;
					Vector2Data offset2 = new Vector2Data(1f, -1f);
					if (!DTool.BoundaryCheckOnly1(in valueRO22, in offset2))
					{
						ref readonly BoundaryBase_Dots valueRO23 = ref uncheckedRefRO.ValueRO;
						Vector2Data offset3 = new Vector2Data(0f, -2f);
						if (DTool.BoundaryCheckOnly1(in valueRO23, in offset3))
						{
							ref readonly BoundaryBase_Dots valueRO24 = ref uncheckedRefRO.ValueRO;
							Vector2Data offset4 = new Vector2Data(0f, 2f);
							if (DTool.BoundaryCheckOnly1(in valueRO24, in offset4))
							{
								RandomDetail(uncheckedRefRW.ValueRO.detailChance, uncheckedRefRO.ValueRO, singletonRW, ecb, uncheckedRefRW.ValueRO.ett_Detail_URD, flip: true);
							}
						}
					}
				}
				flag13 = true;
				flag20 = true;
				ecb.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
				Entity e13 = ecb.Instantiate(uncheckedRefRW.ValueRO.ett_Collider_Full);
				ecb.SetComponent(e13, new LocalTransform
				{
					Position = uncheckedRefRW2.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (flag && flag3 && flag5 && flag7)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_LayerTile);
				Entity e14 = ecb.Instantiate(uncheckedRefRW.ValueRO.ett_Collider_Full);
				ecb.SetComponent(e14, new LocalTransform
				{
					Position = uncheckedRefRW2.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
				if (!flag2 && flag4 && flag6 && flag8)
				{
					flag15 = true;
				}
				else if (flag2 && !flag4 && flag6 && flag8)
				{
					flag14 = true;
				}
				else if (flag2 && flag4 && !flag6 && flag8)
				{
					flag14 = true;
					ecb.AddComponent(e, new PostTransformMatrix
					{
						Value = float4x4.Scale(-1f, 1f, 1f)
					});
				}
				else if (flag2 && flag4 && flag6 && !flag8)
				{
					flag15 = true;
					ecb.AddComponent(e, new PostTransformMatrix
					{
						Value = float4x4.Scale(-1f, 1f, 1f)
					});
				}
			}
			if (!flag9)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_AO_LUR);
			}
			if (!flag10)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_AO_RD);
			}
			if (!flag11)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_AO_RDL);
			}
			if (!flag12)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_AO_UR);
			}
			if (!flag13)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_AO_URD);
			}
			if (!flag14)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_Wall_Corner_RD);
			}
			if (!flag15)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_Wall_Corner_UR);
			}
			if (!flag16)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_Wall_LUR);
			}
			if (!flag17)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_Wall_RD);
			}
			if (!flag18)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_Wall_RDL);
			}
			if (!flag19)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_Wall_UR);
			}
			if (!flag20)
			{
				ecb.DestroyEntity(uncheckedRefRW.ValueRO.ett_Wall_URD);
			}
			ecb.SetComponentEnabled<Boundary_T11_Dots>(e, value: false);
		}
	}

	private void RandomDetail(float detailChance, BoundaryBase_Dots boundaryBase, RefRW<GlobalRandom> gRnadom, EntityCommandBuffer ecb, Entity createEtt, bool flip)
	{
		if (boundaryBase.shouldCreateDetail && ((boundaryBase.selfPosition.x % 2f == 0f && boundaryBase.selfPosition.y % 2f == 0f) || ((boundaryBase.selfPosition.x + 1f) % 2f == 0f && (boundaryBase.selfPosition.y + 1f) % 2f == 0f)) && DTool.RandomValue(ref gRnadom.ValueRW.random) <= detailChance)
		{
			Entity e = ecb.Instantiate(createEtt);
			ecb.SetComponent(e, new LocalTransform
			{
				Position = boundaryBase.roomPosition + boundaryBase.selfPosition.GetFloat3(),
				Rotation = quaternion.identity,
				Scale = 1f
			});
			if (flip)
			{
				ecb.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<BoundaryBase_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<BoundaryT2RoomCtrller>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Boundary_T11_Dots>();
		__query_877289916_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_877289916_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_877289916_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SceneEttBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_877289916_3 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_000058A8_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Boundary_T11System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Boundary_T11System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Boundary_T11System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
