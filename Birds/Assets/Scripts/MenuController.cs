using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject _menuCanvas;
    [SerializeField] GameObject _levelSelectCanvas;
    public string _level;

    void Start()
    {
        _menuCanvas.SetActive(true);
        _levelSelectCanvas.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LevelSelect()
    {
        _menuCanvas.SetActive(false);
        _levelSelectCanvas.SetActive(true);
    }
    
    public void Back()
    {
        _levelSelectCanvas.SetActive(false);
        _menuCanvas.SetActive(true);
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene(this.name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
