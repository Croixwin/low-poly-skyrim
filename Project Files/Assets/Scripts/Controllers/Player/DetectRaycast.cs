using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRaycast : MonoBehaviour {

    public static RaycastHit DetectRay(float dist) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        var layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");

        Physics.Raycast(ray, out hit, dist, ~layerMask);
        
        return hit;
    }
}
