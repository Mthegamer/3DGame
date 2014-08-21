using UnityEngine;
using System.Collections;

public class GuardAnim : MonoBehaviour {

	//private GameObject golem;
	public bool moving = false;
	//public EnemyAI enemy = transform.parent.GetComponent<EnemyAI>();
	public EnemyAI enemyAI;
	//public Vector3 lastPos = new Vector3();
	//public Vector3 currentPos = new Vector3();

	void Awake()
	{
		enemyAI = transform.parent.GetComponent<EnemyAI>();
	}

	// Use this for initialization
	void Start () 
	{
		//golem = GameObject.Find("golem");
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		CheckMoving();
		lastPos = transform.position;
		Wait();
		currentPos = transform.position;
		*/
		StartCoroutine(CheckMoving());

		//if(rigidbody.velocity.magnitude < 0)
		if (moving == true)
		{
			//Debug.Log("Walking");
			animation.Blend("walk");
		}
		if (enemyAI.chasing == true)
		{
			animation.Blend("run");
		}
		else
		{
			//Debug.Log("Idling");
			animation.Blend("idle");
		}
	}

	private IEnumerator CheckMoving()
	{
		//Debug.Log("Checking Movement");
		Vector3 startPos = transform.parent.position;
		yield return new WaitForSeconds(1f);
		Vector3 finalPos = transform.parent.position;

		if( startPos.x != finalPos.x || startPos.z != finalPos.z)
		{
			moving = true;
		}
		else
		{
			moving = false;
		}
	}
}
