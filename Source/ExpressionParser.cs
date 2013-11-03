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
        public static unsafe FormattedStringGenerator Create( string input ) {
            fixed ( char* inputP = input )
                return ParseE( new Mov( inputP, input.Length ), new ASCIIEncoding() );
        }

        //passe reader must point to first char after { || reader.Start
        //reader will point to reader.End || next char after }
        private static unsafe FormattedStringGenerator ParseE( Mov reader, ASCIIEncoding enc = null ) {
            var exprs = new List<IExpression>();            //sub expression list
            var start = reader.Current;                     //expression start
            if ( enc == null ) enc = new ASCIIEncoding();   //encoding for inner expressions
            Action addPrevStringExpression = () => {
                var a = AddPrevStringExpression( start, reader.Current, enc );
                if ( a != null )
                    exprs.Add( a );
            };
            while ( reader.HasNext ) {              //while !eof
                var c = reader.GetChar();
                if ( c == '}' ) break;              //expression end
                if ( c != '{' ) continue;           //StaticString expression
                addPrevStringExpression();
                exprs.Add( ExpresionSelect( reader, enc ) );
            }
            addPrevStringExpression();
            return new FormattedStringGenerator { Expressions = exprs.ToArray() };
        }

        unsafe private static StaticASCIIStringExpression AddPrevStringExpression( char* start, char* current, ASCIIEncoding enc ) {
            var prevSSEL = (int) ( current - 1 - start ); //static string expression exists before expression start?
            return ( prevSSEL > 0 ) ? new StaticASCIIStringExpression( new string( start, 0, prevSSEL ), enc ) : null;
        }
        
        //passed reader must point first char of the first parameter
        //reader will point to reader.End || next char after }
        //Validation regex: \{I:[DH]:[0-9]+:[0-9]+\}
        private static unsafe IntExpression ParseIntE( Mov reader ) {
            var exp = new IntExpression();
            switch ( Char.ToUpperInvariant(reader.GetChar())) { //parse int format
                case 'd':
                    exp.Format = NumberFormat.Decimal;
                    break;
                case 'h':
                    exp.Format = NumberFormat.Hex;
                    break;
            }
            reader.Current++;   //skip separator
            // ParseE Min
            var parseLen = Generators.FindChar(reader.Current, reader.End, ':');
            if (parseLen<=0) throw new FormatException("Bad Int expression(Min)");
            exp.Min = Generators.QIntParse( reader.Current, parseLen );
            reader.Current+= parseLen + 1; //skip min + separator
            // ParseE Max
            parseLen = Generators.FindChar( reader.Current, reader.End, '}' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad Int expression(Max)" );
            exp.Min = Generators.QIntParse( reader.Current, parseLen );
            reader.Current += parseLen;     //skip max + closing bracket
            return exp;
        }

        //passed reader must point first char of the first parameter
        //reader will point to reader.End || next char after }
        //\{C:[0-9]+:[0-9]+\}
        private static unsafe CharExpression ParseCharE( Mov reader ) {
            var exp = new CharExpression();

            // ParseE Min
            var parseLen = Generators.FindChar( reader.Current, reader.End, ':' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad Int expression(Min)" );
            exp.Min = Generators.QIntParse( reader.Current, parseLen );
            reader.Current += parseLen + 1; //skip min + separator
            // ParseE Max
            parseLen = Generators.FindChar( reader.Current, reader.End, '}' );
            if ( parseLen <= 0 ) throw new FormatException( "Bad Int expression(Max)" );
            exp.Min = Generators.QIntParse( reader.Current, parseLen );
            reader.Current += parseLen;     //skip max + closing bracket
            return exp;
        }
        
        private static unsafe RepeatExpression ParseRepeat( Mov reader, ASCIIEncoding enc = null ) {
            enc = enc ?? new ASCIIEncoding();
            @from += 3;
            var exp = new RepeatExpression {
                Expressions = ParseE( ref @from, out outcount, end - 3, enc ).
                    Expressions
            };
            @from += 3;
            outcount += 6;
            var cnt = Generators.FindChar( @from, @from + maxcount - outcount, ':' );
            exp.Min = Generators.QIntParse( @from, cnt );
            @from += cnt + 1;
            cnt = Generators.FindChar( @from, @from + maxcount - outcount, '}' );
            exp.Max = Generators.QIntParse( @from, cnt );
            @from += cnt;
            return exp;
        }
        
        
        private static unsafe StringExpression ParseStringE( Mov reader ) {
            /*
            * TODO: add string validation
            */
            #region Variables
            var end = @from + maxCount;
            outcount = 0;
            var exp = new StringExpression();
            #endregion
            #region ParseE Format
            switch ( *( @from += 2 ) ) {
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
            #endregion
            @from += 2; //skip format+separator
            outcount += 4; //total move
            #region ParseE Min
            //get min value length
            var cnt = Generators.FindChar( @from, end, ':' );
            //parse min length
            exp.Min = Generators.QIntParse( @from, cnt );
            @from += cnt + 1; //skip separator
            outcount += cnt + 1; //add skip 4 min
            #endregion
            #region ParseE Max
            //same for max
            cnt = Generators.FindChar( @from, end, '}' );
            exp.Max = Generators.QIntParse( @from, cnt );
            @from += cnt;
            outcount += cnt;
            //skip closing bracket
            //_from++;
            #endregion
            return exp;
        }
        private static IExpression ExpresionSelect( Mov reader, ASCIIEncoding enc = null ) {
            enc = enc ?? new ASCIIEncoding();
            IExpression expr;
            var c = reader.GetChar();
            switch ( c ) {
                case 'I':
                    expr = ParseIntE( reader ); //works
                    break;
                case 'C':
                    expr = ParseCharE( reader );
                    break;
                case 'S':
                    expr = ParseStringE( reader );
                    break;
                case 'R':
                    expr = ParseRepeat( reader, enc ); //works
                    break;
                case '}':
                case '{':

                    break;
                default:
                    throw new FormatException( "Not supported expression" );
            }
            return expr;
        }
    }
}