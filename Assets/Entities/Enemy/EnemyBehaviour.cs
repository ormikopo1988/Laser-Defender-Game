using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject projectile;
	
	public float projectileSpeed = 10f;

	public float health = 150f;
	
	public float shotsPerSecond = 0.5f;
	
	public int scoreValue = 150;
	
	public AudioClip fireSound;
	
	public AudioClip deathSound;
	
	private ScoreKeeper scoreKeeper;
	
	void Start() {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
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
		AudioSource.PlayClipAtPoint(deathSound, this.transform.position);
		scoreKeeper.Score(this.scoreValue);
		Destroy(gameObject);
	}
	
	void Fire() {
		Vector3 startPosition = this.transform.position + new Vector3(0, -1, 0);
		GameObject missile = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
		missile.rigidbody2D.velocity = new Vector2(0, -projectileSpeed);
		AudioSource.PlayClipAtPoint(fireSound, this.transform.position);
	}
	
	void Update() {
		//calculate probability of fire
		float probability = shotsPerSecond * Time.deltaTime;
		if(Random.value < probability) {
			Fire();
		}
	}
	
}
