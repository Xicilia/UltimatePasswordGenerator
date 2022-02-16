using System;
using System.Text.RegularExpressions;
/*

    Password Length: Number or Number-Number (10 or 5-10)

    Mask: String contains SpecSymbols (ccssddccssdd/snus/)

    Literals:
        c: char
        s: special symbol
        d: number
        -text- : plain text
    
    Repeater:
        Literal{n} - repeat Literal n times (repeating plain text is unsupported)

    Symbol Set: Preset or own created


*/

namespace UPG
{
    class Program {
        static void Main(string[] args)
        {
            Console.WriteLine("Started");

            GeneratorInfo Info = GetInitialInfo();
            Generator generator = new Generator(Info);
            int MaskSize = generator.Info.GeneratorMask.Size();

            Console.WriteLine($"Using mask is: {generator.Info.GeneratorMask.GetMaskActual()}");

            if(MaskSize != generator.Info.Length.Item1 && MaskSize != generator.Info.Length.Item1) {
                Console.WriteLine("your mask length always or sometimes is not equal to password length so it will be reduced or supplemented to it");
            }

            Console.WriteLine("\nGenerated Passwords:");
            for(int i = 0; i < 10; i++) {
                Console.WriteLine(generator.GeneratePassword());
            }
            

        }
        public static int GetRandomFromTuple(Tuple<int,int> Values) {
            return new Random().Next(Values.Item1,Values.Item2);
        }
        public static Tuple<int,int> ValidateLength(string Input) {
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
        }
        static GeneratorInfo GetInitialInfo() {
            return new GeneratorInfo();
        }
    }
}
