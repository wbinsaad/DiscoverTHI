using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public int totalPieces = 4;
    private int placedPieces = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckGameComplete()
    {
        placedPieces++;
        if (placedPieces >= totalPieces)
        {
            UIManager.Instance.ShowSuccessButton();
        }
    }

    public void ReloadScene()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    public void ChangeScnese(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnSuccessButtonClick()
    {
        Handheld.Vibrate();
        // GameManagerController.Instance.UpdateUserProfile(Levels.hint2);
        ChangeScnese("Hint2Scene");
    }
}
