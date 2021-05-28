/*
* (Greg Brandt, Sam Carpenter)
* (Project 2)
* Allows the player to respond to input
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 1;
	[SerializeField] float jumpForce = 1.75f;
	[SerializeField] float fallingVelocityJumpThreshold = 0.2f;
	[SerializeField] GameObject lineOfSiteArrow;
	public bool isBlock;
	[SerializeField] Transform eyes;
	public MouseDetector mouseDetector;
	public static PlayerController mousedOverBlock;
	public Vector2 velocity;
	public bool isPlayable = true;
    GameOverController gameOverHandle;
	
	public int coyoteframes = 120;
	public int coyotethresh = 3;
	
	public int gravy = 3;
	//public int layerr = 8;

	private void Start()
	{
		if(!isBlock) { isPlayable = true; }
        //transform.gameObject.layer = layerr;
        gameOverHandle = FindObjectOfType<GameOverController>();
	}

	//Compensates for the height of the collider 
	float distToGround = 1.15f;

	Rigidbody2D rigidbody;
	public Tilemap pinktiles;
	public Tilemap greytiles;

	private void Awake()
	{
		rigidbody =  GetComponentInChildren<Rigidbody2D>();
		//distToGround = GetComponent<Collider2D>().bounds.extents.y;
	}
	
	private void Update()
	{
		if (isPlayable)
		{
			//Move Left and Right
			float horizontalInput = Input.GetAxis("Horizontal");
			RaycastHit2D[] hitsys = new RaycastHit2D[999];
			int hitnum = rigidbody.Cast(new Vector2(horizontalInput, 0), hitsys, 0.05f);
			if (hitnum == 0 || hitsys[0].transform.gameObject.tag == "Finish"){
				transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
			}
			//for use with transforming
			rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
			rigidbody.gravityScale = gravy;
			pinktiles.color = Color.white;
			greytiles.color = Color.clear;
			if(Input.GetMouseButtonDown(0)) { TransformOther(); }
		} 
		else
		{
			//for use with transforming
			rigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
			rigidbody.gravityScale = 0;
			pinktiles.color = Color.clear;
			greytiles.color = Color.white;
		}

		if (Input.GetMouseButtonUp(0)) { lineOfSiteArrow.SetActive(false); }

        if (rigidbody.transform.position.y < -10)
        {
            gameOverHandle.ReducePlayer();
            Destroy(gameObject);
        }
	}

	private void FixedUpdate()
	{
		bool g = IsGrounded();
		//Jump
		if (Input.GetKeyDown(KeyCode.Space) && isPlayable && (g || coyoteframes <= coyotethresh))
		{
			rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			coyoteframes = coyotethresh + 1;
		}
		
		if(!g){
			coyoteframes++;
		} else{
			coyoteframes = 0;
		}
		
		//if (Input.GetKeyDown(KeyCode.Z) && canTransform) isPlayable = !isPlayable;
	}

	//Shoots a raycast towards the mouse pointer and transforms a block into a player.
	void TransformOther()
	{
		//Determine direction from eyes to mouse position;
		Vector3 mousePosition;
		mousePosition = Input.mousePosition;
		mousePosition.z = 0.0f;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		Vector3 shootDirection = mousePosition - eyes.position;

		//Shoots a raycast toward mouse position
		RaycastHit2D hit = Physics2D.Raycast(eyes.position, shootDirection);//, layerr);
		lineOfSiteArrow.gameObject.SetActive(true);
		lineOfSiteArrow.transform.localScale = new Vector3(Vector2.Distance(eyes.position, mousePosition), 1,1);
		float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
		lineOfSiteArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		if (hit)
		{
			PlayerController otherBlock = hit.transform.GetComponentInParent<PlayerController>();
			if (otherBlock)
			{
				if (otherBlock.mouseDetector.isMousedOver && !otherBlock.isPlayable)
				{
					otherBlock.isPlayable = !otherBlock.isPlayable;
                    gameOverHandle.IncreasePlayer();
				}
			}
		}
	}

	//variable for testing purposes. expendable
	public bool canTransform = true;
	//Checks if there is a collider under the player
	bool IsGrounded()
	{
		//RaycastHit2D[] hitsys = new RaycastHit2D[999];
		//int hitnum = rigidbody.Cast(Vector2.down, hitsys, distToGround);
		//if (hitnum > 0){
		//	//Debug.Log(hit.collider.tag);
		//	//if (!hit.collider.CompareTag("Player")) { return hit; }
		//	//else { return false; }
		//	return true;
		//}
		//return false;

        if (Mathf.Abs(rigidbody.velocity.y) > .03f)
        {
            return false;
        }
        return true;
	}

}
