using System;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [Header("Animation settings")]
    public Material LeftPadMat;
    public Material RightPadMat;
    public float AnimationCycle;

    [SerializeField] private GameplaySettingsPOD _default = new GameplaySettingsPOD(true, false, 3, 7.0f, 15.0f);
    public GameplaySettingsPOD DefaultSettings { get { return _default; } }

    public float WaitTimeBetweenEachRound
    {
        get
        {
            return AnimationCycle * 4;
        }
    }

}
