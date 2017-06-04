using UnityEngine;
using System.Collections;

public class Mortar : MonoBehaviour {

	float origNumBounces;
	public int numBounces;

	public GameObject explosion;

	[HideInInspector] public GameObject owner;

	[HideInInspector] public Color myColor;

	Color origColor;
	Color flickColor = Color.red;

	float mortarPower;

	bool dead = false;
	// Use this for initialization
	void Start () {
		origNumBounces = numBounces;

		mortarPower = 1.0f / Mathf.Clamp(numBounces+1,1,Mathf.Infinity);

		SpriteRenderer[] allsps = GetComponentsInChildren<SpriteRenderer> ();
		
		foreach (SpriteRenderer s in allsps)
			s.color = Color.Lerp (s.color, myColor, 0.5f);

		origColor = transform.GetChild (0).GetChild(0).GetComponent<SpriteRenderer> ().color;

		GetComponentInChildren<ParticleSystem> ().startColor = Color.Lerp (GetComponentInChildren<ParticleSystem> ().startColor, myColor, 0.5f);

	}
	
	// Update is called once per frame
	void Update () {

		if (!dead) {
			transform.GetChild (0).up = GetComponent<Rigidbody2D> ().velocity.normalized;
		}else {
			if(GetComponentInChildren<ParticleSystem>().particleCount <= 0)
				Destroy(gameObject);
		}

	}

	void OnCollisionEnter2D(Collision2D coll){

		if (coll.gameObject.tag == "Player" && coll.gameObject != owner) {

			coll.gameObject.GetComponent<Player> ().SendMessage ("OnHit", mortarPower);

			Die ();
		} else if (numBounces <= 0)
			Die ();
		else {
			--numBounces;
			transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(flickColor,origColor,numBounces/origNumBounces);
		}
	}

	void Die(){

		SpriteRenderer[] rends = GetComponentsInChildren<SpriteRenderer> ();
		Collider2D[] colls = GetComponentsInChildren<Collider2D> ();

		foreach (SpriteRenderer r in rends)
				r.enabled = false;

		foreach (Collider2D c in colls)
			c.enabled = false;

		GetComponent<Rigidbody2D>().isKinematic = true;

		dead = true;

		GameObject explo = (GameObject)Instantiate (explosion, transform.position, Quaternion.identity);
		explo.GetComponent<Explosion> ().myColor = myColor;
	}
}
