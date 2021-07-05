using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest Info")]
public class Quest : ScriptableObject {
    public int index;

    [TextArea(5, 15)]
    public string questInfo;
}