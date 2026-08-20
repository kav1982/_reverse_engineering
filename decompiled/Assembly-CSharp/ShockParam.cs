using System;

[Serializable]
public struct ShockParam
{
	public float radius;

	public float speed;

	public float time;

	public ShockParam(float radius, float speed, float time)
	{
		this.radius = radius;
		this.speed = speed;
		this.time = time;
	}

	public static bool operator ==(ShockParam sp1, ShockParam sp2)
	{
		if (sp1.radius == sp2.radius && sp1.speed == sp2.speed && sp1.time == sp2.time)
		{
			return true;
		}
		return false;
	}

	public static bool operator !=(ShockParam sp1, ShockParam sp2)
	{
		if (sp1.radius != sp2.radius || sp1.speed != sp2.speed || sp1.time != sp2.time)
		{
			return true;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
