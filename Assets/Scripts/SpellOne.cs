using UnityEngine;
using System.Collections;

public class SpellOne : MonoBehaviour {
	public KeyCode spellKey;
	public float doubleTapTimeThreshold;
	public int singleTapIndex;
	public int doubleTapIndex;
	public float[] spellCost;
	public float[] spellDamage;
	public float[] cooldown;
	public Transform[] spellOneTransforms;

	private bool spellCasted;
	private float timeOfLastTap;
	private ResourceLogic resLogic;
	private float singleTapSpellNextAvailableTime;
	private float doubleTapSpellNextAvailableTime;
	// Use this for initialization
	void Start () {
		singleTapIndex = 0;
		singleTapSpellNextAvailableTime = 0;
		doubleTapIndex = 1;
		doubleTapSpellNextAvailableTime = 999999999;
		spellCasted = true;
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (spellKey)) {
			if ((Time.time - doubleTapTimeThreshold) > timeOfLastTap) {
				timeOfLastTap = Time.time;
				spellCasted = false;
			} else if ((Time.time - doubleTapTimeThreshold) < timeOfLastTap) {
				if (Time.time > doubleTapSpellNextAvailableTime){
					castDoubleTap();
					timeOfLastTap = 0;
					spellCasted = true;
				}
			}
		}
		if ((Time.time - doubleTapTimeThreshold) > timeOfLastTap && !spellCasted) {
			if (Time.time > singleTapSpellNextAvailableTime) {
				castSingleTap();
				timeOfLastTap = 0;

			}
			spellCasted = true;
		}
	}

	public void setSingleTapIndex(int level)
	{
		singleTapIndex = level;
	}

	public void setDoubleTapIndex(int level)
	{
		doubleTapSpellNextAvailableTime = 0;
		doubleTapIndex = level;
	}

	public void castSingleTap()
	{
		resLogic.spellReduceBlood(spellCost[singleTapIndex]);
		float xPosition = transform.position.x;
		Quaternion rotationQuart;
		if (transform.rotation.y == 1){
			rotationQuart = new Quaternion(0,0,1,0);
			xPosition -= 0.7f;
		} else {
			rotationQuart = new Quaternion(0,0,0,0);
			xPosition += 0.7f;
		}
		Vector3 clawLocation = new Vector3(xPosition, transform.position.y,0);
		Transform newProjectile =(Transform) Instantiate(spellOneTransforms[singleTapIndex],clawLocation,Quaternion.identity * rotationQuart);
		newProjectile.GetComponent<SpellOneProjectile>().projectileDamage = spellDamage[singleTapIndex];
		singleTapSpellNextAvailableTime = Time.time + cooldown[singleTapIndex];
	}
	
	public void castDoubleTap()
	{
		resLogic.spellReduceBlood(spellCost[doubleTapIndex]);
		float xPosition = transform.position.x;
		Quaternion rotationQuart;
		if (transform.rotation.y == 1){
			rotationQuart = new Quaternion(0,0,1,0);
			xPosition -= 0.7f;
		} else {
			rotationQuart = new Quaternion(0,0,0,0);
			xPosition += 0.7f;
		}
		Vector3 clawLocation = new Vector3(xPosition, transform.position.y,0);
		Transform newProjectile =(Transform) Instantiate(spellOneTransforms[doubleTapIndex],clawLocation,Quaternion.identity * rotationQuart);
		newProjectile.GetComponent<SpellOneProjectile>().projectileDamage = spellDamage[doubleTapIndex];
		doubleTapSpellNextAvailableTime = Time.time + cooldown[doubleTapIndex];
	}
} 
