using UnityEngine;

public class LayerMgr
{
	public const string Abyss = "Abyss";

	public const string Brittleness = "Brittleness";

	public const string Cliff = "Cliff";

	public const string Default = "Default";

	public const string Model = "Model";

	public const string Monster = "Monster";

	public const string MonsterFly = "Monster_Fly";

	public const string MonsterGhost = "Monster_Ghost";

	public const string NavAction = "NavAction";

	public const string NavGround = "NavGround";

	public const string NavFly = "NavFly";

	public const string Player = "Player";

	public const string Player_Fly = "Teammate_Fly";

	public const string T6Boundary = "T6Boundary";

	public const string T6FakeLight = "T6FakeLight";

	public const string TeammateFly = "Teammate_Fly";

	public const string Wall = "Wall";

	public const string Invisible = "Invisible";

	public const string Item = "Item";

	public const string Destructible = "Destructible";

	public const int Abyss_int = 10;

	public const int Brittleness_int = 15;

	public const int Cliff_int = 16;

	public const int Default_int = 0;

	public const int Model_int = 29;

	public const int Monster_int = 11;

	public const int MonsterFly_int = 12;

	public const int MonsterGhost_int = 13;

	public const int NavAction_int = 19;

	public const int NavGround_int = 6;

	public const int NavFly_int = 22;

	public const int Player_int = 9;

	public const int T6Boundary_int = 28;

	public const int T6FakeLight_int = 27;

	public const int TeammateFly_int = 21;

	public const int Wall_int = 8;

	public const int Invisible_int = 7;

	public const int Item_int = 18;

	public const int Destructible_int = 17;

	private static int _invisibleMask = -1;

	public static int InvisibleMask
	{
		get
		{
			if (_invisibleMask == -1)
			{
				_invisibleMask = LayerMask.GetMask("Invisible");
			}
			return _invisibleMask;
		}
	}
}
