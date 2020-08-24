using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButton : MonoBehaviour
{
    public Image _border;
    public Button _button;
    public TextMeshProUGUI _text;


    public static Sprite _borderSelected;
    public static Sprite _borderNormal;

    public bool _interactable;

    public bool _test;

    private void Awake()
    {
        //How are these not loading??????
        if(_borderSelected==null)
            _borderSelected = Resources.Load<Sprite>("UI/B3_yellow");
        if (_borderNormal==null)
            _borderNormal = Resources.Load<Sprite>("UI/B2_red");
    }
    void Start()
    {
        FindButtonAndText();
    }

    void LateUpdate()
    {
        //Test
        SetInteractable(_test);
        SetSelected(_interactable);
    }

    public void SetText(string text, bool interactable)
    {
        if(_text)
            _text.text = text;
        SetInteractable(interactable);
    }
    public void SetText(string text)
    {
        if (_text)
            _text.text = text;
    }
    public void SetInteractable(bool cond)
    {
        if (_text == null || _button==null)
            return;

        _interactable = _button.interactable = cond;

        if (cond)
            _text.color = ColorManager.Instance._menuValid;
        else
            _text.color = ColorManager.Instance._menuInvalid;

    }
    public void SetSelected(bool cond)
    {
        if (_button == null)
            return;

        //The current changes to color are barely noticable, but hopefully this can change and logic remains the same
        ColorBlock cb = _button.colors;
        if (cond)
        {
            cb.normalColor = ColorManager.Instance._buttonHighlighed;
            _border.sprite = _borderSelected;
        }
        else
        {
            cb.normalColor = ColorManager.Instance._buttonNormal;
            _border.sprite = _borderNormal;
        }
    }

    private void FindButtonAndText()
    {
        if (_border == null)
            _border = this.GetComponent<Image>();

        if (_button == null)
            _button = this.GetComponentInChildren<Button>();

        //HOW DOES THIS NOT WORK??
        /*
        if (_text = null)
            _text = this.GetComponentInChildren<TextMeshProUGUI>();
        else
            Debug.Log("Were good");
        */
        if(_text==null)
        {
            Debug.LogWarning("Somehow text is missing immedately after");
            Transform t = _button.transform.GetChild(0);
            var item = t.GetComponent<TextMeshProUGUI>();
            if (item)
                _text = item;
            else
                Debug.Log("Cant find here either??");
        }

        if (_border == null || _border == null || _text == null)
            Debug.LogWarning("Cant find Button components for " + this.gameObject.name);

    }

}
