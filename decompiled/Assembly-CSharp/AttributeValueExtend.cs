using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;

[BurstCompile]
public static class AttributeValueExtend
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate float CalculateWithNewBaseValue_000081B8_0024PostfixBurstDelegate(in AttributeValue value, float overwriteBase);

	internal static class CalculateWithNewBaseValue_000081B8_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CalculateWithNewBaseValue_000081B8_0024PostfixBurstDelegate>(CalculateWithNewBaseValue).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static float Invoke(in AttributeValue value, float overwriteBase)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<ref AttributeValue, float, float>)functionPointer)(ref value, overwriteBase);
				}
			}
			return value.CalculateWithNewBaseValue_0024BurstManaged(overwriteBase);
		}
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(CalculateWithNewBaseValue_000081B8_0024PostfixBurstDelegate))]
	public static float CalculateWithNewBaseValue(this in AttributeValue value, float overwriteBase)
	{
		return CalculateWithNewBaseValue_000081B8_0024BurstDirectCall.Invoke(in value, overwriteBase);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static float CalculateWithNewBaseValue_0024BurstManaged(this in AttributeValue value, float overwriteBase)
	{
		AttributeValue attributeValue = value;
		attributeValue.Base = overwriteBase;
		return attributeValue.Calculate();
	}
}
