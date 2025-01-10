using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject failPanel;
    public GameObject successPanel;
    public GameObject successButton;
    
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

    public void ShowFailPanel()
    {
        failPanel.SetActive(true);
    }

    public void ShowSuccessPanel()
    {
        successPanel.SetActive(true);
    }

    public void ShowSuccessButton()
    {
        successButton.SetActive(true);
    }

    
}