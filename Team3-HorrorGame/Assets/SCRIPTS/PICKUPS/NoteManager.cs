using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public List<NoteContent> notes = new List<NoteContent>();

    // Singleton instance
    public static NoteManager Instance { get; private set; }


    // list of note instances

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
