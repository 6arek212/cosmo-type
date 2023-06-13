using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;

namespace Assets.Scripts
{

    public enum LangKey
    {
        [Description("en")]
        ENGLISH,
        [Description("he")]
        HEBREW
    }


    public static class KeyboardTranslation
    {
        private readonly static Dictionary<char, char> HETranslationMap = new Dictionary<char, char>()
        {
              { 'a', 'ש' },
            { 'b', 'נ' },
            { 'c', 'ב' },
            { 'd', 'ג' },
            { 'e', 'ק' },
            { 'f', 'כ' },
            { 'g', 'ע' },
            { 'h', 'י' },
            { 'i', 'ן' },
            { 'j', 'ח' },
            { 'k', 'ל' },
            { 'l', 'ך' },
            { 'm', 'צ' },
            { 'n', 'מ' },
            { 'o', 'ם' },
            { 'p', 'פ' },
            { 'q', '/' },
            { 'r', 'ר' },
            { 's', 'ד' },
            { 't', 'א' },
            { 'u', 'ו' },
            { 'v', 'ה' },
            { 'w', '\'' },
            { 'x', 'ס' },
            { 'y', 'ט' },
            { 'z', 'ז' },
            { '`', 'ף' },
            { '1', '!' },
            { '2', '@' },
            { '3', '#' },
            { '4', '$' },
            { '5', '%' },
            { '6', '^' },
            { '7', '&' },
            { '8', '*' },
            { '9', '(' },
            { '0', ')' },
            { '-', '_' },
            { '=', '+' },
            { '[', '}' },
            { ']', '{' },
            { '\\', '|' },
            { ';', ':' },
            { '\'', '"' },
            { ',', 'ת' },
            { '.', 'ץ' },
            { '/', '?' },
            { ' ', ' ' }
        };


        public static char Translate(this char ch, LangKey langKey)
        {
            if (langKey == LangKey.HEBREW && HETranslationMap.ContainsKey(ch))
                return HETranslationMap[ch];
            return ch;
        }

    }
}
