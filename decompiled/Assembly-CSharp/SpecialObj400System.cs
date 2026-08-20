using System;
using System.Runtime.InteropServices;
using Unity.Entities;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct SpecialObj400System : ISystem
{
	public void OnCreate(ref SystemState state)
	{
	}

	public void OnUpdate(ref SystemState state)
	{
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((SpecialObj400System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpecialObj400System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
