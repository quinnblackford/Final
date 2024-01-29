using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetChangeSwitch : MonoBehaviour
{
    public GameObject txtToDisplay;             //display the UI text

    private bool PlayerInZone;                  //check if the player is in trigger

    public GameObject lightorobj;
    public CurtainsSwitch CurtainsSwitch;

    //Reference to the cue script
    public CueScript CueScript;
    public TextMeshProUGUI CueTextTwo;

    //Reference to set one and two
    public GameObject SetOne;
    public GameObject SetTwo;
    public GameObject ButtonOne;
    public GameObject ButtonTwo;
    public GameObject ButtonThree;
    public GameObject ButtonFour;
    public GameObject ButtonFive;
    public GameObject SetOneTexts;
    public GameObject SetOneIndicators;
    public GameObject SetTwoButtons;


    //Used with cues
    public bool isSetChangeOn = false;
    public bool setIsChanged = false;

    private void Start()
    {
        //References to other scripts
        CueScript = GameObject.Find("CueCaller").GetComponent<CueScript>();
        CurtainsSwitch = GameObject.Find("CurtainsButtonOpen").GetComponent<CurtainsSwitch>();

        PlayerInZone = false;                   //player not in zone       
        txtToDisplay.SetActive(false);
    }

    private void Update()
    {
        if (CueScript.totalCues == 15 && setIsChanged == false)
        {
            StartCoroutine(SetChangeTextChange());
        }

        if (PlayerInZone && Input.GetKeyDown(KeyCode.E) && CurtainsSwitch.showStarted == true && CueScript.totalCues == 15 && setIsChanged == false)           //if in zone and press E key
        {
            //Update cue count then start changing sets
            CueScript.totalCorrectCues++;
            CueScript.CorrectCueCount.text = "Correct Cues: " + CueScript.totalCorrectCues;
            CueTextTwo.text = "Act Two is about to begin...";
            setIsChanged = true;
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<Animator>().Play("switch");
            lightorobj.GetComponent<Animator>().Play("switch");
            StartCoroutine(SetChange());
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

    IEnumerator SetChange() 
    {
        yield return new WaitForSeconds(5);
        //Turn off all set one objects
        SetOneTexts.SetActive(false);
        SetOne.SetActive(false);
        SetTwo.SetActive(true);
        ButtonOne.SetActive(false);
        ButtonTwo.SetActive(false);
        ButtonThree.SetActive(false);
        ButtonFour.SetActive(false);
        ButtonFive.SetActive(false);
        SetOneIndicators.SetActive(false);
        SetTwoButtons.transform.position = new Vector3(-4.33023f, -2.255661f, -3.818892f);
        yield return new WaitForSeconds(10);
        isSetChangeOn = true;
    }

    IEnumerator SetChangeTextChange() 
    {
        CueTextTwo.text = "Time to change sets! The button is located on the left wall.";
        yield return new WaitForSeconds (10);
    }
}
