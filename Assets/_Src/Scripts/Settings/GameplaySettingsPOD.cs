using System;
using UnityEngine;

[Serializable]
public struct GameplaySettingsPOD
{
    public bool IsLeftPadAIControlled;
    public bool IsRightPadAIControlled; 
    public int RoundCnt;
    public float PadSpd;
    public float BallSpd;
    public int ResolutionIndex;
    public float AspectRatio;
    public bool IsFullScreen;

    public GameplaySettingsPOD(bool leftPadAI, bool rightPadAI, int roundCnt, int padSpd, int ballSpd, int resolutionIndex, float aspectRatio, bool isFullScreen)
    {
        IsLeftPadAIControlled = leftPadAI;
        IsRightPadAIControlled = rightPadAI;
        RoundCnt = roundCnt;
        PadSpd = padSpd;
        BallSpd = ballSpd;
        ResolutionIndex = resolutionIndex;
        AspectRatio = aspectRatio;
        IsFullScreen = isFullScreen;
    }

    // @todo Perhaps add a save/load from file Config?
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void FromJson(string jsonContents)
    {
        JsonUtility.FromJsonOverwrite(jsonContents, this);
    }

}


