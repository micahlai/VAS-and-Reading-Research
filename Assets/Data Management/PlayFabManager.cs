using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager manager;

    public TextMeshProUGUI messageText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameInput;
    // Start is called before the first frame update

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        passwordInput.inputType = TMP_InputField.InputType.Password;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RegisterButton()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text.Remove(usernameInput.text.Length - 1),
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, onError);
    }
    void OnRegisterSuccess(RegisterPlayFabUserResult ressult)
    {
        messageText.text = "Registered " + ressult.Username;
        FindObjectOfType<startScreen>().showScenes();
    }
    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, onSuccess, onError);
    }
    void onSuccess(LoginResult loginResult)
    {
        Debug.Log("Login Success");
        messageText.text = "Log Success";
        FindObjectOfType<startScreen>().showScenes();
    }
    void onError(PlayFabError error)
    {
        Debug.LogWarning("Error while logging in");
        Debug.LogWarning(error.GenerateErrorReport());
        try
        {
            messageText.text = error.GenerateErrorReport();
        }
        catch
        {

        }
    }
    public void sendReadingData(gameManager.ReadingData readingData)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Reading Data", JsonConvert.SerializeObject(readingData) }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, onDataSend, onError);
    }
    public void sendVisualSearchData(shapeSpawner.VisualSearchData[] visualSearchData)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Visual Search Data", JsonConvert.SerializeObject(visualSearchData) }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, onDataSend, onError);
    }
    void onDataSend(UpdateUserDataResult result)
    {
        Debug.Log(result);
    }
    void sendShapeData()
    {

    }
}
