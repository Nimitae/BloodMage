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
	public float[] spawnDistance;
	public float[] freezeDelay;
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
	public void setSpellLevel(int mode, int level)
	{
		if (mode == 1) {
			singleTapIndex = level;
		} else {
			doubleTapSpellNextAvailableTime = 0;
			doubleTapIndex = level;
		}
	}

	public void castSingleTap()
	{
		singleTapSpellNextAvailableTime = Time.time + cooldown[singleTapIndex];
		resLogic.spellReduceBlood(spellCost[singleTapIndex]);
		float xPosition = transform.position.x;
		if (transform.rotation.y == 1){
			xPosition -= spawnDistance[singleTapIndex];
		} else {
			xPosition += spawnDistance[singleTapIndex];
		}
		Vector3 hammerLocation = new Vector3(xPosition, transform.position.y + 1,0);
		Transform newProjectile =(Transform) Instantiate(spellOneTransforms[singleTapIndex],hammerLocation,Quaternion.identity);
		newProjectile.GetComponent<SpellOneProjectile>().projectileDamage = spellDamage[singleTapIndex];
		singleTapSpellNextAvailableTime = Time.time + cooldown[singleTapIndex];
	}
	
	public void castDoubleTap()
	{
		doubleTapSpellNextAvailableTime = Time.time + cooldown[doubleTapIndex];
		resLogic.spellReduceBlood(spellCost[doubleTapIndex]);
		float xPosition = transform.position.x;
		if (transform.rotation.y == 1){
			xPosition -= spawnDistance[doubleTapIndex];
		} else {
			xPosition += spawnDistance[doubleTapIndex];
		}
		Vector3 hammerLocation = new Vector3(xPosition, transform.position.y + 1,0);
		Transform newProjectile =(Transform) Instantiate(spellOneTransforms[doubleTapIndex],hammerLocation,Quaternion.identity);
		newProjectile.GetComponent<SpellOneProjectile>().projectileDamage = spellDamage[doubleTapIndex];
		doubleTapSpellNextAvailableTime = Time.time + cooldown[doubleTapIndex];
	}
} 
