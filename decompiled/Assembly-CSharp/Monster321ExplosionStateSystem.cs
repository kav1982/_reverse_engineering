using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[CompilerGenerated]
internal struct Monster321ExplosionStateSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct TypeHandle
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1851961830_0;

	public void OnCreate(ref SystemState state)
	{
		state.EntityManager.CreateSingletonBuffer<Monster321ExplosionData>();
		state.RequireForUpdate<Monster321ExplosionData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		DynamicBuffer<Monster321ExplosionData> singletonBuffer = __query_1851961830_0.GetSingletonBuffer<Monster321ExplosionData>();
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		for (int num = singletonBuffer.Length - 1; num >= 0; num--)
		{
			Monster321ExplosionData value = singletonBuffer[num];
			if (!value.IsInitialized)
			{
				value.IsInitialized = true;
				ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle", Tool2D.IgnoreZPoint(value.CenterPoint)).GetComponent<WarningArea>().Initialize(value.DamageRange, value.DelayExplosionDuration);
			}
			value.Timer += state.WorldUnmanaged.Time.DeltaTime;
			if (value.Timer >= value.DelayExplosionDuration)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster309_Explosion", value.CenterPoint, Quaternion.identity, Vector3.one * value.DamageRange / 3f, 3f);
				SEMgr.Inst.monster34Explosion.PlaySE();
				UnitDotsSyncSystem.GetCollidersInRange(value.CenterPoint, value.DamageRange, GameConst.Filter_MonsterAoe, list);
				for (int i = 0; i < list.Count; i++)
				{
					UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
					uint layer = UnitDotsSyncSystem.GetLayer(distanceHitResult.entity);
					if ((layer == 512 || layer == 2097152) && UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
					{
						TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
						info.damage = value.BaseDamage;
						UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
					}
				}
				singletonBuffer.RemoveAt(num);
			}
			else
			{
				singletonBuffer[num] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster321ExplosionData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1851961830_0 = entityQueryBuilder2.Build(ref state);
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
		((Monster321ExplosionStateSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Monster321ExplosionStateSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Monster321ExplosionStateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
