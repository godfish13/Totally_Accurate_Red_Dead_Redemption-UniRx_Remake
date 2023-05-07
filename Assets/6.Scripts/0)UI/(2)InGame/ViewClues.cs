using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class ViewClues : MonoBehaviour
{
    private PlayerStatus playerStatus;

    public GameObject CluePanel;
    public GameObject CluePanelClosed;
    private bool CluePanelisOpen;

    public GameObject Clue1;
    public GameObject Clue2;
    public GameObject Clue3;

    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<PlayerStatus>();
        CluePanelisOpen = false;

        this.UpdateAsObservable()
            .Subscribe(_ => CluePanelControl());

        playerStatus.KillScoreObservable
            .Where(KillScore => KillScore == 2)
            .Subscribe(x =>
            {
                if (x < 2)
                {
                    Clue1.GetComponent<Text>().text = "Need " + (2 - x) + " more kill to see clue";
                }
                else
                {
                    Clue1.GetComponent<Text>().text = "Betrayer has a Mustache!";
                }
                if (x < 4)
                {
                    Clue2.GetComponent<Text>().text = "Need " + (4 - x) + " more kill to see clue";
                }
                else
                {
                    Clue2.GetComponent<Text>().text = "Betrayer dont wear a Hat!!";
                }
                if (x < 6)
                {
                    Clue3.GetComponent<Text>().text = "Need " + (6 - x) + " more kill to see clue";
                }
                else
                {
                    Clue3.GetComponent<Text>().text = "Betrayer's hair is Long!!!";
                }
            });
    }

    private void CluePanelControl()
    {
        if (Input.GetButtonDown("Clue"))
        {
            if (CluePanelisOpen == false)
            {
                CluePanelOpen();
            }
            else
            {
                CluePanelClose();
            }
        }
    }

    private void CluePanelOpen()
    {
        CluePanel.SetActive(true);
        CluePanelClosed.SetActive(false);
        CluePanelisOpen = true;
    }

    private void CluePanelClose()
    {
        CluePanel.SetActive(false);
        CluePanelClosed.SetActive(true);
        CluePanelisOpen = false;
    }
}
