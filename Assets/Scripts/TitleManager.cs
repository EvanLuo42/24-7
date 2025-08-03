using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button startGameButton;
    public Button optionsButton;
    public Button quitGameButton;
    public Button itchButton;
    public Button xButton;

    private void Start()
    {
        startGameButton.onClick.AddListener(OnClickStartGame);
        optionsButton.onClick.AddListener(OnClickOptions);
        quitGameButton.onClick.AddListener(OnClickQuit);
        itchButton.onClick.AddListener(OnClickItch);
        xButton.onClick.AddListener(OnClickX);
    }

    private static void OnClickStartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    private void OnClickOptions()
    {
        
    }
    
    private void OnClickQuit()
    {
        Application.Quit();
    }
    
    private void OnClickItch()
    {
        Application.OpenURL("https://itch.io");
    }
    
    private void OnClickX()
    {
        Application.OpenURL("https://x.com");
    }
}
