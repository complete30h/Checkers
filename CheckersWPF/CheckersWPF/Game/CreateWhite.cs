using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using CheckersWPF;

namespace CheckersWPF.Game
{
    static class CreateWhite
    {

        static public List<Ellipse> Create()
        {
            double width = 46, height = 46;
            List<Ellipse> ellipsesW = new List<Ellipse>(12);
            for (int i = 0; i < 12; i++)
            {
                ellipsesW.Add(new Ellipse());
                ellipsesW[i].HorizontalAlignment = HorizontalAlignment.Left;
                ellipsesW[i].VerticalAlignment = VerticalAlignment.Top;
                ellipsesW[i].Width = width;
                ellipsesW[i].Height = height;
                ellipsesW[i].Margin = new Thickness(-50, -50, 0, 0);
                ellipsesW[i].Stroke = new LinearGradientBrush(Color.FromRgb(230, 230, 230), Color.FromRgb(190, 190, 190), new Point(0, 0.5), new Point(0.5, 1));
                
                ellipsesW[i].Fill = new LinearGradientBrush(Color.FromRgb(150, 150, 150), Color.FromRgb(190, 190, 190), new Point(0, 0.5), new Point(0.5, 1));
                ellipsesW[i].Name = "c_W_" + i;
                ellipsesW[i].StrokeThickness = 4;

               // ellipse.Stroke = new LinearGradientBrush(Color.FromRgb(230, 230, 230), Color.FromRgb(170, 170, 170), new Point(0, 0.5), new Point(0.5, 1));
                //ellipse.StrokeThickness = 2;
                //ellipse.Fill = new LinearGradientBrush(Color.FromRgb(150, 150, 150), Color.FromRgb(190, 190, 190), new Point(0, 0.5), new Point(0.5, 1));



            }

            ellipsesW[0].Margin = new Thickness(52, 2, 0, 0);
            ellipsesW[1].Margin = new Thickness(152, 2, 0, 0);
            ellipsesW[2].Margin = new Thickness(252, 2, 0, 0);
            ellipsesW[3].Margin = new Thickness(352, 2, 0, 0);
            ellipsesW[4].Margin = new Thickness(2, 52, 0, 0);
            ellipsesW[5].Margin = new Thickness(102, 52, 0, 0);
            ellipsesW[6].Margin = new Thickness(202, 52, 0, 0);
            ellipsesW[7].Margin = new Thickness(302, 52, 0, 0);
            ellipsesW[8].Margin = new Thickness(52, 102, 0, 0);
            ellipsesW[9].Margin = new Thickness(152, 102, 0, 0);
            ellipsesW[10].Margin = new Thickness(252, 102, 0, 0);
            ellipsesW[11].Margin = new Thickness(352, 102, 0, 0);
            return ellipsesW;
        }
}
}
