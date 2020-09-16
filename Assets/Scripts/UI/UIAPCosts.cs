using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAPCosts : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    private void Awake()
    {
        if(!_text)
         _text = this.GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        cEventSystem.OnHasMovementPrediction += UpdateCosts;
        cEventSystem.OnShowAPCostPrediction += ShowText;
        cEventSystem.OnHideAPCostPrediction += HideText;
    }
    private void OnEnable()
    {
        cEventSystem.OnHasMovementPrediction += UpdateCosts;
        cEventSystem.OnShowAPCostPrediction += ShowText;
        cEventSystem.OnHideAPCostPrediction += HideText;
    }
    private void OnDisable()
    {
        cEventSystem.OnHasMovementPrediction -= UpdateCosts;
        cEventSystem.OnShowAPCostPrediction -= ShowText;
        cEventSystem.OnHideAPCostPrediction -= HideText;
    }


    public void ShowText()
    {
        _text.enabled = true;
    }
    public void HideText()
    {
        _text.enabled = false;
    }

    public void UpdateCosts(int amount)
    {
        Playable activeChar = SelectionManager.Instance.GetActiveCharacter();
        if (activeChar)
        {
            DetermineValidColor(activeChar.GetCurrentAP() >= amount);
            _text.text = "AP: " + amount;
        }
    }

    private void DetermineValidColor(bool cond)
    {
        if (cond)
            _text.color = ColorManager.Instance._textValid;
        else
            _text.color = ColorManager.Instance._textInvalid;
    }
       

}
