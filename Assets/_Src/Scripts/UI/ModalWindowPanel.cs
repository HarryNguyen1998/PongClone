using UnityEngine;

public class ModalWindowPanel : MonoBehaviour
{
    void Start()
    {
        Close();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
