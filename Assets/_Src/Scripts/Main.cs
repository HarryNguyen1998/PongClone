using UnityEngine;

// Credits from https://low-scope.com/unity-tips-1-dont-use-your-first-scene-for-global-script-initialization/
public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        GameObject main = Instantiate(Resources.Load("Main")) as GameObject;
        DontDestroyOnLoad(main);
    }
}
