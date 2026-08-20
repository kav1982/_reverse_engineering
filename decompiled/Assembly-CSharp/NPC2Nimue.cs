using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

public class NPC2Nimue : NPCBase
{
	private bool alreadyCantInteract;

	protected override NPCPlot ImportantPlot => DataMgr.selectedWorldData.npc2NimueImportantPlot;

	protected override NPCPlot SchedulePlot => DataMgr.selectedWorldData.npc2NimueSchedulePlot;

	protected override NPCPlot CasualPlot => DataMgr.selectedWorldData.npc2NimueCasualPlot;

	protected override NPCPlot RandomPlot => DataMgr.selectedWorldData.npc2NimueRandomPlotV2;

	public override void HidePlot()
	{
		base.HidePlot();
		if (finishPlot)
		{
			SetDotsCantInteract();
		}
	}

	public override void CheckPlot()
	{
		base.CheckPlot();
		if (finishPlot)
		{
			SetDotsCantInteract();
		}
		else
		{
			base.tag = "InteractiveObj";
		}
	}

	public override void UseNPCFunction()
	{
		Debug.LogError("想要和妮妙做什么？");
	}

	protected override void OnDialogFinish(int hdId)
	{
		base.OnDialogFinish(hdId);
		SetDotsCantInteract();
	}

	private void SetDotsCantInteract()
	{
		base.gameObject.tag = "Untagged";
		if (!alreadyCantInteract && belongEtt != Entity.Null)
		{
			alreadyCantInteract = true;
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			PhysicsCollider collider = entityManager.GetComponentData<PhysicsCollider>(belongEtt);
			collider.MakeUnique(in belongEtt, entityManager);
			DTool.SetCollider(in collider, 512u);
			entityManager.SetComponentData(belongEtt, collider);
		}
	}
}
