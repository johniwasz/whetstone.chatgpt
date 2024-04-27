// SPDX-License-Identifier: MIT
using AngleSharp.Dom;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Blazor.App.Components;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Blazor.App;
using Whetstone.ChatGPT.Blazor.Test.BlazoriseHelper;

namespace Whetstone.ChatGPT.Blazor.Test
{
    public class ErrorNotificationComponentTests : TestContext
    {
        public ErrorNotificationComponentTests()
        {
            BlazoriseConfig.AddBootstrapProviders(Services);
            BlazoriseConfig.JSInterop.AddClosable(this.JSInterop);
        }

        [Fact(Skip ="Not worth testing")]
        public void RenderErrorNotification()
        {
            string errorMessage = "This is a test";

            // Arrange
            var errorNotification = RenderComponent<ErrorNotification>(parameters => parameters
                .Add(p => p.Exception, new Exception(errorMessage)));
            
            IRefreshableElementCollection<IElement> foundDivs = errorNotification.FindAll("div");

            Assert.True(foundDivs.Count == 2);

            foundDivs[1].MarkupMatches($"<div class=\"row\">Message: {errorMessage}</div>");
        }
    }
}
