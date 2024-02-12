using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Linq;
using System;
using System.Text.RegularExpressions;

public class gameManager : MonoBehaviour
{
    public Text text;
    public Text statusText;
    public bool showStatusText = false;
    [Header("Timing")]
    public float timeInterval = 5000f;
    public float startTimeShown = 1500f;
    [Space]
    [Range(0, 100)]
    public float minSimilarity = 90f;
    public float timeReduction = 150f;
    public float timeAdd = 100f;
    [Space]
    float timeShown;
    float timeState = 0;
    bool textShown = false;
    [Space]
    public TextAsset sentences;
    string[] sentenceSample;
    bool startText = false;
    string[] lastSentenceSplice;
    List<int> sentencesShown;
    [Header("Check")]
    public InputField check;
    bool inputting = false;

    //no repeats
    List<int> IDshown = new List<int>();

    // Start is called before the first frame update
    void Awake()
    {
        text.text = "";
        statusText.text = "";
        sentenceSample = sentences.text.Split(";\n");
        timeShown = startTimeShown;
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
            statusText.text = "";
            ShowText();

        }
        check.gameObject.SetActive(inputting);
        if (inputting)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                inputting = false;
                check.gameObject.SetActive(false);

                string given = RemovePunctuation(RemoveWhitespacesUsingLinq(lastSentenceSplice[1].ToLower()));
                string inputted = RemovePunctuation(RemoveWhitespacesUsingLinq(check.text.ToLower()));

                bool correct = CheckAccuracy(inputted, given);
                double similarity = Mathf.Round((float)CheckSimilarity(inputted, given) * 10)/10;

                Debug.Log("Got correct: " + correct.ToString());
                Debug.Log("% Accuracy: " + similarity.ToString());

                if(CheckSimilarity(inputted,given) > minSimilarity)
                {
                    if (timeShown > timeReduction)
                    {
                        timeShown -= timeReduction;
                    }
                    else
                    {
                        timeShown = timeReduction;
                    }
                }
                else
                {
                    timeShown += timeAdd;
                }

                check.text = "";

                if (showStatusText)
                {
                    statusText.text = "Got correct: " + correct.ToString() + ", " + "% Accuracy: " + similarity.ToString() + ", " + "Time Interval (ms): " + timeShown.ToString();
                }

            }
        }
        


        
    }
    void SendResultData()
    {

    }
    bool CheckAccuracy(string inputText, string matchText)
    {
        bool gotCorrect = (matchText == inputText);

        return gotCorrect;
    }
    #region accuracy test
    public static double CheckSimilarity(string str1, string str2)
    {
        List<string> pairs1 = WordLetterPairs(str1.ToUpper());
        List<string> pairs2 = WordLetterPairs(str2.ToUpper());

        int intersection = 0;
        int union = pairs1.Count + pairs2.Count;

        for (int i = 0; i < pairs1.Count; i++)
        {
            for (int j = 0; j < pairs2.Count; j++)
            {
                if (pairs1[i] == pairs2[j])
                {
                    intersection++;
                    pairs2.RemoveAt(j);//Must remove the match to prevent "AAAA" from appearing to match "AA" with 100% success
                    break;
                }
            }
        }

        return (2.0 * intersection * 100) / union; //returns in percentage
        //return (2.0 * intersection) / union; //returns in score from 0 to 1
    }
    public static List<string> WordLetterPairs(string str)
    {
        List<string> AllPairs = new List<string>();

        // Tokenize the string and put the tokens/words into an array
        string[] Words = Regex.Split(str, @"\s");

        // For each word
        for (int w = 0; w < Words.Length; w++)
        {
            if (!string.IsNullOrEmpty(Words[w]))
            {
                // Find the pairs of characters
                string[] PairsInWord = LetterPairs(Words[w]);

                for (int p = 0; p < PairsInWord.Length; p++)
                {
                    AllPairs.Add(PairsInWord[p]);
                }
            }
        }
        return AllPairs;
    }

    // Generates an array containing every two consecutive letters in the input string
    public static string[] LetterPairs(string str)
    {
        int numPairs = str.Length - 1;
        string[] pairs = new string[numPairs];

        for (int i = 0; i < numPairs; i++)
        {
            pairs[i] = str.Substring(i, 2);
        }
        return pairs;
    }
    #endregion
    void ClearText()
    {
        text.text = "";

    }
    void ShowText()
    {
        int sentenceID = UnityEngine.Random.Range(0, sentenceSample.Length);
        while (IDshown.Contains(sentenceID))
        {
            sentenceID = UnityEngine.Random.Range(0, sentenceSample.Length);
        }
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
