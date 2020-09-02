using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//I'll make this class better later 
public class ErrorManager : MonoBehaviour
{
    public static ErrorManager Instance {get; private set;}

    public Text _textBox;

    private bool _started;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        if (_textBox == null)
            _textBox = this.GetComponent<Text>();

    }
    private void Start()
    {
        DisplayError("TEST");
    }

    public bool DisplayError(string errormsg)
    {
        if (_textBox && !_started)
            StartCoroutine(TextDelay(errormsg));


        Debug.LogError(errormsg);
        return false;
    }
    IEnumerator TextDelay(string errormsg)
    {
        _started = true;
        _textBox.gameObject.SetActive(true);
        _textBox.text = errormsg;
        yield return new WaitForSeconds(3);
        _textBox.gameObject.SetActive(false);
        _started = false;
    }

    
}
