using UnityEngine;
using System.Collections;

public class Z_HitShield : MonoBehaviour {

	public Material shieldMaterial;
	public AudioClip[] shieldSounds;
	// Use this for initialization
	void Start () {
	
		shieldMaterial.color = new Color(1,1,1);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator OnCollisionEnter(Collision thing){

		if (thing.gameObject.tag == "Alien") {
			audio.PlayOneShot(shieldSounds[0]);
			shieldMaterial.color = new Color(1,0,0);
			yield return new WaitForSeconds (.4f);
			shieldMaterial.color = new Color(1,1,1);

		}
		if (thing.gameObject.tag == "Laser" || thing.gameObject.tag == "Fireball") {
			audio.PlayOneShot(shieldSounds[1]);
			shieldMaterial.color = new Color(1,0,0);
			yield return new WaitForSeconds (.4f);
			shieldMaterial.color = new Color(1,1,1);
			
		}


	}
}
