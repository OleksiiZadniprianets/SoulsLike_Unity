using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image healthb;

    private Camera cam;


    void Start()
    {
        cam = Camera.main;
    }
    
    public void UPDHealthBar(float maxHealth, float health)
    {
        healthb.fillAmount = health / maxHealth;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
