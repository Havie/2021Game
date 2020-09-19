using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PortraitHolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Image _character;
    [SerializeField] Image _frame;

    private Sprite _alliedFrame;
    private Sprite _enemyFrame;
    private Sprite _alliedCFrame;
    private Sprite _enemyCFrame;

    private void Awake()
    {
        _alliedFrame = Resources.Load<Sprite>("UI/Battle/CharacterPortraits/Frame_ally");
        _enemyFrame = Resources.Load<Sprite>("UI/Battle/CharacterPortraits/Frame_enemy");
        _alliedCFrame = Resources.Load<Sprite>("UI/Battle/CharacterPortraits/FrameCurrentTurn_Allied");
        _enemyCFrame = Resources.Load<Sprite>("UI/Battle/CharacterPortraits/FrameCurrentTurn_Enemy");

        if (!_alliedFrame || !_alliedCFrame || !_enemyFrame || !_enemyCFrame)
            Debug.LogError("(PortraitHolder) Can't locate a Frame");
    }
    void Start()
    {
        //Find the sub children to cache 
    }

    public void SetPortait(Sprite portrait, bool isFriendly, bool isCurrent, string name)
    {
        _name.text = name;
        _character.sprite = portrait;
        DetermineFrame(isFriendly, isCurrent);
    }
    private void DetermineFrame(bool isFriendly, bool isCurrent)
    {
        if(isFriendly)
        {
            if (isCurrent)
                _frame.sprite = _alliedCFrame;
            else
                _frame.sprite = _alliedFrame;
        }
        else
        {
            if (isCurrent)
                _frame.sprite = _enemyCFrame;
            else
                _frame.sprite = _enemyFrame;
        }
    }

}
