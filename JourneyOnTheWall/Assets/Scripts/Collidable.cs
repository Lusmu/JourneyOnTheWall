﻿using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class Collidable : MonoBehaviour 
	{
		public float size = 1;

		public bool isStatic = false;

		public Transform tr { get; private set; }

		public Quaternion TempRotation { get; set; }

		void Awake()
		{
			tr = GetComponent<Transform>();
		}

		void OnEnable()
		{
			CollisionManager.Instance.RegisterCollider(this);
		}

		void OnDrawGizmos()
		{
			Gizmos.color = new Color(1,1,1,0.5f);
			Gizmos.DrawSphere(tr.forward * 100, size * 2);
		}
	}
}