using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
           // collision.gameObject.GetComponent<Monster>().TakeDamage(damage, transform.position);
        }

        Destroy(gameObject, 0.1f);
    }
}
