using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
	public float invincibleDuration = 2.0f;
    public float minMoveSpeed = 4;
    public float maxMoveSpeed = 10;
    public float moveSpeedIncrease = 0.1f;

    private float moveSpeed = 0.1f;
    private float speedIncreasePerMilisecond;
    //private bool playerGrounded = true;
    private bool dead = false;
	private bool invincible = false;
    private Animator anim;
	private float invincibleEndTime;

    // Used for calculating and displaying Score
    private string lastContactObject;
    private int lastContactObjectScore;
    private FloatingText floatingText;
    private Controller controller;

    /// <Animation States Summary>
    /// 0 = idle
    /// 1 = Standing
    /// 2 = Walk
    /// 3 = Jump
    /// 4 = Crouched
    /// 5 = Take_Damage
    /// </summary>

    void Start()
    {
        // Cache components for ease of access.
        anim = GetComponentInChildren<Animator>();
        floatingText = GetComponent<FloatingText>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
        audiosource = this.GetComponent<AudioSource>();
        // Set the move speed to the minimum
        moveSpeed = minMoveSpeed;
        Controller.playerSpeed = (int)minMoveSpeed;

        // Set the player into the "walk" animation state.
        anim.SetInteger("state", 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        if (moveSpeed <= maxMoveSpeed)
        {
            moveSpeed += moveSpeedIncrease * Time.deltaTime;
        }
            
        if (Time.time >= invincibleEndTime)
			invincible = false;

        
    }

    private void FixedUpdate()
    {
        // Check to see if we are jumping over an enemy.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10, LayerMask.GetMask("Enemies"));
        if(hit.collider != null)
        {
            // We are, store the enemy name
            lastContactObject = hit.collider.name;
            lastContactObjectScore = hit.collider.GetComponent<GrantPoints>().PointsValue;
        }
        else
        {
            // We are not. Did we just jump an enemy though (lastContact will be set if we did).
            if (lastContactObject != null)
            {
                // Yes we did, consider this a sucessful jump and show the floating text.
                floatingText.CreateFloatingText("+" , lastContactObjectScore);
                controller.AddPoints(lastContactObjectScore);
                // Reset the contact object back to null ready for the next jump.
                lastContactObject = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Hazard")
        {
			if(!invincible)
			{
				// Oh you dead.
				anim.SetInteger("state", 5);
				dead = true;
                controller.PlayerDied();
				Time.timeScale = 0;
			}
        }

		if (collision.gameObject.tag == "Terrain" && !dead)
		{
			anim.SetInteger("state", 2);
		}
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain" && !dead)
        {
            anim.SetInteger("state", 2);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.tag == "Terrain" && !dead)
        {
            anim.SetInteger("state", 3);
        }   
    }

    public bool isDead()
    {
        return dead;
    }

	public void Revive()
	{
		dead = false;
		invincible = true;
		invincibleEndTime = Time.time + invincibleDuration;
		Time.timeScale = 1;
	}
}
