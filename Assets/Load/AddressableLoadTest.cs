using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoadTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        var handle = Addressables.LoadAssetAsync<AudioClip>("b");
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GetComponent<AudioSource>().clip = handle.Result;
            GetComponent<AudioSource>().Play();
        }
    }
}