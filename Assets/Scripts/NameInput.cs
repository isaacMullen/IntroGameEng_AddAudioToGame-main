using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameInput : MonoBehaviour
{
    //public GameObject nameInputFieldObject;
    public TMP_InputField nameInputField;
    public GameManager gameManager;

    private void Awake()
    {
        //nameInputFieldObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        nameInputField.onEndEdit.AddListener(SubmitName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SubmitName(string inputText)
    {
        Debug.Log("Entered Text: " + inputText);

        if (!string.IsNullOrEmpty(inputText))
        {
            Debug.Log($"Player's Name: {inputText}");
            gameManager.StartGame();
        }
        
    }
}
