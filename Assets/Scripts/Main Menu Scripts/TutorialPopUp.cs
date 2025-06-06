using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUp : MonoBehaviour
{
    public GameObject canvas;
    public Button theTutorialButton;
    public GameObject thePopUp, canCountinueNow;
    public TMP_Text theInfoText, howToGetAwayText;
    public Image theTutorialButtonAgain;
    bool startedThePop;

    [Header("Closing options")]
    public KeyCode closeKey = KeyCode.Escape;

    [Header("See only once ever")]
    public bool willSeeOnlyOnce = true;
    public string thePlayerPrefName = "TutorialPopUp0";

    [Header("Scale Speed and stuff")]
    public float howFastScales = .33f;
    public float howLongToCanClose = 2f;

    [Header("If want to in the start")]
    public bool showInStart = false;
    public string wantToWriteInStart;
    public float howLongNeedToWaitIfFade = .75f;

    [Header("If want to from trigger")]
    public bool fromATrigger = false;
    public string wantToWriteInTrigger;

    [Header("If Contains Multiple Dias")]
    public bool usesMultipleDias;
    public string[] theDifferentDias;
    private int currentDiaIndex = 0;

    [Header("If Want to say something else when countinue")]
    public bool doYouWantToSoDifferentCountinueText = false;
    public string whatToSayAwayText;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(howLongNeedToWaitIfFade);

        if (showInStart && !usesMultipleDias)
        {
            StartThePopUp(wantToWriteInStart);
        }
        else if (showInStart)
        {
            StartThePopUp(theDifferentDias[0]);
        }
    }

    public void StartThePopUp(string textToShow)
    {
        if(willSeeOnlyOnce)
        {
            int hasSeen = PlayerPrefs.GetInt(thePlayerPrefName, 0);

            if (hasSeen == 0)
            {
                canvas.SetActive(true);
                startedThePop = true;
                thePopUp.transform.localScale = Vector3.zero;
                theTutorialButton.gameObject.SetActive(true);
                if(!usesMultipleDias)
                {
                    theInfoText.text = textToShow;
                }
                else
                {
                    theInfoText.text = theDifferentDias[0];
                }
                    theTutorialButton.interactable = false;
                AudioManager.Instance.PlaySFX("PopUp");
                StartCoroutine(ScaleTheButton(true));
                theTutorialButton.onClick.AddListener(ClosePopUpAndClearButton);
            }
            else
            {
                Debug.Log("Has Seen Already");
                Destroy(gameObject);
            }
        }
        else
        {
            canvas.SetActive(true);
            startedThePop = true;
            thePopUp.transform.localScale = Vector3.zero;
            theTutorialButton.gameObject.SetActive(true);
            theInfoText.text = textToShow;
            theTutorialButton.interactable = false;
            AudioManager.Instance.PlaySFX("PopUp");
            StartCoroutine(ScaleTheButton(true));
            theTutorialButton.onClick.AddListener(ClosePopUpAndClearButton);
        }
    }

    private void ClosePopUpAndClearButton()
    {
        if(usesMultipleDias)
        {
            if(currentDiaIndex >= theDifferentDias.Length - 1)
            {
                theTutorialButton.onClick.RemoveListener(ClosePopUpAndClearButton);
            }
        }
        else
        {
            theTutorialButton.onClick.RemoveListener(ClosePopUpAndClearButton);
        }

        // Then, close the pop-up
        CloseThePopUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (fromATrigger)
        {
            StartThePopUp(wantToWriteInTrigger);
        }
    }
    IEnumerator ScaleTheButton(bool scaleUp)
    {
        Vector3 initialScale = thePopUp.transform.localScale;
        Vector3 targetScale = scaleUp ? Vector3.one : Vector3.zero;
        float timeElapsed = 0f;

        // Fade effect for dark background
        Color initialColor = theTutorialButtonAgain.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, scaleUp ? 240f / 255f : 0f); // Fade to 240 on scale up, to 0 on scale down

        while (timeElapsed < howFastScales)
        {
            thePopUp.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / howFastScales);

            // Fade the dark background's alpha value
            theTutorialButtonAgain.color = Color.Lerp(initialColor, targetColor, timeElapsed / howFastScales);

            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        thePopUp.transform.localScale = targetScale;
        theTutorialButtonAgain.color = targetColor;

        if (scaleUp)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(howLongToCanClose);
            AudioManager.Instance.PlaySFX("PopUpClickAppear");
            canCountinueNow.SetActive(true);
            theTutorialButton.interactable = true;

            if (usesMultipleDias)
            {
                if (doYouWantToSoDifferentCountinueText)
                {
                    howToGetAwayText.text = whatToSayAwayText;
                }
                else
                {
                    howToGetAwayText.text = "Click to next or\r\n" + closeKey.ToString();
                }
            }
            else
            {
                if (doYouWantToSoDifferentCountinueText)
                {
                    howToGetAwayText.text = whatToSayAwayText;
                }
                else
                {
                    howToGetAwayText.text = "Click to Close or\r\n" + closeKey.ToString();
                }
            }
        }
        else
        {
            Time.timeScale = 1f;
            theInfoText.text = "";
            howToGetAwayText.text = "";
            canCountinueNow.SetActive(false);
            theTutorialButton.gameObject.SetActive(false);
            PlayerPrefs.SetInt(thePlayerPrefName, 1);
            PlayerPrefs.Save();
            canvas.SetActive(false);
            Destroy(gameObject);
        }
    }

    IEnumerator ScaleTheCanCountinue()
    {
        yield return new WaitForSecondsRealtime(howLongToCanClose);
        AudioManager.Instance.PlaySFX("PopUpClickAppear");
        canCountinueNow.SetActive(true);
    }

    public void CloseThePopUp()
    {
        if(startedThePop)
        {
            if (usesMultipleDias && currentDiaIndex < theDifferentDias.Length - 1)
            {
                currentDiaIndex++;
                canCountinueNow.SetActive(false);
                ShowNextDialogue();
            }
            else
            {
                currentDiaIndex = 0;
                theTutorialButton.interactable = false;
                AudioManager.Instance.PlaySFX("PopUp");
                StartCoroutine(ScaleTheButton(false));
            }
        }
    }

    private void ShowNextDialogue()
    {
        theInfoText.text = theDifferentDias[currentDiaIndex];
        AudioManager.Instance.PlaySFX("PopUpClickAppear");
        StartCoroutine(ScaleTheCanCountinue());

        if (usesMultipleDias && currentDiaIndex >= theDifferentDias.Length - 1)
        {
            if(doYouWantToSoDifferentCountinueText)
            {
                howToGetAwayText.text = whatToSayAwayText;
            }
            else
            {
                howToGetAwayText.text = "Click to Close or\r\n" + closeKey.ToString();
            }
        }
    }

    void Update()
    {
        if (canCountinueNow.activeSelf && Input.GetKeyDown(closeKey))
        {
            CloseThePopUp();
        }
    }
}