using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("CONFIG PARAMS")]
    // Panel de controles
    [SerializeField] GameObject panel;

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }
    /*Llamado del menu de pause, en el cual el time scale es igual a 0, 
     * por tanto debe restaurarse el time scale a 1 y destruir el scene persist
     al igual que cuando se sale por un level exit*/
    public void LoadMainMenu()
    {
        if(Time.timeScale == 0) { Time.timeScale = 1;}
        FindObjectOfType<GameSesion>().ResetGameSesionAndLoadMenu();
        ScenePersist scenePersist = FindObjectOfType<ScenePersist>();
        if (scenePersist != null)
        {
            scenePersist.DestroyPersist();
        }
    }
    
    public void ActiveControlPanel()
    {
        panel.SetActive(true);
    }

    public void DeactiveControlPanel()
    {
        panel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadFinalLevel()
    {
        SceneManager.LoadScene(9);
    }
}

