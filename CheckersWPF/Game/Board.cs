using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using CheckersWPF;
using System.Windows.Controls;

namespace CheckersWPF.Game
{
    class Board
    {
        Rectangle[] rectangles = new Rectangle[64];
        List<Ellipse> ellipsesWhite = new List<Ellipse>(12);
        List<Ellipse> ellipsesBlack = new List<Ellipse>(12);
        public Rectangle[] CreateBoard()
        {
            
            double x = 0, y = 0, step = 0;
            double width = 50, height = 50;
            bool color = true;
            for (int i = 0; i<rectangles.Length;i++)
            {
                if (step == 8)
                {
                    y += height;
                    step = 0;
                    x = 0;
                    color = !(color);
                }
                x = width * step;
                step++;
                rectangles[i] = new Rectangle();
                rectangles[i].Width = width;
                rectangles[i].Height = height;
                rectangles[i].Margin = new Thickness(x, y, 0, 0);
                rectangles[i].Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                rectangles[i].HorizontalAlignment = HorizontalAlignment.Left;
                rectangles[i].VerticalAlignment = VerticalAlignment.Top;
                

                if (color)
                    {
                        rectangles[i].Fill = new SolidColorBrush(Color.FromRgb(210, 210, 210));
                        color = !(color);
                    }
                    else
                    {
                        rectangles[i].Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                        color = !(color);
                    }

            }
            return rectangles;
            
        }
    }
}
