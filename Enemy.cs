using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Config params
    [Header("Config Params")]
    [SerializeField] float moveSpeed = 1;
    // Tiempo en morir luego del impacto de player
    [SerializeField] float timeToDie = 0.15F;

    // References cached
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myHeadCollider2D;
    Animator myAnimator;
    

    // Start is called before the first frame update
    void Start()
    {
        myHeadCollider2D = GetComponent<CapsuleCollider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
        // moverse hacia el lado que afronta
        if (IsFacingRight())
        {
            myRigidbody2D.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            myRigidbody2D.velocity = new Vector2(-moveSpeed, 0);
        }        
    }
    // comprobar si esta caminando hacia la derecha
    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // si un enemigo golpea el headcollider no hacer flip
        if (myHeadCollider2D.IsTouchingLayers(LayerMask.GetMask("Player"))){ return; }
        // Si llegaste al final del camino, hacer flip al sprite
        transform.localScale = new Vector2(-Mathf.Sign(myRigidbody2D.velocity.x), 1f);
    }

    private void Dead()
    {
        // Morir si un player o una flecha golpea el headcollider
        if (myHeadCollider2D.IsTouchingLayers(LayerMask.GetMask("Player", "Arrow")))
        {
            GetComponent<Animator>().SetTrigger("Enemy Dead");
            StartCoroutine(EnemyDead());
        }
        // Morir si una flecha golpea mi boddycollider
        else if(GetComponent<CircleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Arrow")))
        {
            GetComponent<Animator>().SetTrigger("Enemy Dead");
            StartCoroutine(EnemyDead());
        }
    }
    // Coroutine para la muerte
    private IEnumerator EnemyDead ()
    {
        GetComponent<AudioSource>().Play();
        moveSpeed = 0;
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}
