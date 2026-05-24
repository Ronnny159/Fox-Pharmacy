using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public static class Colores
    {

        public static Color ColorPrimario { get; set; }
        public static Color ColorSecundario { get; set; }

        public static List<string> ListaColores = new List<string>() { "#D6ECF9"
                                                                        ,"#D2EAF9"
                                                                        ,"#CEE8F8"
                                                                        ,"#CAE6F8"
                                                                        ,"#C6E4F8"
                                                                        ,"#C2E2F8"
                                                                        ,"#BEE0F7"
                                                                        ,"#BADEF7"
                                                                        ,"#B6DCF6"
                                                                        ,"#B2DAF6"
                                                                        ,"#AED8F5"
                                                                        ,"#AAD6F5"
                                                                        ,"#A6D4F4"
                                                                        ,"#A2D2F4"
                                                                        ,"#9ED0F3"
                                                                        ,"#9ACEF2"
                                                                        ,"#96CCF1"
                                                                        ,"#92CAF0"
                                                                        ,"#8EC8EF"
                                                                        ,"#8AC6EE"
                                                                        ,"#86C4ED"
                                                                        ,"#82C2EC"
                                                                        ,"#7EC0EB"
                                                                        ,"#7ABEEA"
                                                                        ,"#76BCE9"
                                                                        ,"#72BAE8"
                                                                        ,"#6EB8E7"
                                                                        ,"#6AB6E6"
                                                                        ,"#66B4E5"
                                                                        ,"#62B2E4"
                                                                        ,"#5EB0E3"
                                                                        ,"#5AAEE2"
                                                                        ,"#56ACE1"
                                                                        ,"#52AAE0"
                                                                        ,"#4EA8DF"
                                                                        ,"#4AA6DE"
                                                                        ,"#46A4DD"
                                                                        ,"#42A2DC"
                                                                        ,"#3EA0DB"
                                                                        ,"#3A9EDA"
                                                                        ,"#369CD9"
                                                                        ,"#329AD8"
                                                                        ,"#2E98D7"
                                                                        ,"#2A96D6"
                                                                        ,"#2694D5"
                                                                        ,"#2292D4"
                                                                        ,"#1E90D3"
                                                                        ,"#1A8ED2"
                                                                        ,"#168CD1"
                                                                        ,"#128AD0"
                                                                        ,"#0E88CF"
                                                                        ,"#0A86CE"};

        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;
            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }
            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
    }
}
