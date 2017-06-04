using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	Vector3 origPos;

	float shakeAmount = 0;
	float shakeDampening = 0;

	void Start(){
		origPos = transform.position;
	}

	void Update(){

		if (shakeAmount <= 0) {
			shakeAmount = 0;
			shakeDampening = 0;
			transform.position = origPos;
		} else {
			transform.position = origPos + (Vector3)(Random.insideUnitCircle * shakeAmount * Time.deltaTime);

			shakeAmount -= shakeDampening * Time.deltaTime;
		}

	}

	public void Shake(float amt, float damp){
		shakeAmount += amt;
		shakeDampening += damp;
	}

}
