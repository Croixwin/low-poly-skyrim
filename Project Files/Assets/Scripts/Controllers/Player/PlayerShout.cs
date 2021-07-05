using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShout : MonoBehaviour {

    public float blastPower = 30000f;
    public float blastRadius = 500f;

    public ParticleSystem shoutParticle;
    
    bool shout;

    void Start() {
        shout = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && PlayerController.stamina >= 75f) {
            shout = true;
            SoundController.shout = 1;

            shoutParticle.Play(true);

            PlayerController.stamina -= 75f;
        } else if (Input.GetKeyUp(KeyCode.F)) {
            shout = false;
        }
    }

    void OnTriggerStay(Collider other) {
        Rigidbody rg = other.GetComponent<Rigidbody>();

        if (rg && !rg.isKinematic && shout) {
            other.GetComponent<Rigidbody>().AddExplosionForce(-blastPower, this.transform.localPosition, blastRadius);

            shout = false;
        }
    }
}
