using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CompositeSelectionResponse : MonoBehaviour, ISelectionResponse, IChangeable 
{
    [SerializeField] private GameObject selectionResponseHolder;

    private List<ISelectionResponse> _selectionResponses;
    private int _currentIndex;
    private Transform _selection;
    private int playerScore = 0;
    public Text scoreText;


    private void Start()
    {
        _selectionResponses = selectionResponseHolder.GetComponents<ISelectionResponse>().ToList();
        scoreText.text = "0";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           if(_selection != null && _selection.GetComponent<Count>().type == _currentIndex)
            {
                playerScore += 500;
                Debug.Log(playerScore);
                scoreText.text = playerScore.ToString("0");
                _selection.position = new Vector3(0, -10, 0);
            } else if (_selection != null && _selection.GetComponent<Count>().type != _currentIndex)
            {
                playerScore += 100;
                Debug.Log(playerScore);
                scoreText.text = playerScore.ToString("0");
                _selection.position = new Vector3(0, -10, 0);
            }

        }
    }

    [ContextMenu(itemName:"Next")]
    public void Next()
    {
        _selectionResponses[_currentIndex].OnDeselect(_selection);  
        _currentIndex = (_currentIndex + 1) % _selectionResponses.Count;
        _selectionResponses[_currentIndex].OnSelect(_selection);
    }

    public void OnSelect(Transform selection)
    {
        _selection = selection;
        if(HasSelection())
        {
            _selectionResponses[_currentIndex].OnSelect(selection);
        }
    }

    public void OnDeselect(Transform selection)
    {
        _selection = null;
        if (HasSelection())
        {
            _selectionResponses[_currentIndex].OnDeselect(selection);
        }
    }

    private bool HasSelection()
    {
        return _currentIndex > -1 && _currentIndex < _selectionResponses.Count;
    }
}
