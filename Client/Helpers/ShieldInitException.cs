using System;
using System.Collections.Generic;
using System.Text;

namespace Shield.Client.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ShieldInitException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ShieldInitException(string message) : base(message)
        {
            
        }
    }
}
