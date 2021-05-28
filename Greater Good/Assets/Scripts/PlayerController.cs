using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float sprintSpeed = 15;
    [SerializeField] float rotateSpeed = 10;
    [SerializeField] float maxStamina = 5;
    [SerializeField] float staminaRegen = 0.5f;
    [SerializeField] float staminaRegenDelay = 1;
    [SerializeField] int hitPoints = 3;
    [SerializeField] int maxHitPoints = 3;
    [SerializeField] bool isSprinting = false;
    [SerializeField] float invincibilityTime = 1;
    [SerializeField] Slider staminaBar;
    [SerializeField] HeartCell[] hearts;
    public float stamina;
    public bool sprintCooledDown = true;
    bool isInvincible = false;

    MonsterPathing[] allNodes;
    public MonsterPathing nearestNode;

    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;
        allNodes = FindObjectsOfType<MonsterPathing>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isInvincible)
        {
            if (collision.collider.gameObject.name == "Monster Fist") { hitPoints--; }
            int i = 0;
            while (i < hitPoints)
            {
                hearts[i].TurnOn();
                i++;
            }
            while (i < maxHitPoints)
            {
                hearts[i].TurnOff();
                i++;
            }
            if (hitPoints == 0) { GameManager.instance.GameOver(); }
            StartCoroutine(BecomeInvincible());
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!CutSceneManager.cutScenePlaying)
        {
            staminaBar.value = 1 - (stamina / maxStamina);
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");
            if (stamina > 0 && Input.GetKey(KeyCode.LeftShift) && sprintCooledDown && (horizontalMovement != 0 || verticalMovement != 0))
            {
                GetComponent<Rigidbody>().MovePosition(transform.position + (new Vector3(horizontalMovement, 0, verticalMovement).normalized * sprintSpeed * Time.fixedDeltaTime));
                stamina -= Time.fixedDeltaTime;
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
                GetComponent<Rigidbody>().MovePosition(transform.position + (new Vector3(horizontalMovement, 0, verticalMovement).normalized * moveSpeed * Time.fixedDeltaTime));
                if (stamina < maxStamina && sprintCooledDown) { stamina += staminaRegen * Time.fixedDeltaTime; }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift ) && sprintCooledDown) { StartCoroutine(StaminaRegenDelay()); }

            Vector3 temp = Input.mousePosition;
            temp.z = temp.y;
            temp = Camera.main.ScreenToWorldPoint(temp);
            temp.y = transform.position.y;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(temp - transform.position), rotateSpeed);
        }
    }

    IEnumerator BecomeInvincible()
	{
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
	}

    IEnumerator StaminaRegenDelay()
	{
        sprintCooledDown = false;
        yield return new WaitForSeconds(staminaRegenDelay);
        sprintCooledDown = true;
	}

    public bool IsItSprinting()
    {
        return isSprinting;
    }

    public void FindNearestNode()
    {
        float lenght = 9999.0f;

        MonsterPathing startNode = null;

        for (int i = 0; i < allNodes.Length; i++)
        {
            if (Mathf.Abs((this.transform.position - allNodes[i].transform.position).magnitude) < lenght)
            {
                lenght = Mathf.Abs((this.transform.position - allNodes[i].transform.position).magnitude);
                startNode = allNodes[i];
            }
        }

         nearestNode = startNode;
    }
}
