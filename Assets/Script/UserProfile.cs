

using System;

[Serializable]
public class UserProfile
{
    public string Name;
    public Levels Level;

    public UserProfile()
    {

    }

    public UserProfile(string Name, Levels Level)
    {
        this.Name = Name;
        this.Level = Level;
    }
}

[Serializable]
public enum Levels
{
    hint1,
    hint2,
    hint3,
    game1,
    game2,
    game3
}
