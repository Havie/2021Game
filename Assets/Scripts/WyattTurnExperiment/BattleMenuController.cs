using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleMenuController : MonoBehaviour
{
    // Components of a menu selection.
    // Each selection has a border, a button, and a text element.
    [SerializeField]
    private Image[] _borders = new Image[0];
    [SerializeField]
    private Button[] _buttons = new Button[0];
    [SerializeField]
    private TextMeshProUGUI[] _disTexts = new TextMeshProUGUI[0];

    // The sprites for the border images. Yellow is selected, red is default
    [SerializeField]
    private Sprite YELLOW_BORDER = null;
    [SerializeField]
    private Sprite RED_BORDER = null;

    // The current index selected
    private int _selIndex = 0;

    // Time to wait before changing selection again
    private const float INPUT_DELAY = 0.1f;
    // Current time waited
    private float _curTimer = INPUT_DELAY;

    // Called every frame
    private void Update()
    {
        // If the input timer is up, read for input
        if (_curTimer >= INPUT_DELAY)
        {
            // Read for menu input
            Vector2Int menuAxis = InputController.GetMenuAxis();
            if (menuAxis.y != 0)
            {
                int addAm = 0;
                if (menuAxis.y > 0)
                    addAm = _borders.Length - 1;
                else
                    addAm = 1;

                // Change the selection
                int newSel = (_selIndex + addAm) % _borders.Length;
                ChangeSelection(newSel);

                // Reset the timer
                _curTimer = 0;
            }
        }
        else
        {
            _curTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Selects the selection at the given index and deselects the other selections.
    /// </summary>
    /// <param name="_index_">Index to select.</param>
    public void ChangeSelection(int _index_)
    {
        // Change all of them to default
        // Sprite
        foreach (Image bor in _borders)
        {
            bor.sprite = RED_BORDER;
        }
        // Highlight
        foreach (Button but in _buttons)
        {
            ColorBlock butColors = but.colors;
            butColors.highlightedColor = ColorManager.Instance._buttonNormal;
            but.colors = butColors;
        }

        // Change the selected one to on
        // Sprite
        _borders[_index_].sprite = YELLOW_BORDER;
        // Highlight
        ColorBlock selButColors = _buttons[_index_].colors;
        selButColors.highlightedColor = ColorManager.Instance._buttonNormal;
        _buttons[_index_].colors = selButColors;

        _selIndex = _index_;
    }
}
