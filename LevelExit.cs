using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("CONFIG PARAMS")]
    [SerializeField] float colorConstant = 0.05f;
    [Header("AUDIO")]
    [SerializeField] AudioClip activateClip;
    [SerializeField] AudioClip loadLevelClip;
    [SerializeField] float clipsVolumen;
    [SerializeField] GameObject musicPlayer;

    // States
    private bool changeTransparency = false;
    private bool soundPlayed = false;

    //Cached References
    SpriteRenderer mySpriteRenderer;
    AudioSource myAudioSource;
    BoxCollider2D myBoxCollider2D;

    void Start()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(StartLoadNextScene());
    }

    void Update()
    {
        if (mySpriteRenderer.color.a >= 1) { return; }
        // si change transparency es true, activar turn on para hacer aparecer la puerta
        if (changeTransparency)
        {
            TurnOn();
        }
        
        // Reproducir el audioclip una sola vez luego de abrir la puerta
        if (mySpriteRenderer.color.a > 0 && !soundPlayed)
        {
            AudioSource.PlayClipAtPoint(activateClip, FindObjectOfType<Camera>().transform.position,clipsVolumen);
            soundPlayed = true;
        }
    }

    private void TurnOn()
    {
        mySpriteRenderer.color += new Color(0, 0, 0, colorConstant);
    }

    // Desactivar el sonido background(MusicPlayer)
    private IEnumerator StartLoadNextScene()
    {
        musicPlayer.SetActive(false);
        myAudioSource.PlayOneShot(loadLevelClip, 1);
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<GameSesion>().ResetTortchInSceneAndScore();
        ScenePersist scenePersist = FindObjectOfType<ScenePersist>();
        if (scenePersist != null)
        {
            scenePersist.DestroyPersist();
        }
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
    // Desaparecer la salida y desativar sus colldiers
    public void DeactivateLevelExit()
    {
        mySpriteRenderer.enabled = false;
        myBoxCollider2D.enabled = false;
        changeTransparency = false;
    }
    // Aparecer la salida y desativar sus colldiers
    public void ActivateLevelExit()
    {
        mySpriteRenderer.enabled = true;
        myBoxCollider2D.enabled = true;
        changeTransparency = true;
    }

    
}
