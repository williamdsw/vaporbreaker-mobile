using System;
using System.IO;

namespace Utilities
{
    public class FileManager
    {
        /// <summary>
        /// File exists ?
        /// </summary>
        /// <param name="path"> Path of requested file </param>
        /// <returns> true | false </returns>
        public static bool Exists(string path) => File.Exists(path);

        /// <summary>
        /// Copy file from source path to destination path
        /// </summary>
        /// <param name="source"> Source path of File </param>
        /// <param name="destination"> Destinarion path of File </param>
        /// <returns> true | false </returns>
        public static bool Copy(string source, string destination)
        {
            try
            {
                File.Copy(source, destination);
                return Exists(destination);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Write all bytes to a file
        /// </summary>
        /// <param name="path"> File path </param>
        /// <param name="bytes"> Bytes to be written </param>
        /// <returns> true | false </returns>
        public static bool WriteAllBytes(string path, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(path, bytes);
                return Exists(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete file from path
        /// </summary>
        /// <param name="path"> File's path </param>
        public static void Delete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}