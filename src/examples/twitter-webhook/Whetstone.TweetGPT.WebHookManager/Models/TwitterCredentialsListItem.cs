// SPDX-License-Identifier: MIT
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.TweetGPT.WebHookManager.Models
{
    public class TwitterCredentialListItem
    {
        
        public Guid Id { get; set; }

        
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ConsumerKey { get; set; }

        public string? ConsumerSecret { get; set; }

        public string? AccessToken { get; set; }

        public string? AccessTokenSecret { get; set; }

        public string? BearerToken { get; set; }

        public bool IsEnabled { get; set; }


    }
}
