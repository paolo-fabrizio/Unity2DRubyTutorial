using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    [SerializeField] float moveTime = 3.0f;
    [SerializeField] Vector2 moveAxis;
    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] AudioClip fixedAudio;

    private Rigidbody2D _rigidbody;
    private Vector2 _movement;
    private float _movingTimer;
    private Animator _animator;
    private bool _broken = true;
    private AudioSource _audioSource;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        _movement = moveAxis;
    }

    private void Update() {

        //Timing the movement
        _movingTimer -= Time.deltaTime;
        if(_movingTimer < 0){
            _movingTimer = moveTime;
            _movement = _movement * new Vector2(-1, -1); //Changing position
        }
        //Timing the movement

        //Setting the correct animation
        if(Mathf.Approximately(_movement.x, 0.0f)){ //Use Mathf.Approximately instead of == because the way computers store float numbers means there is a tiny loss in precision
            _animator.SetFloat("MoveX", 0);
            _animator.SetFloat("MoveY", _movement.y);

        } else{
            _animator.SetFloat("MoveX", _movement.x);
            _animator.SetFloat("MoveY", 0);
        }
        //Setting the correct animation


        if(!_broken) return;
    }

    private void FixedUpdate() {
        float horizontalVelocity = _movement.normalized.x * speed;
        float verticalVelocity = _movement.normalized.y * speed;
        _rigidbody.velocity = new Vector2(horizontalVelocity, verticalVelocity);

        if(!_broken) return;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")){
            RubyController controller = collision.gameObject.GetComponent<RubyController>();
            if(controller.health > 0)
            {
                collision.gameObject.SendMessageUpwards("ChangeHealth", -1);
            }
        }
    }


    // Aditional Functions
    public void Fix(){
        _broken = false;
        _rigidbody.simulated = false;
        _animator.SetTrigger("Fixed");

        _audioSource.PlayOneShot(fixedAudio);
        _audioSource.clip = null;

        smokeEffect.Stop();
    }
    // End
}