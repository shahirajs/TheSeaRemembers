using UnityEngine;

public class AutoLoadFromSlot : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPrefs.HasKey("LoadFromSlot"))
        {
            int slot = int.Parse(PlayerPrefs.GetString("LoadFromSlot"));
            SaveData data = SaveManager.LoadGame(slot);

            if (data != null)
            {
                DialogueProgressManager.ResumeLineIndex = data.dialogueLineIndex;
                PlayerPrefs.DeleteKey("LoadFromSlot");
            }
        }
    }
}
