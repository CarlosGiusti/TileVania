using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("CONFIG PARAMS")]
    [SerializeField] Boss boss;
    
    // Start is called before the first frame update
    void Start()
    {
        boss.AddSpike(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DropSpike(float spikeSpeed)
    {
        // Dejar un spike en su lugar y luego caer
        Spikes spike = Instantiate(this, transform.position, transform.rotation);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, spikeSpeed);
        Destroy(gameObject, 2);
    }
}
