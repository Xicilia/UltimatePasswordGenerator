using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace UPG {
    class GeneratorInfo {
        public Tuple<int,int> Length {
            get;
            set;
        }
        //public List<Mask> GeneratorMask {get;set;}
        public Mask GeneratorMask {get;set;}

        public Set SymbolSet {get;set;}
        public GeneratorInfo() {
            Console.Write("Enter password length (enter an empty string if you want to use mask length): ");
            bool IsCorrect = false;
            while (!IsCorrect) {
                string Input = Console.ReadLine();

                if(Input.Length == 0) {
                    Length = new Tuple<int, int>(-1,-1);
                    Console.WriteLine("\nGenerator will use mask length");
                    break;
                } 

                try {
                    Length = Program.ValidateLength(Input);

                } catch(IncorrectLengthException) {
                    continue;
                }
                IsCorrect = true;
            }

            Console.WriteLine("\n1.Use symbol set presets\n2.Create own symbol set");
            IsCorrect = false;
            bool Presets = true;
            while(!IsCorrect) {
                ConsoleKeyInfo Key =  Console.ReadKey();

                if(Key.KeyChar == '1') {
                    IsCorrect = true;
                } else if(Key.KeyChar == '2') {
                    Presets = false;
                    IsCorrect = true;
                }
                
            }

            IsCorrect = false;
            string SymbolsRaw = "";
            if(Presets) {
                try {
                    List<Preset> PresetsList = Preset.ParsePresetsFromFile();
                    foreach(Preset CurrentPreset in PresetsList) {
                        Console.WriteLine($"{PresetsList.IndexOf(CurrentPreset) + 1}. {CurrentPreset.GetCaption()}");
                    }
                    Console.WriteLine();
                    while (!IsCorrect) {
                        Console.WriteLine("Enter string contains number of presets you want to use");
                        string  PresetString = Console.ReadLine();
                        bool IsPresetCorrect = true;
                        foreach(char CurrentPreset in PresetString) {
                            try {
                                int PresetNumber = int.Parse(CurrentPreset.ToString());
                                if(PresetNumber <= 0 || PresetNumber > PresetsList.Count) {
                                    Console.WriteLine($"Numbers can only be between 1 and {PresetsList.Count}");
                                    IsPresetCorrect = false;
                                    break;
                                }
                                SymbolsRaw += PresetsList[PresetNumber - 1].GetSymbols();
                                //Console.WriteLine(PresetNumber);
                            } catch {
                                Console.WriteLine($"Invalid syntax: {CurrentPreset}");
                                IsPresetCorrect = false;
                                break;
                            }
                        }
                        IsCorrect = IsPresetCorrect;
                    }
                } catch(InvalidPresetSyntaxException) {
                    Console.WriteLine("Finded problems in Presets.txt, create your own symbol set");
                    Presets = false;
                }
            }
            if(!Presets) {
                Console.WriteLine("Enter string with any symbols you want to use");
                while (!IsCorrect) {
                    SymbolsRaw = Console.ReadLine();
                    if(SymbolsRaw.Length == 0) {
                        Console.WriteLine("Cant be null");
                        continue;
                    }
                    IsCorrect = true;
                }
            }
            /*if(Presets) {
                Console.WriteLine("\nEnter string contains only number of presets you want to use\n1.Letters\n2.Numbers\n3.Special symbols\nEnter empty string if you want to use all of them");
                string Input = Console.ReadLine();
                
                if(Input.Contains('1')) {
                    SymbolsRaw += SymbolPresets.Letters;
                }
                if(Input.Contains('2')) {
                    SymbolsRaw += SymbolPresets.Numbers;
                }
                if(Input.Contains('3')) {
                    SymbolsRaw += SymbolPresets.OtherSymbols;
                }
                if(SymbolsRaw.Length == 0) {
                    SymbolsRaw = SymbolPresets.Letters + SymbolPresets.Numbers + SymbolPresets.OtherSymbols;
                }

            } else {
                Console.WriteLine("Enter string with any symbols you want to use");
                while (!IsCorrect) {
                    SymbolsRaw = Console.ReadLine();
                    if(SymbolsRaw.Length == 0) {
                        Console.WriteLine("Cant be null");
                        continue;
                    }
                    IsCorrect = true;
                }
            }*/
            Console.WriteLine($"Using set is {SymbolsRaw}");
            SymbolSet = new Set(SymbolsRaw);
            //Console.ReadLine();

            IsCorrect = false;
            while (!IsCorrect) {
                Console.Write("\nEnter generator mask: ");
                string RawMask = Console.ReadLine();
                try{
                    GeneratorMask = new Mask(RawMask);

                } catch(MaskLiteralNotFoundException) {
                    continue;

                }
                IsCorrect = true;
            }
            //GeneratorMask.ShowMask();
        }

        /*private Tuple<int,int> ParseLength(string Input) {
            if(Regex.IsMatch(Input,@"^-?\d+$")) {
                int Length = int.Parse(Input);
                if(Length <= 0) {
                    Console.WriteLine("Length should be greater than 0");
                    throw new IncorrectLengthException();
                }
                return new Tuple<int, int>(Length,Length);

            } else if(Regex.IsMatch(Input,@"^-?\d+:-?\d+$")) {
                string[] LengthInput = Input.Split(":");
                Tuple<int,int> Length = new Tuple<int, int>(int.Parse(LengthInput[0]),int.Parse(LengthInput[1]));

                if(Length.Item1 <= 0 || Length.Item2 <= 0) {
                    Console.WriteLine("Length items should be greater than 0");
                    throw new IncorrectLengthException();
                } else if(Length.Item1 > Length.Item2) {
                    Console.WriteLine("Second number of length should be greater than first");
                    throw new IncorrectLengthException();
                } else if(Length.Item1 == Length.Item2) {
                    Console.WriteLine($"Instead of using {Input} use only {Length.Item1}");
                    throw new IncorrectLengthException();
                }
                return Length;

            } else {
                Console.WriteLine($"Unknown input: {Input}");
                throw new IncorrectLengthException();
            }
        }*/
    }

    class Generator {
        public GeneratorInfo Info {get;set;}
        public Generator(GeneratorInfo Info) {
            this.Info = Info;
        }

        public string GeneratePassword() {
            string Password = "";
            Random randomizer = new Random();
            //int Length = randomizer.Next(Info.Length.Item1,Info.Length.Item2);
            int Length;

            if (Info.Length.Item1 == -1) {
                Length = Info.GeneratorMask.GetMaskActual().Length;
            } else {
                Length = Program.GetRandomFromTuple(Info.Length);
            }

            Mask CurrentMask = Mask.NormalizeMask(Info.GeneratorMask,Length);

            int PlainTextIndex = 0;

            //Console.WriteLine("Using Mask: " + CurrentMask.GetRaw());
            for (int i = 0; i < Length; i++) {
                MaskLiterals Literal = CurrentMask.GetLiteralByIndex(i);
                if (Literal == MaskLiterals.PlainText) {
                    Password += CurrentMask.GetPlainTextByIndex(PlainTextIndex);
                    PlainTextIndex++;
                } else {
                    Password += Info.SymbolSet.GetSymbolByLiteral(Literal);
                }
                
            }

            return Password;
        }
    }
}