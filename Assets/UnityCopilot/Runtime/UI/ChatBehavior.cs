using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatBehavior : MonoBehaviour
{
    public UnityCopilot Copilot;
    public UIDocument ChatUi;
    public VisualTreeAsset UserMessage;
    public VisualTreeAsset BotMessage;

    private TextField textField;
    private ScrollView scrollView;
    private Button sendButton;

    private void Awake()
    {
        textField = ChatUi.rootVisualElement.Query<TextField>();
        scrollView = ChatUi.rootVisualElement.Query<ScrollView>();
        sendButton = ChatUi.rootVisualElement.Query<Button>();

        sendButton.clicked += async () => await SendChatMessage();
        textField.RegisterCallback<KeyDownEvent>(OnTextFieldKeyDown);
    }

    private void OnTextFieldKeyDown(KeyDownEvent evt)
    {
        if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
        {
            _ = SendChatMessage();
            textField.value = "";
            evt.StopPropagation();
        }
    }

    private async Task SendChatMessage()
    {
        // Save prompt
        var prompt = textField.value;
        textField.value = "";

        // Add user message
        VisualElement messageElement = UserMessage.CloneTree();
        var userLabel = messageElement.Q<Label>();
        userLabel.text = prompt;
        scrollView.Add(messageElement);
        scrollView.verticalScroller.value = scrollView.verticalScroller.highValue;

        var handle = Copilot.kernel.AutoExecuteCopilotFunctionStreamedAsync(prompt);
        VisualElement botMessageElement = BotMessage.CloneTree();
        var botLabel = botMessageElement.Q<Label>();
        botLabel.text = "Thinking...";
        scrollView.Add(botMessageElement);
        var finishedThinking = false;
        await foreach (var item in handle)
        {
            if(item == string.Empty) continue;

            if(finishedThinking == false)
            {
                finishedThinking = true;
                botLabel.text = string.Empty;
            }

            botLabel.text += item;
            await Task.Delay(100);
        }


        //// Start Task Non Streamed
        //var handle = Copilot.kernel.AutoExecuteCopilotFunctionAsync(prompt);

        //// Add Bot Message Waiting...
        //VisualElement botMessageElement = BotMessage.CloneTree();
        //var botLabel = botMessageElement.Q<Label>();
        //botLabel.text = "Thinking";
        //scrollView.Add(botMessageElement);

        //// Thinking animation
        //while (handle.Status != TaskStatus.RanToCompletion) 
        //{
        //    if(botLabel.text.Contains("..."))
        //        botLabel.text = "Thinking";
        //    else
        //        botLabel.text += ".";
        //    await Task.Delay(250);
        //}

        //// Show bot message
        //var answer = await handle;
        //botLabel.text = answer;
        //scrollView.verticalScroller.value = scrollView.verticalScroller.highValue;
    }
}
