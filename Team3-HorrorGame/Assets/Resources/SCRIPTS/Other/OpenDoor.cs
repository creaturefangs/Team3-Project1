using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OpenDoor : MonoBehaviour
{
    private Animator anim;
    private bool isAtDoor = false;
    [SerializeField] private TextMeshProUGUI CodeText;
    string codeTextValue = "";
    public string safeCode;
    public GameObject CodePanel;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CodeText.text = codeTextValue;

        if(codeTextValue == safeCode)
        {
            anim.SetTrigger("OpenDoor");
            CodePanel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(codeTextValue.Length >= 5)
        {
            codeTextValue = "";
        }

        if(Input.GetKey(KeyCode.E) && isAtDoor == true)
        {
            CodePanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GameController")
        {
            isAtDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isAtDoor = false;
        CodePanel.SetActive(false);
    }

    public void AddDigits(string digit)
    {
        codeTextValue += digit;
    }
}
