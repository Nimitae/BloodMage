using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceLogic : MonoBehaviour {
	public static float bloodAmount;
	public static float goldAmount;
	public static float potionsAmount;
	public static float storePotionStock;

	public float initialBlood;
	public float maximumBlood;
	public float initialGold;
	public float initialPotions;
	public Transform player;
	public Transform healthbar;
	public Transform goldValue;
	public KeyCode potionHotkey;
	public float potionBaseCost;
	public float potionExponentialDecay;
	public float potionHealing;
	public float potionIncreaseByWave;
	public Transform potionTransform;
	public Transform playerPotionAmount;
	public Transform storePotionAmount;
	public Transform potionUIAmount;
	public Transform buyRate;
	public Transform sellRate;

	[HideInInspector]
	public float timeInvulOver;
	private CharacterControl charControlScript;
	private float invulDuration;
	private Image healthbarImage;
	private Text goldText;
	private Text potionsText;
	private Text storePotionText;
	private Text buyRateText;
	private Text sellRateText;
	private Text potionUIAmountText;
	private Animator charAnimator;
	private bool playerDead;
	private float timeToMoveOn;
	private Image potionDisabled;
		
	public void monsterDealDamage(float amount)
	{
		if (Time.time > timeInvulOver) {
			bloodAmount -= amount;
			timeInvulOver = Time.time + invulDuration;
			print ("Took " + amount + " damage!");
			print ("Health remaining: " + bloodAmount);
			this.updateHealthbar ();
		}
	}

	public void spellReduceBlood(float amount)
	{
		bloodAmount -= amount;
		print ("Spent " + amount + " blood on skill!");
		print ("Health remaining: " + bloodAmount);
		this.updateHealthbar ();
	}

	public void spendGoldOnSkillTree(float amount)
	{
		goldAmount -= amount;
		print ("Spent " + amount + " gold on skill tree!");
		this.updateGoldText ();
	}

	public void gainGoldFromMonsterKill(float amount)
	{
		goldAmount += amount;
		print ("Gained " + amount + " gold from killing monster!");
		this.updateGoldText ();
	}

	public void spendGoldOnBlood(float goldLost, float bloodGained)
	{
		goldAmount -= goldLost;
		bloodAmount += bloodGained;
		if (bloodAmount > maximumBlood) {
			bloodAmount = maximumBlood;
		}
		this.updateHealthbar ();
		this.updateGoldText ();
	}

	void Start()
	{
		timeToMoveOn = 999999999999;
		playerDead = false;
		storePotionStock = 2;
		bloodAmount = initialBlood;
		goldAmount = initialGold;
		potionsAmount = initialPotions;
		charControlScript = player.GetComponent<CharacterControl> ();
		charAnimator = player.GetComponent<Animator> ();
		invulDuration = charControlScript.timeInvulInSeconds;
		timeInvulOver = 0;
		healthbarImage = healthbar.GetComponent<Image> ();
		goldText = goldValue.GetComponent<Text> ();
		potionsText = playerPotionAmount.GetComponent<Text> ();
		buyRateText = buyRate.GetComponent<Text> ();
		sellRateText = sellRate.GetComponent<Text> ();
		storePotionText = storePotionAmount.GetComponent<Text> ();
		potionUIAmountText = potionUIAmount.GetComponent<Text> ();
		potionDisabled = GameObject.Find ("PotionDisabled").GetComponent<Image> ();
		updateHealthbar();
		updateGoldText ();
		updatePotionStore ();
	}

	void Update()
	{
		if (Input.GetKeyDown(potionHotkey)) {
			usePotion();
		}

		if (bloodAmount <= 0 && !playerDead) {
			//Debug.Log ("Game over, you have died!");
			charAnimator.SetBool("playerDied", true);
			timeToMoveOn = Time.time+ 1f;
			playerDead = true;
			//Application.LoadLevel(1);
		}

		if (Time.time > timeToMoveOn)
		{
			Application.LoadLevel(1);
		}
	}

	void updateHealthbar()
	{
		healthbarImage.fillAmount = bloodAmount / maximumBlood;
	}

	void updateGoldText()
	{
		goldText.text = "" + goldAmount;
	}

	public void dropPotion(Vector3 potionLocation)
	{
		Instantiate (potionTransform, potionLocation, Quaternion.identity);
	}

	public static void pickupPotion()
	{
		potionsAmount++;
		ResourceLogic resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
		resLogic.updatePotionStore ();
	}

	public void increaseStorePotionStock(float amount)
	{
		storePotionStock+= amount;
		updatePotionStore ();
	}

	public void increasePotionStockByWave()
	{
		increaseStorePotionStock (potionIncreaseByWave);
		updatePotionStore ();
	}

	public float getCurrentPotionPrice()
	{
		//return Mathf.Round(potionBaseCost * Mathf.Exp(-1 * potionExponentialDecay * storePotionStock));
		return potionBaseCost;
	}

	public void updatePotionStore()
	{
		potionsText.text = "" + potionsAmount;
		storePotionText.text = storePotionStock + "";
		buyRateText.text = "" + getCurrentPotionPrice() + " gold";
		sellRateText.text = "" + getCurrentPotionPrice() + " gold";
		potionUIAmountText.text = "" + potionsAmount;
		if (potionsAmount <= 0) {
			potionDisabled.fillAmount =1;
		} else {
			potionDisabled.fillAmount =0;
		}
	}

	public void usePotion()
	{
		if (potionsAmount > 0) {
			bloodAmount += potionHealing;
			updateHealthbar ();
			potionsAmount --;
			updatePotionStore ();
		}
	}

	public void buyPotionFromShop()
	{
		float currentPrice = getCurrentPotionPrice ();
		if (goldAmount >= currentPrice && storePotionStock > 0) {
			storePotionStock -= 1;
			potionsAmount += 1;
			goldAmount -= currentPrice;
			updatePotionStore ();
			updateGoldText();
		}
	}

	public void sellPotionToShop()
	{
		float currentPrice = getCurrentPotionPrice ();
		if (potionsAmount > 0) {
			storePotionStock += 1;
			potionsAmount -= 1;
			goldAmount += currentPrice;
			updatePotionStore ();
			updateGoldText ();
		}
	}

}
