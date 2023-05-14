// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.WebhookCommander
{
    internal static class CommandSwitches
    {
        public static readonly string[] ENVIRONMENT = {"e", "environment" };

        public static readonly string[] WEBHOOK = { "wu", "webhookurl" };


        public static readonly string[] WEBHOOKID = { "wi", "webhookid" };

        public static readonly string[] USERID = { "u", "userid" };

    }
}
