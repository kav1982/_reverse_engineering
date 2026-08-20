using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Boss4_ReverseBeam : LayerCorrect
{
	private enum BeamState
	{
		Close,
		Warning,
		Open
	}

	[Space(50f)]
	public GameObject go_Stage1_Warning;

	public ParticleSystem ps_Stage1_Warning;

	public GameObject go_Stage1_Open;

	public ParticleSystem ps_Stage1_Open;

	public GameObject go_Stage2_Warning;

	public ParticleSystem ps_Stage2_Warning;

	public GameObject go_Stage2_Open;

	public ParticleSystem ps_Stage2_Open;

	public GameObject go_Stage3_Warning;

	public ParticleSystem ps_Stage3_Warning;

	public GameObject go_Stage3_Open;

	public ParticleSystem ps_Stage3_Open;

	public LayerMask checkLayer;

	public float checkInterval;

	public float reverseDuration;

	public float beamRadius;

	public float beamHalfAngle;

	private BeamState state;

	private Boss4 boss4;

	private float checkIntervalTimer;

	private void Update()
	{
		switch (state)
		{
		case BeamState.Warning:
			base.transform.position = boss4.transform.position;
			base.transform.up = boss4.ReverseBeamDir;
			break;
		case BeamState.Open:
			if (boss4.BossStage == Boss4Stage.Stage3)
			{
				if (base.transform.position != LevelMgr.Inst.CurrentRoomCtrller.CenterPoint)
				{
					base.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
					base.transform.rotation = Quaternion.identity;
				}
			}
			else
			{
				base.transform.position = boss4.transform.position;
				base.transform.up = boss4.ReverseBeamDir;
			}
			checkIntervalTimer += Time.deltaTime;
			if (!(checkIntervalTimer >= checkInterval))
			{
				break;
			}
			checkIntervalTimer = 0f;
			switch (boss4.BossStage)
			{
			case Boss4Stage.Stage1:
			{
				UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(boss4.transform.position, base.transform.up, beamRadius, 100f, GameConst.Filter_Friendly);
				for (int j = 0; j < array.Length; j++)
				{
					UnitProperty_Dots componentData3 = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(array[j].entity);
					componentData3.SetReverseMove(reverseDuration);
					UnitDotsSyncSystem.SetComponentData(componentData3, array[j].entity);
				}
				break;
			}
			case Boss4Stage.Stage2:
			{
				if (Vector3.Angle(base.transform.up, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position)) < beamHalfAngle)
				{
					UnitProperty_Dots componentData4 = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
					componentData4.SetReverseMove(reverseDuration);
					UnitDotsSyncSystem.SetComponentData(componentData4, PlayerMgr.Inst.PlayerEtt);
				}
				for (int k = 0; k < LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList.Count; k++)
				{
					Entity entity2 = LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList[k];
					if (Vector3.Angle(base.transform.up, Tool2D.IgnoreZV2ToV1Normal(UnitDotsSyncSystem.GetComponentData<LocalTransform>(entity2).Position, base.transform.position)) < beamHalfAngle)
					{
						UnitProperty_Dots componentData5 = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(entity2);
						componentData5.SetReverseMove(reverseDuration);
						UnitDotsSyncSystem.SetComponentData(componentData5, entity2);
					}
				}
				break;
			}
			case Boss4Stage.Stage3:
			{
				UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
				componentData.SetReverseMove(reverseDuration);
				UnitDotsSyncSystem.SetComponentData(componentData, PlayerMgr.Inst.PlayerEtt);
				for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList.Count; i++)
				{
					Entity entity = LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList[i];
					UnitProperty_Dots componentData2 = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(entity);
					componentData2.SetReverseMove(reverseDuration);
					UnitDotsSyncSystem.SetComponentData(componentData2, entity);
				}
				break;
			}
			default:
				Debug.LogError(boss4.BossStage);
				break;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case BeamState.Close:
			break;
		}
	}

	public void Initialize(Boss4 boss4)
	{
		this.boss4 = boss4;
	}

	public void Warning()
	{
		state = BeamState.Warning;
		switch (boss4.BossStage)
		{
		case Boss4Stage.Stage1:
			go_Stage1_Warning.SetActive(value: true);
			ps_Stage1_Warning.Play();
			break;
		case Boss4Stage.Stage2:
			go_Stage2_Warning.SetActive(value: true);
			ps_Stage2_Warning.Play();
			break;
		case Boss4Stage.Stage3:
			go_Stage3_Warning.SetActive(value: true);
			ps_Stage3_Warning.Play();
			break;
		default:
			Debug.LogError(boss4.BossStage);
			break;
		}
	}

	public void Open()
	{
		state = BeamState.Open;
		switch (boss4.BossStage)
		{
		case Boss4Stage.Stage1:
			go_Stage1_Warning.SetActive(value: false);
			go_Stage1_Open.SetActive(value: true);
			ps_Stage1_Open.Play();
			break;
		case Boss4Stage.Stage2:
			go_Stage2_Warning.SetActive(value: false);
			go_Stage2_Open.SetActive(value: true);
			ps_Stage2_Open.Play();
			break;
		case Boss4Stage.Stage3:
			go_Stage3_Warning.SetActive(value: false);
			go_Stage3_Open.SetActive(value: true);
			ps_Stage3_Open.Play();
			break;
		default:
			Debug.LogError(boss4.BossStage);
			break;
		}
	}

	public void Close()
	{
		state = BeamState.Close;
		switch (boss4.BossStage)
		{
		case Boss4Stage.Stage1:
			go_Stage1_Open.SetActive(value: false);
			ps_Stage1_Warning.Stop();
			ps_Stage1_Open.Stop();
			break;
		case Boss4Stage.Stage2:
			go_Stage2_Open.SetActive(value: false);
			ps_Stage2_Warning.Stop();
			ps_Stage2_Open.Stop();
			break;
		case Boss4Stage.Stage3:
			go_Stage3_Open.SetActive(value: false);
			ps_Stage3_Warning.Stop();
			ps_Stage3_Open.Stop();
			break;
		default:
			Debug.LogError(boss4.BossStage);
			break;
		}
	}
}
