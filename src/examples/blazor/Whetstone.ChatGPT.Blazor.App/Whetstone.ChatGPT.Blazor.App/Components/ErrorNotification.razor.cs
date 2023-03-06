using Microsoft.AspNetCore.Components;
using System.Net;
using Whetstone.ChatGPT.Models;
using Blazorise;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class ErrorNotification
    {

        [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
        public Exception? Exception
#pragma warning restore BL0007 // Component parameters should be auto properties
        {
            get
            {
                return exception;
            }
            set
            {
                exception = value;
                if (exception is null)
                {
                    errAlert?.Hide();
                }
                else
                {
                    errAlert?.Show();
                }
            }
        }

        [Parameter]
        public EventCallback ErrorNotificationClosed { get; set; } = default!;

        private ChatGPTError? chatError { get; set; } = default;

        private HttpStatusCode? StatusCode { get; set; } = default!;

        private Exception? exception { get; set; } = default!;

        private Alert? errAlert = default!;
       
        protected override void OnParametersSet()
        {
            chatError = null;
            StatusCode = null;

            ChatGPTException? chatGPTException = exception as ChatGPTException;

            if (chatGPTException is not null)
            {
                StatusCode = chatGPTException.StatusCode;

                if (chatGPTException.ChatGPTError is not null)
                {
                    chatError = chatGPTException.ChatGPTError;
                }
            }

            base.OnParametersSet();
        }

        public async Task ErrorClosedAsync()
        {
            await ErrorNotificationClosed.InvokeAsync();
        }
    }
}
