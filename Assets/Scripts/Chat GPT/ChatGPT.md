# GPT Project

## Class Diagram
ref https://note.com/negipoyoc/n/n88189e590ac3

By using tools such as [mermaid live editor](https://mermaid-js.github.io/mermaid-live-editor/), it is possible to view the class diagram.
```mermaid  
%%%
title: GPT Classes  
%%%  
classDiagram
class ChatGPTConnection {
    - const string _apiUrl
    - const string GPTModel
    - Dictionary<string, string> _headers
    - List<ChatGPTMessageModel> _messageList
    - ChatGPTMessageModel _systemSetting
    - ChatGPTMessageModel _lastMessage
    + ChatGPTMessageModel LastMessage
    + ChatGPTMessageModel SystemSetting
    + ChatGPTConnection(string apiKey, ChatGPTMessageModel systemSetting)
    + SetGPTSetting(string setting)
    + AddGPTSetting(string setting)
    + ResetLog()
    + async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage)
}

class ChatGPTMessageModel{
    - string role
    - string content
}

class ChatGPTCompletionRequestModel{
    - string model
    - List<ChatGPTMessageModel> messages
}

class ChatGPTResponseModel{
    - List<ResponseChoiceModel> choices
}

class ResponseChoiceModel{
    - float _textOffset
    - string _text
    + ChatGPTMessageModel message
}

ChatGPTConnection --> ChatGPTMessageModel
ChatGPTCompletionRequestModel --> ChatGPTMessageModel
ChatGPTResponseModel --> ResponseChoiceModel
```