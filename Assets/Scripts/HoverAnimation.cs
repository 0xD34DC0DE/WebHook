using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject _latestSelection;
    private const int FontSizeSelected = 22;
    private const int DefaultFontSize = 20;
    
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (_latestSelection == pointerEventData.pointerCurrentRaycast.gameObject) return;
        _latestSelection = pointerEventData.pointerCurrentRaycast.gameObject;
        _latestSelection.GetComponent<Text>().fontSize = FontSizeSelected;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _latestSelection.GetComponent<Text>().fontSize = DefaultFontSize;
        _latestSelection = null;
    }
}
