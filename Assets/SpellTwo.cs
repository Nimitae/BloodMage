using UnityEngine;
using System.Collections;

public class SpellTwo : MonoBehaviour {
	public KeyCode spellKey;
	public float doubleTapTimeThreshold;
	public int singleTapIndex;
	public int doubleTapIndex;
	public float[] spellCost;
	public float[] spellDamagePerBullet;
	public float[] cooldown;
	public float[] projectileSpeed;
	public float[] projectileForce;
	public float[] penetrationLimit;
	public Transform[] spellTwoTransform;
	
	private bool spellCasted;
	private float timeOfLastTap;
	private ResourceLogic resLogic;
	private float singleTapSpellNextAvailableTime;
	private float doubleTapSpellNextAvailableTime;
	// Use this for initialization
	void Start () {
		singleTapIndex = 0;
		singleTapSpellNextAvailableTime = 0;
		doubleTapIndex = 0;
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
	{print ("single");
		castSpellLevel (singleTapIndex);
		singleTapSpellNextAvailableTime = Time.time + cooldown [singleTapIndex];
	}
	
	public void castDoubleTap()
	{
		print ("double");
		castSpellLevel (doubleTapIndex);
		doubleTapSpellNextAvailableTime = Time.time + cooldown [doubleTapIndex];
	}

	public void castSpellLevel(int index)
	{
		if (index <= 3) {
			resLogic.spellReduceBlood(spellCost[index]);
			float xPosition = transform.position.x;
			Quaternion rotationQuart;
			if (transform.rotation.y == 1){
				rotationQuart = new Quaternion(0,0,1,0);
				xPosition -= 0.2f;
			} else {
				rotationQuart = new Quaternion(0,0,0,0);
				xPosition += 0.2f;
			}
			Vector3 projectilePosition = new Vector3(xPosition, transform.position.y,0);
			Transform newProjectile =(Transform) Instantiate(spellTwoTransform[index],projectilePosition,Quaternion.identity * rotationQuart);
			SpellTwoProjectile script = newProjectile.GetComponent<SpellTwoProjectile>(); 
			script.projectileDamage = spellDamagePerBullet[index];
			script.projectileSpeed = projectileSpeed[index];
			script.penetrationLimit = penetrationLimit[index];
			script.projectileForce = projectileForce[index];
		}
	}
} 
