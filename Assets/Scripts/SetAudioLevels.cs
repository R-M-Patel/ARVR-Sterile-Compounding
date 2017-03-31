using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudioLevels : MonoBehaviour {

	public AudioMixer MasterMixer;                    //Used to hold a reference to the AudioMixer mainMixer


    //Call this function and pass in the float parameter masterLvl to set the volume of the AudioMixerGroup Master in mainMixer
    public void SetMasterLevel(float masterLvl)
    {
        MasterMixer.SetFloat("masterVol", masterLvl);
    }

    //Call this function and pass in the float parameter menuLvl to set the volume of the AudioMixerGroup MenuSounds in mainMixer
    public void SetMenuSoundsLevel(float menuLvl)
	{
        MasterMixer.SetFloat("menuVol", menuLvl);
	}

	//Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup InGameSoundFx in mainMixer
	public void SetSfxLevel(float sfxLevel)
	{
        MasterMixer.SetFloat("sfxVol", sfxLevel);
	}
}
