using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows.Speech;
using System.Linq;

//Source URL : https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/voice-input-in-unity

public class SpeechInteraction : MonoBehaviour
{
    /*

    // Add a few fields to your class to store the recognizer and keyword->action dictionary
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();


    // Start is called before the first frame update
    void Start()
    {
        // Add keywords to the dictionary

        //Create keywords for keyword recognizer
        keywords.Add("activate", () =>
        {
            // action to be performed when this keyword is spoken
        });
        keywords.Add("deactivate", () =>
        {
            // action to be performed when this keyword is spoken
        });

        // Create the keyword recognizer and tell it what we want to recognize:
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register for the OnPhraseRecognized event
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        // Start recognizing
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
    */
}
