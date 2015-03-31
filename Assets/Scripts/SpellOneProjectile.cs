using UnityEngine;
using System.Collections;

public class SpellOneProjectile : MonoBehaviour {
	[HideInInspector]
	public float projectileDamage;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Enemy") {
			col.gameObject.GetComponent<IEnemy>().takeDamage(projectileDamage);
		} else if (col.transform.tag == "Ground") {
			Destroy (gameObject);
		}
	}
}
