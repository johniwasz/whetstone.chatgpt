// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Blazor.Test.BazoriseHelper
{
    internal class ModalSharedContext
    {
        /// <summary>
        /// Raises the modals <see cref="OpenIndex"/> and returns the new index.
        /// </summary>
        /// <returns>Returns the new open index.</returns>
        public int RaiseModalOpenIndex()
        {
            return ++OpenIndex;
        }

        /// <summary>
        /// Decrease the modals <see cref="OpenIndex"/> and returns the new index.
        /// </summary>
        /// <returns>Returns the new open index.</returns>
        public int DecreaseModalOpenIndex()
        {
            --OpenIndex;

            if (OpenIndex < 0)
                OpenIndex = 0;

            return OpenIndex;
        }

        /// <summary>
        /// Gets or sets the modal open index.
        /// </summary>
        public int OpenIndex { get; private set; }
    }
}
