using System;
using kasthack.Tools;
using RandomStringGenerator;

namespace ExApp1 {
    class Program {
        static void Main( string[] args ) {
            var exp = ExpressionParser.Create("Number: {I:D:0:1000}");
            exp.Dump();
            Console.ReadLine();
        }
    }
}
