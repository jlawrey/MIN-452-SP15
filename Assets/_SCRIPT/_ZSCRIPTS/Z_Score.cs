using UnityEngine;
using System.Collections;

public class Z_Score : MonoBehaviour {

	//score stuff
	public  int score = 0;
	public  int death = 0;
	public  GameObject scoretick;

	//trigger weapon swap stuff
	public  GameObject[] weapons = new GameObject[4];
	public  int numToKill = 5;
	public  Texture2D[] weaponTextures = new Texture2D[4];
	public  AudioClip[] weaponIntros = new AudioClip[4];
	public  GameObject weaponIcon;


	// Use this for initialization
	void Start () {

		weaponIcon = GameObject.FindGameObjectWithTag ("WeaponIcon");
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
		StartCoroutine (UpgradeWeapon ());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddScore(){


		score += 1;
		print ("score " + score);
		StartCoroutine(UpgradeWeapon());
		GameObject scoreguy = Resources.Load<GameObject> ("Score");
		Vector3 flat = new Vector3(scoreguy.transform.position.x +(score*.04f),scoreguy.transform.position.y,scoreguy.transform.position.z);
		Instantiate (scoreguy, flat, Quaternion.identity);
  		

	}
	public void AddDeath(){
		
		death += 1;
		print ("death " + death);
		StartCoroutine (UpgradeWeapon ());
		GameObject deathguy = Resources.Load<GameObject> ("Death");
		Vector3 flat = new Vector3(deathguy.transform.position.x + (death*.07f),deathguy.transform.position.y,deathguy.transform.position.z);
		Instantiate (deathguy, flat, Quaternion.identity);
		if (death > 9) {

			Application.LoadLevel(4);

		}
		
	}


	public IEnumerator UpgradeWeapon(){
		
		//check for current score being divided by iterator
		for(int i=0; i<weapons.Length;i++){
			if (isWeaponAdded(score,i)) {
				weapons[i].SetActive(true);
				weaponIcon.gameObject.SetActive(true);
				weaponIcon.renderer.material.mainTexture = weaponTextures[i];
				audio.PlayOneShot(weaponIntros[i]);
				yield return new WaitForSeconds(weaponIntros[i].length);
				weaponIcon.SetActive(false);
			}else if (!isWeaponAdded(score,i)){
				weapons[i].SetActive(false);
			}
		}
		
	}
	
	public bool isWeaponAdded(int nscore, int weapon_id){
		
		if (nscore/numToKill == weapon_id){
			print (nscore/numToKill + " should not equal weapon_id");
			return true;
		}else{
			return false;
		}
		
	}

}
