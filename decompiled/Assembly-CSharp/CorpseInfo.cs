using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CorpseInfo
{
	public CorpseType type;

	public GameObject prefab;

	public VariableFloat forwardForceNoDirect;

	public VariableFloat forwardForceHaveDirect;

	public VariableFloat upForce;

	public VariableFloat scale;

	public VariableInt bounceTime;

	public VariableFloat rotateSpeed;

	public float angleOffset;

	public float bounceRemainRatio;

	public float gravity;

	public float duration;

	public float reduceAlphaSpeed;

	public float minAlpha;

	public bool isEnemyCorpse;

	public bool isBulletShell;

	public List<Color> colors;
}
