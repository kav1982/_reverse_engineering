using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public static class DTool
{
	public static float3 IgnoreZPosition(in float3 position, float z = 0f)
	{
		return new float3(position.x, position.y, z);
	}

	public static float3 IgnoreZDir(in float3 to, in float3 from)
	{
		return math.normalizesafe(new float3(to.x, to.y, 0f) - new float3(from.x, from.y, 0f));
	}

	public static float IgnoreZDistanceSqr(in float3 point1, in float3 float2)
	{
		return math.distancesq(new float3(point1.x, point1.y, 0f), new float3(float2.x, float2.y, 0f));
	}

	public static float IgnoreZDistance(in float3 point1, in float3 float2)
	{
		return math.distance(new float3(point1.x, point1.y, 0f), new float3(float2.x, float2.y, 0f));
	}

	public static float3 GetDir(ref Unity.Mathematics.Random random)
	{
		float2 @float = random.NextFloat2Direction();
		return new float3(@float.x, @float.y, 0f);
	}

	public static float3 GetDir(ref Unity.Mathematics.Random random, float minDistance, float maxDistance)
	{
		float2 @float = random.NextFloat2Direction();
		return new float3(@float.x, @float.y, 0f) * random.NextFloat(minDistance, maxDistance);
	}

	public static float3 GetDir(ref Unity.Mathematics.Random random, in RandomFloat randomFloat)
	{
		float2 @float = random.NextFloat2Direction();
		return new float3(@float.x, @float.y, 0f) * random.NextFloat(randomFloat.value1, randomFloat.value2);
	}

	public static float3 GetDir(float degree)
	{
		return math.mul(quaternion.Euler(0f, 0f, degree), new float3(0f, 1f, 0f));
	}

	public static float3 GetDir(in float3 oldDir, float angle)
	{
		float x = math.radians(angle);
		float num = math.cos(x);
		float num2 = math.sin(x);
		float x2 = oldDir.x * num - oldDir.y * num2;
		float y = oldDir.x * num2 + oldDir.y * num;
		return new float3(x2, y, 0f);
	}

	public static quaternion DirectionToRotation(in float2 dir)
	{
		float z = math.atan2(dir.y, dir.x);
		return quaternion.Euler(0f, 0f, z);
	}

	public static quaternion GetRotation(ref Unity.Mathematics.Random random)
	{
		return quaternion.Euler(0f, 0f, random.NextFloat(0f, MathF.PI * 2f));
	}

	public static float Random(ref Unity.Mathematics.Random random, float float1, float float2)
	{
		return random.NextFloat(float1, float2);
	}

	public static int Random(ref Unity.Mathematics.Random random, int int1, int int2)
	{
		return random.NextInt(int1, int2);
	}

	public static float RandomValue(ref Unity.Mathematics.Random random)
	{
		return random.NextFloat(0f, 1f);
	}

	public static bool IsEqual(in float3 f1, in float3 f2)
	{
		return math.all(math.abs(f1 - f2) < 0.0001f);
	}

	public static bool IsTotallySame(in float3 f1, in float3 f2)
	{
		if (f1.x == f2.x && f1.y == f2.y && f1.z == f2.z)
		{
			return true;
		}
		return false;
	}

	public static float GetDirOffset(in float2 a1, in float2 a2)
	{
		float num = math.atan2(a1.y, a1.x);
		return (math.atan2(a2.y, a2.x) - num) * 57.29578f;
	}

	public static bool BoundaryCheck(in BoundaryBase_Dots boundaryBase, in Vector2Data offset)
	{
		for (int i = 0; i < boundaryBase.allBoundary1Position.Value.Length; i++)
		{
			if (boundaryBase.allBoundary1Position.Value[i] == boundaryBase.selfPosition + offset)
			{
				return true;
			}
		}
		for (int j = 0; j < boundaryBase.allBoundary2Position.Value.Length; j++)
		{
			if (boundaryBase.allBoundary2Position.Value[j] == boundaryBase.selfPosition + offset)
			{
				return true;
			}
		}
		return false;
	}

	public static bool BoundaryCheckOnly1(in BoundaryBase_Dots boundaryBase, in Vector2Data offset)
	{
		for (int i = 0; i < boundaryBase.allBoundary1Position.Value.Length; i++)
		{
			if (boundaryBase.allBoundary1Position.Value[i] == boundaryBase.selfPosition + offset)
			{
				return true;
			}
		}
		return false;
	}

	public static bool BoundaryCheckOnly2(in BoundaryBase_Dots boundaryBase, in Vector2Data offset)
	{
		for (int i = 0; i < boundaryBase.allBoundary2Position.Value.Length; i++)
		{
			if (boundaryBase.allBoundary2Position.Value[i] == boundaryBase.selfPosition + offset)
			{
				return true;
			}
		}
		return false;
	}

	public static bool BoundaryCheckTile0(in BoundaryBase_Dots boundaryBase, in Vector2Data offset)
	{
		for (int i = 0; i < boundaryBase.allTile0Position.Value.Length; i++)
		{
			if (boundaryBase.allTile0Position.Value[i] == boundaryBase.selfPosition + offset)
			{
				return true;
			}
		}
		return false;
	}

	public static bool TileCheck(in TileBase_Dots tileBase, in Vector2Data offset)
	{
		for (int i = 0; i < tileBase.allTilePosition.Value.Length; i++)
		{
			if (tileBase.allTilePosition.Value[i] == tileBase.selfPosition + offset)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsSameCamp(UnitType unitType1, UnitType unitType2)
	{
		switch (unitType1)
		{
		default:
			if (unitType2 == UnitType.Player || unitType2 == UnitType.Teammate || unitType2 == UnitType.TeammateNotAttack)
			{
				return true;
			}
			return false;
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
			if (unitType2 == UnitType.Monster || unitType2 == UnitType.Elite || unitType2 == UnitType.Boss || unitType2 == UnitType.WillAttack)
			{
				return true;
			}
			return false;
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			if (unitType2 == UnitType.NotAttack || unitType2 == UnitType.Brittleness)
			{
				return true;
			}
			return false;
		}
	}

	public static float3 GetShiftedDir(float angle)
	{
		float x = math.radians(angle);
		return new float3(math.sin(x), math.cos(x), 0f);
	}

	public static float3 GetShiftedDir(in float3 oldDir, float degree)
	{
		return Quaternion.Euler(0f, 0f, degree) * oldDir;
	}

	public static CollisionFilter CreateOtherCampFilter(UnitType self, bool containsBrittleness)
	{
		CollisionFilter @default = CollisionFilter.Default;
		@default.BelongsTo = 1073741824u;
		@default.CollidesWith = (IsSameCamp(self, UnitType.Player) ? 14336u : 2097664u);
		if (containsBrittleness)
		{
			@default.CollidesWith |= 163840u;
		}
		return @default;
	}

	public static float3 Float44ToFloat3(in float4x4 input)
	{
		return new float3(input.c0.x, input.c1.y, input.c2.z);
	}

	public static void GetUnitEntityInRange(in float3 point, float radius, in CollisionFilter filter, in ComponentLookup<UnitProperty_Dots> unitLookup, in PhysicsWorldSingleton physics, ref NativeList<Entity> result)
	{
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		if (!physics.OverlapSphere(point, radius, ref outHits, filter))
		{
			return;
		}
		foreach (DistanceHit item in outHits)
		{
			if (unitLookup.TryGetComponent(item.Entity, out var componentData) && componentData.CanBeTarget)
			{
				Entity value = item.Entity;
				result.Add(in value);
			}
		}
	}

	public static void GetUnitEntityOnSphereCast(in float3 start, in float3 end, in float width, in CollisionFilter filter, in PhysicsWorldSingleton physics, ref NativeList<ColliderCastHit> sphereHits)
	{
		float num = math.distance(start, end);
		if (num != 0f)
		{
			physics.SphereCastAll(start, width, math.normalize(end - start), num, ref sphereHits, filter);
		}
	}

	public static void GetEnemyEntityInRange(in float3 startPoint, float checkRadius, UnitType selfUnitType, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> cluUnitPpt, in PhysicsWorldSingleton pws, ref NativeList<Entity> result)
	{
		CollisionFilter filter = CreateOtherCampFilter(selfUnitType, containsBrittleness);
		GetUnitEntityInRange(in startPoint, checkRadius, in filter, in cluUnitPpt, in pws, ref result);
	}

	public static void GetEnemyEntityInBox(in float3 boxCenter, float3 boxSize, in CollisionFilter filter, in ComponentLookup<UnitProperty_Dots> unitLookup, in PhysicsWorldSingleton pws, ref NativeList<Entity> hits, Quaternion quaternion)
	{
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		if (!pws.OverlapBox(boxCenter, quaternion, new float3(boxSize.x, boxSize.y, math.max(boxSize.x, boxSize.y)), ref outHits, filter))
		{
			return;
		}
		foreach (DistanceHit item in outHits)
		{
			if (unitLookup.TryGetComponent(item.Entity, out var componentData) && componentData.CanBeTarget)
			{
				Entity value = item.Entity;
				hits.Add(in value);
			}
		}
	}

	public static void GetEnemyHitInSpherer(in float3 start, float3 end, float width, UnitType selfUnitType, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> cluUnitPpt, in PhysicsWorldSingleton pws, ref NativeList<ColliderCastHit> result)
	{
		NativeList<ColliderCastHit> outHits = new NativeList<ColliderCastHit>(Allocator.Temp);
		float num = math.distance(start, end);
		if (num == 0f || !pws.SphereCastAll(start, width, math.normalize(end - start), num, ref outHits, CreateOtherCampFilter(selfUnitType, containsBrittleness)))
		{
			return;
		}
		foreach (ColliderCastHit item in outHits)
		{
			ColliderCastHit value = item;
			if (cluUnitPpt.HasComponent(value.Entity))
			{
				result.Add(in value);
			}
		}
	}

	public static bool RaycastWallHit(float3 start, float3 end, out Unity.Physics.RaycastHit hitResult, in PhysicsWorldSingleton physicsWorld)
	{
		hitResult = default(Unity.Physics.RaycastHit);
		RaycastInput raycastInput = default(RaycastInput);
		raycastInput.Start = start;
		raycastInput.End = end;
		raycastInput.Filter = new CollisionFilter
		{
			BelongsTo = 16777216u,
			CollidesWith = 256u,
			GroupIndex = 0
		};
		RaycastInput input = raycastInput;
		if (physicsWorld.CastRay(input, out var closestHit))
		{
			hitResult = closestHit;
			return true;
		}
		return false;
	}

	public static int GetNearestEnemyIndex(in NativeArray<UnitProperty_Dots> unitsArray, in NativeArray<LocalTransform> unitsTransformArray, in UnitType shooterType, in float3 startPoint)
	{
		int result = -1;
		float num = float.MaxValue;
		for (int i = 0; i < unitsTransformArray.Length; i++)
		{
			UnitType unitType = unitsArray[i].unitCfg.unitType;
			if (unitType != UnitType.Brittleness && unitType != UnitType.NotAttack && !IsSameCamp(shooterType, unitType) && unitsArray[i].CanBeTarget)
			{
				float num2 = math.distancesq(unitsTransformArray[i].Position, startPoint);
				if (!(num2 > num))
				{
					num = num2;
					result = i;
				}
			}
		}
		return result;
	}

	public static Entity GetNearestTargetEtt(float3 startPoint, float checkRadius, UnitType selfUnitType, ComponentLookup<UnitProperty_Dots> cluUnitPpt, EntityStorageInfoLookup ettStorageInfoLookUp, PhysicsWorldSingleton pws)
	{
		Entity entity = Entity.Null;
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 16777216u;
		collisionFilter.CollidesWith = (IsSameCamp(selfUnitType, UnitType.Player) ? 14336u : 2097664u);
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		if (pws.OverlapSphere(startPoint, checkRadius, ref outHits, filter))
		{
			float num = 100000000f;
			for (int i = 0; i < outHits.Length; i++)
			{
				if (ettStorageInfoLookUp.Exists(outHits[i].Entity) && cluUnitPpt.HasComponent(outHits[i].Entity) && !IsSameCamp(selfUnitType, cluUnitPpt.GetRefRO(outHits[i].Entity).ValueRO.unitCfg.unitType))
				{
					if (entity == Entity.Null)
					{
						entity = outHits[i].Entity;
						num = outHits[i].Distance;
					}
					else if (outHits[i].Distance < num)
					{
						entity = outHits[i].Entity;
						num = outHits[i].Distance;
					}
				}
			}
		}
		return entity;
	}

	public static quaternion GetFallEffectRotation(in SpellMovementComponentData movement)
	{
		float3 @float = movement.Speed * movement.Direction;
		if (movement.Type == SpellSpecialMovementType.Rotation)
		{
			@float = movement.OriginalSpellHorizontalSpeed * movement.Direction;
		}
		float3 rootPosition = @float + new float3(0f, 0f, movement.CurrentFallSpeed);
		float3 layerPosition = GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
		rootPosition += layerPosition;
		return quaternion.Euler(0f, 0f, math.atan2(rootPosition.y, rootPosition.x));
	}

	public static float3 GetLayerPosition(in float3 rootPosition, LayerCorrectType type)
	{
		return type switch
		{
			LayerCorrectType.Lava0 => new float3(0f, 0f, 1.4f), 
			LayerCorrectType.Lava1 => new float3(0f, 0f, 1.39f), 
			LayerCorrectType.Lava2 => new float3(0f, 0f, 1.38f), 
			LayerCorrectType.Lava3 => new float3(0f, 0f, 1.36f), 
			LayerCorrectType.Cliff => new float3(0f, 0f, 1.37f), 
			LayerCorrectType.Tile0 => new float3(0f, 0f, 1.35f), 
			LayerCorrectType.Tile1 => new float3(0f, 0f, 1.34f), 
			LayerCorrectType.Tile2 => new float3(0f, 0f, 1.33f), 
			LayerCorrectType.Tile3 => new float3(0f, 0f, 1.32f), 
			LayerCorrectType.Tile4 => new float3(0f, 0f, 1.31f), 
			LayerCorrectType.BoundaryAO => new float3(0f, 0f, 1.3f), 
			LayerCorrectType.Tile5_AboveAO => new float3(0f, 0f, 1.29f), 
			LayerCorrectType.Tile6_AboveAO => new float3(0f, 0f, 1.28f), 
			LayerCorrectType.Tile7_AboveAO => new float3(0f, 0f, 1.27f), 
			LayerCorrectType.Tile8_AboveAO => new float3(0f, 0f, 1.26f), 
			LayerCorrectType.Tile9_AboveAO => new float3(0f, 0f, 1.25f), 
			LayerCorrectType.ExplosionTrace => new float3(0f, 0f, 1.24f), 
			LayerCorrectType.AccessOpen => new float3(0f, 0f, 1.23f), 
			LayerCorrectType.SO13 => new float3(0f, 0f, 1.22f), 
			LayerCorrectType.SO7 => new float3(0f, 0f, 1.21f), 
			LayerCorrectType.SO15 => new float3(0f, 0f, 1.2f), 
			LayerCorrectType.Corpse => new float3(0f, 0f, 1.19f), 
			LayerCorrectType.Blood => new float3(0f, 0f, 1.17f), 
			LayerCorrectType.T6Door => new float3(0f, 0f, 1.07f), 
			LayerCorrectType.Water => new float3(0f, 0f, 1.18f), 
			LayerCorrectType.Mucus => new float3(0f, 0f, 1.16f), 
			LayerCorrectType.Venom => new float3(0f, 0f, 1.15f), 
			LayerCorrectType.SO38 => new float3(0f, 0f, 1.13f), 
			LayerCorrectType.GroundEffectLow => new float3(0f, 0f, 1.12f), 
			LayerCorrectType.Elite7Trap => new float3(0f, 0f, 1.11f), 
			LayerCorrectType.SO8_Abyss => new float3(0f, 0f, 1.09f), 
			LayerCorrectType.WarningArea => new float3(0f, 0f, 1.1f), 
			LayerCorrectType.GroundEffect => new float3(0f, 0f, 1.08f), 
			LayerCorrectType.Shadow => new float3(0f, 0f, 1.05f), 
			LayerCorrectType.TreeRoot => new float3(0f, 0f, 1.04f), 
			LayerCorrectType.BoundaryLow => new float3(0f, 0f, 1.03f), 
			LayerCorrectType.SlimeOnGround => new float3(0f, 0f, 1.02f), 
			LayerCorrectType.EndlessBoundary => new float3(0f, 0f, 1f), 
			LayerCorrectType.BoundaryHigh => new float3(0f, 0f, -1f), 
			LayerCorrectType.Chapter1Leaf => GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate) + new float3(0f, 0f, -1.01f), 
			LayerCorrectType.Ghost => new float3(0f, 0f, -1.02f), 
			LayerCorrectType.RoomParticle => new float3(0f, 0f, -2.01f), 
			LayerCorrectType.Chapter3Boundary => new float3(0f, 0f, -2f), 
			LayerCorrectType.RT_Blood => new float3(0f, 0f, -105f), 
			LayerCorrectType.RT_Water => new float3(0f, 0f, -110f), 
			LayerCorrectType.RT_Mucus => new float3(0f, 0f, -120f), 
			LayerCorrectType.RT_Venom => new float3(0f, 0f, -130f), 
			LayerCorrectType.RT_Player => new float3(0f, 0f, -150f), 
			LayerCorrectType.RT_Elite7Trap => new float3(0f, 0f, -160f), 
			LayerCorrectType.RT_Boss3Stage2 => new float3(0f, 0f, -170f), 
			LayerCorrectType.Ignore => float3.zero, 
			_ => new float3(0f, 0f - rootPosition.z, (rootPosition.y + rootPosition.z) * 0.01f - rootPosition.z), 
		};
	}

	public static float3 CalculateLayerPosition(in float3 rootPosition, LayerCorrectType type)
	{
		return rootPosition + GetLayerPosition(in rootPosition, type);
	}

	public static void SetLocalTransformLayerPosition(in LocalTransform root, ref LocalTransform layer, LayerCorrectType type)
	{
		float3 point;
		if (type == LayerCorrectType.Coordinate)
		{
			point = root.Position + GetLayerPosition(in root.Position, type);
		}
		else
		{
			float3 rootPosition = root.Position.IgnoreZ();
			point = rootPosition + GetLayerPosition(in rootPosition, type);
		}
		layer.Position = root.InverseTransformPoint(point);
	}

	public static BlobAssetReference<BlobArray<T>> ListToBlobArray<T>(List<T> list) where T : unmanaged
	{
		BlobAssetReference<BlobArray<T>> blobAssetReference = default(BlobAssetReference<BlobArray<T>>);
		using BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
		BlobBuilderArray<T> blobBuilderArray = blobBuilder.Allocate(ref blobBuilder.ConstructRoot<BlobArray<T>>(), list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			blobBuilderArray[i] = list[i];
		}
		return blobBuilder.CreateBlobAssetReference<BlobArray<T>>(Allocator.Persistent);
	}

	public static BlobAssetReference<BlobArray<T>> ArrayToBlobArray<T>(T[] array) where T : unmanaged
	{
		BlobAssetReference<BlobArray<T>> blobAssetReference = default(BlobAssetReference<BlobArray<T>>);
		using BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
		BlobBuilderArray<T> blobBuilderArray = blobBuilder.Allocate(ref blobBuilder.ConstructRoot<BlobArray<T>>(), array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			blobBuilderArray[i] = array[i];
		}
		return blobBuilder.CreateBlobAssetReference<BlobArray<T>>(Allocator.Persistent);
	}

	public unsafe static float GetPhysicsColliderRadius(in PhysicsCollider pc)
	{
		Unity.Physics.SphereCollider* unsafePtr = (Unity.Physics.SphereCollider*)pc.Value.GetUnsafePtr();
		return unsafePtr->Geometry.Radius;
	}

	public static float2 DirMoveTowards(in float2 source, in float2 target, float maxDelta)
	{
		float2 x = math.normalizesafe(source);
		float2 @float = math.normalizesafe(target);
		float num = math.acos(math.clamp(math.dot(x, @float), -1f, 1f));
		float num2 = math.radians(maxDelta);
		if (num <= num2)
		{
			return @float;
		}
		float num3 = math.sign(x.x * @float.y - x.y * @float.x);
		if (num3 == 0f)
		{
			num3 = 1f;
		}
		float num4 = math.sin(num2 * num3);
		float num5 = math.cos(num2 * num3);
		float2 zero = float2.zero;
		zero.x = x.x * num5 - x.y * num4;
		zero.y = x.x * num4 + x.y * num5;
		return math.normalize(zero);
	}

	public static float3 DirMoveTowardsIgnoreZ(in float3 source, in float3 target, float maxDelta)
	{
		float2 source2 = source.xy;
		float2 target2 = target.xy;
		return new float3(DirMoveTowards(in source2, in target2, maxDelta), 0f);
	}

	public static float MoveTowards(float current, float target, float maxDelta)
	{
		if (math.abs(target - current) <= maxDelta)
		{
			return target;
		}
		return current + math.sign(target - current) * maxDelta;
	}

	public static float3 MoveTowards(in float3 current, in float3 target, float maxDistanceDelta)
	{
		float3 @float = target - current;
		float num = math.length(@float);
		if (!(num <= maxDistanceDelta) && !(num < 1E-06f))
		{
			return current + @float / num * maxDistanceDelta;
		}
		return target;
	}

	public unsafe static void ChangeCollisionFilter(Unity.Physics.Collider* collider, uint belongsToMask, BitOperator belongsToOp, uint collidesWithMask, BitOperator collidesWithOp, bool changeTrigger, bool changeCollider)
	{
		if (collider->Type == ColliderType.Compound)
		{
			for (int i = 0; i < ((CompoundCollider*)collider)->NumChildren; i++)
			{
				ChangeCollisionFilter(((CompoundCollider*)collider)->Children[i].Collider, belongsToMask, belongsToOp, collidesWithMask, collidesWithOp, changeTrigger, changeCollider);
			}
			((CompoundCollider*)collider)->RefreshCollisionFilter();
			return;
		}
		CollisionResponsePolicy collisionResponse = collider->GetCollisionResponse();
		if ((changeTrigger || collisionResponse != CollisionResponsePolicy.RaiseTriggerEvents) && (changeCollider || (collisionResponse != 0 && collisionResponse != CollisionResponsePolicy.CollideRaiseCollisionEvents)))
		{
			CollisionFilter collisionFilter = collider->GetCollisionFilter();
			switch (collidesWithOp)
			{
			case BitOperator.And:
				collisionFilter.CollidesWith &= collidesWithMask;
				break;
			case BitOperator.Or:
				collisionFilter.CollidesWith |= collidesWithMask;
				break;
			case BitOperator.Set:
				collisionFilter.CollidesWith = collidesWithMask;
				break;
			}
			switch (belongsToOp)
			{
			case BitOperator.And:
				collisionFilter.BelongsTo &= belongsToMask;
				break;
			case BitOperator.Or:
				collisionFilter.BelongsTo |= belongsToMask;
				break;
			case BitOperator.Set:
				collisionFilter.BelongsTo = belongsToMask;
				break;
			}
			collider->SetCollisionFilter(collisionFilter);
		}
	}

	public unsafe static void ChangeCollisionFilter(in PhysicsCollider collider, uint belongsToMask, BitOperator belongsToOp, uint collidesWithMask, BitOperator collidesWithOp, bool changeTrigger, bool changeCollider)
	{
		ChangeCollisionFilter(collider.ColliderPtr, belongsToMask, belongsToOp, collidesWithMask, collidesWithOp, changeTrigger, changeCollider);
	}

	public unsafe static uint GetColliderBlongsTo(Entity entity)
	{
		return World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<PhysicsCollider>(entity).ColliderPtr->GetCollisionFilter().BelongsTo;
	}

	public unsafe static void SetCollidesWith(in PhysicsCollider collider, uint collidesWith)
	{
		ChangeCollisionFilter(collider.ColliderPtr, 0u, BitOperator.Or, collidesWith, BitOperator.Set, changeTrigger: true, changeCollider: true);
	}

	public unsafe static void SetCollider(in PhysicsCollider pc, uint belongsTo, uint collideWith)
	{
		Unity.Physics.Collider* colliderPtr = pc.ColliderPtr;
		CollisionFilter collisionFilter = colliderPtr->GetCollisionFilter();
		collisionFilter.CollidesWith = collideWith;
		collisionFilter.BelongsTo = belongsTo;
		colliderPtr->SetCollisionFilter(collisionFilter);
	}

	public unsafe static void SetCollider(in PhysicsCollider pc, uint belongsTo)
	{
		uint collidesWith = GetCollidesWith(belongsTo);
		Unity.Physics.Collider* colliderPtr = pc.ColliderPtr;
		CollisionFilter collisionFilter = colliderPtr->GetCollisionFilter();
		collisionFilter.CollidesWith = collidesWith;
		collisionFilter.BelongsTo = belongsTo;
		colliderPtr->SetCollisionFilter(collisionFilter);
	}

	public static uint GetCollidesWith(uint belongsTo)
	{
		uint num = 0u;
		return belongsTo switch
		{
			uint.MaxValue => uint.MaxValue, 
			1u => 1101511425u, 
			8u => 1098921984u, 
			64u => 0u, 
			128u => 0u, 
			256u => 1102338561u, 
			512u => 1183286017u, 
			1024u => 262144u, 
			2048u => 1224969481u, 
			4096u => 1224741129u, 
			8192u => 1224736777u, 
			16384u => 256u, 
			32768u => 1099172353u, 
			65536u => 1099172353u, 
			131072u => 1099172353u, 
			262144u => 1107789569u, 
			524288u => 0u, 
			1048576u => 256u, 
			2097152u => 1073742081u, 
			4194304u => 0u, 
			8388608u => 1101249289u, 
			16777216u => 1084472073u, 
			33554432u => 262656u, 
			67108864u => 512u, 
			134217728u => 0u, 
			268435456u => 0u, 
			536870912u => 0u, 
			1073741824u => 1093122817u, 
			_ => 0u, 
		};
	}

	public static FixedString32Bytes GetSpellSEName(int spellID, in FixedString32Bytes seName)
	{
		return $"SE_Spell{spellID}{seName}";
	}

	public static float3 RotateDir(float3 oldDir, float degree)
	{
		return Quaternion.Euler(0f, 0f, degree) * oldDir;
	}

	public static float GetDegree(float3 dir)
	{
		dir = RotateDir(dir, -90f);
		float num = math.atan2(dir.y, dir.x) * 57.29578f;
		if (num < 0f)
		{
			num += 360f;
		}
		return num;
	}

	public static (float a, float b) MoveTowardsAngleCounterClockWiseReTurn2Angle(float current, float target, float maxDelta)
	{
		float num = math.abs(DeltaAngle(current, target));
		if (0f - maxDelta < num && num < maxDelta)
		{
			return (current, target);
		}
		target = current + num;
		return (current, target);
	}

	public static float DeltaAngle(float current, float target)
	{
		float num = math.abs(target - current) % 360f;
		if (num > 180f)
		{
			num = 360f - num;
		}
		return num;
	}

	public static float Lerp(float start, float end, float lerp)
	{
		return start + math.clamp(lerp, 0f, 1f) * (end - start);
	}

	public static float3 Lerp(in float3 start, in float3 end, float lerp)
	{
		return start + math.clamp(lerp, 0f, 1f) * (end - start);
	}

	public static (float a, float b) MoveTowardsAngleClockWiseReTurn2Angle(float current, float target, float maxDelta)
	{
		float num = 0f - math.abs(DeltaAngle(current, target));
		if (0f - maxDelta < num && num < maxDelta)
		{
			return (current, target);
		}
		target = current + num;
		return (current, target);
	}

	public static float GetClockwiseAngleBetweenDirection(in float3 direction1, in float3 direction2)
	{
		float3 x = IgnoreZPosition(in direction1);
		float3 x2 = IgnoreZPosition(in direction2);
		float num = math.acos(math.clamp(math.dot(math.normalize(x), math.normalize(x2)), -1f, 1f));
		if (!(x.x * x2.y - x.y * x2.x < 0f))
		{
			return MathF.PI * 2f - num;
		}
		return num;
	}

	public static float3 QuadraticBezierCurve(in float3 v0, in float3 v1, in float3 v2, float t)
	{
		return (1f - t) * (1f - t) * v0 + 2f * (1f - t) * t * v1 + t * t * v2;
	}

	public static float3 CubicBezierCurve(in float3 v0, in float3 v1, in float3 v2, in float3 v3, float t)
	{
		return (1f - t) * (1f - t) * (1f - t) * v0 + 3f * (1f - t) * (1f - t) * t * v1 + 3f * (1f - t) * t * t * v2 + t * t * t * v3;
	}

	public static float GetCurveY(in float2 p1, in float2 p2, float t)
	{
		t = math.clamp(t, 0f, 1f);
		if (t == 0f)
		{
			return 0f;
		}
		if (Mathf.Approximately(t, 1f))
		{
			return 1f;
		}
		float num = math.pow(p1.x, 3f) - p1.x;
		float num2 = p1.x * p1.x - p1.x;
		float num3 = p1.y - p1.x;
		float num4 = math.pow(p2.x, 3f) - p2.x;
		float num5 = p2.x * p2.x - p2.x;
		float num6 = p2.y - p2.x;
		float num7 = num * num5 - num4 * num2;
		float num8 = (num3 * num5 - num6 * num2) / num7;
		float num9 = (num * num6 - num4 * num3) / num7;
		float num10 = 1f - num8 - num9;
		return num8 * math.pow(t, 3f) + num9 * t * t + num10 * t;
	}

	public static float3 CubicBezierCurve_Uniform(in float3 v0, in float3 v1, in float3 v2, in float3 v3, float t, int sampleCount = 20)
	{
		NativeArray<float> nativeArray = new NativeArray<float>(sampleCount + 1, Allocator.Temp);
		nativeArray[0] = 0f;
		float num = 0f;
		float3 x = CubicBezierCurve(in v0, in v1, in v2, in v3, 0f);
		for (int i = 1; i <= sampleCount; i++)
		{
			float t2 = (float)i / (float)sampleCount;
			float3 @float = CubicBezierCurve(in v0, in v1, in v2, in v3, t2);
			float num2 = math.distance(x, @float);
			num = (nativeArray[i] = num + num2);
			x = @float;
		}
		float num4 = t * num;
		int num5 = 0;
		for (int j = 1; j <= sampleCount; j++)
		{
			if (nativeArray[j] >= num4)
			{
				num5 = j - 1;
				break;
			}
		}
		float num6 = nativeArray[num5];
		float num7 = nativeArray[num5 + 1];
		float num8 = (num4 - num6) / (num7 - num6);
		float t3 = ((float)num5 + num8) / (float)sampleCount;
		return CubicBezierCurve(in v0, in v1, in v2, in v3, t3);
	}

	public static int GetMinAngleTargetIndex(in NativeArray<UnitProperty_Dots> UnitsArray, in NativeArray<LocalTransform> UnitsTransformArray, in float3 startPos, in float3 direction, UnitType spellShooterTypes)
	{
		int result = -1;
		float num = float.MaxValue;
		quaternion q = quaternion.LookRotation(new float3(direction.x, direction.y, 0f), new float3(0f, 0f, 1f));
		for (int i = 0; i < UnitsArray.Length; i++)
		{
			UnitType unitType = UnitsArray[i].unitCfg.unitType;
			if (unitType != UnitType.NotAttack && unitType != UnitType.Brittleness && !IsSameCamp(spellShooterTypes, UnitsArray[i].unitCfg.unitType) && UnitsArray[i].CanBeTarget)
			{
				float3 input = UnitsTransformArray[i].Position - startPos;
				float3 @float = input.IgnoreZ();
				float num2 = math.angle(quaternion.LookRotation(new float3(@float.x, @float.y, 0f), new float3(0f, 0f, 1f)), q);
				if (num2 < num)
				{
					num = num2;
					result = i;
				}
			}
		}
		return result;
	}

	public static Entity GetRootEntity(in Entity childEtt, ComponentLookup<Parent> parentLookup)
	{
		Entity entity = childEtt;
		Parent componentData;
		while (parentLookup.TryGetComponent(entity, out componentData))
		{
			entity = componentData.Value;
		}
		return entity;
	}
}
