using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [Header("Player Health")]
    public float MaxHealth = 100f;
    public float Health = 0f;

    public HealthBar healthBar;
    public GameOver gameOver;

    private float playerPositionY;
    private Animator MyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        MyAnimator = GetComponent<Animator>();

        Health = MaxHealth;
        healthBar.SetMaxHealth(Health);
        playerPositionY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        TakeFallDamage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DamageObject") {
            TakeDamage(MaxHealth * 0.2f);
        }
    }

    public void TakeFallDamage()
    {
        if (playerPositionY - this.transform.position.y > 30)
        {
            TakeDamage(MaxHealth);
            //healthBar.SetHealth(0f);
            //gameOver.setup();
        }
    }

    void TakeDamage(float damage)
    {
        Health -= damage;
        healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            MyAnimator.SetTrigger("IsDead");
            gameOver.setup();
        } else
        {
            MyAnimator.SetTrigger("GetHurt");
        }
    }
}

/* Update is called once per frame
    void Update()
    {
        TakeFallDamage();
    }

    public void TakeFallDamage()
    {
        if (playerPositionY - this.transform.position.y > 20)
        {
            TakeDamage(MaxHealth);
        }
    }

    void TakeDamage(float damage)
    {
        Health -= damage;
        healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            gameOver.setup();
        }
    }*/