using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    CharacterController controller;

    public float playerSpeed = 4f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public float sprintMult = 2f;

    public static float health, stamina;
    public static GameObject currEnemy;

    Vector3 velocity;
    bool isGrounded;
    float sprintAmt;

    void Start() {
        controller = GetComponent<CharacterController>();

        health = 100;
        stamina = 100;

        sprintAmt = 1f;
    }

    void Update() {
        //Ground collision detection
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        
        //Player movement
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;
        move.Normalize();

        //Play walking sound
        if (move != Vector3.zero) {
            SoundController.walk = 1;
        }

        controller.Move(move * playerSpeed * sprintAmt * Time.deltaTime);

        //Player jump
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Player sprint
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0f) {
            sprintAmt = sprintMult;

            stamina -= Time.deltaTime * 10f;
        } else {
            sprintAmt = 1f;

            if (stamina < 100f) {
                stamina += Time.deltaTime * 7f;
            }
        }

        //Regenerate health
        if (health <= 0) {
            SceneManager.LoadScene("scene_1");
        } else if (health < 100f) {
            health += Time.deltaTime;
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
