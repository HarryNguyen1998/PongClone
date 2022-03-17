using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField] GameState _menuToSwitchTo;

    void Start()
    {
#if UNITY_WEBGL
        if (_menuToSwitchTo == GameState.kQuit)
            gameObject.SetActive(false);
#endif
    }

    public void ChangeMenu()
    {
        GameManager.Instance.ChangeState(_menuToSwitchTo);
    }

    public void FromMainMenuBtn()
    {
        UIManager.Instance.FromMainMenuBtn();
    }
}
