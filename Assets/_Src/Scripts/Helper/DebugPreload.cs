#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A helper class that always load the preload scene 1st when you test in the editor.
/// </summary>
public static class DebugPreload
{
    public static int otherScene = -2;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadPreloadScene()
    {
#if UNITY_EDITOR
        if (EditorBuildSettings.scenes.Length == 0)
        {
            Debug.Log("Please set build settings first!");
            return;
        }

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0)
            return;
        otherScene = sceneIndex;
        Debug.Log("---------Loading preload scene---------");
        SceneManager.LoadScene(0);
#endif
    }
}
