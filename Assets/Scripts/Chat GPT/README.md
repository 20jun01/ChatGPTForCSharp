﻿# GPT Project

## Class Diagram
ref https://note.com/negipoyoc/n/n88189e590ac3

By using tools such as [mermaid live editor](https://mermaid-js.github.io/mermaid-live-editor/), it is possible to view the class diagram.
```mermaid  
classDiagram
class ChatGPTConnection {
    - Dictionary<string, string> _headers
    - List<ChatGPTMessageModel> _messageList
    + ChatGPTMessageModel LastMessage
    + ChatGPTMessageModel SystemSetting
    + ChatGPTConnection(string apiKey, ChatGPTMessageModel systemSetting)
    + ResetLog()
    + async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage)
    - HandleResponse(string response)
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
ChatGPTConnection --> ChatGPTCompletionRequestModel
ChatGPTConnection --> ChatGPTResponseModel
```