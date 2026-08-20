using System.Runtime.InteropServices;
using Unity.Entities;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Spell1008ReadyToDeathTag : IComponentData, IQueryTypeParameter, IEnableableComponent
{
}
