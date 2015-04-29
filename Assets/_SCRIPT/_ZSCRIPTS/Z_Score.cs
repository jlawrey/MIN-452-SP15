using UnityEngine;
using System.Collections;

public class Z_Score : MonoBehaviour {

	//score stuff
	public  int score = 0;
	public  int death = 0;
	public  GameObject scoretick;
	public static int difficulty = 2;

	//trigger weapon swap stuff
	public  GameObject[] weapons = new GameObject[4];
	public  int numToKill = 5;
	public int numToDead = 15;
	public  Texture2D[] weaponTextures = new Texture2D[4];
	public  AudioClip[] weaponIntros = new AudioClip[4];
	public  GameObject weaponIcon;


	// Use this for initialization
	void Start () {

		//weaponIcon = GameObject.FindGameObjectWithTag ("WeaponIcon");
		//load all the weapon prefabs into the weaponIcon class
//		weapons = new GameObject[]{
//			GameObject.FindGameObjectWithTag ("Hammer"),
//			GameObject.FindGameObjectWithTag ("Sword"),
//			GameObject.FindGameObjectWithTag ("Spear"),
//			GameObject.FindGameObjectWithTag ("Crossbow")
//		};
		//loads all weapon Introductory sounds
		weaponIntros = new AudioClip[]{
			Resources.Load <AudioClip>("Weapon_Intros/peacekeeper"),
			Resources.Load <AudioClip>("Weapon_Intros/steel_of_justice"),
			Resources.Load <AudioClip>("Weapon_Intros/spear"),
			Resources.Load <AudioClip>("Weapon_Intros/crossbow"),
		};
		//loads all weapon textures
		weaponTextures = new Texture2D[]{
			Resources.Load <Texture2D>("hammer_icon"),
			Resources.Load <Texture2D>("sword_icon"),
			Resources.Load <Texture2D>("spear_icon"),
			Resources.Load <Texture2D>("crossbow_icon"),
		};

		score = 0;
		scoretick = GameObject.FindGameObjectWithTag ("scoretick");
		//StartCoroutine(playWeaponEntry (0));


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddScore(){


		score += 1;
		print ("score " + score);
		int caseswitch = score;
		print ("adjusted score = " + caseswitch);
		switch (caseswitch) {

			case 1:
				weapons[1].SetActive(false);
				weapons[2].SetActive(false);
				weapons[3].SetActive(false);
				weapons[0].SetActive(true);
				StartCoroutine(playWeaponEntry (0));
				break;
			case 6:
				weapons[2].SetActive(false);
				weapons[3].SetActive(false);
				weapons[0].SetActive(false);
				weapons[1].SetActive(true);
				StartCoroutine(playWeaponEntry (1));
				break;
			case 12:
				weapons[1].SetActive(false);
				weapons[3].SetActive(false);
				weapons[0].SetActive(false);
				weapons[2].SetActive(true);
				StartCoroutine(playWeaponEntry (2));
				break;
			case 18:
				weapons[1].SetActive(false);
				weapons[0].SetActive(false);
				weapons[2].SetActive(false);
				weapons[3].SetActive(true);
				StartCoroutine(playWeaponEntry (3));
				break;
		}


		//load up the Hit alien Icon
		GameObject scoreguy = Resources.Load<GameObject> ("Score");
		Vector3 flat = new Vector3(scoreguy.transform.position.x +(score*.07f),scoreguy.transform.position.y,scoreguy.transform.position.z);
		Instantiate (scoreguy, flat, Quaternion.identity);
  		

	}
	public void AddDeath(){
		
		death += 1;
		print ("death " + death);


		//load up the death UI indicator
		GameObject deathguy = Resources.Load<GameObject> ("Death");
		Vector3 flat = new Vector3(deathguy.transform.position.x + (death*.07f),deathguy.transform.position.y,deathguy.transform.position.z);
		Instantiate (deathguy, flat, Quaternion.identity);
		if (death > numToDead) {

			Application.LoadLevel(4);

		}
		
	}



	public IEnumerator playWeaponEntry(int weapon_id){

		weaponIcon.renderer.material.mainTexture = weaponTextures [weapon_id];
		weaponIcon.SetActive (true);
		audio.PlayOneShot(weaponIntros[weapon_id]);
		yield return new WaitForSeconds(weaponIntros[weapon_id].length);
		weaponIcon.SetActive (false);
	}

}
