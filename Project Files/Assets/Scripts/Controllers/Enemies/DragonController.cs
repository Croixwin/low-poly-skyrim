using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour {

    public ParticleSystem fireParticle;
    public Transform initPos;
    public float rotSpeed = 5f, moveSpeed = 50f;

    public float dragonHealth, distToPlayer, fireCooldown;

    public static bool breathFire;

    void Start() {
        fireCooldown = 5f;
    }

    void Update() {
        dragonHealth = this.GetComponent<EnemyController>().health;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        distToPlayer = this.GetComponent<EnemyController>().distToPlayer;

        if (dragonHealth <= 0f) {
            Destroy(this.gameObject);
        } else if (dragonHealth < 100f) {
            dragonHealth += Time.deltaTime / 2f;
        }

        //Check dragon distance to player
        if (distToPlayer <= 30f) {
            this.transform.SetParent(null);
            Vector3 movePos = Vector3.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.deltaTime);

            if (distToPlayer >= 15f) { 
                //Move towards player
                this.transform.position = movePos;
            } else {
                this.transform.position = new Vector3(this.transform.position.x, movePos.y, this.transform.position.z);

                //Attack player
                DragonAttack();
            }

            //Rotate towards player
            RotateTowards(player.transform, rotSpeed * Time.deltaTime);
        } else {
            if (Vector3.Distance(initPos.position, this.transform.position) >= 20f) {
                //Move towards
                this.transform.position = Vector3.MoveTowards(this.transform.position, initPos.position, moveSpeed * Time.deltaTime);

                //Rotate towards initial point
                RotateTowards(initPos, moveSpeed * Time.deltaTime);
            } else {
                if (transform.parent == null)
                    this.transform.SetParent(initPos);
                
                //Rotate pivot point
                initPos.transform.Rotate(new Vector3(0, -1, 0) * moveSpeed * Time.deltaTime);
            }
        }
    }

    void RotateTowards(Transform other, float moveSpeed) {
        Vector3 dir = (other.position - this.transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRot, Time.deltaTime * rotSpeed);
    }

    void DragonAttack() {
        fireCooldown -= Time.deltaTime;

        if (!fireParticle.isPlaying) {
            breathFire = false;
        }

        if (fireCooldown <= 0) {
            fireParticle.Play(true);

            breathFire = true;
            fireCooldown = 15f;
        }
    }

    //Dragon collide with player
    void OnCollisionStay(Collision other) {
        if (other.transform.CompareTag("Player")) {
            PlayerController.health -= Time.deltaTime * 2f;
        }
    }
}
