using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestController : MonoBehaviour {
    
    public GameObject endScreen;

    public RawImage compassBackground, questMarker;
    public Text questTxt;
    public Quest[] quests;
    
    public static int currQuestNum;

    Quest currQuestObj;
    Transform questLocation;
    GameObject player;

    Image cover;
    Text endText;

    float compassUnit;
    bool fadeOut;
    byte alpha;

    void Start() {
        compassUnit = compassBackground.rectTransform.rect.width / 400f;
        questMarker.gameObject.SetActive(false);
        endScreen.SetActive(false);

        fadeOut = false;
        alpha = 0;
    }

    void Update() {
        player = GameObject.FindGameObjectWithTag("Player");
        compassBackground.uvRect = new Rect(player.transform.localEulerAngles.y / 360f, 0f, 1f, 1f);
        
        if (currQuestNum != 0) {
            //Find current quest in list
            if (!currQuestObj) {
                foreach (Quest quest in quests) {
                    //Quest index found
                    if (quest.index == currQuestNum) {
                        currQuestObj = quest;
                        questMarker.gameObject.SetActive(true);
                    }
                }
            }

            //All quest related objects in the world
            GameObject[] questObjects = GameObject.FindGameObjectsWithTag("Quest");

            //Find quest objects related to current quest
            int currQuestCount = 0;
            foreach (GameObject questObj in questObjects) {
                if (questObj.GetComponent<QuestObject>().index == currQuestNum) {
                    currQuestCount++;
                }

                if (!questLocation) {
                    questLocation = questObj.transform;
                }
            }

            //Make quest marker follow object
            if (questLocation) {
                questMarker.rectTransform.anchoredPosition = GetPosOnCompass();
            }

            //No quest objects related to current quest
            // means quest is complete
            if (currQuestCount == 0) {
                currQuestObj = null;
                questLocation = null;

                questMarker.gameObject.SetActive(false);

                if (currQuestNum == 1) {
                    endScreen.SetActive(true);

                    cover = endScreen.transform.GetChild(0).GetComponent<Image>();
                    endText = endScreen.transform.GetChild(1).GetComponent<Text>();

                    fadeOut = true;
                }

                currQuestNum = 0;
            }
        }

        //End Screen
        if (fadeOut) {
            if (alpha < 254) {
                alpha += 1;

                cover.color = new Color32((byte) cover.color.r, (byte) cover.color.g, (byte) cover.color.b, alpha);
                endText.color = new Color32((byte) endText.color.r, (byte) endText.color.g, (byte) endText.color.b, alpha);
            }
        }

        //Go to main menu
        if (Input.GetKeyDown(KeyCode.M)) {
            SceneManager.LoadScene("scene_menu");
        }

        //Quest Text UI
        if (currQuestNum == 0) {
            questTxt.text = "0 Active Quests";
        } else {
            questTxt.text = "Current Quest: " + currQuestObj.questInfo;
        }
    }

    //Calculate marker position to quest object
    Vector2 GetPosOnCompass() {
        Vector2 questPos = new Vector2(questLocation.position.x, questLocation.position.z);
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(questPos - playerPos, playerFwd);
        return new Vector2(compassUnit * angle, questMarker.rectTransform.anchoredPosition.y);
    }
}