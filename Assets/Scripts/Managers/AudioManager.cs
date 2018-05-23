using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                if(instance == null)
                {
                    GameObject temp = new GameObject("AudioManager");
                    instance = temp.AddComponent<AudioManager>();
                }
            }

            return instance;
        }
    }

    private AudioClip[] audioClips;

    private void FindAssetBundle()
    {
        List<AssetBundle> abl = AssetManager.Instance.AssetBundlesList;

        AssetBundle ab = AssetManager.Instance.AssetBundlesList.Find(AB => AB.name == "audios");

        audioClips = ab.LoadAllAssets<AudioClip>();
    }

    /// <summary>
    /// Find AudioClip by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(string name)
    {
        if(audioClips == null)
        {
            Debug.LogError("AudioClips array is empty. Find AssetBundle first!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif        
            return null;
        }

        AudioClip result = null;

        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name.ToLower() == name.ToLower())
            {
                result = audioClips[i];
                break;
            }
        }

        if(result == null)
        {
            Debug.LogError("Could not find AudioClip: " + name );
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif  
        }

        return result;
    }

    public AudioClip[] GetAudioClips(string match)
    {
        if (audioClips == null)
        {
            Debug.LogError("AudioClips array is empty. Find AssetBundle first!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif        
            return null;
        }

        List<AudioClip> result = new List<AudioClip>();

        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name.ToLower().Contains(match.ToLower()))
            {
                result.Add(audioClips[i]);
            }
        }

        if (result == null)
        {
            Debug.LogError("Could not find AudioClip that contains: " + match);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif  
        }

        return result.ToArray();
    }


    private void Start()
    {
        FindAssetBundle();
    }
}
