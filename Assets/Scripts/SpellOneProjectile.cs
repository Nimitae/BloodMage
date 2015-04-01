using UnityEngine;
using System.Collections;

public class SpellOneProjectile : MonoBehaviour {
	[HideInInspector]
	public float projectileDamage;
	[HideInInspector]
	public float projectileForce;

	private float projectileDuration;
	private float projectileDestroyTime;

	void Start()
	{
		projectileDuration = 0.5f;
		projectileDestroyTime = Time.time + projectileDuration;
	}

	void Update()
	{
		if (Time.time > projectileDestroyTime) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Enemy") {
			col.gameObject.GetComponent<IEnemy>().takeDamage(projectileDamage);
		} 
	}
}
