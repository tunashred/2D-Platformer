using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // not needed to use specific class, but could use a interface
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            bool gotHit = damageable.Hit(attackDamage, knockback);
        }
    }
}