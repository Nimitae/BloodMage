using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class MultiDimensionalFloat
{
	public float[] floatArray;
}

public class GameplayLogic : MonoBehaviour {
	public KeyCode pauseKey;
	public bool gamePaused;
	public float[] skillTreeTierCosts;
	public GameObject pauseCanvas;

	private ResourceLogic resLogic;

	public Transform player;
	public Transform[] enemyPrefabs;
	public Vector2[] spawnPoints;
	public int numWaves;
	public int numEnemyTypes;
	public MultiDimensionalFloat[] waveEnemies;
	public float[] maximumEnemies;
	private float currentAliveEnemies;
	public int currentWave;
	private float enemiesSpawned;
	public float[] minimumSpawnTime;
	public float[] maximumEnemiesPerSpawn;
	private float nextPossibleSpawnTime;

	private float[] totalWaveEnemies;
	private bool[] skillOneUnlocks;
	public GameObject[] skillOnePanels;
	public Sprite[] skillOneSprites;
	public Button[] skillOneButtons;
	private bool[] skillTwoUnlocks;
	public GameObject[] skillTwoPanels;
	public Sprite[] skillTwoSprites;
	public Button[] skillTwoButtons;


	// Use this for initialization
	void Start () {
		totalWaveEnemies = new float[numWaves];
		gamePaused = false;
		currentWave = 0;
		enemiesSpawned = 0;
		nextPossibleSpawnTime = 0;
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
		initialiseSkillTree ();
		float totalEnemiesInWave;
		for (int i = 0; i < numWaves; i++) {
			totalEnemiesInWave = 0;
			for (int j = 0; j < numEnemyTypes; j++)
			{
				totalEnemiesInWave+=waveEnemies[i].floatArray[j];
			}
			totalWaveEnemies[i] = totalEnemiesInWave;
		}
	}

	void Update() {
		if (enemiesSpawned < totalWaveEnemies [currentWave] && Time.time > nextPossibleSpawnTime && currentAliveEnemies < maximumEnemies[currentWave]) {
			spawnEnemy ();
		}

		if (enemiesSpawned == totalWaveEnemies [currentWave] && currentAliveEnemies == 0 && currentWave < numWaves) {
			currentWave++;
			resLogic.increasePotionStockByWave();
			nextPossibleSpawnTime = 0;
			enemiesSpawned = 0;
		}

		if (Input.GetKeyUp(pauseKey)){
			pauseGameButtonPressed();
		}
	}

	public void spawnEnemy()
	{
		int numToSpawn = Random.Range (1, (int) maximumEnemiesPerSpawn[currentWave]);
		numToSpawn = Mathf.Min (numToSpawn, (int)(totalWaveEnemies [currentWave] - enemiesSpawned));
		for (int i = 0; i < numToSpawn; i++) {
			int spawnPoint = Random.Range (0, 4);
			int enemyType = Random.Range (0, numEnemyTypes);
			while (waveEnemies[currentWave].floatArray[enemyType]  <= 0) {
				enemyType = Random.Range (0, numEnemyTypes);
			}
			waveEnemies [currentWave].floatArray [enemyType] -= 1;
			Transform newEnemy = (Transform)Instantiate (enemyPrefabs [enemyType], spawnPoints [spawnPoint], Quaternion.identity);
			newEnemy.GetComponent<IEnemyAI> ().setPlayer (player);
			enemiesSpawned++;
			currentAliveEnemies++;
		}
		nextPossibleSpawnTime = Time.time + minimumSpawnTime[currentWave];
	}

	public void enemyDeath(){
		currentAliveEnemies--;
		QWERSpell.enemiesKilledSinceLastSpell++;
	}

	public void pauseGameButtonPressed()
	{
		if (gamePaused) {
			gamePaused = false;
			pauseCanvas.SetActive( false);
			Time.timeScale = 1;
		} else {
			gamePaused = true;
			pauseCanvas.SetActive (true);
			Time.timeScale = 0;
		}
	} 

	public void spendGoldOnSkillOneTier(int skillTier)
	{
		if (ResourceLogic.goldAmount >= skillTreeTierCosts [skillTier]) {
			resLogic.spendGoldOnSkillTree(skillTreeTierCosts [skillTier]);
			skillOneUnlocks[skillTier] = true;
			updateSkillTree(skillOneUnlocks, skillOnePanels, skillOneButtons, skillOneSprites);
			skillOnePanels[skillTier].SetActive(false);
		}
	}

