using TMPro;
using UnityEngine;

public class Hint3Controller : MonoBehaviour
{
    public GameObject PrinterObject;
    public GameObject EndGamePopup;

    // private InputActionAsset _touch;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-8, 8), 0, UnityEngine.Random.Range(-8, 8));
        Vector3 cameraPosition = Camera.main.transform.position;

        Instantiate(PrinterObject, (randomOffset + cameraPosition), Quaternion.identity);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            UnityEngine.Touch touch = Input.GetTouch(0);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit = new RaycastHit();


            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag == "Printer")
                {
                    Handheld.Vibrate();
                    GameManagerController.Instance.UpdateUserProfile(Levels.game3);
                    EndGamePopup.SetActive(true);
                }
            }
        }
    }
}