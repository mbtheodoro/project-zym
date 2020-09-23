using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region REFERENCES
    public GameObject parryHitbox;
    public GameObject shieldHitbox;
    public Transform container;
    public Transform shootPoint;

    public Rigidbody2D rb;

    public GameObject spellPrefab;
    #endregion

    #region PARAMETERS
    public float moveSpeed;
    public int maxShieldHealth;
    public float shieldRecoveryTime;
    public int maxHealth;
    public float hitBuffer;
    #endregion

    #region ATTRIBUTES
    private Vector2 moveDir;
    public Vector2 lookDir;
    public Vector2 lastPos;

    private int shieldHealth;
    private int curHealth;
    private float shieldRecoveryTimer;

    #endregion

    #region EVENTS
    public void OnShield(InputAction.CallbackContext context)
    {
        if (shieldHealth <= 0)
            return;

        if (context.started)
        {
            shieldHitbox.SetActive(false);
            parryHitbox.SetActive(true);
        }
        if (context.performed)
        {
            shieldHitbox.SetActive(true);
            parryHitbox.SetActive(false);
        }
        if (context.canceled)
        {
            shieldHitbox.SetActive(false);
            parryHitbox.SetActive(false);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>().normalized * moveSpeed;
        rb.velocity = moveDir;
    }

    public void OnLookGamepad(InputAction.CallbackContext context)
    {
        if (context.canceled)
            return;

        lookDir = context.ReadValue<Vector2>();
    }

    public void OnLookMouse(InputAction.CallbackContext context)
    {
        Vector2 mousepos = (Vector2) Camera.main.ScreenToWorldPoint((Vector3)context.ReadValue<Vector2>()) ;
        lookDir = mousepos - (Vector2) transform.position;
    }
    
    public void TakeDamage(int dmg)
    {
        if (curHealth - dmg <= 0)
            Death();
        else
            curHealth -= dmg;

        rb.position = lastPos;
    }

    public void ShieldTakeDamage(int dmg)
    {
        if (shieldHealth > 0)
        {
            shieldHealth -= dmg;
            Debug.Log("shield Health: "+shieldHealth);
        }
        if(shieldHealth <= 0)
            shieldHitbox.SetActive(false);
        shieldRecoveryTimer = 0f;
    }
    #endregion

    #region METHODS

    private void Shoot()
    {
        GameObject go = Instantiate(spellPrefab, shootPoint.position, Quaternion.identity);
        Spell s = go.GetComponent<Spell>();
        s.ChangeDirection(lookDir);
    }
    
    private void ShieldRecovery()
    {
        if (shieldHealth < maxShieldHealth)
        {
            shieldHealth++;
            Debug.Log("shield Health: " + shieldHealth);
        }
        shieldRecoveryTimer = 0f;
    }

    private void Death()
    {
        Debug.Log("Player dead"); 
        //Destroy(gameObject);
    }
    #endregion

    #region UNITY
    private void Start()
    {
        curHealth = maxHealth;
        shieldHealth = maxShieldHealth;
        shieldRecoveryTimer = 0f;
    }

    private void FixedUpdate()
    {
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        container.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //rb.velocity = moveDir;

        lastPos = rb.position;
    }

    void Update()
    {
        if (shieldHealth < maxShieldHealth)
        {
            shieldRecoveryTimer += Time.deltaTime;
            if (shieldRecoveryTimer >= shieldRecoveryTime)
                ShieldRecovery();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.velocity = moveDir;
    }
    //private Vector2 LookForCollisionNextFrame()
    //{
    //    //check for collisions on X axis
    //    Vector2 directionX = new Vector2(moveDir.x, 0f);
    //    RaycastHit2D[] contacts = new RaycastHit2D[16];
    //    float distanceX = Mathf.Abs(moveDir.x);

    //    int count = rb.Cast(directionX, contacts, directionX.magnitude * Time.fixedDeltaTime + hitBuffer);

    //    for (int i = 0; i < count; i++)
    //    {
    //        if (!contacts[i].transform.gameObject.CompareTag("Wall"))
    //            break;
    //        distanceX = contacts[i].distance - hitBuffer;
    //        moveDir.x = 0f;
    //    }


    //    //check for collisions on Y axis
    //    Vector2 directionY = new Vector2(0f, moveDir.y);
    //    float distanceY = Mathf.Abs(moveDir.y);

    //    count = rb.Cast(directionY, contacts, directionY.magnitude * Time.fixedDeltaTime + hitBuffer);

    //    for (int i = 0; i < count; i++)
    //    {
    //        if (!contacts[i].transform.gameObject.CompareTag("Wall"))
    //            break;
    //        distanceY = contacts[i].distance - hitBuffer;
    //        moveDir.y = 0f;
    //    }

    //    //adjust direction
    //    if (directionX.x < 0)
    //        distanceX *= -1;
    //    if (directionY.y < 0)
    //        distanceY *= -1;

    //    return new Vector2(distanceX, distanceY);
    //}
    #endregion
}
