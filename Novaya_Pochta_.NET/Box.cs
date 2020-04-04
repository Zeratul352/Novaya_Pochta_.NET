using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novaya_Pochta_.NET
{
    [Serializable]
    public class Box //a box, that we deliver
    {
        public double volume { get; set; }//physical characteristic
        public string number { get; set; }//difference charasteristic
        public int adress { get; set; }//code of place, use adress table in Deliverer to decode

        public Box() { }
        public Box(double vol, string num, int adr)
        {
            volume = vol;
            number = num;
            adress = adr;
        }
        public string GetStringAdress(List <string> adresses)//to get real adress
        {
            return adresses[adress];
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
                return new Box(first.volume + second.volume, first.number + ',' + second.number, first.adress);
            }
            else
            {
                throw new Exception("Boxes to sum up have different adresses");
            }
        }
    }
}
