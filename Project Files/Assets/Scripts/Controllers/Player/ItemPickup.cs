using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour {

    public Transform holdPosition;
    public ItemController heldItem;
    public Text interactTxt;

    public float dist = 2f;
    public bool holding;

    public static bool lookingAtItem;

    ItemController newItem;

    void Start() {
        lookingAtItem = false;
    }

    void Update() {
        //Detect item raycast
        if (DetectItem()) {
            lookingAtItem = true;
            interactTxt.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E)) {
                if (holding) { //Drop current item
                    heldItem.itemHeld = false;
                }
                
                //Assign new item
                heldItem = null;
            
                heldItem = newItem;
                heldItem.itemHeld = true;

                //Set item parent and position
                heldItem.transform.SetParent(holdPosition, false);

                holding = true;
            }
        } else if (!DialogueUI.interacting) { //Disable pickup text when not looking
            lookingAtItem = false;
            interactTxt.gameObject.SetActive(false);
        }

        //Drop item
        if (Input.GetKeyDown(KeyCode.Q) && holding) {
            heldItem.itemHeld = false;
            heldItem = null;

            holding = false;
        }
    }

    //Item Raycast Function
    bool DetectItem() {
        RaycastHit hit = DetectRaycast.DetectRay(10f);

        if (hit.collider != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Item")) {
            newItem = hit.transform.GetComponent<ItemController>();
            return true;
        }

        return false;
    }
}
