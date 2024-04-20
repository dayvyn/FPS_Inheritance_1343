using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public IA_Player controls { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        controls = new IA_Player();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new IA_Player();
        }
        controls.Enable();
    }

    private void OnDisable()
    {
        if (controls == null)
        {
            controls = new IA_Player();
        }
        controls.Disable();
    }
   
}
