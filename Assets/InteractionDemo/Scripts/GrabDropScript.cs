using UnityEngine;
using System.Collections;

public class GrabDropScript : MonoBehaviour 
{
	public GameObject[] draggableObjects;
	public float dragSpeed = 3.0f;
	public Material selectedObjectMaterial;
	
	private InteractionManager manager;
	private bool kill_manager;

	private bool isLeftHandDrag;

	private GameObject draggedObject;
	private float draggedObjectDepth;
	private Vector3 draggedObjectOffset;
	private Material draggedObjectMaterial;
	
	GameObject infoGUI;
	
//	void Start(){
//		if(manager == null)
//		{
//			manager = InteractionManager.Instance;
//		}
//	}
	void Awake() 
	{
		infoGUI = GameObject.Find("HandGuiText");
		kill_manager = false;
	}
	
	
	void Update() 
	{
		// get the interaction manager instance
		if(manager == null && !kill_manager)
		{
			manager = InteractionManager.Instance;
		}

		if(manager != null && manager.IsInteractionInited())
		{
			Vector3 screenNormalPos = Vector3.zero;
			Vector3 screenPixelPos = Vector3.zero;
			
			if(draggedObject == null)
			{
				// no object is currently selected or dragged.
				// if there is a hand grip, try to select the underlying object and start dragging it.
				if(manager.IsLeftHandPrimary())
				{
					// if the left hand is primary, check for left hand grip
					if(manager.GetLastLeftHandEvent() == InteractionWrapper.InteractionHandEventType.Grip)
					{
						isLeftHandDrag = true;
						screenNormalPos = manager.GetLeftHandScreenPos();
					}
				}
				else if(manager.IsRightHandPrimary())
				{
					// if the right hand is primary, check for right hand grip
					if(manager.GetLastRightHandEvent() == InteractionWrapper.InteractionHandEventType.Grip)
					{
						isLeftHandDrag = false;
						screenNormalPos = manager.GetRightHandScreenPos();
					}
				}
				
				// check if there is an underlying object to be selected
				if(screenNormalPos != Vector3.zero)
				{
					// convert the normalized screen pos to pixel pos
					screenPixelPos.x = (int)(screenNormalPos.x * this.camera.pixelWidth);
					screenPixelPos.y = (int)(screenNormalPos.y * this.camera.pixelHeight);
					Ray ray = camera.ScreenPointToRay(screenPixelPos);
					
					// check for underlying objects
					RaycastHit hit;
					if(Physics.Raycast(ray, out hit))
					{
						foreach(GameObject obj in draggableObjects)
						{
							if(hit.collider.gameObject == obj && obj.gameObject.tag == "Play")
							{

								print("hit game object");
								manager = null;
								kill_manager = true;
								Application.LoadLevel("MMM_Level_01");
								Destroy(gameObject);
								//gameObject.SetActive(false);
								break;
							}else if (hit.collider.gameObject == obj && obj.gameObject.tag == "Instructions"){

								print("hit game object");
								manager = null;
								kill_manager = true;
								Application.LoadLevel("Instructions");
								Destroy(gameObject);
								//gameObject.SetActive(false);
								break;
							}else if (hit.collider.gameObject == obj && obj.gameObject.tag == "Options"){

								print("hit game object");
								manager = null;
								kill_manager = true;
								Application.LoadLevel("Options");
								Destroy(gameObject);
								//gameObject.SetActive(false);
								break;
							}else if (hit.collider.gameObject == obj && obj.gameObject.tag == "Main"){

								print("hit " + obj.gameObject.tag);
								manager = null;
								kill_manager = true;
								Application.LoadLevel("MainMenu");
								Destroy(gameObject);
								//gameObject.SetActive(false);
								break;
							}
						}
					}
				}
				
			}
			else
			{
				// continue dragging the object
				screenNormalPos = isLeftHandDrag ? manager.GetLeftHandScreenPos() : manager.GetRightHandScreenPos();
				
//				// check if there is pull-gesture
//				bool isPulled = isLeftHandDrag ? manager.IsLeftHandPull(true) : manager.IsRightHandPull(true);
//				if(isPulled)
//				{
//					// set object depth to its original depth
//					draggedObjectDepth = -Camera.main.transform.position.z;
//				}
				
				// convert the normalized screen pos to 3D-world pos
				screenPixelPos.x = (int)(screenNormalPos.x * Camera.main.pixelWidth);
				screenPixelPos.y = (int)(screenNormalPos.y * Camera.main.pixelHeight);
				screenPixelPos.z = screenNormalPos.z + draggedObjectDepth;
				
				Vector3 newObjectPos = Camera.main.ScreenToWorldPoint(screenPixelPos) - draggedObjectOffset;
				draggedObject.transform.position = Vector3.Lerp(draggedObject.transform.position, newObjectPos, dragSpeed * Time.deltaTime);
				
				// check if the object (hand grip) was released
				bool isReleased = isLeftHandDrag ? (manager.GetLastLeftHandEvent() == InteractionWrapper.InteractionHandEventType.Release) :
					(manager.GetLastRightHandEvent() == InteractionWrapper.InteractionHandEventType.Release);
				
				if(isReleased)
				{
					// restore the object's material and stop dragging the object
					draggedObject.renderer.material = draggedObjectMaterial;
					draggedObject = null;
				}
			}
		}
	}
	
	void OnGUI()
	{
		if(infoGUI != null && manager != null && manager.IsInteractionInited())
		{
			string sInfo = string.Empty;
			
			uint userID = manager.GetUserID();
			if(userID != 0)
			{
				if(draggedObject != null)
					sInfo = "Dragging the " + draggedObject.name + " around.";
				else
					sInfo = "Grab and drag an object around.";
			}
			else
			{
				sInfo = "Waiting for Users...";
			}
			
			infoGUI.guiText.text = sInfo;
		}
	}
	
}
