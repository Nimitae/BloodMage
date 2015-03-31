using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceLogic : MonoBehaviour {
	public static float bloodAmount;
	public static float goldAmount;

	public float initialBlood;
	public float maximumBlood;
	public float initialGold;
	public Transform player;
	public Transform healthbar;
	public Transform goldValue;

	private float timeInvulOver;
	private CharacterControl charControlScript;
	private float invulDuration;
	private Image healthbarImage;
	private Text goldText;
		
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
		print ("Spent " + goldAmount + " gold on skill tree!");
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
		bloodAmount = initialBlood;
		goldAmount = initialGold;
		charControlScript = player.GetComponent<CharacterControl> ();
		invulDuration = charControlScript.timeInvulInSeconds;
		timeInvulOver = 0;
		healthbarImage = healthbar.GetComponent<Image> ();
		goldText = goldValue.GetComponent<Text> ();
		this.updateHealthbar();
		this.updateGoldText ();
	}

	void Update()
	{
		if (bloodAmount <= 0) {
			Debug.Log ("Game over, you have died!");
			Destroy(player.gameObject);
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
}
