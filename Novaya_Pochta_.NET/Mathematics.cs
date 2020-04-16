using GMap.NET.MapProviders;
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
        public static List<LandPoint> GetRoute(List<LandPoint> fullroute)
        {
            List<LandPoint> ToMatrix = new List<LandPoint>(fullroute);
            ToMatrix.RemoveAt(ToMatrix.Count - 1);
            List<LandPoint> ToAlgorythm = new List<LandPoint>(ToMatrix);
            ToAlgorythm.Remove(ToAlgorythm.First());
            int bestlength = 1000000;
            List<LandPoint> result = new List<LandPoint>();
            int[,] matrix = GetDistanceMatrix(ToMatrix);
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
    }
}
