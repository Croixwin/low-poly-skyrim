using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    public GameObject enemyhealth;
    public Image healthBar, staminaBar;

    void Update() {
        //Health UI
        healthBar.transform.localScale = new Vector3(PlayerController.health / 100f, 1f, 1f);

        //Stamina UI
        staminaBar.transform.localScale = new Vector3(PlayerController.stamina / 100f, 1f, 1f);

        if (PlayerController.currEnemy) {
            enemyhealth.SetActive(true);

            float enemyHP = PlayerController.currEnemy.transform.parent.GetComponent<EnemyController>().health;

            enemyhealth.transform.GetChild(1).localScale = new Vector3(enemyHP / 100f, 1f, 1f);
        } else {
            enemyhealth.SetActive(false);
        }
    }
}
