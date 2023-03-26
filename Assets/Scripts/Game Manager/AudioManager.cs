using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StardropTools;
using StardropTools.Audio;
using StardropTools.ScriptableValue;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioListWithSource uiSource;
    [SerializeField] AudioListWithSource playingSource;
    [SerializeField] ScriptableBool canPlaySound;

    private void Start()
    {
        
    }

    public void PlayJump()
    {
        if (canPlaySound.Bool == false)
            return;

        playingSource.PlayClipOneShotAtIndex(0, true);
    }

    public void PlayCollision()
    {
        if (canPlaySound.Bool == false)
            return;

        playingSource.PlayClipOneShotAtIndex(1, true);
    }

    public void PlayCollect()
    {
        if (canPlaySound.Bool == false)
            return;

        playingSource.PlayClipOneShotAtIndex(2, true);
    }
}
