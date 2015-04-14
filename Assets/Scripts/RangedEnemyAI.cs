using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class RangedEnemyAI: MonoBehaviour, IEnemyAI {
	public Transform player;
	public float updateRate = 2f;
	private Seeker seeker;
	private Rigidbody2D rigid;
	public Path path;
	public float speed = 300f;
	public ForceMode2D fMode;
	public float attackCooldown;
	public Transform rangedEnemyAttackTransform;	
	public float projectileSpeed;
	public float attackDamage;

	[HideInInspector]
	public bool pathIsEnded = false;
	public float nextWaypointDistance = 3;
	private int currentWaypoint = 0;
	private float nextPossibleAttackTime;
	
	// Use this for initialization
	void Start () {
		nextPossibleAttackTime = 0;
		seeker = GetComponent<Seeker>();
		rigid = GetComponent<Rigidbody2D> ();
		if (player == null) {
			Debug.LogError ("No player found!");
			return;
		}
		seeker.StartPath (transform.position, player.position, OnPathComplete);
		StartCoroutine (UpdatePath ());
	}
	
	IEnumerator UpdatePath()
	{
		if (player == null) {
			Debug.LogError ("No player found!");
			return false;
		}
		
		seeker.StartPath (transform.position, player.position, OnPathComplete);
		yield return new WaitForSeconds (1f / updateRate);
		StartCoroutine (UpdatePath());
	}
	
	public void OnPathComplete(Path p)
	{
		//Debug.Log ("We got a path. Did it have an error? " + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
	
	void FixedUpdate()
	{
		/*
		if (player == null) {
			Debug.LogError ("No player found!");
			return;
		}
		
		if (path == null)
		{
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) {
			if (pathIsEnded){
				return;
			}
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;
		Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		float angle = Mathf.Rad2Deg*Mathf.Atan (Mathf.Abs(dir.y / dir.x));


		if (angle < 20 && Time.time > nextPossibleAttackTime) {
			attackPlayer();
		} else {
			dir.x = 0;
			dir *= speed * Time.fixedDeltaTime;
			rigid.AddForce (dir, fMode);
		}


		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);
		if (dist < nextWaypointDistance)
		{
			currentWaypoint++;
			return;
		}
		*/
		Vector3 dir = (player.position - transform.position).normalized;
		float angle = Mathf.Rad2Deg*Mathf.Atan (Mathf.Abs(dir.y / dir.x));
		//print (angle);
		if (angle < 5 && Time.time > nextPossibleAttackTime) {
			attackPlayer();
		} else {
			dir.x = 0;
			dir *= speed * Time.fixedDeltaTime;
			rigid.AddForce (dir, fMode);
		}

		if (player.position.x - transform.position.x > 0) {
			transform.rotation = new Quaternion(0,1,0,0);
		} else {
			transform.rotation = new Quaternion(0,0,0,0);
		}
	}
	
	public void setPlayer(Transform playerTransform)
	{
		player = playerTransform;
	}

	public void attackPlayer()
	{
		Vector3 projectileLocation = new Vector3(transform.position.x, transform.position.y,0);
		Transform newProjectile;
		if (player.transform.position.x > transform.position.x) { 
			newProjectile = (Transform)Instantiate (rangedEnemyAttackTransform, projectileLocation, Quaternion.identity);
		} else {
			newProjectile =(Transform) Instantiate(rangedEnemyAttackTransform,projectileLocation,Quaternion.identity * new Quaternion(0,0,1,0));
		}
		newProjectile.GetComponent<rangedEnemyAttack>().attackDamage = attackDamage;
		newProjectile.GetComponent<rangedEnemyAttack>().projectileSpeed = projectileSpeed;
		nextPossibleAttackTime = Time.time + attackCooldown;
	}
}