using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteContent
{
    public List<string> paragraphs = new List<string>();

public void NotesConent()
    {
        paragraphs.Add(" It's been 300 years after the Fall. We never knew how much the surface has changed. We were attacked on our way to the city. If you find this note, beware of the darkness. There is a village ahead for you to take shelter in. Do not use flashlights or lanterns, the Beast will find you. Do your best to survive until daybreak.");
        paragraphs.Add(" Rick injured his leg while he was out looting. I'm making the morning rounds now, leaving the house as soon as the day breaks and heading back before the sun can set. Every day I have to travel farther and farther for less and less food. Fall comes, the days grow shorter. I think this is it.");
    }

}
