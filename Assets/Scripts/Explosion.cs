using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public int radius = 2;

	ParticleSystem psys;
	ParticleSystem.Particle[] particles;
	int numParticles;

	[HideInInspector] public Color myColor;

	// Use this for initialization
	void Start () {
		psys = GetComponentInChildren<ParticleSystem> ();
		particles = new ParticleSystem.Particle[psys.maxParticles];

		psys.startColor = myColor;

		psys.Emit (100);
		numParticles = psys.GetParticles (particles);

		int numStrands = Random.Range (3, 9);

		int numLayers = numParticles / numStrands;

		for (int i = 0; i < numParticles; ++i) {

			int currLayer = i / numStrands;

			Vector3 nextVec = Quaternion.Euler(0,0,360.0f * ((i%numStrands)/(float)numStrands)) * (Vector3.up * (i/(float)numStrands));

			particles[i].position -= Vector3.forward * (particles[i].position.z + 0.1f);
			particles[i].velocity = (nextVec * Mathf.Lerp(0,0.5f,currLayer/(float)numLayers)) + (Vector3)Random.insideUnitCircle;
			particles[i].size *= (numLayers-currLayer)/(float)numLayers;

			particles[i].color = Color.Lerp(particles[i].color,Color.black,Random.value);
		}

		psys.SetParticles (particles, numParticles);

		GameObject.FindObjectOfType<ScreenShake> ().Shake (1, 1);


		GameObject[] allplayers = GameObject.FindGameObjectsWithTag ("Player");

		foreach(GameObject currPlayer in allplayers){
			Vector2 toPlayer = currPlayer.transform.position - transform.position;

			if(toPlayer.magnitude < radius)
				currPlayer.GetComponent<Rigidbody2D>().AddForce(toPlayer.normalized * 500 * ((radius-toPlayer.magnitude)/radius));
		}

		GameObject[] allmortars = GameObject.FindGameObjectsWithTag ("Mortar");

		foreach (GameObject currMortar in allmortars) {
			Vector2 toMort = currMortar.transform.position - transform.position;

			if(toMort.magnitude < radius)
				currMortar.GetComponent<Rigidbody2D>().AddForce(toMort.normalized * 500 * ((radius-toMort.magnitude)/radius));
		}

	}
	
	// Update is called once per frame
	void Update () {

		numParticles = psys.GetParticles (particles);

		if (numParticles <= 0)
			Destroy (gameObject);
		else {
			int numStrands = Random.Range (3, 9);
		
			int numLayers = numParticles / numStrands;

			for (int i = 0; i < numParticles; ++i) {
			
				int currLayer = i / numStrands;
			
				Vector3 nextVec = Vector3.down * (i / (float)numStrands);

				particles [i].velocity += nextVec * Mathf.Lerp (0, 0.1f, currLayer / (float)numLayers)
					* Mathf.Lerp (1, 0, particles [i].lifetime / psys.startLifetime);
			}
		
			psys.SetParticles (particles, numParticles);
		}

	}
}
