using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public int health { get { return _currentHealth; }}
    [SerializeField] float timeInvincible = 2.0f;
    [SerializeField] float speed = 3.0f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int projectileForce = 500;
    [SerializeField] AudioClip projectileClip;
    [SerializeField] AudioClip getDamagedClip;

    private Rigidbody2D _rigidbody;
    private Vector2 _movement;
    private int _currentHealth;
    private bool _isInvincible;
    private float _invincibleTimer;

    private Animator _animator;
    private Vector2 _lookDirection = new Vector2(0,-1);

    private AudioSource _audioSource;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 60;

        _currentHealth = maxHealth;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput   = Input.GetAxisRaw("Vertical");
        _movement = new Vector2(horizontalInput, verticalInput);

        //Checks Ruby's look direction to set the correct animation
        if(!Mathf.Approximately(_movement.x, 0.0f) || !Mathf.Approximately(_movement.y, 0.0f)) //Use Mathf.Approximately instead of == because the way computers store float numbers means there is a tiny loss in precision
        {
            _lookDirection.Set(_movement.x, _movement.y);
            _lookDirection.Normalize();
        }

        _animator.SetFloat("Look X", _lookDirection.x);
        _animator.SetFloat("Look Y", _lookDirection.y);
        _animator.SetFloat("Speed", _movement.magnitude);
        //End

        //Activates when Ruby is damaged
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer < 0)
                _isInvincible = false;  
        }
        //End

        //Checks if Ruby is launching
        if(Input.GetButtonDown("Fire1")){
            LaunchProjectile();
        }
        //End

        //Checks if Ruby wants to talk
        if (Input.GetKeyDown(KeyCode.X)){
            RaycastHit2D _hit = Physics2D.Raycast(_rigidbody.position + Vector2.up * 0.2f, _lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            // Debug.DrawRay(origin, direction, Color.red, 10.0f);
            if (_hit.collider){
                NPCController character = _hit.collider.GetComponent<NPCController>();
                if(character){
                    character.DisplayDialog();
                }
            }
        }
        //End
    }

    private void FixedUpdate() {
        float horizontalVelocity = _movement.normalized.x * speed;
        float verticalVelocity = _movement.normalized.y * speed;
        _rigidbody.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    private void LateUpdate()
    {
    
    }

    // Additional Functions
    public void ChangeHealth(int amount)
    {
        if (amount < 0) //Ruby got damaged
        {
            if (_isInvincible)
                return;
            
            _isInvincible = true;
            _invincibleTimer = timeInvincible;
            
            _animator.SetTrigger("Hit");

            this.PlaySound(getDamagedClip);
        }

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        // Debug.Log("Health: "+_currentHealth + "/" + maxHealth);

        // Both currentHealth and maxHealth are integers, and dividing two integers is treated as a whole division by C#, so 2/4 won’t give 0.5 but 0. Forcing one of the numbers to be a float makes this become 2/4.0, which gives us a float result equal to 0.5.
        UIHealthBar.instance.SetValue(_currentHealth / (float)maxHealth);
    }

    public void PlaySound(AudioClip clip){
        _audioSource.PlayOneShot(clip);
    }


    private void LaunchProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, _rigidbody.position + Vector2.up * 0.5f, Quaternion.identity);
        ProjectileController projectile = projectileObject.GetComponent<ProjectileController>();

        projectile.Launch(_lookDirection, projectileForce);
        _animator.SetTrigger("Launch");

        this.PlaySound(projectileClip);
    }
    // End
}