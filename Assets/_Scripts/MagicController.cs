using UnityEngine;
using System.Collections;
using System;

public enum MagicSpells
{
		NONE,
		DAZE,
		SNEAK,
		MISREPORT,
		VISION,
		SPEED
}

public class MagicController : MonoBehaviour
{
	
		public MagicSpells activeSpell;
		private int activeSpellDuration;
		private RaycastHit raycast;
		private Boolean targetSelf;
		public Camera visionCamera;
		public int spellDuration = 5;
		public Boolean allowSpell = true;
		public GameObject spellText;
	
		public static event Action<MagicSpells> castSpell;
		// Use this for initialization
		void Start ()
		{
				activeSpell = MagicSpells.NONE;
				targetSelf = false;
				visionCamera.enabled = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				// Set an active spell
				if (Input.GetKey (KeyCode.Alpha1)) {
						activeSpell = MagicSpells.DAZE;
						spellText.GetComponent<TextMesh> ().text = "Slow";
						targetSelf = false;
				} else if (Input.GetKey (KeyCode.Alpha2)) {
						activeSpell = MagicSpells.MISREPORT;
						spellText.GetComponent<TextMesh> ().text = "False Report";
						targetSelf = false;
				} else if (Input.GetKey (KeyCode.Alpha3)) {
						activeSpell = MagicSpells.SNEAK;
						spellText.GetComponent<TextMesh> ().text = "Sneak";
						targetSelf = false;
				} else if (Input.GetKey (KeyCode.Alpha4)) {
						//Debug.Log ("aaa");
						activeSpell = MagicSpells.VISION;
						spellText.GetComponent<TextMesh> ().text = "Vision";
						targetSelf = true;
				} else if (Input.GetKey (KeyCode.Alpha5)) {
						activeSpell = MagicSpells.SPEED;
						targetSelf = true;
						// Cast the active spell
				} else if (Input.GetMouseButton (0) && !targetSelf) {
						Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
						if (Physics.Raycast (ray, out raycast)) {
								if (!targetSelf && raycast.transform.tag == "guardParent" && allowSpell) { // All objects have a transform, prevents errors.
										//Debug.Log ("Test2 hit guard");
										allowSpell = false;
//										raycast.transform.GetComponent<enemysight> ().receiveSpell (activeSpell);
										castSpell (activeSpell);
								}
						} 
				} else if (Input.GetMouseButton (1) && targetSelf && allowSpell) {
						//Debug.Log ("a");
						allowSpell = false;
						StartCoroutine (spellTimer ());
				}
		}

		public IEnumerator spellTimer ()
		{
				if (activeSpell == MagicSpells.VISION) {
						visionCamera.enabled = true;
						//Debug.Log ("b");
				}
				yield return new WaitForSeconds (spellDuration);
				if (activeSpell == MagicSpells.VISION) {
						visionCamera.enabled = false;
				}
				allowSpell = true;
		}
}