using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace UPG {

    class Preset {
        private string Caption;
        private string Symbols;
        public Preset(string Caption, string Symbols) {
            this.Caption = Caption;
            this.Symbols = Symbols;
        }
        public string GetCaption() {
            return Caption;
        }
        public string GetSymbols() {
            return Symbols;
        }
        public static Preset GetPresetFromString(string Preset) {
            string[] PresetElements = Preset.Split(",");
            
            if(PresetElements.Length != 2) {
                Console.WriteLine($"Invalid Preset Syntax: {Preset}");
                throw new InvalidPresetSyntaxException();
            }
            //Preset NewPreset = new Preset(PresetElements[0].Trim(), PresetElements[1].Trim());
            return new Preset(PresetElements[0].Trim(), PresetElements[1].Trim());
        }
        public static List<Preset> ParsePresetsFromFile(string Filename = "Presets.txt") {
            string[] PresetsRaw = File.ReadAllLines(Filename);
            List<Preset> Presets = new List<Preset>();
            foreach(string PresetRaw in PresetsRaw) {
                try {
                    Presets.Add(Preset.GetPresetFromString(PresetRaw));
                } catch(InvalidPresetSyntaxException) {
                    throw new InvalidPresetSyntaxException();
                }
            }
            return Presets;
        }
    }

    static class SymbolPresets {
        public static string EngLettersLower = "abcdefghijklmnopqrstuvwxyz";
        public static string EngLettersCapital = "abcdefghijklmnopqrstuvwxyz".ToUpper();

        public static string RusLettersLower = "éöóêåíãøùçõúôûâàïðîëäæýÿ÷ñìèòüáþ";
        public static string RusLettersCapital = RusLettersLower.ToUpper();
        //public static string CapitalLetters = LowerLetters.ToUpper();
        public static string Numbers = "0123456789";
        public static string OtherSymbols = "!@#$%%^&*()_+|\\/{}><?";
    }
    class Set {
        private List<string> Chars {get;set;}
        private List<string> Numbers {get;set;}
        private List<string> OtherSymbols {get;set;}

        public string GetSymbolByLiteral(MaskLiterals Literal) {
            switch(Literal) {
                case MaskLiterals.Any:
                    Random randomizer = new Random();
                    switch(randomizer.Next(3)) {
                        case 0:
                            return GetChar();

                        case 1:
                            return GetNumber();

                        case 2:
                            return GetOther();
                            
                        default:
                            return "";
                    }
                
                case MaskLiterals.Char:
                    return GetChar();
                
                case MaskLiterals.Number:
                    return GetNumber();

                case MaskLiterals.Other:
                    return GetOther();

                default:
                    return "";
                    
            }
        }

        public string GetChar() {
            return GetRandomElement(Chars);
        }
        public string GetNumber() {
            return GetRandomElement(Numbers);
        }
        public string GetOther() {
            return GetRandomElement(OtherSymbols);
        }
        private string GetRandomElement(List<string> Elements) {
            Random randomizer = new Random();
            return Elements[randomizer.Next(Elements.Count - 1)];
        }

        /*public void Show() {
            //Console.WriteLine($"String Set: {Chars.ToString()}\nNumber Set: {Numbers.ToString()}\n Other symbols Set: {OtherSymbols.ToString()}");
            Console.Write("Chars: ");
            ShowList(Chars);
            Console.WriteLine();
            Console.Write("Numbers: ");
            ShowList(Numbers);
            Console.WriteLine();
            Console.Write("Other: ");
            ShowList(OtherSymbols);
            Console.WriteLine();
        }
        private void ShowList(List<string> ShowingList) {
            foreach(string Element in ShowingList) {
                Console.Write($"{Element} ");
            }
        }*/

        public Set(string Symbols) {
            Chars = new List<string>();
            Numbers = new List<string>();
            OtherSymbols = new List<string>();
            InitChars(Symbols);
            InitNumber(Symbols);
            InitOther(Symbols);
        }

        private void InitChars(string Symbols) {
            Regex Pattern = new Regex(@"[a-zA-Z]|[à-ÿÀ-ß]");
            MatchCollection Matches = Pattern.Matches(Symbols);

            if(Matches.Count > 0) {
                foreach(Match match in Matches) {
                    Chars.Add(match.Value);
                }
            }
        }
        
        private void InitNumber(string Symbols) {
            Regex Pattern = new Regex(@"\d");
            MatchCollection Matches = Pattern.Matches(Symbols);

            if(Matches.Count > 0) {
                foreach(Match match in Matches) {
                    Numbers.Add(match.Value);
                }
            }
        }
        private void InitOther(string Symbols) {
            Regex Pattern = new Regex(@"\W");
            MatchCollection Matches = Pattern.Matches(Symbols);

            if(Matches.Count > 0) {
                foreach(Match match in Matches) {
                    OtherSymbols.Add(match.Value);
                }
            }
        }

    }
}