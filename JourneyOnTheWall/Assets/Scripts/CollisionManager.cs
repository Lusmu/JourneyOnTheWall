using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JourneyOnTheWall
{
	public class CollisionManager : MonoBehaviour 
	{
		private static CollisionManager _instance;
		public static CollisionManager Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject go = new GameObject("CollisionManager");
					_instance = go.AddComponent<CollisionManager>();
				}

				return _instance;
			}
		}

		private List<Collidable> dynamicColliders;
		private List<Collidable> staticColliders;

		private const int iterations = 3;

		void Awake()
		{
			dynamicColliders = new List<Collidable>();
			staticColliders = new List<Collidable>();
		}

		public void RegisterCollider(Collidable col)
		{
			if (col.isStatic) staticColliders.Add(col);
			else dynamicColliders.Add(col);
		}

		void LateUpdate()
		{
			if (GameManager.Instance.IsGameOver) return;

			var collisions = new List<CollisionData>();

			for (int i = 0; i < dynamicColliders.Count; i++)
			{
				if (dynamicColliders[i] == null) 
				{
					dynamicColliders.RemoveAt(i);
					i--;
					continue;
				}

				dynamicColliders[i].TempRotation = dynamicColliders[i].tr.rotation;
			}

			for (int i = 0; i < iterations; i++)
			{
				for (int j = 0; j < dynamicColliders.Count; j++)
				{
					for (int k = 0; k < dynamicColliders.Count; k++)
					{
						if (j == k) continue;

						var angle = Quaternion.Angle(dynamicColliders[j].TempRotation, dynamicColliders[k].TempRotation);
						var maxAngle = dynamicColliders[j].size + dynamicColliders[k].size;

						if (angle < maxAngle)
						{
							var eulerDiff = dynamicColliders[j].TempRotation.eulerAngles - dynamicColliders[k].TempRotation.eulerAngles;

							eulerDiff = eulerDiff.normalized;

							if (Mathf.Abs(dynamicColliders[j].TempRotation.eulerAngles.y - dynamicColliders[k].TempRotation.eulerAngles.y) > 180)
							{
								eulerDiff.y *= -1;
							}

							var desiredAngle = dynamicColliders[j].TempRotation.eulerAngles + eulerDiff * maxAngle * Time.deltaTime;
							desiredAngle.z = 0;
							if (dynamicColliders[j].collidesWithBorders) desiredAngle.x = Mathf.Clamp(desiredAngle.x, 300, 355);
							var desiredAngle2 = dynamicColliders[k].TempRotation.eulerAngles - eulerDiff * maxAngle * Time.deltaTime;
							desiredAngle2.z = 0;
							if (dynamicColliders[k].collidesWithBorders) desiredAngle2.x = Mathf.Clamp(desiredAngle2.x, 300, 355);

							dynamicColliders[j].TempRotation = Quaternion.Euler(desiredAngle);
							dynamicColliders[k].TempRotation = Quaternion.Euler(desiredAngle2);

							var col = new CollisionData(dynamicColliders[j], dynamicColliders[k]);
							if (!collisions.Contains(col)) collisions.Add(col);
						}
					}

					for (int k = 0; k < staticColliders.Count; k++)
					{
						if (staticColliders[k] == null) 
						{
							staticColliders.RemoveAt(k);
							k--;
							continue;
						}

						var angle = Quaternion.Angle(dynamicColliders[j].TempRotation, staticColliders[k].tr.rotation);
						var maxAngle = dynamicColliders[j].size + staticColliders[k].size;
						
						if (angle < maxAngle)
						{
							var eulerDiff = dynamicColliders[j].TempRotation.eulerAngles - staticColliders[k].tr.eulerAngles;
							
							//eulerDiff = eulerDiff.normalized;

							if (Mathf.Abs(dynamicColliders[j].TempRotation.eulerAngles.y - staticColliders[k].tr.eulerAngles.y) > 180)
							{
								eulerDiff.y *= -1;
							}
							
							var desiredAngle = dynamicColliders[j].TempRotation.eulerAngles + eulerDiff * Time.deltaTime;
							desiredAngle.z = 0;
							if (dynamicColliders[j].collidesWithBorders) desiredAngle.x = Mathf.Clamp(desiredAngle.x, 300, 355);
							
							dynamicColliders[j].TempRotation = Quaternion.Euler(desiredAngle);

							var col = new CollisionData(dynamicColliders[j], staticColliders[k]);
							if (!collisions.Contains(col)) collisions.Add(col);
						}
					}
				}
			}

			for (int i = 0; i < dynamicColliders.Count; i++)
			{
				dynamicColliders[i].tr.rotation = dynamicColliders[i].TempRotation;
			}

			for (int i = 0; i < collisions.Count; i++)
			{
				var move1 = collisions[i].collider1.GetComponent<MoveController>();
				if (move1 != null) move1.OnCollision(collisions[i].collider2);

				var move2 = collisions[i].collider2.GetComponent<MoveController>();
				if (move2 != null) move2.OnCollision(collisions[i].collider1);

				var combat = collisions[i].collider1.GetComponent<CombatActor>();
				if (combat != null) combat.OnCollision(collisions[i].collider2);
				
				var combat2 = collisions[i].collider2.GetComponent<CombatActor>();
				if (combat2 != null) combat2.OnCollision(collisions[i].collider1);

				var clanMember = collisions[i].collider1.GetComponent<ClanMember>();
				if (clanMember != null) clanMember.OnCollision(collisions[i].collider2);
				
				var clanMember2 = collisions[i].collider2.GetComponent<ClanMember>();
				if (clanMember2 != null) clanMember2.OnCollision(collisions[i].collider1);
			}
		}

		class CollisionData
		{
			public Collidable collider1;
			public Collidable collider2;

			public CollisionData(Collidable collider1, Collidable collider2)
			{
				this.collider1 = collider1;
				this.collider2 = collider2;
			}

			public override bool Equals (object other)
			{
				var a = other as CollisionData;

				if (a == null) return false;

				return (collider1 == a.collider1 && collider2 == a.collider2) 
					|| (collider1 == a.collider2 && collider2 == a.collider1);
			}

			public override int GetHashCode ()
			{
				return base.GetHashCode ();
			}
		}

		public Collidable GetCollision(Quaternion point, float angle = 0.1f)
		{
			for (int i = 0; i < staticColliders.Count; i++)
			{
				if (staticColliders[i] == null)
				{
					staticColliders.RemoveAt(i);
					i--;
					continue;
				}
				else
				{
					if (Quaternion.Angle(point, staticColliders[i].tr.rotation) < angle) return staticColliders[i];
				}
			}

			for (int i = 0; i < dynamicColliders.Count; i++)
			{
				if (dynamicColliders[i] == null)
				{
					dynamicColliders.RemoveAt(i);
					i--;
					continue;
				}
				else
				{
					if (Quaternion.Angle(point, staticColliders[i].tr.rotation) < angle) return dynamicColliders[i];
				}
			}

			return null;
		}
	}
}