using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimmyGuessManager : MonoBehaviour
{
    public int pegsInStorage = 3;
    private UITilesManager uTilesManager;
    public float timeUntilPick = 8f;
    public float pickTimer = 0f;
    public int pegsHeld = 0;
    public TextMeshProUGUI pegsHeldText;
    public Transform pegsContainer;

    public GameObject arrow;
    public GameObject pegToCopy;

    public bool waitingOnPegs = false;


    private List<(int, int)> pickedList = new List<(int, int)>();

    private void Start()
    {
        uTilesManager = GetComponent<UITilesManager>();
    }
    public void DepositAll()
    {
        pegsInStorage = pegsInStorage + pegsHeld;
        for(var i = 0; i < pegsInStorage; i++ )
        {
            GameObject newObj = Instantiate(pegToCopy, pegsContainer);
            newObj.SetActive(true);
            newObj.transform.position = new Vector3(pegToCopy.transform.position.x, pegToCopy.transform.position.y + i * 5f, pegToCopy.transform.position.z);
            newObj.transform.rotation = Random.rotation;
        }
        pegsHeld = 0;
        pegsHeldText.text = "x" + pegsHeld.ToString();
    }
    public void IncreasePegsHeld()
    {
        pegsHeld = Mathf.Min(pegsHeld + 1, 9);
        pegsHeldText.text = "x" + pegsHeld.ToString();
        arrow.SetActive(false);
        waitingOnPegs = false;
    }

    private void showArrow()
    {
        arrow.SetActive(true);
    }
    private void doDialogPrompt()
    {

    }

    private void Update()
    {
        if(pegsInStorage == 0 && !waitingOnPegs)
        {
            showArrow();
            doDialogPrompt();
            waitingOnPegs = true;
        }
        pickTimer += Time.deltaTime;
        if(pickTimer >= timeUntilPick && pegsInStorage > 0)
        {
            uTilesManager.activeTile.OnReveal();
            pegsInStorage = pegsInStorage - 1;
            Destroy(pegsContainer.GetChild(0).gameObject);
            pickedList.Add(uTilesManager.activeTile.tilePos);
            bool foundRandomTile = false;
            do
            {
                (int, int) randomTile = (Random.Range(0, 8), Random.Range(0, 7));
                if(!pickedList.Contains(randomTile))
                {
                    foundRandomTile = true;
                    uTilesManager.tiles[randomTile.Item1, randomTile.Item2].MakeActiveTile();
                }
            }
            while (!foundRandomTile);
            pickTimer = 0f;
        }
    }
}
