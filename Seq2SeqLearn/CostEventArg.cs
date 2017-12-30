using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seq2SeqLearn
{
    public class CostEventArg : EventArgs
    {
        public double Cost { get; set; }
         
        public int Iteration { get; set; }
    }
}
