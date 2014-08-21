using UnityEngine;
using System.Collections;

public class ReasonTitle : MonoBehaviour {

	public int check = 0;

	void Start () 
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void ChangeView(int reason)
	{
		check = reason;
	}
}
