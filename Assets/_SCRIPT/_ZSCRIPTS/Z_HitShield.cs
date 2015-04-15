using UnityEngine;
using System.Collections;

public class Z_HitShield : MonoBehaviour {

	public Material shieldMaterial;
	// Use this for initialization
	void Start () {
	
		shieldMaterial.color = new Color(1,1,1);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator OnCollisionEnter(Collision thing){

		if (thing.gameObject.tag == "Alien") {

			shieldMaterial.color = new Color(1,0,0);
			yield return new WaitForSeconds (.4f);
			shieldMaterial.color = new Color(1,1,1);

		}

	}
}
