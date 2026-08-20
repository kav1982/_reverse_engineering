using System.Runtime.InteropServices;
using Unity.Entities;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Gallery_Dots : IComponentData, IQueryTypeParameter
{
}
