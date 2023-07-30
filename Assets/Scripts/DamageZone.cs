using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.CompareTag("Player")){
            RubyController controller = collision.GetComponent<RubyController>();
            if(controller.health > 0)
            {
                collision.SendMessageUpwards("ChangeHealth", -1);
            }
        }
    }
}