using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript : MonoBehaviour
{
	public int speed;
	private bool hasbeencool;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float stamina = GetComponentInParent<PlayerController>().stamina;
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
		speed = 0;
        if(stamina > 0 && Input.GetKey(KeyCode.LeftShift) && hasbeencool && (horizontalMovement != 0 || verticalMovement != 0)){
           speed = 2;
		   if(stamina < 0.025) speed = 1;
        }
		else if (horizontalMovement != 0 || verticalMovement != 0){
            speed = 1;
        }
		hasbeencool = GetComponentInParent<PlayerController>().sprintCooledDown;
		GetComponent<Animator>().SetInteger("speed", speed);
    }
}
