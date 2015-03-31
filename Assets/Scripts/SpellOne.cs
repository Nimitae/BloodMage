using UnityEngine;
using System.Collections;

public class SpellOne : MonoBehaviour {
	public KeyCode spellKey;
	public int spellLevel;
	public float[] spellCost;
	public float[] spellDamage;
	public float[] cooldown;
	public float[] spawnDistance;
	public float[] freezeDelay;
	public Transform[] spellOneTransforms;
	
	private ResourceLogic resLogic;
	private float nextAvailableTime;
	// Use this for initialization
	void Start () {
		spellLevel = 0;
		nextAvailableTime = 0;
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (spellKey) && Time.time > nextAvailableTime) {
			resLogic.spellReduceBlood(spellCost[spellLevel]);
			float xPosition = transform.position.x;
			if (transform.rotation.y == 1){
				xPosition -= spawnDistance[spellLevel];
			} else {
				xPosition += spawnDistance[spellLevel];
			}
			Vector3 hammerLocation = new Vector3(xPosition, transform.position.y + 1,0);
			Transform newProjectile =(Transform) Instantiate(spellOneTransforms[spellLevel],hammerLocation,Quaternion.identity);
			newProjectile.GetComponent<SpellOneProjectile>().projectileDamage = spellDamage[spellLevel];
			nextAvailableTime = Time.time + cooldown[spellLevel];
		}
	}

	public void setSpellLevel(int level)
	{
		spellLevel = level;
	}
} 
