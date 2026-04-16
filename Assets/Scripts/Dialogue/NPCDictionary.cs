using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDictionary : MonoBehaviour
{
    public static NPCDictionary Instance { get; private set; }
    public List<NPC> NPCObjects;
    private Dictionary<string, bool> SpokenTo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SpokenTo = new Dictionary<string, bool>();

        //at game start no npcs have been spoken to
        for(int i=0; i<NPCObjects.Count; i++)
        {
            SpokenTo.Add(NPCObjects[i].npcID, false);
            Debug.LogWarning($"NPC {NPCObjects[i].npcID} added to dictionary");
        }
    }

    //mark an npc as spoken to
    public void MarkSpokenTo(string NPC_ID)
    {
        //gets called only after dialogue ends
        SpokenTo[NPC_ID] = true;
        Debug.Log("Marked spoken to for " + NPC_ID + "state: " + SpokenTo[NPC_ID]);
    } 


    //query whether npc has been spoken to
    public bool QueryNPCState(string NPC_ID)
    {
        bool wasFound = SpokenTo.TryGetValue(NPC_ID, out bool state);
        if (wasFound == false) {
            Debug.LogWarning($"NPC with ID {NPC_ID} not found in dictionary");
            return false;
        }
        else
        {
            return state;
        }
    }
}
