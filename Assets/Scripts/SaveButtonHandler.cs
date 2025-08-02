using UnityEngine;

public class SaveButtonHandler : MonoBehaviour
{
    public int saveSlot = 0;
    public void Save() => SaveManager.SaveGame(saveSlot);
}
