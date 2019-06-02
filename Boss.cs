using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Config Params
    [Header("CONFIG PARAMS")]
    [SerializeField] int bossLife = 5;
    [SerializeField] float stunTime = 10;
    [SerializeField] List<Spikes> Spikes;
    [SerializeField] float spikeSpeed = -3;
    [Header("AUDIO")]
    [SerializeField] AudioClip attackSwishClip;
    [SerializeField] float attackSwishClipVolumen;
    [SerializeField] AudioClip[] hurtClips;
    [SerializeField] float hurtClipVolumen;
    [SerializeField] AudioClip[] attackAudioClips;
    [SerializeField] float attackAudioClipsVolumen;

    private float stuneTimeAtStart;

    // States
    [SerializeField] bool vulnerable = false;
    
    // Cached References
    CircleCollider2D myCircleCollider2D;
    BoxCollider2D myBoxCollider2D;
    Animator myAnimator;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        stuneTimeAtStart = stunTime;
        myAnimator = GetComponent<Animator>();
        myCircleCollider2D = GetComponent<CircleCollider2D>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();

    }

    public int GetBossLife()
    {
        return bossLife;
    }

    // Update is called once per frame
    void Update()
    {
        Stuned();
        Die();
    }

    private void Die()
    {
        if(bossLife  <= 0)
        {            
            myAnimator.SetTrigger("Die");
            FindObjectOfType<GameSesion>().ActivateFinalPanel();
        }
    }

    private void Stuned()
    {
        if (vulnerable)
        {
            if(stunTime > 0)
            {
                stunTime -= Time.deltaTime;
            }
            else
            {
                ResetState(); 
            }

            if (myCircleCollider2D.IsTouchingLayers(LayerMask.GetMask("Arrow")))
            {
                int hurtCLipSelectd = UnityEngine.Random.Range(0, hurtClips.Length);
                AudioSource.PlayClipAtPoint(hurtClips[hurtCLipSelectd] , FindObjectOfType<Camera>().transform.position);
                bossLife--;
                ResetState();
            }
        }
    }

    private void ResetState()
    {
        if (bossLife > 0)
        {
            if (bossLife >= 3)
            {
                myAnimator.SetTrigger("Iddle");
            }
            else
            {
                myAnimator.SetTrigger("Hurt");
            }
            vulnerable = false;
            stunTime = stuneTimeAtStart;
            myBoxCollider2D.enabled = true;
            myCircleCollider2D.enabled = false;
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Rock")
        {
            int hurtCLipSelectd = UnityEngine.Random.Range(0, hurtClips.Length);
            AudioSource.PlayClipAtPoint(hurtClips[hurtCLipSelectd], FindObjectOfType<Camera>().transform.position, hurtClipVolumen);
            myBoxCollider2D.enabled = false;
            myCircleCollider2D.enabled = true;
            myAnimator.SetTrigger("Knee");
            vulnerable = true;
        }      
    }

    public void Attack()
    {
        myAudioSource.PlayOneShot(attackSwishClip, attackSwishClipVolumen);
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    public void PlayAttackClip()
    {
        int attackAudioClipSelected = UnityEngine.Random.Range(0, attackAudioClips.Length);
        AudioSource.PlayClipAtPoint(attackAudioClips[attackAudioClipSelected], FindObjectOfType<Camera>().transform.position, attackAudioClipsVolumen);
    }

    public void BackToIddle()
    {
        myAnimator.SetTrigger("Iddle");
    }

    public void AddSpike(Spikes spike)
    {
        Spikes.Add(spike);
    }

    public void SelectSpikeToDrop()
    {
        if(Spikes.Count <= 0) { return; }
        int spikeDroped = UnityEngine.Random.Range(0, Spikes.Count);
        Spikes[spikeDroped].DropSpike(spikeSpeed);
        Spikes.RemoveAt(spikeDroped);
    }
}
