using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    [SerializeField] private string playSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene(playSceneName, LoadSceneMode.Additive);    
    }

    public void QuitApplication()
    {
        Application.Quit();
    }   
}
