using UnityEngine;
using System.Collections;

public class RangedEnemyLogic : MonoBehaviour, IEnemy {
	
	public float health;
	public float damage;
	public float invulDuration;
	public float goldOnKill;
	public float potionDropChance;
	public float flinchDuration;
	
	private float flinchEndTime;
	private bool isFlinching;
	private ResourceLogic resLogic;
	private GameplayLogic gameplayLogic;
	private float vulnerableTime;
	private Animator animator;
	
	void Start()
	{
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
		gameplayLogic = GameObject.Find ("GameManager").GetComponent<GameplayLogic> ();
		animator = GetComponent<Animator> ();
		vulnerableTime = 0;
		isFlinching = false;
		flinchEndTime = 0;
	}
	
	void Update()
	{
		if (health <= 0) {
			resLogic.gainGoldFromMonsterKill(goldOnKill);
			int potionRoll = Random.Range (0, 100);
			if (potionRoll < potionDropChance){
				resLogic.dropPotion(new Vector3(transform.position.x, transform.position.y, 0));
			}
			gameplayLogic.enemyDeath();
			Destroy (gameObject);
		}
		
		if (Time.time > flinchEndTime && isFlinching) {
			animator.SetBool("monsterFlinch", false);
			isFlinching = false;
		}
	}
	
	public void takeDamage(float damageReceived)
	{
		if (Time.time > vulnerableTime) {
			health -= damageReceived;
			vulnerableTime = Time.time + invulDuration;
			animator.SetBool("monsterFlinch", true);
			isFlinching = true;
			flinchEndTime = Time.time+flinchDuration;
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
