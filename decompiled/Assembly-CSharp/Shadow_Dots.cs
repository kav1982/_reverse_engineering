using Unity.Entities;

public struct Shadow_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Shadow;

	public float shadowScale;

	public bool updateEveryFrame;

	public bool controlBySpellTransparent;

	public bool isInitialized;

	public bool onShow;

	public bool onHide;

	public bool isHiding;

	public void Show()
	{
		onShow = true;
	}

	public void Hide()
	{
		onHide = true;
	}
}
