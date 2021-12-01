using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CityPuzzle.Classes
{
    public class RegexDelegate
    {
        public static bool ValidUsername(string text)
        {
            string x = "^(?=.{6,12}$)(?![_.])(?!.*[_.]{2})[a-z0-9._]+(?<![_.])$";
            Regex reg =  new Regex(x, RegexOptions.IgnoreCase);
            return reg.IsMatch(text);
        }

        public static bool ValidPassword(string text)
        {
            string x = "(?=.*[0-9])^(?=.*[a-z])^(?=.*[A-Z])^([a-zA-Z0-9]{8,15})";
            Regex reg = new Regex(x, RegexOptions.None);
            return reg.IsMatch(text);
        }

        public static bool ValidEmail(string text)
        {
            string x = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            Regex reg = new Regex(x, RegexOptions.IgnoreCase);
            return reg.IsMatch(text);
        }
    }
}
