using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpecialObj223GameData
{
	[Serializable]
	public class SpecialObjCarPiece
	{
		public Vector2 direction = Vector2.up;

		public Vector2 position1;

		public Vector2 position2;

		public SpecialObjCarPiece(Vector2 position1, Vector2 position2, Vector2 direction)
		{
			this.position1 = new Vector2((position1.x > position2.x) ? position2.x : position1.x, (position1.y > position2.y) ? position2.y : position1.y);
			this.position2 = new Vector2((position1.x > position2.x) ? position1.x : position2.x, (position1.y > position2.y) ? position1.y : position2.y);
			this.direction = direction;
		}

		public bool CheckEmpty(Vector2 vector2)
		{
			if (vector2.x >= position1.x && vector2.x <= position2.x && vector2.y >= position1.y && vector2.y <= position2.y)
			{
				return false;
			}
			return true;
		}

		public bool TryDelete(SpecialObj223GameData specialObj223GameData, Vector2 vector2)
		{
			if (vector2.x >= position1.x && vector2.x <= position2.x && vector2.y >= position1.y && vector2.y <= position2.y)
			{
				specialObj223GameData.specialObjCarPieces.Remove(this);
				return true;
			}
			return false;
		}

		public SpecialObjCarPiece CopyNew()
		{
			return new SpecialObjCarPiece(position1, position2, direction);
		}

		public SpecialObjCarPiece CopyFrom(SpecialObjCarPiece target, SpecialObjCarPiece source)
		{
			target.position1 = source.position1;
			target.position2 = source.position2;
			target.direction = source.direction;
			return target;
		}
	}

	[Serializable]
	public class SpecialObjBlock
	{
		public Vector2 position1;

		public Vector2 position2;

		public SpecialObjBlock(Vector2 position1, Vector2 position2)
		{
			this.position1 = new Vector2((position1.x > position2.x) ? position2.x : position1.x, (position1.y > position2.y) ? position2.y : position1.y);
			this.position2 = new Vector2((position1.x > position2.x) ? position1.x : position2.x, (position1.y > position2.y) ? position1.y : position2.y);
		}

		public bool CheckEmpty(Vector2 vector2)
		{
			if (vector2.x >= position1.x && vector2.x <= position2.x && vector2.y >= position1.y && vector2.y <= position2.y)
			{
				return false;
			}
			return true;
		}

		public bool TryDelete(SpecialObj223GameData specialObj223GameData, Vector2 vector2)
		{
			if (vector2.x >= position1.x && vector2.x <= position2.x && vector2.y >= position1.y && vector2.y <= position2.y)
			{
				specialObj223GameData.blocks.Remove(this);
				return true;
			}
			return false;
		}

		public SpecialObjBlock Copy()
		{
			return new SpecialObjBlock(position1, position2);
		}
	}

	public int width;

	public int height;

	public List<SpecialObjCarPiece> specialObjCarPieces = new List<SpecialObjCarPiece>();

	public List<SpecialObjBlock> blocks = new List<SpecialObjBlock>();

	public bool CheckEmpty(Vector2 vector2)
	{
		foreach (SpecialObjBlock block in blocks)
		{
			if (!block.CheckEmpty(vector2))
			{
				return false;
			}
		}
		foreach (SpecialObjCarPiece specialObjCarPiece in specialObjCarPieces)
		{
			if (!specialObjCarPiece.CheckEmpty(vector2))
			{
				return false;
			}
		}
		return true;
	}

	public void DeleteAt(Vector2 vector2)
	{
		for (int i = 0; i < blocks.Count; i++)
		{
			if (blocks[i].TryDelete(this, vector2))
			{
				return;
			}
		}
		for (int j = 0; j < specialObjCarPieces.Count && !specialObjCarPieces[j].TryDelete(this, vector2); j++)
		{
		}
	}

	public Vector3 CheckClick(ref SpecialObjCarPiece specialObjCarPiece)
	{
		SpecialObjCarPiece specialObjCarPiece2 = specialObjCarPiece.CopyNew();
		TryOneDirection(specialObjCarPiece2);
		if (CheckOutLevel(specialObjCarPiece2.position1, specialObjCarPiece2.position2))
		{
			specialObjCarPiece = specialObjCarPiece2.CopyFrom(specialObjCarPiece, specialObjCarPiece2);
			return specialObjCarPiece.position1;
		}
		specialObjCarPiece2 = specialObjCarPiece.CopyNew();
		specialObjCarPiece2.direction *= -1f;
		TryOneDirection(specialObjCarPiece2);
		if (CheckOutLevel(specialObjCarPiece2.position1, specialObjCarPiece2.position2))
		{
			specialObjCarPiece = specialObjCarPiece2.CopyFrom(specialObjCarPiece, specialObjCarPiece2);
			return specialObjCarPiece.position1;
		}
		return TryOneDirection(specialObjCarPiece);
		Vector3 TryOneDirection(SpecialObjCarPiece _specialObjCarPiece)
		{
			int num = 0;
			while (num < 999)
			{
				num++;
				float num2 = 1f;
				if ((_specialObjCarPiece.direction == Vector2.left && _specialObjCarPiece.position2.x < 0f - num2) || (_specialObjCarPiece.direction == Vector2.right && _specialObjCarPiece.position1.x >= (float)width + num2) || (_specialObjCarPiece.direction == Vector2.up && _specialObjCarPiece.position1.y >= (float)height + num2) || (_specialObjCarPiece.direction == Vector2.down && _specialObjCarPiece.position2.y < 0f - num2))
				{
					_specialObjCarPiece.direction *= -1f;
					return _specialObjCarPiece.position1;
				}
				Vector2 vector = default(Vector2);
				if (_specialObjCarPiece.direction == Vector2.left)
				{
					vector = _specialObjCarPiece.position1 + _specialObjCarPiece.direction;
				}
				else if (_specialObjCarPiece.direction == Vector2.right)
				{
					vector = _specialObjCarPiece.position2 + _specialObjCarPiece.direction;
				}
				else if (_specialObjCarPiece.direction == Vector2.up)
				{
					vector = _specialObjCarPiece.position2 + _specialObjCarPiece.direction;
				}
				if (_specialObjCarPiece.direction == Vector2.down)
				{
					vector = _specialObjCarPiece.position1 + _specialObjCarPiece.direction;
				}
				if (CheckEmpty(vector))
				{
					_specialObjCarPiece.position1 += _specialObjCarPiece.direction;
					_specialObjCarPiece.position2 += _specialObjCarPiece.direction;
				}
				else
				{
					_specialObjCarPiece.direction *= -1f;
					if (num != 1)
					{
						return _specialObjCarPiece.position1;
					}
				}
			}
			_specialObjCarPiece.direction *= -1f;
			return _specialObjCarPiece.position1;
		}
	}

	public bool CheckOutLevel(Vector2 position1, Vector2 position2)
	{
		if (position2.x < 0f || position1.x >= (float)width || position1.y >= (float)height || position2.y < 0f)
		{
			return true;
		}
		return false;
	}

	public bool RemoveAt(Vector2 position1)
	{
		for (int i = 0; i < specialObjCarPieces.Count; i++)
		{
			if (specialObjCarPieces[i].position1 == position1)
			{
				specialObjCarPieces.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public SpecialObj223GameData Copy()
	{
		SpecialObj223GameData specialObj223GameData = new SpecialObj223GameData();
		specialObj223GameData.height = height;
		specialObj223GameData.width = width;
		foreach (SpecialObjCarPiece specialObjCarPiece in specialObjCarPieces)
		{
			specialObj223GameData.specialObjCarPieces.Add(specialObjCarPiece.CopyNew());
		}
		foreach (SpecialObjBlock block in blocks)
		{
			specialObj223GameData.blocks.Add(block.Copy());
		}
		return specialObj223GameData;
	}
}
