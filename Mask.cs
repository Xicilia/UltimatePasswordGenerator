using System;
using System.Collections.Generic;

namespace UPG {
    
    enum MaskLiterals {
        Any,
        PlainText,
        Char,
        Number,
        Other
    }

    class Mask {

        private List<MaskLiterals> Literals;
        private string RawMask;
        private List<string> PlainTexts;
        public string GetRaw(){
            return RawMask;
        }
        public string GetMaskActual() {
            string ActualMask = "";
            foreach(MaskLiterals Literal in Literals) {
                switch(Literal) {
                    case MaskLiterals.Any:
                        ActualMask += "*";
                        break;
                    case MaskLiterals.Char:
                        ActualMask += "c";
                        break;
                    case MaskLiterals.Number:
                        ActualMask += "d";
                        break;
                    case MaskLiterals.Other:
                        ActualMask += "s";
                        break;
                    case MaskLiterals.PlainText:
                        ActualMask += "P";
                        break;
                }
            }
            return ActualMask;
        }
        public string GetPlainTextByIndex(int index) {
            return PlainTexts[index];
        }
        public int Size(){
            return Literals.Count;
        }
        private void RepeatLastLiteral(int Times) {
            MaskLiterals LastLiteral  = Literals[Literals.Count - 1];
            for(int i = 0; i < Times - 1; i++) {
                Literals.Add(LastLiteral);
            }
        }
        public Mask(string RawMaskInfo) {
            RawMask = RawMaskInfo;
            Literals = new List<MaskLiterals>();
            PlainTexts = new List<string>();

            int LiteralIndex = 0;
            char[] MaskInfo = RawMaskInfo.ToCharArray();

            while (LiteralIndex < MaskInfo.Length) {
                char CurrentLiteral = MaskInfo[LiteralIndex];

                switch(CurrentLiteral) {
                    case '*':
                       Literals.Add(MaskLiterals.Any);
                       break;
                   case 'c':
                       Literals.Add(MaskLiterals.Char);
                       break;
                   case 'd':
                       Literals.Add(MaskLiterals.Number);
                       break;
                   case 's':
                       Literals.Add(MaskLiterals.Other);
                       break;
                   case '-':
                        LiteralIndex++;
                        char NextSymbol = MaskInfo[LiteralIndex];
                        if(NextSymbol == '-') {
                            Console.WriteLine("Plain text can't be empty");
                            throw new MaskLiteralNotFoundException($"");
                        }
                        string PlainText = "";
                        while(NextSymbol != '-') {
                            PlainText += NextSymbol.ToString();
                            LiteralIndex++;
                            try{
                                NextSymbol = MaskInfo[LiteralIndex];
                            } catch(IndexOutOfRangeException) {
                                Console.WriteLine("Unclosed plain text");
                                throw new MaskLiteralNotFoundException($"");
                            }
                        }
                        Literals.Add(MaskLiterals.PlainText);
                        PlainTexts.Add(PlainText);
                        //Console.WriteLine($"added plain text: {PlainText}");
                        break;
                    case '{':
                        char PreviousLiteral;
                        try{
                            PreviousLiteral = MaskInfo[LiteralIndex - 1]; 
                        } catch(IndexOutOfRangeException) {
                            Console.WriteLine("Repeater repeats nothing");
                            throw new MaskLiteralNotFoundException($"");
                        }
                        if(PreviousLiteral == '-') {
                            Console.WriteLine("Repeater cant go after plain text");
                            throw new MaskLiteralNotFoundException($"");
                        }
                        LiteralIndex++;
                        char NextNumber = MaskInfo[LiteralIndex];
                        if(NextNumber == '}') {
                            Console.WriteLine("empty repeater");
                            throw new MaskLiteralNotFoundException($"");
                        }
                        string Repeater = "";
                        while(NextNumber != '}') {
                            Repeater += NextNumber.ToString();
                            LiteralIndex++;
                            try {
                                NextNumber = MaskInfo[LiteralIndex];
                            } catch(IndexOutOfRangeException) {
                                Console.WriteLine("Unclosed repeater");
                                throw new MaskLiteralNotFoundException($"");
                            }
                        }
                        try {
                            Tuple<int,int> Length = Program.ValidateLength(Repeater);
                            RepeatLastLiteral(Program.GetRandomFromTuple(Length));
                        } catch(IncorrectLengthException) {
                            throw new MaskLiteralNotFoundException($"");
                        }
                        break;
                   default:
                       Console.WriteLine($"Unknown literal {CurrentLiteral}");
                       throw new MaskLiteralNotFoundException($"");
                }
                LiteralIndex++;
            }
        }

        public MaskLiterals GetLiteralByIndex(int Index) {
            return Literals[Index];
        }

        public static Mask NormalizeMask(Mask NormalizingMask, int Length) {
            if(NormalizingMask.Literals.Count > Length) {
                return new Mask(NormalizingMask.GetRaw().Substring(0,Length));
            } else if (NormalizingMask.Literals.Count < Length) {
                return new Mask(NormalizingMask.GetRaw() + new String('*',Length - NormalizingMask.Literals.Count));
            }
            return new Mask(NormalizingMask.GetRaw());
        }

        public void ShowMask() {
            Console.WriteLine("Mask elements:");

            foreach(MaskLiterals Literal in Literals) {
                switch(Literal) {
                    case MaskLiterals.Any:
                        Console.WriteLine("Any Literal");
                        break;
                    case MaskLiterals.Char:
                        Console.WriteLine("Char Literal");
                        break;
                    case MaskLiterals.Number:
                        Console.WriteLine("Decimal number Literal");
                        break;
                    case MaskLiterals.Other:
                        Console.WriteLine("Special symbol Literal");
                        break;
                }
            }
        }
    }

}