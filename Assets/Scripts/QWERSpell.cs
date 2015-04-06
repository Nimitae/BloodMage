using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QWERSpell : MonoBehaviour {
	public KeyCode firstSpell;
	public KeyCode secondSpell;
	public KeyCode thirdSpell;
	public KeyCode fourthSpell;
	public float sharedSpellCooldown;
	public float firstFlatBloodGain;
	public float secondInvulDuration;
	public float fourthSpeedIncrease;
	public float fourthSpeedDuration;
	public float firstSkillKillRequirement;

	public static float enemiesKilledSinceLastSpell;
	private Image cooldownImage;
	private float nextAvailableSpellTime;
	private ResourceLogic resLogic;
	private CharacterControl charControl;

	// Use this for initialization
	void Start () {
		nextAvailableSpellTime = 0;
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
		charControl = GameObject.Find ("character").GetComponent<CharacterControl> ();
		cooldownImage = GameObject.Find ("LifeTapCooldown").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= nextAvailableSpellTime) {
			if (Input.GetKey(firstSpell) && enemiesKilledSinceLastSpell >= firstSkillKillRequirement){
				executeFirstSpell();
			} else if (Input.GetKey(secondSpell)){
				executeSecondSpell();
			} else if (Input.GetKey(thirdSpell)){
				executeThirdSpell();
			} else if (Input.GetKey(fourthSpell)){
				executeFourthSpell();
			}
		}

		if (enemiesKilledSinceLastSpell < firstSkillKillRequirement) {
			cooldownImage.fillAmount = 1;

		} else if (Time.time < nextAvailableSpellTime) {
			cooldownImage.fillAmount = (nextAvailableSpellTime - Time.time) / sharedSpellCooldown;
		} else {
			cooldownImage.fillAmount = 0;
		}
	}

	public void executeFirstSpell(){
		resLogic.spendGoldOnBlood(0, firstFlatBloodGain);
		nextAvailableSpellTime = Time.time + sharedSpellCooldown;
		enemiesKilledSinceLastSpell = 0;
	}

	public void executeSecondSpell()
	{
		//TODO: Double player's next spell damage
		nextAvailableSpellTime = Time.time + sharedSpellCooldown;
	}

	public void executeThirdSpell()
	{
		resLogic.timeInvulOver = Time.time + secondInvulDuration;
		nextAvailableSpellTime = Time.time + sharedSpellCooldown;
	}

	public void executeFourthSpell()
	{
		charControl.increaseMoveSpeedForDuration(fourthSpeedIncrease, fourthSpeedDuration);
		nextAvailableSpellTime = Time.time + sharedSpellCooldown;
	}
}
