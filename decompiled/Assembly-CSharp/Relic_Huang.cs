using UnityEngine;

public class Relic_Huang : MonoBehaviour
{
	public GameObject go_Active;

	public Transform tsf_JumpRotate;

	public Animator anima;

	[Header("Body")]
	public float bodyInclineSpeed;

	public float walkAnimaSpeed;

	public Transform tsf_Rotate;

	public float rotateSpeed;

	public GameObject[] go_UpOpens;

	public GameObject[] go_DownOpens;

	[Header("Face")]
	public SpriteRenderer sr_Face;

	public Sprite sprite_FaceNormal;

	public Sprite sprite_FaceAttack;

	public Sprite sprite_FaceAmaze;

	public Sprite sprite_FaceDead;

	[Header("Cloak")]
	public LineRenderer lr_Cloak;

	public int nodeCount;

	public float segmentLength;

	public float lerp;

	public float rootHight;

	public float rootOffset;

	public float faceLeftRightAngleOffset;

	public float aaa;

	public float flyRootOffset;

	private UnitProperty_Dots playerPpt;

	private PlayerBodyAnima currentBodyAnima;

	private float currentAnimaTimeScale = 1f;

	private Vector2 currentBodyIncline;

	private float currentWalkAnimaSpeed = 1f;

	private bool currentFaceRight = true;

	private bool isStop;

	private Vector3[] nodePoints;

	private Vector3[] nodePoints2;

	private Vector3 faceRightDir;

	private Vector3 faceLeftDir;

	private Vector3 currentFaceDir;

	private bool inPlot;

	private bool isLie;

	public RelicConfig RelicCfg { get; private set; }

	private PlayerController PlayerCtrller => PlayerMgr.Inst.PlayerCtrller;

	public UIRelic_Huang UIRelicHuang { get; private set; }

