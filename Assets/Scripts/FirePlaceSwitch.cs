using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlaceSwitch : MonoBehaviour
{
    public GameObject txtToDisplay;             //display the UI text

    private bool PlayerInZone;                  //check if the player is in trigger

    public GameObject lightorobj;
    public CurtainsSwitch CurtainsSwitch;

    //Used with cues
    public bool isFirePlaceOn = false;

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
            lightorobj.SetActive(!lightorobj.activeSelf);
            isFirePlaceOn ^= true;
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
