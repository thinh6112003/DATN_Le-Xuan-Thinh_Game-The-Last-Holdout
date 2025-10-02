using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class UserData 
{
    public int currentLevel = 0; // Current Level
    public bool sfxStatus = true; // Sound Effects
    public bool musicStatus = true; // Music
    public bool rungStatus = true; // Vibration
    public float volume = 1.0f; // Volume level
    public List<int> levelStars = new List<int>{0,0,0,0,0}; // key: level, value: stars
    public float currentVolume = 1.0f;
    public bool firstTimePlay = true;
}
