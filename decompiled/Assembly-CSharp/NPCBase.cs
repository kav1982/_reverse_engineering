using System.Collections;
using Spine;
using Spine.Unity;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

public abstract class NPCBase : InteractiveObj
{
	[Space(50f)]
	public SkeletonAnimation sAnima_Outline;

	public SkeletonAnimation sAnima;

	public UnityEngine.Material mat_Original;

	public UnityEngine.Material mat_Outline;

	public Entity belongEtt;

	private EntityManager ettMgr;

	protected GameObject go_HDBubble;

	protected bool finishPlot;

	private bool _updateSkeleton;

	private float _lastFrameDeltaTime;

	private int wait2FrameToCreateEtt;

	protected abstract NPCPlot ImportantPlot { get; }

	protected abstract NPCPlot SchedulePlot { get; }

	protected abstract NPCPlot CasualPlot { get; }

	protected abstract NPCPlot RandomPlot { get; }

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public virtual void Start()
	{
		sAnima_Outline.gameObject.SetActive(value: false);
		if (GameMgr.IsMobile_Static)
		{
			sAnima_Outline.UpdateTiming = Spine.Unity.UpdateTiming.ManualUpdate;
			sAnima.UpdateTiming = Spine.Unity.UpdateTiming.ManualUpdate;
		}
		CheckPlot();
	}

	public virtual void Hide()
	{
		if (go_HDBubble != null)
		{
			go_HDBubble.SetActive(value: false);
		}
		base.gameObject.SetActive(value: false);
	}

	public virtual void Show()
	{
		if (!finishPlot && go_HDBubble != null)
		{
			go_HDBubble.SetActive(value: true);
		}
		base.gameObject.SetActive(value: true);
	}

	public virtual void HidePlot()
	{
		if (go_HDBubble != null)
		{
			finishPlot = true;
			go_HDBubble.SetActive(value: false);
			go_HDBubble = null;
		}
	}

	public virtual void CheckPlot()
	{
		HidePlot();
		if (base.gameObject.activeSelf)
		{
			finishPlot = false;
			if (ImportantPlot.hdID != 0 && !ImportantPlot.isInteract && HardDialogueConfig.dic.ContainsKey(ImportantPlot.hdID))
			{
				go_HDBubble = UIDialogueMgr.GetHDBubble(base.transform);
			}
			else if (SchedulePlot.hdID != 0 && !SchedulePlot.isInteract && HardDialogueConfig.dic.ContainsKey(SchedulePlot.hdID))
			{
				go_HDBubble = UIDialogueMgr.GetHDBubble(base.transform);
			}
			else if (CasualPlot.hdID != 0 && !CasualPlot.isInteract && HardDialogueConfig.dic.ContainsKey(CasualPlot.hdID))
			{
				go_HDBubble = UIDialogueMgr.GetHDBubble(base.transform);
			}
			else if (RandomPlot.hdID != 0 && !RandomPlot.isInteract && HardDialogueConfig.dic.ContainsKey(RandomPlot.hdID))
			{
				go_HDBubble = UIDialogueMgr.GetHDBubble(base.transform);
			}
			else
			{
				finishPlot = true;
			}
		}
	}

