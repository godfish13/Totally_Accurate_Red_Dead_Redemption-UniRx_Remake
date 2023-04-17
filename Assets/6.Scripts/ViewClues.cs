using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewClues : MonoBehaviour
{
    private PlayerStatus playerStatus;

    public GameObject CluePanel;
    public GameObject CluePanelClosed;
    private bool CluePanelisOpen;

    public GameObject Clue1;
    public GameObject Clue2;
    public GameObject Clue3;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<PlayerStatus>();
        CluePanelisOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Clue"))
        {
            if(CluePanelisOpen == false)
            {
                CluePanelOpen();
            }
            else
            {
                CluePanelClose();
            }
        }
        if(playerStatus.KillScore < 2)
        {
            Clue1.GetComponent<Text>().text = "Need " + (2 - playerStatus.KillScore) +" more kill to see clue";
        }
        else
        {
            Clue1.GetComponent<Text>().text = "Betrayer has a Mustache!";
        }
        if (playerStatus.KillScore < 4)
        {
            Clue2.GetComponent<Text>().text = "Need " + (4 - playerStatus.KillScore) + " more kill to see clue";
        }
        else
        {
            Clue2.GetComponent<Text>().text = "Betrayer dont wear a Hat!!";
        }
        if (playerStatus.KillScore < 6)
        {
            Clue3.GetComponent<Text>().text = "Need " + (6 - playerStatus.KillScore) + " more kill to see clue";
        }
        else
        {
            Clue3.GetComponent<Text>().text = "Betrayer's hair is Long!!!";
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
