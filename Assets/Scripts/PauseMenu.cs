using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
        
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    
}
