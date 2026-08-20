using System.Runtime.InteropServices;
using Unity.Entities;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct EnterDoorDestroy : IComponentData, IQueryTypeParameter, IEnableableComponent
{
}
