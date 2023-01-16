using Blazorise;
using Blazorise.Localization;

using Microsoft.AspNetCore.Components;

namespace Whetstone.ChatGPT.Blazor.App.Layouts
{
    public partial class MainLayout
    {

        [CascadingParameter] 
        protected Theme? Theme { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        Task OnThemeEnabledChanged(bool value)
        {
            if (Theme is null)
                return Task.CompletedTask;

            Theme.Enabled = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        Task OnThemeGradientChanged(bool value)
        {
            if (Theme is null)
                return Task.CompletedTask;

            Theme.IsGradient = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        Task OnThemeRoundedChanged(bool value)
        {
            if (Theme is null)
                return Task.CompletedTask;

            Theme.IsRounded = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        Task OnThemeColorChanged(string value)
        {
            if (Theme is null)
                return Task.CompletedTask;

            Theme.ColorOptions ??= new();

            Theme.BackgroundOptions ??= new();

            Theme.TextColorOptions ??= new();

            Theme.ColorOptions.Primary = value;
            Theme.BackgroundOptions.Primary = value;
            Theme.TextColorOptions.Primary = value;

            Theme.InputOptions ??= new();

            Theme.InputOptions.CheckColor = value;
            Theme.InputOptions.SliderColor = value;

            Theme.SpinKitOptions ??= new();

            Theme.SpinKitOptions.Color = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }


    }
}