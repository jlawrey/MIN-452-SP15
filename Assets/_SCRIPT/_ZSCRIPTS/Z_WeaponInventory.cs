using UnityEngine;
using System.Collections;

public class Z_WeaponInventory : MonoBehaviour {

	public GameObject[] weapons;
	private bool[] weaponOn;
	public int score;
	public int numToKill;

	// Use this for initialization
	void Start () {

		//initialize weapons on
		weaponOn [0] = true;
		for (int i=1; i<weapons.Length; i++) {
			weaponOn[i] = false;
		}
		//start coroutine to swap out weapons as the player obtains them
		//StartCoroutine (weaponSwap ());
	
	}
	
	// Update is called once per frame
	void Update () {

		score = Z_Score.score;
		//constantly checks for 
		for(int i=0; i<weapons.Length;i++){
			if (isWeaponAdded(Z_Score.score,i)) {
				weapons[i].SetActive(true);
			}else if (!isWeaponAdded(Z_Score.score,i)){
				weapons[i].SetActive(false);
			}
		}

	
	}

	public IEnumerator weaponSwap(){

		for(int i=0;i<weapons.Length;i++) {
			if(!weaponOn[i]){
				weapons[i].SetActive(false);
			}else{
				weapons[i].SetActive(true);
			}
			
		}
		yield return null;
	}

	public bool isWeaponAdded(int nscore, int weapon_id){

		if (nscore/numToKill == weapon_id){
			return true;
		}else{
			return false;
		}

	}
}
