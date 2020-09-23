using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    #region REFERENCES
    public Rigidbody2D rb;
    #endregion
    
    #region PARAMETERS
    public float moveSpeed;
    public int maxHealth;
    
    public Vector2 startDirection = Vector2.right;
    public float speedControllerTime = 0.5f;

    public float minimalTimeToChaos;
    public float chanceToChaos;
    public float chaosParameter;
    #endregion

    #region ATTRIBUTES
    Vector2 lastVelocity;
    int curHealth;

    private float framesToChaos;
    #endregion

    #region METHODS
    void Death()
    {
        Destroy(gameObject);
    }

    bool Chaos()
    {
        if (Random.Range(0f, 1f) > chanceToChaos)
            return false;

        Vector2 perp = Vector2.Perpendicular(rb.velocity) * Random.Range(-chaosParameter, chaosParameter); ;

        Vector2 newDirection = (rb.velocity + perp).normalized;

        ChangeVelocity(newDirection);
        return true;
    }

    void ChangeVelocity(Vector2 direction)
    {
        rb.velocity = direction.normalized * moveSpeed;
        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    public void TakeDamage(int dmg)
    {
        if (curHealth - dmg <= 0)
            Death();
        else
            curHealth -= dmg;
    }

    IEnumerator SpeedController()
    {
        for (;;)
        {
            rb.velocity = rb.velocity.normalized * moveSpeed;
            yield return new WaitForSeconds(speedControllerTime);
        }
    }
    #endregion

    #region UNITY
    void Start()
    {
        ChangeVelocity(startDirection);

        framesToChaos = minimalTimeToChaos * 60;

        StartCoroutine(SpeedController());

        curHealth = maxHealth;
    }
    
    void Update()
    {
        lastVelocity = rb.velocity;

        framesToChaos += Time.deltaTime;
        if (framesToChaos >= minimalTimeToChaos)
        {
            if(Chaos())
                framesToChaos = 0;
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            Vector2 direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            ChangeVelocity(direction);
        }
    }
    #endregion
}
