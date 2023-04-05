using UnityEngine;
using Chat_GPT;
using Environments;

namespace Operator
{
    public class OperatorObject : MonoBehaviour
    {
        private string _openAIApiKey;

        private void Awake()
        {
            _openAIApiKey = EnvironmentManager.Instance.GetConfig("OpenAI_API_KEY");
        }

        async void Start()
        {
            var chatGptConnection = new ChatGPTConnection(_openAIApiKey);
            var response = await chatGptConnection.RequestAsync("{{AIに言いたいことをここに書く}}");
        }
    }
}