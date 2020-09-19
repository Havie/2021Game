using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIBattleResult : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _resultText;
    [SerializeField] Button _returnButton;

    private void Awake()
    {
        if (_resultText == null)
            _resultText = GetComponentInChildren<TextMeshProUGUI>();
        if (_returnButton == null)
            _returnButton = GetComponentInChildren<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cEventSystem.OnBattleEnd += DisplayResult;
        EnableComponents(false);
    }

    private void OnDestroy()
    {
        cEventSystem.OnBattleEnd -= DisplayResult;
    }

    //TODO make fly in animations for this
    public void DisplayResult(bool PlayerWon)
    {
        if (PlayerWon)
        {
            _resultText.text = "Dark Elves Win";
            _resultText.color = ColorManager.Instance._textWin;
        }
        else
        {
            _resultText.text = "Dark Elves Lose";
            _resultText.color = ColorManager.Instance._textLose;
        }

        EnableComponents(true);
    }

    private void EnableComponents(bool cond)
    {
        _resultText.gameObject.SetActive(cond);
        _returnButton.gameObject.SetActive(cond);
    }
}
