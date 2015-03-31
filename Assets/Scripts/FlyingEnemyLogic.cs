using UnityEngine;
using System.Collections;

public class FlyingEnemyLogic : MonoBehaviour, IEnemy {

	public float health;
	public float damage;
	public float invulDuration;
	public float goldOnKill;
	
	private ResourceLogic resLogic;
	private GameplayLogic gameplayLogic;
	private float vulnerableTime;
	// Use this for initialization
	void Start () {
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
		gameplayLogic = GameObject.Find ("GameManager").GetComponent<GameplayLogic> ();
		vulnerableTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			resLogic.gainGoldFromMonsterKill(goldOnKill);
			gameplayLogic.enemyDeath();
			Destroy (gameObject);
		}
	}
	public void takeDamage(float damageReceived)
	{
		if (Time.time > vulnerableTime) {
			health -= damageReceived;
			vulnerableTime = Time.time + invulDuration;
		}
	}
	
	public void dealDamage()
	{
		resLogic.monsterDealDamage (damage);
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			dealDamage();
		}
	}
}
