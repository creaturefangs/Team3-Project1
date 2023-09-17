using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objectives : MonoBehaviour
{
    private GameObject objectiveUI;
    private GameObject[] objectives;

    private void Start()
    {
        objectiveUI = transform.GetChild(0).GetChild(0).gameObject;
        GetObjectives();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetObjectives()
    {
        objectives = GameObject.FindGameObjectsWithTag("Objective");
        for (int i = 0; i < (objectives.Length); i++)
        {
            if (i == 0)
            {
                objectiveUI.transform.GetChild(1).GetComponent<TMP_Text>().text = $"- Get {objectives[i].name.ToUpper()}";
            }
            else
            {
                GameObject lastObj = objectiveUI.transform.GetChild(objectiveUI.transform.childCount - 1).gameObject;
                float yPos = lastObj.transform.position.y - lastObj.GetComponent<RectTransform>().sizeDelta.y;
                GameObject currentObj = Instantiate(lastObj, new Vector3(lastObj.transform.position.x, yPos, lastObj.transform.position.z), lastObj.transform.rotation, objectiveUI.transform);
                currentObj.GetComponent<TMP_Text>().text = $"- Get {objectives[i].name.ToUpper()}";
            }
        }
    }

    public void UpdateObjective(string name)
    {
        foreach (Transform item in objectiveUI.transform)
        {
            GameObject obj = item.gameObject;
            string text = obj.GetComponent<TMP_Text>().text;
            Regex re = new Regex(@$"{name.ToUpper()}");
            Regex completed = new Regex(@"<s>.+</s>"); // Checks that the objective is not already completed.
            if (re.IsMatch(text) && !completed.IsMatch(text))
            {
                obj.GetComponent<TMP_Text>().text = $"<s>{text}</s>"; // Puts strike-through on the objective text.
                break; // Breaks the foreach loop to prevent doubles of an objective being crossed out.
            }
        }
        CheckCompletion();
    }

    void CheckCompletion()
    {
        bool complete = true;
        foreach (Transform item in objectiveUI.transform)
        {
            GameObject obj = item.gameObject;
            string text = obj.GetComponent<TMP_Text>().text;
            Regex completed = new Regex(@"<s>.+</s>"); // Checks if the objective is completed.
            if (!completed.IsMatch(text) && obj.name != "TaskListTitle") { complete = false; break; }
        }
        if (complete)
        {
            GameObject lastObj = objectiveUI.transform.GetChild(objectiveUI.transform.childCount - 1).gameObject;
            float yPos = lastObj.transform.position.y - lastObj.GetComponent<RectTransform>().sizeDelta.y;
            GameObject currentObj = Instantiate(lastObj, new Vector3(lastObj.transform.position.x, yPos, lastObj.transform.position.z), lastObj.transform.rotation, objectiveUI.transform);
            currentObj.GetComponent<TMP_Text>().text = $"- Find the exit.";
        }
    }
}
