using System;
using System.Collections.Generic;
using System.Text;

namespace Shield.Client.Extensions
{
    public static class RouteExtensions
    {

        public static string ToApiRoute(this string path) => $"api{(!path.StartsWith("/") ? "/" : "")}{path}";
    }
}
