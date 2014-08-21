using UnityEngine;
using System.Collections;

public class Stealth : MonoBehaviour
{
		public enum Stance
		{
				STANDING,
				CROUCHING,
				PRONE }
		;

		public enum Menu
		{
				PLAYING,
				PAUSED }
		;
		//private ReasonTitle reasonTitle;
		public Stance stance;
		public Menu menu;
		public bool goal = false;
		public GUIText text;
		private string startWord = "Sneak Into The Tower";
		private string midWord = "Steal The Prize";
		private string endWord = "Escape The Tower";
		private string warnWord = "You Cannot Leave Yet!";
		private string menuWord = "PAUSED";
		public static int score = 0;

		void Awake ()
		{
				if (Application.loadedLevel == 1 || Application.loadedLevel == 3) {
						goal = true;
				} else {
						goal = false;
				}
		}

		// Use this for initialization
		void Start ()
		{
				Screen.lockCursor = true;

				//reasonTitle = GameObject.FindGameObjectWithTag("Reason").GetComponent<ReasonTitle>();

				text.alignment = TextAlignment.Center;
				text.fontSize = 20;

				if (Application.loadedLevel == 1) {
						text.text = startWord;
				}
				if (Application.loadedLevel == 2) {
						text.text = midWord;
				}
				if (Application.loadedLevel == 3) {
						text.text = endWord;
				}

		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetButtonDown ("Stance")) {
						switch (stance) {
						case Stance.STANDING:
					//Debug.Log ("Standing");
								stance = Stance.CROUCHING;
								break;
						case Stance.CROUCHING:
					//Debug.Log ("Crouching");
								stance = Stance.PRONE;
								break;
						case Stance.PRONE:
					//Debug.Log ("Prone");
								stance = Stance.STANDING;
								break;
						}
				}

				if (Input.GetButtonDown ("Action")) {
						//Tags:
						//LowValue - $10
						//MediumValue - $50
						//HighValue - $100
						//Goal - $1000

						Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit, 100)) {
								//Debug.DrawLine(ray.origin, hit.point);
								//Debug.Log (hit.collider.gameObject.name);
								if (hit.collider.gameObject.tag == "LowValue") {
										score += 10;
										Destroy (hit.collider.gameObject);
										//Debug.Log (score);
								}
								if (hit.collider.gameObject.tag == "MediumValue") {
										score += 50;
										Destroy (hit.collider.gameObject);
										//Debug.Log (score);
								}
								if (hit.collider.gameObject.tag == "HighValue") {
										score += 250;
										Destroy (hit.collider.gameObject);
										//Debug.Log (score);
								}
								if (hit.collider.gameObject.tag == "Goal") {
										score += 1000;
										goal = true;
										Destroy (hit.collider.gameObject);
										//Debug.Log (score);
								}
						}
				}

		
				if (Input.GetButtonDown ("Menu")) {
						switch (menu) {
						case Menu.PLAYING:
								menu = Menu.PAUSED;
								Screen.lockCursor = false;
								text.text = menuWord;
								text.enabled = true;
								Time.timeScale = 0;
								MouseLook.inUse = false;
								break;
						case Menu.PAUSED:
								menu = Menu.PLAYING;
								Screen.lockCursor = true;
								text.enabled = false;
								Time.timeScale = 1;
								MouseLook.inUse = true;
								break;
						}
				}
		}

		void OnTriggerEnter (Collider other)
		{
			if (other.tag == "Exit") 
			{
				if (goal == true) 
					{
						if (Application.loadedLevel == 3) 
						{
							WordAlignment.view = WordAlignment.View.WIN;
							Application.LoadLevel(0);
						} 
						else 
						{				
							Application.LoadLevel (Application.loadedLevel + 1);
						}
					}
						if (goal == false) {
								text.text = warnWord;
								text.enabled = true;
						}
				}

				if (other.tag == "Guard") {
						//wordAlignment.ChangeView(1);
						//reasonTitle.ChangeView(1);
						WordAlignment.view = WordAlignment.View.LOSE;
						Application.LoadLevel (0);
				}
		}

		void OnTriggerExit (Collider other)
		{
				if (other.tag == "Exit" || other.tag == "MessageClose") {
						text.enabled = false;

						if (Application.loadedLevel == 1) {
								text.text = startWord;
						}
						if (Application.loadedLevel == 2) {
								text.text = midWord;
						}
						if (Application.loadedLevel == 3) {
								text.text = endWord;
						}
				}
		}

		void OnGUI ()
		{
				GUI.Label (new Rect (0, 0, 100, 50), "Money: $" + score);

				if (menu == Menu.PAUSED) {
						if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 50, 200, 50), "Quit")) {
								//Send player to main menu
								//wordAlignment.ChangeView(0);
								WordAlignment.view = WordAlignment.View.MAIN;
								Application.LoadLevel (0);
						}
				}
		}
}
