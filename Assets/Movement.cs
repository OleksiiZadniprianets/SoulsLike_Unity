using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float _speedwalk;
    public float _gravty;
    public float _jumpPower;
    public float _speedRun;
    public float _dash;
    public float _dashCD;

    [SerializeField] private Healthbar _healthbar;
    [SerializeField] private float maxHealth = 10;

    public bool IsDashing = false;

    [SerializeField] public float rotationSpeed = 1;

    private CharacterController _charactercontroller;
    [SerializeField] private Vector3 _walkDirection;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private float _speed;
    public bool Alive = true;
    public Animator anim;
    public GameObject character;
    public static float health;
    public static int end = 0;

    [SerializeField] private Transform cameraTransform;



    public void Start()
    {
        _speed = _speedwalk;
        _charactercontroller = GetComponent<CharacterController>();
        anim = character.GetComponent<Animator>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        health = maxHealth;

        _healthbar.UPDHealthBar(maxHealth, health);

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        Jump(Input.GetKey(KeyCode.Space) && _charactercontroller.isGrounded);
        Run(Input.GetKey(KeyCode.LeftShift));
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        _walkDirection = transform.right * x + transform.forward * z;

        if (health <= 0)
        {
            Alive = false;
            _speedwalk = 0;
            _speedRun = 0;
            anim.SetBool("Dead", true);
            Enemy.Alive = false;
        }

        if (Input.GetKey(KeyCode.E) && IsDashing == false && Alive == true)
        {
            StartCoroutine(DashCoroutine());
        }
        if (Input.GetKey(KeyCode.W) && Alive == true)
        {
            anim.SetBool("Forward", true);
        }
        else
        {
            anim.SetBool("Forward", false);
        }

        if (Input.GetKey(KeyCode.A) && Alive == true)
        {
            anim.SetBool("Left", true);
        }
        else
        {
            anim.SetBool("Left", false);
        }

        if (Input.GetKey(KeyCode.D) && Alive == true)
        {
            anim.SetBool("Right", true);
        }
        else
        {
            anim.SetBool("Right", false);
        }
        if (Input.GetKey(KeyCode.S) && Alive == true)
        {
            anim.SetBool("Backwards", true);
        }
        else
        {
            anim.SetBool("Backwards", false);
        }
    }
    private void FixedUpdate()
    {
        Walk(_walkDirection);
        DoGravity(_charactercontroller.isGrounded);

        Vector3 flatForward = cameraTransform.forward;
        flatForward.y = 0f;

        if (flatForward.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(flatForward);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * 100f * Time.deltaTime
        );
    }

    private void Walk(Vector3 direction)
    {
        _charactercontroller.Move(direction * _speedwalk * Time.fixedDeltaTime);
    }

    IEnumerator DashCoroutine()
    {
        IsDashing = true;
        anim.SetBool("Roll", true);

        _speed += _dash;

        yield return new WaitForSeconds(0.5f);

        _speed -= _dash;
        anim.SetBool("Roll", false);

        yield return new WaitForSeconds(_dashCD);
        IsDashing = false;
    }

    private void DoGravity(bool isGrounded)
    {
        if (isGrounded && _velocity.y < 0)
            _velocity.y = -1f;
        _velocity.y -= _gravty * Time.fixedDeltaTime;
        _charactercontroller.Move(_velocity * Time.fixedDeltaTime);
    }

    private void Jump(bool canJump)
    {
        if (canJump)
            _velocity.y = _jumpPower;
    }

    private void Run(bool canRun)
    {
        _speedwalk = canRun ? _speedRun : _speed;
    }

    void OnTriggerStay(Collider myCollider)
    {
        if (myCollider.tag == ("EnmRad") && Enemy.Attack == true)
        {
            Debug.Log("PlayerHit");
            health -= Enemy.Damage;
            Enemy.Attack = false;
            Debug.Log("And he took damage");
            _healthbar.UPDHealthBar(maxHealth, health);
        }
    }

}


