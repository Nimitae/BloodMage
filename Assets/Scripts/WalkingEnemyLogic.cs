using UnityEngine;
using System.Collections;

public class WalkingEnemyLogic : MonoBehaviour, IEnemy {

	public float health;
	public float damage;
	public float invulDuration;
	public float goldOnKill;

	private ResourceLogic resLogic;
	private GameplayLogic gameplayLogic;
	private float vulnerableTime;

	void Start()
	{
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
		gameplayLogic = GameObject.Find ("GameManager").GetComponent<GameplayLogic> ();
		vulnerableTime = 0;
	}
	
	void Update()
	{
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

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player") {
			dealDamage();
		}
	}
}
