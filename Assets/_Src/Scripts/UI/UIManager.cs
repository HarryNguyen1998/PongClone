using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour
{
    [SerializeField] List<MenuData> _menus = new List<MenuData>();
    MenuData _lastActiveMenu;

    void Start()
    {
        _menus.ForEach(menu => menu.gameObject.SetActive(false));
        _lastActiveMenu = _menus.Find(menu => menu.MenuType == GameStateEventRelayer.Instance.PeekState());
        _lastActiveMenu.gameObject.SetActive(true);
    }

    void OnEnable()
    {
        GameStateEventHandler.Instance.GameStateChanged += SwitchTo;
    }

    void OnDisable()
    {
        GameStateEventHandler.Instance.GameStateChanged -= SwitchTo;
    }

    public void SwitchTo(GameState newMenu)
    {
        if (newMenu == GameState.kQuit)
            return;

        _lastActiveMenu.gameObject.SetActive(false);
        _lastActiveMenu = _menus.Find(menu => menu.MenuType == newMenu);
        _lastActiveMenu.gameObject.SetActive(true);
    }

}