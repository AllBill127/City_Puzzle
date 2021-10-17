

using System;
using System.Text.RegularExpressions;

public static class RegexUtil
{
	public static Regex ValidPassword()
	{
        Regex regex = new Regex("^(?=.*?[a-z])(?=.*?[0-9]).{8,}$"); //lower case, digit, length 8
        return regex;
	}
}