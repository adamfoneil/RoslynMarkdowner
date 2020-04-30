using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace MarkdownViewer.App.Extensions
{
    public static class RequestExtensions
    {
        /// <summary>
        /// thanks to https://stackoverflow.com/a/44775206/2023653
        /// </summary>
        public static bool IsLocal(this HttpContext context)
        {
            if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
            {
                return true;
            }

            if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
            {
                return true;
            }

            return false;
        }
    }
}
