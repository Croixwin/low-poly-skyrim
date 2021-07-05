using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float health = 100f;
    public float distToPlayer;

    void Update() {
        distToPlayer = Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
    }
}