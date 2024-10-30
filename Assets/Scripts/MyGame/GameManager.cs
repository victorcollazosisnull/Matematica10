using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text timer;
    public int totalDianas = 10;
    private int dianasDestroys = 0;
    private float time = 25f;
    private bool finishGame = false;

    void Update()
    {
        if (!finishGame)
        {
            UpdateTimer();
        }
    }
    private void UpdateTimer()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timer.text = Mathf.Ceil(time).ToString();

            if (time <= 0)
            {
                End(false); 
            }
        }
    }
    public void DestroyDiana()
    {
        dianasDestroys = dianasDestroys + 1;
        if (dianasDestroys >= totalDianas)
        {
            End(true); 
        }
    }
    private void End (bool end)
    {
        finishGame = true;
        if (end && time > 0)
        {
            timer.text = "GG EZZ";
        }
        else
        {
            timer.text = "BAJAZO";
        }
    }
}