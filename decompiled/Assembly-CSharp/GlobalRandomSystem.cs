using System;
using System.Runtime.InteropServices;
using Unity.Entities;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct GlobalRandomSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
		state.EntityManager.CreateSingleton(new GlobalRandom((uint)UnityEngine.Random.Range(0f, 4.2949673E+09f)));
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((GlobalRandomSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
