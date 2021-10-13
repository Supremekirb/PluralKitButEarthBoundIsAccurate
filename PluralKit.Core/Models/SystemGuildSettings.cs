using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace PluralKit.Core
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AutoproxyMode
    {
        Off = 1,
        Front = 2,
        Latch = 3,
        Member = 4
    }

    public class SystemGuildSettings
    {
        public ulong Guild { get; }
        public SystemId System { get; }
        public bool ProxyEnabled { get; } = true;

        public AutoproxyMode AutoproxyMode { get; } = AutoproxyMode.Off;
        public MemberId? AutoproxyMember { get; }

        public string? Tag { get; }
        public bool TagEnabled { get; }
    }

    public static class SystemGuildExt
    {
        public static JObject ToJson(this SystemGuildSettings settings, string? memberHid = null)
        {
            var o = new JObject();

            o.Add("proxying_enabled", settings.ProxyEnabled);
            o.Add("autoproxy_mode", settings.AutoproxyMode.ToString().ToLower());
            o.Add("autoproxy_member", memberHid);
            o.Add("tag", settings.Tag);
            o.Add("tag_enabled", settings.TagEnabled);

            return o;
        }

        public static AutoproxyMode? ParseAutoproxyMode(this JToken o)
        {
            if (o.Type == JTokenType.Null)
                return AutoproxyMode.Off;
            else if (o.Type != JTokenType.String)
                return null;

            var value = o.Value<string>();

            switch (value)
            {
                case "off":
                    return AutoproxyMode.Off;
                case "front":
                    return AutoproxyMode.Front;
                case "latch":
                    return AutoproxyMode.Latch;
                case "member":
                    return AutoproxyMode.Member;
                default:
                    throw new ValidationError($"Value '{value}' is not a valid autoproxy mode.");
            }
        }
    }
}