using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance { get; private set; }

    [System.Serializable]

    public class EffectData
    {
        public string effectName;
        public GameObject effectPrefabs;
        public float defaultDuration = 2f;
    }

    [Header("이펙트 목록")]
    [SerializeField] private List<EffectData> effectList = new List<EffectData>();
    private Dictionary<string, EffectData> effectDictionary = new Dictionary<string, EffectData>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDictionary()
    {
        effectList.Clear();
        foreach(var effect in effectList)
        {
            if (!effectDictionary.ContainsKey(effect.effectName))
            {
                effectDictionary.Add(effect.effectName, effect);
            }
            else
            {
                Debug.LogWarning($"중복된 이펙트 이름 : { effect.effectName}");
            }
        }
    }

    public GameObject PlayEffect(string effectName , Vector3 position, Quaternion rotation)
    {
        if (effectDictionary.TryGetValue(effectName, out EffectData data))
        {
            GameObject effect = Instantiate(data.effectPrefabs, position, rotation);
            Destroy(effect, data.defaultDuration);
            return effect;
        }
        else
        {
            Debug.LogWarning($"이펙트를 찾을 수 없습니다. {effectName}");
            return null;
        }
    }

    public GameObject PlayEffect(string effectName, Vector3 position, Quaternion rotation, float duration)
    {
        if (effectDictionary.TryGetValue(effectName, out EffectData data))
        {
            GameObject effect = Instantiate(data.effectPrefabs, position, rotation);
            Destroy(effect, duration);
            return effect;
        }
        else
        {
            Debug.LogWarning($"이펙트를 찾을 수 없습니다. {effectName}");
            return null;
        }
    }

    public GameObject PlayEffect(string effectName, Vector3 position, float duration)
    {
        return PlayEffect(effectName, position, quaternion.identity, duration);
    }

    public IEnumerator PlayEffectDelayed (string effectName, Vector3 position, Quaternion rotation, float delay,float duration)
    {
        yield return new WaitForSeconds(delay);

        if(duration > 0)
        {
            PlayEffect(effectName, position, rotation, duration);
        }
        else
        {
            PlayEffect(effectName, position, rotation);
        }
    }

}
