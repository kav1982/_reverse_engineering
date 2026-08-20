using System.Runtime.InteropServices;
using Unity.Entities;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ResourceChanger_Dots : IComponentData, IQueryTypeParameter
{
}
