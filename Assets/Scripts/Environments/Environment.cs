using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Environments
{
    public class EnvironmentManager : MonoBehaviour
    {
        private static EnvironmentManager _instance;
        private static readonly object Padlock = new object();

        private Dictionary<string, string> _configs = new Dictionary<string, string>();
        
        private const string ConfigPath = "config.xml";

        public static EnvironmentManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<EnvironmentManager>();
                        _instance.LoadConfig();
                    }

                    return _instance;
                }
            }
        }

        public string GetConfig(string key)
        {
            _configs.TryGetValue(key, out var value);
            return value;
            
        }

        private void LoadConfig()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(ConfigPath);

            XmlElement root = xml.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("/configs/config");

            foreach (XmlNode node in nodes)
            {
                string key = node.Attributes["name"].Value;
                string value = node.Attributes["value"].Value;
                _configs.Add(key, value);
            }
        }

        public void SetConfig(string key, string value)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(ConfigPath);

            XmlElement root = xml.DocumentElement;
            XmlNode node = root.SelectSingleNode($"config[@name='{key}']");
            if (node == null)
            {
                node = xml.CreateElement("config");
                XmlAttribute nameAttr = xml.CreateAttribute("name");
                nameAttr.Value = key;
                XmlAttribute valueAttr = xml.CreateAttribute("value");
                valueAttr.Value = value;
                node.Attributes.Append(nameAttr);
                node.Attributes.Append(valueAttr);
                root.AppendChild(node);
            }
            else
            {
                node.Attributes["value"].Value = value;
            }

            xml.Save(ConfigPath);
        }

        public static bool IsExistFile()
        {
            if (File.Exists(ConfigPath))
            {
                return true;
            }
            else
            {
                XmlDocument xml = new XmlDocument();
                XmlElement root = xml.CreateElement("configs");
                xml.AppendChild(root);

                XmlNode node = xml.CreateElement("config");
                XmlAttribute nameAttr = xml.CreateAttribute("name");
                nameAttr.Value = "ApiKey";
                XmlAttribute valueAttr = xml.CreateAttribute("value");
                valueAttr.Value = "YourApiKey";
                node.Attributes.Append(nameAttr);
                node.Attributes.Append(valueAttr);
                root.AppendChild(node);

                node = xml.CreateElement("config");
                nameAttr = xml.CreateAttribute("name");
                nameAttr.Value = "ApiSecret";
                valueAttr = xml.CreateAttribute("value");
                valueAttr.Value = "YourApiSecret";
                node.Attributes.Append(nameAttr);
                node.Attributes.Append(valueAttr);
                root.AppendChild(node);

                xml.Save(ConfigPath);

                return false;
            }
        }
    }
}