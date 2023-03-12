using AngleSharp.Dom;
using Blazorise.Bootstrap5;
using Blazorise;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Blazor.App;
using Whetstone.ChatGPT.Blazor.App.Components;
using Whetstone.ChatGPT.Blazor.App.State;
using Blazorise.Icons.FontAwesome;
using Whetstone.ChatGPT.Blazor.Test.BlazoriseHelper;

namespace Whetstone.ChatGPT.Blazor.Test
{
    public class AuthenticationStatusTests : TestContext
    {

        public AuthenticationStatusTests()
        {
            BlazoriseConfig.AddBootstrapProviders(Services);
            BlazoriseConfig.JSInterop.AddButton(this.JSInterop);
            BlazoriseConfig.JSInterop.AddTextEdit(this.JSInterop);
            Services
                .AddBootstrap5Providers();
        }

        [Fact(Skip = "Blazorise mocks not working")]
        public void RenderAuthenticationContext()
        {
            ApplicationState appState = new ApplicationState();

            Mock<IOpenAICredentialValidator> validatorMoq = new();

            // Register services
            Services.AddSingleton(validatorMoq.Object);

            // Arrange
            var authStatus = RenderComponent<AuthenticationStatus>(parameters => parameters
                .Add(p => p.AppState, new ApplicationState()));


            // Assert that content of the paragraph shows counter at zero
            IRefreshableElementCollection <IElement> foundButtons = authStatus.FindAll("button");


        }
    }
}
