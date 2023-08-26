using LootLocker.Requests;
using UnityEngine;
using UnityEngine.UI;

public class ShowLeaderBoard : MonoBehaviour
{
    private string leaderboardKey = "birdpartygame_leaderboard";
    int maxScore = 5;
    public Text[] PlayerNameList;
    public Text[] PlayerScoreList;

    private void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");

                return;
            }

            Debug.Log("successfully started LootLocker session");
        });
    }

    public void ShowScore()
    {

        AudioManager.Instance.PlaySFX("ClickButton");
        LootLockerSDKManager.GetScoreList(leaderboardKey, maxScore, (response) =>
        {
            if (response.statusCode == 200)
            {
                LootLockerLeaderboardMember[] score = response.items;

                for (int i = 0; i < score.Length; i++)
                {
                    PlayerNameList[i].text = score[i].member_id;
                    if (score[i].score == 0)
                    {
                        PlayerScoreList[i].text = "0";
                    }
                    else if (score[i].score / 100 < 10)
                    {
                        PlayerScoreList[i].text = "0" + score[i].score / 100 + " : " + score[i].score % 100;
                    }
                    else if (score[i].score / 100 == 0)
                    {
                        PlayerScoreList[i].text = "00" + score[i].score / 100 + " : " + score[i].score % 100;
                    }
                    else if (score[i].score % 100 == 0)
                    {
                        PlayerScoreList[i].text = score[i].score / 100 + " : 00";
                    }
                    else
                    {
                        PlayerScoreList[i].text = score[i].score / 100 + " : " + score[i].score % 100;
                    }
                }

                if (score.Length < maxScore)
                {
                    for (int i = score.Length; i < maxScore; i++)
                    {
                        PlayerNameList[i].text = "None";
                        PlayerScoreList[i].text = "00 : 00";
                    }
                }
            }

        });
    }
}
