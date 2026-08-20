using UnityEngine;

public class Relic_RemoteShoot : MonoBehaviour
{
	public enum matType
	{
		Normal,
		Green,
		White,
		Brown
	}

	public LineRenderer lr_Hand;

	public LineRenderer lr_Shadow;

	public float minDistance;

	public Material mat_Normal;

	public Material mat_Frog;

	public Material mat_White;

	public Material mat_Brown;

	private matType nowMatType;

	private Vector3 wandRootPointOffset;

	private Vector3 point1Offset;

	private Vector3 point2Offset;

	private float originalLRWidth;

	public RelicConfig RelicCfg { get; private set; }

	private void CheckMat()
	{
		matType matType = matType.Normal;
		if (DataMgr.selectedWorldData.playerLook == PlayerLook.Frog && PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow == null && PlayerMgr.Inst.ItemCtrller.relic_Reaper == null && PlayerMgr.Inst.ItemCtrller.relic_Huang == null && PlayerMgr.Inst.ItemCtrller.uiRelic_DaveHarpoons == null && PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard == null)
		{
			matType = matType.Green;
		}
		else if ((DataMgr.selectedWorldData.playerLook == PlayerLook.TVMan || DataMgr.selectedWorldData.playerLook == PlayerLook.SnowMan) && PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow == null && PlayerMgr.Inst.ItemCtrller.relic_Reaper == null && PlayerMgr.Inst.ItemCtrller.relic_Huang == null && PlayerMgr.Inst.ItemCtrller.uiRelic_DaveHarpoons == null && PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard == null)
		{
			matType = matType.White;
		}
		else if (DataMgr.selectedWorldData.playerLook == PlayerLook.Horse && PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow == null && PlayerMgr.Inst.ItemCtrller.relic_Reaper == null && PlayerMgr.Inst.ItemCtrller.relic_Huang == null && PlayerMgr.Inst.ItemCtrller.uiRelic_DaveHarpoons == null && PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard == null)
		{
			matType = matType.Brown;
		}
		if (matType != nowMatType)
		{
			nowMatType = matType;
			Object.Destroy(lr_Hand.material);
			switch (nowMatType)
			{
			case matType.Normal:
				lr_Hand.material = mat_Normal;
				break;
			case matType.Green:
				lr_Hand.material = mat_Frog;
				break;
			case matType.White:
				lr_Hand.material = mat_White;
				break;
			case matType.Brown:
				lr_Hand.material = mat_Brown;
				break;
			}
		}
	}

