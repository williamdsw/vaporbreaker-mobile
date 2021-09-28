using System;
using System.Collections;
using UnityEngine.Networking;

namespace API
{
    /// <summary>
    /// API handler class
    /// </summary>
    public class API
    {
        /// <summary>
        /// Get request data at path
        /// </summary>
        /// <param name="path"> Desired path </param>
        /// <param name="callback"> Callback with data bytes </param>
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
                    throw ex;
                }
            }
        }
    }
}