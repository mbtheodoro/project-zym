using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBall : Spell
{
    #region PARAMETERS
    public Light2D light2d;
    #endregion

    #region PARAMETERS
    #endregion

    #region ATTRIBUTES
    protected Vector2 lastVelocity;
    #endregion

    #region OVERRIDES
    protected override void Awake()
    {
        base.Awake();
        light2d.color = color;
    }
    #endregion

    #region METHODS
    #endregion

    #region UNITY
    private void Update()
    {
        lastVelocity = rb.velocity;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (!DefaultCollisionBehaviour(collision))
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
            else if (collision.gameObject.name == "Parry")
            {
                Vector2 direction = Vector2.Reflect(lastVelocity.normalized, collision.GetContact(0).normal);

                ChangeDirection(direction);
            }
        }
    }
    #endregion
}
