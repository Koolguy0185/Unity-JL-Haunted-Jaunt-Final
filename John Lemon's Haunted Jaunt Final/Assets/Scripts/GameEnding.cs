using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public GameObject timer;
    public GameObject weaponText;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public CanvasGroup timesUpBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public AudioSource caughtAudio;
    public TextMeshProUGUI timerText;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    bool m_HasAudioPlayed;
    float m_Timer;
    int m_GameTime;

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        } 
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    private void Start()
    {
        timer.SetActive(true);
        m_GameTime = 90;
        timerText.text = "Timer: " + m_GameTime;
        StartCoroutine(DecreaseTime());
    }

    IEnumerator DecreaseTime()
    {
        yield return new WaitForSeconds(1);
        m_GameTime--;
        timerText.text = "Timer: " + m_GameTime;
        StartCoroutine(DecreaseTime());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weaponText.SetActive(false);
        }

        if(m_IsPlayerAtExit)
        {
            timer.SetActive(false);
            weaponText.SetActive(false);
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            timer.SetActive(false);
            weaponText.SetActive(false);
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
        else if(m_GameTime == 0 && (!m_IsPlayerAtExit || !m_IsPlayerCaught))
        {
            StopAllCoroutines();
            timer.SetActive(false);
            weaponText.SetActive(false);
            EndLevel(timesUpBackgroundImageCanvasGroup, true, caughtAudio);
        }
    } 

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSoure)
    {
        if (!m_HasAudioPlayed)
        {
            audioSoure.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
