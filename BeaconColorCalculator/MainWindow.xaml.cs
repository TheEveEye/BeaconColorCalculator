using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace BeaconColorCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    class RGB
    {
        byte red;
        byte green;
        byte blue;

        public RGB(byte r, byte g, byte b)
        {
            red = r;
            green = g;
            blue = b;
        }
        public byte GetRed()
        {
            return red;
        }
        public byte GetGreen()
        {
            return green;
        }
        public byte GetBlue()
        {
            return blue;
        }
        public static RGB FromHex(string hex)
        {
            return new(ColorTranslator.FromHtml(hex).R, ColorTranslator.FromHtml(hex).G, ColorTranslator.FromHtml(hex).B);
        }
        public override string ToString()
        {
            return $"{red}, {green}, {blue}";
        }
        public static RGB FindDifference(RGB rgb1, RGB rgb2)
        {
            return new RGB(Convert.ToByte(Math.Abs(Convert.ToInt32(rgb2.red) - Convert.ToInt32(rgb1.red))), Convert.ToByte(Math.Abs(Convert.ToInt32(rgb2.green) - Convert.ToInt32(rgb1.green))), Convert.ToByte(Math.Abs(Convert.ToInt32(rgb2.blue) - Convert.ToInt32(rgb1.blue))));
        }
        public static int FindTotalDifference(RGB rgb1, RGB rgb2)
        {
            return FindDifference(rgb1, rgb2).red + FindDifference(rgb1, rgb2).green + FindDifference(rgb1, rgb2).blue;
        }
        public static byte Abs(byte input)
        {
            return Convert.ToByte(Math.Abs(Convert.ToInt32(input)));
        }

        static string DecToHexa(int n)
        {

            // char array to store
            // hexadecimal number
            char[] hexaDeciNum = new char[2];

            // Counter for hexadecimal
            // number array
            int i = 0;
            while (n != 0)
            {

                // Temporary variable to
                // store remainder
                int temp = 0;

                // Storing remainder in
                // temp variable.
                temp = n % 16;

                // Check if temp < 10
                if (temp < 10)
                {
                    hexaDeciNum[i] = (char)(temp + 0x30);
                    i++;
                }
                else
                {
                    hexaDeciNum[i] = (char)(temp + 0x41 - 10);
                    i++;
                }
                n = n / 16;
            }
            string hexCode = "";

            if (i == 2)
            {
                hexCode += hexaDeciNum[1];
                hexCode += hexaDeciNum[0];
            }
            else if (i == 1)
            {
                hexCode = "0";
                hexCode += hexaDeciNum[0];
            }
            else if (i == 0)
                hexCode = "00";

            // Return the equivalent
            // hexadecimal color code
            return hexCode;
        }

        // Function to convert the
        // RGB code to Hex color code
        public string ToHex()
        {
            if ((red >= 0 && red <= 255) &&
                (green >= 0 && green <= 255) &&
                (blue >= 0 && blue <= 255))
            {
                string hexCode = "#";
                hexCode += DecToHexa(red);
                hexCode += DecToHexa(green);
                hexCode += DecToHexa(blue);

                return hexCode;
            }

            // The hex color code doesn't exist
            else
                return "-1";
        }
        public static RGB FindAverage(RGB[] rGBs)
        {
            int totalR = 0;
            int totalG = 0;
            int totalB = 0;

            for (int i = 0; i < rGBs.Length; i++)
            {
                totalR += rGBs[i].GetRed();
                totalG += rGBs[i].GetGreen();
                totalB += rGBs[i].GetBlue();
            }
            return new(Convert.ToByte(totalR / rGBs.Length), Convert.ToByte(totalG / rGBs.Length), Convert.ToByte(totalB / rGBs.Length));
        }
        public static RGB FindAverage(RGB rgb1, RGB rgb2)
        {
            RGB[] rGBs = {rgb1, rgb2};
            return FindAverage(rGBs);
        }
        public int GetMax()
        {
            return Math.Max(Math.Max(red, green), blue);
        }
        public int GetMin()
        {
            return Math.Min(Math.Min(red, green), blue);
        }
        public int GetHue()
        {
            double min = GetMin();
            double max = GetMax();

            if (min == max)
            {
                return 0;
            }

            double hue = 0;
            if (max == red)
            {
                hue = (green - blue) / (max - min);
            }
            else if (max == green)
            {
                hue = 2 + (blue - red) / (max - min);
            }
            else
            {
                hue = 4 + (red - green) / (max - min);
            }

            hue = hue * 60;
            if (hue < 0)
            {
                hue = hue + 360;
            }
            return Convert.ToInt32(Math.Round(hue));
        }
        public int GetBrightness()
        {
            double luminance = (double)(red + green + blue) / 3 / 255;
            return Convert.ToInt32(Math.Round(luminance));
        }
        public SolidColorBrush FindForegroundColor()
        {
            SolidColorBrush color = new SolidColorBrush(Colors.Black);
            if (GetHue() < 240 && GetHue() > 60)
            {
                color = new(Colors.Black);
            }
            else
            {
                color = new(Colors.White);
            }
            if (GetBrightness() > 0.5)
            {
                color = new(Colors.Black);
            }
            return color;
        }
    }

    class Glass
    {
        string type;
        RGB color;

        public Glass(string t, RGB c)
        {
            type = t;
            color = c;
        }

        static Glass[] glasses = 
        { 
            new("black", RGB.FromHex("#1D1D21")),
            new("red", RGB.FromHex("#B02E26")),
            new("green", RGB.FromHex("#5E7C16")),
            new("brown", RGB.FromHex("#835432")),
            new("blue", RGB.FromHex("#3C44AA")),
            new("purple", RGB.FromHex("#8932B8")),
            new("cyan", RGB.FromHex("#169C9C")),
            new("lightGray", RGB.FromHex("#9D9D97")),
            new("gray", RGB.FromHex("#474F52")),
            new("pink", RGB.FromHex("#F38BAA")),
            new("lime", RGB.FromHex("#80C71F")),
            new("yellow", RGB.FromHex("#FED83D")),
            new("lightBlue", RGB.FromHex("#3AB3DA")),
            new("magenta", RGB.FromHex("#C74EBD")),
            new("orange", RGB.FromHex("#F9801D")),
            new("white", RGB.FromHex("#F9FFFE"))
        };
        public RGB GetColor()
        {
            return color;
        }

        public new string GetType()
        {
            return type;
        }

        // Step 1: Ziel R/G/B mit allen Glass-Blöcken vergleichen
        // Step 2: Gucken wo die kleinste Differenz ist.
        // Step 3: 
        // Step 4: 
        // Step 5:

        public static Glass FindClosestGlass(RGB target, int maxGlasses) //maxGlasses can be a maximum of 6 since the color does not change after 6 glass blocks
        {
            int bestInt = 0;
            int bestDifference = 765;
            int difference;
            for (int i = 0; i < 16; i++)
            {
                difference = RGB.FindTotalDifference(target, glasses[i].color);
                if (difference < bestDifference)
                {
                    bestInt = i;
                    bestDifference = difference;
                }
            }
            return glasses[bestInt];
        }

        public static GlassCombination FindBestCombination(RGB target)
        {
            int glass1 = 0;
            int glass2 = 0;
            int glass3 = 0;
            int glass4 = 0;
            int glass5 = 0;
            int glass6 = 0;

            Glass[] bestComb = new Glass[6];
            int bestDifference = 765;
            RGB bestColor = new RGB(0, 0, 0);
            Glass[] currentComb = new Glass[6];
            int difference = 0;
            RGB color = new RGB(0, 0, 0);

            for (int i = 0; (glass6 < 15) || (bestDifference == 0); i++)
            {
                currentComb[0] = glasses[glass1];
                currentComb[1] = glasses[glass2];
                currentComb[2] = glasses[glass3];
                currentComb[3] = glasses[glass4];
                currentComb[4] = glasses[glass5];
                currentComb[5] = glasses[glass6];

                glass1++;

                if (glass1 > 15)
                {
                    glass1 -= 16;
                    glass2++;
                }
                if (glass2 > 15)
                {
                    glass2 -= 16;
                    glass3++;
                }
                if (glass3 > 15)
                {
                    glass3 -= 16;
                    glass4++;
                }
                if (glass4 > 15)
                {
                    glass4 -= 16;
                    glass5++;
                }
                if (glass5 > 15)
                {
                    glass5 -= 16;
                    glass6++;
                }

                color = FindAverageColor(currentComb);
                difference = RGB.FindTotalDifference(target, color);

                if (difference < bestDifference)
                {
                    bestDifference = difference;
                    bestColor = color;
                    bestComb[0] = currentComb[0];
                    bestComb[1] = currentComb[1];
                    bestComb[2] = currentComb[2];
                    bestComb[3] = currentComb[3];
                    bestComb[4] = currentComb[4];
                    bestComb[5] = currentComb[5];
                }
            }

            return new GlassCombination(bestComb, bestColor);
        }

        public static RGB FindAverageColor(Glass[] glasses) //Glasses can be a maximum of 6 since the color does not change after 6 glass blocks
        {
            RGB[] colors = new RGB[6];

            for (int i = 0; i < 6; i++)
            {
                colors[i] = glasses[i].color;
            }
            return RGB.FindAverage(colors[5], RGB.FindAverage(colors[4], RGB.FindAverage(colors[3], RGB.FindAverage(colors[2], colors[1]))));
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
            
            /*RGB target = RGB.FromHex("#FF4500");
            GlassCombination output = Glass.FindBestCombination(target);
            lbl_outputColor.Content = output.GetRGB().ToString();
            lbl_outputColor.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            lbl_outputColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetRGB().GetRed(), output.GetRGB().GetGreen(), output.GetRGB().GetBlue()));

            lbl_glass1.Content = output.GetGlasses()[0].GetType();
            lbl_glass1.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetGlasses()[0].GetColor().GetRed(), output.GetGlasses()[0].GetColor().GetGreen(), output.GetGlasses()[0].GetColor().GetBlue()));

            lbl_glass2.Content = output.GetGlasses()[1].GetType();
            lbl_glass2.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetGlasses()[1].GetColor().GetRed(), output.GetGlasses()[1].GetColor().GetGreen(), output.GetGlasses()[1].GetColor().GetBlue()));
            //lbl_glass3.Content = output.GetGlasses()[2].GetType();
            //lbl_glass4.Content = output.GetGlasses()[3].GetType();
            //lbl_glass5.Content = output.GetGlasses()[4].GetType();
            //lbl_glass6.Content = output.GetGlasses()[5].GetType();

            // Glas-Kombination als Output, nicht der beste RGB Wert
            //
            */
        }

        private void btn_cont_Click(object sender, RoutedEventArgs e)
        {
            RGB target = new(0, 0, 0);
            if (rb_rgb.IsChecked == true)
            {
                target = new RGB(Convert.ToByte(txt_red.Text), Convert.ToByte(txt_green.Text), Convert.ToByte(txt_blue.Text));
                
                txt_hex.Text = target.ToHex();
            }
            else if (rb_hex.IsChecked == true)
            {
                target = new RGB(ColorTranslator.FromHtml(txt_hex.Text).R, ColorTranslator.FromHtml(txt_hex.Text).G, ColorTranslator.FromHtml(txt_hex.Text).B);
                
                txt_red.Text = target.GetRed().ToString();
                txt_green.Text = target.GetGreen().ToString();
                txt_blue.Text = target.GetBlue().ToString();
            }

            lbl_inputColor.Content = target.ToString() + "\n" + target.ToHex();
            lbl_inputColor.Foreground = target.FindForegroundColor();
            lbl_inputColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(target.GetRed(), target.GetGreen(), target.GetBlue()));
            
            GlassCombination output = Glass.FindBestCombination(target);
            lbl_outputColor.Content = output.GetRGB().ToString() + "\n" + output.GetRGB().ToHex();
            lbl_outputColor.Foreground = output.GetRGB().FindForegroundColor();
            lbl_outputColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetRGB().GetRed(), output.GetRGB().GetGreen(), output.GetRGB().GetBlue()));

            lbl_glass6.Content = output.GetGlasses()[5].GetType();
            lbl_glass6.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetGlasses()[5].GetColor().GetRed(), output.GetGlasses()[5].GetColor().GetGreen(), output.GetGlasses()[5].GetColor().GetBlue()));
            lbl_outputColor.Foreground = output.GetGlasses()[5].GetColor().FindForegroundColor();

            lbl_glass5.Content = output.GetGlasses()[4].GetType();
            lbl_glass5.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetGlasses()[4].GetColor().GetRed(), output.GetGlasses()[4].GetColor().GetGreen(), output.GetGlasses()[4].GetColor().GetBlue()));
            lbl_outputColor.Foreground = output.GetGlasses()[4].GetColor().FindForegroundColor();

            lbl_glass4.Content = output.GetGlasses()[3].GetType();
            lbl_glass4.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetGlasses()[3].GetColor().GetRed(), output.GetGlasses()[3].GetColor().GetGreen(), output.GetGlasses()[3].GetColor().GetBlue()));
            lbl_outputColor.Foreground = output.GetGlasses()[3].GetColor().FindForegroundColor();

            lbl_glass3.Content = output.GetGlasses()[2].GetType();
            lbl_glass3.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetGlasses()[2].GetColor().GetRed(), output.GetGlasses()[2].GetColor().GetGreen(), output.GetGlasses()[2].GetColor().GetBlue()));
            lbl_outputColor.Foreground = output.GetGlasses()[2].GetColor().FindForegroundColor();

            lbl_glass2.Content = output.GetGlasses()[1].GetType();
            lbl_glass2.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetGlasses()[1].GetColor().GetRed(), output.GetGlasses()[1].GetColor().GetGreen(), output.GetGlasses()[1].GetColor().GetBlue()));
            lbl_outputColor.Foreground = output.GetGlasses()[1].GetColor().FindForegroundColor();

            lbl_glass1.Content = output.GetGlasses()[0].GetType();
            lbl_glass1.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(output.GetGlasses()[0].GetColor().GetRed(), output.GetGlasses()[0].GetColor().GetGreen(), output.GetGlasses()[0].GetColor().GetBlue()));
            lbl_outputColor.Foreground = output.GetGlasses()[0].GetColor().FindForegroundColor();
        }
    }
    struct GlassCombination
    {
        Glass[] glasses;
        RGB bestRGB;

        public GlassCombination(Glass[] g, RGB bRGB)
        {
            this.glasses = g;
            this.bestRGB = bRGB;
        }

        public Glass[] GetGlasses()
        {
            return glasses;
        }
        public RGB GetRGB()
        {
            return bestRGB;
        }
    }
}
