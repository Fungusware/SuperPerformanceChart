using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SuperPerformanceChart
{
    public class StringFormatter
    {
        private readonly IFormatProvider _formatProvider;
        private readonly string _mask;

        private Dictionary<string, ParameterInfo> Parameters { get; set; }

        public StringFormatter(string mask, IFormatProvider formatProvider = null)
        {
            Parameters = new Dictionary<string, ParameterInfo>();

            var regex = new Regex(@"@(\w+)?");
            var matches = regex.Matches(mask);
            foreach (Match match in matches)
            {
                Parameters.Add(match.Groups[0].Value, new ParameterInfo() { FullMatch = match.Groups[0].Value, Value = null });
            }

            _formatProvider = formatProvider;
            _mask = mask;
        }

        public bool HasParameter(string key)
        {
            return Parameters.ContainsKey(key);
        }

        public bool Set(string key, object val)
        {
            if (Parameters.ContainsKey(key))
            {
                Parameters[key].Value = val;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Parameters.Aggregate(_mask, (current, parameter) => current.Replace(parameter.Value.FullMatch, parameter.Value.Value?.ToString()));
        }

        private class ParameterInfo
        {
            public object Value { get; internal set; }
            public string FullMatch { get; internal set; }
        }
    }
}