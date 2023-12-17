using System;
using Newtonsoft.Json;

namespace EntityFrameworkCore;

internal static class ObjectExtensions
{
    public static void Dump<T>(this T value) => Console.WriteLine(JsonConvert.SerializeObject(value));
}