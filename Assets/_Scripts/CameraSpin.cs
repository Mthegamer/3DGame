using UnityEngine;
using System.Collections;

public class CameraSpin : MonoBehaviour {

	public Transform target;
	public GameObject model;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		model = GameObject.Find ("golem");
		target = model.transform.Find("CenterFocus");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(target);
		transform.Translate(Vector3.right * Time.deltaTime);
	}
}
