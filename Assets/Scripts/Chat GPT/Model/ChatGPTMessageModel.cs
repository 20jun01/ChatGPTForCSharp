using System;
using System.Collections.Generic;

namespace Chat_GPT.Model
{
    [Serializable]
    public class ChatGPTMessageModel
    {
        public string role;
        public string content;
    }
}
