using System;
using Common.GlobalFlow;
using TMPro;
using UnityEngine;
using Common.Tool;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _subtitle;

    private void Update()
    {
        _subtitle.color = _subtitle.color.Alpha(Mathf.Sin(Time.time) / 2f + .5f);
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(0))
        {
            SceneFlow.LoadScene(SceneFlow.EScene.Battle);
        }
    }
}