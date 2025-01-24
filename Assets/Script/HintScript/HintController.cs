using UnityEngine;
using UnityEngine.SceneManagement;

public class HintController : MonoBehaviour
{
    public GameObject HintObject;
    public string NextSceneName;
    public Levels NextLevel;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-8, 8), 0, UnityEngine.Random.Range(-8, 8));
        Vector3 cameraPosition = Camera.main.transform.position;

        Instantiate(HintObject, (randomOffset + cameraPosition), Quaternion.identity);
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

                if (hit.transform.tag == "HintObject")
                {
                    Handheld.Vibrate();
                    GameManagerController.Instance.UpdateUserProfile(NextLevel);
                    SceneManager.LoadScene(NextSceneName);
                }
            }
        }
    }
}