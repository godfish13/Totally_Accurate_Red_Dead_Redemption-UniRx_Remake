using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaloonOpen : MonoBehaviour
{
    private PlayerStatus playerStatus;

    public GameObject SaloonDoor1;
    public GameObject SaloonDoor2;

    public GameObject Micha;
    public GameObject Dutch;
    public GameObject Charles;
    public GameObject John;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<PlayerStatus>();
        SaloonDoor1.SetActive(true);
        SaloonDoor2.SetActive(true);
        Micha.GetComponent<CapsuleCollider>().enabled = false;
        Dutch.GetComponent<CapsuleCollider>().enabled = false;
        Charles.GetComponent<CapsuleCollider>().enabled = false;
        John.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void OpenTheDoor()
    {
        SaloonDoor1.SetActive(false);
        SaloonDoor2.SetActive(false);
        Micha.GetComponent<CapsuleCollider>().enabled = true;
        Dutch.GetComponent<CapsuleCollider>().enabled = true;
        Charles.GetComponent<CapsuleCollider>().enabled = true;
        John.GetComponent<CapsuleCollider>().enabled = true;
    }
}
