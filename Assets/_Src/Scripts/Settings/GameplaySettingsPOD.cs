using System;
using UnityEngine;

[Serializable]
public class GameplaySettingsPOD
{
    [SerializeField]
    public bool IsLeftPadAIControlled;

    [SerializeField]
    public bool IsRightPadAIControlled;

    [SerializeField]
    public int RoundCnt;

    [SerializeField]
    public float PadSpd;

    [SerializeField]
    public float BallSpd;

    public int ResolutionIndex;

    public float AspectRatio;

    public bool IsFullScreen;

    public GameplaySettingsPOD(bool isLeftPadAI = true, bool isRightPadAI = false, int roundCnt = 3, float padSpd = 16, float ballSpd = 18, int resIndex = 2, float aspectRatio = 1.77778f, bool isFullScreen = false)
    {
        IsLeftPadAIControlled = isLeftPadAI;
        IsRightPadAIControlled = isRightPadAI;
        RoundCnt = roundCnt;
        PadSpd = padSpd;
        BallSpd = ballSpd;
        ResolutionIndex = resIndex;
        AspectRatio = aspectRatio;
        IsFullScreen = isFullScreen;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void FromJson(string jsonContents)
    {
        JsonUtility.FromJsonOverwrite(jsonContents, this);
    }

    public GameplaySettingsPOD DeepCopy()
    {
        GameplaySettingsPOD other = (GameplaySettingsPOD)MemberwiseClone();
        return other;
    }

}


