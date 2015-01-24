using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JourneyOnTheWall
{
	public class CollisionManager : MonoBehaviour 
	{
		public static CollisionManager Instance { get; private set; }

		private List<Collidable> dynamicColliders;
		private List<Collidable> staticColliders;

		private const int iterations = 3;

		void Awake()
		{
			Instance = this;

			dynamicColliders = new List<Collidable>();
			staticColliders = new List<Collidable>();
		}

		public void RegisterCollider(Collidable col)
		{
			if (col.isStatic) staticColliders.Add(col);
			else dynamicColliders.Add(col);
		}

		public void UnregisterCollider(Collidable col)
		{
			if (col.isStatic) staticColliders.Remove(col);
			else dynamicColliders.Remove(col);
		}

		void LateUpdate()
		{
			for (int i = 0; i < dynamicColliders.Count; i++)
			{
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

							var desiredAngle = dynamicColliders[j].TempRotation.eulerAngles + eulerDiff * maxAngle * Time.deltaTime;
							desiredAngle.z = 0;
							desiredAngle.x = Mathf.Clamp(desiredAngle.x, 300, 355);
							var desiredAngle2 = dynamicColliders[k].TempRotation.eulerAngles - eulerDiff * maxAngle * Time.deltaTime;
							desiredAngle2.z = 0;
							desiredAngle2.x = Mathf.Clamp(desiredAngle2.x, 300, 355);

							dynamicColliders[j].TempRotation = Quaternion.Euler(desiredAngle);
							dynamicColliders[k].TempRotation = Quaternion.Euler(desiredAngle2);
						}
					}

					for (int k = 0; k < staticColliders.Count; k++)
					{
						var angle = Quaternion.Angle(dynamicColliders[j].TempRotation, staticColliders[k].TempRotation);
						var maxAngle = dynamicColliders[j].size + staticColliders[k].size;
						
						if (angle < maxAngle)
						{
							var eulerDiff = dynamicColliders[j].TempRotation.eulerAngles - staticColliders[k].TempRotation.eulerAngles;
							
							eulerDiff = eulerDiff.normalized;
							
							var desiredAngle = dynamicColliders[j].TempRotation.eulerAngles + eulerDiff * maxAngle * Time.deltaTime;
							desiredAngle.z = 0;
							desiredAngle.x = Mathf.Clamp(desiredAngle.x, 300, 355);
							
							dynamicColliders[j].TempRotation = Quaternion.Euler(desiredAngle);
						}
					}
				}
			}

			for (int i = 0; i < dynamicColliders.Count; i++)
			{
				dynamicColliders[i].tr.rotation = dynamicColliders[i].TempRotation;
			}
		}
	}
}