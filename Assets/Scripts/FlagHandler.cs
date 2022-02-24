using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using System.Text;

public class FlagHandler : MonoBehaviour
{
    [Header("UI")]
    public Canvas textCanvas;
    public Canvas flagsCanvas;
    public Canvas infoCanvas;
    public Button retryButton;
    public Button downloadButton;

    [Header("FLAGS")]
    public GameObject flagPrefab;
    public GameObject flagsPanel;
    public List<Sprite> flags;


    [TextArea(15,20)]public string myText;


    private void Start()
    {
        textCanvas.enabled = true;
        infoCanvas.enabled = false;
        flagsCanvas.enabled = false;
    }

    #region SPAWN FLAGS
    public void updateText(string newtext)
    {
        myText = newtext;
    }

    public void spawnFlags()
    {
        textCanvas.enabled = false;
        flagsCanvas.enabled = true;
        retryButton.gameObject.SetActive(false);
        downloadButton.gameObject.SetActive(false);

        StartCoroutine(spawnFlagsCoroutine());
    }
    private IEnumerator spawnFlagsCoroutine()
    {
        yield return new WaitForSeconds(.2f);

        myText = myText.ToUpper().Trim(); //CONVERTIMOS TODO EL TEXTO A MAYÚSCULAS
        myText = Regex.Replace(myText.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", ""); //LO NORMALIZAMOS PARA EVITAR TILDES O DIÉRESIS
        Debug.Log(myText);

        foreach (char letter in myText)
        {
            yield return new WaitForSeconds(.2f);

            foreach (Sprite flag in flags)
            {
                if (flag.name.Contains(letter))
                {
                    flagPrefab.GetComponent<Image>().sprite = flag;
                    Instantiate(flagPrefab, flagsPanel.transform);
                }
                else
                {
                    continue;
                }
            }
        }
        yield return new WaitForSeconds(1f);
        SaveImage();
        yield return new WaitForSeconds(4f);

        retryButton.gameObject.SetActive(true);
        downloadButton.gameObject.SetActive(true);
    }
    #endregion


    #region INFO CANVAS
    public void checkFlag(GameObject flagImage)
    {
        // change color
        Color flagColor = flagImage.GetComponent<Image>().color;
        flagColor.a = 0.25f;
        flagImage.GetComponent<Image>().color = flagColor;

        //show text
        flagImage.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
    }
    public void uncheckFlag(GameObject flagImage)
    {
        // change color
        Color flagColor = flagImage.GetComponent<Image>().color;
        flagColor.a = 1f;
        flagImage.GetComponent<Image>().color = flagColor;

        //hide text
        flagImage.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
    }

    #endregion

    #region DOWNLOAD IMAGE
    private void SaveImage()
    {
        PhotoCamera.Screenshot(500, 500);
    }
    public void DownloadImage()
    {

    }
    #endregion


    #region END
    public void ExitGame()
    {
        Application.Quit();
    }

    public void Retry()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
    #endregion
}
