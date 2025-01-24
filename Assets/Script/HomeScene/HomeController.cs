using UnityEngine;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    public Sprite ActiveButton;
    public Sprite FinishedButton;
    public Sprite LockButton;

    public GameObject Hint1;
    public GameObject Hint2;
    public GameObject Hint3;

    public GameObject Game1;
    public GameObject Game2;
    public GameObject Game3;

    private GameObject[] hints;
    private GameObject[] games;

    void Start()
    {
        // Initialize arrays for easier access
        hints = new GameObject[] { Hint1, Hint2, Hint3 };
        games = new GameObject[] { Game1, Game2, Game3 };

        var userProfile = GameManagerController.Instance.GetCurrentUser();
        UpdateUIBasedOnLevel(userProfile.Level);
    }

    private void UpdateUIBasedOnLevel(Levels currentLevel)
    {
        // Determine which levels are active, locked, or completed
        for (int i = 0; i < hints.Length; i++)
        {
            if ((int)currentLevel > i * 2 + 1) // Finished levels
            {
                SetButtonState(hints[i], FinishedButton);
                SetButtonState(games[i], FinishedButton);
            }
            else if ((int)currentLevel == i * 2 + 1) // Active level
            {
                SetButtonState(hints[i], ActiveButton);
                SetButtonState(games[i], LockButton);
            }
            else if ((int)currentLevel == i * 2 + 2) // Active game
            {
                SetButtonState(hints[i], ActiveButton);
                SetButtonState(games[i], ActiveButton);
            }
            else // Locked levels
            {
                SetButtonState(hints[i], LockButton);
                SetButtonState(games[i], LockButton);
            }
        }
    }

    private void SetButtonState(GameObject buttonObject, Sprite stateSprite)
    {
        if (buttonObject != null)
        {
            buttonObject.GetComponent<Image>().sprite = stateSprite;
        }
    }
}
