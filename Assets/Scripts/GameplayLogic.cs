using UnityEngine;
using System.Collections;

public class GameplayLogic : MonoBehaviour {

	public bool gamePaused;
	public bool skillTreeOpen;
	public float[] skillTreeTierCosts;
	public GameObject skillTree;
	public bool shopOpen;
	public GameObject shopObject;
	public float potionCost;
	public float potionHealing;
	private ResourceLogic resLogic;

	public Transform player;
	public Transform[] enemyPrefabs;
	public Vector2[] spawnPoints;
	public float[] waveEnemies;
	public float[] maximumEnemies;
	private float currentAliveEnemies;
	public int currentWave;
	private float enemiesSpawned;
	public float minimumSpawnTime;

	private float nextPossibleSpawnTime;

	// Use this for initialization
	void Start () {
		gamePaused = false;
		skillTreeOpen = false;
		shopOpen = false;
		currentWave = 0;
		enemiesSpawned = 0;
		nextPossibleSpawnTime = 0;
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
	}

	void Update() {
		if (enemiesSpawned < waveEnemies [currentWave] && Time.time > nextPossibleSpawnTime && currentAliveEnemies < maximumEnemies[currentWave]) {
			this.spawnEnemy ();
		}

		if (enemiesSpawned == waveEnemies [currentWave] && currentAliveEnemies == 0 && currentWave < waveEnemies.Length - 1) {
			currentWave++;
			nextPossibleSpawnTime = 0;
			enemiesSpawned = 0;
		}
	}

	public void spawnEnemy()
	{
		int spawnPoint = Random.Range (0, 4);
		int enemyType = Random.Range (0, 2);
		Transform newEnemy = (Transform) Instantiate (enemyPrefabs[enemyType], spawnPoints [spawnPoint], Quaternion.identity);
		newEnemy.GetComponent<IEnemyAI> ().setPlayer (player);
		enemiesSpawned++;
		currentAliveEnemies++;
		nextPossibleSpawnTime = Time.time + minimumSpawnTime;
	}

	public void enemyDeath(){
		currentAliveEnemies--;
	}

	public void togglePauseGame()
	{
		if (gamePaused) {
			gamePaused = false;
			Time.timeScale = 1;
		} else {
			gamePaused = true;
			Time.timeScale = 0;
		}
	} 

	public void pauseGame()
	{
		gamePaused = true;
		Time.timeScale = 0;
	}
	public void unpauseGame()
	{
		if (!shopOpen && !skillTreeOpen) {
			gamePaused = false;
			Time.timeScale = 1;
		}
	}

	public void skillTreeButtonPressed()
	{
		if (skillTreeOpen) {
			skillTree.SetActive(false);
			skillTreeOpen = false;
			unpauseGame();
		} else {
			skillTree.SetActive(true);
			skillTreeOpen = true;
			pauseGame();
		}
	}

	public void shopButtonPressed()
	{
		if (shopOpen) {
			shopObject.SetActive(false);
			shopOpen = false;
			unpauseGame();
		} else {
			shopObject.SetActive(true);
			shopOpen = true;
			pauseGame();
		}
	}

	public void spendGoldOnTier(int skillTier)
	{
		if (ResourceLogic.goldAmount >= skillTreeTierCosts [skillTier]) {
			resLogic.spendGoldOnSkillTree(skillTreeTierCosts [skillTier]);
		}
	}

	public void purchasePotion()
	{
		if (ResourceLogic.goldAmount >= potionCost) {
			resLogic.spendGoldOnBlood(potionCost, potionHealing);
		}
	}
}
