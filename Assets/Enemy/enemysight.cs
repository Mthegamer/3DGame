using UnityEngine;
using System.Collections;

public class enemysight : MonoBehaviour
{

		public float fieldOfViewAngle = 360f;
		public bool playerInSight;
		public Vector3 personalLastSighting;
		private NavMeshAgent nav;
		private SphereCollider col;
		private lastPlayerSighting lastPlayerSighting;
		private MagicController magicController;
		public GameObject player;
		public Vector3 previousSighting;
		public MagicSpells spellEffect = MagicSpells.NONE;
		public float spellDuration = 5f;
		private float defaultSphereColliderRadius;
		private float defaultColliderDrag;
		public GameObject randomWaypoint1;
		public GameObject randomWaypoint2;
		public GameObject randomWaypoint3;
		public GameObject randomWaypoint4;
		public GameObject randomWaypoint5;
		public static Vector3[] randomWaypoints = new Vector3[5];

		void Awake ()
		{
				nav = GetComponent<NavMeshAgent> ();
				col = GetComponent<SphereCollider> ();

				lastPlayerSighting = GameObject.FindGameObjectWithTag ("GameController").GetComponent<lastPlayerSighting> ();
				magicController = GameObject.FindGameObjectWithTag ("Player").GetComponent<MagicController> ();
				player = GameObject.FindGameObjectWithTag ("Player");
		
		 
				personalLastSighting = lastPlayerSighting.resetPosition;
				previousSighting = lastPlayerSighting.resetPosition;

				MagicController.castSpell += receiveSpell;
				defaultSphereColliderRadius = GetComponent<SphereCollider> ().radius;

				randomWaypoints [0] = randomWaypoint1.transform.position;
				randomWaypoints [1] = randomWaypoint1.transform.position;
				randomWaypoints [2] = randomWaypoint1.transform.position;
				randomWaypoints [3] = randomWaypoint1.transform.position;
				randomWaypoints [4] = randomWaypoint1.transform.position;
		}

		void Start ()
		{
		}
	
		void Update ()
		{
				if (lastPlayerSighting.position != previousSighting)
						personalLastSighting = lastPlayerSighting.position;
				previousSighting = lastPlayerSighting.position;
		}
	
		void OnTriggerStay (Collider other)
		{
				Vector3 direction = other.transform.position - transform.position;
				Vector3 forward = new Vector3 (0f, 0f, direction.z > 0 ? 0.3f : -0.3f);
				if (other.gameObject.tag == "Player") {
						playerInSight = false;
						RaycastHit hit;

						if (Physics.Raycast (transform.position + (transform.up - new Vector3 (0f, .5f, 0f)) + forward, direction.normalized, out hit, col.radius * 2)) {
								if (hit.collider.gameObject.tag == "Player") {
										playerInSight = true;
										lastPlayerSighting.position = player.transform.position;
								}
						}
				}
		}
	
		void OnTriggerExit (Collider other)
		{
				if (other.gameObject == player)
						playerInSight = false;
		}
	
		float CalculatePathLength (Vector3 targetPosition)
		{
				// Create a path and set it based on a target position.
				NavMeshPath path = new NavMeshPath ();
				if (nav.enabled)
						nav.CalculatePath (targetPosition, path);
		
				// Create an array of points which is the length of the number of corners in the path + 2.
				Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
		
				// The first point is the enemy's position.
				allWayPoints [0] = transform.position;
		
				// The last point is the target position.
				allWayPoints [allWayPoints.Length - 1] = targetPosition;
		
				// The points inbetween are the corners of the path.
				for (int i = 0; i < path.corners.Length; i++) {
						allWayPoints [i + 1] = path.corners [i];
				}
		
				// Create a float to store the path length that is by default 0.
				float pathLength = 0;
		
				// Increment the path length by an amount equal to the distance between each waypoint and the next.
				for (int i = 0; i < allWayPoints.Length - 1; i++) {
						pathLength += Vector3.Distance (allWayPoints [i], allWayPoints [i + 1]);
				}
		
				return pathLength;
		}

		public void receiveSpell (MagicSpells spell)
		{
				spellEffect = spell;
				StartCoroutine (spellTimer ());
		}

		public IEnumerator spellTimer ()
		{
				Debug.Log ("3");
				if (spellEffect == MagicSpells.SNEAK) {
						gameObject.GetComponent<SphereCollider> ().radius = .5f;
				} else if (spellEffect == MagicSpells.DAZE) {
						gameObject.GetComponent<EnemyAI> ().patrolSpeed = 0.1f;
						gameObject.GetComponent<EnemyAI> ().chaseSpeed = 1f;
				} else if (spellEffect == MagicSpells.MISREPORT) {
						lastPlayerSighting.position = randomWaypoints [UnityEngine.Random.Range (0, 4)];
				}
				Debug.Log ("4");
				yield return new WaitForSeconds (spellDuration);
				Debug.Log ("5");
				if (spellEffect == MagicSpells.SNEAK) {
						gameObject.GetComponent<SphereCollider> ().radius = defaultSphereColliderRadius;
				} else if (spellEffect == MagicSpells.DAZE) {
						gameObject.GetComponent<EnemyAI> ().patrolSpeed = gameObject.GetComponent<EnemyAI> ().defaultPatrolSpeed;
						gameObject.GetComponent<EnemyAI> ().chaseSpeed = gameObject.GetComponent<EnemyAI> ().defaultChaseSpeed;
				}
				Debug.Log ("6");
				spellEffect = MagicSpells.NONE;
				magicController.allowSpell = true;
		}
}

