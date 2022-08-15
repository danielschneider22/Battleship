using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropOffColliderManager : MonoBehaviour
{
    public AnimationAndMovementController moveController;
    public GameObject ePromptGameObject;

    private void LateUpdate()
    {
        if (moveController.canDropOff)
        {
            ePromptGameObject.transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + 3.5f, transform.parent.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DropOffArea" && !moveController.isDragging)
        {
            moveController.canDropOff = true;
            ePromptGameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Deposit [E]";
            ePromptGameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "DropOffArea")
        {
            moveController.canDropOff = false;
            ePromptGameObject.SetActive(false);
        }
    }
}
