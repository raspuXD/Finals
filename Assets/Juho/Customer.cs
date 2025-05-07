using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CustomerData
{
    public string theLineToBeSaid;
    public WholeFood theFoodCustomerWants;
}

public class Customer : MonoBehaviour
{
    public CustomerData[] womanCustomers, maleCustomers;
    public string[] womanNames, manNames;
    public Sprite[] womanSprites, manSprites;
    public WholeFood[] foodsToBeWantedBeltLeve1;
    public WholeFood[] foodsToBeWantedBeltLeve2;
    public WholeFood[] foodsToBeWantedBeltLeve3;
    public int whatLevelBelt = 1; // between 1-3
    public CustomerData selectedCustomer;
    bool isMan = false;

    public MoneyManager moneyManager;

    [Header("Visual")]
    public Image theCustomerUIImage, anotherOne;
    public float howFastFadesInAndOut = .5f;
    public Image speakCloud;
    public TMP_Text theSpeakText, nameText;  // Added nameText TMP_Text
    public float howFastWritesLetter = 0.04f;
    public float howLongWaitAfterFullyWriten = 2f;
    public GameObject buttonHolder;

    [Header("Accept")]
    [SerializeField] FullFade fullFade;




    private void Start()
    {
        StartCoroutine(WaitBeforeNewCustomer());
    }

    public void Accepct()
    {
        fullFade.StartFullFade();
    }

    public void FinishTheCustomer()
    {
        // Hide UI elements


        buttonHolder.SetActive(false);
        theSpeakText.text = "";
        nameText.text = "";
        theSpeakText.gameObject.SetActive(false);
        speakCloud.gameObject.SetActive(false);
        theCustomerUIImage.color = new Color(1, 1, 1, 0); // Make invisible

        // Reset customer data
        MoneyManager money = FindObjectOfType<MoneyManager>();
        money.IncreaseMoney(selectedCustomer.theFoodCustomerWants.maxCost);

        selectedCustomer = null;

        // Wait for new customer
        StartCoroutine(WaitBeforeNewCustomer());
    }


    public void Decline()
    {
        buttonHolder.SetActive(false);
        StartCoroutine(FadeOut(theCustomerUIImage));
        StartCoroutine(FadeOut(speakCloud));
        StartCoroutine(FadeOutText(nameText));
        StartCoroutine(FadeOutText(theSpeakText));
        StartCoroutine(WaitBeforeNewCustomer());
        moneyManager.DecreaseMoney(10);
    }

    public IEnumerator WaitBeforeNewCustomer()
    {
        float random = Random.Range(2f, 4f);
        yield return new WaitForSeconds(random);
        SpawnNewCustomer();
    }

    public void SpawnNewCustomer()
    {
        isMan = false;

        if (Random.value > 0.5f)  // Randomly decide between male and female
        {
            selectedCustomer = womanCustomers[Random.Range(0, womanCustomers.Length)];
            theCustomerUIImage.sprite = womanSprites[Random.Range(0, womanSprites.Length)];
            anotherOne.sprite = womanSprites[Random.Range(0, womanSprites.Length)];
            nameText.text = womanNames[Random.Range(0, womanNames.Length)];  // Set the name for the woman
            isMan = false ;
        }
        else
        {
            selectedCustomer = maleCustomers[Random.Range(0, maleCustomers.Length)];
            theCustomerUIImage.sprite = manSprites[Random.Range(0, manSprites.Length)];
            anotherOne.sprite = manSprites[Random.Range(0, manSprites.Length)];
            nameText.text = manNames[Random.Range(0, manNames.Length)];  // Set the name for the man
            isMan = true;
        }

        // Based on whatLevelBelt, select the correct food
        WholeFood[] foodOptions = whatLevelBelt == 1 ? foodsToBeWantedBeltLeve1 :
                                  whatLevelBelt == 2 ? foodsToBeWantedBeltLeve2 : foodsToBeWantedBeltLeve3;

        // Assign the selected food to the customer
        selectedCustomer.theFoodCustomerWants = foodOptions[Random.Range(0, foodOptions.Length)];

        // Start by fading in the customer image
        StartCoroutine(FadeInCustomerImage());
    }

