using UnityEngine;
using System.Collections;

public class TitleMgr : MonoBehaviour 
{

    public GameObject _titleG, _titleK, _titleT, _titleM;
    private IslandMove _titleGMove, _titleKMove, _titleTMove, _titleMMove;

    void Awake()
    {
        _titleGMove = _titleG.GetComponent<IslandMove>();
        _titleKMove = _titleK.GetComponent<IslandMove>();
        _titleTMove = _titleT.GetComponent<IslandMove>();
        _titleMMove = _titleM.GetComponent<IslandMove>();
    }

    public void DisplayTitle()
    {
        _titleG.SetActive(true);
        _titleK.SetActive(true);
        _titleT.SetActive(true);
        _titleM.SetActive(true);

        _titleGMove.Enter();
        _titleKMove.Enter();
        _titleTMove.Enter();
        _titleMMove.Enter();
    }


    public void HideTitle()
    {
        _titleGMove.Exit(() => { SetDisable(_titleG); });
        _titleKMove.Exit(() => { SetDisable(_titleK); });
        _titleTMove.Exit(() => { SetDisable(_titleT); });
        _titleMMove.Exit(() => { SetDisable(_titleM); });
    }

    private void SetDisable(GameObject obj)
    {
        obj.SetActive(false);
    }
}
