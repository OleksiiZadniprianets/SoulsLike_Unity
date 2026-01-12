using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent EnemyAi;
    [SerializeField] GameObject Player;
    [SerializeField] private Healthbar _healthbar;
    [SerializeField] private float maxHealth = 10;
    public float health = 10f;
    public static bool Alive = true;
    public Animator anime;

    [SerializeField] private GameObject AtckRadius;
    [SerializeField] private float hitTime = 0.1f;
    [SerializeField] private float attackCooldown = 0.5f;

    public static bool Attack = false;
    public static float Damage = 10;

    private bool IsAttacking = false;

    void Start()
    {
        EnemyAi = gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("GameController");
        Player.SetActive(true);

        health = maxHealth;
        
        _healthbar.UPDHealthBar(maxHealth, health);
        StartCoroutine(AttackCoroutine());
    }
    void Update()
    {

        if (Alive == true)
        {
            anime.SetBool("Run", true);
            EnemyAi.SetDestination(Player.transform.position);
        }

        if (health <= 0)
        {
            anime.SetBool("Run", false);
            anime.SetBool("Attack", false);
            StartCoroutine(DeathCoroutine());
            Alive = false;
        }
        float Dist_Player = Vector3.Distance(Player.transform.position, gameObject.transform.position); 
        if (Dist_Player < 5f && IsAttacking == false && Alive == true)
        {
            StartCoroutine(AttackCoroutine());
        }
    }
    void Dmg()
    {
        if (health > 0)
        {
            Alive = true;
        }
    }
    void OnTriggerStay(Collider myCollider)
    {
        if (myCollider.tag == ("Radius") && AttackSystem.Attack == true)
        {
            Debug.Log("ImHit");
            health = health - AttackSystem.Damage;
            AttackSystem.Attack = false;
            Debug.Log("And i took damage");
            _healthbar.UPDHealthBar(maxHealth, health);
        }
    }


    IEnumerator AttackCoroutine()
    {
        anime.SetBool("Attack", true);
        IsAttacking = true;

        yield return new WaitForSeconds(3f);

        Attack = true;
        AtckRadius.SetActive(true);

        yield return new WaitForSeconds(hitTime);

        Attack = false;
        AtckRadius.SetActive(false);

        yield return new WaitForSeconds(attackCooldown);

        IsAttacking = false;
        anime.SetBool("Attack", false);
    }


    IEnumerator DeathCoroutine()
    {
        anime.SetBool("Death", true);

        yield return new WaitForSeconds(6f);

        Destroy(this.gameObject);
    }
}
