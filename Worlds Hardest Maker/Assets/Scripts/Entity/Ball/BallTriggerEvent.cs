using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * script to detect player death event 
 * 
 * attach to ball objects
 */
public class BallTriggerEvent : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;
        if (collider.CompareTag("Player"))
        {
            PlayerController controller = collider.GetComponent<PlayerController>();
            if (!controller.IsOnSafeField())
            {
                controller.Die();
            }
        }
    }
}
