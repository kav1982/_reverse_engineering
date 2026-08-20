using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[CompilerGenerated]
public struct Relic_ShowUnitHPSystem : ISystem, ISystemCompilerGenerated
{
	public enum NumUnitIndex
	{
		None = -1,
		K,
		M,
		B,
		T,
		Qa,
		Qi,
		Sx,
		Sp,
		万,
		亿,
		兆,
		京,
		垓,
		秭
	}

	private struct TypeHandle
	{
		public Relic_ShowUnitHPJob.InternalCompilerQueryAndHandleData __Relic_ShowUnitHPJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Relic_ShowUnitHPJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00004F82_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00004F82_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00004F82_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private ComponentLookup<UnitProperty_Dots> cluUnitPpt;

	private ComponentLookup<LocalTransform> cluLocalTsf;

	private ComponentLookup<PostTransformMatrix> cluPTM;

	private ComponentLookup<MatOverrideNumberAndLength> cluMatOverrideNumberAndLength;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_317401716_0;

	public static NumUnitIndex CaculateNumIndex(float num, out float numResult, out int numberLenth, bool isChinese)
	{
		num = ((num < 1f) ? math.ceil(num) : math.round(num));
		NumUnitIndex numUnitIndex = NumUnitIndex.None;
		numberLenth = 0;
		if (isChinese)
		{
			while (num > 10000f && numUnitIndex != NumUnitIndex.秭)
			{
				num /= 10000f;
				if (num > 10000f)
				{
					num = math.round(num);
				}
				numUnitIndex = ((numUnitIndex != NumUnitIndex.None) ? (numUnitIndex + 1) : NumUnitIndex.万);
			}
		}
		else
		{
			while (num > 1000f && numUnitIndex != NumUnitIndex.Sp)
			{
				num /= 1000f;
				if (num > 1000f)
				{
					num = math.round(num);
				}
				numUnitIndex++;
			}
		}
		num = Get13Num(num, out numberLenth, isChinese);
		numResult = num;
		return numUnitIndex;
	}

	public static float Get13Num(float x, out int numberLength, bool isChinese)
	{
		float num = 0f;
		float num2;
		for (num2 = 1f; num2 < x * 0.1f; num2 *= 10f)
		{
		}
		numberLength = 0;
		bool flag = false;
		float num3 = -1f;
		float num4 = (isChinese ? 4 : 3);
		for (int i = 0; (float)i < num4; i++)
		{
			num *= 13f;
			float num5 = math.floor(x / num2);
			num += num5;
			x -= num5 * num2;
			num2 *= 0.1f;
			numberLength++;
			if ((x == 0f && num2 < 1f) || (flag && num3 > 0f && (float)numberLength - num3 > 1f))
			{
				break;
			}
			if (num2 < 1f && !flag)
			{
				if ((float)i == num4 - 1f)
				{
					break;
				}
				flag = true;
				num *= 13f;
				num += 10f;
				numberLength++;
				num3 = numberLength;
			}
		}
		return num;
	}

	public static float GetUnitIndex(float x)
	{
		if (x < 0f)
		{
			return -1f;
		}
		return x * 2f * 28f + (x * 2f + 1f);
	}

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<Relic_ShowUnitHP>();
		cluUnitPpt = state.GetComponentLookup<UnitProperty_Dots>();
		cluLocalTsf = state.GetComponentLookup<LocalTransform>();
		cluPTM = state.GetComponentLookup<PostTransformMatrix>();
		cluMatOverrideNumberAndLength = state.GetComponentLookup<MatOverrideNumberAndLength>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_317401716_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		cluUnitPpt.Update(ref state);
		cluLocalTsf.Update(ref state);
		cluPTM.Update(ref state);
		cluMatOverrideNumberAndLength.Update(ref state);
		LanguageType language = DataMgr.settingData.language;
		bool isChinese = language == LanguageType.ChineseS || language == LanguageType.ChineseT;
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Relic_ShowUnitHPJob
		{
			cluUnitPpt = cluUnitPpt,
			cluLocalTsf = cluLocalTsf,
			cluPTM = cluPTM,
			cluMatOverrideNumberAndLength = cluMatOverrideNumberAndLength,
			ecb = entityCommandBuffer.AsParallelWriter(),
			isChinese = isChinese
		}, __TypeHandle.__Relic_ShowUnitHPJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Relic_ShowUnitHPJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Relic_ShowUnitHPJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Relic_ShowUnitHPJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Relic_ShowUnitHPJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Relic_ShowUnitHPJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_317401716_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00004F82_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Relic_ShowUnitHPSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Relic_ShowUnitHPSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Relic_ShowUnitHPSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
