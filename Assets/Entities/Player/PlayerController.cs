using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 15.0f;
	public float padding = 0.5f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health = 250f;
	
	public AudioClip fireSound;
	
	private float xmin;
	private float xmax;
	
	void Start() {
		//distance between the main camera and the object of the player
		float distance = this.transform.position.z - Camera.main.transform.position.z;
		//calculate xmin and xmax from the camera itself
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}
	
	void Fire() {
		Vector3 offset = new Vector3(0, 1, 0);
		GameObject beam = Instantiate(projectile, this.transform.position + offset, Quaternion.identity) as GameObject;
		beam.rigidbody2D.velocity = new Vector3(0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fireSound, this.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
		//space key pressed
		if(Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("Fire", 0.000001f, firingRate);
		}
		
		if(Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke("Fire");
		}
	
		if(Input.GetKey(KeyCode.LeftArrow)) {
			//this.transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
			this.transform.position += Vector3.left * speed * Time.deltaTime; //move spaceship to the left
		} else if(Input.GetKey(KeyCode.RightArrow)) {
			//this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
			this.transform.position += Vector3.right * speed * Time.deltaTime; //move spaceship to the right
		}
	
		//restrict player to the gamespace	
		float newX = Mathf.Clamp(this.transform.position.x, xmin, xmax);
		this.transform.position = new Vector3(newX, this.transform.position.y, this.transform.position.z);
		
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		//check whether the enemy is hitted by a Projectile - first get the component from the collider
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		//minus the health
		if(missile) {
			health -= missile.GetDamage();
			missile.Hit();
			if(health <= 0) {
				Die();
			}
		}
	}
	
	void Die() {
		LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		man.LoadLevel("Win Screen");
		Destroy(gameObject);
	}
}
