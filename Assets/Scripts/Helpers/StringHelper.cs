using System;
using System.Text.RegularExpressions;

public static class StringHelper
{
    public static string RemoveWhitespace(this string s)
    {
        return Regex.Replace(s, @"\s+", "");
    }
    
    public static string IntToBits(this int number)
    {
        return Convert.ToString(number, 2);
    }

    public static string FirstWord(this string sentence)
    {
        string[] words = sentence.Split(" ");
        if (words.Length > 0)
            return words[0];
        return sentence;
    }
}
