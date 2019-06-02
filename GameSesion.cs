using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSesion : MonoBehaviour
{
    //Config params
    [Header("Config Params")]
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore;
    [SerializeField] int playerArrows;
    [SerializeField] List<Tortch> tortchInScene;
    [SerializeField] List<Tortch> secretDoorTortchInScene;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI textLives;
    [SerializeField] TextMeshProUGUI textCoins;
    [SerializeField] TextMeshProUGUI textArrows;
    [SerializeField] TextMeshProUGUI textArmor;
    [SerializeField] TextMeshProUGUI textFinalScore;
    [SerializeField] GameObject imageArmor;
    [SerializeField] GameObject imageCoin;
    [SerializeField] GameObject finalPanel;
    [SerializeField] GameObject pausePanel;

    private int playerArmor;
    private int coinsInScene = 3;
    private int totalCoinsTaked;
    // Antorchas encendidas
    private int tortchOn = 0;
    private int secretDoorTortchOn = 0;


    //Singleton
    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        textLives.text = playerLives.ToString();
        textCoins.text = playerScore.ToString();
        textArrows.text = playerArrows.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        ActivatePausePanel();
    }
    
    // Añadir monedas al puntaje del pj, si tienes 3 monedas aumentas 1 vida 
    public void AddToScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        totalCoinsTaked += scoreToAdd;

        textCoins.text = playerScore.ToString();
        // Si Obtuviste todas las monedas, Añade una vida.
        if(playerScore == coinsInScene)
        {
            AddPlayerLife();
        }
    }
    // añadir armadura y activar al UI de armadura
    public void AddArmor(int value)
    {
        playerArmor += value;

        if (imageCoin.activeInHierarchy == true)
        {
            imageCoin.SetActive(false);
        }
        if (imageArmor.activeInHierarchy == false)
        {
            imageArmor.SetActive(true);
        }

        textArmor.text = playerArmor.ToString();
    }
    // Restar armadura y actualizar el UI de armadura
    public void TakeArmor()
    {
        if(playerArmor <= 0)
        {
            return;
        }
        playerArmor--;
        textArmor.text = playerArmor.ToString();
    }
    
    // añadir flechas y actualizar el UI de flechas
    public void AddArrows(int arrowsToAdd)
    {
        playerArrows += arrowsToAdd;
        textArrows.text = playerArrows.ToString();
    }
    
    // Revisar si hay flechas
    public bool HasArrows()
    {        
        return playerArrows > 0;
    }
    
    // Gastar flecha y actualizar UI
    public void SpendArrow()
    {
        playerArrows--;
        textArrows.text = playerArrows.ToString();
    }
    
    // Añadir vidas al personaje y actualizar UI
    public void AddPlayerLife()
    {
        playerLives++;
        textLives.text = playerLives.ToString();
    }

    // Restar vida al player y si tiene 0 vidas, cargar el menu
    public void TakePlayerLife()
    {

        if (playerLives > 1)
        {
            TakeALife();         
        }
        else
        {
            ResetGameSesionAndLoadMenu();
        }
    }

    private void TakeALife()
    {
        playerLives--;
        playerArmor = 0;
        if(SceneManager.GetActiveScene().buildIndex >= 8)
        {
            playerArrows = 1;
            textArrows.text = playerArrows.ToString();
        }
        textLives.text = playerLives.ToString();
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void ResetGameSesionAndLoadMenu()
    {
        ScenePersist scenePersist = FindObjectOfType<ScenePersist>();
        if (scenePersist != null)
        {
            scenePersist.DestroyPersist();
        }
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    
    // Resetear el conteo de antorchas presentes en la escena(tortchInScene)
    public void ResetTortchInSceneAndScore()
    {
        //Debug.Log("Reset conteo de antorchas");
        tortchInScene.Clear();
        tortchOn = 0;

        secretDoorTortchInScene.Clear();
        secretDoorTortchOn = 0;

        playerScore = 0;
        textCoins.text = playerScore.ToString();
    }
    
    // Añadir antorchas al contador tortchInScene
    public void AddTortchInTheScene(Tortch tortch)
    {
        //Debug.Log("tortch");
        tortchInScene.Add(tortch);
        TurnLevelExitAndSecretDoor();
    }

    public void AddSecretDoorTortchInTheScene(Tortch tortch)
    {
       // Debug.Log("Secret tortch");
        secretDoorTortchInScene.Add(tortch);
        TurnLevelExitAndSecretDoor();
    }
    
    // Añadir una antorcha encendida
    public void TurnOnTortch()
    {
        tortchOn++;
        TurnLevelExitAndSecretDoor();
        //Debug.Log("tortch" + tortchOn + "/" + tortchInScene.Count);
    }
    
    // Añadir una antorcha encendida de puerta secreta
    public void TurnOnTortchSecretDoor()
    {
        secretDoorTortchOn++;
        TurnLevelExitAndSecretDoor();
        Debug.Log("Secret Tortch" + secretDoorTortchOn + "/" + secretDoorTortchInScene.Count);
    }
    
    // abrir puerta sereta o salida secreta
    private void TurnLevelExitAndSecretDoor()
    {
        SecretDoor secretDoor = FindObjectOfType<SecretDoor>();
        LevelExit levelExit = FindObjectOfType<LevelExit>();
        if (levelExit)
        {
           // print("hay salida");
            if (tortchOn >= tortchInScene.Count)
            {
               // print("aparece salida");
                levelExit.ActivateLevelExit();
            }
            else
            {
                levelExit.DeactivateLevelExit();
            }
        }
        if (secretDoor)
        {
           //print("Hay secret door");
            if (secretDoorTortchOn >= secretDoorTortchInScene.Count)
            {
                //print("Desaparece secret door");
                secretDoor.OpenSecretDoor();
            }
            else
            {
                secretDoor.CloseSecretDoor();
            }
        }
    }

    // Activar el panel de pause, y colocar el time scale en 0 para detener el tiempo
    private void ActivatePausePanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeInHierarchy == false)
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
    // activar el panel final luego de vencer al jefe
    public void ActivateFinalPanel()
    {
        if (pausePanel.activeInHierarchy == true)
        {
            pausePanel.SetActive(false);
        }

        StartCoroutine(LoadFinalPanel());
    }

    private IEnumerator LoadFinalPanel()
    {
        yield return new WaitForSeconds(1f);
        finalPanel.SetActive(true);
        textFinalScore.text = totalCoinsTaked.ToString();
        Time.timeScale = 0;
    }
}
