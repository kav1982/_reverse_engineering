using UnityEngine;

public class RoomObjData
{
	public RoomObjType objType;

	public Vector2Data point;

	public int id;

	public Vector2Data offset;

	public float extraData1;

	public float extraData2;

	public float extraData3;

	public RoomObjData()
	{
	}

	public RoomObjData(RoomObjType objType, int id)
	{
		this.objType = objType;
		this.id = id;
	}

	public RoomObjData Copy()
	{
		return new RoomObjData
		{
			objType = objType,
			point = point,
			id = id,
			offset = offset,
			extraData1 = extraData1,
			extraData2 = extraData2,
			extraData3 = extraData3
		};
	}

	public Vector3 GetFinalVector3()
	{
		return point.GetVector3() + offset.GetVector3();
	}
}
