using System;
using System.Collections.Generic;
using System.Text;
using Chat_GPT.Model;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Chat_GPT
{
    public class ChatGPTConnection
    {
        // api settings
        private const string APIUrl = "https://api.openai.com/v1/chat/completions";
        private const string GptModel = "gpt-3.5-turbo";
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>
            {
                {"Content-type", "application/json"},
                {"X-Slack-No-Retry", "1"}
            };

        //会話履歴を保持するリスト
        private readonly List<ChatGPTMessageModel> _messageList = new();

        private ChatGPTMessageModel LastMessage { get; set; }

        private ChatGPTMessageModel SystemSetting { get; }

        public ChatGPTConnection(string apiKey = "", ChatGPTMessageModel systemSetting = null)
        {
            _headers["Authorization"] = "Bearer " + apiKey;
            SystemSetting = systemSetting?? new ChatGPTMessageModel(){role = "system", content = "語尾に「にゃ」をつけて"};
        }

        public void SetGptSetting(string setting)
        {
            SystemSetting.content = setting;
        }

        public void AddGptSetting(string setting)
        {
            SystemSetting.content += setting;
        }

        public void ResetLog()
        {
            _messageList.Clear();
        }

        public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage)
        {
            _messageList.Add(new ChatGPTMessageModel {role = "user", content = userMessage});

            //文章生成で利用するモデルやトークン上限、プロンプトをオプションに設定
            var options = new ChatGPTCompletionRequestModel()
            {
                model = GptModel,
                messages = _messageList
            };
            var jsonOptions = JsonUtility.ToJson(options);

            Debug.Log("自分:" + userMessage);

            //OpenAIの文章生成(Completion)にAPIリクエストを送り、結果を変数に格納
            using var request = new UnityWebRequest(APIUrl, "POST")
            {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
                downloadHandler = new DownloadHandlerBuffer()
            };

            foreach (var header in _headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                throw new Exception();
            }
            else
            {
                var responseString = request.downloadHandler.text;
                var responseObject = JsonUtility.FromJson<ChatGPTResponseModel>(responseString);
                Debug.Log("ChatGPT:" + responseObject.choices[0].message.content);
                _messageList.Add(responseObject.choices[0].message);
                LastMessage = responseObject.choices[0].message;
                return responseObject;
            }
        }
    }
}