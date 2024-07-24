using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 소리 재생을 위한 컴포넌트
/// </summary>
public sealed class SoundManager : ManagerClassBase<SoundManager>
{
    /// <summary>
    /// 생성된 효과음 객체를 저장해둘 리스트
    /// </summary>
    public List<AudioSource> _EffectSoundPool = new List<AudioSource>();






    public AudioSource PlayEffectSound(string effectSoundID, Vector3 position)
    {
        // 추가된 사운드 객체가 존재한다면
        if(_EffectSoundPool.Count > 0)
        {
            // 현재 재생중이지 않은 사운드 객체를 찾습니다.
            AudioSource createdAudioSource =
                _EffectSoundPool.Find(audiooSource => !audiooSource.isPlaying);

            // 사용 가능한 객체를 찾은 경우 해당 객체를 반환합니다.
            if (createdAudioSource != null)
                return createdAudioSource;
        }

        // AudioSource 컴포넌트를 갖게 될 새로운 게임 오브젝트를 생성합니다.
        GameObject audioObject = new GameObject($"Object_Audio");

        audioObject.transform.position = position;

        // 생성된 게임 오브젝트에 AudioSource 컴포넌트를 추가합니다.
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        // 생성된 사운드 객체를 리스트에 담습니다.
        _EffectSoundPool.Add(audioSource);

        return audioSource;
    }



}
