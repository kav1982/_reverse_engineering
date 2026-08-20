public class Stick
{
	public Particle particleA;

	public Particle particleB;

	public float length;

	public Stick(Particle a, Particle b)
	{
		particleA = a;
		particleB = b;
		length = (a.position - b.position).magnitude;
	}
}
