using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
public class DamageRecordSystem : SystemBase
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

	private EntityQuery __query_823862911_0;

	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingletonBuffer<DamageRecordBuffer>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		DynamicBuffer<DamageRecordBuffer> singletonBuffer = __query_823862911_0.GetSingletonBuffer<DamageRecordBuffer>();
		foreach (DamageRecordBuffer item in singletonBuffer)
		{
			if (item.Damage <= 0f)
			{
				Debug.Log("复数或者伤害为0，不应该记录伤害");
			}
			else if (!item.Damage.IsValidFloat())
			{
				Debug.LogError($"异常的伤害统计数字：{item.Damage}\n" + Environment.StackTrace);
			}
			else
			{
				DamageRecordeManager.Recorde(item.SpellOrRelicId, item.Damage, item.HitUnitId);
			}
		}
		singletonBuffer.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<DamageRecordBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_823862911_0 = entityQueryBuilder2.Build(ref state);
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
	public DamageRecordSystem()
	{
	}
}
