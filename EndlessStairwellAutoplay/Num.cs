using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EndlessStairwellAutoplay
{
	internal class Num
	{
		enum Mag
		{ 
			exp
		}

		// the magnitude
		Mag mag;

		// most significant to lowest
		List<double> val= new List<double>();


		public override string ToString()
		{
			return $"Num: {mag} { string.Join(",",val )}";
		}

		public static Num From( long lng )
		{
			Num n = new Num();
			n.mag= Mag.exp;

			double e = Math.Log10(lng);
			n.val.Add( e );
			n.val.Add(lng / Math.Pow(10,e) );
			return n;
		}
		public static Num From(double v)
		{
			Num n= new Num();	
			n.mag = Mag.exp;
			double e = Math.Log10(v);
			n.val.Add(e);
			n.val.Add(v / Math.Pow(10, e));
			return n;
		}

		public static bool operator >=( Num left , long right ) 
		{
			return GreaterOrEqual(left, Num.From(right));
		}

		public static bool operator <=(Num left, long right)
		{
			return GreaterOrEqual(Num.From(right), left );
		}

		public static bool operator <=(Num left, Num right)
		{
			return GreaterOrEqual(right, left);
		}

		public static bool operator >=(Num left, Num right)
		{
			return GreaterOrEqual(left, right);
		}
		public static bool operator >(Num left, Num right)
		{
			return Greater(left, right);
		}

		public static bool operator <(Num left, Num right)
		{
			return Greater(right, left);
		}

		public static bool GreaterOrEqual(Num left, Num right)
		{
			return Greater(left, right) || Equal(left, right);
		}

		public static bool Greater(Num left, Num right)
		{
			if (left.mag > right.mag)
				return true;

			if (left.mag == right.mag )
			{
				for (int i = 0; i < left.val.Count; i++)
				{
					if (left.val[i] > right.val[i])
						return true;
					if (left.val[i] < right.val[i])
						return false;
				}
			}

			return false;
		}


		public static bool Equal(Num left, Num right)
		{
			if (left.mag != right.mag)
				return false;

			for (int i = 0; i < left.val.Count; i++)
				if (left.val[i] != right.val[i])
					return false;
				
			return true;
		}

		public static Num Parse( string s )
		{
			Num n = new Num();
			s = s.Replace(",", "");

			Regex reg0 = new Regex("^([\\d\\.]+)$");
			Regex reg1 = new Regex("([\\d\\.]+)(e(\\d+))?");

			var m = reg0.Match(s);

			if( m.Success ) 
			{
				n.mag = Mag.exp;
				double v = double.Parse(s);
				double e = Math.Log10(v);
				n.val.Add(e);
				n.val.Add(v / Math.Pow(10, e));
			}
			else
			{
				m= reg1.Match(s);

				if( m.Success )
				{
					n.mag = Mag.exp;
					n.val.Add(Double.Parse(m.Groups[3].Value));
					n.val.Add(Double.Parse(m.Groups[1].Value));
				}
			}

			return n;
		}

		public static void DoTests()
		{
			Debug.Assert(Num.Equal( Num.From(100), Num.Parse("100") ));
			Debug.Assert(Num.From(1e200) > Num.Parse("1e100"));
		}
	}
}
