using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Assets.Scripts
{
    public class KeyboardLanguageChanger
    {
        // Define the language identifier for the desired keyboard language (e.g., "0000040D" for Hebrew , "00000409" for English - United States)
        private readonly static Dictionary<string, string> LanguageIdentifiers = new Dictionary<string, string>()
        {
            { "en", "00000409"},
            { "he", "0000040D"},
        };

        // Import the necessary functions from user32.dll (Windows) or libx11 (Linux/macOS)
        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        [DllImport("user32.dll")]
        private static extern bool ActivateKeyboardLayout(IntPtr hkl, uint Flags);


        private const uint KLF_ACTIVATE = 0x00000001;
        private const uint KLF_SUBSTITUTE_OK = 0x00000002;

        public static void ChangeKeyboardLanguage(string identifier = "en")
        {
            // Load the keyboard layout for the specified language code
            IntPtr hkl = LoadKeyboardLayout(LanguageIdentifiers.GetValueOrDefault(identifier ?? "en"), KLF_ACTIVATE);

            // Activate the new keyboard layout
            ActivateKeyboardLayout(hkl, KLF_SUBSTITUTE_OK);
        }
    }
}
