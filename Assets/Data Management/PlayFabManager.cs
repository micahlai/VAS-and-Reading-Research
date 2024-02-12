using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class PlayFabManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameInput;
    // Start is called before the first frame update
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
        messageText.text = "Registered";
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
    }
    void onError(PlayFabError error)
    {
        Debug.LogWarning("Error while logging in");
        Debug.LogWarning(error.GenerateErrorReport());
        messageText.text = error.GenerateErrorReport();
    }
}
