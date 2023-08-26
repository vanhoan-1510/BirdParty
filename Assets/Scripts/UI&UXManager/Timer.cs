using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Timer : MonoBehaviourPunCallbacks
{
    public static Timer Instance;

    [SerializeField]
    private float timerDuration; //Duration of the timer in seconds

    [SerializeField]
    private bool countDown = true;

    private float timer;
    [SerializeField]
    private Text firstMinute;
    [SerializeField]
    private Text secondMinute;
    [SerializeField]
    private Text separator;
    [SerializeField]
    private Text firstSecond;
    [SerializeField]
    private Text secondSecond;

    public Text remindText;

    private float flashTimer;
    [SerializeField]
    private float flashDuration = 1f; //The full length of the flash

    public int timeToPost;

    string currentTime;

    public int countDownTime = 5;
    public Text countDownText;

    private bool allPlayersLoaded = false;

    public GameObject gameOverPanel;

    bool isStartGame = false;

    private void Awake()
    {
        if (Instance == null)
        {
              Instance = this;
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            CheckAllPlayersLoaded();
        }

        ResetTimer();
        StartCoroutine(CountDownToStart());

        remindText.text = "You have 45 minutes to complete the game!";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CheckAllPlayersLoaded();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckAllPlayersLoaded();
    }

    private void CheckAllPlayersLoaded()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.Players.Count)
        {
            allPlayersLoaded = true;
            Debug.Log("All players have loaded.");
        }
        else
        {
            allPlayersLoaded = false;
            Debug.Log("Not all players have loaded.");
        }
    }

    private void ResetTimer()
    {
        if (countDown)
        {
            timer = timerDuration;
        }
        else
        {
            timer = 0;
        }
        SetTextDisplay(true);
    }

    void Update()
    {
        if (isStartGame)
        {
            CountTime();
        }
    }

    IEnumerator RemindTime()
    {
        int timeLeft = int.Parse(currentTime);
        if(currentTime == "0010")
        {
            remindText.text = "";
        }

        if (currentTime == "3000")
        {
            remindText.text = "You have 15 minutes left!";
            yield return new WaitForSeconds(10f);
            remindText.text = "";
        }

        if(currentTime == "4000")
        {
            remindText.text = "You have 5 minutes left!";
            yield return new WaitForSeconds(10f);
            remindText.text = "";
        }

        if(currentTime == "4430")
        {
            remindText.text = "You have " +  (4500 - timeLeft)  + " seconds left!";
        }
    }

    [PunRPC]
    public void CountTime()
    {
        if (countDown && timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay(timer);
        }
        else if (!countDown && timer < timerDuration)
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay(timer);
        }
        else
        {
            FlashTimer();
        }
        //Debug.Log(currentTime);
        timeToPost = int.Parse(currentTime);
        PlayerPrefs.SetInt("timeToPost", timeToPost);

        if (timeToPost == 4500)
        {
            StartCoroutine(GameOver());
        }

        StartCoroutine(RemindTime());
    }

    private void UpdateTimerDisplay(float time)
    {
        if (time < 0)
        {
            time = 0;
        }

        if (time > 3660)
        {
            Debug.LogError("Timer cannot display values above 3660 seconds");
            ErrorDisplay();
            return;
        }

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        currentTime = string.Format("{00:00}{01:00}", minutes, seconds);
        firstMinute.text = currentTime[0].ToString();
        secondMinute.text = currentTime[1].ToString();
        firstSecond.text = currentTime[2].ToString();
        secondSecond.text = currentTime[3].ToString();

        //Use this for a single text object
        //timerText.text = currentTime.ToString();
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Game Over");
        gameOverPanel.SetActive(true);
        AudioManager.Instance.PlaySFX("GameOver");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    private void ErrorDisplay()
    {
        firstMinute.text = "-";
        secondMinute.text = "-";
        firstSecond.text = "-";
        secondSecond.text = "-";


        //Use this for a single text object
        //timerText.text = "ERROR";
    }

    private void FlashTimer()
    {
        if (countDown && timer != 0)
        {
            timer = 0;
            UpdateTimerDisplay(timer);
        }

        if (!countDown && timer != timerDuration)
        {
            timer = timerDuration;
            UpdateTimerDisplay(timer);
        }

        if (flashTimer <= 0)
        {
            flashTimer = flashDuration;
        }
        else if (flashTimer <= flashDuration / 2)
        {
            flashTimer -= Time.deltaTime;
            SetTextDisplay(true);
        }
        else
        {
            flashTimer -= Time.deltaTime;
            SetTextDisplay(false);
        }
    }

    private void SetTextDisplay(bool enabled)
    {
        firstMinute.enabled = enabled;
        secondMinute.enabled = enabled;
        separator.enabled = enabled;
        firstSecond.enabled = enabled;
        secondSecond.enabled = enabled;

        //Use this for a single text object
        //timerText.enabled = enabled;
    }

    IEnumerator CountDownToStart()
    {
         yield return new WaitForSeconds(2f);
        if (allPlayersLoaded)
        {
            while (countDownTime > 0)
            {
                countDownText.text = countDownTime.ToString();
                yield return new WaitForSeconds(1f);
                countDownTime--;
                if (countDownTime > 0)
                {
                    AudioManager.Instance.PlaySFX("CountDown");
                }              
            }
            countDownText.text = "GO!";
            AudioManager.Instance.PlaySFX("GoSound");

            yield return new WaitForSeconds(1f);

            countDownText.gameObject.SetActive(false);
        }
        else
        {
            countDownTime = 5;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isStartGame = true;
            Debug.Log("Tinh thoi gian");
        }
    }
}
