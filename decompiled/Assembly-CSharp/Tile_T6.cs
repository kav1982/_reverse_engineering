using Unity.Mathematics;
using UnityEngine;

public class Tile_T6 : MonoBehaviour
{
	public Transform tsf_MR;

	public void TileCorrect(RoomController roomCtrller)
	{
		Transform obj = tsf_MR;
		float3 rootPosition = base.transform.position;
		obj.localPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Tile0);
		tsf_MR.localScale = new Vector3(27f, 17f, 1f);
		Object.Destroy(this);
	}
}
