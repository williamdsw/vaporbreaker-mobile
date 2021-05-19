using System;
using System.IO;
using UnityEngine;

namespace Utilities
{
    public class FileManager
    {
        public static bool Exists(string path) => File.Exists(path);

        public static bool Copy(string source, string destination)
        {
            try
            {
                File.Copy(source, destination);
                return Exists(destination);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("FileManager::Copy -> {0}", ex.Message);
                throw ex;
            }
        }

        public static bool WriteAllBytes(string path, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(path, bytes);
                return Exists(path);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("FileManager::WriteAllBytes -> {0}", ex.Message);
                throw ex;
            }
        }
    }
}