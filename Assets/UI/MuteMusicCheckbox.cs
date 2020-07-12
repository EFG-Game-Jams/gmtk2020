using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteMusicCheckbox : MonoBehaviour
{
    void Start()
    {
        var toggle = GetComponent<Toggle>();
        var player = MusicPlayer.Instance;
        toggle.isOn = player.GetComponent<AudioSource>().mute;
    }

    public void OnToggle()
    {
        var player = MusicPlayer.Instance;
        var mute = GetComponent<Toggle>().isOn;
        player.GetComponent<AudioSource>().mute = mute;
    }
}
