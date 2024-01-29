using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSwitch : MonoBehaviour
{
    public GameObject txtToDisplay;             //display the UI text
    public GameObject Fox;

    private bool PlayerInZone;                  //check if the player is in trigger

    public GameObject lightorobj;
    public CurtainsSwitch CurtainsSwitch;

    //Used with cues
    public bool isFoxOn = false;

    private void Start()
    {
        CurtainsSwitch = GameObject.Find("CurtainsButtonOpen").GetComponent<CurtainsSwitch>();
        PlayerInZone = false;                   //player not in zone       
        txtToDisplay.SetActive(false);
    }

    private void Update()
    {
        if (PlayerInZone && Input.GetKeyDown(KeyCode.E) && CurtainsSwitch.showStarted == true)           //if in zone and press E key
        {
            Fox.transform.position = new Vector3(-2.6f,0.71f,-6.8f);
            lightorobj.SetActive(!lightorobj.activeSelf);
            isFoxOn ^= true;
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<Animator>().Play("switch");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")     //if player in zone
        {
            txtToDisplay.SetActive(true);
            PlayerInZone = true;
        }
    }


    private void OnTriggerExit(Collider other)     //if player exit zone
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInZone = false;
            txtToDisplay.SetActive(false);
        }
    }
}
