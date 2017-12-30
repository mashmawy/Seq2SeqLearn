using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seq2SeqLearn
{

    public static class Matrix
    {

        public static void MultiplyScalar(double[][] A, double c)
        {
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    A[i][j] *= c;
                }
            }
        }
        public static void SubtractMatrix(double[][] A, double[][] c)
        {
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    A[i][j] -= c[i][j];
                }
            }
        }


        public static double[][] CreateCSRMatrix(double[] data, double[] rows, double[] columns, int rowcount)
        {
            double[][] csr = MatrixCreate(rowcount, (int)columns.Max() + 1);


            for (int i = 0; i < data.Length; i++)
            {
                int row = (int)rows[i];
                if (row == 10)
                {
                    row = 0;
                }
                csr[row][(int)columns[i]] = data[i];
            }
            return csr;
        }
        public static double[][] CreateCSRMatrix(double[] data, double[] rows, double[] columns)
        {
            double[][] csr = MatrixCreate((int)rows.Max() + 1, (int)columns.Max() + 1);


            for (int i = 0; i < data.Length; i++)
            {
                int row = (int)rows[i];
                if (row == 10)
                {
                    row = 0;
                }
                csr[row][(int)columns[i]] = data[i];
            }
            return csr;
        }

        public static double[] RangeArray(double m)
        {
            double[] csr = new double[(int)m];

            for (int i = 0; i < m; i++)
            {
                csr[i] = i;
            }

            return csr;
        }
        public static double[] Ones(double m)
        {
            double[] csr = new double[(int)m]; 
            for (int i = 0; i < m; i++)
            {
                csr[i] = 1;
            } 
            return csr;
        }

        public static double[][] Korn(double[][] A, double[][] B)
        {
            double[][] rotate = Matrix.MatrixCreate(A.Length * B.Length, A[0].Length * B[0].Length);
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    var element = A[i][j];
                    var newInd = i * B.Length;
                    var newIndj = j * B[0].Length;
                    for (int ii = newInd; ii < B.Length + newInd; ii++)
                    {
                        for (int jj = newIndj; jj < B[0].Length + newIndj; jj++)
                        {
                            rotate[ii][jj] = A[i][j] * B[ii - newInd][jj - newIndj];
                        }
                    }

                }
            }
            return rotate;
        }
        public static double[] Scale(this double[] arr, double min, double max)
        {
            double m = (max - min) / (arr.Max() - arr.Min());
            double c = min - arr.Min() * m;
            var newarr = new double[arr.Length];
            for (int i = 0; i < newarr.Length; i++)
                newarr[i] = m * arr[i] + c;
            return newarr;
        }
        public static double[][] Whitening(double[][] X, double epsilon)
        {
            var avg = X.Mean(1);
            X = X.Subtract(avg.repmat(X.Length).Transpose());
            //sigma = x * x' / size(x, 2);
            var sigma = X.Multiply(X.Transpose()).Divide(X[0].Length);
            //[U,S,V] = svd(sigma);
            SingularValueDecomposition svd = new SingularValueDecomposition(sigma);
            var U = svd.getU();
            var S = svd.getS();

            var xRot = U.Transpose().Multiply(X);
            //xPCAwhite = diag(1./sqrt(diag(S) + epsilon)) * U' * x;
            var xPCAwhite = (1.0d).Divide(S.Diagonal().Add(epsilon).Sqrt()).Diagonal().Multiply(U.Transpose()).Multiply(X);

            var xZCAwhite2 = U.Multiply((1.0d).Divide(S.Diagonal().Add(epsilon).Sqrt()).Diagonal().Multiply(U.Transpose()));
            var xZCAwhite = xZCAwhite2.Multiply(X);
            return xZCAwhite;
        }
        public static double[][] PCA(double[][] X, int epsilon)
        {
            var avg = X.Mean(1);
            X = X.Subtract(avg.repmat(X.Length).Transpose());
            //sigma = x * x' / size(x, 2);
            var sigma = X.Multiply(X.Transpose()).Divide(X[0].Length);
            //[U,S,V] = svd(sigma);
            SingularValueDecomposition svd = new SingularValueDecomposition(sigma);
            var U = svd.getU();
            var S = svd.getS();
            List<int> indcies = new List<int>();
            for (int i = 0; i < 60; i++)
            {
                indcies.Add(i);
            }
            var xRot = U.GetColumns(indcies.ToArray()).Transpose().GetColumns(indcies.ToArray()).Multiply(X);
            //xPCAwhite = diag(1./sqrt(diag(S) + epsilon)) * U' * x;
            // var xPCAwhite = (1.0d).Divide(S.Diagonal().Add(epsilon).Sqrt()).Diagonal().Multiply(U.Transpose()).Multiply(X);

            // var xZCAwhite2 = U.Multiply((1.0d).Divide(S.Diagonal().Add(epsilon).Sqrt()).Diagonal().Multiply(U.Transpose()));
            // var xZCAwhite = xZCAwhite2.Multiply(X);
            return xRot.Transpose();
        }
        private static double[][] repmat(this double[] b1, int m)
        {
            double[][] r = new double[m][];
            for (int i = 0; i < m; i++)
            {
                r[i] = b1;
            }


            return r.Transpose();
        }
        public static double[][] Diagonal(this double[] A)
        {

            double[][] means = new double[A.Length][];
            for (int i = 0; i < A.Length; i++)
            {

                means[i] = new double[A.Length];

                means[i][i] = A[i];


            }

            return means;

        }
        public static double[][] Reshape(this double[] A, int x, int y)
        {

            double[][] means = MatrixCreate(x, y);

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    means[i][j] = A[i * x + j];
                }
            }

            return means;

        }
        public static double[] Diagonal(this double[][] A)
        {

            double[] means = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {

                means[i] = A[i][i];

            }

            return means;

        }

        public static double Max(this double[][] A)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i].Max();

            }
            return C.Max();

        }
        public static double[][] Sqrt(this double[][] A)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = Math.Sqrt(A[i][j]);
                }
            }
            return C;

        }
        public static double[][] Abs(this double[][] A)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = Math.Abs(A[i][j]);
                }
            }
            return C;

        }
        public static double[] Sqrt(this double[] A)
        {

            double[] C = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {

                C[i] = Math.Sqrt(A[i]);

            }
            return C;

        }
        public static double Variance(this double[] A)
        {


            double average = A.Average();
            double sumOfSquaresOfDifferences = A.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / A.Length);
            return sd * sd;

        }
        public static double[] Mean(this double[][] A)
        {

            double[] means = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    means[i] += A[i][j];
                }
            }
            for (int i = 0; i < means.Length; i++)
            {
                means[i] /= A.Length;
            }
            return means;

        }
        public static double[] Mean(this double[][] A, int dim)
        {
            if (dim == 0)
            {
                return A.Mean();
            }
            else
            {

                double[] means = new double[A[0].Length];
                for (int i = 0; i < A.Length; i++)
                {
                    for (int j = 0; j < A[0].Length; j++)
                    {
                        means[j] += A[i][j];
                    }
                }
                for (int i = 0; i < means.Length; i++)
                {
                    means[i] /= A[0].Length;
                }
                return means;

            }
        }

        public static double[] ElementwisePower(this double[] A, int pow)
        {

            double[] res = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                res[i] = Math.Pow(A[i], pow);
            }
            return res;

        }

        public static double Mean(this double[] A)
        {


            double average = A.Average();
            return average;

        }
        public static double StandardDeviation(this double[] A)
        {


            double average = A.Average();
            double sumOfSquaresOfDifferences = A.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / A.Length);
            return sd;

        }
        public static double StandardDeviation(this double[] A, double average)
        {
            double sumOfSquaresOfDifferences = A.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / A.Length);
            return sd;

        }
        public static double[] Sum(this double[][] A, int c)
        {

            double[] C;
            if (c == 0)
            {
                C = new double[A[0].Length];

                for (int i = 0; i < A.Length; i++)
                {
                    for (int j = 0; j < A[0].Length; j++)
                    {
                        C[j] += A[i][j];
                    }
                }
                return C;
            }
            else
            {

                C = new double[A.Length];
                for (int i = 0; i < A.Length; i++)
                {
                    for (int j = 0; j < A[0].Length; j++)
                    {
                        C[i] += A[i][j];
                    }
                }
                return C;
            }


        }
        public static double[] Sum(this double[][] A)
        {

            double[] C;


            C = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i] += A[i][j];
                }
            }
            return C;



        }
        public static double[][] Log(this double[][] A)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = Math.Log(A[i][j]);
                }
            }
            return C;

        }
        public static double[][] Exp(this double[][] A)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = Math.Exp(A[i][j]);
                }
            }
            return C;

        }
        public static double[][] Divide(this double c, double[][] A)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = c / A[i][j];
                }
            }
            return C;

        }
        public static double[][] Subtract(this double c, double[][] A)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = c - A[i][j];
                }
            }
            return C;

        }
        public static double[] Divide(this double c, double[] A)
        {
            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {
                C[i] = c / A[i];

            }
            return C;

        }
        public static double[][] ElementwiseDivide(this double[][] A, double[] b, int dimension = 0, bool inPlace = false)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    if (dimension == 0)
                    {
                        C[i][j] = A[i][j] / b[i];

                    }
                    else
                    {

                        C[i][j] = A[i][j] / b[j];
                    }

                }
            }

            return C;
        }
        public static double[][] ElementwiseMultiply(this double[][] A, double[][] c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] * c[i][j];
                }
            }
            return C;

        }
        public static double[][] ElementwiseDivide(this double[][] A, double[][] c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] / c[i][j];
                }
            }
            return C;

        }
        public static double[][] Add(this double[][] A, double c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] + c;
                }
            }
            return C;

        }

        public static double[][] Divide(this double[][] A, double c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] / c;
                }
            }
            return C;

        }
        public static double[][] Subtract(this double[][] A, double[][] c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] - c[i][j];
                }
            }
            return C;

        }
        public static double[][] Subtract(this double[][] A, double c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] - c;
                }
            }
            return C;

        }
        public static double[][] Subtract(this double[][] A, double[] c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);
            if (c.Length == A[0].Length)
            {
                for (int i = 0; i < A.Length; i++)
                {
                    for (int j = 0; j < A[0].Length; j++)
                    {
                        C[i][j] = A[i][j] - c[j];
                    }
                }

            }
            else
            {

                for (int i = 0; i < A.Length; i++)
                {
                    for (int j = 0; j < A[0].Length; j++)
                    {
                        C[i][j] = A[i][j] - c[i];
                    }
                }
            }
            return C;

        }
        public static double[][] Add(this double[][] A, double[] c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] + c[j];
                }
            }
            return C;

        }
        public static double[] Subtract(this double[] A, double c)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i] - c;

            }
            return C;

        }
        public static double[][] Transpose(this double[][] A)
        {

            double[][] C = MatrixCreate(A[0].Length, A.Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[j][i] = A[i][j];
                }
            }
            return C;

        }
        public static double[] Subtract(this double c, double[] A)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i] - c;

            }
            return C;

        }
        public static double[] Subtract(this double[] A, double[] c)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i] - c[i];

            }
            return C;

        }
        public static double[] Add(this double[] A, double[] c)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i] + c[i];

            }
            return C;

        }
        public static double[] Add(this double[] A, double c)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i] + c;

            }
            return C;

        }
        public static double[] Divide(this double[] A, double c)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i] / c;

            }
            return C;

        }

        public static double[] ElementwiseDivide(this double[] A, double[] c)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i] / c[i];

            }
            return C;

        }

        public static double[][] Multiply(this double[][] A, double[][] B)
        {

            double[][] C = MatrixCreate(A.Length, B[0].Length);

            var source = Enumerable.Range(0, A.Length);
            var pquery = from num in source.AsParallel()
                         select num;
            pquery.ForAll((e) => MultiplyKernel(A, B, C, e));
            return C;

        }
        public static double[][] Multiply(this double[][] A, double c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] * c;
                }
            }
            return C;

        }
        public static double[][] Multiply(this double c, double[][] A)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] * c;
                }
            }
            return C;
        }
        public static double[] Multiply(this double c, double[] A)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i] * c;

            }
            return C;

        }
        public static double[][] Pow(this double[][] A, int d)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = Math.Pow(A[i][j], d);
                }
            }
            return C;

        }
        public static double[] Pow(this double[] A, int d)
        {

            double[] C = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {

                C[i] = Math.Pow(A[i], d);

            }
            return C;

        }
        public static double[] Multiply(this double[] A, double c)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i] * c;

            }
            return C;

        }
        static void MultiplyKernel(double[][] A, double[][] B, double[][] C, int i)
        {
            double[] iRowA = A[i];
            double[] iRowC = C[i];



            for (int k = 0; k < A[0].Length; k++)
            {

                double[] kRowB = B[k];
                double ikA = iRowA[k];
                for (int j = 0; j < B[0].Length; j++)
                {
                    iRowC[j] += ikA * kRowB[j];
                }
            }
        }
 

        public static double[][] Add(this double[][] A, double[][] c)
        {

            double[][] C = MatrixCreate(A.Length, A[0].Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    C[i][j] = A[i][j] + c[i][j];
                }
            }
            return C;

        }

        private static Random rand = new Random(); //reuse this if you are generating many

        public static double[][] RandomN(int rows, int cols, double min, double max)
        {
            // do error checking here
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
            {
                result[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    double mean = 0;
                    double stdDev = Math.Pow(10, -4);
                    double u1 = rand.NextDouble(); //these are uniform(0,1) random doubles
                    double u2 = rand.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                 Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                    double randNormal =
                                 mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)


                    result[i][j] = randNormal;
                }
            }
            return result;
        }

        public static double[][] MatrixCreate(int rows, int cols)
        {
            // do error checking here
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }


        static Random rng = new Random(3);
        public static double[][] Random(int rows, int cols, double min, double max)
        {
            // do error checking here
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
            {
                result[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = min + (rng.NextDouble() * (max - min));
                }
            }
            return result;
        }

        public static double Norm2(this double[][] A)
        {
            double norm = 0.0;
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    norm += A[i][j] * A[i][j];
                }
            }
            return (double)Math.Sqrt(norm);

        }

        public static double Norm2(this double[] A)
        {
            double norm = 0.0;
            for (int i = 0; i < A.Length; i++)
            {

                norm += A[i] * A[i];

            }
            return (double)Math.Sqrt(norm);

        }


        public static double[][] GetColumns(this double[][] A, int[] c)
        {

            double[][] C = MatrixCreate(A.Length, c.Length);

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < c.Length; j++)
                {
                    C[i][j] = A[i][c[j]];
                }
            }
            return C;

        }



        public static double[] GetColumn(this double[][] A, int c)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < A.Length; i++)
            {

                C[i] = A[i][c];

            }
            return C;

        }

        public static double[] GetRow(this double[][] A, int c)
        {

            double[] C = new double[A[0].Length];

            for (int i = 0; i < C.Length; i++)
            {

                C[i] = A[c][i];

            }
            return C;

        }
        public static double[] Abs(this double[] A)
        {

            double[] C = new double[A.Length];

            for (int i = 0; i < C.Length; i++)
            {

                C[i] = Math.Abs(A[i]);

            }
            return C;

        }


        public static double[] Ones(int n)
        {
            double[] res = new double[n];
            for (int i = 0; i < n; i++)
            {
                res[i] = 1;
            }
            return res;
        }
        public static double[][] Ones(int n, int m)
        {
            double[][] res = MatrixCreate(n, m);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    res[i][j] = 1;
                }
            }
            return res;
        }


        public static double[] Zeros(int n)
        {
            double[] res = new double[n];
            for (int i = 0; i < n; i++)
            {
                res[i] = 0;
            }
            return res;
        }
        public static double[][] Zeros(int n, int m)
        {
            double[][] res = MatrixCreate(n, m);
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < m; j++)
            //    {
            //        res[i][j] = 0;
            //    }
            //}
            return res;
        }



        public static double Get(this double[][] A, int x, int y)
        {
            return A[x][y];
        }

        public static void Set(this double[][] A, int x, int y, double val)
        {
            A[x][y] = val;
        }



        public static double[][] Rot90(this double[][] start)
        {
            double[][] rotate = Matrix.MatrixCreate(start[0].Length, start.Length);
            for (int i = 0; i < start.Length; i++)
            {
                int ith = start[0].Length - 1;
                for (int j = 0; j < start[0].Length; j++)
                {
                    rotate[ith][i] = start[i][j];
                    ith--;
                }
            }
            return rotate;
        }
        public static double[][] Rot90(this double[][] start, int num)
        {
            double[][] res = start;
            for (int i = 0; i < num; i++)
            {
                res = Rot90(res);
            }
            return res;
        }

        public static double[][] Copy(this double[][] copy )
        {
            double[][] res = MatrixCreate(copy.Length, copy[0].Length);
            for (int i = 0; i < res.Length; i++)
            {
                for (int j = 0; j < res[0].Length; j++)
                {
                    res[i][j] = copy[i][j];
                }
            }
            return res;
        }

        public static double[][] ConcatColumnVector(this double[][] first,double[][] second)
        {
            double[][] res = MatrixCreate(1,first[0].Length+ second[0].Length);
            for (int i = 0; i < first[0].Length; i++)
            {
                res[0][i] = first[0][i];
            }
            for (int i = first[0].Length; i < second[0].Length; i++)
            {
                res[0][i] = second[0][i - first[0].Length];
            } 
            return res;
        }

        public static Tuple<double[][],double[][]> SplitColumnVector(this double[][] res,int firstlen)
        {
            double[][] first = MatrixCreate(1, firstlen);
            double[][] second = MatrixCreate(1, res[0].Length - firstlen);
            for (int i = 0; i < first[0].Length; i++)
            {
                first[0][i] = res[0][i];
            }
            for (int i = first[0].Length; i < second[0].Length; i++)
            {
                second[0][i - first[0].Length] = res[0][i];
            }
            return new Tuple<double[][],double[][]>(first,second);
        }


    }
}
