using UnityEngine;

public class SpecialObj8_T6 : LayerCorrect, IRoomCtrller
{
	public SpriteRenderer sr;

	[Range(0f, 1f)]
	[Header("TongueCluster")]
	public float tongueClusterChance;

	public SpecialObj8_T6TongueCluster tongueCluster;

	public bool isLargeAbyss;

	private void Start()
	{
		sr.flipX = ((Random.Range(0, 2) == 0) ? true : false);
		if ((float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.ScavengingTongueCluster) / 100f != 0f && isLargeAbyss)
		{
			Object.Instantiate(tongueCluster, base.transform.position, Quaternion.identity, base.transform.parent).Initialize(sr.flipX);
		}
		Object.Destroy(this);
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		roomCtrller.AbyssRegister(base.gameObject);
	}
}
