using System;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;


namespace Environments
{
    public static class EnvironmentManager
    {
        private const string SecretFileName = "secret.xml";

        [Serializable]
        public class SecretData
        {
            public string apiKey;
        }
        
        public static void SaveSecretFile(string apiKey)
        {
            SecretData secretData = new SecretData
            {
                apiKey = apiKey
            };

            Serialize(SecretFileName, secretData);
        }

        private static SecretData LoadSecretFile()
        {
            if (File.Exists(SecretFileName))
            {
                SecretData secretData = Deserialize<SecretData>(SecretFileName);
                Debug.Log("シークレットファイルを読み込みました。");
                return secretData;
            }
            else
            {
                Debug.Log("シークレットファイルが存在しません。");
                return null;
            }
        }
        
        public static string GetApiKey()
        {
            SecretData secretData = LoadSecretFile();
            if (secretData != null)
            {
                return secretData.apiKey;
            }
            else
            {
                return null;
            }
        }

        private static T Serialize<T>(string filename, T data) {
            using var stream = new FileStream(filename, FileMode.Create);
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, data);

            return data;
        }

        private static T Deserialize<T>(string filename)
        {
            using var stream = new FileStream(filename, FileMode.Open);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
    }
}