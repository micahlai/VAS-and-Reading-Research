using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Linq;

public class gameManager : MonoBehaviour
{
    public Text text;
    public Vector2 sampleLength = new Vector2(3, 7);
    public bool sentence = false;
    [Header("Timing")]
    public float timeInterval = 5000f;
    public float timeShown = 500f;
    float timeState = 0;
    bool textShown = false;
    [Space]
    public TextAsset sentences;
    string[] sentenceSample;
    bool startText = false;
    string[] lastSentenceSplice;
    [Header("Check")]
    public InputField check;
    bool inputting = false;

    //no repeats
    List<int> IDshown = new List<int>();

    // Start is called before the first frame update
    void Awake()
    {
      
        sentenceSample = sentences.text.Split(";\n");
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputting)
        {
            timeState += Time.deltaTime;
        }
        if (textShown)
        {
            if (timeState >= timeShown/1000)
            {
                textShown = false;
                ClearText();
                check.gameObject.SetActive(true);
                check.Select();
                inputting = true;
                timeState = 0;
            }
        } else if (timeState >= timeInterval/1000)
        {
            textShown = true;
            startText = true;
            timeState = 0;
        }
        if (startText)
        {
            startText = false;
            ShowText();
        }
        check.gameObject.SetActive(inputting);
        if (inputting)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                inputting = false;
                check.gameObject.SetActive(false);
                Debug.Log(CheckAccuracy(check.text));
                check.text = "";


            }
        }
        


        
    }
    void SendResultData()
    {

    }
    bool CheckAccuracy(string inputText)
    {
        string given = RemovePunctuation(RemoveWhitespacesUsingLinq(lastSentenceSplice[1].ToLower()));
        string inputted = RemovePunctuation(RemoveWhitespacesUsingLinq(inputText.ToLower()));

        Debug.Log(given);
        Debug.Log(inputted);

        bool gotCorrect = (given == inputted);

        return gotCorrect;
    }
    void ClearText()
    {
        text.text = "";

    }
    void ShowText()
    {
        int sentenceID = Random.Range(0, sentenceSample.Length);
        IDshown.Add(sentenceID);
        string[] textSplice = sentenceSample[sentenceID].Split(',');
        text.text = textSplice[1];
        //Debug.Log(int.Parse(textSplice[0]));
        lastSentenceSplice = textSplice;
        Debug.Log(textSplice.ToString());
        Debug.Log(textSplice[1].Split(' ').Length - 1);
    }
    public static string RemoveWhitespacesUsingLinq(string source)
    {
        return new string(source.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }
    public static string RemovePunctuation(string source)
    {
        return new string(source.Where(c => !char.IsPunctuation(c)).ToArray());
    }
}
