using UnityEngine;
using System.Collections;

public class PotionScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Player") {
			ResourceLogic.pickupPotion();
			Destroy (gameObject);
		} 
	}
}
