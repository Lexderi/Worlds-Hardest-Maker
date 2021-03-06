using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Apply this to every field which the player can stand on
 */
public class FieldTracking : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;
        if (collider.CompareTag("Player"))
        {
            PlayerController controller = collider.GetComponent<PlayerController>();
            controller.currentFields.Add(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;
        if (collider.CompareTag("Player"))
        {
            PlayerController controller = collider.GetComponent<PlayerController>();
            controller.currentFields.Remove(gameObject);
        }
    }

}
