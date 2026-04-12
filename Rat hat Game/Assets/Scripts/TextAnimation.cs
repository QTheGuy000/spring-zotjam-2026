using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

// Recreation of the TeleType script
public class TextAnimation : MonoBehaviour
{
    // The Text Component
    private TMP_Text textbox;
    // List of all the dialogues it will iterate through
    public List<string> dialogues = new(){};

    // Speed of revealing text
    [SerializeField] private float timeBtwnChars = 0.05f;
    [SerializeField] private float timeBtwnWords = 0.05f;
    
    void Awake(){
        textbox = GetComponent<TMP_Text>();
    }
    void Start(){
        
    }

    // Reveals the text over time
    public IEnumerator RevealText(){
        foreach (string dialogue in dialogues.ToList()){
            textbox.text = dialogue;
            // Ensures that the text doesn't shift due to line-wrapping
            textbox.ForceMeshUpdate();
            textbox.maxVisibleCharacters = 0;
            int totalChars = textbox.textInfo.characterCount;
            // Shows each character one at a time
            for (int c = 0; c < totalChars; c ++){
                textbox.maxVisibleCharacters = c + 1;
                // New words are indicated after spaces
                if (dialogue[c] == ' '){
                    yield return new WaitForSeconds(timeBtwnWords);
                }
                // New character
                else{
                    yield return new WaitForSeconds(timeBtwnChars);
                }
                
            }
        }
        // Reaching here means all dialogues have been finished
    }

    public void AddDialogue(string dialogue){
        dialogues.Add(dialogue);
    }

}
