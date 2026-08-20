using Unity.Entities;
using UnityEngine;

public struct SpecialObj40_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_Anima;

	public int tipsCountPC;

	public int tipsCountMobile;

	public bool isInitialized;

	public UnityObjectRef<Transform> emptyTransform;

	public int currentTipsID;

	public int GetTipToTall()
	{
		if (GameMgr.IsMobile_Static)
		{
			return tipsCountMobile;
		}
		return tipsCountPC;
	}
}
