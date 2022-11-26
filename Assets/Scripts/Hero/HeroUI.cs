using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{

    HeroData PlayerData;

    // Tests and Debugging
    [Header("Tests")]
    [SerializeField]
    Text Timer;
    [SerializeField]
    Text TimerSpeed;
    [SerializeField]
    Text TestState;
    [SerializeField]
    Text TestMethod;
    [SerializeField]
    bool DisableTestUI = false;

    // Start is called before the first frame update
    void Start()
    {
        if (DisableTestUI)
        {
            Timer.enabled = false;
            TimerSpeed.enabled = false;
            TestState.enabled = false;
            TestMethod.enabled = false;
        }

        PlayerData = GetComponentInChildren<HeroData>();
    }


    public void Tests()
    {
        if (!DisableTestUI)
        {
            TimerSpeed.text = PlayerData.Velocity.x.ToString();
            //Timer.text = (xTimer).ToString();
            TestState.text = PlayerData.PlayerState;
            TestMethod.text = PlayerData.PlayerMethod;
        }
    }

}
