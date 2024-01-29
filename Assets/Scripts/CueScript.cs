using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class CueScript : MonoBehaviour
{
    //Game Objects
    public GameObject OverTooSoonObject;
    public GameObject ResultsScreen;
    public TextMeshProUGUI PlayerResults;
    public TextMeshProUGUI PlayerResultsText;

    //Variables
    public int totalCorrectCues = 0;
    public int totalCues = 0;
    private int endGameNum = 30;


    //References to other scripts
    public CurtainsSwitch CurtainsSwitch;
    public CurtainsSwitchClose CurtainsSwitchClose;
    public LightSwitch LightSwitch;
    public RadioSwitch RadioSwitch;
    public FirePlaceSwitch FirePlaceSwitch;
    public LaughSwitch LaughSwitch;
    public PlayerMovement Player;
    public SetChangeSwitch SetChangeSwitch;
    public SunMoonSwitch SunMoonSwitch;
    public FoxSwitch FoxSwitch;
    public SnowSwitch SnowSwitch;
    public BirdSwitch BirdSwitch;


    //Updates the CueText and Correct Cues
    public TextMeshProUGUI CueText;
    public TextMeshProUGUI CueTextTwo;
    public GameObject CueTextToggle;
    public TextMeshProUGUI CorrectCueCount;

    //Bool values
    private bool isEventChecked = false;
    private bool TextOneToggle = false;
    private bool lastPoint = false;
    
    //Array of the different events to cue
    private string[] events = {"Lights", "LightsOff", "Radio", "RadioOff", "Fireplace", "FireplaceOff", "Knock", "KnockOff"};
    private string[] eventsTwo = { "SunMoon", "SunMoonOff", "Fox", "FoxOff", "Snow", "SnowOff", "Bird", "BirdOff" };


    // Start is called before the first frame update
    void Start()
    {
        //Object References
        Player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        //Script References
        CurtainsSwitch = GameObject.Find("CurtainsButtonOpen").GetComponent<CurtainsSwitch>();
        CurtainsSwitchClose = GameObject.Find("CurtainsButtonClose").GetComponent<CurtainsSwitchClose>();
        LightSwitch = GameObject.Find("LightButton").GetComponent<LightSwitch>();
        RadioSwitch = GameObject.Find("RadioButton").GetComponent<RadioSwitch>();
        FirePlaceSwitch = GameObject.Find("FirePlaceButton").GetComponent<FirePlaceSwitch>();
        LaughSwitch = GameObject.Find("LaughButton").GetComponent<LaughSwitch>();
        SetChangeSwitch = GameObject.Find("SetChangeButton").GetComponent<SetChangeSwitch>();
        SunMoonSwitch = GameObject.Find("SunMoonButton").GetComponent<SunMoonSwitch>();
        FoxSwitch = GameObject.Find("FoxButton").GetComponent<FoxSwitch>();
        SnowSwitch = GameObject.Find("SnowButton").GetComponent<SnowSwitch>();
        BirdSwitch = GameObject.Find("BirdButton").GetComponent<BirdSwitch>();

        CueText.text = "Open the curtains to cut the music, and start the show!";
    }

    // Update is called once per frame
    void Update()
    {
        if (CurtainsSwitchClose.areCurtainsClosed == true && totalCues == endGameNum)
        {
            if (lastPoint == false) 
            {
                totalCorrectCues++;
                lastPoint = true;
            }
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            CueTextTwo.text = "Well done! You made it through the show, now let's see how you performed.";
            GameOver();
            StartCoroutine(ResultScreen());
        }

        if (totalCues == endGameNum && CurtainsSwitchClose.areCurtainsClosed == false) 
        {
            CueTextTwo.text = "Thats the end of the show! Time to close the curtains...";
        }

        if (totalCues == 15) 
        {
            CueTextToggle.SetActive(false);
            TextOneToggle = true;
        }

        if (totalCues != endGameNum && CurtainsSwitchClose.areCurtainsClosed == true) 
        {
            GameOver();
            if (TextOneToggle == false)
            {
                CueText.text = "You closed the curtains before the show was over! What a disaster!";
                StartCoroutine(OverTooSoon());
            }
            else 
            {
                SetChangeSwitch.CueTextTwo.text = "You closed the curtains before the show was over! What a disaster!";
                StartCoroutine(OverTooSoon());
            }
        }
        if (totalCues == endGameNum) 
        {
            CueText.text = "Thats the end of the show! Close the Curtains...";
            if (CurtainsSwitchClose.areCurtainsClosed == true) 
            {
                GameOver();
            }
        }

        //Set One cues
        if (isEventChecked == false && CurtainsSwitch.showStarted == true && totalCues != endGameNum && totalCues < 15) 
        {
            CueText.text = "Wait for the cue...";
            isEventChecked = true;

            //Grab a new cue
            var cue = RandomCue();

            //Light cue calls
            //Check which cue is staged and call it
            if (cue == "Lights" && LightSwitch.isLightOn == false)
            {
                StartCoroutine(Lights());
            }

            if (cue == "LightsOff" && LightSwitch.isLightOn == true) 
            {
                StartCoroutine(LightsOff());  
            }

            //Contingency if the cue is on lights on but the lights are already on
            if (cue == "Lights" && LightSwitch.isLightOn == true)
            {
                StartCoroutine(LightsOff());
            }
            //Contingency if the cue is on lights off but the lights are already off
            if (cue == "LightsOff" && LightSwitch.isLightOn == false)
            {
                StartCoroutine(Lights());
            }


            //Radio cue calls
            if (cue == "Radio" && RadioSwitch.isRadioOn == false)
            {
                StartCoroutine(Radio());
            }

            //Contingency if the cue is on radio on but the radio is already on
            if (cue == "Radio" && RadioSwitch.isRadioOn == true)
            {
                StartCoroutine(RadioOff());
            }

            if (cue == "RadioOff" && RadioSwitch.isRadioOn == true)
            {
                StartCoroutine(RadioOff());
            }

            //Contingency if the cue is on radio off but is already off
            if (cue == "RadioOff" && RadioSwitch.isRadioOn == false)
            {
                StartCoroutine(Radio());
            }


            //Fireplace cue calls
            if (cue == "Fireplace" && FirePlaceSwitch.isFirePlaceOn == false)
            {
                StartCoroutine(FirePlace());
            }

            //Contingency if the cue is on fireplace on but is already on
            if (cue == "Fireplace" && FirePlaceSwitch.isFirePlaceOn == true)
            {
                StartCoroutine(FirePlaceOff());
            }

            if (cue == "FireplaceOff" && FirePlaceSwitch.isFirePlaceOn == true)
            {
                StartCoroutine(FirePlaceOff());
            }

            //Contingency if the cue is on fireplace off but is already off
            if (cue == "FireplaceOff" && FirePlaceSwitch.isFirePlaceOn == false)
            {
                StartCoroutine(FirePlace());
            }


            //Knocking cue calls
            if (cue == "Knock" && LaughSwitch.isLaughOn == false)
            {
                StartCoroutine(Knock());
            }

            //Contingency if the cue is on fireplace on but is already on
            if (cue == "Knock" && LaughSwitch.isLaughOn == true)
            {
                StartCoroutine(KnockOff());
            }

            if (cue == "KnockOff" && LaughSwitch.isLaughOn == true)
            {
                StartCoroutine(KnockOff());
            }

            //Contingency if the cue is on fireplace off but is already off
            if (cue == "KnockOff" && LaughSwitch.isLaughOn == false)
            {
                StartCoroutine(Knock());
            }

        }

        //SET TWO CALL BEGIN HERE
        //if (totalCues >= 15 && SetChangeSwitch.isSetChangeOn == true) 
        //{
            if (isEventChecked == false && CurtainsSwitch.showStarted == true && totalCues != endGameNum && totalCues >= 15 && SetChangeSwitch.isSetChangeOn == true)
            {
                CueText.text = "Wait for the cue...";
                isEventChecked = true;

                //Grab a new cue
                var cue = RandomCueTwo();

                //SunMoon cue calls
                //Check which cue is staged and call it
                if (cue == "SunMoon" && SunMoonSwitch.isLightOn == false)
                {
                    StartCoroutine(SunMoon());
                }

                if (cue == "SunMoonOff" && SunMoonSwitch.isLightOn == true)
                {
                    StartCoroutine(SunMoonOff());
                }

                if (cue == "SunMoon" && SunMoonSwitch.isLightOn == true)
                {
                    StartCoroutine(SunMoonOff());
                }
                if (cue == "SunMoonOff" && SunMoonSwitch.isLightOn == false)
                {
                    StartCoroutine(SunMoon());
                }

                //Fox cue calls
                if (cue == "Fox" && FoxSwitch.isFoxOn == false)
                {
                    StartCoroutine(Fox());
                }

                if (cue == "FoxOff" && FoxSwitch.isFoxOn == true)
                {
                    StartCoroutine(FoxOff());
                }

                if (cue == "Fox" && FoxSwitch.isFoxOn == true)
                {
                    StartCoroutine(FoxOff());
                }
                if (cue == "FoxOff" && FoxSwitch.isFoxOn == false)
                {
                    StartCoroutine(Fox());
                }

                //Snow cue calls
                if (cue == "Snow" && SnowSwitch.isSnowOn == false)
                {
                    StartCoroutine(Snow());
                }

                if (cue == "SnowOff" && SnowSwitch.isSnowOn == true)
                {
                    StartCoroutine(SnowOff());
                }

                if (cue == "Snow" && SnowSwitch.isSnowOn == true)
                {
                    StartCoroutine(SnowOff());
                }
                if (cue == "SnowOff" && SnowSwitch.isSnowOn == false)
                {
                    StartCoroutine(Snow());
                }

                //Bird cue calls
                if (cue == "Bird" && BirdSwitch.isBirdOn == false)
                {
                    StartCoroutine(Bird());
                }

                if (cue == "BirdOff" && BirdSwitch.isBirdOn == true)
                {
                    StartCoroutine(BirdOff());
                }

                if (cue == "Bird" && BirdSwitch.isBirdOn == true)
                {
                    StartCoroutine(BirdOff());
                }
                if (cue == "BirdOff" && BirdSwitch.isBirdOn == false)
                {
                    StartCoroutine(Bird());
                }

        }
    }

    //Coroutines

        //Lights Coroutines
    IEnumerator Lights() 
    {
        totalCues ++;
        //Give the player time to react
        CueText.text = "Cue the lights, Turn On!";
        yield return new WaitForSeconds(5);

        //No point if cue is missed in time, one point awarded if clicked in time
        if (LightSwitch.isLightOn != true)
        {
            MissedCue();
        }
        else 
        {
            CueText.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDone());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    IEnumerator LightsOff()
    {
        totalCues ++;
        CueText.text = "Cue the lights, Turn Off!";
        yield return new WaitForSeconds(5);
        if (LightSwitch.isLightOn == true)
        {
            MissedCue();
        }
        else
        {
            CueText.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDone());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    //Radio Corountines
    IEnumerator Radio()
    {
        totalCues++;
        //Give the player time to react
        CueText.text = "Cue the Radio, Turn On!";
        yield return new WaitForSeconds(5);

        //No point if cue is missed in time, one point awarded if clicked in time
        if (RadioSwitch.isRadioOn != true)
        {
            MissedCue();
        }
        else
        {
            CueText.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDone());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    IEnumerator RadioOff()
    {
        totalCues++;
        CueText.text = "Cue the Radio, Turn Off!";
        yield return new WaitForSeconds(5);
        if (RadioSwitch.isRadioOn == true)
        {
            MissedCue();
        }
        else
        {
            CueText.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDone());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    //Fireplace Corountines
    IEnumerator FirePlace()
    {
        totalCues++;
        //Give the player time to react
        CueText.text = "Cue the Fireplace, Turn On!";
        yield return new WaitForSeconds(5);

        //No point if cue is missed in time, one point awarded if clicked in time
        if (FirePlaceSwitch.isFirePlaceOn != true)
        {
            MissedCue();
        }
        else
        {
            CueText.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDone());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    IEnumerator FirePlaceOff()
    {
        totalCues++;
        CueText.text = "Cue the Fireplace, Turn Off!";
        yield return new WaitForSeconds(5);
        if (FirePlaceSwitch.isFirePlaceOn == true)
        {
            MissedCue();
        }
        else
        {
            CueText.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDone());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    //Knocking Corountines
    IEnumerator Knock()
    {
        totalCues++;
        //Give the player time to react
        CueText.text = "Cue the Doorbell, Turn On!";
        yield return new WaitForSeconds(5);

        //No point if cue is missed in time, one point awarded if clicked in time
        if (LaughSwitch.isLaughOn != true)
        {
            MissedCue();
        }
        else
        {
            CueText.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDone());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    IEnumerator KnockOff()
    {
        totalCues++;
        CueText.text = "Cue the Doorbell, Turn Off!";
        yield return new WaitForSeconds(5);
        if (LaughSwitch.isLaughOn == true)
        {
            MissedCue();
        }
        else
        {
            CueText.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDone());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    //SET TWO COROUTINES
        //SunMoon Coroutines
    IEnumerator SunMoon()
    {
        totalCues++;
        //Give the player time to react
        CueTextTwo.text = "Cue the time of day switch, go to nighttime!";
        yield return new WaitForSeconds(5);

        //No point if cue is missed in time, one point awarded if clicked in time
        if (SunMoonSwitch.isLightOn != true)
        {
            MissedCueTwo();
        }
        else
        {
            CueTextTwo.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDoneTwo());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    IEnumerator SunMoonOff()
    {
        totalCues++;
        CueTextTwo.text = "Cue the time of day switch, go the daytime!";
        yield return new WaitForSeconds(5);
        if (SunMoonSwitch.isLightOn == true)
        {
            MissedCueTwo();
        }
        else
        {
            CueTextTwo.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDoneTwo());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    //Fox Cues
    IEnumerator Fox()
    {
        totalCues++;
        //Give the player time to react
        CueTextTwo.text = "Cue the Fox switch, turn it on!";
        yield return new WaitForSeconds(5);

        //No point if cue is missed in time, one point awarded if clicked in time
        if (FoxSwitch.isFoxOn != true)
        {
            MissedCueTwo();
        }
        else
        {
            CueTextTwo.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDoneTwo());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    IEnumerator FoxOff()
    {
        totalCues++;
        CueTextTwo.text = "Cue the Fox switch, turn it off!";
        yield return new WaitForSeconds(5);
        if (FoxSwitch.isFoxOn == true)
        {
            MissedCueTwo();
        }
        else
        {
            CueTextTwo.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDoneTwo());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    //Snow cues
    IEnumerator Snow()
    {
        totalCues++;
        //Give the player time to react
        CueTextTwo.text = "Cue the Snow switch, turn it on!";
        yield return new WaitForSeconds(5);

        //No point if cue is missed in time, one point awarded if clicked in time
        if (SnowSwitch.isSnowOn != true)
        {
            MissedCueTwo();
        }
        else
        {
            CueTextTwo.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDoneTwo());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    IEnumerator SnowOff()
    {
        totalCues++;
        CueTextTwo.text = "Cue the Snow switch, turn it off!";
        yield return new WaitForSeconds(5);
        if (SnowSwitch.isSnowOn == true)
        {
            MissedCueTwo();
        }
        else
        {
            CueTextTwo.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDoneTwo());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    //Bird cues
    IEnumerator Bird()
    {
        totalCues++;
        //Give the player time to react
        CueTextTwo.text = "Cue the Birds switch, turn it on!";
        yield return new WaitForSeconds(5);

        //No point if cue is missed in time, one point awarded if clicked in time
        if (BirdSwitch.isBirdOn != true)
        {
            MissedCueTwo();
        }
        else
        {
            CueTextTwo.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDoneTwo());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    IEnumerator BirdOff()
    {
        totalCues++;
        CueTextTwo.text = "Cue the Birds switch, turn it off!";
        yield return new WaitForSeconds(5);
        if (BirdSwitch.isBirdOn == true)
        {
            MissedCueTwo();
        }
        else
        {
            CueTextTwo.text = "Well Done!";
            totalCorrectCues++;
            CorrectCueCount.text = "Correct Cues: " + totalCorrectCues;
            StartCoroutine(WellDoneTwo());
        }
        //Wait for the next cue
        yield return new WaitForSeconds(8);
        isEventChecked = false;
    }

    IEnumerator Wait() 
    {
       yield return new WaitForSeconds(8);
    }
    IEnumerator WellDone() 
    {
        yield return new WaitForSeconds(2);
        if (totalCues < 15)
        {
            CueText.text = "Wait for the cue...";
        }
        else 
        {
            CueTextTwo.text = "Wait for the cue...";
        }
        yield return new WaitForSeconds(1);
    }

    IEnumerator WellDoneTwo()
    {
        yield return new WaitForSeconds(2);
            CueTextTwo.text = "Wait for the cue...";
        yield return new WaitForSeconds(1);
    }

    //Private functions
    private void MissedCue() 
    {
        if (totalCues < 15)
        {
            CueText.text = "Cue Missed! But, the show must go on!";
        }
    }

    private void MissedCueTwo()
    {
           CueTextTwo.text = "Cue Missed! But, the show must go on!";
    }

    private string RandomCue() 
    {
        //Get a random element in the array
        var MyIndex = UnityEngine.Random.Range(0, (events.Length));
        //Take this debug line out when finished
        Debug.Log(events[MyIndex]);
        return events[MyIndex];
    }

    private string RandomCueTwo() 
    {
        //Get a random element in the array
        var MyIndex = UnityEngine.Random.Range(0, (eventsTwo.Length));
        //Take this debug line out when finished
        Debug.Log(eventsTwo[MyIndex]);
        return eventsTwo[MyIndex];
    }

    private void GameOver() 
    {
        Player.walkSpeed = 0;
        Player.lookSpeed = 0;
    }

    IEnumerator OverTooSoon() 
    {
        yield return new WaitForSeconds(5);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        OverTooSoonObject.SetActive(true);
    }

    IEnumerator ResultScreen() 
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        yield return new WaitForSeconds(5);
        ResultsScreen.SetActive(true);
        PlayerResults.text = totalCorrectCues.ToString();
        if (totalCorrectCues == endGameNum) 
        {
            PlayerResultsText.text = "Perfect! You didn't miss a beat! Excellent Work!";
        }
        if (totalCorrectCues < endGameNum && totalCorrectCues > 24) 
        {
            PlayerResultsText.text = "Well done! Try again to get the perfect show!";
        }
        if (totalCorrectCues < 25 && totalCorrectCues > 19) 
        {
            PlayerResultsText.text = "Not bad, practice some more to improve your score!";
        }
        if (totalCorrectCues < 20) 
        {
            PlayerResultsText.text = "Keep trying! Practice makes perfect!";
        } 
    }
}
    
