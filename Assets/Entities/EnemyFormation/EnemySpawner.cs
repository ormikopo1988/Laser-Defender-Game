using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	
	//give it width and height to represent the formation itself
	public float width = 10.0f;
	public float height = 5.0f;
	
	public float speed = 0.1f;
	public float spawnDelay = 0.5f;
	
	private bool movingRight = false;
	private float xmin;
	private float xmax;

	// Use this for initialization
	void Start () {
	
		//distance between the main camera and the object of the player
		float distanceToCamera = this.transform.position.z - Camera.main.transform.position.z;
		//calculate xmin and xmax from the camera itself
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distanceToCamera));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distanceToCamera));
		
		xmin = leftmost.x;
		xmax = rightmost.x;
	
		//draw enemies
		SpawnUntilFull();
	}
	
	void SpawnEnemies() {
		//get all the position children of the EnemyFormation GameObject
		foreach(Transform child in this.transform) {
			//create a new enemy and position it in the place of each child position
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			//put the new enemy created inside the EnemyFormation in each position in the hierarchy on the left
			enemy.transform.parent = child;	
		}
	}
	
	void SpawnUntilFull() {
		Transform nextFreePosition = NextFreePosition();
		if(nextFreePosition) {
			GameObject enemy = Instantiate(enemyPrefab, nextFreePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = nextFreePosition;
		} 
		if(NextFreePosition()) {
			Invoke("SpawnUntilFull", spawnDelay);
		}
	}
	
	public void OnDrawGizmos() {
		Gizmos.DrawWireCube(this.transform.position, new Vector3(width, height, 0));
	}
	
	// Update is called once per frame
	void Update () {
		if(movingRight) {
			this.transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			this.transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
		//reverse the direction if the formation reaches a left or right edge
		float rightEdgeOfFormation = this.transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = this.transform.position.x - (0.5f * width);
		if(leftEdgeOfFormation < xmin) {
			movingRight = true;
		} else if(rightEdgeOfFormation > xmax) {
			movingRight = false;
		}
		
		//check for all enemies being killed
		if(AllMembersDead()) {
			//redraw enemies
			SpawnUntilFull();
		}
	}
	
	Transform NextFreePosition() {
		//iterate through all objects in the EnemyFormation hierarchy
		foreach(Transform childPositionGameObject in this.transform) {
			if(childPositionGameObject.childCount == 0) { 
				return childPositionGameObject;
			}
		}
		return null;
	}
	
	bool AllMembersDead() {
		//iterate through all objects in the EnemyFormation hierarchy
		foreach(Transform childPositionGameObject in this.transform) {
			if(childPositionGameObject.childCount > 0) { 
				return false;
			}
		}
		return true;
	}
}
