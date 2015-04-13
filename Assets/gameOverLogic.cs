using UnityEngine;
using System.Collections;

public class gameOverLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void playAgainButtonPressed()
	{
		Application.LoadLevel (4);
	}

	public void menuButtonPressed()
	{
		Application.LoadLevel (1);
	}
}
