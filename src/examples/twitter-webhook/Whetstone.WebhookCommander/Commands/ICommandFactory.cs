// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.WebhookCommander.Commands;

internal interface ICommandFactory
{
    ICommand GetCommand<T>() where T : ICommand?;
}
