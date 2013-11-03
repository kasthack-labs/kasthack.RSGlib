// RandomStringGenerator / RandomStringGenerator / ExpressionParser.cs
// Written by kasthack
// ( 2013.09.21 )
using System;
using System.Collections.Generic;
using System.Text;
using RandomStringGenerator.Expressions;
using RandomStringGenerator.Helpers;

namespace RandomStringGenerator {
    public static class ExpressionParser {
        public static unsafe MultiExpression Create( string input ) {
            fixed ( char* inputP = input ) {
                var mov = new Mov(inputP, input.Length);
                return ParseE(ref mov,  new ASCIIEncoding() );
            }
        }
        /*
         * For parsers:
         * passed reader must point to first char after { || reader.Start
         * reader will point to reader.End || next char after }
         */
        private static unsafe MultiExpression ParseE( ref Mov reader, ASCIIEncoding enc = null ) {
            var exprs = new List<IExpression>();            //sub expression list
            var start = reader.Current;                     //expression start
            if ( enc == null ) enc = new ASCIIEncoding();   //encoding for inner expressions
            while ( reader.HasNext ) {              //while !eof
                var c = reader.GetChar();
                if ( c == '}' ) break;              //expression end
                if ( c != '{' ) continue;           //StaticString expression
                var a = AddPrevStringExpression( start, reader.Current, enc );
                if ( a != null )
                    exprs.Add( a );
                exprs.Add( ExpresionSelect( ref reader, enc ) );
                start = reader.Current;
            }
            var b = AddPrevStringExpression( start, reader.Current, enc );
            if ( b != null )
                exprs.Add( b );
            return new MultiExpression { Expressions = exprs.ToArray() };
        }
        //Validator regex: \{I:[DH]:[0-9]+:[0-9]+\}
        private static unsafe IntExpression ParseIntE( ref Mov reader ) {
            var exp = new IntExpression();
            var c = Char.ToUpperInvariant( reader.GetChar() );
            switch ( c ) { //parse int format
                case 'D':
                    exp.Format = NumberFormat.Decimal;
                    break;
                case 'H':
                    exp.Format = NumberFormat.Hex;
                    break;
                default:
                    throw new FormatException(@"Bad int format in Int expression");
            }
            reader.Current++;   //skip separator
            // ParseE Min
            var parseLen = Generators.FindChar( reader.Current, reader.End, ':' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad Int expression(Min)" );
            exp.Min = Generators.QIntParse( reader.Current, parseLen );
            reader.Current += parseLen + 1; //skip min + separator
            // ParseE Max
            parseLen = Generators.FindChar( reader.Current, reader.End, '}' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad Int expression(Max)" );
            exp.Max = Generators.QIntParse( reader.Current, parseLen );
            var p = reader.Current + parseLen;     //skip max + closing bracket
            reader.Current = p < reader.End ? p : reader.End;
            return exp;
        }
        //Validator regex: \{C:[0-9]+:[0-9]+\}
        private static unsafe CharExpression ParseCharE( ref Mov reader ) {
            var exp = new CharExpression();
            // ParseE Min
            var parseLen = Generators.FindChar( reader.Current, reader.End, ':' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad Char expression(Min)" );
            exp.Min = Generators.QIntParse( reader.Current, parseLen );
            reader.Current += parseLen + 1; //skip min + separator
            // ParseE Max
            parseLen = Generators.FindChar( reader.Current, reader.End, '}' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad Char expression(Max)" );
            exp.Max = Generators.QIntParse( reader.Current, parseLen );
            var p = reader.Current + parseLen;     //skip max + closing bracket
            reader.Current = p < reader.End ? p : reader.End;
            return exp;
        }
        //Validator regex: \{R:\{.*\}:[0-9]+:[0-9]+\}
        private static unsafe RepeatExpression ParseRepeat( ref Mov reader, ASCIIEncoding enc = null ) {
            enc = enc ?? new ASCIIEncoding();
            reader.Current++;   //skip inner expression opening bracket
            var exp = new RepeatExpression {
                Expressions = ParseE( ref reader, enc ).
                    Expressions
            };
            reader.Current++;   //skip separator
            // ParseE Min
            var parseLen = Generators.FindChar( reader.Current, reader.End, ':' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad String expression(Min)" );
            exp.Min = Generators.QIntParse( reader.Current, parseLen );
            reader.Current += parseLen + 1; //skip min + separator
            // ParseE Max
            parseLen = Generators.FindChar( reader.Current, reader.End, '}' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad String expression(Max)" );
            exp.Max = Generators.QIntParse( reader.Current, parseLen );
            var p = reader.Current + parseLen + 1;     //skip max + closing bracket
            reader.Current = p < reader.End ? p : reader.End;
            return exp;
        }
        //Validator regex: \{S:[DHLaRSAU]:[0-9]+:[0-9]+\}
        private static unsafe StringExpression ParseStringE( ref Mov reader ) {
            var exp = new StringExpression();
            switch ( reader.GetChar() ) {
                case 'D':
                    exp.Format = StringFormat.Decimal;
                    break;
                case 'H':
                    exp.Format = StringFormat.Hexadecimal;
                    break;
                case 'L':
                    exp.Format = StringFormat.Letters;
                    break;
                case 'a':
                    exp.Format = StringFormat.LowerCase;
                    break;
                case 'R':
                    exp.Format = StringFormat.Random;
                    break;
                case 'S':
                    exp.Format = StringFormat.Std;
                    break;
                case 'A':
                    exp.Format = StringFormat.UpperCase;
                    break;
                case 'U':
                    exp.Format = StringFormat.Urlencode;
                    break;
                default:
                    throw new FormatException( "Bad string format" );
            }
            reader.Current++;   //skip separator
            // ParseE Min
            var parseLen = Generators.FindChar( reader.Current, reader.End, ':' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad String expression(Min)" );
            exp.Min = Generators.QIntParse( reader.Current, parseLen );
            reader.Current += parseLen + 1; //skip min + separator
            // ParseE Max
            parseLen = Generators.FindChar( reader.Current, reader.End, '}' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad String expression(Max)" );
            exp.Max = Generators.QIntParse( reader.Current, parseLen );
            
            var p = reader.Current + parseLen + 1;     //skip max + closing bracket
            reader.Current = p < reader.End ? p : reader.End;
            return exp;
        }
        //passed reader must point to expression type parameter
        //Validator regex: \{[a-zA-Z]\:\.*
        private static unsafe IExpression ExpresionSelect( ref Mov reader, ASCIIEncoding enc = null ) {
            enc = enc ?? new ASCIIEncoding();
            var c = reader.GetChar();
            reader.Current++;   //skip separator
            switch ( c ) {
                case 'I':
                    return ParseIntE( ref reader ); //works
                case 'C':
                    return ParseCharE( ref reader );
                case 'S':
                    return ParseStringE( ref reader );
                case 'R':
                    return ParseRepeat( ref reader, enc ); //works
                case '}':
                case '{':
                    if ( reader.GetChar( false ) != '}' )
                        throw new FormatException( @"Escaped bracket is not propely terminated" );
                    return new StaticASCIIStringExpression( c.ToString(), enc );
                default:
                    throw new FormatException( @"Not supported expression" );
            }
        }

        unsafe private static StaticASCIIStringExpression AddPrevStringExpression( char* start, char* current, ASCIIEncoding enc ) {
            var prevSSEL = (int) ( current - 1 - start ); //static string expression exists before expression start?
            return ( prevSSEL > 0 ) ? new StaticASCIIStringExpression( new string( start, 0, prevSSEL ), enc ) : null;
        }

    }
}