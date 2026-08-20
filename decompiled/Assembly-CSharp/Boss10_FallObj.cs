using UnityEngine;

public class Boss10_FallObj : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public MeshRenderer meshRenderer;

	public Sprite[] sprites;

	public float gravity;

	private float verticalSpeed;

	private bool isFalling;

	public float height;

	public ShockParam shockParam;

	public float knockBack;

	public LayerMask attackMask;

	public float detectionRadius;

	public Transform tsfShadow;

	private void OnEnable()
	{
		isFalling = true;
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f - height);
		verticalSpeed = 0f;
		spriteRenderer.sprite = sprites[Random.Range(0, 3)];
		meshRenderer.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[Random.Range(0, 3)].texture);
		ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), new Vector3(base.transform.position.x, base.transform.position.y, 0f)).GetComponent<WarningArea>().Initialize(detectionRadius, 1.6f);
		tsfShadow.gameObject.SetActive(value: true);
		spriteRenderer.enabled = true;
		meshRenderer.enabled = true;
	}

	private void Update()
	{
		tsfShadow.localScale = new Vector3(1f, 0.5f, 1f) * Mathf.Lerp(0f, 1f, (height + base.transform.position.z) / height * 2f);
		if (isFalling)
		{
			verticalSpeed += gravity * Time.deltaTime;
			base.transform.position += new Vector3(0f, 0f, verticalSpeed * Time.deltaTime);
			if (base.transform.position.z >= 0f)
			{
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f);
				isFalling = false;
				OnLand();
			}
		}
	}

	private void OnLand()
	{
		tsfShadow.gameObject.SetActive(value: false);
		spriteRenderer.enabled = false;
		meshRenderer.enabled = false;
		SEMgr.Inst.elite12_RockLand.PlaySE(SEPlayMode.Replay, 3, 0.2f).pitch = Random.Range(0.8f, 1.2f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_Trace", base.transform.position, 12f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_DustDirtySoft", Tool2D.GetLayerPoint(base.transform.position), 2f);
		CamController.Inst.SetShock(shockParam);
		Collider[] array = Physics.OverlapSphere(base.transform.position, detectionRadius, attackMask);
		foreach (Collider collider in array)
		{
			UnitProperty component = collider.GetComponent<UnitProperty>();
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss10.Inst.myPpt.myEntity);
			info.teammateTakeDamageRatio = 3f;
			info.knockbackForce = Vector3.left.normalized * knockBack;
			info.damage = 20f;
			switch (collider.tag)
			{
			case "Player":
			case "Teammate":
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", collider.transform.position, 1f);
				UnitDotsSyncSystem.AddTakeDamageRequest(component.myEntity, info);
				break;
			case "Brittleness":
				UnitDotsSyncSystem.AddTakeDamageRequest(component.myEntity, info);
				break;
			case "Destructible":
				info.damage = 200f;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequest(component.myEntity, info);
				break;
			}
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, 3f);
	}
}
