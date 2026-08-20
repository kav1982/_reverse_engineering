using Unity.Entities;
using UnityEngine;

public class Elite52_Drop : MonoBehaviour
{
	public float interval;

	public int maxGenerateCount;

	public float dropSpacing;

	public bool dropCenter;

	private Entity master;

	private bool useCrossDrop;

	private int generatedLineCount;

	private float intervalTimer;

	private bool useTargetAngle;

	private float targetAngle;

	public void Initialize(Entity master, bool useCrossDrop, bool useTargetAngle = false, float targetAngle = 0f)
	{
		this.master = master;
		this.useCrossDrop = useCrossDrop;
		generatedLineCount = 0;
		intervalTimer = 0f;
		this.useTargetAngle = useTargetAngle;
		this.targetAngle = targetAngle;
		if (dropCenter)
		{
			GenerateDrop(base.transform.position);
		}
	}

	private void Update()
	{
		if (generatedLineCount >= maxGenerateCount)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		if (SpecialObj301EndlessMonsterSpawner.Inst.StageFinished)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		intervalTimer += Time.deltaTime;
		if (intervalTimer >= interval)
		{
			intervalTimer = 0f;
			GenerateNextDrops();
		}
	}

	private void GenerateNextDrops()
	{
		int num = generatedLineCount + 1;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = (useTargetAngle ? Tool2D.GetDir(targetAngle + (float)(90 * i)) : GetDropDir(i));
			GenerateDrop(base.transform.position + vector * dropSpacing * num);
		}
		generatedLineCount++;
	}

	private Vector3 GetDropDir(int index)
	{
		if (useCrossDrop)
		{
			return index switch
			{
				0 => Vector3.right, 
				1 => Vector3.left, 
				2 => Vector3.up, 
				_ => Vector3.down, 
			};
		}
		return index switch
		{
			0 => Tool2D.GetDir(45f), 
			1 => Tool2D.GetDir(135f), 
			2 => Tool2D.GetDir(225f), 
			_ => Tool2D.GetDir(315f), 
		};
	}

	private void GenerateDrop(Vector3 point)
	{
		if (!(point != Tool2D.GetNavMeshPointIngoreZ(point)))
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster310_Drop", point, 3f).GetComponent<Monster310_Drop>().Initialize(master, buffed: false);
		}
	}
}