	public override void Interact()
	{
		if (finishPlot)
		{
			UseNPCFunction();
			return;
		}
		finishPlot = true;
		go_HDBubble.SetActive(value: false);
		go_HDBubble = null;
		if (ImportantPlot.hdID != 0 && !ImportantPlot.isInteract && HardDialogueConfig.dic.ContainsKey(ImportantPlot.hdID))
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(ImportantPlot.hdID, DialogEvent, delegate
			{
				ImportantPlot.isInteract = true;
				OnDialogFinish(ImportantPlot.hdID);
				DataMgr.SaveSelectedWorldData();
			});
			return;
		}
		if (SchedulePlot.hdID != 0 && !SchedulePlot.isInteract && HardDialogueConfig.dic.ContainsKey(SchedulePlot.hdID))
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(SchedulePlot.hdID, DialogEvent, delegate
			{
				SchedulePlot.isInteract = true;
				OnDialogFinish(SchedulePlot.hdID);
				DataMgr.SaveSelectedWorldData();
			});
			return;
		}
		if (CasualPlot.hdID != 0 && !CasualPlot.isInteract && HardDialogueConfig.dic.ContainsKey(CasualPlot.hdID))
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(CasualPlot.hdID, DialogEvent, delegate
			{
				CasualPlot.isInteract = true;
				OnDialogFinish(CasualPlot.hdID);
				DataMgr.SaveSelectedWorldData();
			});
			return;
		}
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(RandomPlot.hdID, DialogEvent, delegate
		{
			RandomPlot.isInteract = true;
			DataMgr.selectedWorldData.AddRandomFinishedDialogueID(RandomPlot.hdID);
			OnDialogFinish(RandomPlot.hdID);
			DataMgr.SaveSelectedWorldData();
		});
	}

	public abstract void UseNPCFunction();

	protected virtual void OnDialogFinish(int hdId)
	{
	}

	protected virtual void DialogEvent(string eventName)
	{
	}

	public override void Select()
	{
		sAnima_Outline.CustomMaterialOverride.Add(mat_Original, mat_Outline);
		sAnima_Outline.AnimationState.ClearTracks();
		StartCoroutine(DelayOpenOutline());
		TrackEntry trackEntry = sAnima_Outline.AnimationState.SetAnimation(0, sAnima.AnimationName, loop: true);
		trackEntry.AnimationStart = 0f;
		trackEntry.MixDuration = 0f;
		trackEntry.TrackTime = sAnima.AnimationState.GetCurrent(0).AnimationTime;
	}

	private IEnumerator DelayOpenOutline()
	{
		yield return new WaitForEndOfFrame();
		if (GameMgr.IsMobile_Static)
		{
			yield return new WaitForEndOfFrame();
		}
		sAnima_Outline.gameObject.SetActive(value: true);
	}

	public override void Unselect()
	{
		StopAllCoroutines();
		sAnima_Outline.gameObject.SetActive(value: false);
		sAnima_Outline.CustomMaterialOverride.Remove(mat_Original);
	}

	public void Update()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (_updateSkeleton)
			{
				_updateSkeleton = false;
				sAnima_Outline.Update(Time.deltaTime + _lastFrameDeltaTime);
				sAnima.Update(Time.deltaTime + _lastFrameDeltaTime);
			}
			else
			{
				_updateSkeleton = true;
				_lastFrameDeltaTime = Time.deltaTime;
			}
		}
		if (!(belongEtt == Entity.Null))
		{
			return;
		}
		wait2FrameToCreateEtt++;
		if (wait2FrameToCreateEtt <= 9)
		{
			return;
		}
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(AllMixedEtt));
		if (entityQuery.CalculateEntityCount() > 0)
		{
			belongEtt = QuickCreateSystem.Inst.CreateMixedEtt("NPC", base.transform.position);
			LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(belongEtt);
			componentData.Position = base.transform.position;
			entityManager.SetComponentData(belongEtt, componentData);
			NPCBaseComponent componentData2 = entityManager.GetComponentData<NPCBaseComponent>(belongEtt);
			componentData2.npcBase = this;
			entityManager.SetComponentData(belongEtt, componentData2);
			InteractiveObj_Dots componentData3 = entityManager.GetComponentData<InteractiveObj_Dots>(belongEtt);
			if (this is NPC1Vivian)
			{
				componentData3.type = InteractiveObjType.NPC1Vivian;
			}
			if (this is NPC2Nimue)
			{
				componentData3.type = InteractiveObjType.NPC2Nimue;
			}
			if (this is NPC3)
			{
				componentData3.type = InteractiveObjType.NPC3;
			}
			if (this is NPC4)
			{
				componentData3.type = InteractiveObjType.NPC4;
			}
			if (this is NPC5)
			{
				componentData3.type = InteractiveObjType.NPC5;
			}
			if (this is NPC6)
			{
				componentData3.type = InteractiveObjType.NPC6;
			}
			if (this is NPC7)
			{
				componentData3.type = InteractiveObjType.NPC7;
			}
			if (this is NPC9_EndlessGuide)
			{
				componentData3.type = InteractiveObjType.NPC9_EndlessGuide;
			}
			entityManager.SetComponentData(belongEtt, componentData3);
			if (base.gameObject.tag == "Untagged")
			{
				PhysicsCollider collider = entityManager.GetComponentData<PhysicsCollider>(belongEtt);
				collider.MakeUnique(in belongEtt, entityManager);
				DTool.SetCollider(in collider, 512u);
				entityManager.SetComponentData(belongEtt, collider);
			}
		}
	}
}
