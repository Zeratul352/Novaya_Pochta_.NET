﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novaya_Pochta_.NET
{
    [Serializable]
    public class Box //a box, that we deliver
    {
        private static Random local_random = new Random(0);
        public int count { get; set; }//how many boxes are there actually
        public double value { get; set; }// how much it costs
        public double volume { get; set; }//physical characteristic
        public double mass { get; set; }//weight of the box
        public string number { get; set; }//difference charasteristic
        public LandPoint adress { get; set; }//code of place, use adress table in Deliverer to decode

        public Box() { }
        public Box(double vol, double mas, string num, LandPoint adr)
        {
            volume = vol;
            number = num;
            mass = mas;
            adress = adr;
            value = CalcValue() * -1;
            count = 1;
        }
        public Box(double vol, double mas, string num, LandPoint adr, double val, int count1)
        {
            volume = vol;
            number = num;
            mass = mas;
            adress = adr;
            value = val;
            count = count1;
        }
        public static void SwapBoxes(Box A, Box B)//change two boxes
        {
            Box temp = A;
            A = B;
            B = temp;
        }
        public static Box operator+(Box first, Box second)//to sum boxes with same adresses
        {
            if (first.adress == second.adress)
            {
                return new Box(first.volume + second.volume, first.mass + second.mass,first.number + ',' + second.number, first.adress, first.value + second.value, first.count + second.count);
            }
            else
            {
                throw new Exception("Boxes to sum up have different adresses");
            }
        }
        private double CalcValue()
        {
            int deliver_type = local_random.Next(2);// randomly chosen: region delivery or country delivery
            if(mass <= 0.5)
            {
                return 35 + deliver_type * 5;
            }else if(mass <= 1)
            {
                return 40 + deliver_type * 5;
            }else if(mass <= 2)
            {
                return 45 + deliver_type * 5;
            }else if(mass <= 5)
            {
                return 50 + deliver_type * 5;
            }else if(mass <= 10)
            {
                return deliver_type * 5 + 60;
            }else if(mass <= 20)
            {
                return 80 + deliver_type * 5;
            }else if(mass <= 30)
            {
                return 100 + deliver_type * 5;
            }
            else
            {
                int extra = System.Convert.ToInt32(Math.Ceiling(mass - 30));
                return 100 + extra * 2 + deliver_type * (5 + extra * 3);
            }
        }
        public static int Sum(List<Box> boxes)
        {
            double sum = 0;
            foreach(Box box in boxes)
            {
                sum += box.value;
            }
            return System.Convert.ToInt32(sum);
        }
        public static double SumVolume(List<Box> boxes)
        {
            double sum = 0;
            foreach (Box box in boxes)
            {
                sum += box.volume;
            }
            return sum;
        }
    }
}
