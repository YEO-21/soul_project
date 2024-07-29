using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "SoundInfo",
    menuName = "ScriptableObject/SoundInfo",
    order = int.MinValue)]
public sealed class SoundInfoScriptableObject : ScriptableObject
{
    public List<SoundInfo> m_SoundInfo;

    public AudioClip GetAudioClipFromSoundID(string soundID)
        => m_SoundInfo.Find(info => info.m_SoundID == soundID).m_AudioClip;
}

[Serializable]
public struct SoundInfo
{
    [Header("# ¼Ò¸® ID")]
    public string m_SoundID;

    [Header("# Audio Clip")]
    public AudioClip m_AudioClip;
}
