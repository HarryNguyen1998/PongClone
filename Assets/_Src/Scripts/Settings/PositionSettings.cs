using System;
using UnityEngine;

[Serializable]
struct PositionData
{
    public Vector3 LeftPadPos;
    public Vector3 RightPadPos;
    public Vector3 LeftWallPos;
    public Vector3 RightWallPos;
}

public class PositionSettings : MonoBehaviour
{
    // References
    [SerializeField] Transform LeftWall;
    [SerializeField] Transform RightWall;
    [SerializeField] Transform LeftPad;
    [SerializeField] Transform RightPad;

    // Members
    [SerializeField] PositionData PositionData43;
    [SerializeField] PositionData PositionData169;

    void OnEnable()
    {
        SettingsDialog.ResolutionChanged += OnResolutionChanged;
    }

    void OnDisable()
    {
        SettingsDialog.ResolutionChanged -= OnResolutionChanged;
    }

    void OnResolutionChanged(float newRes)
    {
        if (Mathf.Approximately(newRes, 1.7777778f))
        {
            LeftWall.position = PositionData169.LeftWallPos;
            RightWall.position = PositionData169.RightWallPos;
            LeftPad.position = PositionData169.LeftPadPos;
            RightPad.position = PositionData169.RightPadPos;
        }
        else if (Mathf.Approximately(newRes, 1.3333333f))
        {
            LeftWall.position = PositionData43.LeftWallPos;
            RightWall.position = PositionData43.RightWallPos;
            LeftPad.position = PositionData43.LeftPadPos;
            RightPad.position = PositionData43.RightPadPos;
        }
        else
            Debug.Log($"Uh oh, Resolution not supported!");
    }

}
