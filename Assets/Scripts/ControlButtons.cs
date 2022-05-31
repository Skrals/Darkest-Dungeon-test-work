using UnityEngine;

public class ControlButtons : MonoBehaviour
{
    [SerializeField] GameObject _gameMenu;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _gameMenu.SetActive(!_gameMenu.activeSelf);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
