using System;
using System.Diagnostics;
using kasthack.Tools;
using RandomStringGenerator;
namespace ExApp1 {
    class Program {
        static void Main( string[] args ) {
            var string_exps = new[] {
                "Number: {I:D:0:1000}",
                "Char: {C:1:65535}",
                "String: {S:a:3:10}",
                "Repeat: {R:{Test}:1:1}",
            };
            foreach ( var stringExp in string_exps ) {
                var exp = ExpressionParser.Create( stringExp );
                var src = exp.Decompile();
                Debug.Assert(src==stringExp);
                src.Dump();
                exp.Dump();
            }
            Console.ReadLine();
        }
    }
}
