using UnityEngine;
using System.Collections;

public class Z_WeaponInventory : MonoBehaviour {

	public GameObject[] weapons;
	private bool[] weaponOn;
	public int score;
	public int numToKill;
	public Texture2D[] weaponTextures;
	public AudioClip[] weaponIntros;
	public GameObject weaponIcon;
	public static bool check = true;

	// Use this for initialization
	void Start () {

		//initialize weapons on
		//start coroutine to swap out weapons as the player obtains them
		StartCoroutine (UpgradeWeapon ());
	
	}
	
	// Update is called once per frame
	void Update () {

//		score = Z_Score.score;
//		//constantly checks for 
//		for(int i=0; i<weapons.Length;i++){
//			if (isWeaponAdded(Z_Score.score,i)) {
//				weapons[i].SetActive(true);
//				weaponIcon.renderer.material.mainTexture = weaponTextures[i];
//			}else if (!isWeaponAdded(Z_Score.score,i)){
//				weapons[i].SetActive(false);
//			}
//		}

	
	}

	public IEnumerator UpgradeWeapon(){

			print (check + " is check");
			if(check){//do check for weapon upgrade
				score = Z_Score.score;//loads score value
				//check for current score being divided by iterator
				for(int i=0; i<weapons.Length;i++){
					if (isWeaponAdded(Z_Score.score,i)) {
						weapons[i].SetActive(true);
						weaponIcon.gameObject.SetActive(true);
						weaponIcon.renderer.material.mainTexture = weaponTextures[i];
						audio.PlayOneShot(weaponIntros[i]);
						yield return new WaitForSeconds(weaponIntros[i].length);
						weaponIcon.SetActive(false);
						check = false;

					}else if (!isWeaponAdded(Z_Score.score,i)){
						weapons[i].SetActive(false);
					}
				}
			}

	}

	public bool isWeaponAdded(int nscore, int weapon_id){

		if (nscore/numToKill == weapon_id){
			return true;
		}else{
			return false;
		}

	}
}
