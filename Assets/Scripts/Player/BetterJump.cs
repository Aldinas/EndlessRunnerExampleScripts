using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    [Range(1, 10)]
    public float jumpVelocity;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;
    private bool isJumping = false;

    private Controller gameController;
    private PlayerController playerController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<Controller>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Has the player hit the jump button, and are they already jumping?
        if(Input.GetButtonDown("Jump") && !isJumping && !gameController.Paused)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
            playerController.PlayJumpSound();
        }

        // Are we falling?
        if (rb.velocity.y < 0)
        {
            // Yes, apply gravity multiplier.
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier -1) * Time.deltaTime;
        }
        // Are we jumping, and does the player does not have the jump button held down?
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Yes, apply jump multiplier.
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            isJumping = true;
        }
        // else if(rb.velocity.y == 0)
        // {
        //     isJumping = false;
        // }

        // Get the layermask of the ground.
        LayerMask groundMask = LayerMask.GetMask("Terrain Engine");
        // Add a raycast to detect the ground, rather than relying on velocity. 
        RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundMask);
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 1f);

        if(groundCheckRay.collider != null)
        {
            // We are on the ground, re-enable jumping.
            isJumping = false;
        }
    }
}
