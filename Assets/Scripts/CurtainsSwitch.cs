using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainsSwitch : MonoBehaviour
{
    public GameObject txtToDisplay;             //display the UI text

    private bool PlayerInZone;                  //check if the player is in trigger

    public GameObject lightorobj;

    private bool areCurtainsOpen = false;
    public bool showStarted = false;

    //intro music
    public GameObject IntroMusic;

    private void Start()
    {

        PlayerInZone = false;                   //player not in zone       
        txtToDisplay.SetActive(false);
    }

    private void Update()
    {
        if (PlayerInZone && Input.GetKeyDown(KeyCode.E) && areCurtainsOpen == false)           //if in zone and press E key
        {
            showStarted = true;
            IntroMusic.SetActive(false);
            lightorobj.GetComponent<Animator>().Play("curtains");
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<Animator>().Play("switch");
            StartCoroutine(Wait());
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
        areCurtainsOpen = true;
        yield return new WaitForSeconds(4.0f);
        lightorobj.SetActive(!lightorobj.activeSelf);
    }
}
