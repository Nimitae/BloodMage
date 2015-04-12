using UnityEngine;
using System.Collections;

public class SpellOneBonusHit : MonoBehaviour {
	public float bonusHitChance;
	public float timeAfterFirstHit;
//	public float bonusHitDuration;
	public float spellDamage;
	public Transform bonusHitTransform;

	private int random;
	private float timeToGenerate;


	void Start()
	{
		random = Random.Range (0, 100);
		print (random);
		timeToGenerate = Time.time + timeAfterFirstHit;
	}

	void Update()
	{
		print (Time.time + "" +  timeToGenerate);
		if (random <= bonusHitChance && Time.time > timeToGenerate) {
			this.generateBonusHit();
		}
	}

	public void generateBonusHit()
	{
		print (12312312);
		float xPosition = transform.position.x;
		Quaternion rotationQuart;
		if (transform.rotation.z == 1){
			rotationQuart = new Quaternion(0,0,0,0);
		} else {
			rotationQuart = new Quaternion(0,0,1,0);
		}
		Vector3 clawLocation = new Vector3(xPosition, transform.position.y,0);
		Transform newProjectile =(Transform) Instantiate(bonusHitTransform,clawLocation,Quaternion.identity * rotationQuart);
		newProjectile.GetComponent<SpellOneProjectile>().projectileDamage = spellDamage;
	}
}
