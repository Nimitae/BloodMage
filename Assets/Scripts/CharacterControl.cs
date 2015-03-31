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

	Rigidbody2D rigid;
	private bool isGrounded = true;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		float verticalVelocity = rigid.velocity.y;
		if (Input.GetKey (moveLeft)) {
			transform.localEulerAngles = new Vector3(0f,180f,0f);
			rigid.velocity = new Vector2 (-speed, verticalVelocity);
		} else if (Input.GetKey (moveRight)) {
			transform.localEulerAngles = new Vector3(0f,0f,0f);
			rigid.velocity = new Vector2 (speed, verticalVelocity);
		} else {
			rigid.velocity = new Vector2 (0, verticalVelocity);
		}

		if (Input.GetKey (jump) && isGrounded) {
			rigid.AddForce(new Vector2(0, jumpForce));
			isGrounded = false;
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

}
