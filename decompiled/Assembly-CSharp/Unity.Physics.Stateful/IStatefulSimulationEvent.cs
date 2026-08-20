using System;
using Unity.Entities;

namespace Unity.Physics.Stateful;

public interface IStatefulSimulationEvent<T> : IBufferElementData, ISimulationEvent<T>, IComparable<T>
{
	StatefulEventState State { get; set; }
}
