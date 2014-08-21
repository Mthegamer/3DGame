using UnityEngine;
using System.Collections;

public class WordAlignment : MonoBehaviour 
{
	public enum View { MAIN, LOSE, WIN };
	public GUIText text;
	//private ReasonTitle reasonTitle; 
	//GameObject gamething;
	public GUIText winText;
	public static View view;
	public GameObject model;
	private bool played = false;
	public static int lastLevel = 0;

		// Use this for initialization
	void Start () 
	{
		Screen.lockCursor = false;
		text.pixelOffset = new Vector2(0, Screen.height/2 - 100);
		winText.pixelOffset = new Vector2(0, Screen.height/2 - 200);
		text.fontSize = 80;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void ChangeView(int check)
	{
		if (check == 0)
		{
			view = View.MAIN;
		}
		if (check == 1)
		{
			view = View.LOSE;
		}
		if (check == 2)
		{
			view = View.WIN;
		}
	}

	void OnGUI()
	{
		switch(view)
		{
			case View.MAIN:
				text.text = "Mystic Thievery";
				winText.text = "";

				model.animation.Play("idle");
				
				if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 50, 200, 50), "Play")) 
				{
					Application.LoadLevel(1);
					played = false;
				}
				if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 120, 200, 50), "Exit")) 
				{
					Application.Quit();
					//view = View.WIN;
				}
				break;
			case View.LOSE:
				text.text = "Failure!";
				winText.text = "";
				
				if (played == false)
				{
					model.animation.Play("death");
					played = true;
				}

				if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 50, 200, 50), "Retry")) 
				{
					Application.LoadLevel(lastLevel);
					played = false;
				}
				if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 120, 200, 50), "Quit")) 
				{
					view = View.MAIN;
					played = false;
				}
				break;
			case View.WIN:
				text.text = "Success!";
				winText.text = "Final Tally\nMoney: $" + Stealth.score.ToString();
				
				model.animation.Play("jump");

				if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 50, 200, 50), "Quit")) 
				{
					view = View.MAIN;
					played = false;
				}
				break;
		}
	}
}
