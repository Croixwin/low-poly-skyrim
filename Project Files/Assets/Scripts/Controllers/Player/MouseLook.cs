using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

    public float mouseSensitivity = 250f;
    public Transform playerBody;

    Vector2 mouseDelta;
    float xRotation = 0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        //Used for smooth editor running/delete later
        QualitySettings.vSyncCount = 2;
    }

    void Update() {
        if (!DialogueUI.isTalking) {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            //Smooth movement
            mouseDelta = Vector2.MoveTowards(mouseDelta, new Vector2(mouseX, mouseY), mouseSensitivity * Time.deltaTime);

            xRotation -= mouseDelta.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseDelta.x);
        }
    }
}
