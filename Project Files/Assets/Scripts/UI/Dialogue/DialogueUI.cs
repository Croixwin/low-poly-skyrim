using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour {

    public GameObject npcPanel, playerPanel;
    public Button[] choiceBtns;
    public Text[] choiceTxt;
    
    public Text npcNameTxt, npcDialogueTxt, interactTxt;
    public static NPC currentNPC;

    Dialogue currDialogue;
    Response[] currResponses;

    public float dist = 2.5f;
    public static bool interacting, isTalking;
    
    int currIndex;

    void Start() {
        interacting = false;
        isTalking = false;

        npcPanel.SetActive(false);
        playerPanel.SetActive(false);

        interactTxt.gameObject.SetActive(false);
    }

    void Update() {
        //Detect interaction with NPC
        if (DetectNPC() && !currentNPC.npc.questComplete) {
            if (!isTalking) {
                interactTxt.gameObject.SetActive(true);
                interacting = true;
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                if (!isTalking) {
                    SoundController.talk = 1;
                }

                isTalking = !isTalking;
                InitDialogue();
            }
        } else if (!ItemPickup.lookingAtItem) {
            interactTxt.gameObject.SetActive(false);
            interacting = false;
        }

        //Interaction logic
        if (isTalking) {
            //Update UI values
            npcNameTxt.text = currentNPC.npc.npcName;
            npcDialogueTxt.text = currDialogue.text;

            interactTxt.gameObject.SetActive(false);

            if (currResponses.Length > 0) {
                Cursor.lockState = CursorLockMode.None;

                int btnIndex1 = currResponses[0].index;
                int btnIndex2 = currResponses[1].index;
                
                //Set player choice buttons
                choiceBtns[0].onClick.AddListener(delegate{clickChoice(btnIndex1);});
                choiceBtns[1].onClick.AddListener(delegate{clickChoice(btnIndex2);});

                //Set player choice text
                choiceTxt[0].text = currResponses[0].text;
                choiceTxt[1].text = currResponses[1].text;
            } else {
                playerPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    //Set dialogue state
    void InitDialogue() {
        if (npcPanel.activeInHierarchy) {
            npcPanel.SetActive(false);
            playerPanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
        } else {
            npcPanel.SetActive(true);
            playerPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
        }

        //Initial index value
        currIndex = 0;

        currDialogue = currentNPC.npc.dialogues[currIndex];
        currResponses = currDialogue.responses;
    }

    //Player button click function
    public void clickChoice(int index) {
        if (index < 0 || index >= currentNPC.npc.dialogues.Length) {
            return;
        }

        currIndex = index;

        currDialogue = currentNPC.npc.dialogues[currIndex];
        currResponses = currDialogue.responses;

        if (currDialogue.quest > 0) {
            QuestController.currQuestNum = currDialogue.quest;
        }
    }

    //NPC Raycast Function
    bool DetectNPC() {
        RaycastHit hit = DetectRaycast.DetectRay(dist);

        if (hit.collider != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("NPC")) {
            currentNPC = hit.transform.GetComponent<NPC>();
            return true;
        }

        return false;
    }
}
