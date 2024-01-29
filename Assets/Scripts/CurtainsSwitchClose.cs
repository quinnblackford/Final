using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainsSwitchClose : MonoBehaviour
{
    public GameObject txtToDisplay;             //display the UI text

    private bool PlayerInZone;                  //check if the player is in trigger

    //Reference to game objects
    public GameObject lightorobj;
    public GameObject CurtainsIdle;

    //Reference to other scripts
    public CurtainsSwitch CurtainsSwitch;

    //Bool for curtains being closed
    public bool areCurtainsClosed = false;

    private void Start()
    {
        CurtainsSwitch = GameObject.Find("CurtainsButtonOpen").GetComponent<CurtainsSwitch>();
        PlayerInZone = false;                   //player not in zone       
        txtToDisplay.SetActive(false);
    }

    private void Update()
    {
        if (PlayerInZone && Input.GetKeyDown(KeyCode.E) && areCurtainsClosed == false && CurtainsSwitch.showStarted == true)           //if in zone and press E key
        {
            lightorobj.GetComponent<Animator>().Play("curtainsClose");
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<Animator>().Play("switch");
            StartCoroutine(Wait());
        }
        else 
        {

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

    IEnumerator Wait()
    {
        areCurtainsClosed = true;
        yield return new WaitForSeconds(6.61f);
        CurtainsIdle.SetActive(true);
    }
}
