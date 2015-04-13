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
	public float[] lifeTapBloodGain;
	public static int enemiesKilledSinceLastSpell;

	private Image cooldownImage;
	private float nextAvailableSpellTime;
	private ResourceLogic resLogic;
	private CharacterControl charControl;
	private Animator animator;
	private bool lifeTapActive;
	private float lifeTapDeactivateTime;

	public GameObject lifeTapCounter;
	private Text lifeTapCounterText;

	// Use this for initialization
	void Start () {
		lifeTapActive = false;
		animator = GetComponent<Animator> ();
		nextAvailableSpellTime = 0;
		lifeTapCounterText = lifeTapCounter.GetComponent<Text> ();
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
		charControl = GameObject.Find ("character").GetComponent<CharacterControl> ();
		cooldownImage = GameObject.Find ("LifeTapCooldown").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (lifeTapActive && Time.time > lifeTapDeactivateTime) {
			lifeTapActive = false;
			animator.SetBool ("castedLifeTap", false);
		}

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
		if (enemiesKilledSinceLastSpell <= lifeTapBloodGain.Length) {
			resLogic.spendGoldOnBlood (0, lifeTapBloodGain [enemiesKilledSinceLastSpell]);
			print ("Lifetap" + enemiesKilledSinceLastSpell);
		} else {
			resLogic.spendGoldOnBlood (0, lifeTapBloodGain [lifeTapBloodGain.Length-1]);
			print ("Lifetap" + enemiesKilledSinceLastSpell);
		}
		nextAvailableSpellTime = Time.time + sharedSpellCooldown;
		QWERSpell.enemiesKilledSinceLastSpell = 0;
		updateLifeTapCounter ();
		animator.SetBool ("castedLifeTap", true);
		lifeTapActive = true;
		lifeTapDeactivateTime = Time.time + 0.3f;
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

	public void updateLifeTapCounter()
	{
		lifeTapCounterText.text = "" + QWERSpell.enemiesKilledSinceLastSpell;
	}

}
