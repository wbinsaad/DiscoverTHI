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

    // Start is called before the first frame update
    void Start()
    {
        var userProfile = GameManagerController.Instance.GetCurrentUser();
        switch (userProfile.Level)
        {
            case Levels.hint1:
                Hint1.GetComponent<Image>().sprite = ActiveButton;
                Game1.GetComponent<Image>().sprite = LockButton;

                Hint2.GetComponent<Image>().sprite = LockButton;
                Game2.GetComponent<Image>().sprite = LockButton;

                Hint3.GetComponent<Image>().sprite = LockButton;
                Game3.GetComponent<Image>().sprite = LockButton;
                break;

            case Levels.game1:
                Hint1.GetComponent<Image>().sprite = ActiveButton;
                Game1.GetComponent<Image>().sprite = ActiveButton;

                Hint2.GetComponent<Image>().sprite = LockButton;
                Game2.GetComponent<Image>().sprite = LockButton;

                Hint3.GetComponent<Image>().sprite = LockButton;
                Game3.GetComponent<Image>().sprite = LockButton;
                break;

            case Levels.hint2:
                Hint1.GetComponent<Image>().sprite = ActiveButton;
                Game1.GetComponent<Image>().sprite = ActiveButton;

                Hint2.GetComponent<Image>().sprite = ActiveButton;
                Game2.GetComponent<Image>().sprite = LockButton;

                Hint3.GetComponent<Image>().sprite = LockButton;
                Game3.GetComponent<Image>().sprite = LockButton;
                break;

            case Levels.game2:
                Hint1.GetComponent<Image>().sprite = ActiveButton;
                Game1.GetComponent<Image>().sprite = ActiveButton;

                Hint2.GetComponent<Image>().sprite = ActiveButton;
                Game2.GetComponent<Image>().sprite = ActiveButton;

                Hint3.GetComponent<Image>().sprite = LockButton;
                Game3.GetComponent<Image>().sprite = LockButton;
                break;

            case Levels.hint3:
                Hint1.GetComponent<Image>().sprite = ActiveButton;
                Game1.GetComponent<Image>().sprite = ActiveButton;

                Hint2.GetComponent<Image>().sprite = ActiveButton;
                Game2.GetComponent<Image>().sprite = ActiveButton;

                Hint3.GetComponent<Image>().sprite = ActiveButton;
                Game3.GetComponent<Image>().sprite = LockButton;
                break;

            case Levels.game3:
                Hint1.GetComponent<Image>().sprite = ActiveButton;
                Game1.GetComponent<Image>().sprite = ActiveButton;

                Hint2.GetComponent<Image>().sprite = ActiveButton;
                Game2.GetComponent<Image>().sprite = ActiveButton;

                Hint3.GetComponent<Image>().sprite = ActiveButton;
                Game3.GetComponent<Image>().sprite = ActiveButton;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
