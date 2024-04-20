using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.controls.UI.Enable();
    }

    private void OnDisable()
    {
        InputManager.controls.UI.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        InputManager.controls.UI.Pause.performed += Pause;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Pause(InputAction.CallbackContext ctx)
    {
        bool isLoaded = SceneManager.GetSceneByBuildIndex(1).isLoaded;
        if (!isLoaded)
        {
            FindObjectOfType<PlayerInputHandler>().GetComponent<PlayerInputHandler>().enabled = false;
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
        else
        {
            FindObjectOfType<PlayerInputHandler>().GetComponent<PlayerInputHandler>().enabled = true;
            SceneManager.UnloadSceneAsync(1);
        }
    }
}
