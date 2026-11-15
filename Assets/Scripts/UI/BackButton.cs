using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public enum Action { Quit, LoadScene }

    [SerializeField] Action action;
    [SerializeField] string sceneName = "Menu";
    [SerializeField] GameObject closeFirst;   // optional: a panel to close before quitting

    void Update()
    {
        // Android's back button counts as the Escape key in the Input System
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;

        if (closeFirst != null && closeFirst.activeSelf) { closeFirst.SetActive(false); return; }

        if (action == Action.Quit) Application.Quit();
        else SceneManager.LoadScene(sceneName);
    }
}