	public void Initialize(RelicConfig relicCfg, bool inPlot)
	{
		RelicCfg = relicCfg;
		this.inPlot = inPlot;
		PlayerMgr.Inst.PlayerPpt.SAnima.gameObject.SetActive(value: false);
		if (lr_Cloak.positionCount != nodeCount)
		{
			lr_Cloak.positionCount = nodeCount;
			nodePoints = new Vector3[nodeCount];
			nodePoints2 = new Vector3[nodeCount];
			for (int i = 0; i < nodeCount; i++)
			{
				if (i == 0)
				{
					nodePoints[i] = PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0f, 0f - rootHight);
				}
				else
				{
					nodePoints[i] = nodePoints[i - 1] + Vector3.left * segmentLength;
				}
				lr_Cloak.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position.z));
			}
			faceRightDir = Tool2D.GetDir(180f - faceLeftRightAngleOffset);
			faceLeftDir = Tool2D.GetDir(180f + faceLeftRightAngleOffset);
			currentFaceDir = faceRightDir;
		}
		if (inPlot)
		{
			return;
		}
		if (UIRelicHuang == null)
		{
			UIRelicHuang = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIRelic_Huang"), UIPlayerDataMgr.Inst.rtsf_ActiveRelicUIRoot).GetComponent<UIRelic_Huang>();
			if (GameMgr.IsMobile_Static)
			{
				RectTransform component = UIRelicHuang.GetComponent<RectTransform>();
				component.anchoredPosition = new Vector2(UIPlayerDataMgr.Inst.skillUIOffsetMobile[0], component.anchoredPosition.y);
			}
		}
		UIRelicHuang.Initialize(this);
	}

	private void Update()
	{
		PlayerMgr.Inst.TryGetPlayerPpt(out playerPpt);
		BodyControl();
		ClockControl();
	}

	private void BodyControl()
	{
		if (!inPlot && PlayerMgr.Inst.PlayerPpt.tsf_Layer.gameObject.activeSelf != go_Active.activeSelf)
		{
			go_Active.SetActive(PlayerMgr.Inst.PlayerPpt.tsf_Layer.gameObject.activeSelf);
		}
		if (!PlayerMgr.Inst.PlayerPpt.tsf_Layer.gameObject.activeSelf)
		{
			return;
		}
		base.transform.position = PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position;
		if (inPlot || isStop)
		{
			return;
		}
		SetBodyAnima(PlayerCtrller.CurrentBodyAnima, PlayerCtrller.CurrentAnimaTimeScale);
		if (currentBodyAnima == PlayerBodyAnima.Dead)
		{
			return;
		}
		if (!PlayerCtrller.isStandInLastFrame)
		{
			if (!playerPpt.IsFly)
			{
				if (PlayerCtrller.CurrentMotion.x > 0f)
				{
					tsf_Rotate.transform.Rotate(0f, 0f, (0f - PlayerMgr.Inst.PlayerPpt.MoveSpeed) * rotateSpeed * Time.deltaTime);
				}
				else
				{
					tsf_Rotate.transform.Rotate(0f, 0f, PlayerMgr.Inst.PlayerPpt.MoveSpeed * rotateSpeed * Time.deltaTime);
				}
			}
			if (currentWalkAnimaSpeed != walkAnimaSpeed)
			{
				currentWalkAnimaSpeed = walkAnimaSpeed;
				anima.SetFloat("WalkAnimaSpeed", currentWalkAnimaSpeed);
			}
		}
		else if (currentWalkAnimaSpeed != 1f)
		{
			currentWalkAnimaSpeed = 1f;
			anima.SetFloat("WalkAnimaSpeed", currentWalkAnimaSpeed);
		}
		if (PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint.y > PlayerMgr.Inst.PlayerPoint.y)
		{
			if (!go_UpOpens[0].activeSelf)
			{
				for (int i = 0; i < go_UpOpens.Length; i++)
				{
					go_UpOpens[i].SetActive(value: true);
				}
				for (int j = 0; j < go_DownOpens.Length; j++)
				{
					go_DownOpens[j].SetActive(value: false);
				}
			}
		}
		else if (go_UpOpens[0].activeSelf)
		{
			for (int k = 0; k < go_UpOpens.Length; k++)
			{
				go_UpOpens[k].SetActive(value: false);
			}
			for (int l = 0; l < go_DownOpens.Length; l++)
			{
				go_DownOpens[l].SetActive(value: true);
			}
		}
		bool flag = PlayerCtrller.ShootWorldPoint.x > base.transform.position.x;
		if (flag && base.transform.localScale.x != 1f)
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (!flag && go_Active.transform.localScale.x != -1f)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		Vector2 b = PlayerCtrller.CurrentMoveDir;
		if (!flag)
		{
			b.x = 0f - b.x;
		}
		if (currentFaceRight != flag)
		{
			currentFaceRight = flag;
			currentBodyIncline.x = 0f - currentBodyIncline.x;
		}
		currentBodyIncline = Vector2.Lerp(currentBodyIncline, b, bodyInclineSpeed * PlayerMgr.Inst.PlayerDeltaTime);
		anima.SetFloat("BodyInclineX", currentBodyIncline.x);
		anima.SetFloat("BodyInclineY", currentBodyIncline.y);
		if (!PlayerCtrller.rigid.isKinematic)
		{
			if (PlayerCtrller.isHoldMouse0)
			{
				FaceAttack();
			}
			else
			{
				FaceNormal();
			}
		}
	}

	private void ClockControl()
	{
		float num = ((PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint.y > PlayerMgr.Inst.PlayerPoint.y) ? (-0.05f) : 0.05f);
		if (inPlot || isStop)
		{
			num = 0.05f;
		}
		else
		{
			currentFaceDir = ((PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint.x > PlayerMgr.Inst.PlayerPoint.x) ? faceRightDir : faceLeftDir);
		}
		if (currentBodyAnima == PlayerBodyAnima.Dead)
		{
			num = 0.05f;
		}
		for (int i = 0; i < nodeCount; i++)
		{
			switch (i)
			{
			case 0:
				if (inPlot)
				{
					nodePoints[i] = base.transform.position + new Vector3(0f, rootHight, 0f);
					if (isLie)
					{
						nodePoints[i].y += -0.2f;
					}
				}
				else
				{
					nodePoints[i] = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position + new Vector3(0f, rootHight, 0f);
				}
				if (currentFaceDir == faceRightDir)
				{
					nodePoints[i].x += rootOffset;
				}
				else
				{
					nodePoints[i].x -= rootOffset;
				}
				if (playerPpt.IsFly)
				{
					nodePoints[i].y += flyRootOffset;
				}
				break;
			case 1:
				nodePoints[i] = nodePoints[i - 1] + currentFaceDir * segmentLength;
				break;
			default:
			{
				Vector3 b = nodePoints[i - 1] - nodePoints[i - 2];
				Vector3 vector = Vector3.Lerp(nodePoints[i] - nodePoints[i - 1], b, lerp);
				Vector3 target = nodePoints[i - 1] + vector.normalized * segmentLength;
				nodePoints[i] = Vector3.SmoothDamp(nodePoints[i], target, ref nodePoints2[i], aaa);
				break;
			}
			}
			Vector3 position = nodePoints[i];
			if (inPlot)
			{
				position.z = base.transform.position.z + num;
			}
			else
			{
				position.z = PlayerMgr.Inst.PlayerCtrller.layerC_PlayerRTRoot.tsf_Layer.position.z + num;
			}
			lr_Cloak.SetPosition(i, position);
		}
	}

	private void SetBodyAnima(PlayerBodyAnima bodyAnima, float timeScale)
	{
		if (PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed != null && !playerPpt.IsFly && (bodyAnima == PlayerBodyAnima.FlyIdleDown || bodyAnima == PlayerBodyAnima.FlyIdleUp || bodyAnima == PlayerBodyAnima.FlyWalkDown || bodyAnima == PlayerBodyAnima.FlyWalkUp))
		{
			bodyAnima = PlayerBodyAnima.GroundIdleDown;
		}
		if (currentBodyAnima == bodyAnima)
		{
			if (currentAnimaTimeScale != timeScale)
			{
				currentAnimaTimeScale = timeScale;
				anima.speed = currentAnimaTimeScale;
			}
			return;
		}
		currentBodyAnima = bodyAnima;
		switch (bodyAnima)
		{
		case PlayerBodyAnima.GroundIdleDown:
		case PlayerBodyAnima.GroundIdleUp:
		case PlayerBodyAnima.GroundWalkDown:
		case PlayerBodyAnima.GroundWalkUp:
			anima.Play("GroundWalk");
			break;
		case PlayerBodyAnima.FlyIdleDown:
		case PlayerBodyAnima.FlyIdleUp:
		case PlayerBodyAnima.FlyWalkDown:
		case PlayerBodyAnima.FlyWalkUp:
			anima.Play("FlyWalk");
			break;
		case PlayerBodyAnima.Dead:
		{
			anima.Play("BigSitDead");
			FaceDead();
			for (int i = 0; i < go_UpOpens.Length; i++)
			{
				go_UpOpens[i].SetActive(value: false);
			}
			for (int j = 0; j < go_DownOpens.Length; j++)
			{
				go_DownOpens[j].SetActive(value: true);
			}
			break;
		}
		default:
			Debug.LogError(bodyAnima);
			break;
		}
	}

	public void AnimaBigSitJump()
	{
		anima.Play("BigSitJump", 1, 0f);
	}

	public void AnimaBigSitOnGround()
	{
		anima.Play("BigSitOnGround", 1, 0f);
	}

	public void AnimaBigSitIdle()
	{
		currentBodyAnima = PlayerBodyAnima.GroundIdleDown;
		anima.Play("BigSitIdle", 1, 0f);
	}

	public void FaceNormal()
	{
		if (sr_Face.sprite != sprite_FaceNormal)
		{
			sr_Face.sprite = sprite_FaceNormal;
		}
	}

	public void FaceAttack()
	{
		if (sr_Face.sprite != sprite_FaceAttack)
		{
			sr_Face.sprite = sprite_FaceAttack;
		}
	}

	public void FaceDead()
	{
		if (sr_Face.sprite != sprite_FaceDead)
		{
			sr_Face.sprite = sprite_FaceDead;
		}
	}

	public void SetStop(bool isStop)
	{
		this.isStop = isStop;
		if (!isStop)
		{
			return;
		}
		if (go_UpOpens[0].activeSelf)
		{
			for (int i = 0; i < go_UpOpens.Length; i++)
			{
				go_UpOpens[i].SetActive(value: false);
			}
			for (int j = 0; j < go_DownOpens.Length; j++)
			{
				go_DownOpens[j].SetActive(value: true);
			}
		}
		currentBodyIncline = Vector2.zero;
		anima.SetFloat("BodyInclineX", currentBodyIncline.x);
		anima.SetFloat("BodyInclineY", currentBodyIncline.y);
		if (currentWalkAnimaSpeed != 1f)
		{
			currentWalkAnimaSpeed = 1f;
			anima.SetFloat("WalkAnimaSpeed", currentWalkAnimaSpeed);
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < nodeCount; i++)
		{
			nodePoints[i] += changeValue;
		}
	}

	public void DestroySelf()
	{
		PlayerMgr.Inst.PlayerPpt.SAnima.gameObject.SetActive(value: true);
		Object.Destroy(UIRelicHuang.gameObject);
		Object.Destroy(base.gameObject);
	}

	public void PlotAmaze()
	{
		anima.Play("Amaze", 0, 0f);
		if (sr_Face.sprite != sprite_FaceAmaze)
		{
			sr_Face.sprite = sprite_FaceAmaze;
		}
	}

	public void PlotIdle()
	{
		anima.Play("GroundWalk", 0, 0f);
		if (sr_Face.sprite != sprite_FaceNormal)
		{
			sr_Face.sprite = sprite_FaceNormal;
		}
	}

	public void PlotFaceRight()
	{
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		currentFaceDir = faceRightDir;
	}

	public void PlotFaceLeft()
	{
		base.transform.localScale = new Vector3(-1f, 1f, 1f);
		currentFaceDir = faceLeftDir;
	}

	public void PlotLie()
	{
		anima.Play("Lie", 0, 0f);
		isLie = true;
		FaceDead();
	}

	public void PlotLieUp()
	{
		anima.Play("LieUp", 0, 0f);
		sr_Face.sprite = sprite_FaceAmaze;
	}
}
