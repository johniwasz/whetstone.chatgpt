using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Blazor.App;
using Whetstone.ChatGPT.Blazor.App.Components;
using Whetstone.ChatGPT.Blazor.App.State;
using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components;
using Whetstone.ChatGPT.Blazor.Test.BlazoriseHelper;

namespace Whetstone.ChatGPT.Blazor.Test
{
    public class LoginTests : TestContext
    {

        public LoginTests()
        {
            

            BlazoriseConfig.AddBootstrapProviders(Services);
            BlazoriseConfig.JSInterop.AddButton(this.JSInterop);
            BlazoriseConfig.JSInterop.AddClosable(this.JSInterop);
        }


        [Fact(Skip = "Blazorise mocks not working")]
        public void LoginButtonIsDisabledWhenNoUsername()
        {

            Mock<IOpenAICredentialValidator> validatorMoq = new();

            // Register services
            Services.AddSingleton(validatorMoq.Object);

            // Arrange
            ApplicationState appState = new ApplicationState();
            var cut = this.RenderComponent<LogIn>(parameters => parameters
                .Add(p => p.AppState, appState));
            
            // Assert that content of the paragraph shows counter at zero
            // IRefreshableElementCollection<IElement> foundButtons = cut.FindAll("button");
                
                //.MarkupMatches("<p>Current count: 0</p>");
        }

    }
}
