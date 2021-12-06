using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackCooldown = 5;
    [SerializeField] Transform firepoint;
    [SerializeField] GameObject[] ammo;


    float timeSinceLastShot = Mathf.Infinity;
    PlayerController playerController;
    SpriteRenderer spriteRenderer;
    Animator playerAnimator;


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (timeSinceLastShot > attackCooldown && Input.GetButtonDown("Fire1") && playerController.CanAttack()){
            Attack();
        }

        timeSinceLastShot += Time.deltaTime;
    }


    private void Attack(){
        var fireBallIndex = GetNextFireball();
        if (fireBallIndex >= 0){
            playerAnimator.SetTrigger("shoot");
            timeSinceLastShot = 0;
            ammo[fireBallIndex].transform.position = firepoint.position;
            ammo[fireBallIndex].GetComponent<Projectile>().ResetProjectile(spriteRenderer.flipX);
        }
    }

    private int GetNextFireball(){
        for (int i = 0; i < ammo.Length; i++)
        {
            if (!ammo[i].activeInHierarchy){
                return i;
            }
        }
        return -1;
    }

}
