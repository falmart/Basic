using MoogleEngine;

//Esta clase implementa un conjunto de operaciones con matrices y vectores
//No sera necesaria para el funcionamiento del proyecto Moogle!

namespace MoogleEngine.Clases
{
    class Matriz
    {
        //Producto de matrices, multiplica el primero factor(fact1) por el segundo factor(fact2)
        static public double[,] ProdMMatriz (double[,] fact1, double[,] fact2)
        {
            double[,] prod = new double[fact1.GetLength(0), fact2.GetLength(1)];
            if(fact1.GetLength(1)==fact2.GetLength(0))
            {
                for(int i=0;i<prod.GetLength(0);i++)
                {
                    for(int j=0;j<prod.GetLength(1);j++)
                    {
                        double tempvalue=0;
                        for(int k=0;k<fact1.GetLength(1);k++)
                        {
                            tempvalue=tempvalue+fact1[i,k]*fact2[k,j];
                        }
                        prod[i,j]=tempvalue;
                    }
                }
                
            }
            else
            {
                 System.Console.WriteLine("No se puede multiplicar las matrices dadas");
            }
            return prod;
            

        }

        //Producto de una matriz por un escalar
        static public double[,] ProdMatrizEsc(double[,] prod1, double prod2)
        {
            double[,] solve = (double[,])prod1.Clone();

            for(int i=0;i<solve.GetLength(0);i++)
            {
                for(int j=0;j<solve.GetLength(1);j++)
                {
                    solve[i,j]=solve[i,j]*prod2;
                }
            }
            return solve;
        }

        //Suma o Resta de Matrices
        static public double[,] SumRestMatriz(double[,] fact1, double[,] fact2)
        {
            double[,] result = (double[,])fact1.Clone();

            if(fact1.GetLength(0)==fact2.GetLength(0) && fact1.GetLength(1)==fact2.GetLength(1))
            {
                for(int i=0;i<result.GetLength(0);i++)
                {
                    for(int j=0;j<result.GetLength(1);i++)
                    {
                        //Para restar solo cambiar el + por un -
                        result[i,j] += fact2[i,j];
                    }
                }
                return result;               
            }
            else
            {
                System.Console.WriteLine("No es posible sumar o restar matrices de distinto tamaño");
                for(int i=0;i<result.GetLength(0);i++)
                {
                    for(int j=0;j<result.GetLength(1);i++)
                    {
                        result[i,j] += 0;
                    }
                }
                return result;
            }
        }
        
        //Hallar la traspuesta de una matriz
        static public double[,] Trasp(double[,] Orig)
        {
            double[,] resp = new double[Orig.GetLength(1), Orig.GetLength(0)];

            for(int i=0;i<resp.GetLength(0);i++)
            {
                for(int j=0;j<resp.GetLength(1);j++)
                {
                    resp[i,j]=Orig[j,i];
                }
            }
            return resp;
        }
    }
}