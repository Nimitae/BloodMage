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
	
	[HideInInspector]
	public bool pathIsEnded = false;
	public float nextWaypointDistance = 3;
	private int currentWaypoint = 0;
	
	// Use this for initialization
	void Start () {
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
		dir *= speed * Time.fixedDeltaTime;
		rigid.AddForce (dir, fMode);
		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);
		if (dist < nextWaypointDistance)
		{
			currentWaypoint++;
			return;
		}
	}
	
	public void setPlayer(Transform playerTransform)
	{
		player = playerTransform;
	}
}