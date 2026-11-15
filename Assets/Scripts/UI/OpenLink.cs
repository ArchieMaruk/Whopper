using UnityEngine;

public class OpenLink : MonoBehaviour
{
    [SerializeField] string url;

    public void Open() => Application.OpenURL(url);
}