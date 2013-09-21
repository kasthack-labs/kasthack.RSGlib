using System;
using System.Collections.Generic;
using System.Text;
namespace RandomStringGenerator
{
	public static class ExpressionParser
	{
		/// <summary>
		/// Generate random string
		/// syntax:
		/// {I:type:_from:to} - integer
		/// \{I:[DH]:[0-9]+:[0-9]+\}
		/// type
		/// D:dec
		/// H- 0x....
		/// Example:
		/// {I:D:0:1000}
		/// Result example
		/// 384
		/// {C:_from:to} -character
		/// \{C:[0-9]+:[0-9]+\}
		/// Example
		/// {C:1:65535}
		/// Result example
		/// Ё
		/// {S:type:min_length:max_length} -string
		/// \{S:[DHLaRSAU]:[0-9]+:[0-9]+\}
		/// type
		/// D, //0-9
		/// H, //0-f
		/// L, //a-Z
		/// a, //a-z
		/// R, //*
		/// S, //0-Z
		/// A, //A-Z
		/// U //full UTF-8
		/// Example:
		/// {S:a:3:1000}
		/// Result example
		/// gfdfyhtueyrstgdfggfr
		/// {R:{expressions}:min_count:_max_count} - mutiple generator invocation
		/// \{R:\{.*\}:[0-9]+:[0-9]+\}
		/// //2+ level expressions are not supported yet
		/// Example
		/// {R:{{S:L:1:5}={S:U:1:50}&}:1:5}
		/// Result example
		/// werf=%A1%B3&tjy=%5F%9C%2D%42%A1%B3&ertg=%39%7E%E8%B2&
		/// 
		/// 
		/// \{#[ICS](:[a-zA-Z])?:[0-9]+:[0-9]+#\}
		/// Warning! string will NOT be validated
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static unsafe FormattedStringGenerator Parse(string str) {
		    fixed ( char* cp = str ) {
				var p = cp;
			    int len;
			    return Parse(ref p, out len, str.Length, new ASCIIEncoding());
			}
		}

	    private static unsafe RepeatExpression ParseRepeatE(ref char* @from, out int outcount, int maxcount, ASCIIEncoding enc = null) {
			if ( enc == null )
				enc = new ASCIIEncoding();
			@from += 3;
			var exp = new RepeatExpression {
			    Expressions = Parse(ref @from, out outcount, maxcount - 3, enc).
			        Expressions
			};
		    @from += 3;
			outcount += 6;
		    var cnt = Generators.FindChar(@from, @from + maxcount - outcount, ':');
			exp.Min = Generators.QIntParse(@from, cnt);
			@from += cnt + 1;
			cnt = Generators.FindChar(@from, @from + maxcount - outcount, '}');
			exp.Max = Generators.QIntParse(@from, cnt);
			@from += cnt;
			return exp;
		}

