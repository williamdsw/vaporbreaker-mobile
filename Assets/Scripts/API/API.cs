using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    public class API
    {
        public static IEnumerator Get(string path, Action<byte[]> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(path))
            {
                yield return request.SendWebRequest();

                try
                {
                    if (string.IsNullOrEmpty(request.error))
                    {
                        callback(request.downloadHandler.data);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("API::Get -> {0}", ex.Message);
                    throw ex;
                }
            }
        }
    }
}