using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TicTacToeGameServer.Services.SerializationResolution {
public class CustomJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Dictionary<string, object>) || objectType == typeof(List<object>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        object obj = serializer.Deserialize(reader);
        
        if (obj is Dictionary<string, object> dict)
        {
            return ConvertDictionary(dict);
        }
        if (obj is List<object> list)
        {
            return ConvertList(list);
        }
        return obj;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }

    private Dictionary<string, object> ConvertDictionary(Dictionary<string, object> dict)
    {
        Dictionary<string, object> newDict = new Dictionary<string, object>();

        foreach (var kvp in dict)
        {
            newDict[kvp.Key] = ConvertValue(kvp.Value);
        }
        return newDict;
    }

    private List<object> ConvertList(List<object> list)
    {
        return list.Select(ConvertValue).ToList();
    }

    private object ConvertValue(object value)
    {
        if (value is string strVal)
        {
            if (int.TryParse(strVal, out int intVal))
                return intVal;
            if (double.TryParse(strVal, out double doubleVal))
                return doubleVal;
        }
        else if (value is Dictionary<string, object> dict)
        {
            return ConvertDictionary(dict);
        }
        else if (value is List<object> list)
        {
            return ConvertList(list);
        }
        else if (value is IPAddress ip)
        {
            return ip.ToString();
        }

        return value;
    }
}

}