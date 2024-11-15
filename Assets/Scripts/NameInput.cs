using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameInput : MonoBehaviour
{
    //public GameObject nameInputFieldObject;
    public TMP_InputField nameInputField;
    public GameManager gameManager;

    public string playerName;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string SubmitName(string inputText)
    {
        
        

        if (!string.IsNullOrEmpty(inputText))
        {
            playerName = inputText;
            //Debug.Log($"Player's Name: {inputText}");
            
            gameManager.StartGame();
            
        }
        return playerName;  
    }
}
