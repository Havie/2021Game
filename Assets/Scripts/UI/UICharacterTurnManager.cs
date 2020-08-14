using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterTurnManager : MonoBehaviour
{
    static UICharacterTurnManager _instance;
    public GameObject _PREFABTurnOrder;

    private List<GameObject> _icons;
    private int _offset = 50;

    public static UICharacterTurnManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<UICharacterTurnManager>();
            return _instance;
        }
    }

    private void Awake()
    {

        if (_instance == null)
            _instance = this;
        else if(_instance!=this)
            Destroy(this);

        _PREFABTurnOrder = Resources.Load<GameObject>("Prefabs/UI/TurnOrderFrame");
        if (_PREFABTurnOrder == null)
            Debug.LogError("(UICharacterTurnManager) Prefab is Missing ");
    }
    private void Start()
    {
        _icons = new List<GameObject>();

    }
    private void Update()
    {
    }

    public void SetUpBar(List<GameObject> characters)
    {
        int index = 0;
        foreach (GameObject g in characters)
        {
            GameObject button = Instantiate(_PREFABTurnOrder);
            button.gameObject.transform.SetParent(this.transform);
            button.transform.localPosition = new Vector3(-120 + (_offset * index++), 50, 0);
            Debug.Log(button.transform.localPosition);
            button.transform.localRotation = Quaternion.identity; 
            button.transform.localScale = new Vector3(1, 1, 1);
            _icons.Add(button);
        }
    }
}
