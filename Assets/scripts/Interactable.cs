
using UnityEngine;

using UnityEngine.AI;
public class Interactable : MonoBehaviour {
    [HideInInspector]
	public NavMeshAgent playerAgent;
	private bool hasInteracted;
    bool isEnemy;

	public virtual void MoveToInteraction(NavMeshAgent playerAgent){
        isEnemy = gameObject.tag == "Enemy";
		hasInteracted = false;
		this.playerAgent = playerAgent;
		playerAgent.destination = this.transform.position;
	}


	void Update(){
		if (playerAgent != null && !playerAgent.pathPending && !hasInteracted) {
			if (playerAgent.remainingDistance <= playerAgent.stoppingDistance) {
                if(!isEnemy)
                    Interact ();
                else
                {

                    GameObject.Find("Player").GetComponent<PlayerWeaponController>().PerformWeaponAttack();
                }
                EnsureLookDirection();
				hasInteracted = true;
			}
		}
	}

    public void CancelAction()
    {
        hasInteracted = true;
    }

    void EnsureLookDirection()
    {
        playerAgent.updateRotation = false;
        Vector3 lookDirection = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        playerAgent.transform.LookAt(lookDirection);
        playerAgent.updateRotation = true;
    }

	public virtual void Interact(){
		Debug.Log ("Interacting with base class");
	}

}
