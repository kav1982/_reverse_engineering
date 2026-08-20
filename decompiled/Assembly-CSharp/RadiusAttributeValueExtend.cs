using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;

[BurstCompile]
public static class RadiusAttributeValueExtend
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate float CalculateWithNewBaseValue_000081BC_0024PostfixBurstDelegate(in RadiusAttributeValue value, float overwriteBase);

	internal static class CalculateWithNewBaseValue_000081BC_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CalculateWithNewBaseValue_000081BC_0024PostfixBurstDelegate>(CalculateWithNewBaseValue).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static float Invoke(in RadiusAttributeValue value, float overwriteBase)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<ref RadiusAttributeValue, float, float>)functionPointer)(ref value, overwriteBase);
				}
			}
			return value.CalculateWithNewBaseValue_0024BurstManaged(overwriteBase);
		}
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(CalculateWithNewBaseValue_000081BC_0024PostfixBurstDelegate))]
	public static float CalculateWithNewBaseValue(this in RadiusAttributeValue value, float overwriteBase)
	{
		return CalculateWithNewBaseValue_000081BC_0024BurstDirectCall.Invoke(in value, overwriteBase);
	}

	public static float CalculateWithNewBaseValueIgnoreFall(this in RadiusAttributeValue value, float overwriteBase)
	{
		RadiusAttributeValue radiusAttributeValue = value;
		radiusAttributeValue.Base = overwriteBase;
		return radiusAttributeValue.CalculateIgnoreFall();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static float CalculateWithNewBaseValue_0024BurstManaged(this in RadiusAttributeValue value, float overwriteBase)
	{
		RadiusAttributeValue radiusAttributeValue = value;
		radiusAttributeValue.Base = overwriteBase;
		return radiusAttributeValue.Calculate();
	}
}
