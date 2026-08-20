using Unity.Mathematics;
using UnityEngine;

public class HideBoundary_Theme8 : HideBoundaryBase
{
	[Space(50f)]
	public GameObject pfb_Ash;

	public float[] noOrnamentsRadius;

	private Tile_T8 tile_T8;

	public void InitializeT8(Tile_T8 tile_T8)
	{
		this.tile_T8 = tile_T8;
		tile_T8.HideAccess();
		for (int i = 0; i < noOrnamentsRadius.Length; i++)
		{
			if (i >= tile_T8.ornamentPfbs.Length)
			{
				continue;
			}
			Transform child = tile_T8.tsf_OrnamentsParent.GetChild(i);
			for (int num = child.childCount - 1; num >= 0; num--)
			{
				Transform child2 = child.GetChild(num);
				if (child2.position.y > tile_T8.transform.position.y && Mathf.Abs(child2.position.x - tile_T8.transform.position.x) < noOrnamentsRadius[i])
				{
					Object.Destroy(child2.gameObject);
				}
			}
		}
	}

	public override void Disappear()
	{
		tile_T8.RecreateBossRoom();
		Object.Instantiate(pfb_Ash, base.transform.position, quaternion.identity, base.transform.parent);
	}
}
