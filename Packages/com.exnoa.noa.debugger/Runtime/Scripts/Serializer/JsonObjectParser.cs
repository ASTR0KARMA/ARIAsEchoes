using System.Text;

namespace NoaDebugger
{
    sealed class JsonObjectParser: IKeyValueParser
    {
        string _key;
        string _json;

        public JsonObjectParser(string key, string json)
        {
            _key = key;
            _json = json;
        }

        public string ToJsonString()
        {
            var builder = new StringBuilder();
            builder.Append($"\"{_key}\":{_json}");
            return builder.ToString();
        }
    }
}
