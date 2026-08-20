using System.Runtime.InteropServices;
using Unity.Entities;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Spell2007SuicideBugInitializeTag : IComponentData, IQueryTypeParameter, IEnableableComponent
{
}
