using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config params
    [Header("MOVEMENT")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float fallSpeed = 5f;
    [SerializeField] float gamplingSpeed = 5f;
    [SerializeField] float gamplingSpeedReduction = 1f;
    [SerializeField] float climbingSpeed = 3f;
    [SerializeField] float miniJump = 5f;   
    [Header("VFX")]
    [SerializeField] GameObject hazardVfx;
    [SerializeField] GameObject waterVfx;
    [SerializeField] GameObject lavaVfx;
    [Header("AUDIO")]
    [SerializeField] AudioClip hazardDeadClip;
    [SerializeField] AudioClip waterDeadClip;
    [SerializeField] AudioClip spikesClip;
    [SerializeField] AudioClip swordClashClip;
    [SerializeField] AudioClip shootClip;
    [SerializeField] AudioClip ropeClip;
    [Header("ARROW")]
    [SerializeField] float hookDistance = 10f;
    [SerializeField] ArrowRope arrowWithRope;
    [SerializeField] float arrowSpeed = 5f;
    [SerializeField] Arrow arrow;
    [SerializeField] GameObject gun;
    [Header("GENERAL")]
    [SerializeField] int armor;
    [SerializeField] float invulnerableTime = 1;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 15f);
    [SerializeField] Vector2 deathLavaKick = new Vector2(0f, 20f);
    [SerializeField] float colorChangeFactor;
    
    // States
    bool isAlive = true;
    bool invulnerable = false;
    float gravityAtStart;

    // Cached components references
    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    CapsuleCollider2D myBoddyCollider2D;
    BoxCollider2D myFeetCollider2D;
    TargetJoint2D targetJoint2D;
    DistanceJoint2D distanceJoint2D;
    LineRenderer line;
    AudioSource myAudioSource;
    SpriteRenderer mySpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        armor = 0;
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoddyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        gravityAtStart = myRigidBody2D.gravityScale;
        //Distance Joint
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        //Target Joint
        targetJoint2D = GetComponent<TargetJoint2D>();
        targetJoint2D.enabled = false;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        Climbing();
        MiniJump();
        Jump();
        MiniJump();
        FlipSprite();
        Died();
        ShootArrow();
        ShootArrowAndRope();
        DropHook();
        DropGamplingHook();
        ChangeVulnerableState();
        print(myRigidBody2D.velocity.x);
    }

    private void ChangeVulnerableState()
    {
        if(invulnerable)
        {
            invulnerableTime -= Time.deltaTime;

            mySpriteRenderer.color = new Color(1, 1, 1, colorChangeFactor);

            if (invulnerableTime <= Mathf.Epsilon)
            {
                mySpriteRenderer.color = new Color(1, 1, 1, 1);
                invulnerableTime = 1f;
                invulnerable = false;
            }
        }        
    }

    private void Run()
    {       
        if (distanceJoint2D.enabled == true) { return; }
        //if (myRigidBody2D.velocity.y == 0)
        {
            float inputValue = Input.GetAxis("Horizontal"); // Value between 1 and -1
            Vector2 runVelocity = new Vector2(inputValue * moveSpeed, myRigidBody2D.velocity.y);
            myRigidBody2D.velocity = runVelocity;

            bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
            if (playerHasHorizontalSpeed)
            {
                myAnimator.SetBool("Shoot", false);
            }
            myAnimator.SetBool("Running", playerHasHorizontalSpeed);           
        }        
    }

    private void Climbing()
    {
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody2D.gravityScale = gravityAtStart;
            myAnimator.SetBool("Climbing", false);
            return;
        }

        float inputValue = Input.GetAxis("Vertical"); // Value between 1 and -1
        Vector2 climbVelocity = new Vector2(myRigidBody2D.velocity.x, inputValue * climbingSpeed);
        myRigidBody2D.velocity = climbVelocity;
        myRigidBody2D.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody2D.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);       
    }

    private void Jump()
    {
        if(distanceJoint2D.enabled == true) { return; }
        if (myRigidBody2D.velocity.y < 0 && !myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Foreground", "Canon", "MovingPlatform", "BrickSpot")))
        {
            myRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        }       

        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Foreground", "Canon", "MovingPlatform", "BrickSpot"))) { return; }

        if (Input.GetButtonDown("Jump"))
        {
            if(myAnimator.GetBool("Shoot") == true)
            {
                myAnimator.SetBool("Shoot", false);
            }
            Vector2 setJumpVelocity = new Vector2(0, jumpSpeed);
            myRigidBody2D.velocity += Vector2.up * setJumpVelocity;
        }
    }

    private void MiniJump()
    {
        if (myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            myRigidBody2D.velocity = new Vector2(0, miniJump);
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1f);
        }
    }

    public void AddArmor(int value)
    {
        armor += value;
    }

    public void Died()
    {
        // Contacto con Agua, Lava, Proyectiles, Enemigos, Hazard, Boss
        if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("Water", "EnemyProjectile", "Enemy", "Hazard", "Boss", "Lava", "Spikes")) 
            || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Water", "EnemyProjectile", "Hazard", "Boss", "Lava", "Spikes")))
        {
            // CONTACTO: con Agua
            if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("Water")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Water")))
            {
                myAudioSource.PlayOneShot(waterDeadClip);
                GameObject waterParticles = Instantiate(waterVfx, transform.position, transform.rotation);
                // Disparar animacion de muerte y quitar vida al player (Para todos los muertes) 
                isAlive = false;
                myAnimator.SetTrigger("Dead");
                StartCoroutine(TakePLayerLifeAndRestoreLvl());
                return;
            }
            // CONTACTO: con Lava
            else if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("Lava")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Lava")))
            {
                myAudioSource.PlayOneShot(hazardDeadClip);
                GameObject lavaParticles = Instantiate(lavaVfx, transform.position, transform.rotation);
                myRigidBody2D.velocity = deathLavaKick;
                // Disparar animacion de muerte y quitar vida al player (Para todos los muertes) 
                isAlive = false;
                myAnimator.SetTrigger("Dead");
                StartCoroutine(TakePLayerLifeAndRestoreLvl());
                return;
            }
            // CONTACTO: con Proyectil
            else if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("EnemyProjectile")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("EnemyProjectile")))
            {
                myAudioSource.PlayOneShot(hazardDeadClip);
                // Disparar animacion de muerte y quitar vida al player (Para todos los muertes) 
                isAlive = false;
                myRigidBody2D.velocity = new Vector2(0, 0);
                myAnimator.SetTrigger("Dead");
                StartCoroutine(TakePLayerLifeAndRestoreLvl());
                return;
            }
            else if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazard")))
            {
                if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazard")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazard")))
                {
                    GameObject bloodParticles = Instantiate(hazardVfx, transform.position, transform.rotation);
                }

                myAudioSource.PlayOneShot(hazardDeadClip);
                myRigidBody2D.velocity = deathKick;

                // Disparar animacion de muerte y quitar vida al player (Para todos los muertes) 
                isAlive = false;
                myAnimator.SetTrigger("Dead");
                StartCoroutine(TakePLayerLifeAndRestoreLvl());
                return;
            }
            // CONTACTO: Boss y Spike
            else if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("Boss", "Spikes")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Boss", "Spikes")))
            {
                // Si tengo armadura
                if (armor >= 0)
                {
                    if (!invulnerable)
                    {
                        // Reproducir sonido indicado entre hazard y boss
                        if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("Spikes")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Spikes")))
                        {
                            myAudioSource.PlayOneShot(spikesClip);
                        }
                        else if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("Boss")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Boss")))
                        {
                            myRigidBody2D.velocity = deathKick;
                            myAudioSource.PlayOneShot(swordClashClip);
                        }
                        armor--;
                        FindObjectOfType<GameSesion>().TakeArmor();
                        invulnerable = true;
                        return;
                    }
                    else if (invulnerable)
                    {
                        return;
                    }
                }

                // Boss y Spike al tener un contacto sin armadura
                GameObject bloodParticles = Instantiate(hazardVfx, transform.position, transform.rotation);
                myAudioSource.PlayOneShot(hazardDeadClip);
                myRigidBody2D.velocity = deathKick;

                // Disparar animacion de muerte y quitar vida al player (Para todos los muertes) 
                isAlive = false;
                myAnimator.SetTrigger("Dead");
                StartCoroutine(TakePLayerLifeAndRestoreLvl());
                return;
            }            
        }
    }

    private IEnumerator TakePLayerLifeAndRestoreLvl()
    {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<GameSesion>().TakePlayerLife();
    }

    private void ShootArrow()
    {
        if (myAnimator.GetBool("Shoot")) { return; }
        else if(Time.timeScale == 0) { return; }
        if (Input.GetButtonDown("Fire1"))
        {
            if (FindObjectOfType<GameSesion>().HasArrows())
            {
                // Direccion de la flecha (Posicion del mouse)
                Vector3 shootingDirection = FindObjectOfType<Camera>().ScreenToWorldPoint(Input.mousePosition);
                shootingDirection.z = 0f;

                // Revisar si la direccion es frontal al personaje
                if (Mathf.Sign(transform.localScale.x) > Mathf.Epsilon && transform.position.x > shootingDirection.x)
                {
                    return;
                }
                else if (Mathf.Sign(transform.localScale.x) < Mathf.Epsilon && transform.position.x < shootingDirection.x)
                {
                    return;
                }

                //Activar animacion de disparo
                myAnimator.SetBool("Shoot", true);
                // Consumir Flecha
                FindObjectOfType<GameSesion>().SpendArrow();
                // sonido disparo
                myAudioSource.PlayOneShot(shootClip);
                // Instanciar la flecha 
                Arrow arrowShooted = Instantiate(arrow,gun.transform.position, Quaternion.identity);              
                // Darle direccion y velocidad
                Vector2 direction = shootingDirection - arrowShooted.transform.position;
                arrowShooted.transform.up = direction;
                arrowShooted.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;
                // Si la flecha llego a la posicion objetivo se destruye
                arrowShooted.AddTargetPosition(shootingDirection);
            }
        }
    }

    private void ShootArrowAndRope()
    {
        // chequear si no estas disparando en estos momentos
        if (!myAnimator.GetBool("Shoot") == false) { return; }
        // chequear si el GamplingHook esta activado
        if (distanceJoint2D.enabled == true) { return; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Direccion de la flecha (Posicion del mouse)
            Vector3 shootingDirection = FindObjectOfType<Camera>().ScreenToWorldPoint(Input.mousePosition);
            shootingDirection.z = 0f;

            // Revisar si la direccion es frontal al personaje
            if (Mathf.Sign(transform.localScale.x) > Mathf.Epsilon && transform.position.x > shootingDirection.x)
            {
                return;
            }
            else if (Mathf.Sign(transform.localScale.x) < Mathf.Epsilon && transform.position.x < shootingDirection.x)
            {
                return;
            }

            //Activar animacion de disparo          
            myAnimator.SetBool("Shoot", true);
            // sonido disparo
            myAudioSource.PlayOneShot(shootClip);
            // Instanciar la flecha con soga
            ArrowRope arrowShooted = Instantiate(arrowWithRope,gun.transform.position, Quaternion.identity);
            // Darle direccion y velocidad
            Vector2 direction = shootingDirection - arrowShooted.transform.position;
            arrowShooted.transform.up = direction;
            arrowShooted.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;
            // Si la flecha llego a la posicion objetivo se destruye
            arrowShooted.AddTargetPosition(shootingDirection);
        }
    }

    public void Hook(Vector3 targetPosition)
    {
        //Sonido cuerda
        myAudioSource.PlayOneShot(ropeClip);
        // Activar el hook y dar un objetivo al joint.
        targetJoint2D.enabled = true;
        targetJoint2D.target = targetPosition;
        // activar la "cuerda" y darle un origen y final.
        line.enabled = true;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, targetPosition);
    }

    private void DropHook()
    {
        // Seguir dibujando el comienzo (elemento 0) de la linea cuando estas en movimiento
        if (targetJoint2D.enabled == true)
        {
            line.SetPosition(0, transform.position);
        }
        // Romper el hook si llegaste a la posicion de destino
        if (Vector2.Distance(transform.position, targetJoint2D.target) < 0.2f)
        {
            targetJoint2D.enabled = false;
            line.enabled = false;
        }
        if (!isAlive)
        {
            targetJoint2D.enabled = false;
            line.enabled = false;
        }
    }
    // llamado mediante un event al terminar la animacion "Shoot" 
    public void ShootAnimatonOff()
    {
        myAnimator.SetBool("Shoot", false);
    }
    // llamado por la flecha al colisionar con un GamplingHookTarget
    public void GamplingHook(Vector3 targetPosition)
    {
        //Sonido cuerda
        myAudioSource.PlayOneShot(ropeClip);

        distanceJoint2D.enabled = true;
        distanceJoint2D.connectedAnchor = targetPosition;

        line.enabled = true;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, targetPosition);
    }

    private void DropGamplingHook()
    {
        // Seguir dibujando el comienzo (elemento 0) de la linea cuando estas en movimiento
        if (distanceJoint2D.enabled == true)
        {
            line.SetPosition(0, transform.position);

            GamplingHookMovement();           
        }
        if (Input.GetKeyDown(KeyCode.E) && distanceJoint2D.enabled == true)
        {
            distanceJoint2D.enabled = false;
            line.enabled = false;
        }
        if (!isAlive)
        {
            distanceJoint2D.enabled = false;
            line.enabled = false;
        }
    }

    private void GamplingHookMovement()
    {
        // Cambiar la velocidad si se introduce algun input
        if (transform.localScale.x > 0 && Input.GetAxis("Horizontal") != 0)
        {
            float inputValue = Mathf.Sign(Input.GetAxis("Horizontal"));
            Vector2 gamplingVelocity = new Vector2(inputValue, 0) * gamplingSpeed * Time.deltaTime;
            myRigidBody2D.velocity += gamplingVelocity;
        }
        else if (transform.localScale.x < 0 && Input.GetAxis("Horizontal") != 0)
        {
            float inputValue = Mathf.Sign(Input.GetAxis("Horizontal"));
            Vector2 gamplingVelocity = new Vector2(inputValue, 0) * gamplingSpeed * Time.deltaTime;
            myRigidBody2D.velocity += gamplingVelocity;
        }
        // Reducir la velocidad gradualmente si no se tiene ningun input
        else if(transform.localScale.x > 0 && Input.GetAxis("Horizontal") == 0)
        {
            print("Reduc 1");
            Vector2 gamplingVelocity = new Vector2(-gamplingSpeedReduction, 0) * gamplingSpeed * Time.deltaTime;
            myRigidBody2D.velocity += gamplingVelocity;
        }
        else if (transform.localScale.x < 0 && Input.GetAxis("Horizontal") == 0)
        {
            print("Reduc 2");

            Vector2 gamplingVelocity = new Vector2(gamplingSpeedReduction, 0) * gamplingSpeed * Time.deltaTime;
            myRigidBody2D.velocity += gamplingVelocity;
        }
        
        // Detener el movimiento si se esta casi estatico.
        if (myRigidBody2D.velocity.x < 0.1 && myRigidBody2D.velocity.x > -0.1 && Input.GetAxis("Horizontal") == 0)
        {
            myRigidBody2D.velocity = new Vector2(0, 0);                       
        }
    }
}
