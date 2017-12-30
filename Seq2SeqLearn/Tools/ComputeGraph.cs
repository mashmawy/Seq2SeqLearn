using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seq2SeqLearn
{
    public class ComputeGraph
    {
        List<Action> backprop = new List<Action>();

        public bool needs_backprop { get; set; }
        public ComputeGraph(bool needBack = true)
        {
            this.needs_backprop = needBack;
        }
        public WeightMatrix tanh(WeightMatrix m)
        {
            // tanh nonlinearity
            var res = new WeightMatrix(m.Rows, m.Columns,   0);
            var n = m.Weight.Length;
            for (var i = 0; i < n; i++)
            {
                res.Weight[i] = Math.Tanh(m.Weight[i]);
            }

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (var i = 0; i < n; i++)
                    {
                        // grad for z = tanh(x) is (1 - z^2)
                        var mwi = res.Weight[i];
                        m.Gradient[i] += (1.0 - mwi * mwi) * res.Gradient[i];
                    }
                };
                this.backprop.Add(backward);
            }
            return res;
        }
        public WeightMatrix mergeSY(List<WeightMatrix> vols)
        {
            WeightMatrix merged = new WeightMatrix(vols.Count, vols[0].Columns,   0);

            for (int j = 0; j < vols.Count; j++)
			{
                var item = vols[j];
                int n = item.Columns;
                for (int i = 0; i < n; i++)
                {
                    merged.Set(j,i ,   item.Weight[i]);
                 //merged.Set_Grad(j, i, 0, item.DW[i]);

                }
            }
            if (this.needs_backprop)
            {
                Action backward = () =>
                {

                    for (int j = 0; j < vols.Count; j++)
                    {
                        var item = vols[j];
                        int n = item.Columns;
                        for (int i = 0; i < n; i++)
                        {
                            item.Gradient[i] += merged.Get_Grad(j, i );
                        }
                    }
                };
                this.backprop.Add(backward);
            }
            return merged;
        }
        public List<WeightMatrix> splitSY(WeightMatrix merged)
        {
            List<WeightMatrix> vols = new List<WeightMatrix>();
             
            for (int j = 0; j < merged.Rows; j++)
            {
                var item = new WeightMatrix(1, merged.Columns,   0);
                int n = merged.Columns;
                for (int i = 0; i < n; i++)
                {
                    item.Weight[i] = merged.Get(j, i );
                  //item.DW[i] = merged.Get_Grad(j, i, 0);
                }
                vols.Add(item);
            }
            if (this.needs_backprop)
            {
                Action backward = () =>
                {

                    for (int j = 0; j < vols.Count; j++)
                    {
                        var item = vols[j];
                        int n = item.Columns;
                        for (int i = 0; i < n; i++)
                        {
                            merged.Add_Grad(j, i,   item.Gradient[i]);
                        }
                    }
                
                };
                this.backprop.Add(backward);
            }
            return vols;
        }

        public WeightMatrix concatRows(List<WeightMatrix> m1)
        { 

            var res = new WeightMatrix(m1.Count,m1[0].Columns, 0);
          
            for (var i = 0; i < m1.Count; i++)
            {
                for (int j = 0; j < m1[i].Columns; j++)
                {
                    var el = m1[i].Get(0, j);
                    res.Set(i, j, el);
                    //res.Set_Grad(i, j, 0, m1.Get_Grad(i,j,0));
                }
            } 

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (var i = 0; i < m1.Count; i++)
                    {
                        for (int j = 0; j < m1[i].Columns; j++)
                        {

                            var el = res.Get_Grad(i, j);
                            m1[i].Add_Grad(0, j, el);
                        }
                    }  
                    
                };
                this.backprop.Add(backward);
            }
            return res;
        }
        public WeightMatrix RepeatRows( WeightMatrix  m1,int rows)
        {

            var res = new WeightMatrix(rows, m1.Columns, 0);

            for (var i = 0; i < rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    var el = m1.Get(0, j);
                    res.Set(i, j, el);
                  //  res.Set_Grad(i, j,m1.Get_Grad(i,j));
                }
            }

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (var i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < m1.Columns; j++)
                        {

                            var el = res.Get_Grad(i, j);
                            m1.Add_Grad(0, j, el);
                        }
                    }

                };
                this.backprop.Add(backward);
            }
            return res;
        }
      
        public WeightMatrix concatRows(WeightMatrix m1, WeightMatrix m2)
        {
            int sx = 1;
            int sy = 1;
           
                sx = m1.Rows + m2.Rows;
                sy = m1.Columns  ;
             
            var res = new WeightMatrix(sx, sy,   0);
            var n = m1.Weight.Length;
            for (var i = 0; i < m1.Rows; i++)
            { 
                for (int j = 0; j < m1.Columns; j++)
                {
                    var el = m1.Get(i, j );
                    res.Set(i, j,  el);
                   //res.Set_Grad(i, j, 0, m1.Get_Grad(i,j,0));
                } 
            }
            for (var i = m1.Rows; i < m2.Rows + m1.Rows; i++)
            {

                for (int j = 0; j < m2.Columns; j++)
                {
                    var el = m2.Get(i - m1.Rows, j );
                    res.Set(i, j,   el);
                   //res.Set_Grad(i, j, 0, m2.Get_Grad(i - m1.SX, j, 0));
                }
            }

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (var i = 0; i < m1.Rows; i++)
                    {
                        for (int j = 0; j < m1.Columns; j++)
                        {
                            var el = res.Get_Grad(i, j );
                            m1.Add_Grad(i, j,   el);
                        }
                    }
                    for (var i = m1.Rows; i < m2.Rows + m1.Rows; i++)
                    {

                        for (int j = 0; j < m2.Columns; j++)
                        {
                            var el = res.Get_Grad(i, j );
                            m2.Add_Grad(i - m1.Rows, j,   el);
                        }
                    }
                };
                this.backprop.Add(backward);
            }
            return res;
        }
        public WeightMatrix concatColumns(WeightMatrix m1, WeightMatrix m2)
        {
            int sx = 1;
            int sy = 1;

            sy = m1.Columns + m2.Columns;
            sx = m1.Rows;

            var res = new WeightMatrix(sx, sy,   0);
            var n = m1.Weight.Length;
            for (var i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    var el = m1.Get(i, j );
                    res.Set(i, j,   el);
                     //res.Set_Grad(i, j, 0, m1.Get_Grad(i, j, 0));
                }
            }
            for (var i = 0; i < m2.Rows ; i++)
            {

                for (int j = m1.Columns; j < m2.Columns + m1.Columns; j++)
                {
                    var el = m2.Get(i , j- m1.Columns );
                    res.Set(i, j,  el);
                   //res.Set_Grad(i, j, 0, m2.Get_Grad(i, j - m1.SY, 0));
                }
            }

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (var i = 0; i < m1.Rows; i++)
                    {
                        for (int j = 0; j < m1.Columns; j++)
                        {
                            var el = res.Get_Grad(i, j );
                            m1.Add_Grad(i, j,   el);
                        }
                    }
                    for (var i = 0; i < m2.Rows; i++)
                    {

                        for (int j = m1.Columns; j < m2.Columns + m1.Columns; j++)
                        {
                            var el = res.Get_Grad(i, j );
                            m2.Add_Grad(i, j - m1.Columns,   el);
                        }
                    }
                };
                this.backprop.Add(backward);
            }
            return res;
        }
      
        public WeightMatrix rowPluck(WeightMatrix m, int ix)
        {
            var d = m.Columns;
            var res = new WeightMatrix(d, 1,   0);
            for (int i = 0, n = d; i < n; i++) { res.Weight[i] = m.Weight[d * ix + i]; } // copy over the data

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (int i = 0, n = d; i < n; i++) { m.Gradient[d * ix + i] += res.Gradient[i]; }
                };
                this.backprop.Add(backward);
            }
            return res;
        }
        public WeightMatrix PeekRow(WeightMatrix m, int ix)
        {
            var d = m.Columns;
            var res = new WeightMatrix(1,d ,   0);
            for (int i = 0, n = d; i < n; i++) { res.Weight[i] = m.Weight[d * ix + i]; } // copy over the data

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (int i = 0, n = d; i < n; i++) { m.Gradient[d * ix + i] += res.Gradient[i]; }
                };
                this.backprop.Add(backward);
            }
            return res;
        }

        private double sig(double x)
        {
            // helper function for computing sigmoid
            return 1.0 / (1 + Math.Exp(-x));
        }

        public WeightMatrix sigmoid(WeightMatrix m)
        {
            // sigmoid nonlinearity
            WeightMatrix res = new WeightMatrix(m.Rows, m.Columns,   0);
            var n = m.Weight.Length;
            for (var i = 0; i < n; i++)
            {
                res.Weight[i] = sig(m.Weight[i]);
            }

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (var i = 0; i < n; i++)
                    {
                        // grad for z = tanh(x) is (1 - z^2)
                        var mwi = res.Weight[i];
                        m.Gradient[i] += mwi * (1.0 - mwi) * res.Gradient[i];
                    }
                };
                this.backprop.Add(backward);
            }
            return res;
        }

        public WeightMatrix relu(WeightMatrix m)
        {
            var res = new WeightMatrix(m.Rows, m.Columns,   0);
            var n = m.Weight.Length;
            for (var i = 0; i < n; i++)
            {
                res.Weight[i] = Math.Max(0, m.Weight[i]); // relu
            }
            if (this.needs_backprop)
            {
                Action backward = () =>
                        {
                            for (var i = 0; i < n; i++)
                            {
                                m.Gradient[i] += m.Weight[i] > 0 ? res.Gradient[i] : 0.0;
                            }
                        };
                this.backprop.Add(backward);
            }
            return res;
        }

        Random ra = new Random();
        public WeightMatrix Dropout(WeightMatrix V, double drop_prob)
        {
            var res = new WeightMatrix(V.Rows, V.Columns,   0);
            var N = V.Weight.Length;
            bool[] dropped = new bool[V.Rows * V.Columns];
            var V2 = V.Clone(); 
         
                for (var i = 0; i < N; i++)
                {
                    if (ra.NextDouble() < drop_prob) { V2.Weight[i] = 0; dropped[i] = true; } // drop!
                    else {  dropped[i] = false; }
                }

                res = V2; 


            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    var chain_grad = res;
                    V.Gradient = new double[N]; // zero out gradient wrt data
                    for (var i = 0; i < N; i++)
                    {
                        if (!( dropped[i]))
                        {
                            V.Gradient[i] += chain_grad.Gradient[i]; // copy over the gradient
                        }
                    }

                };
                this.backprop.Add(backward);
            }
            return res;
        }
        public WeightMatrix Dropout(WeightMatrix V , bool[] droppedMask)
        {
            var res = new WeightMatrix(V.Rows, V.Columns,   0);
            var N = V.Weight.Length; 
            var V2 = V.Clone();

            for (var i = 0; i < N; i++)
            {
                if (droppedMask[i]) { V2.Weight[i] = 0;  } // drop!
            }

            res = V2;


            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    var chain_grad = res;
                    V.Gradient = new double[N]; // zero out gradient wrt data
                    for (var i = 0; i < N; i++)
                    {
                        if (!(droppedMask[i]))
                        {
                            V.Gradient[i] += chain_grad.Gradient[i]; // copy over the gradient
                        }
                    }

                };
                this.backprop.Add(backward);
            }
            return res;
        }

        public WeightMatrix mul2(WeightMatrix m1, WeightMatrix m2)
        {
            
            var res = mulParalel(m1, m2);

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    DmulParalel(m1, m2, res);
                };
                this.backprop.Add(backward);
            }
            return res;
        }
        public WeightMatrix mul (WeightMatrix m1, WeightMatrix m2)
        {
            var n = m1.Rows;
            var d = m2.Columns;
            var res = new WeightMatrix(n, d,   0);
            for (var i = 0; i < m1.Rows; i++)
            { // loop over rows of m1
                for (var j = 0; j < m2.Columns; j++)
                { // loop over cols of m2
                    var dot = 0.0;
                    for (var k = 0; k < m1.Columns; k++)
                    { // dot product loop
                        dot += m1.Weight[m1.Columns * i + k] * m2.Weight[m2.Columns * k + j];
                    }
                    res.Weight[d * i + j] = dot;
                }
            }
            //  var res = mulParalel(m1, m2);

            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (var i = 0; i < m1.Rows; i++)
                    { // loop over rows of m1
                        for (var j = 0; j < m2.Columns; j++)
                        { // loop over cols of m2
                            for (var k = 0; k < m1.Columns; k++)
                            { // dot product loop
                                var b = res.Gradient[d * i + j];
                                m1.Gradient[m1.Columns * i + k] += m2.Weight[m2.Columns * k + j] * b;
                                m2.Gradient[m2.Columns * k + j] += m1.Weight[m1.Columns * i + k] * b;
                            }
                        }
                    }
                };
                this.backprop.Add(backward);
            }
            return res;
        }

        public WeightMatrix mulParalel(WeightMatrix m1, WeightMatrix m2)
        { 
            var n = m1.Rows;
            var d = m2.Columns;
            var res = new WeightMatrix(n, d,   0);
            var source = Enumerable.Range(0, n);
            var pquery = from num in source.AsParallel()
                         select num;
            pquery.ForAll((e) => MulKernel(m1, m2, d, res, e));
            
            return res;
        }

        private static void MulKernel(WeightMatrix m1, WeightMatrix m2, int d, WeightMatrix res, int i)
        {
            for (var j = 0; j < m2.Columns; j++)
            { // loop over cols of m2
                var dot = 0.0;
                for (var k = 0; k < m1.Columns; k++)
                { // dot product loop
                    dot += m1.Weight[m1.Columns * i + k] * m2.Weight[m2.Columns * k + j];
                }
                res.Weight[d * i + j] = dot;
            }
        }
        public void DmulParalel(WeightMatrix m1, WeightMatrix m2,WeightMatrix res)
        {
            var d = m2.Columns;

            var source = Enumerable.Range(0, m1.Rows);
            var pquery = from num in source.AsParallel()
                         select num;
            pquery.ForAll((e) => DmulKernel(m1, m2, res, d, e));

            
        }

        private static void DmulKernel(WeightMatrix m1, WeightMatrix m2, WeightMatrix res, int d, int i)
        {
            for (var j = 0; j < m2.Columns; j++)
            { // loop over cols of m2
                for (var k = 0; k < m1.Columns; k++)
                { // dot product loop
                    var b = res.Gradient[d * i + j];
                    m1.Gradient[m1.Columns * i + k] += m2.Weight[m2.Columns * k + j] * b;
                    m2.Gradient[m2.Columns * k + j] += m1.Weight[m1.Columns * i + k] * b;
                }
            }
        }


        public WeightMatrix add(WeightMatrix m1, WeightMatrix m2)
        {

            var res = new WeightMatrix(m1.Rows, m1.Columns,   0);
            for (int i = 0, n = m1.Weight.Length; i < n; i++)
            {
                res.Weight[i] = m1.Weight[i] + m2.Weight[i];
            }
            if (this.needs_backprop)
            {

                Action backward = () =>
                {
                    for (int i = 0, n = m1.Weight.Length; i < n; i++)
                    {
                        m1.Gradient[i] += res.Gradient[i];
                        m2.Gradient[i] += res.Gradient[i];
                    }
                };
                this.backprop.Add(backward);
            }
            return res;

        }

        public WeightMatrix eladd(WeightMatrix m1, WeightMatrix m2)
        {

            var res = new WeightMatrix(m1.Rows, m1.Columns,   0);
            for (int i = 0, n = m1.Weight.Length; i < n; i++)
            {
                res.Weight[i] = m1.Weight[i] + m2.Weight[0];
            }
            if (this.needs_backprop)
            {

                Action backward = () =>
                {
                    for (int i = 0, n = m1.Weight.Length; i < n; i++)
                    {
                        m1.Gradient[i] += res.Gradient[i];
                        m2.Gradient[0] += res.Gradient[i];
                    }
                };
                this.backprop.Add(backward);
            }
            return res;

        }

        public WeightMatrix eltmul(WeightMatrix m1, WeightMatrix m2)
        {

            var res = new WeightMatrix(m1.Rows, m1.Columns,   0);
            for (int i = 0, n = m1.Weight.Length; i < n; i++)
            {
                res.Weight[i] = m1.Weight[i] * m2.Weight[i];
            }
            if (this.needs_backprop)
            {

                Action backward = () =>
                {
                    for (int i = 0, n = m1.Weight.Length; i < n; i++)
                    {
                        m1.Gradient[i] += m2.Weight[i] * res.Gradient[i];
                        m2.Gradient[i] += m1.Weight[i] * res.Gradient[i];
                    }
                };
                this.backprop.Add(backward);
            }
            return res;
        }

        public WeightMatrix scalemul(WeightMatrix m1, WeightMatrix m2)
        {

            var res = new WeightMatrix(m1.Rows, m1.Columns,   0);
            for (int i = 0, n = m1.Weight.Length; i < n; i++)
            {
                res.Weight[i] = m1.Weight[i] * m2.Weight[0];
            }
            if (this.needs_backprop)
            {

                Action backward = () =>
                {
                    for (int i = 0, n = m1.Weight.Length; i < n; i++)
                    {
                        m1.Gradient[i] += m2.Weight[0] * res.Gradient[i];
                        m2.Gradient[0] += m1.Weight[i] * res.Gradient[i];
                      
                    }
                };
                this.backprop.Add(backward);
            }
            return res;
        }

        public WeightMatrix SoftmaxWithCrossEntropy(WeightMatrix m)
        {
            var res = new WeightMatrix(m.Rows, m.Columns,   0); // probability volume
            var maxval = -999999.0;
            for (int i = 0, n = m.Weight.Length; i < n; i++)
            {
                if (m.Weight[i] > maxval) maxval = m.Weight[i];
            }

            var s = 0.0;
            for (int i = 0, n = m.Weight.Length; i < n; i++)
            {
                res.Weight[i] = Math.Exp(m.Weight[i] - maxval);
                s += res.Weight[i];
            }
            for (int i = 0, n = m.Weight.Length; i < n; i++) { res.Weight[i] /= s; }

            // no backward pass here needed
            // since we will use the computed probabilities outside
            // to set gradients directly on m
            return res;
        }


        public WeightMatrix Softmax(WeightMatrix m)
        {
            var res = new WeightMatrix(m.Rows, m.Columns, 0); // probability volume
            var maxval = -999999.0;
            for (int i = 0, n = m.Weight.Length; i < n; i++)
            {
                if (m.Weight[i] > maxval) maxval = m.Weight[i];
            }

            var s = 0.0;
            for (int i = 0, n = m.Weight.Length; i < n; i++)
            {
                res.Weight[i] = Math.Exp(m.Weight[i] - maxval);
                s += res.Weight[i];
            }
            for (int i = 0, n = m.Weight.Length; i < n; i++) { res.Weight[i] /= s; }


           
            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                   
                        double ss = 0.0;
                        for (int ix = 0; ix < m.Weight.Length; ix++)
                        { 
                            m.Gradient[ix] = res.Gradient[ix] * res.Weight[ix];
                            ss += res.Gradient[ix] * res.Weight[ix];
                        }
                        for (int ix = 0; ix < m.Weight.Length; ix++)
                        { 
                            m.Gradient[ix] -= ss * res.Weight[ix];
                        }

                  
                    //for (int i = 0; i < m.Count; i++)
                    //{
                    //    m[i].Gradient[0]  = res[i].Gradient[0] * res[i].Weight[0];

                    //    ss += res[i].Gradient[0] * res[i].Weight[0];
                    //}
                    //for (int i = 0; i < m.Count; i++)
                    //{
                    //    m[i].Gradient[0] -= ss * res[i].Weight[0];

                    //}
                };
                this.backprop.Add(backward);
            }
            return res;
        }
        public WeightMatrix weightRows(WeightMatrix m1, WeightMatrix weightRow)
        {

            var res = new WeightMatrix(m1.Rows, m1.Columns, 0);
            for (int x = 0; x < m1.Rows; x++)
            {
                for (int y = 0; y < m1.Columns; y++)
                {
                     var ix = ((m1.Columns * x) + y);
                     res.Weight[ix] = m1.Weight[ix] * weightRow.Weight[x];
                  //  res.Add(x, y, m1.Get(x, y) * weightRow.Weight[x]);
                }
            }


            if (this.needs_backprop)
            {

                Action backward = () =>
                {
                    for (int x = 0; x < m1.Rows; x++)
                    {
                        for (int y = 0; y < m1.Columns; y++)
                        {
                            var ix = ((m1.Columns * x) + y);

                            m1.Gradient[ix] += weightRow.Weight[x] * res.Gradient[ix];
                            weightRow.Gradient[x] += m1.Weight[ix] * res.Gradient[ix];
                        }
                    }
                    //for (int i = 0, n = m1.Weight.Length; i < n; i++)
                    //{
                    //    m1.Gradient[i] += weightRow.Weight[0] * res.Gradient[i];
                    //    weightRow.Gradient[0] += m1.Weight[i] * res.Gradient[i];

                    //}
                };
                this.backprop.Add(backward);
            }
            return res;
        }
        public WeightMatrix sumColumns(WeightMatrix m1)
        {

            var res = new WeightMatrix(1, m1.Columns, 0);
            for (int x = 0; x < m1.Rows; x++)
            {
                for (int y = 0; y < m1.Columns; y++)
                {
                    //var ix = ((m1.Columns * x) + y);
                    //res.Weight[y] += m1.Weight[ix]  ;
                    //  res.Add(x, y, m1.Get(x, y) * weightRow.Weight[x]);

                    res.Add(0, y, m1.Get(x, y));
                }
            }


            if (this.needs_backprop)
            {

                Action backward = () =>
                {
                    for (int x = 0; x < m1.Rows; x++)
                    {
                        for (int y = 0; y < m1.Columns; y++)
                        {
                            //var ix = ((m1.Columns * x) + y);

                            //m1.Gradient[ix] += res.Gradient[y];


                            m1.Add_Grad(x, y, res.Get_Grad(0, y));
                        }
                    }
                    //for (int i = 0, n = m1.Weight.Length; i < n; i++)
                    //{
                    //    m1.Gradient[i] += weightRow.Weight[0] * res.Gradient[i];
                    //    weightRow.Gradient[0] += m1.Weight[i] * res.Gradient[i];

                    //}
                };
                this.backprop.Add(backward);
            }
            return res;
        }

        public List<WeightMatrix> Softmax(List<WeightMatrix> m)
        {
            var res = new List<WeightMatrix>(); // probability volume

            var maxval = -999999.0;
            for (int i = 0, n = m.Count; i < n; i++)
            {
                if (m[i].Weight[0] > maxval) maxval = m[i].Weight[0];
                res.Add(new WeightMatrix(m[i].Rows,m[i].Columns, 0));
            }

            var s = 0.0;
            for (int i = 0, n = m.Count; i < n; i++)
            {
                res[i].Weight[0] = Math.Exp(m[i].Weight[0] - maxval);
                s += res[i].Weight[0];
            }
            for (int i = 0, n = m.Count; i < n; i++) { res[i].Weight[0] /= s; }

            if (this.needs_backprop)
            {
                Action backward = () =>
                {


                    double ss = 0.0;
                    for (int i = 0; i < m.Count; i++)
                    {
                        m[i].Gradient[0]  += res[i].Gradient[0] * res[i].Weight[0];

                        ss += res[i].Gradient[0] * res[i].Weight[0];
                    }
                    for (int i = 0; i < m.Count; i++)
                    {
                        m[i].Gradient[0] -= ss * res[i].Weight[0];

                    } 
                };
                this.backprop.Add(backward);
            }
            return res;
        }
        public void backward()
        {
            for (var i = this.backprop.Count - 1; i >= 0; i--)
            {
                this.backprop[i](); // tick!
            }
        }



        public WeightMatrix repeatSX(WeightMatrix m, int p)
        {
            var res = new WeightMatrix(  p, m.Columns,   0);
            for (int i = 0; i < p; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    res.Set(i, j,  m.Get(0, j ));
                   //res.Set_Grad(i, j, 0, m.Get_Grad(0, j, 0));

                }
            }
            if (this.needs_backprop)
            {
                Action backward = () =>
                {
                    for (int i = 0; i < p; i++)
                    {
                        for (int j = 0; j < m.Columns; j++)
                        {

                            m.Add_Grad(0, j,   res.Get_Grad(i, j ));
                        }
                    }

                };
                this.backprop.Add(backward);
            }
            return res;
        }


         
    }
     
     
}