	public void spendGoldOnSkillTwoTier(int skillTier)
	{
		if (ResourceLogic.goldAmount >= skillTreeTierCosts [skillTier]) {
			resLogic.spendGoldOnSkillTree(skillTreeTierCosts [skillTier]);
			skillTwoUnlocks[skillTier] = true;
			updateSkillTree(skillTwoUnlocks, skillTwoPanels, skillTwoButtons, skillTwoSprites);
			skillTwoPanels[skillTier].SetActive(false);
		}
	}

	void initialiseSkillTree()
	{
		skillOneUnlocks = new bool[7];
		skillTwoUnlocks = new bool[7];
		skillOneUnlocks [0] = true;
		skillTwoUnlocks [0] = true;
	}

	private void displaySkillTree()
	{
	//	updateSkillTree (skillOneUnlocks, skillOnePanels);
	//	updateSkillTree (skillTwoUnlocks, skillTwoPanels);
	}

	private void updateSkillTree(bool[] skillUnlocksArray, GameObject[] skillPanelsArray, Button[] buttonArray, Sprite[] spritesArray)
	{
		int[] activeButtons = new int[7];
		if (skillUnlocksArray [1]) {
			skillPanelsArray [1].transform.Find("YesButton").gameObject.SetActive(false);
			activeButtons[1] = 3;
			if (skillUnlocksArray[2]) {
				activeButtons[2] = 3;
				skillPanelsArray [2].transform.Find("YesButton").gameObject.SetActive(false);
				if (skillUnlocksArray[3]) {
					activeButtons[3] = 3;
					activeButtons[5] = 4;
					activeButtons[6] = 4;
					skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
					if (skillUnlocksArray[4]){
						activeButtons[4] = 3;
						skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
					} else {
						skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(true);
						activeButtons[4] = 1;
					}
				} else if (skillUnlocksArray[5]) {
					activeButtons[3] = 4;
					activeButtons[4] = 4;
					activeButtons[5] = 3;
					skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(false);
					if (skillUnlocksArray[6]){
						activeButtons[6] = 3;
						skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
					} else {
						skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(true);
						activeButtons[6] = 1;
					}
				}else {
					skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(true);
					activeButtons[3] = 1;
					skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(true);
					activeButtons[5] = 1;
					activeButtons[4] = 4;
					activeButtons[6] = 4;
					skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
				}
			} else {
				skillPanelsArray [2].transform.Find("YesButton").gameObject.SetActive(true);
				activeButtons[2] = 1;
				activeButtons[3] = 4;
				activeButtons[4] = 4;
				activeButtons[5] = 4;
				activeButtons[6] = 4;
				skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(false);
				skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
				skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(false);
				skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
			}
		} else {
			skillPanelsArray [1].transform.Find("YesButton").gameObject.SetActive(true);
			activeButtons[1] = 1;
			activeButtons[2] = 4;
			activeButtons[3] = 4;
			activeButtons[4] = 4;
			activeButtons[5] = 4;
			activeButtons[6] = 4;
			skillPanelsArray [2].transform.Find("YesButton").gameObject.SetActive(false);
			skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(false);
			skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
			skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(false);
			skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
		}
		for (int i = 0; i < 7; i++) {
			if (activeButtons[i]== 1 && ResourceLogic.goldAmount < skillTreeTierCosts[i]){
				skillPanelsArray [i].transform.Find("YesButton").gameObject.SetActive(false);
				activeButtons[i] = 2;
			}
		}

		for (int i = 1; i < 7; i++) {
			if (activeButtons[i] == 1){
				buttonArray[i].image.sprite = spritesArray[i];
			} else if (activeButtons[i] == 2){
				buttonArray[i].image.sprite = spritesArray[0];
			} else if (activeButtons[i] == 3){
				buttonArray[i].image.sprite = spritesArray[i];
			} else {
				buttonArray[i].image.sprite = spritesArray[7];
			}
		}
	}

	public void activateSkillOnePanel(int panelIndex)
	{
		skillOnePanels [panelIndex].SetActive (true);
	}

	public void deactivateSkillOnePanel(int panelIndex)
	{
		skillOnePanels [panelIndex].SetActive (false);
	}

	public void activateSkillTwoPanel(int panelIndex)
	{
		skillTwoPanels [panelIndex].SetActive (true);
	}
	
	public void deactivateSkillTwoPanel(int panelIndex)
	{
		skillTwoPanels [panelIndex].SetActive (false);
	}
}
