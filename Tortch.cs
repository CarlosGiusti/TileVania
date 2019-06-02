using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tortch : MonoBehaviour
{
    // Config Params
    [Header("CONFIG PARAMS")]
    [SerializeField] bool secretDoorTortch;
    [Header("AUDIO")]
    [SerializeField] AudioClip tortchClip;
    [SerializeField] float torthcClipVolumen;
    
    // Cached References
    Animator myAnimator;
    GameSesion myGameSesion;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        StartCoroutine(AddTortchToGameSesion());       
    }

    // Añadir antorcha al contador de GameSesion
    // Se hace con una  coroutine para esperar a que el singleton del
    // gamesesion haga efecto y se añadan al gamesesion adecuado.
    private IEnumerator AddTortchToGameSesion()
    {
        yield return new WaitForEndOfFrame();
        // Añadir antorcha al contador de GameSesion
        if (secretDoorTortch)
        {
            FindObjectOfType<GameSesion>().AddSecretDoorTortchInTheScene(this);
        }
        else
        {
            FindObjectOfType<GameSesion>().AddTortchInTheScene(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        AddTortchOn();
    }

    private void AddTortchOn()
    {
        /*i mi collider esta tocando un VFX de fireball de particlesFireball
        activar la animacion de encendida, reproducir sonido y aumentar el 
        conteo en gamsesion de atorchas encendidas de secret door y level exit*/
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("FireballParticles")))
        {
            if (myAnimator.GetBool("TortchOn") == false)
            {
                myAnimator.SetBool("TortchOn", true);

                AudioSource.PlayClipAtPoint(tortchClip, FindObjectOfType<Camera>().transform.position, torthcClipVolumen);

                if (secretDoorTortch)
                {
                    FindObjectOfType<GameSesion>().TurnOnTortchSecretDoor();
                }
                else
                {
                    FindObjectOfType<GameSesion>().TurnOnTortch();
                }
            }
        }
    }
}
