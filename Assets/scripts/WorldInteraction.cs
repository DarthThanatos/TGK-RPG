
using UnityEngine;
using UnityEngine.AI;

public class WorldInteraction : MonoBehaviour {

	NavMeshAgent playerAgent;
    private Interactable lastInteractedObject;

	void Start () {
		playerAgent = GetComponent<NavMeshAgent> ();
	}


	void Update () {
		Vector3 playerPos = playerAgent.transform.position;
		float CameraY = Camera.main.transform.position.y;
		Camera.main.transform.LookAt(playerAgent.transform.position);
		Camera.main.transform.position = new Vector3 (playerPos.x, CameraY, playerPos.z + 10);
		if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			GetInteraction ();
		}

	}



	void GetInteraction(){
		Ray interactionRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit interactionInfo;
		if (Physics.Raycast (interactionRay, out interactionInfo, Mathf.Infinity)) {
            playerAgent.updateRotation = true;
			GameObject interactedObject = interactionInfo.collider.gameObject;
            if(interactedObject.tag == "Enemy")
            {
                CancelLastInteractable();
                interactedObject.GetComponent<Interactable>().MoveToInteraction(playerAgent);
            }
			else if (interactedObject.tag == "Interactable Object")
            {
                CancelLastInteractable(interactedObject.GetComponent<Interactable>());
                interactedObject.GetComponent<Interactable> ().MoveToInteraction(playerAgent);
			} 
			else
            {
                CancelLastInteractable();
                playerAgent.stoppingDistance = 0;
				playerAgent.destination = interactionInfo.point;

			}
		}
			
	}

    private void CancelLastInteractable(Interactable currentInteractable = null)
    {
        if (lastInteractedObject != null)
        {
            Debug.Log("Cancelling action of " + lastInteractedObject.name);
            lastInteractedObject.CancelAction();
        }
        lastInteractedObject = currentInteractable;
    }
}
