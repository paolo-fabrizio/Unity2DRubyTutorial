using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] float livingTime = 3f;
    
    private Rigidbody2D _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        Destroy(gameObject, livingTime); // Destroys the object after some time
    }

    void OnCollisionEnter2D(Collision2D collision){
        // Se llama a la función con conocimiento que el GameObject es Enemy y no otro
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();

        if (enemy) {
            enemy.Fix();
        }
        Destroy(gameObject);

        // collision.gameObject.SendMessageUpwards("Fix"); // Se llama una función sin necesidad de evaluar si el GameObject es Enemy
    }

    // Aditional Functions
    public void Launch(Vector2 direction, float force)
    {
        _rigidbody.AddForce(direction * force);
    }
    // End
}