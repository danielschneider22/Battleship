using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragColliderManager : MonoBehaviour
{
    public AnimationAndMovementController moveController;
    public GameObject ePromptGameObject;
    private List<string> dragList = new List<string>();

    private void LateUpdate()
    {
        if(moveController.canDrag)
        {
            ePromptGameObject.transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + 3.5f, transform.parent.position.z);
        }
    }
    private string cardinalDirectionAwayFromPlayer(Vector3 playerPos, Vector3 obj)
    {
        float xDiff = playerPos.x - obj.x;
        float zDiff = playerPos.z - obj.z;
        if(Mathf.Abs(xDiff) > Mathf.Abs(zDiff))
        {
            return playerPos.x > obj.x ? "left" : "right";
        } else
        {
            return playerPos.z > obj.z ? "bottom" : "top";
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "CanDragShipCollider" && !moveController.isDragging)
        {
            dragList.Add(other.gameObject.transform.parent.parent.name);
            moveController.draggingGameObjLoc = cardinalDirectionAwayFromPlayer(moveController.transform.position, other.transform.position);
            moveController.canDrag = true;
            moveController.draggingGameObj = other.gameObject.transform.parent.parent.gameObject;
            ePromptGameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "CanDragShipCollider")
        {
            dragList.Remove(other.gameObject.transform.parent.parent.name);
            if(dragList.Count == 0)
            {
                moveController.canDrag = false;
                ePromptGameObject.SetActive(false);
            }
            
        }
    }
}
