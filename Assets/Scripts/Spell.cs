using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    //to do: use pool
    #region PARAMETERS
    public int damage;
    public float moveSpeed;
    public float secondsToDie;
    [ColorUsage(true, true)]
    public Color color;
    #endregion

    #region REFERENCES
    public Rigidbody2D rb;
    public Material mat;
    #endregion

    #region METHODS
    public void ChangeDirection(Vector2 direction)
    {
        rb.velocity = direction.normalized * moveSpeed;
    }
    #endregion

    #region UNITY
    protected virtual void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        mat.SetColor("_color", color);

        if(secondsToDie > 0)
            Destroy(gameObject, secondsToDie);
    }

    protected bool DefaultCollisionBehaviour(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HasHp"))
        {
            collision.gameObject.SendMessage("TakeDamage", (int)damage);
            Destroy(gameObject);
            return true;
        }
        else if (collision.gameObject.name == "Shield")
        {
            collision.gameObject.SendMessageUpwards("ShieldTakeDamage", (int)damage);
            Destroy(gameObject);
            return true;
        }
        else
            return false;
    }
    #endregion
}
