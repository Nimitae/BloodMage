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
	public float potionBaseCost;
	public float potionExponentialDecay;
	public float potionHealing;
	public float potionIncreaseByWave;
	public Transform potionTransform;
	public Transform playerPotionAmount;
	public Transform storePotionAmount;
	public Transform exchangeRate;

	[HideInInspector]
	public float timeInvulOver;
	private CharacterControl charControlScript;
	private float invulDuration;
	private Image healthbarImage;
	private Text goldText;
	private Text potionsText;
	private Text storePotionText;
	private Text exchangeRateText;
		
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
		storePotionStock = 2;
		bloodAmount = initialBlood;
		goldAmount = initialGold;
		potionsAmount = initialPotions;
		charControlScript = player.GetComponent<CharacterControl> ();
		invulDuration = charControlScript.timeInvulInSeconds;
		timeInvulOver = 0;
		healthbarImage = healthbar.GetComponent<Image> ();
		goldText = goldValue.GetComponent<Text> ();
		potionsText = playerPotionAmount.GetComponent<Text> ();
		exchangeRateText = exchangeRate.GetComponent<Text> ();
		storePotionText = storePotionAmount.GetComponent<Text> ();
		updateHealthbar();
		updateGoldText ();
		updatePotionStore ();
	}

	void Update()
	{
		if (bloodAmount <= 0) {
			Debug.Log ("Game over, you have died!");
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
		potionsText.text = "x " + potionsAmount;
		storePotionText.text = storePotionStock + " stock!";
		exchangeRateText.text = "= " + getCurrentPotionPrice();
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
