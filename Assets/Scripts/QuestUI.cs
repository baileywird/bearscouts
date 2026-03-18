using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;

    public Quest testQuest;
    public int testQuestAmount;
    private List<QuestProgress> testQuests = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < testQuestAmount; i++)
        {
            testQuests.Add(new QuestProgress(testQuest));
        }
        UpdateQuestUI();
    }

    // Update is called once per frame
    void UpdateQuestUI()
    {
        //destroy any existing quest entries
        foreach(Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        //build quest entries
        foreach(var quest in testQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");

            questNameText.text = quest.quest.name;

            foreach(var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})"; //collect 5 potions (0/5)
            }
        }
    }
}
