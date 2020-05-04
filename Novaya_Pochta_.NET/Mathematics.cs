﻿using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Novaya_Pochta_.NET
{
    class Mathematics
    {
        private static int inf1 = 100000;
        private static int inf2 = 200000;
        private static int DeathClock = 0;
        private static int record = inf1;
        private static List<Tuple<int, int>> path = new List<Tuple<int, int>>();
        private static List<int> solution = new List<int>();
        private static List<Tuple<int, int>> lastStep = new List<Tuple<int, int>>();
        private static Matrix _sourceMatrix;
        public static List<LandPoint> GetRoute(List<LandPoint> fullroute)
        {
            List<LandPoint> ToMatrix = new List<LandPoint>(fullroute);
            ToMatrix.RemoveAt(ToMatrix.Count - 1);
            
            List<LandPoint> result = new List<LandPoint>();
            int[,] matrix = GetDistanceMatrix(ToMatrix);
            //matrix = MatrixReduction(matrix);
            InitLittle(new Matrix(matrix), 1000000);
            SolveLittle();
            foreach(var i in solution)
            {
                result.Add(fullroute[i]);
            }
            return result;
        }
        public static List<LandPoint> GetGenetic(List<LandPoint> fullroute)
        {
            List<LandPoint> ToMatrix = new List<LandPoint>(fullroute);
            ToMatrix.RemoveAt(ToMatrix.Count - 1);
            List<LandPoint> ToAlgorythm = new List<LandPoint>(ToMatrix);
            ToAlgorythm.Remove(ToAlgorythm.First());
            int bestlength = 1000000;
            List<LandPoint> result = new List<LandPoint>();
            int[,] matrix = GetDistanceMatrix(ToMatrix);
            //matrix = MatrixReduction(matrix);
            //result = HungaryAlgorythm(matrix, ToMatrix);
            for (int i = 0; i < 10; i++)
            {
                Tuple<List<LandPoint>, int> candidate = GetRoute(matrix, ToAlgorythm, fullroute.First(), fullroute.First());
                if(i == 0)
                {
                    bestlength = candidate.Item2;
                    result = candidate.Item1;
                }
                else
                {
                    if(candidate.Item2 < bestlength)
                    {
                        bestlength = candidate.Item2;
                        result = candidate.Item1;
                    }
                }
            }
            return result;
        }
        public static int[,] GetDistanceMatrix(List<LandPoint> points)
        {
            int[,] matrix = new int[points.Count, points.Count];
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string origin = "";
            //string origin = "Slobozhanska St, 35, Lyubotin, Ukraine+ON|Slobozhanska St, 51, Lyubotin, Ukraine+ON|Шевченка вул. 15, Lyubotyn, Харківська, 62433+ON|Provulok Sanitarnyy, 6, Liubotyn, Kharkivs'ka oblast, 62434+ON|Klavdii Shulzhenko St, 45, Liubotyn, Kharkivs'ka oblast, 62435+ON|Fisaka St, 4, Lybotin, Ukraine+ON|Academician Amosov St, 45, Lyubotin, Ukraine+ON|Gaidar St, 47, Lyubotin, Ukraine+ON|Chernyshevsky St, 13, Lyubotin, Ukraine+ON|Guards St, 24, Lyubotin, Ukraine+ON";
            foreach(LandPoint point in points)
            {
                //string lat = point.coordinates.Lat.ToString();
                //string lng = point.coordinates.Lng.ToString();
                origin += point.adress + "+ON";
                if (point != points.Last())
                {
                    origin += '|';
                }
            }
            string url = @"https://maps.googleapis.com/maps/api/distancematrix/xml?units=metric&origins=" + origin + " &destinations=" + origin + "&key=" + GoogleMapProvider.Instance.ApiKey;//нужно попробовать координаты точных адресов, а не взятых наобум
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            if(response == null)
            {
                throw new Exception("No URL responce returned in distance matrix request");
            }
            Stream dataStream = response.GetResponseStream();            
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(dataStream);
            xmldoc.Save("MatrixResponse");
            XmlElement xRoot = xmldoc.DocumentElement;
            int i = 0;
            int j = 0;
            foreach (XmlNode xNode in xRoot)
            {
                if (xNode.Name == "row")
                {
                    foreach (XmlNode childnode in xNode.ChildNodes)
                    {
                        foreach (XmlNode node in childnode.ChildNodes)
                        {
                            if (node.Name == "distance")
                            {
                                matrix[i, j] = System.Convert.ToInt32(node.FirstChild.InnerText);
                                j++;
                                //Console.Write(node.FirstChild.InnerText + "   ");
                            }
                        }
                    }
                    i++;
                    j = 0;
                    //Console.WriteLine();
                }
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            return matrix;
        }

        public static int [,] MatrixReduction(int [,] Matrix)
        {
            for(int i = 0; i < Matrix.GetLength(1); i++)
            {
                Matrix[i, i] = inf1;
            }
            for(int i = 0; i < Matrix.GetLength(1); i++)
            {
                int min = inf1;
                for(int j = 0; j < Matrix.GetLength(0); j++)
                {
                    if(Matrix[i,j] < min)
                    {
                        min = Matrix[i, j];
                    }
                }
                for(int j = 0; j < Matrix.GetLength(0); j++)
                {
                    Matrix[i, j] -= min;
                }
            }
            for (int i = 0; i < Matrix.GetLength(1); i++)
            {
                int min = 90000;
                for (int j = 0; j < Matrix.GetLength(0); j++)
                {
                    if (Matrix[j, i] < min)
                    {
                        min = Matrix[j, i];
                    }
                }
                for (int j = 0; j < Matrix.GetLength(0); j++)
                {
                    Matrix[j, i] -= min;
                }
            }

            return Matrix;
        }
        public static bool TryToGetSolution(int [,] matrix)
        {
            int combinations = 1;
            int count = matrix.GetLength(0);
            for(int i = 0; i < count; i++)
            {
                int zeros = 0;
                for(int j = 0; j < count; j++)
                {
                    if(matrix[i, j] == 0)
                    {
                        zeros += 1;
                    }
                }
                combinations *= zeros;
            }
            if(combinations == 0)
            {
                return false;
            }

            return false;
        }
        public static List<LandPoint> HungaryAlgorythm(int [,] Matrix, List<LandPoint> destinations)
        {
            /*int[,] matrix = new int[Matrix.GetLength(0), Matrix.GetLength(1)];
            for(int i = 0; i < Matrix.GetLength(0); i++)
            {
                for(int j = 0; j < Matrix.GetLength(1); j++)
                {
                    matrix[i, j] = Matrix[i, j];
                }
            }
            int count = Matrix.GetLength(0);
            matrix = MatrixReduction(matrix);
            while (!TryToGetSolution(matrix))
            {
                for(int line = 0; line < count; line++)
                {
                    for(int row = 0; row < count; row++)
                    {
                        int zero_rows = 0;
                        int zero_lines = 0;
                        if (matrix[line, row] == 0)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                if (matrix[line, i] == 0)
                                {
                                    zero_lines += 1;
                                }
                                if(matrix[i, row] == 0)
                                {
                                    zero_rows += 1;
                                }
                            }
                            if(zero_lines >= zero_rows)
                            {
                                for(int j = 0; j < count; j++)
                                {
                                    if (matrix[line, j] < 0)
                                    {
                                        matrix[line, j] *= -1;
                                    }
                                }
                            }
                            else
                            {
                                for(int j = 0; j < count; j++)
                                {
                                    if(matrix[j, row] < 0)
                                    {
                                        matrix[j, row] *= -1;
                                    }                                
                                }
                            }
                        }
                    }
                }//finding and crossing out lines and rows
                int min = 100000;
                for(int i = 0; i < count; i++)
                {
                    for(int j = 0; j < count; j++)
                    {
                        if((matrix[i,j] < min) && (matrix[i,j] > 0))
                        {
                            min = matrix[i, j];
                        }
                    }
                }
                for(int i = 0; i < count; i++)
                {
                    for(int j = 0; j < count; j++)
                    {
                        if(matrix[i,j] > 0)
                        {
                            matrix[i, j] -= min;
                        }
                    }
                }
                for(int line = 0; line < count; line++)
                {
                    for(int row = 0; row < count; row++)
                    {
                        if(matrix[line, row] < 0)
                        {
                            bool flag = true;
                            for(int i = 0; i < count; i++)
                            {
                                if((matrix[i, row] > 0) && (matrix[line, i] > 0))
                                {
                                    flag = false;
                                }
                            }
                            if (flag)
                            {
                                matrix[line, row] -= min;
                            }
                        }
                    }
                }
                for(int i = 0; i < count; i++)
                {
                    for(int j = 0; j < count; j++)
                    {
                        if(matrix[i, j] < 0)
                        {
                            matrix[i, j] *= -1;
                        }
                    }
                }
                matrix = MatrixReduction(matrix);
            }
            List<LandPoint> result = new List<LandPoint>();
            for (int i = 0; i < count; i++)
            {
                result.Clear();
                result.Add(destinations[0]);
                if (matrix[0, i] == 0)
                {
                    
                    int temp = i;
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        if (matrix[temp, j] == 0)
                        {
                            result.Add(destinations[temp]);
                            temp = j;
                            j = 0;
                        }
                    }
                    if (temp == 0)
                    {
                        break;
                    }
                }
            }*/
            int count = Matrix.GetLength(1);
            for(int i = 0; i < count; i++)
            {
                for(int j = 0; j < count; j++)
                {
                    Matrix[i, j] *= -1;
                }
            }
            for(int i = 0; i < count; i++)
            {
                Matrix[i, i] = 100000;
            }
            List<int> u = new List<int>();
            List<int> v = new List<int>();
            List<int> p = new List<int>();
            List<int> way = new List<int>();
            for(int i = 0; i <= count; i++)
            {
                u.Add(0);
                v.Add(0);
                p.Add(0);
                way.Add(0);
            }
            for(int i = 1; i <= count; i++)
            {
                p[0] = i;
                int j0 = 0;
                List<int> minv = new List<int>();
                List<bool> used = new List<bool>();
                for(int counter = 0; counter <= count; counter++)
                {
                    minv.Add(100000);
                    used.Add(false);
                }
                do
                {
                    used[j0] = true;
                    int i0 = p[j0];
                    int delta = 100000;
                    int j1 = 0;
                    for (int j = 1; j <= count; j++)
                    {
                        if (!used[j])
                        {
                            int cur = Matrix[i0 - 1, j - 1] - u[i0] - v[j];
                            if (cur < minv[j])
                            {
                                minv[j] = cur;
                                way[j] = j0;
                            }
                            if (minv[j] < delta)
                            {
                                delta = minv[j];
                                j1 = j;
                            }
                        }
                    }
                    for (int j = 0; j <= count; j++)
                    {
                        if (used[j])
                        {
                            u[p[j]] += delta;
                            v[j] -= delta;
                        }
                        else
                        {
                            minv[j] -= delta;
                        }
                    }
                    j0 = j1;
                } while (p[j0] != 0);
                do
                {
                    int j1 = way[j0];
                    p[j0] = p[j1];
                    j0 = j1;
                } while (j0 != 0);
            }
            List<int> ans = new List<int>();
            for(int i = 0; i <= count; i++)
            {
                ans.Add(0);
            }
            for(int j = 1; j <= count; j++)
            {
                ans[p[j]] = j;
            }
           
            List<LandPoint> result = new List<LandPoint>();
            /*for(int i = 1; i <= count; i++)
            {
                if(ans[i] == 1)
                {
                    for(int j = 0; j < count; j++)
                    {
                        result.Add(destinations[ans[i] - 1]);
                        i = i % count + 1;
                    }
                    break;
                }
            }
            result.Add(destinations[0]);*/
            for(int i = 1; i <= count; i++)
            {
                result.Add(destinations[ans[i] - 1]);
            }
            result.Add(destinations[ans[1]- 1]);
            return result;
        }
        public static Tuple<List<LandPoint>, int> GetRoute(int [,] Matrix, List<LandPoint> original, LandPoint start, LandPoint last)
        {
            List<LandPoint> points = new List<LandPoint>(original);
            if(start != last)
            {
                throw new Exception("This is not a loop route");
            }
            if(Matrix.GetLength(0) -1 != points.Count)
            {
                throw new Exception("Unpropriate matrix for a loop route");
            }
            if(points.Count == 1)// special case for an obvious path
            {
                points.Insert(0, start);
                points.Add(last);
                int length = Matrix[0, 1] * 2;
                return new Tuple<List<LandPoint>, int>(points, length);
            }
            int deadline = 0;
            int cases = 50;
            int generations = 50;
            int[,] RandVays = new int[cases, points.Count + 3];
            int[,] Generation = new int[cases, points.Count + 3];
            Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            List<int> conformity = new List<int>();
            for(int i = 1; i <= points.Count; i++)
            {
                conformity.Add(i);
            }
            points.Insert(0, start);
            points.Add(last);
            for (int i = 0; i < cases; i++)
            {
                for(int j = 0; j < conformity.Count; j++)// random shuffle
                {
                    int temp = conformity[0];
                    conformity.RemoveAt(0);
                    conformity.Insert(random.Next(conformity.Count), temp);
                }
                RandVays[i, 0] = 0;
                for(int j = 1; j <= conformity.Count; j++)
                {
                    RandVays[i, j] = conformity[j - 1];
                }
                RandVays[i, conformity.Count + 1] = 0;
                int sum = 0;
                for(int j = 0; j <= conformity.Count; j++)
                {
                    sum += Matrix[RandVays[i, j], RandVays[i, j + 1]];
                }
                RandVays[i, conformity.Count + 2] = sum;
            }
            int end = points.Count;
            List<int> BestWay = new List<int>();
            for(int i = 0; i <= end; i++)
            {
                BestWay.Add(RandVays[0, i]);
            }
            int bestgeneration = 0;
            for(int generation = 0; generation < generations; generation++)
            {
                for(int i = 0; i < cases; i++) {// tournament selection
                    int first = random.Next(cases);
                    int second = random.Next(cases);
                    if(RandVays[first, end] < RandVays[second, end])
                    {
                        for(int k = 0; k <= end; k++)
                        {
                            Generation[i, k] = RandVays[first, k];
                        }
                    }
                    else
                    {
                        for(int k = 0; k <= end; k++)
                        {
                            Generation[i, k] = RandVays[second, k];
                        }
                    }
                }

                for (int counter = 0; counter < cases; counter += 2)//crossover
                {
                    int first = random.Next(cases);// first parent
                    int second = random.Next(cases);// second parent
                    int devider = random.Next(end - 1) + 1;// random crossover coefficient
                    int[] firstchild = new int[end + 1];
                    int[] secondchild = new int[end + 1];
                    //List<int> secondchild = new List<int>(end + 1);
                    int sum1 = 0;
                    int sum2 = 0;
                    for (int k = 0; k < devider; k++)
                    {
                        firstchild[k] = Generation[second, k];
                        secondchild[k] = Generation[first, k];
                    }
                    for(int k = devider; k < end; k++)
                    {
                        firstchild[k] = Generation[first, k];
                        secondchild[k] = Generation[second, k];
                    }
                    for (int i = devider; i < end; i++)
                    {
                        for (int j = 0; j < devider; j++)
                        {
                            if (firstchild[i] == firstchild[j])// every number has to be unique to correct work of the algorythm
                            {
                                for (int k = devider; k < end; k++)
                                {
                                    for (int m = 0; m < devider; m++)
                                    {
                                        if (secondchild[k] == secondchild[m])
                                        {
                                            int temp = secondchild[m];
                                            secondchild[m] = firstchild[j];
                                            firstchild[j] = temp;
                                            k = end;
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < end - 1; i++)
                    {
                        sum1 += Matrix[firstchild[i], firstchild[i + 1]];// calculating length of the path
                        sum2 += Matrix[secondchild[i], secondchild[i + 1]];
                    }
                    firstchild[end] = sum1;
                    secondchild[end] = sum2;
                    for (int k = 0; k <= end; k++)
                    {
                        RandVays[counter, k] = firstchild[k];
                        RandVays[counter + 1, k] = secondchild[k];
                    }
                }

                    for (int i = 0; i < cases / 10; i++)//increased number of mutations
                    {
                        int randnum1 = random.Next(cases);// mutation
                        int randnum2 = random.Next(end - 2) + 1;
                        int randnum3 = random.Next(end - 2) + 1;
                        int bubble = RandVays[randnum1, randnum2];
                        RandVays[randnum1, randnum2] = RandVays[randnum1, randnum3];
                        RandVays[randnum1, randnum3] = bubble;
                        RandVays[randnum1, end] = 0;
                        for(int k = 0; k < end - 1; k++)
                        {
                            RandVays[randnum1, end] += Matrix[RandVays[randnum1, k], RandVays[randnum1, k + 1]];
                        }
                    }
                    int index = -1;// the next process records the best way and kill algoryth if it is needed
                    int length = BestWay[end];
                    for(int i = 0; i < cases; i++)
                    {
                        if(RandVays[i, end] < length)
                        {
                            length = RandVays[i, end];
                            index = i;
                        }
                    }
                    if(index != -1)
                    {
                        for(int i = 0; i <= end; i++)
                        {
                            BestWay[i] = RandVays[index, i];
                            bestgeneration = generation;
                        }
                        deadline = 0;
                    }
                    else
                    {
                        deadline++;
                    }
                    if(deadline == 5)
                    {
                        generation = generations;
                        break;
                    }

                

            }
            List<LandPoint> result = new List<LandPoint>();
            for(int i = 0; i < points.Count; i++)
            {
                result.Add(points[BestWay[i]]);
            }
            return new Tuple<List<LandPoint>, int>(result, BestWay[end]);
        }
        public static void InitLittle(Matrix m, int record)
        {
            inf1 = 0;
            path.Clear();
            lastStep.Clear();
            solution.Clear();
            int[,] copy = m.matrix;
            _sourceMatrix = new Matrix(copy);
            for (int i = 0; i < m.matrix.GetLength(0); i++)
                for (int j = 0; j < m.matrix.GetLength(0); j++)
                    _sourceMatrix.matrix[i, j] = m.matrix[i, j];
            for(int i = 0; i < m.matrix.GetLength(0); i++)
            {
                for(int j = i + 1; j < m.matrix.GetLength(1); j++)
                {
                    inf1 += m.matrix[i, j] + m.matrix[j, i];
                }
            }
            for(int i = 0; i < m.matrix.GetLength(0); i++)
            {
                _sourceMatrix.matrix[i, i] = inf1;
            }
            Mathematics.record = record;
        }

        public static void SolveLittle()
        {
            DeathClock = 0;
            try
            {
                handleMatrix(_sourceMatrix, new List<Tuple<int, int>>(), 0);//solution
            }
            finally
            {
                //recording solution
                //adding zero as begin
                solution.Add(0);
                while (path.Count > 0)
                {
                    
                    foreach (var iter in path)
                    {
                        //if is present edge , coming out of current point, adding next one and removing edge
                        if (iter.Item1 == solution.Last())
                        {
                            solution.Add(iter.Item2);
                            path.Remove(iter);
                            break;
                        }
                    }
                }
            }
            
        }
        public static int Reduction(Matrix m)
        {
            int ret = 0;// sum of all extracted values
            int count = m.matrix.GetLength(0);
            List<int> minRow = new List<int>();// arrays of minimal elements
            List<int> minColomn = new List<int>();
            for(int i = 0; i < count; i++)
            {
                minRow.Add(inf1);
                minColomn.Add(inf2);
            }
            
            for(int i = 0; i < count; i++)// getting around all matrix
            {
                for(int j = 0; j < count; j++)//seek for min element in a line
                {
                    if(m.matrix[i,j] < minRow[i])
                    {
                        minRow[i] = m.matrix[i, j];
                    }
                }
                for(int j = 0; j < count; j++)
                {
                    if((m.matrix[i, j] < inf1) && (m.matrix[i, j] < inf2))// substract min element from all not - infinity lines
                    {
                        m.matrix[i, j] -= minRow[i];
                    }
                    if(m.matrix[i, j] < minColomn[j])// look for min elem in rows after substraction
                    {
                        minColomn[j] = m.matrix[i, j];

                    }
                }
            }
            //substracting min element from all not - infinity rows
            for(int j = 0; j < count; j++)
            {
                for(int i = 0; i < count; i++)
                {
                    if((m.matrix[i, j] < inf1) && (m.matrix[i, j] < inf2))
                    {
                        m.matrix[i, j] -= minColomn[j];
                    }
                }
            }
            // sum all substracted values
            foreach(int i in minColomn)
            {
                ret += i;
            }
            foreach(int i in minRow)
            {
                ret += i;

            }

            return ret;
        }

        public static int GetKoefficients(Matrix m, int r, int c)
        {
            int rmin, cmin;
            rmin = cmin = inf1;
            for(int i = 0; i < m.matrix.GetLength(0); i++)
            {
                if(i != r)
                {
                    if(m.matrix[i, c] < rmin)
                    {
                        rmin = m.matrix[i, c];
                    }
                }
                if(i != c)
                {
                    if(m.matrix[r, i] < cmin)
                    {
                        cmin = m.matrix[r, i];
                    }
                }
            }
            return rmin + cmin;
        }
        public static void logPath(List<Tuple<int,int>> path)
        {
            lastStep = path;
        }
        public static void AddInfinity(Matrix m)
        {
            List<bool> infRow = new List<bool>();// lists of rows and columns that contain infinity
            List<bool> infColumn = new List<bool>();
            for(int i = 0; i < m.matrix.GetLength(0); i++)
            {
                infRow.Add(false);
                infColumn.Add(false);
            }
            for(int i = 0; i < m.matrix.GetLength(0); i++)//look for infinitives
            {
                for(int j = 0; j < m.matrix.GetLength(0); j++)
                {
                    if(m.matrix[i,j] == inf1)
                    {
                        infRow[i] = true;
                        infColumn[j] = true;
                    }
                }
            }
            //look for line without infinity
            int notInf = 0;
            for(int i = 0; i < m.matrix.GetLength(0); i++)
            {
                if(infRow[i] == false)
                {
                    notInf = i;
                    break;
                }
            }
            //look for row without infinity and add it
            for(int i = 0; i < m.matrix.GetLength(0); i++)
            {
                if(infColumn[i] == false)
                {
                    m.matrix[notInf, i] = inf1;
                    break;
                }
            }
        }
        public static List<Tuple<int, int>> findBestZeros(Matrix m)
        {
            // list of zero elements coordinates
            List<Tuple<int, int>> zeros = new List<Tuple<int, int>>();
            // list of coefficients
            List<int> coefflist = new List<int>();
            // max coefficient
            int maxCoeff = 0;
            for (int i = 0; i < m.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < m.matrix.GetLength(1); j++)
                {
                    if (m.matrix[i, j] == 0)// looking for zero elements
                    {
                        zeros.Add(new Tuple<int, int>(i, j));// count their coefficients and add
                        coefflist.Add(GetKoefficients(m, i, j));
                        if (coefflist.Last() > maxCoeff)
                        {
                            maxCoeff = coefflist.Last();
                        }
                    }
                }
            }
            for(int i = 0; i < zeros.Count; i++)
            {
                if(coefflist[i] != maxCoeff)
                {
                    zeros.RemoveAt(i);
                    coefflist.RemoveAt(i);
                    i--;
                }
            }
            return zeros;
        }

        public static int CalcCost(List<Tuple<int, int>> source)// calculate value of the path
        {
            int result = 0;
            foreach(Tuple<int, int> tuple in source)
            {
                result += _sourceMatrix.matrix[tuple.Item1, tuple.Item2];
            }
            return result;
        }
        public static void candidateSolution(List<Tuple<int, int>> aWay)
        {
            int curCost = CalcCost(aWay);
            if(record < curCost)
            {
                return;
            }
            record = curCost;
            path = aWay;
        }

        public static void handleMatrix(Matrix m, List<Tuple<int, int>> way, int bottomlimit)
        {
            if(DeathClock >= 500)
            {
                return;
            }
            DeathClock++;
            if(m.matrix.GetLength(0) < 2)
            {
                throw new Exception("Logic error: matrix smaller than 2x2");
            }
            if(m.matrix.GetLength(0) == 2)
            {
                //log the current way like the last watched
                logPath(way);
                //choose not infinity element in first line
                int i = m.matrix[0, 0] >= inf1 ? 1 : 0;
                // creating list with result way
                List<Tuple<int, int>> result = new List<Tuple<int, int>>(way);
                result.Add(new Tuple<int, int>(m.GetRowNumber(0), m.GetColumnNumber(i)));
                result.Add(new Tuple<int, int>(m.GetRowNumber(1), m.GetColumnNumber(1 - i)));
                candidateSolution(result);
                return;
            }
            Matrix matrix = m.copy();
            bottomlimit += Reduction(matrix);
            if (bottomlimit > record)
            {
                logPath(way);// log this way as the last searched
                return;
            }// getting list of zeros with max coefficients
            var zeros = findBestZeros(matrix);
            var edge = zeros.First();
            var newMatrix = matrix.copy();//going for 2 matrixes: with an edge and without
            newMatrix.CrossOut(edge.Item1, edge.Item2); // removing the edge
            var newPath = new List<Tuple<int, int>>(way);
            newPath.Add(new Tuple<int, int>(matrix.GetRowNumber(edge.Item1), matrix.GetColumnNumber(edge.Item2)));
            AddInfinity(newMatrix);// to avoid loops
            handleMatrix(newMatrix, newPath, bottomlimit);// look for a set with edge
            // set without edge
            newMatrix = matrix;
            newMatrix.matrix[edge.Item1, edge.Item1] = inf1 + 1;// add infinity instead of edge
            handleMatrix(newMatrix, way, bottomlimit);


            
        }
    }
}
