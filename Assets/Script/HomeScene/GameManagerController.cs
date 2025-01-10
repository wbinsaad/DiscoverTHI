using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;


public class GameManagerController : MonoBehaviour
{
    public static GameManagerController Instance { get; private set; }
    public static UserProfile UserProfile { get; private set; }

    private string path;

    public AudioClip BackGroundClip;

    public List<AudioClip> PlaySounds { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SoundController.Instance.PlaySoundLoop(BackGroundClip);

        path = Path.Combine(Application.persistentDataPath, "game.json");

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }

        UserProfile = GetCurrentUser();
        if (UserProfile == null) return;

    }

    public void UpdateUserProfile(Levels updatedLevel)
    {
        if (File.Exists(path))
        {
            UserProfile userProfile = new UserProfile("Waleed", updatedLevel);
            string userProfileJson = JsonUtility.ToJson(userProfile, true);
            File.WriteAllText(path, userProfileJson);
        }
    }

    public UserProfile GetCurrentUser()
    {
        if (File.Exists(path))
        {
            string ReadFile = File.ReadAllText(path);
            UserProfile userProfile = JsonUtility.FromJson<UserProfile>(ReadFile);
            return userProfile;
        }
        else
        {
            UserProfile userProfile = new UserProfile("Waleed", Levels.hint1);
            string userProfileJson = JsonUtility.ToJson(userProfile, true);
            File.WriteAllText(path, userProfileJson);
            return userProfile;
        }

    }

}
