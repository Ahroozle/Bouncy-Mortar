using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject mortarType;

	public float firingForce = 5;

	public float health = 5;

	Color myColor;

	// Use this for initialization
	void Start () {

		myColor = new Color (Random.Range(0.5f,1), Random.Range(0.5f,1), Random.Range(0.5f,1));

		SpriteRenderer[] allsps = GetComponentsInChildren<SpriteRenderer> ();

		foreach (SpriteRenderer s in allsps)
			s.color = Color.Lerp (s.color, myColor, 0.5f);
	
	}
	
	// Update is called once per frame
	void Update () {

		//if (!GameManager.matchStarted)
		//	WalkingMode ();
		//else
			FiringMode ();

		if (GetComponent<Rigidbody2D> ().velocity.y < 0)
			transform.up = Vector3.Lerp (transform.up, Vector3.up, 16 * Time.deltaTime);

	}

	void WalkingMode(){
	}

	void FiringMode(){

		transform.GetChild (0).transform.up =
			(Vector2)(Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.GetChild (0).position).normalized;

		if (Input.GetMouseButtonDown (0))
			Fire ();
	}

	void Fire(){

		GameObject currmortar = (GameObject)Instantiate (mortarType, transform.GetChild(0).position + (transform.GetChild(0).up * 0.20f), Quaternion.identity);

		currmortar.GetComponent<Mortar> ().owner = gameObject;
		currmortar.GetComponent<Mortar> ().myColor = myColor;


		float modif = Mathf.Lerp (1,
		                          1.5f,
		                          Mathf.Clamp (GameManager.maxNumBounces - currmortar.GetComponent<Mortar>().numBounces,
		             						   1.0f,
		             						   Mathf.Infinity) / GameManager.maxNumBounces);
		currmortar.GetComponent<Rigidbody2D> ().AddForce (transform.GetChild (0).up * (firingForce * modif));

		currmortar.transform.up = transform.GetChild (0).up;

	}

	void OnHit(float damage){
	}
}
