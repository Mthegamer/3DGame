using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

		public float patrolSpeed = 2f;
		public float defaultPatrolSpeed;
		public float chaseSpeed = 5f;
		public float defaultChaseSpeed;
		public float chaseWaitTime = 5f;
		public float patrolWaitTime = 1f;
		public Transform[] patrolWayPoints;
		public bool chasing = false;
		private enemysight enemySight;
		private NavMeshAgent nav;
		private Transform player;
		private lastPlayerSighting lastPlayerSighting;
		public float chaseTimer;
		private float patrolTimer;
		private int wayPointIndex;
		public float remaingdistance;
		public float stopdistance;
	
		void Awake ()
		{
				defaultPatrolSpeed = patrolSpeed;
				defaultChaseSpeed = chaseSpeed;
				enemySight = GetComponent<enemysight> ();
				nav = GetComponent<NavMeshAgent> ();
		 
				lastPlayerSighting = GameObject.FindGameObjectWithTag ("GameController").GetComponent<lastPlayerSighting> ();
		}
	
		void Update ()
		{
		 
				if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition /*&& playerHealth.health > 0f*/)
			 
						Chasing ();
				else
			 
						Patrolling ();
		}
	
		void Chasing ()
		{
				chasing = true;

				remaingdistance = nav.remainingDistance;
				stopdistance = 3f;
		 
				Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
		
		 
				if (sightingDeltaPos.sqrMagnitude > 4f)
			 
						nav.destination = enemySight.personalLastSighting;
		
		 
				nav.speed = chaseSpeed;
		
		 
				if (remaingdistance < stopdistance) {
			 
						chaseTimer += Time.deltaTime;
			
			 
						if (chaseTimer >= chaseWaitTime) {

								lastPlayerSighting.position = lastPlayerSighting.resetPosition;
								enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
								chaseTimer = 0f;
						}
				} else
			 
						chaseTimer = 0f;
		}
	
		void Patrolling ()
		{
				chasing = false;

		 
				nav.speed = patrolSpeed;
				remaingdistance = nav.remainingDistance;
				stopdistance = nav.stoppingDistance;
		
		 
				if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance <= nav.stoppingDistance) {
			 
						patrolTimer += Time.deltaTime;
			
			 
						if (patrolTimer >= patrolWaitTime) {
				 
								if (wayPointIndex == patrolWayPoints.Length - 1)
										wayPointIndex = 0;
								else
										wayPointIndex++;
				
				 
								patrolTimer = 0;
						}
				} else
			 
						patrolTimer = 0;
		
		 
				nav.destination = patrolWayPoints [wayPointIndex].position;
		}
}
