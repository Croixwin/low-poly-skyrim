using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour {

    void OnTriggerStay(Collider other) {
        if (other.transform.CompareTag("Player") && DragonController.breathFire) {
            PlayerController.health -= Time.deltaTime * 5f;
            SoundController.fire = 1;
        }
    }
}
