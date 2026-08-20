using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class StoryUnlock_Gallary : MonoBehaviour
{
	private enum StoryState
	{
		Focus,
		FocusWait,
		dialogue_delay_start,
		dialogue_start,
		dialogue_in,
		EFAppear,
		Finish,
		end
	}

	public float dialogue_delay = 0.8f;

	public float camFocusSize;

	public float camFocusTime;

	public float focusWaitTime;

	public float efWaitTime;

	public Vector3 smokeOffset;

	private StoryState state;

	private EntityManager ettMgr;

	private Entity focusEtt;

	private int hdID;

	private float timer;

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void Update()
	{
		switch (state)
		{
		case StoryState.Focus:
		{
			GameUISingletonMono<UIResearch>.Inst.Hide();
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			PlayerMgr.Inst.PlayerPpt.InvincibleRegister();
			UIPlayerDataMgr.Inst.Hide();
			UIMgr.Inst.uiFilmBlackEdge.Show(camFocusTime);
			float3 position2 = ettMgr.GetComponentData<LocalToWorld>(focusEtt).Position;
			position2.z = 0f;
			CamController.Inst.FocusOn(camFocusSize, camFocusTime, position2);
			state = StoryState.FocusWait;
			break;
		}
		case StoryState.FocusWait:
			timer += Time.deltaTime;
			if (timer >= focusWaitTime)
			{
				timer = 0f;
				state = StoryState.dialogue_delay_start;
				CampMgr.Inst.SetEttEnable(focusEtt, enable: true);
				float3 position = ettMgr.GetComponentData<LocalToWorld>(focusEtt).Position;
				position.z = 0f;
				position += (float3)smokeOffset;
				ObjPoolMgr.Inst.GetGO("Prefabs/Item/Potion_WhiteSmoke", position, 2f);
				SEMgr.Inst.puzzleSucceed.PlaySE();
			}
			break;
		case StoryState.dialogue_delay_start:
			timer += Time.deltaTime;
			if (timer >= dialogue_delay)
			{
				timer = 0f;
				state = StoryState.dialogue_start;
			}
			break;
		case StoryState.dialogue_start:
			state = StoryState.dialogue_in;
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(hdID, (Action)delegate
			{
				state = StoryState.EFAppear;
			});
			break;
		case StoryState.EFAppear:
			timer += Time.deltaTime;
			if (timer >= efWaitTime)
			{
				state = StoryState.Finish;
				CamController.Inst.FocusRecover(camFocusTime);
				UIMgr.Inst.uiFilmBlackEdge.Hide(camFocusTime, delegate
				{
					PlayerMgr.Inst.PlayerCtrller.StartMotion();
					PlayerMgr.Inst.PlayerPpt.InvincibleUnregister();
					UIPlayerDataMgr.Inst.Show();
					UnityEngine.Object.Destroy(base.gameObject);
				});
			}
			state = StoryState.Finish;
			break;
		default:
			Debug.LogError(state);
			break;
		case StoryState.dialogue_in:
		case StoryState.Finish:
			break;
		}
	}

	public void Initialize(Entity focusEtt, int hdID)
	{
		this.focusEtt = focusEtt;
		this.hdID = hdID;
	}
}
