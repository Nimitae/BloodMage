using UnityEngine;
using System.Collections;

public class mainMenuScript : MonoBehaviour {

	public void startButtonPressed()
	{
		Application.LoadLevel (3);
	}

	public void instructionsButtonPressed()
	{
		Application.LoadLevel (2);
	}

	public void creditsButtonPressed()
	{

	}
}
