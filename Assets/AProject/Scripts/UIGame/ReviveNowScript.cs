using TMPro;
using UnityEngine;

public class ReviveNowScript : MonoBehaviour
{
    public TextMeshProUGUI txt_timeCountDown;
    public SoundController soundCountDownController;
    public int int_timeCountDown = 5;
    private int int_timeShow;

    private void OnEnable()
    {
        int_timeShow = int_timeCountDown;
        txt_timeCountDown.text = int_timeShow.ToString();
    }

    public void TimeCountDown()
    {
        soundCountDownController.PlaySound(SoundData.SoundName.TimeCountDown);
        if (int_timeShow == 0f) return;
        int_timeShow--;
        txt_timeCountDown.text = int_timeShow.ToString();
    }

    public bool TimeShowEnd() {
        return int_timeShow == 0;
    }
}
