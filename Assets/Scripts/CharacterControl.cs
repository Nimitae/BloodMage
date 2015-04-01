using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {

	public KeyCode jump;
	public KeyCode moveDown;
	public KeyCode moveLeft;
	public KeyCode moveRight;
	public int speed = 10;
	public int jumpForce = 2;
	public float timeInvulInSeconds = 1;
	public GameObject gameManager;
	public bool disableMovement;
	private float activeSpeedMultiplier;
	public bool speedIsIncreased;
	public float timeDeactivateSpeed;

	Rigidbody2D rigid;
	private bool isGrounded = true;

	// Use this for initialization
	void Start () {
		speedIsIncreased = false;
		activeSpeedMultiplier = 1;
		rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		float verticalVelocity = rigid.velocity.y;
		if (Input.GetKey (moveLeft)) {
			transform.localEulerAngles = new Vector3(0f,180f,0f);
			rigid.velocity = new Vector2 (-speed*activeSpeedMultiplier, verticalVelocity);
		} else if (Input.GetKey (moveRight)) {
			transform.localEulerAngles = new Vector3(0f,0f,0f);
			rigid.velocity = new Vector2 (speed*activeSpeedMultiplier, verticalVelocity);
		} else {
			rigid.velocity = new Vector2 (0, verticalVelocity);
		}

		if (Input.GetKey (jump) && isGrounded) {
			rigid.AddForce(new Vector2(0, jumpForce));
			isGrounded = false;
		}

		if (speedIsIncreased && Time.time > timeDeactivateSpeed) {
			speedIsIncreased = false;
			activeSpeedMultiplier = 1;
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.transform.tag == "Ground") {
			if (this.rigid.velocity.y < 0.001)
			{
				isGrounded = true;
			}
		}
	}

	void OnCollisionExit2D (Collision2D col)
	{
		if (col.transform.tag == "Ground") {
			isGrounded = false;
		}
	}

	public void increaseMoveSpeedForDuration(float multiplier, float duration)
	{
		speedIsIncreased = true;
		timeDeactivateSpeed = Time.time + duration;
		activeSpeedMultiplier *= multiplier;
	}

}
