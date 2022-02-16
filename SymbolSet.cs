using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UPG {

    static class SymbolPresets {
        public static string Letters = "abcdefghijklmnopqrstuvwxyz" + "abcdefghijklmnopqrstuvwxyz".ToUpper();
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
            Regex Pattern = new Regex(@"[a-zA-Z]");
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