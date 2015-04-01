using UnityEngine;
using UnityEngine.UI;
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


	private bool[] skillOneUnlocks;
	public GameObject[] skillOnePanels;
	private bool[] skillTwoUnlocks;
	public GameObject[] skillTwoPanels;


	// Use this for initialization
	void Start () {
		gamePaused = false;
		skillTreeOpen = false;
		shopOpen = false;
		currentWave = 0;
		enemiesSpawned = 0;
		nextPossibleSpawnTime = 0;
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
		initialiseSkillTree ();
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
			displaySkillTree();
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

	public void spendGoldOnSkillOneTier(int skillTier)
	{
		if (ResourceLogic.goldAmount >= skillTreeTierCosts [skillTier]) {
			resLogic.spendGoldOnSkillTree(skillTreeTierCosts [skillTier]);
			skillOneUnlocks[skillTier] = true;
			updateSkillTree(skillOneUnlocks, skillOnePanels);
			skillOnePanels[skillTier].SetActive(false);
		}
	}

	public void spendGoldOnSkillTwoTier(int skillTier)
	{
		if (ResourceLogic.goldAmount >= skillTreeTierCosts [skillTier]) {
			resLogic.spendGoldOnSkillTree(skillTreeTierCosts [skillTier]);
			skillTwoUnlocks[skillTier] = true;
			updateSkillTree(skillTwoUnlocks, skillTwoPanels);
			skillTwoPanels[skillTier].SetActive(false);
		}
	}

	public void purchasePotion()
	{
		if (ResourceLogic.goldAmount >= potionCost) {
			resLogic.spendGoldOnBlood(potionCost, potionHealing);
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
		updateSkillTree (skillOneUnlocks, skillOnePanels);
	}

	private void updateSkillTree(bool[] skillUnlocksArray, GameObject[] skillPanelsArray)
	{
		bool[] activeButtons = new bool[7];
		if (skillUnlocksArray [1]) {
			skillPanelsArray [1].transform.Find("YesButton").gameObject.SetActive(false);
			if (skillUnlocksArray[2]) {
				skillPanelsArray [2].transform.Find("YesButton").gameObject.SetActive(false);
				if (skillUnlocksArray[3]) {
					skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
					if (skillUnlocksArray[4]){
						skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
					} else {
						skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(true);
						activeButtons[4] = true;
					}
				} else if (skillUnlocksArray[5]) {
					skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(false);
					if (skillUnlocksArray[6]){
						skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
					} else {
						skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(true);
						activeButtons[6] = true;
					}
				}else {
					skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(true);
					activeButtons[3] = true;
					skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(true);
					activeButtons[5] = true;
					skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
					skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
				}
			} else {
				skillPanelsArray [2].transform.Find("YesButton").gameObject.SetActive(true);
				activeButtons[2] = true;
				skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(false);
				skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
				skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(false);
				skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
			}
		} else {
			skillPanelsArray [1].transform.Find("YesButton").gameObject.SetActive(true);
			activeButtons[1] = true;
			skillPanelsArray [2].transform.Find("YesButton").gameObject.SetActive(false);
			skillPanelsArray [3].transform.Find("YesButton").gameObject.SetActive(false);
			skillPanelsArray [4].transform.Find("YesButton").gameObject.SetActive(false);
			skillPanelsArray [5].transform.Find("YesButton").gameObject.SetActive(false);
			skillPanelsArray [6].transform.Find("YesButton").gameObject.SetActive(false);
		}
		for (int i = 0; i < 7; i++) {
			if (activeButtons[i] && ResourceLogic.goldAmount < skillTreeTierCosts[i]){
				skillPanelsArray [i].transform.Find("YesButton").gameObject.SetActive(false);
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
