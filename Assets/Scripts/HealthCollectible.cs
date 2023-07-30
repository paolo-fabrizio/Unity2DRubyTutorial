using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player"))
        {
            RubyController controller = collision.GetComponent<RubyController>();
            if(controller.health < controller.maxHealth)
            {
                collision.SendMessageUpwards("ChangeHealth", 1);
                Destroy(gameObject);

                controller.PlaySound(audioClip);
            }
        }
    }
}