using UnityEngine;
using System.Collections;

public class SpellOneShockwave : MonoBehaviour {
	public Transform shockwave;
	public float shockwaveDamage;
	public float shockwaveSpeed;
	public float shockwaveDuration;
	
	void Start()
	{
		float xPosition = transform.position.x;
		Quaternion rotationQuart;
		if (transform.rotation.z == 1){
			rotationQuart = new Quaternion(0,1,0,0);
			xPosition -= 0.5f;
		} else {
			rotationQuart = new Quaternion(0,0,0,0);
			xPosition += 0.5f;
		}
		Vector3 shockwavePos = new Vector3(xPosition, transform.position.y,0);
		Transform newProjectile =(Transform) Instantiate(shockwave,shockwavePos,Quaternion.identity * rotationQuart);
		ShockwaveProjectile script = newProjectile.GetComponent<ShockwaveProjectile>(); 
		script.projectileDamage = shockwaveDamage;
		script.projectileSpeed = shockwaveSpeed;
		script.projectileDuration = shockwaveDuration;
		secondShockwave ();
	}

	void secondShockwave()
	{
		float xPosition = transform.position.x;
		Quaternion rotationQuart;
		if (transform.rotation.z == 1){
			rotationQuart = new Quaternion(0,0,0,0);
			xPosition += 0.1f;
		} else {
			rotationQuart = new Quaternion(0,1,0,0);
			xPosition -= 0.1f;
		}
		Vector3 shockwavePos = new Vector3(xPosition, transform.position.y,0);
		Transform newProjectile =(Transform) Instantiate(shockwave,shockwavePos,Quaternion.identity * rotationQuart);
		ShockwaveProjectile script = newProjectile.GetComponent<ShockwaveProjectile>(); 
		script.projectileDamage = shockwaveDamage;
		script.projectileSpeed = shockwaveSpeed;
		script.projectileDuration = shockwaveDuration;
	}
}
