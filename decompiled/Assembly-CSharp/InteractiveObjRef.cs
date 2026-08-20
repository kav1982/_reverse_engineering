using Unity.Entities;

public struct InteractiveObjRef : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<InteractiveObj> obj;
}
