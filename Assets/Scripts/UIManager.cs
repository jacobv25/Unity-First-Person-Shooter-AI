using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public NPC[] npcs; // assign all NPCs you want to track in the inspector
    public TextMeshProUGUI npcListText; // assign a Text UI element in the inspector

    void Update()
    {
        npcListText.text = ""; // reset the text each frame
        foreach (NPC npc in npcs)
        {
            // add each NPC's state to the text
            npcListText.text += npc.gameObject.name + ": " + npc.CurrentState.GetType().Name + "\n";
        }
    }
}
