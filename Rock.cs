using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [Header("CONFIG PARAMS")]
    [SerializeField] PickeableArrow pickeableArrow;
    [SerializeField] float dropVelocity;
    [SerializeField] float minPickeableArrowXSpeed;
    [SerializeField] float maxPickeableArrowXSpeed;
    [SerializeField] float minPickeableArrowYSpeed;
    [SerializeField] float maxPickeableArrowYSpeed;
    [Header("AUDIO & VFX")]
    [SerializeField] AudioClip destroyClip;
    [SerializeField] float rockPickUpSoundVolumen;
    [SerializeField] GameObject destroyVfx;

    // Cached References
    Rigidbody2D myRigidbody2D;
    AudioSource myAudioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "ArrowRope")
        {
            //Rock rock = Instantiate(this, transform.position, transform.rotation);
            myRigidbody2D.velocity = new Vector2(0f, dropVelocity);
        }
        else if(other.tag == "Boss")
        {
            PickeableArrow arrow = Instantiate(pickeableArrow, transform.position, transform.rotation);
            // Darle una velocidad random a la direccion del drop
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(minPickeableArrowXSpeed, maxPickeableArrowXSpeed), Random.Range(minPickeableArrowYSpeed, maxPickeableArrowYSpeed));
            AudioSource.PlayClipAtPoint(destroyClip, FindObjectOfType<Camera>().transform.position, rockPickUpSoundVolumen);
            GameObject destroyRockVfx = Instantiate(destroyVfx, transform.position, transform.rotation);
            Destroy(destroyRockVfx, 0.2f);
            Destroy(gameObject);
        }       
        else
        {
            GameObject destroyRockVfx = Instantiate(destroyVfx, transform.position, transform.rotation);
            Destroy(destroyRockVfx, 0.2f);
            AudioSource.PlayClipAtPoint(destroyClip, FindObjectOfType<Camera>().transform.position, rockPickUpSoundVolumen);
            Destroy(gameObject);
        }
    }
}
