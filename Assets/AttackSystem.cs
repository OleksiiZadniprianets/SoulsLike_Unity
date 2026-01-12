using UnityEngine;
using System.Collections;

public class AttackSystem : MonoBehaviour
{
    [SerializeField] private GameObject AtckRadius;
    [SerializeField] private float hitTime = 0.1f;
    [SerializeField] private float attackCooldown = 1f;

    public static bool Attack = false;
    public static float Damage = 5;

    private bool IsAttacking = false;

    public Animator anim;
    public GameObject character;


    void Start()
    {
        anim = character.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator AttackCoroutine()
    {
        IsAttacking = true;
        anim.SetBool("Attack", true);


        yield return new WaitForSeconds(0.9f);


        Attack = true;
        AtckRadius.SetActive(true);

        yield return new WaitForSeconds(hitTime);

        Attack = false;
        AtckRadius.SetActive(false);


        yield return new WaitForSeconds(attackCooldown);

        IsAttacking = false;
        anim.SetBool("Attack", false);
    }
}
