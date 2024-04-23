using Microsoft.AspNetCore.Http;
using FantaxyWebApplication.Models;
using System.Text.Json;
using Newtonsoft.Json;

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
    }


    public static int? GetInt(this ISession session, string key)
    {
        string? valueString = session.GetString(key);

        if (string.IsNullOrEmpty(valueString))
        {
            return -1;
        }

        try
        {
            return int.Parse(valueString);
        }
        catch (FormatException ex)
        {
            // Handle error if required
            // For example, logging or removing the invalid key
            session.Remove(key);
            return -1;
        }
    }
    public static T? Get<T>(this ISession session, string key)
    {
        string? valueString = session.GetString(key);

        if (string.IsNullOrEmpty(valueString))
        {
            return default(T);
        }

        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(valueString);
        }
        catch (System.Text.Json.JsonException ex)
        {
            // Handle error if required
            // For example, logging or removing the invalid key
            session.Remove(key);
            return default(T);
        }
    }

    public static FileModel? GetObject<FileModel>(this ISession session, string key)
    {
        string? valueString = session.GetString(key);

        if (string.IsNullOrEmpty(valueString))
        {
            return default(FileModel);
        }

        try
        {
            return (FileModel)JsonConvert.DeserializeObject(valueString, typeof(FileModel));
        }
        catch (System.Text.Json.JsonException ex)
        {
            // Handle error if required
            // For example, logging or removing the invalid key
            session.Remove(key);
            return default(FileModel);
        }
    }
}