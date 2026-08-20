using UnityEngine;

public class Boss8_AnimationAction : MonoBehaviour
{
	public Boss8 boss8;

	public SpriteRenderer rightArmSpriteRenderer;

	public SpriteRenderer leftArmSpriteRenderer;

	public Transform head;

	public Transform headParent;

	public void SetLeftArmMask(int actionIndex)
	{
		switch (actionIndex)
		{
		case 0:
			rightArmSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			break;
		case 1:
			rightArmSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
			break;
		case 2:
			leftArmSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			break;
		case 3:
			leftArmSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
			break;
		}
	}

	public void SetArmsOut()
	{
		boss8.SetArmsOutside();
	}

	public void SetArmsNone()
	{
		boss8.SetArmsNone();
	}

	public void DoubleCircleAttack(int actionIdnex)
	{
		boss8.DoubleCircleAttack(actionIdnex);
	}

	public void HorizontalAttack()
	{
		boss8.breath = true;
		boss8.breathTimer = 1f;
	}

	public void SetHeadPosition()
	{
		head.localPosition = Vector3.zero;
	}

	public void StopBreath(int actionIndex)
	{
		if (actionIndex == 0)
		{
			boss8.breath = false;
		}
		else
		{
			boss8.breath = true;
		}
	}

	public void SetHeadFollow()
	{
		boss8.headFollow = true;
		head.localPosition = new Vector3(3.8f, 0f, -0.6f);
	}

	public void RocketAttack(float angleOffset)
	{
		boss8.RocketAttack(angleOffset);
	}

	public void BounceKinAttack(int actionIndex)
	{
		boss8.BounceKinAttack(actionIndex);
	}

	public void GenerateKnife(int actionIndex)
	{
		boss8.GenerateKnife(actionIndex);
	}

	public void DoubleArc()
	{
		boss8.DoubleArc();
	}

	public void SetCanAttack()
	{
		boss8.canAttack = true;
	}

	public void FollowSkull()
	{
		boss8.FollowSkull();
	}

	public void FallRightAngle()
	{
		boss8.FallRightAngle();
	}
}
