using Unity.Entities;

public static class EntityMgrHelper
{
	public static void CheckInitialize(this in EntityManager ettMgr, ref bool isDotsInitialized)
	{
		if (isDotsInitialized)
		{
			return;
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(SceneEttBED));
		using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(AllUnitEtt));
		using EntityQuery entityQuery3 = ettMgr.CreateEntityQuery(typeof(AllSpecialObjEtt));
		using EntityQuery entityQuery4 = ettMgr.CreateEntityQuery(typeof(AllMixedEtt));
		bool flag = (isDotsInitialized = !entityQuery.IsEmptyIgnoreFilter && !entityQuery2.IsEmptyIgnoreFilter && !entityQuery3.IsEmptyIgnoreFilter && !entityQuery4.IsEmptyIgnoreFilter);
	}
}
