using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

[System.Serializable]
public class SaveHandler : MonoBehaviour
{
    PlayerInputHandler ih;
    string path;
    FPSController player;
    Dictionary<int, string> availibleGuns = new Dictionary<int, string>();
    private void Awake()
    {
        ih = FindObjectOfType<PlayerInputHandler>().GetComponent<PlayerInputHandler>();
        player = FindObjectOfType<FPSController>();
        path = Application.persistentDataPath + "/Save.json";
    }

    private void OnEnable()
    {
        ih.controls.Player.Save.performed += SavePlayerData;
        ih.controls.Player.Load.performed += LoadButton;
    }

    private void OnDisable()
    {
        ih.controls.Player.Save.performed -= SavePlayerData;
        ih.controls.Player.Load.performed -= LoadButton;
    }

    void LoadButton(InputAction.CallbackContext ctx)
    {
        BeginLoad();
    }
    void SavePlayerData(InputAction.CallbackContext ctx)
    {
        SavePlayerData();
    }

    public void SavePlayerData()
    {
        SaveData sd = new SaveData();
        sd.position = transform.position;
        sd.health = player.health;
        string jsonText = JsonUtility.ToJson(sd);
        File.WriteAllText(path, jsonText);
    }

    void BeginLoad()
    {
        LoadGame(LoadPlayerData());
    }

    SaveData LoadPlayerData()
    {
        try
        {
            string saveText = File.ReadAllText(path);
            SaveData myData = JsonUtility.FromJson<SaveData>(saveText);
            return myData;
        }
        catch (System.IO.FileNotFoundException)
        {
            Debug.Log("That file does not exist");
            return null;
        }
    }

    void LoadGame(SaveData myData)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = myData.position;
        player.GetComponent<CharacterController>().enabled = true;
        player.SetHealth(myData.health);
    }


}

[System.Serializable]
public class SaveData
{
    public float health;
    public Vector3 position;
}
