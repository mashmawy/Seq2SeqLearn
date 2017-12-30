using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seq2SeqLearn
{
     
    [Serializable]
    public class WeightMatrix
    {
        public int Rows { get; set; }
        public int Columns { get; set; } 
        public double[] Weight { get; set; }
        public double[] Gradient { get; set; }
        public double[] Cash { get; set; }

        public WeightMatrix( )
        {
          
        }
        public WeightMatrix(double[] weights)
        {
            this.Rows = weights.Length;
            this.Columns = 1; 
            this.Weight = new double[this.Rows];
            this.Gradient = new double[this.Rows];
            this.Cash = new double[this.Rows];
              this.Weight = weights ;
             
        }

        public WeightMatrix(int rows, int columns,  bool normal=false)
        {
            this.Rows = rows;
            this.Columns = columns; 
            var n = rows * columns  ;
            this.Weight = new double[n];
            this.Gradient = new double[n];
            this.Cash = new double[n];

            var scale = Math.Sqrt(1.0 / (rows * columns ));
            if (normal)
            {
                scale = 0.08;
            }
            for (int i = 0; i < n; i++)
            {
                this.Weight[i] = RandomGenerator.NormalRandom(0.0, scale);  
            }

        }

        public WeightMatrix(int rows, int columns, double c)
        {
            this.Rows = rows;
            this.Columns = columns; 
            var n = rows * columns  ;
            this.Weight = new double[n];
            this.Gradient = new double[n];

            this.Cash = new double[n]; 
            for (int i = 0; i < n; i++)
            {
                this.Weight[i] = c;
            }

        }

        public override string ToString()
        {
            
            return "{"+Rows.ToString()+","+Columns.ToString()+"}";
        }
        public double Get(int x, int y)
        {
            var ix = ((this.Columns * x) + y)  ;
            return this.Weight[ix];
        }

        public void Set(int x, int y, double v)
        {
            var ix = ((this.Columns * x) + y)  ;
              this.Weight[ix]=v;
        }

        public void Add(int x, int y, double v)
        {
            var ix = ((this.Columns * x) + y)  ;
            this.Weight[ix] += v;
        }

        public double Get_Grad(int x, int y )
        {
            var ix = ((this.Columns * x) + y)  ;
            return this.Gradient[ix];
        }

        public void Set_Grad(int x, int y,   double v)
        {
            var ix = ((this.Columns * x) + y)  ;
            this.Gradient[ix] = v;
        }

        public void Add_Grad(int x, int y,  double v)
        {
            var ix = ((this.Columns * x) + y)  ;
            this.Gradient[ix] += v;
        }

        public WeightMatrix CloneAndZero()
        {
            return new WeightMatrix(this.Rows, this.Columns,   0.0);

        }

        public WeightMatrix Clone()
        {
            var v= new WeightMatrix(this.Rows, this.Columns,  0.0);
            var n = this.Weight.Length;
            for (int i = 0; i < n; i++)
            {
                v.Weight[i] = this.Weight[i];
            }
            return v;
        }
 
    }




}
