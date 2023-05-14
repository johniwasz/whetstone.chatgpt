// SPDX-License-Identifier: MIT
using Microsoft.AspNetCore.Components;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class ProfilePicture
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
