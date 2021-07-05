using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class Response {
    public string text;
    public int index;
}

[System.Serializable] 
public class Dialogue {
    public int index;
    public string text;
    public Response[] responses;
    public int quest;
}

[System.Serializable]
public class NPCObject {
    public string npcName;
    public bool questComplete;
    public Dialogue[] dialogues; //Case-sensitive match
}

public class NPC : MonoBehaviour {
    public TextAsset jsonFile;
    public NPCObject npc;

    void Start() {
        npc = JsonUtility.FromJson<NPCObject>(jsonFile.text);
    }
}