	private void LateUpdate()
	{
		CheckMat();
		if (PlayerMgr.Inst.SelectedWand == null || PlayerMgr.Inst.SelectedWand.passiveAutoWand)
		{
			if (lr_Hand.gameObject.activeSelf)
			{
				lr_Hand.gameObject.SetActive(value: false);
				lr_Shadow.gameObject.SetActive(value: false);
			}
			return;
		}
		if (Spell1016Dash.playerOnboard == lr_Hand.gameObject.activeSelf)
		{
			lr_Hand.gameObject.SetActive(!Spell1016Dash.playerOnboard);
			lr_Shadow.gameObject.SetActive(!Spell1016Dash.playerOnboard);
		}
		if (PlayerMgr.Inst.PlayerPpt.Affect_InAbyss)
		{
			PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.position = PlayerMgr.Inst.PlayerPoint + wandRootPointOffset * PlayerMgr.Inst.PlayerT.localScale.x;
			Vector3 vector = PlayerMgr.Inst.PlayerPoint + point1Offset * PlayerMgr.Inst.PlayerT.localScale.x;
			Vector3 vector2 = PlayerMgr.Inst.PlayerPoint + point2Offset * PlayerMgr.Inst.PlayerT.localScale.x;
			lr_Hand.SetPosition(0, Tool2D.GetLayerPoint(vector));
			lr_Hand.SetPosition(1, Tool2D.GetLayerPoint(vector2));
			lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(vector, 1.05f));
			lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(vector2, 1.05f));
		}
		if (lr_Hand.widthMultiplier != originalLRWidth * PlayerMgr.Inst.PlayerT.localScale.x)
		{
			lr_Hand.widthMultiplier = originalLRWidth * PlayerMgr.Inst.PlayerT.localScale.x;
			lr_Shadow.widthMultiplier = originalLRWidth * PlayerMgr.Inst.PlayerT.localScale.x;
		}
		if (PlayerMgr.Inst.PlayerPpt.SR_Models != null && PlayerMgr.Inst.PlayerPpt.SR_Models.Length != 0 && PlayerMgr.Inst.PlayerPpt.SR_Models[0] != null)
		{
			if (lr_Hand.startColor != PlayerMgr.Inst.PlayerPpt.BaseColor)
			{
				lr_Hand.startColor = PlayerMgr.Inst.PlayerPpt.BaseColor;
				lr_Hand.endColor = PlayerMgr.Inst.PlayerPpt.BaseColor;
			}
			float @float = lr_Hand.material.GetFloat("_Alpha");
			float float2 = PlayerMgr.Inst.PlayerPpt.SR_Models[0].material.GetFloat("_Alpha");
			if (@float != float2)
			{
				lr_Hand.material.SetFloat("_Alpha", float2);
			}
			float float3 = lr_Hand.material.GetFloat("_PetrifactionLerp");
			float float4 = PlayerMgr.Inst.PlayerPpt.SR_Models[0].material.GetFloat("_PetrifactionLerp");
			if (float3 != float4)
			{
				lr_Hand.material.SetFloat("_PetrifactionLerp", float4);
			}
		}
		if (PlayerMgr.Inst.PlayerCtrller.CanMotion && !PlayerMgr.Inst.PlayerCtrller.isFrozen)
		{
			if ((PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint - PlayerMgr.Inst.PlayerPoint).sqrMagnitude < minDistance * minDistance)
			{
				PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.position = PlayerMgr.Inst.PlayerPoint;
			}
			else if ((PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint - PlayerMgr.Inst.PlayerPoint).sqrMagnitude < RelicCfg.float1.result * RelicCfg.float1.result)
			{
				PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.position = PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint - (PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint - PlayerMgr.Inst.PlayerPoint).normalized * minDistance;
			}
			else
			{
				PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.position = PlayerMgr.Inst.PlayerPoint + (PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint - PlayerMgr.Inst.PlayerPoint).normalized * (RelicCfg.float1.result - minDistance);
			}
			Vector3 vector3 = PlayerMgr.Inst.PlayerPoint + PlayerMgr.Inst.PlayerCtrller.tsf_WandPoint.position - PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.position;
			Vector3 position = PlayerMgr.Inst.PlayerCtrller.tsf_WandPoint.position;
			float z = PlayerMgr.Inst.PlayerPpt.tsf_Layer.position.z;
			z = ((!(PlayerMgr.Inst.GetMousePoint().y > PlayerMgr.Inst.PlayerPoint.y)) ? (z - 0.001f) : (z + 0.001f));
			lr_Hand.SetPosition(0, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(vector3), z));
			lr_Hand.SetPosition(1, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(position), z));
			lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(vector3, 1.05f));
			lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(position, 1.05f));
			wandRootPointOffset = PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.position - PlayerMgr.Inst.PlayerPoint;
			point1Offset = vector3 - PlayerMgr.Inst.PlayerPoint;
			point2Offset = position - PlayerMgr.Inst.PlayerPoint;
		}
	}

	public void Initialize(RelicConfig relicCfg)
	{
		RelicCfg = relicCfg;
		originalLRWidth = lr_Hand.startWidth;
	}

	public void DestroySelf()
	{
		PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.position = PlayerMgr.Inst.PlayerPoint;
		Object.Destroy(base.gameObject);
	}
}
