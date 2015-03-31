using UnityEngine;
using System.Collections;

public class QWERSpell : MonoBehaviour {
	public KeyCode firstSpell;
	public KeyCode secondSpell;
	public KeyCode thirdSpell;
	public KeyCode fourthSpell;
	public float sharedSpellCooldown;

	private float nextAvailableSpellTime;

	// Use this for initialization
	void Start () {
		nextAvailableSpellTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= nextAvailableSpellTime) {
			if (Input.GetKey(firstSpell)){

			} else if (Input.GetKey(secondSpell)){

			} else if (Input.GetKey(thirdSpell)){
				
			} else if (Input.GetKey(fourthSpell)){
				
			}
		}
	}
}
