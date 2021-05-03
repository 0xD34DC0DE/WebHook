using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MainMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _playButton;
    private Button _exitButton;
    private GameObject _latestSelection;
    private const int FontSizeSelected = 27;
    private const int DefaultFontSize = 21;

    private void Start()
    {
        GetComponents();
        SetListeners();
    }

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

    private void GetComponents()
    {
        _playButton = GameObject.Find("Play").GetComponent<Button>();
        _exitButton = GameObject.Find("Exit").GetComponent<Button>();
    }

    private void SetListeners()
    {
        _playButton.onClick.AddListener(Play);
        _exitButton.onClick.AddListener(Quit);
    }

    private void Play()
    {
        GameManager._instance.LoadScene1();
    }

    private void Quit()
    {
        Application.Quit();
    }
}