    IEnumerator FadeInCustomerImage()
    {
        float time = 0;
        Color startColor = theCustomerUIImage.color;
        startColor.a = 0;  // Start completely transparent
        theCustomerUIImage.color = startColor;

        // Fade in the customer image
        while (time < howFastFadesInAndOut)
        {
            time += Time.deltaTime;
            startColor.a = Mathf.Lerp(0, 1, time / howFastFadesInAndOut);
            theCustomerUIImage.color = startColor;
            yield return null;
        }

        // Ensure the image is fully opaque
        startColor.a = 1;
        theCustomerUIImage.color = startColor;

        // Now start writing the text and show the speech bubble
        yield return new WaitForSeconds(1f);

        theSpeakText.text = ""; // Clear any existing text
        theSpeakText.gameObject.SetActive(true);
        speakCloud.gameObject.SetActive(true);

        // Fade in the speech bubble
        StartCoroutine(FadeIn(speakCloud));
    }

    IEnumerator FadeIn(Graphic graphic)
    {
        float time = 0;
        Color startColor = graphic.color;
        startColor.a = 0;
        graphic.color = startColor;

        while (time < howFastFadesInAndOut)
        {
            time += Time.deltaTime;
            startColor.a = Mathf.Lerp(0, 1, time / howFastFadesInAndOut);
            graphic.color = startColor;
            yield return null;
        }

        if(graphic == speakCloud)
        {
            StartCoroutine(TypeSentence(selectedCustomer));
        }

        startColor.a = 1;
        graphic.color = startColor;
    }

    IEnumerator TypeSentence(CustomerData selectedCustomer)
    {
        string fullSentence = selectedCustomer.theLineToBeSaid + " " + selectedCustomer.theFoodCustomerWants.foodName;
        int letterCounter = 0;
        int nextDebugAt = Random.Range(1, 5); // Random.Range is inclusive of min and exclusive of max for ints

        theSpeakText.text = ""; // Clear text before typing

        foreach (char letter in fullSentence)
        {
            theSpeakText.text += letter;
            letterCounter++;

            if (letterCounter >= nextDebugAt)
            {
                if (isMan)
                {
                    AudioManager.Instance.PlaySFX("ManSpeak");
                }
                else
                {
                    AudioManager.Instance.PlaySFX("WomanSpeak");
                }
                letterCounter = 0;
                nextDebugAt = Random.Range(3, 5); // Set new threshold for next debug
            }

            yield return new WaitForSeconds(howFastWritesLetter);
        }

        StartCoroutine(WaitForComplete());
    }

    IEnumerator WaitForComplete()
    {
        // Wait until the text has finished being written
        yield return new WaitForSeconds(howLongWaitAfterFullyWriten);

        buttonHolder.SetActive(true);
        StopAllCoroutines();
    }

    IEnumerator FadeOutText(TMP_Text theText)
    {
        float time = 0;
        Color startColor = theText.color;
        startColor.a = 1;
        theText.color = startColor;

        while (time < howFastFadesInAndOut)
        {
            time += Time.deltaTime;
            startColor.a = Mathf.Lerp(1, 0, time / howFastFadesInAndOut);
            theText.color = startColor;
            yield return null;
        }

        // Once fully faded out, clear the text
        theText.text = "";
        startColor.a = 1;
        theText.color = startColor;
    }

    IEnumerator FadeOut(Graphic graphic)
    {
        float time = 0;
        Color startColor = graphic.color;
        startColor.a = 1;
        graphic.color = startColor;

        while (time < howFastFadesInAndOut)
        {
            time += Time.deltaTime;
            startColor.a = Mathf.Lerp(1, 0, time / howFastFadesInAndOut);
            graphic.color = startColor;
            yield return null;
        }

        startColor.a = 0;
        graphic.color = startColor;
    }
}
