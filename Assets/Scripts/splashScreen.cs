using UnityEngine;
using System.Collections;

public class splashScreen : MonoBehaviour {
	public float delayTime;
	public float fadingTime;
	public Sprite[] spriteArray;

	private SpriteRenderer spriteRen;
	private float timeToFadeFirstIn;
	private float timeToFadeFirstOut;
	private float timeToChangeSprite;
	private float timeToFadeSecondIn;
	private float timeToFadeSecondOut;
	private float timeToStartGame;

	void Start()
	{
		spriteRen = this.GetComponent<SpriteRenderer> ();
		Color color = spriteRen.material.color;
		color.a = 0;
		spriteRen.material.color = color;
		timeToFadeFirstIn = Time.time;
		timeToFadeFirstOut = timeToFadeFirstIn + delayTime;
		timeToChangeSprite = timeToFadeFirstOut + fadingTime;
		timeToFadeSecondIn = timeToChangeSprite + 0.2f;
		timeToFadeSecondOut = timeToFadeSecondIn + delayTime;
		timeToStartGame = timeToFadeSecondOut + fadingTime;
	}

	void Update()
	{
		if (Time.time > timeToFadeFirstIn && Time.time < timeToFadeFirstOut) {
			fadeIn ();
		}else if (Time.time > timeToFadeFirstOut && Time.time < timeToChangeSprite) {
			fadeOut ();
		} else if (Time.time > timeToChangeSprite && Time.time < timeToFadeSecondIn) {
			changeSprite(1);
		} else if (Time.time > timeToFadeSecondIn && Time.time < timeToFadeSecondOut) {
			fadeIn();
		} else if (Time.time > timeToFadeSecondOut && Time.time < timeToStartGame) {
			fadeOut ();
		} else if (Time.time > timeToStartGame) {
			Application.LoadLevel(1);
		} 
	}

	void changeSprite(int spriteNumber)
	{
		spriteRen.sprite = spriteArray [spriteNumber];
	}

	void fadeOut()
	{
		Color color = spriteRen.material.color;
		color.a -= (1/fadingTime)* (float)Time.deltaTime;
		spriteRen.material.color = color;
	}

	void fadeIn()
	{
		Color color = spriteRen.material.color;
		color.a += (1/fadingTime) * (float)Time.deltaTime;
		spriteRen.material.color = color;
	}

}
