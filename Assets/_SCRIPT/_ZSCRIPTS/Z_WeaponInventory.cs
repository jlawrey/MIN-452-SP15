using UnityEngine;
using System.Collections;

public class Z_WeaponInventory : MonoBehaviour {

	public GameObject[] weapons;
	public bool[] weaponOn;
	

	// Use this for initialization
	void Start () {

		//initialize weapons on
		weaponOn [0] = true;
		for (int i=1; i<weapons.Length; i++) {
			weaponOn[i] = false;
		}
		//start coroutine to swap out weapons as the player obtains them
		StartCoroutine (weaponSwap ());
	
	}
	
	// Update is called once per frame
	void Update () {

		//constantly checks for 
		for(int i=0; i<weapons.Length;i++){
			if (isWeaponAdded(Z_Score.score,i)) {
				weaponOn [i] = true;
			}else{
				weaponOn [i] = false;
			}
		}

	
	}

	public IEnumerator weaponSwap(){

		for(int i=0;i<weapons.Length;i++) {
			if(!weaponOn[i]){
				weapons[i].SetActive(false);
			}
			weapons[i].SetActive(true);
			
		}
		yield return null;
	}

	public bool isWeaponAdded(int nscore, int weapon_id){

		if (nscore/10 == weapon_id){
			return true;
		}else{
			return false;
		}

	}
}
