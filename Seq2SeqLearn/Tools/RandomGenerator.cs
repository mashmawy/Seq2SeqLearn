using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seq2SeqLearn
{
    [Serializable]
    public static class RandomGenerator
    {

        public static bool Return_V { get; set; }
        public static double V_Val { get; set; }

        private static Random random = new Random(3);
        public static double GaussRandom()
        {
            if (Return_V)
            {
                Return_V = false;
                return V_Val;
            }
            var u = 2 * random.NextDouble() - 1;
            var v = 2 * random.NextDouble() - 1;
            var r = (u * u) + (v * v);

            if (r == 0 || r > 1) return GaussRandom();
            var c = Math.Sqrt(-2 * Math.Log(r) / r);
            V_Val = v * c;
            Return_V = true;
            return u * c;
        }

        public static double FloatRandom(double a, double b)
        {

            return random.NextDouble() * (b - a) + a;
        
        }

        public static double IntegarRandom(double a, double b)
        { 
            return Math.Floor(  random.NextDouble() * (b - a) + a); 
        }
        public static double NormalRandom(double mu, double std)
        {
            return mu + GaussRandom() * std;
        }

            

    }
     
}
