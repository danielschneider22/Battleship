using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITileManager : MonoBehaviour
{
    public (int, int) tilePos;
    public ShipController shipController;
    public Image spriteRenderer;
    public bool revealed;
    public Sprite damagedSprite;
    public Sprite destroyedSprite;
    public Sprite activeSprite;
    public Color damagedColor;
    public Color destroyedColor;
    public Color activeColor;
    private UITilesManager uTilesManager;
    private AudioManager audiomanager;

    private void Awake()
    {
        uTilesManager = transform.parent.GetComponent<UITilesManager>();

    }
    private void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void MakeInactiveTile()
    {
        uTilesManager.activeTile = null;
        if(!revealed)
        {
            spriteRenderer.enabled = false;
            GetComponent<Image>().color = Color.white;
        }
        if(revealed && shipController == null)
        {
            spriteRenderer.enabled = false;
        }
    }

    public void MakeActiveTile()
    {
        if(uTilesManager.activeTile != null)
        {
            uTilesManager.activeTile.MakeInactiveTile();
        }
        spriteRenderer.sprite = activeSprite;
        spriteRenderer.enabled = true;
        GetComponent<Image>().color = activeColor;
        uTilesManager.activeTile = this;
    }
    public void TimmyManagerClickButton()
    {
        TimmyGuessManager tGuessManager = uTilesManager.GetComponent<TimmyGuessManager>();
        if (tGuessManager.pegsInStorage > 0)
        {
            tGuessManager.pegsInStorage = tGuessManager.pegsInStorage - 1;
            Destroy(tGuessManager.pegsContainer.GetChild(0).gameObject);
            tGuessManager.UpdateInventoryText();
            OnReveal();
        }
    }
    public void OnReveal()
    {
        revealed = true;
        GetComponent<Button>().enabled = false;
        if (shipController == null)
        {
            GetComponent<Image>().enabled = false;
            audiomanager.Play("Splash", false);
        } else
        {
            audiomanager.Play("Explosion", false);
            shipController.DoHit((tilePos));
            if(shipController.shouldClearCoor)
            {
                foreach((int,int) coor in shipController.shipCoord)
                {
                    UITileManager manager = uTilesManager.tiles[coor.Item1, coor.Item2];
                    manager.spriteRenderer.sprite = destroyedSprite;
                    manager.spriteRenderer.enabled = true;
                    manager.GetComponent<Image>().color = destroyedColor;
                }
            } else
            {
                spriteRenderer.sprite = damagedSprite;
                spriteRenderer.enabled = true;
                GetComponent<Image>().color = damagedColor;
            }
            

        }
    }
}