	    /// <summary>
	    /// parse IntExpression _from string
	    /// pointer will point to closing } of expression
	    /// </summary>
	    /// <param name="from">pointer to 1st char after opening {</param>
	    /// <param name="outcount">returned value 4 read character _count </param>
	    /// <param name="maxCount"></param>
	    /// <returns>parsed expression</returns>
	    //works
	    private static unsafe IntExpression ParseIntE(ref char* @from, out int outcount, int maxCount) {
			/*
			* TODO: add string validation
			*/
			#region Variables

	        var end = @from + maxCount;
			outcount = 0;
			var exp = new IntExpression();
			#endregion
			#region Parse Format
			switch ( *( @from += 2 ) )//skip expression type+separator
			{
				case 'D':
				case 'd':
					exp.Format = NumberFormat.Decimal;
					break;
				case 'H':
				case 'h':
					exp.Format = NumberFormat.Hex;
					break;
			}
			#endregion
			@from += 2;//skip format+separator
			outcount += 4;//total move
			#region Parse Min
			//get min value length
			var cnt = Generators.FindChar(@from, end, ':');
			//parse min length
			exp.Min = Generators.QIntParse(@from, cnt);
			@from += cnt + 1;//skip separator
			outcount += cnt + 1;//add skip 4 min
			#endregion
			#region Parse Max
			//same for max
			cnt = Generators.FindChar(@from, end, '}');
			exp.Max = Generators.QIntParse(@from, cnt);
			@from += cnt;
			outcount += cnt;
			//skip closing bracket
			//_from++;
			#endregion
			return exp;
		}
		/// <summary>
		/// Parses string as ExpressionTree
		/// </summary>
		/// <param name="from">pointer to __start parsing</param>
		/// <param name="outcount">__output to save move __offset</param>
		/// <param name="enc">encoding instanse for generated expressions</param>
		/// <param name="maxCount">max string parse length</param>
		/// <returns>expression tree</returns>
		//works
		private unsafe static FormattedStringGenerator Parse(ref char* @from, out int outcount, int maxCount, ASCIIEncoding enc = null) {
			#region Variables
			var exprs = new List<IExpression>();
			char* start = @from,
			end = @from + maxCount;
			int cnt;
			if ( enc == null )
				enc = new ASCIIEncoding();
			outcount = 0;
			#endregion
			#region Parse
			while ( @from < end ) {
				if ( *@from == '}' ) break;
				if ( *@from == '{' ) {
					#region Add prev string
					if ( --@from >= start )
						exprs.Add(new StaticASCIIStringExpression(new string(start, 0, (int)( @from + 1 - start )), enc));
					@from++;
					#endregion
					exprs.Add(ExpresionSelect(ref @from, out cnt, (int)( end - @from ), enc));
					outcount += cnt;
					start = @from + 1;
				}
				@from++;
				outcount++;
			}
			#endregion
			#region Ending string
		    if ( --@from >= start )
		        exprs.Add(new StaticASCIIStringExpression(new string(start, 0, (int) ( @from + 1 - start )), enc));
		    @from++;
			#endregion
			return new FormattedStringGenerator { Expressions = exprs.ToArray() };
		}

	    private static unsafe StringExpression ParseStringE(ref char* @from, out int outcount, int maxCount) {
			/*
			* TODO: add string validation
			*/
			#region Variables

		    var end = @from + maxCount;
			outcount = 0;
			var exp = new StringExpression();
			#endregion
			#region Parse Format
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
				default: throw new FormatException("Bad string format");
			}
			#endregion
			@from += 2;//skip format+separator
			outcount += 4;//total move
			#region Parse Min
			//get min value length
			var cnt = Generators.FindChar(@from, end, ':');
			//parse min length
			exp.Min = Generators.QIntParse(@from, cnt);
			@from += cnt + 1;//skip separator
			outcount += cnt + 1;//add skip 4 min
			#endregion
			#region Parse Max
			//same for max
			cnt = Generators.FindChar(@from, end, '}');
			exp.Max = Generators.QIntParse(@from, cnt);
			@from += cnt;
			outcount += cnt;
			//skip closing bracket
			//_from++;
			#endregion
			return exp;
		}

	    private static unsafe CharExpression ParseCharE(ref char* @from, out int outcount, int maxCount) {
			/*
			* TODO: add string validation
			*/
			#region Variables

		    var end = @from + maxCount;
			var exp = new CharExpression();
			#endregion
			@from += 2;//skip format+separator
			outcount = 2;//total move
			#region Parse Min
			//get min value length
			var cnt = Generators.FindChar(@from, end, ':');
			//parse min length
			exp.Min = Generators.QIntParse(@from, cnt);
			@from += cnt + 1;//skip separator
			outcount += cnt + 1;//add skip 4 min
			#endregion
			#region Parse Max
			//same for max
			cnt = Generators.FindChar(@from, end, '}');
			exp.Max = Generators.QIntParse(@from, cnt);
			@from += cnt;
			outcount += cnt;
			//skip closing bracket
			//_from++;
			#endregion
			return exp;
		}

	    private static unsafe IExpression ExpresionSelect(ref char* @from, out int outcount, int maxCount, ASCIIEncoding enc = null) {
			enc = enc ?? new ASCIIEncoding();
			outcount = 0;
			maxCount--;
			IExpression expr;
			#region Parse
			switch ( *++@from ) {
				case 'I':
					expr = ParseIntE(ref @from, out outcount, maxCount);//works
					break;
				case 'C':
					expr = ParseCharE(ref @from, out outcount, maxCount);
					break;
				case 'S':
					expr = ParseStringE(ref @from, out outcount, maxCount);
					break;
				case 'R':
					expr = ParseRepeatE(ref @from, out outcount, maxCount, enc);//works
					break;
				default:
					throw new FormatException("Not supported expression");
			}
			#endregion
			outcount++;
			return expr;
		}
	}
}
