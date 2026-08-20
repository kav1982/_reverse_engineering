using System.Runtime.InteropServices;
using Unity.Entities;

namespace Unity.Physics.Stateful;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct StatefulTriggerEventExclude : IComponentData, IQueryTypeParameter
{
}
