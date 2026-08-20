using System.Runtime.InteropServices;
using Unity.Entities;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct SpellEffectTag_TrailSizeByRadius : IComponentData, IQueryTypeParameter
{
}
