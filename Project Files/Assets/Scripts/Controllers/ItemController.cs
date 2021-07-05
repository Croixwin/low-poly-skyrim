using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    public enum ItemType { Default, Sword, Bow, Axe };

    public bool itemHeld, attack; 
    public GameObject meshChild;
    public ItemType type;

    float waitTime;
    bool playSound;

    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        attack = false;
        playSound = false;

        waitTime = 1.3f;
    }

    void Update() {
        if (itemHeld) {
            if (meshChild.GetComponent<BoxCollider>())
                Destroy(meshChild.GetComponent<BoxCollider>());

            this.GetComponent<Rigidbody>().isKinematic = true;
            anim.enabled = true;

            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            float clipTime = animStateInfo.normalizedTime;

            //Attack enemy
            if (attack) {
                waitTime -= Time.deltaTime;

                if (waitTime <= 1f && playSound) {
                    //Play item sound
                    if (type > 0) {
                        SoundController.item = ((int) type) - 1;
                    }

                    playSound = false;
                }

                if (waitTime <= 0 && clipTime >= 0.6f) { //Decrease health
                    if (type == ItemType.Bow) { //Bow logic
                        RaycastHit hit = DetectRaycast.DetectRay(25f);

                        if (DetectEnemy(hit)) {
                            //Attack dragon
                            DragonController drc = hit.transform.parent.GetComponent<DragonController>();
                            if (drc && drc.distToPlayer <= 50f) {
                                hit.transform.parent.GetComponent<EnemyController>().health -= 5f;
                            }
                        }
                    } else if (type == ItemType.Axe) { //Axe logic
                        RaycastHit hit = DetectRaycast.DetectRay(20f);

                        if (DetectEnemy(hit)) {
                            //Attack dragon
                            DragonController drc = hit.transform.parent.GetComponent<DragonController>();
                            if (drc && drc.distToPlayer <= 6f) {
                                hit.transform.parent.GetComponent<EnemyController>().health -= 15f;
                            }
                        }
                    } else { //Sword logic
                        RaycastHit hit = DetectRaycast.DetectRay(15f);

                        if (DetectEnemy(hit)) {
                            //Attack dragon
                            DragonController drc = hit.transform.parent.GetComponent<DragonController>();
                            if (drc && drc.distToPlayer <= 10f) {
                                hit.transform.parent.GetComponent<EnemyController>().health -= 10f;
                            }
                        }
                    }

                    waitTime = 1.3f;
                    attack = false;
                }
            }

            PlayerAction();
        } else {
            if (transform.parent != null) {
                transform.SetParent(null);
                transform.position = new Vector3(transform.position.x, transform.position.y + 8f, transform.position.z);
            }

            anim.enabled = false;

            if (!meshChild.GetComponent<BoxCollider>()) {
                meshChild.AddComponent<BoxCollider>();
                meshChild.GetComponent<BoxCollider>().size *= 2f;
                this.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    bool DetectEnemy(RaycastHit hit) {
        if (hit.collider != null) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                PlayerController.currEnemy = hit.transform.gameObject;

                return true;
            }
        }

        return false;
    }

    void PlayerAction() {
        if (Input.GetMouseButtonDown(0) && !DialogueUI.isTalking && !attack) {
            //Check if idle animation is playing
            anim.SetTrigger("action");

            attack = true;
            playSound = true;
        }
    }
}
