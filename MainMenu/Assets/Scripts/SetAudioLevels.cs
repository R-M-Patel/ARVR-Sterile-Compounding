using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudioLevels : MonoBehaviour {

	public AudioMixer mainMixer;                    //Used to hold a reference to the AudioMixer mainMixer


    //Call this function and pass in the float parameter masterLvl to set the volume of the AudioMixerGroup Master in mainMixer
    public void SetMasterLevel(float masterLvl)
	{
		mainMixer.SetFloat("masterVol", masterLvl);
	}

	//Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup SoundFx in mainMixer
	public void SetSfxLevel(float sfxLevel)
	{
		mainMixer.SetFloat("sfxVol", sfxLevel);
	}
}
