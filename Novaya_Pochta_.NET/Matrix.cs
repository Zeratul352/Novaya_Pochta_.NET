using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novaya_Pochta_.NET
{
    class Matrix// this class is simple int[,] with only feature to save original numbers of rows
    {
        public int [,] matrix { get; set; }
        List<int> rowNumbers;
        List<int> columnNumbers;
        public Matrix(int [,] source)
        {
            matrix = new int[source.GetLength(0), source.GetLength(0)];
            rowNumbers = new List<int>();
            columnNumbers = new List<int>();
            for(int i = 0; i < source.GetLength(0); i++)
                for(int j = 0; j < source.GetLength(0); j++)
                {
                    matrix[i, j] = source[i, j];
                }
            for(int i = 0; i < source.GetLength(0); i++)
            {
                rowNumbers.Add(i);
                columnNumbers.Add(i);
            }
        }
        public int size()
        {
            return matrix.GetLength(0);
        }
        public void CrossOut(int i, int j)// cross out row and column with intersection in current element
        {
            columnNumbers.RemoveAt(j);
            rowNumbers.RemoveAt(i);
            int i_modif = 0;
            int j_modif = 0;
            int[,] newMatrix = new int[size() - 1, size() - 1];
            for (int i1 = 0; i1 < size(); i1++)
            {


                for (int j1 = 0; j1 < size(); j1++)
                {
                    if (i1 == i)
                    {
                        i_modif = -1;
                        continue;
                    }
                    else if (j1 == j)
                    {
                        j_modif = -1;
                        continue;
                    }
                    else
                    {
                        newMatrix[i1 + i_modif, j1 + j_modif] = matrix[i1, j1];
                    }
                }
                j_modif = 0;
            }
            
            matrix = newMatrix;
        }
        public int Get(int i, int j)
        {
            return matrix[i, j];
        }
        public int GetRowNumber(int i)
        {
            return rowNumbers[i];
        }
        public int GetColumnNumber(int j)
        {
            return columnNumbers[j];
        }
        public Matrix copy()
        {
            Matrix newMatr = new Matrix(matrix);
            newMatr.rowNumbers = new List<int>(rowNumbers);
            newMatr.columnNumbers = new List<int>(columnNumbers);
            return newMatr;
        }
    }
}
