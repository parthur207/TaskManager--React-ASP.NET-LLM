using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskManager.Core.ValueObjects
{
    public class EmailVO
    {
        protected EmailVO() { }

        [JsonInclude]
        public string Value { get; init; }

        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public EmailVO(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Email inválido.");

            Value = value.Trim().ToLowerInvariant();
        }

        private static bool IsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }
    }
}
