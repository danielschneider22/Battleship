using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDie : MonoBehaviour
{
    private ShipController shipController;

    private void Start()
    {
        shipController = transform.parent.GetComponent<ShipController>();
    }

    public void DoDeath()
    {
        shipController.ClearShipCoor();
    }
}
