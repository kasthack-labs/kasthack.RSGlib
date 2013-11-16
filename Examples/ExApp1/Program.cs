using System;
using System.Diagnostics;
using kasthack.Tools;
using RandomStringGenerator;
namespace ExApp1 {
    class Program {
        static void Main( string[] args ) {
            var string_exps = new[] {
                //"Number: {I:D:0:1000}",
                //"Char: {C:1:65535}",
                //"String: {S:a:3:10}",
                //"Repeat: {R:{Test}:1:1}",
@"GET / HTTP/1.1
Host: ya.ru
Accept: */*
Connection: close
Accept-Encoding: gzip,deflate
Mozilla/5.0 (Windows NT 6.{I:D:0:2}) AppleWebKit/{I:D:536:537}.{I:D:0:35} (KHTML, like Gecko) Chrome/{I:D:25:35}.0.{I:D:1100:1800}.{I:D:0:10} Safari/{I:D:536:537}.{I:D:0:35}

"
            };
            foreach ( var stringExp in string_exps ) {
                var exp = ExpressionParser.Create( stringExp );
                var src = exp.Decompile();
                Debug.Assert( src == stringExp ); 
                //if (src!=stringExp)
                //    GetFirstBreakIndex( stringExp, src ).Dump();
                ConTools.WriteMessage( src );
                exp.Dump();
            }
            Console.ReadLine();
        }
        public static int GetFirstBreakIndex( string a, string b, bool handleLengthDifference = true ) {
            const int equalsReturnCode = -1;
            if ( String.IsNullOrEmpty( a ) || String.IsNullOrEmpty( b ) )
                return handleLengthDifference ? 0 : equalsReturnCode;
            var longest = b.Length > a.Length ? b : a;
            var shorten = b.Length > a.Length ? a : b;
            for (var i = 0; i < shorten.Length; i++)
                if ( shorten[ i ] != longest[ i ] )
                    return i;
            if ( handleLengthDifference && a.Length != b.Length )
                return shorten.Length;
            return equalsReturnCode;
        }
    }
}
