using Blazorise.Localization;
using Microsoft.AspNetCore.Components;

namespace Whetstone.ChatGPT.Blazor.App.Components.Layout
{
    public partial class TopMenu
    {
        [Inject]
        protected ITextLocalizerService LocalizationService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await SelectCulture("en-US");

            await base.OnInitializedAsync();
        }

        Task SelectCulture(string name)
        {
            LocalizationService.ChangeLanguage(name);

            return Task.CompletedTask;
        }
    }
}