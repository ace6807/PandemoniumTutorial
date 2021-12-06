using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private bool alive = true;
    private float flightDirection = 1;
    private float lifetime;
    private BoxCollider2D projectileCollider;
    private SpriteRenderer projectileSpriteRenderer;
    private Animator projectileAnimator;



    void Awake()
    {
        projectileCollider = GetComponent<BoxCollider2D>();
        projectileSpriteRenderer = GetComponent<SpriteRenderer>();
        projectileAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (alive){
            transform.Translate(Vector3.right * speed * Time.deltaTime * flightDirection);
        }

        lifetime += Time.deltaTime;
        if (lifetime > 3){
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D col){
        KillProjectile();
    }

    private void KillProjectile(){
        projectileCollider.enabled = false;
        alive=false;
        projectileAnimator.SetTrigger("explode");
    }

    public void ResetProjectile(bool flipDirection){
        lifetime = 0;
        gameObject.SetActive(true);
        projectileSpriteRenderer.flipX = flipDirection;
        flightDirection = flipDirection ? -1 : 1;
        alive = true;
        projectileCollider.enabled = true;
    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }

}
