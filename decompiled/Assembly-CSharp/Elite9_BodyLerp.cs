using UnityEngine;

public class Elite9_BodyLerp : MonoBehaviour
{
	public float bodySacleFix;

	public Transform targetObject;

	public Transform verticalFixRoot;

	private float startHorizontalOffset;

	private float startVerticalOffset;

	private float verticalFixRootHeight;

	public Elite9 master;

	private SpriteRenderer mainSprite;

	private Vector3 lastCurrentMotion;

	private Vector3 lastTruePosition;

	public Vector3 truePosition;

	public bool followAnimation;

	public float followAnimationLerp;

	public bool isHead;

	public Transform headParticle;

	public Vector3 headParticleOffset;

	public Sprite headSpriteFront;

	public Sprite headSpriteBack;

	public Sprite headSpriteUp;

	public Sprite headSpriteBackUp;

	[Header("和谐模式")]
	public Sprite headSpriteFront_H;

	public Sprite headSpriteBack_H;

	public Sprite headSpriteUp_H;

	public Sprite headSpriteBackUp_H;

	public bool isBottom;

	public Sprite sprite_Bottom_H;

	private void Start()
	{
		lastCurrentMotion = Vector3.right;
		startHorizontalOffset = targetObject.transform.position.x - master.transform.position.x;
		startVerticalOffset = 0f - Mathf.Abs(targetObject.transform.position.y - master.transform.position.y);
		verticalFixRootHeight = Mathf.Abs(targetObject.transform.position.y - verticalFixRoot.position.y);
		mainSprite = GetComponent<SpriteRenderer>();
		if (isHead)
		{
			headParticleOffset = headParticle.localPosition;
		}
		targetObject.gameObject.SetActive(value: false);
		if (GameMgr.IsHarmony_Static && isBottom)
		{
			mainSprite.sprite = sprite_Bottom_H;
		}
		if (mainSprite != null)
		{
			mainSprite.enabled = true;
		}
		Update();
	}

	private void Update()
	{
		lastCurrentMotion = master.moveDiration;
		if (mainSprite != null)
		{
			if (lastCurrentMotion.x > 0f && mainSprite.flipX)
			{
				mainSprite.flipX = !mainSprite.flipX;
				mainSprite.material.SetFloat(GameConstManaged.shaderFlipXIndex, 1f);
			}
			else if (lastCurrentMotion.x < 0f && !mainSprite.flipX)
			{
				mainSprite.flipX = !mainSprite.flipX;
				mainSprite.material.SetFloat(GameConstManaged.shaderFlipXIndex, -1f);
			}
			if (isHead)
			{
				if (master.state == Elite9.MonsterState.LaserAttack)
				{
					mainSprite.material.SetFloat(GameConstManaged.shaderFlipXIndex, (master.attackDiration.x > 0f) ? 1 : (-1));
				}
				if (Vector3.Angle(Vector3.up, lastCurrentMotion) < 60f)
				{
					if (master.headUp)
					{
						if (GameMgr.IsHarmony_Static)
						{
							mainSprite.sprite = headSpriteBackUp_H;
						}
						else
						{
							mainSprite.sprite = headSpriteBackUp;
						}
					}
					else if (GameMgr.IsHarmony_Static)
					{
						mainSprite.sprite = headSpriteBack_H;
					}
					else
					{
						mainSprite.sprite = headSpriteBack;
					}
					if (mainSprite.flipX)
					{
						Vector3 localPosition = headParticleOffset;
						localPosition.x = 0f - localPosition.x;
						localPosition.z = 0f - localPosition.z;
						headParticle.localPosition = localPosition;
					}
					else
					{
						Vector3 localPosition2 = headParticleOffset;
						localPosition2.z = 0f - localPosition2.z;
						headParticle.localPosition = localPosition2;
					}
				}
				else
				{
					if (master.headUp)
					{
						if (GameMgr.IsHarmony_Static)
						{
							mainSprite.sprite = headSpriteUp_H;
						}
						else
						{
							mainSprite.sprite = headSpriteUp;
						}
					}
					else if (GameMgr.IsHarmony_Static)
					{
						mainSprite.sprite = headSpriteFront_H;
					}
					else
					{
						mainSprite.sprite = headSpriteFront;
					}
					if (mainSprite.flipX)
					{
						Vector3 localPosition3 = headParticleOffset;
						localPosition3.x = 0f - localPosition3.x;
						headParticle.localPosition = localPosition3;
					}
					else
					{
						headParticle.localPosition = headParticleOffset;
					}
				}
			}
		}
		if (followAnimation)
		{
			startHorizontalOffset = targetObject.transform.position.x - master.transform.position.x;
			startVerticalOffset = 0f - Mathf.Abs(targetObject.transform.position.y - master.transform.position.y);
			verticalFixRootHeight = Mathf.Abs(targetObject.transform.position.y - verticalFixRoot.position.y);
		}
		lastTruePosition = truePosition;
		truePosition = master.transform.position + new Vector3(0f, 0f, verticalFixRootHeight + (startVerticalOffset - verticalFixRootHeight) * bodySacleFix) + lastCurrentMotion * startHorizontalOffset * bodySacleFix;
		if (!followAnimation)
		{
			base.transform.position = Tool2D.GetLayerPoint(truePosition);
			return;
		}
		base.transform.position = Tool2D.GetLayerPoint(Vector3.Lerp(lastTruePosition, truePosition, followAnimationLerp * Time.deltaTime));
		if (isHead)
		{
			if (Vector3.Angle(Vector3.up, lastCurrentMotion) < 60f)
			{
				base.transform.position += new Vector3(0f, 0f, 0.005f);
			}
			else
			{
				base.transform.position += new Vector3(0f, 0f, -0.005f);
			}
		}
	}
}
