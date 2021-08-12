﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace CheckersWPF.Game
{
    static class CreateBlack
    {

        static public List<Ellipse> Create()
        {
            double width = 46, height = 46;
            List<Ellipse> ellipsesB = new List<Ellipse>(12);
            for (int i = 0; i < 12; i++)
            {
                ellipsesB.Add(new Ellipse());
                ellipsesB[i].HorizontalAlignment = HorizontalAlignment.Left;
                ellipsesB[i].VerticalAlignment = VerticalAlignment.Top;
                ellipsesB[i].Width = width;
                ellipsesB[i].Height = height;
                ellipsesB[i].Margin = new Thickness(-50, -50, 0, 0);
                ellipsesB[i].Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                ellipsesB[i].Fill = new LinearGradientBrush(Color.FromRgb(0, 0, 0), Color.FromRgb(90, 90, 90), new Point(0, 0.5), new Point(0.5, 1));
                ellipsesB[i].Name = "c_B_" + i;
                //ellipsesB[i].MouseLeftButtonDown += new MouseButtonEventHandler(ChechersClick);
                //MainGrid.Children.Add(ellipsesB[i]);
            }

            ellipsesB[0].Margin = new Thickness(2, 252, 0, 0);
            ellipsesB[1].Margin = new Thickness(102, 252, 0, 0);
            ellipsesB[2].Margin = new Thickness(202, 252, 0, 0);
            ellipsesB[3].Margin = new Thickness(302, 252, 0, 0);
            ellipsesB[4].Margin = new Thickness(52, 302, 0, 0);
            ellipsesB[5].Margin = new Thickness(152, 302, 0, 0);
            ellipsesB[6].Margin = new Thickness(252, 302, 0, 0);
            ellipsesB[7].Margin = new Thickness(352, 302, 0, 0);
            ellipsesB[8].Margin = new Thickness(2, 352, 0, 0);
            ellipsesB[9].Margin = new Thickness(102, 352, 0, 0);
            ellipsesB[10].Margin = new Thickness(202, 352, 0, 0);
            ellipsesB[11].Margin = new Thickness(302, 352, 0, 0);
            return ellipsesB;
        }
    }
}