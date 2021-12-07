using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the active state of each menu. Singleton only works when put into Preload scene.
/// </summary>
public class UIController : MonoBehaviour
{
    ModalWindowPanel _modalWindowPanel;
    List<MenuData> _menus = new List<MenuData>();
    MenuData _lastActiveMenu;

    public static UIController Instance { get; private set; }
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        GameManager.Instance.GameStateChanged += SwitchTo;
        SceneManager.sceneLoaded += InitMenus;
    }

    void OnDisable()
    {
        GameManager.Instance.GameStateChanged -= SwitchTo;
        SceneManager.sceneLoaded -= InitMenus;
    }

    public void InitMenus(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "PongClone")
            return;

        // @note This small delay makes it that menu's Start() won't be called until they become active!
        StartCoroutine(CO_WaitTillMenusAreInit());

        _menus.ForEach(menu => menu.gameObject.SetActive(false));
        _lastActiveMenu = _menus.Find(menu => menu.MenuType == GameManager.Instance.CurrentState);
        _lastActiveMenu.gameObject.SetActive(true);

        _modalWindowPanel.gameObject.SetActive(false);
    }

    IEnumerator CO_WaitTillMenusAreInit()
    {
        yield return null;
    }

    public void RegisterMenu(MenuData menu)
    {
        _menus.Add(menu);
    }

    public void DeregisterMenu(MenuData menu)
    {
        _menus.Remove(menu);
    }

    public void RegisterModalWindow(ModalWindowPanel modal)
    {
        _modalWindowPanel = modal;
    }

    public void DeregisterModalWindow()
    {
        _modalWindowPanel = null;
    }

    public void SwitchTo(GameState newMenu)
    {
        if (newMenu == GameState.kQuit)
            return;

        _lastActiveMenu.gameObject.SetActive(false);
        _lastActiveMenu = _menus.Find(menu => menu.MenuType == newMenu);
        _lastActiveMenu.gameObject.SetActive(true);
    }

    public void FromMainMenuBtn()
    {
        ShowApplyOrNo(GameState.kMainMenu);
    }

    public void ShowApplyOrNo(GameState newState)
    {
        Assert.IsTrue(_lastActiveMenu.MenuType == GameState.kSettingsMenu);
        _modalWindowPanel.MenuToSwitchBackTo = newState;
        _modalWindowPanel.TryShow();
    }

}