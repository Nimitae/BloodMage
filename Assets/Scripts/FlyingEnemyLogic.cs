using UnityEngine;
using System.Collections;

public class FlyingEnemyLogic : MonoBehaviour, IEnemy {

	public float health;
	public float damage;
	public float invulDuration;
	public float goldOnKill;
	public float potionDropChance;
	public Sprite[] enemySprites;

	private int currentSprite;
	private SpriteRenderer spriteRend;
	private ResourceLogic resLogic;
	private GameplayLogic gameplayLogic;
	private float vulnerableTime;
	// Use this for initialization
	void Start () {
		currentSprite = 0;
		spriteRend = transform.GetComponent<SpriteRenderer> ();
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
		gameplayLogic = GameObject.Find ("GameManager").GetComponent<GameplayLogic> ();
		vulnerableTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			resLogic.gainGoldFromMonsterKill(goldOnKill);
			int potionRoll = Random.Range (0, 100);
			if (potionRoll < potionDropChance){
				resLogic.dropPotion(new Vector3(transform.position.x, transform.position.y, 0));
			}
			gameplayLogic.enemyDeath();
			Destroy (gameObject);
		}

		if (Time.time > vulnerableTime && currentSprite == 1) {
			spriteRend.sprite= enemySprites[0];
			currentSprite = 0;
		}
	}
	public void takeDamage(float damageReceived)
	{
		if (Time.time > vulnerableTime) {
			health -= damageReceived;
			spriteRend.sprite = enemySprites[1];
			currentSprite = 1;
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
