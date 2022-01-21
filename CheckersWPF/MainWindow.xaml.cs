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
using CheckersWPF.Game;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Media.Animation;

namespace CheckersWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool white { get; set; } = true;
        int CountKillW = 0;
        int CountKillB = 0;
        int animIter = 0;
        int animIter2 = 0;
        bool D = false;
        volatile bool work = true;
        Socket handler;
        Socket sender;
        Rectangle[] rectangles = new Rectangle[64];
        Grid grid1 = new Grid();
        List<Ellipse> ellipsesW = new List<Ellipse>(12);
        List<Ellipse> ellipsesB = new List<Ellipse>(12);
        List<Rectangle> rectanglesColor = new List<Rectangle>();
        TextBlock label = new TextBlock();
        TextBox textBox = new TextBox();
        volatile string ip;
        Button btnS = new Button();
        Button btnC = new Button();
        Button ClearConsole = new Button();
        Button side = new Button();
        bool mustKill = false;
        Ellipse select;
        Brush selectColor;
        bool ServerBool = false;
        bool CourseWhite = true;
        Ellipse ellipseAnim;
        bool _white = true;


        List<string> PointWayStandart = new List<string>();
        List<string> PointWay = new List<string>();
        List<List<string>> Ways = new List<List<string>>();
        List<string> PointOfCourse = new List<string>();
        List<string> waySelect = new List<string>();

        Thread thread;

        string nowPos = "";
        public MainWindow()
        {
            InitializeComponent();
            Init();
            var abc = 1;
        }
        public void Init()
        {
            this.Width = 604;
            this.Height = 438;
            this.Closed += new EventHandler(CloseWindow);
            this.ResizeMode = ResizeMode.NoResize;
            Board board = new Board();
            rectangles = board.CreateBoard();
            for (int i = 0; i < rectangles.Length; i++)
            {
                rectangles[i].MouseLeftButtonDown += new MouseButtonEventHandler(RectangleClick);
                rectangles[i].MouseEnter += new MouseEventHandler(RectangleMouseEnter);
                MainGrid.Children.Add(rectangles[i]);
            }
            ellipsesW = CreateWhite.Create();
            for (int i = 0; i < ellipsesW.Count; i++)
            {
                ellipsesW[i].MouseLeftButtonDown += new MouseButtonEventHandler(ChechersClick);
                MainGrid.Children.Add(ellipsesW[i]);
            }
            ellipsesB = CreateBlack.Create();
            for (int i = 0; i < ellipsesB.Count; i++)
            {
                ellipsesB[i].MouseLeftButtonDown += new MouseButtonEventHandler(ChechersClick);
                MainGrid.Children.Add(ellipsesB[i]);
            }
            btnS.Width = 180;
            btnS.Height = 25;
            btnS.Content = "Создать";
            btnS.HorizontalAlignment = HorizontalAlignment.Right;
            btnS.VerticalAlignment = VerticalAlignment.Top;
            btnS.Margin = new Thickness(0, 4, 4, 0);
            btnS.Click += new RoutedEventHandler(btnSClick);

            btnC.Width = 180;
            btnC.Height = 25;
            btnC.Content = "Присоединиться";
            btnC.HorizontalAlignment = HorizontalAlignment.Right;
            btnC.VerticalAlignment = VerticalAlignment.Top;
            btnC.Margin = new Thickness(0, 32, 4, 0);
            btnC.Click += new RoutedEventHandler(btnCClick);

            textBox.Width = 180;
            textBox.Height = 24;
            textBox.HorizontalAlignment = HorizontalAlignment.Right;
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.Margin = new Thickness(0, 88, 4, 0);
            textBox.Text = "";

            side.Width = 180;
            side.Height = 25;
            side.Content = "Выбрать сторону";
            side.HorizontalAlignment = HorizontalAlignment.Right;
            side.VerticalAlignment = VerticalAlignment.Top;
            side.Margin = new Thickness(0, 60, 4, 0);
            side.Click += new RoutedEventHandler(sideClick);

            label.Width = 180;
            label.Height = 600;
            label.HorizontalAlignment = HorizontalAlignment.Right;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Margin = new Thickness(0, 100, 4, 0);
            label.Text = "";
            label.TextWrapping = TextWrapping.WrapWithOverflow;

            MainGrid.Children.Add(textBox);
            MainGrid.Children.Add(label);
            MainGrid.Children.Add(btnC);
            MainGrid.Children.Add(btnS);
            MainGrid.Children.Add(side);

        }

        void sideClick(object s, RoutedEventArgs e)
        {
            var frm = new SubWindow();
            frm.Owner = this;
            frm.ShowDialog();


            if (frm.DialogResult == false)
            {
                return;
            }
            else
            {
                //ServerBool = true;
                _white = white;
                ip = textBox.Text;
                if (_white)
                {
                    CourseWhite = true;
                }
                else
                {
                    CourseWhite = false;
                }
            }
        }

        void btnClearClick(object s, RoutedEventArgs e)
        {
            label.Text = "";
        }     // обработчик нажатия по кнопки "отчистить консоль"

        void CloseWindow(object s, EventArgs e)
        {
            work = false;
        } // через глобальную переменную work сообщеает потокам что работа закончена

        void btnCClick(object s, RoutedEventArgs e)
        {
            ip = textBox.Text;

            try
            {
                Client("connect");
                btnS.IsEnabled = false;
                btnC.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Write(ex.Message);
            }
        }  // обработчик нажатия кнопки "клиент"

        void btnSClick(object s, RoutedEventArgs e)
        {

            //var frm = new SubWindow();
          //  frm.Owner = this;
           // frm.ShowDialog();
           

          //  if (frm.DialogResult == false)
           // {
           //     return;
          //  }
          //  else
          //  {
                ServerBool = true;
             //   _white = white;
                ip = textBox.Text;
                if (_white)
                {
                    CourseWhite = true;
                }
                else
                {
                    CourseWhite = false;
                }
                if (thread != null)
                {
                   
                }
                thread = new Thread(Server);
                thread.IsBackground = true;
                thread.Start();
                btnC.IsEnabled = false;
                btnS.IsEnabled = false;
            
        }  // обработчик нажатия кнопки "сервер"

        void ChechersClick(object s, MouseButtonEventArgs e)
        {
            mustKill = false;

            string[] sp = ((Ellipse)s).Name.Split('_');
            if (sp[1] == "W" && CourseWhite && ServerBool && _white || sp[1] == "B" && CourseWhite && ServerBool && !_white || sp[1] == "B" && !CourseWhite && !ServerBool && !_white || sp[1] == "W" && !CourseWhite && !ServerBool && _white)
            {
                if (select != null && selectColor != null)
                    select.Fill = selectColor;

                select = (Ellipse)s;
                selectColor = select.Fill;

                select.Fill = new LinearGradientBrush(Color.FromRgb(0, 200, 0), Color.FromRgb(0, 70, 0), new Point(0, 0.5), new Point(0.5, 1));

                bool isKill = ThereIsKill(select);
                mustKill = false;
                GetPointForCourse(select);

                if (isKill != mustKill)
                {
                    Write("Надо бить!");

                    Ways.Clear();
                    PointWay.Clear();
                    waySelect.Clear();

                    ClearColorRectangles();

                    if (selectColor != null)
                        select.Fill = selectColor;

                    select = null;
                    return;
                }

                GenerateColorRectangle();
            }
        } // обработчик клика по шашке

        bool ThereIsKill(Ellipse ellipse)
        {
            mustKill = false;

            if (ellipse.Name.Split('_')[1] == "W")
            {
                foreach (var el in ellipsesW)
                {
                    GetPointForCourse(el);
                }
            }
            else if (ellipse.Name.Split('_')[1] == "B")
            {
                foreach (var el in ellipsesB)
                {
                    GetPointForCourse(el);
                }
            }

            return mustKill;
        }

        void RectangleClick(object s, MouseButtonEventArgs e)
        {
            if (select != null && select.Name.Split('_')[1] == "W" && CourseWhite && _white || select != null && select.Name.Split('_')[1] == "B" && CourseWhite && !_white||

                select != null && select.Name.Split('_')[1] == "W" && !CourseWhite && _white|| select != null && select.Name.Split('_')[1] == "B" && !CourseWhite && !_white)
            {
                double xRect = 0, yRect = 0, xPoint = 0, yPoint = 0;
                GetPositionRectangle((Rectangle)s, ref xRect, ref yRect);

                for (int i = 0; i < PointOfCourse.Count; i++)
                {
                    GetPositionPoint(PointOfCourse[i], ref xPoint, ref yPoint);

                    if (xRect == xPoint && yRect == yPoint)
                    {
                        SetPosition(select.Name + ";" + xRect + "," + yRect);
                        SendMessage(select.Name + ";" + xRect + "," + yRect);
                        ClearColorRectangles();
                        select.Fill = selectColor;
                        //CourseWhite = !CourseWhite;

                        if (selectColor != null)
                            select.Fill = selectColor;

                        select = null;
                    }
                }
            }
        } // обработчик клика по квадратику

        void RectangleMouseEnter(object s, MouseEventArgs e)
        {
            double xRect = 0, yRect = 0, xPoint = 0, yPoint = 0, xR = 0, yR = 0;
            GetPositionRectangle((Rectangle)s, ref xRect, ref yRect);

            if (select != null && select.StrokeThickness == 2)
                foreach (var way in Ways)
                {
                    if (way.Count > 1)
                    {
                        foreach (var point in way)
                        {
                            GetPositionPoint(way.Last(), ref xPoint, ref yPoint);

                            if (xPoint == xRect && yPoint == yRect)
                            {
                                if (point.Split(',')[2] != "start" && point.Split(',')[2] != "end")
                                {
                                    foreach (var rectangle in rectangles)
                                    {
                                        for (int i = 0; i < way.Count - 1; i++)
                                        {
                                            GetPositionPoint(way[i], ref xPoint, ref yPoint);
                                            GetPositionRectangle(rectangle, ref xR, ref yR);

                                            if (xPoint == xR && yPoint == yR && !rectanglesColor.Exists((r) => r == rectangle))
                                            {
                                                rectanglesColor.Add(rectangle);
                                                rectangle.Fill = new LinearGradientBrush(Color.FromRgb(120, 0, 0), Color.FromRgb(240, 0, 0), new Point(0.5, 0.5), new Point(0, 1));
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ClearColorRectangles();
                            }
                        }
                    }
                }
        } // обработчик события на то, что курсор попал в область квадратика
     

        void GenerateColorRectangle()
        {
            ClearColorRectangles();
            rectanglesColor.Clear();

            foreach (var rectangle in rectangles)
            {
                double x = 0, y = 0;
                GetPositionRectangle(rectangle, ref x, ref y);

                foreach (var point in PointWay)
                {
                    if (double.Parse(point.Split(',')[0]) == x && double.Parse(point.Split(',')[1]) == y)
                    {
                        if (point.Split(',')[2] == "end")
                        {
                            rectanglesColor.Add(rectangle);
                            rectangle.Fill = new LinearGradientBrush(Color.FromRgb(0, 100, 0), Color.FromRgb(0, 210, 0), new Point(0.5, 0.5), new Point(0, 1));
                        }
                        else if (point.Split(',')[2] == "start")
                        {
                            rectanglesColor.Add(rectangle);
                            rectangle.Fill = new LinearGradientBrush(Color.FromRgb(30, 50, 30), Color.FromRgb(120, 160, 120), new Point(0.5, 0.5), new Point(0, 1));
                        }
                        else if (point.Split(',')[2] == "fork" || point.Split(',')[2] == "way")
                        {
                            rectanglesColor.Add(rectangle);
                            rectangle.Fill = new LinearGradientBrush(Color.FromRgb(120, 0, 0), Color.FromRgb(240, 0, 0), new Point(0.5, 0.5), new Point(0, 1));
                        }
                    }
                }

                foreach (var point in PointWayStandart)
                {
                    if (double.Parse(point.Split(',')[0]) == x && double.Parse(point.Split(',')[1]) == y && PointWay.Count <= 1)
                    {
                        rectanglesColor.Add(rectangle);
                        rectangle.Fill = new LinearGradientBrush(Color.FromRgb(0, 100, 0), Color.FromRgb(0, 210, 0), new Point(0.5, 0.5), new Point(0, 1));
                    }
                }
            }
        } // подсветка квадратиков 

        void ClearColorRectangles()
        {
            for (int i = 0; i < rectanglesColor.Count; i++)
            {
                rectanglesColor[i].Fill = new SolidColorBrush(Color.FromRgb(101, 67, 33));
            }

            rectanglesColor.Clear();
        }   // очищает подсвеченые квадратики

        void GetPositionEllipse(Ellipse el, ref double x, ref double y)
        {
            x = (el.Margin.Left - 2) / 50;
            y = (el.Margin.Top - 2) / 50;
        } // получает координаты шашки

        void GetPositionRectangle(Rectangle re, ref double x, ref double y)
        {
            x = re.Margin.Left / 50;
            y = re.Margin.Top / 50;
        } // получает координаты квадратика

        void GetPositionPoint(string point, ref double x, ref double y)
        {
            x = double.Parse(point.Split(',')[0]);
            y = double.Parse(point.Split(',')[1]);
        }  // получает координаты x y в типе double из сторокго представляния координаты

        Ellipse GetEllipseCoordinate(double x, double y)
        {
            double xEl = 0, yEl = 0;

            foreach (var el in ellipsesW)
            {
                GetPositionEllipse(el, ref xEl, ref yEl);

                if (x == xEl && y == yEl)
                    return el;
            }

            foreach (var el in ellipsesB)
            {
                GetPositionEllipse(el, ref xEl, ref yEl);

                if (x == xEl && y == yEl)
                    return el;
            }

            return new Ellipse();
        } // возвращает шашку по координатам

        void SwitchCourse(string msg)
        {
            if (msg.Split(';')[0].Split('_')[1] == "W")
            {
                ServerBool = false;
                CourseWhite = false;
            }
            else if (msg.Split(';')[0].Split('_')[1] == "B")
            {
                ServerBool = true;
                CourseWhite = true;
            }
        }  // переключает ход с белых на черные и наоборот

        void Write(string str)
        {
            Action action = () => { label.Text += "\n" + str; };
            Dispatcher.Invoke(action);
        } // выводит в текстовую область сообщения

        //// логика игры

        void GetPointForCourse(Ellipse ellipse)
        {
            PointWayStandart.Clear();
            PointWay.Clear();
            Ways.Clear();
            PointOfCourse.Clear();

            if (ellipse.StrokeThickness != 2)
            {
                D = false;
                GeneratePointWayStandart(ellipse, PointWayStandart);
                GeneratePointWay(ellipse);
                GenerateWays(ellipse);
            }
            else
            {
                D = true;
                GeneratePointWayStandartD(ellipse, PointWayStandart);

                int[,] desk = new int[8, 8];
                double x = 0, y = 0;
                GetPositionEllipse(ellipse, ref x, ref y);
                List<string> MemoryPoints = new List<string>();
                List<int[,]> MemoryMatrix = new List<int[,]>();
                MemoryPoints.Add(x + "," + y);
                desk = GenerateMatrix();
                MemoryMatrix.Add(desk);
                Ways.Clear();
                desk[(int)y, (int)x] = 0;

                if (ellipse.Name.Split('_')[1] == "W")
                    generateWays(MemoryMatrix.Last(), (int)x, (int)y, MemoryPoints, MemoryMatrix, 1);
                if (ellipse.Name.Split('_')[1] == "B")
                    generateWays(MemoryMatrix.Last(), (int)x, (int)y, MemoryPoints, MemoryMatrix, 2);

                foreach (var way in Ways)
                {
                    PointWay.Add(way.First() + ",start");
                    if (way.Count >= 2)
                    {
                        PointWay.Add(way.Last() + ",end");
                    }
                }

                foreach (var way in Ways)
                {
                    foreach (var point in way)
                    {
                        if (!PointWay.Exists((p) => point.Split(',')[0] + "," + point.Split(',')[1] == p.Split(',')[0] + "," + p.Split(',')[1]))
                            PointWay.Add(point);
                    }
                }

                for (int i = 0; i < PointWay.Count; i++)
                {
                    if (PointWay[i].Length == 3)
                        PointWay[i] += ",no";
                }

                for (int i = 0; i < Ways.Count; i++)
                {
                    for (int j = 0; j < Ways[i].Count; j++)
                    {
                        if (j == 0)
                            Ways[i][j] += ",start";
                        else if (j >= 1 && j < Ways[i].Count)
                            Ways[i][j] += ",no";
                        else if (j == Ways[i].Count)
                            Ways[i][j] += ",end";
                    }
                }

                for (int i = 0; i < Ways.Count; i++)
                {
                    if (Ways[i].Count == 1)
                        Ways.RemoveAt(i);
                }
            }

            if (PointWay.Count <= 1)
            {
                foreach (var point in PointWayStandart)
                {
                    PointOfCourse.Add(point);
                }
            }
            else
            {
                foreach (var point in PointWay)
                {
                    if (point.Split(',')[2] == "end")
                        PointOfCourse.Add(point);
                }
            }
            


        } // получаем точки на которые можно походить/ отсюда запускаются остальные алгоритмы нужные для просчета возможных ходов

        public void GeneratePointWayStandartD(Ellipse ellipse, List<string> list)
        {
            double x = 0, y = 0, x1 = 0, y1 = 0;
            GetPositionEllipse(ellipse, ref x, ref y);
            for (int i = 1; i < 7; i++)
            {
                x1 = x - i; y1 = y - i;
                if (IsRectangleFree(x1, y1) && IsPositionInLimit(x1, y1))
                    list.Add(x1 + "," + y1);
                else
                    break;
            }

            for (int i = 1; i < 7; i++)
            {
                x1 = x + i; y1 = y - i;
                if (IsRectangleFree(x1, y1) && IsPositionInLimit(x1, y1))
                    list.Add(x1 + "," + y1);
                else
                    break;
            }

            for (int i = 1; i < 7; i++)
            {
                x1 = x + i; y1 = y + i;
                if (IsRectangleFree(x1, y1) && IsPositionInLimit(x1, y1))
                    list.Add(x1 + "," + y1);
                else
                    break;
            }

            for (int i = 1; i < 7; i++)
            {
                x1 = x - i; y1 = y + i;
                if (IsRectangleFree(x1, y1) && IsPositionInLimit(x1, y1))
                    list.Add(x1 + "," + y1);
                else
                    break;
            }

        } // генерируем точки на который может походить дамка если некого побить

        public void generateWays(int[,] desk, int xCurr, int yCurr, List<string> list, List<int[,]> listMatrix, int color)
        {
            int xStart = xCurr, yStart = yCurr;
            string pointStart = xStart + "," + yStart;

            if (desk[yCurr, xCurr] == 0)
            {
                desk[yCurr, xCurr] = 10;
                goOnDirWay(color, pointStart, list, listMatrix, ref desk, xCurr, yCurr, (ref int x, ref int y) => x-- != 0 && y-- != 0);
            }
            else if (desk[yCurr, xCurr] == 10)
            {
                desk[yCurr, xCurr]++;
                goOnDirWay(color, pointStart, list, listMatrix, ref desk, xCurr, yCurr, (ref int x, ref int y) => x++ != 7 && y-- != 0);
            }
            else if (desk[yCurr, xCurr] == 11)
            {
                desk[yCurr, xCurr]++;
                goOnDirWay(color, pointStart, list, listMatrix, ref desk, xCurr, yCurr, (ref int x, ref int y) => x++ != 7 && y++ != 7);
            }
            else if (desk[yCurr, xCurr] == 12)
            {
                desk[yCurr, xCurr]++;
                goOnDirWay(color, pointStart, list, listMatrix, ref desk, xCurr, yCurr, (ref int x, ref int y) => x-- != 0 && y++ != 7);
            }
            else if (desk[yCurr, xCurr] == 13)
            {
                if (desk[int.Parse(pointStart.Split(',')[1]), int.Parse(pointStart.Split(',')[0])] == 13)
                {
                    int[,] matrix = new int[listMatrix.Last().GetLength(0), listMatrix.Last().GetLength(1)];

                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            if (listMatrix.Last()[i, j] == 10 || listMatrix.Last()[i, j] == 11 || listMatrix.Last()[i, j] == 12 || listMatrix.Last()[i, j] == 13)
                                matrix[i, j] = 0;
                            else
                                matrix[i, j] = listMatrix.Last()[i, j];
                        }
                    }

                    if (IsEnd(pointStart, matrix, xCurr, yCurr, color))
                        Ways.Add(new List<string>(list));

                    list.Remove(list.Last());
                    listMatrix.Remove(listMatrix.Last());

                    if (list.Count != 0)
                        generateWays(listMatrix.Last(), int.Parse(list.Last().Split(',')[0]), int.Parse(list.Last().Split(',')[1]), list, listMatrix, color);
                }
            }

        } // алгоритм просчитывающий цепочку возможных ходов (для дамок)

        public delegate bool stepMethod(ref int x, ref int y); // делегат который используется как входной параметр метода goOnDirWay

        public void goOnDirWay(int color, string pointStart, List<string> list, List<int[,]> listMatrix, ref int[,] desk, int xCurr, int yCurr, stepMethod isNotEndPoint)
        {
            int xPrev = xCurr, yPrev = yCurr;

            while (isNotEndPoint(ref xCurr, ref yCurr))
            {
                if (desk[yCurr, xCurr] != 0 && desk[yCurr, xCurr] != 10 && desk[yCurr, xCurr] != 11 && desk[yCurr, xCurr] != 12 && desk[yCurr, xCurr] != 13) // если не пустая
                {
                    if (desk[yCurr, xCurr] != color) // если враг
                    {
                        int yStep = yCurr - yPrev;
                        int xStep = xCurr - xPrev;

                        if (isNotEndPoint(ref xCurr, ref yCurr))
                        {
                            if (desk[yCurr, xCurr] == 0 || desk[yCurr, xCurr] == 10 || desk[yCurr, xCurr] == 11 || desk[yCurr, xCurr] == 12)
                            {
                                mustKill = true;
                                list.Add(xCurr + "," + yCurr);

                                int[,] ar = new int[desk.GetLength(0), desk.GetLength(1)];

                                for (int i = 0; i < ar.GetLength(0); i++)
                                {
                                    for (int j = 0; j < ar.GetLength(1); j++)
                                    {
                                        ar[i, j] = desk[i, j];
                                    }
                                }

                                ar[yCurr - yStep, xCurr - xStep] = 0;


                                listMatrix.Add(ar);
                                generateWays(listMatrix.Last(), xCurr, yCurr, list, listMatrix, color);
                                return;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                }
                xPrev = xCurr;
                yPrev = yCurr;
            }
            generateWays(listMatrix.Last(), int.Parse(pointStart.Split(',')[0]), int.Parse(pointStart.Split(',')[1]), list, listMatrix, color);
            return;
        } // просмотривает путь в указанном направлении для поиска объекта который можно побить (используется для дамок)

        public bool IsEnd(string pointStart, int[,] desk, int xCurr, int yCurr, int color)
        {
            if (IsGoOnDirWay(pointStart, desk, xCurr, yCurr, (ref int x, ref int y) => x-- != 0 && y-- != 0, color))
                return false;

            if (IsGoOnDirWay(pointStart, desk, xCurr, yCurr, (ref int x, ref int y) => x++ != 7 && y-- != 0, color))
                return false;

            if (IsGoOnDirWay(pointStart, desk, xCurr, yCurr, (ref int x, ref int y) => x++ != 7 && y++ != 7, color))
                return false;

            if (IsGoOnDirWay(pointStart, desk, xCurr, yCurr, (ref int x, ref int y) => x-- != 0 && y++ != 7, color))
                return false;

            return true;
        } // определяет является ли точка конечной (для дамок)

        public bool IsGoOnDirWay(string pointStart, int[,] desk, int xCurr, int yCurr, stepMethod isNotEndPoint, int color)
        {
            int xPrev = xCurr, yPrev = yCurr;

            while (isNotEndPoint(ref xCurr, ref yCurr))
            {
                if (desk[yCurr, xCurr] != 0 && desk[yCurr, xCurr] != 10 && desk[yCurr, xCurr] != 11 && desk[yCurr, xCurr] != 12 && desk[yCurr, xCurr] != 13) // если не пустая
                {
                    if (desk[yCurr, xCurr] != color) // если враг
                    {
                        int yStep = yCurr - yPrev;
                        int xStep = xCurr - xPrev;

                        if (!isNotEndPoint(ref xCurr, ref yCurr))
                        {

                        }
                        else
                        {
                            if (desk[yCurr, xCurr] == 0 || desk[yCurr, xCurr] == 10 || desk[yCurr, xCurr] == 11 || desk[yCurr, xCurr] == 12 || desk[yCurr, xCurr] == 13)
                            {
                                return true;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                }
                xPrev = xCurr;
                yPrev = yCurr;
            }

            return false;
        } // помогает методу IsEnd определить является ли точка конечной

        int[,] GenerateMatrix()
        {
            int[,] desk = new int[8, 8];

            double x = 0, y = 0;

            foreach (var el in ellipsesB)
            {
                if (el.Margin.Left != -50)
                {
                    GetPositionEllipse(el, ref x, ref y);
                    desk[(int)y, (int)x] = 2;
                }
            }

            foreach (var el in ellipsesW)
            {
                if (el.Margin.Left != -50)
                {
                    GetPositionEllipse(el, ref x, ref y);
                    desk[(int)y, (int)x] = 1;
                }
            }

            return desk;
        } // генерирует матрицу из текущего положения игры (нужно для просчета цепочек ходов дамки)

        void GeneratePointWayStandart(Ellipse ellipse, List<string> list)
        {
            double x = 0, y = 0;
            GetPositionEllipse(ellipse, ref x, ref y);

            if (ellipse.Name.Split('_')[1] == "B" && IsRectangleFree(x + 1, y - 1))
                list.Add((x + 1) + "," + (y - 1));

            if (ellipse.Name.Split('_')[1] == "B" && IsRectangleFree(x - 1, y - 1))
                list.Add((x - 1) + "," + (y - 1));

            if (ellipse.Name.Split('_')[1] == "W" && IsRectangleFree(x - 1, y + 1))
                list.Add((x - 1) + "," + (y + 1));

            if (ellipse.Name.Split('_')[1] == "W" && IsRectangleFree(x + 1, y + 1))
                list.Add((x + 1) + "," + (y + 1));

        } // генерирует пути для шашки если нет возможности побить

        void GeneratePointWay(Ellipse ellipse)
        {
            double x = 0, y = 0;
            GetPositionEllipse(ellipse, ref x, ref y);
            PointWay.Add(x + "," + y + ",start");

            for (int i = 0; i < PointWay.Count; i++)
            {
                GetPositionPoint(PointWay[i], ref x, ref y);

                if (IsRectangleEmeny(ellipse, x + 1, y - 1) && IsRectangleFree(x + 2, y - 2) && !PointWay.Exists((w) => w == (x + 2) + "," + (y - 2)) && IsPositionInLimit(x + 2, y - 2))
                {
                    mustKill = true;
                    PointWay.Add((x + 2) + "," + (y - 2));
                }

                if (IsRectangleEmeny(ellipse, x - 1, y + 1) && IsRectangleFree(x - 2, y + 2) && !PointWay.Exists((w) => w == (x - 2) + "," + (y + 2)) && IsPositionInLimit(x - 2, y + 2))
                {
                    mustKill = true;
                    PointWay.Add((x - 2) + "," + (y + 2));
                }

                if (IsRectangleEmeny(ellipse, x + 1, y + 1) && IsRectangleFree(x + 2, y + 2) && !PointWay.Exists((w) => w == (x + 2) + "," + (y + 2)) && IsPositionInLimit(x + 2, y + 2))
                {
                    mustKill = true;
                    PointWay.Add((x + 2) + "," + (y + 2));
                }
                if (IsRectangleEmeny(ellipse, x - 1, y - 1) && IsRectangleFree(x - 2, y - 2) && !PointWay.Exists((w) => w == (x - 2) + "," + (y - 2)) && IsPositionInLimit(x - 2, y - 2))
                {
                    mustKill = true;
                    PointWay.Add((x - 2) + "," + (y - 2));
                }
            }

            for (int i = 0; i < PointWay.Count; i++)
            {
                if (PointWay[i].Length == 3)
                    PointWay[i] += GetTypePoint(ellipse, PointWay[i]);
            }

        } // генерирует точки из которых будет собран путь для цепочки хода

        void GenerateWays(Ellipse ellipse)
        {
            for (int i = 0; i < PointWay.Count; i++)
            {
                if (PointWay[i].Split(',')[2] == "end")
                {
                    List<string> part2 = GeneratePointWayFromEnd(ellipse, PointWay[i]);

                    if (part2.Exists(p => p.Split(',')[2] == "toStart"))
                    {
                        for (int j = 0; j < part2.Count; j++)
                        {
                            if (part2[j].Split(',')[2] == "toStart")
                                part2[j] = part2[j].Remove(3) + ",end";
                        }

                        foreach (var point in PointWay)
                        {
                            if (point.Split(',')[2] == "start")
                            {
                                part2.Add(point);
                                part2.Reverse();
                                break;
                            }
                        }

                        Ways.Add(new List<string>(part2));
                    }
                    else
                    {
                        List<string> part1 = new List<string>(PointWay);

                        for (int h = 0; h < part1.Count; h++)
                        {
                            if (part1[h].Split(',')[2] == "end")
                            {
                                part1.Remove(part1[h]);
                                h--;
                            }
                        }

                        List<string> part3 = new List<string>();

                        if (part1.Exists((w) => w.Split(',')[2] == "fork"))
                        {
                            for (int j = 0; j < part1.Count; j++)
                            {
                                if (part1[j].Split(',')[2] == "fork")
                                {
                                    part1.RemoveRange(j + 1, part1.Count - j - 1);
                                    part3 = new List<string>();
                                    part3.AddRange(part1);
                                    part3.AddRange(part2);
                                }
                            }
                        }
                        else
                        {
                            part3 = new List<string>() { part1.First() };
                            part3.AddRange(part2);
                        }

                        Ways.Add(part3);
                    }
                }
            }
        } // генерирует пути из точек для цепочки хода

        List<string> GeneratePointWayFromEnd(Ellipse ellipse, string endPosition)
        {
            List<string> list = new List<string>();

            double x = 0, y = 0;
            GetPositionPoint(endPosition, ref x, ref y);
            list.Add(x + "," + y + ",end");

            for (int i = 0; i < list.Count; i++)
            {
                GetPositionPoint(list[i], ref x, ref y);

                if (IsRectangleEmeny(ellipse, x + 1, y - 1) && IsRectangleFree(x + 2, y - 2) && !IsIteration(list, x + 2, y - 2) && IsPositionInLimit(x + 2, y - 2))
                    list.Add((x + 2) + "," + (y - 2));

                if (IsRectangleEmeny(ellipse, x - 1, y + 1) && IsRectangleFree(x - 2, y + 2) && !IsIteration(list, x - 2, y + 2) && IsPositionInLimit(x - 2, y + 2))
                    list.Add((x - 2) + "," + (y + 2));

                if (IsRectangleEmeny(ellipse, x + 1, y + 1) && IsRectangleFree(x + 2, y + 2) && !IsIteration(list, x + 2, y + 2) && IsPositionInLimit(x + 2, y + 2))
                    list.Add((x + 2) + "," + (y + 2));

                if (IsRectangleEmeny(ellipse, x - 1, y - 1) && IsRectangleFree(x - 2, y - 2) && !IsIteration(list, x - 2, y - 2) && IsPositionInLimit(x - 2, y - 2))
                    list.Add((x - 2) + "," + (y - 2));

            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Length == 3)
                    list[i] += GetTypePoint(ellipse, list[i]);
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (!list.Exists((p) => p.Split(',')[2] == "fork") && !list.Exists((p) => p.Split(',')[2] == "way"))
                {
                    list.RemoveRange(i + 1, list.Count - i - 1);

                    string point = "";
                    point = list.Last().Remove(3) + ",toStart";
                    list.Remove(list.Last());
                    list.Add(point);
                    break;
                }

                if (list[i].Split(',')[2] == "fork")
                {
                    list.RemoveRange(i, list.Count - i);
                    break;
                }
            }

            if (list.Count > 1)
                list.Reverse();

            return list;
        }   // просчет пути с конечных точек до развилки (для цепочки хода шашки)

        bool IsIteration(List<string> list, double x, double y)
        {
            foreach (var el in list)
            {
                if (x + "," + y == el.Split(',')[0] + "," + el.Split(',')[1])
                    return true;
            }

            return false;
        } // проверяет содержиться ли такая точка в данном листе

        string GetTypePoint(Ellipse ellipse, string point)
        {
            double x = 0, y = 0, xEllipse = 0, yEllipse = 0;
            GetPositionEllipse(ellipse, ref xEllipse, ref yEllipse);
            int count = 0;

            GetPositionPoint(point, ref x, ref y);

            if (IsRectangleEmeny(ellipse, x + 1, y - 1) && IsRectangleFree(x + 2, y - 2) && IsPositionInLimit(x + 2, y - 2))
                count++;

            if (IsRectangleEmeny(ellipse, x - 1, y + 1) && IsRectangleFree(x - 2, y + 2) && IsPositionInLimit(x - 2, y + 2))
                count++;

            if (IsRectangleEmeny(ellipse, x + 1, y + 1) && IsRectangleFree(x + 2, y + 2) && IsPositionInLimit(x + 2, y + 2))
                count++;

            if (IsRectangleEmeny(ellipse, x - 1, y - 1) && IsRectangleFree(x - 2, y - 2) && IsPositionInLimit(x - 2, y - 2))
                count++;

            if (IsRectangleEmeny(ellipse, x + 1, y - 1) && x + 2 == xEllipse && y - 2 == yEllipse)
                count++;

            if (IsRectangleEmeny(ellipse, x - 1, y + 1) && x - 2 == xEllipse && y + 2 == yEllipse)
                count++;

            if (IsRectangleEmeny(ellipse, x + 1, y + 1) && x + 2 == xEllipse && y + 2 == yEllipse)
                count++;

            if (IsRectangleEmeny(ellipse, x - 1, y - 1) && x - 2 == xEllipse && y - 2 == yEllipse)
                count++;

            if (count == 1)
                return ",end";
            else if (count == 2)
                return ",way";
            else if (count > 2)
                return ",fork";
            else
                return ",null";
        } // получает тип точки (старт, развилка, конец) / нужно для просчета цепочки путей шашки

        bool IsRectangleFree(double x, double y)
        {
            double x2 = 0, y2 = 0;
            foreach (var item in ellipsesW)
            {
                GetPositionEllipse(item, ref x2, ref y2);

                if (x == x2 && y == y2)
                    return false;

            }
            //сравниваем координаты шашек с координатами потенциального хода

            foreach (var item in ellipsesB)
            {
                GetPositionEllipse(item, ref x2, ref y2);

                if (x == x2 && y == y2)
                    return false;
            }

            return true;
        }                          // является ли клетка свободной

        bool IsRectangleEmeny(Ellipse ellipse, double x, double y)
        {
            double x2 = 0, y2 = 0;

            if (ellipse.Name.Split('_')[1] == "B")
            {
                foreach (var item in ellipsesW)
                {
                    GetPositionEllipse(item, ref x2, ref y2);

                    if (x == x2 && y == y2)
                        return true;

                }
            }

            if (ellipse.Name.Split('_')[1] == "W")
            {
                foreach (var item in ellipsesB)
                {
                    GetPositionEllipse(item, ref x2, ref y2);

                    if (x == x2 && y == y2)
                        return true;
                }
            }

            return false;
        }       // есть на данной клетке враг

        bool IsPositionInLimit(double x, double y)
        {
            if (x < 8 && x >= 0 && y < 8 && y >= 0)
                return true;

            return false;
        }        // не выходят ли коориднаты за пределы доски

        void SetPosition(string str)
        {
            animIter2++;
            double xRectangle = 0, yRectangle = 0;
            Ellipse ellipse = new Ellipse();

            GetInfoMessage(str, ref xRectangle, ref yRectangle, ref ellipse);

            MainGrid.Children.Remove(ellipse);
            MainGrid.Children.Add(ellipse);

            GetPointForCourse(ellipse);
            waySelect.Clear();

            if (Ways.Count > 0)
            {
                GetWaySelect(xRectangle, yRectangle);
                GetPositionPoint(waySelect[1], ref xRectangle, ref yRectangle);
                animIter = 1;
                ellipseAnim = ellipse;
                SetPosEllipse(ellipse, xRectangle, yRectangle);
            }
            else
            {
                animIter = 1;
                ellipseAnim = ellipse;
                SetPosEllipse(ellipse, xRectangle, yRectangle);
            }
        }    // отсюда начинается движение шашки

        void GetWaySelect(double x, double y)
        {
            double x2 = 0, y2 = 0;

            for (int i = 0; i < Ways.Count; i++)
            {
                for (int j = 0; j < Ways[i].Count; j++)
                {
                    GetPositionPoint(Ways[i][j], ref x2, ref y2);

                    if (x == x2 && y == y2)
                        waySelect.AddRange(Ways[i]);

                }
            }
        } // получает выбранный путь(какую цепочку хода выбрал пользователь)

        void SetPosEllipse(Ellipse ellipse, double x, double y)
        {
            animIter2 = 1;
            nowPos = x + "," + y;

            Duration dr = new Duration(new TimeSpan(0, 0, 0, 0, 400));
            ThicknessAnimation animation = new ThicknessAnimation(new Thickness(0, -500, 0, 0), dr);
            animation.From = ellipse.Margin;
            animation.To = new Thickness(x * 50 + 2, y * 50 + 2, 0, 0);
            animation.Completed += new EventHandler(CompleteAnimation);
            ellipse.BeginAnimation(MarginProperty, animation);
        } // движение шашки

        void CompleteAnimation(object s, EventArgs e)
        {
            double x = 0, y = 0, x2 = 0, y2 = 0;

            MakeD();

            if (waySelect.Count > 0)
            {
                GetPositionPoint(waySelect[animIter - 1], ref x, ref y);
                GetPositionPoint(waySelect[animIter], ref x2, ref y2);
                DeleteEllipse(x, y, x2, y2);
            }

            if (waySelect.Count > animIter + 1)
            {
                animIter++;
                GetPositionPoint(waySelect[animIter], ref x2, ref y2);
                SetPosEllipse(ellipseAnim, x2, y2);
            }
        }   // обработчик события на конец анимации (запускаем следующую)

        void DeleteEllipse(double x1, double y1, double x2, double y2)
        {
            int length = (int)Math.Abs(x1 - x2) - 1;
            for (int j = 0; j < length; j++)
            {
                if (x1 < x2 && y1 > y2)
                {
                    x1++;
                    y1--;
                }
                else if (x1 > x2 && y1 < y2)
                {
                    x1--;
                    y1++;
                }
                else if (x1 > x2 && y1 > y2)
                {
                    x1--;
                    y1--;
                }
                else if (x1 < x2 && y1 < y2)
                {
                    x1++;
                    y1++;
                }


                if (GetEllipseCoordinate(x1, y1).Name.Length != 0)
                {
                    if (GetEllipseCoordinate(x1, y1).Name.Split('_')[1] == "W")
                        CountKillB++;
                    else
                        CountKillW++;

                    label.Text = "";
                    Write("Счет белых: " + CountKillW);
                    Write("Счет черных: " + CountKillB);

                    if (CountKillB == 12)
                    {
                        label.Text = "";
                        Write("Выиграли черные!");
                    }
                    else if (CountKillW == 12)
                    {
                        label.Text = "";
                        Write("Выиграли белые!");
                    }

                    Ellipse ellipse = GetEllipseCoordinate(x1, y1);
                    MainGrid.Children.Remove(ellipse);

                    for (int i = 0; i < ellipsesW.Count; i++)
                    {
                        if (ellipsesW[i] == ellipse)
                        {
                            ellipsesW.Remove(ellipsesW[i]);
                            break;
                        }
                    }

                    for (int i = 0; i < ellipsesB.Count; i++)
                    {
                        if (ellipsesB[i] == ellipse)
                        {
                            ellipsesB.Remove(ellipsesB[i]);
                            break;
                        }
                    }
                }
            }
        } // удаляет шашку и считает очки

        void MakeD()
        {
            double x = 0, y = 0;
            GetPositionPoint(nowPos, ref x, ref y);

            Ellipse ellipse = GetEllipseCoordinate(x, y);

            if (ellipse.Name.Length < 7 && ellipse.Name.Split('_')[1] == "W" && y == 7 && ellipse.StrokeThickness != 2)
            {
                ellipse.Stroke = new LinearGradientBrush(Color.FromRgb(230, 230, 230), Color.FromRgb(170, 170, 170), new Point(0, 0.5), new Point(0.5, 4));
                ellipse.StrokeThickness = 2;
                ellipse.Fill = new LinearGradientBrush(Color.FromRgb(150, 150, 150), Color.FromRgb(190, 190, 190), new Point(0, 0.5), new Point(0.5, 1));
            }
            else if (ellipse.Name.Length < 7 && ellipse.Name.Split('_')[1] == "B" && y == 0 && ellipse.StrokeThickness != 2)
            {
                ellipse.Stroke = new LinearGradientBrush(Color.FromRgb(80, 80, 80), Color.FromRgb(0, 0, 0), new Point(0, 0.5), new Point(0.5, 4));
                ellipse.StrokeThickness = 2;
                ellipse.Fill = new LinearGradientBrush(Color.FromRgb(0, 0, 0), Color.FromRgb(60, 60, 60), new Point(0, 0.5), new Point(0.5, 1));

            }

        } // создает дамку из простой шашки

        //// network

        void GetInfoMessage(string message, ref double xRect, ref double yRect, ref Ellipse ellipse)
        {
            xRect = double.Parse(message.Split(';')[1].Split(',')[0]);
            yRect = double.Parse(message.Split(';')[1].Split(',')[1]);

            foreach (var el in ellipsesW)
            {
                if (message.Split(';')[0] == el.Name)
                    ellipse = el;
            }

            foreach (var el in ellipsesB)
            {
                if (message.Split(';')[0] == el.Name)
                    ellipse = el;
            }
        } // получаем информацию о ходе (присланную через сеть) / координаты на которые был произведен ход и шашку которой походили

        void SendMessage(string msg)
        {
            if (ServerBool)
            {
                try
                {
                    if ( handler != null)
                    {
                        handler.Send(Encoding.UTF8.GetBytes(msg));
                        CourseWhite = false;
                    }
                }
                catch (Exception ex)
                {
                    Write(ex.Message);
                }
            }
            else
            {
                try
                {
                    sender.Send(Encoding.UTF8.GetBytes(msg));
                    CourseWhite = true;
                }
                catch (Exception ex)
                {
                    Write(ex.Message);
                }
            }
        }  // отправляет сообщение противнику

        void Client(string message)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            //IPAddress ipAddr = IPAddress.Parse("192.168.1.2");
            IPAddress ipAddr = IPAddress.Parse(ip);

            Write("Соединение с " + ip);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 33377);

            sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(ipEndPoint);     //подключение

            int bytesSent = sender.Send(Encoding.UTF8.GetBytes(message));

            // получает сообщение от сервера
            int bytesRec = sender.Receive(bytes);                           // получает сообщение от сервера
            string serverMsg = Encoding.UTF8.GetString(bytes, 0, bytesRec); // получает сообщение от сервера 
            side.IsEnabled = false;


            if (serverMsg != "connectW" && serverMsg != "connectB")
            {
                MessageBox.Show(serverMsg);
            }
            else if (serverMsg == "connectW")
            {

                _white = false;
                Write("Соединение установленно.\nВы играете черными.");
                Thread clientListen = new Thread(ClientListenConnect);
                clientListen.IsBackground = true;
                clientListen.Start();
            }
            else if (serverMsg == "connectB")
            {

                _white = true;
                CourseWhite = false;
                Write("Соединение установленно.\nВы играете белыми.");
                Thread clientListen = new Thread(ClientListenConnect);
                clientListen.IsBackground = true;
                clientListen.Start();
            }

            // Write(Encoding.UTF8.GetString(bytes, 0, bytesRec));
            //sender.Shutdown(SocketShutdown.Both);
            //sender.Close();

        }   // созадем все необходимое для работы клиента/ высылаем ему сообщение о том, что мы присоединились

        void ClientListenConnect()
        {
            while (work)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int countBytes = sender.Receive(buffer);
                    string msg = Encoding.UTF8.GetString(buffer, 0, countBytes);

                    Action act = () =>
                    {
                        SetPosition(msg);

                        //SwitchCourse(msg);
                        CourseWhite = false;
                        ServerBool = false;
                    };
                    Dispatcher.Invoke(act);

                }

                catch (Exception ex)
                {
                    CourseWhite = !CourseWhite;
                    work = false;
                    Write(ex.Message);
                }
            }
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

        } // принимает полученые сообщения от сервера

        void Server()
        {
            IPAddress ipAddr;
            try
            {
                ipAddr = IPAddress.Parse(ip);
                //ipAddr = IPAddress.Parse("0.0.0.0");

            }
            catch (Exception ex)
            {
                Write(ex.Message);
                return;
            }

            Write("Ожидаем " + ip);

            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 33377);

            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {

                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                handler = sListener.Accept();
               
                ServerBool = true;
                _white = white;
                if (_white)
                {
                    CourseWhite = true;
                }
                else
                {
                    CourseWhite = false;
                }
                while (work)
                {
                    try
                    {
                        byte[] bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);      //замирает, принимает сообщение 
                        string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                      
                        if (data == "connect")
                        {

                            
                            if (_white)
                            {
                                Action act = () => { side.IsEnabled = false; Write("Соединение установленно.\nВы играете белыми."); };
                                Dispatcher.Invoke(act);
                            }
                            else
                            {
                                Action act = () => { side.IsEnabled = false; Write("Соединение установленно.\nВы играете черными."); };
                                Dispatcher.Invoke(act);
                            }

                            if (_white)
                            {
                                handler.Send(Encoding.UTF8.GetBytes("connectW"));
                            }
                            else
                                handler.Send(Encoding.UTF8.GetBytes("connectB"));
                   
                            
                        }
                        
                        else
                        {
                            Action act = () => {
                                CourseWhite = true;
                                SetPosition(data);
                                //SwitchCourse(data);
                            };
                            Dispatcher.Invoke(act);
                        }

                    }
                    catch (Exception ex)
                    {
                        CourseWhite = !CourseWhite;
                        work = false;
                        Write(ex.Message);
                  
                    }

                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception ex)
            {

                
                Action act = () => { btnC.IsEnabled = true; };
                ServerBool = false;
                CourseWhite = true;
                Dispatcher.Invoke(act);
                Write(ex.Message);
            }
        }              // создаем все необходимое для работы сервера/ тут же принимает полученные сообщения от клиента

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {       
                
                work = false;
        }
    }
}
