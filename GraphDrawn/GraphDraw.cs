using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GraphDrawn
{
    /// <summary>
    /// Структура Вершина графу
    /// </summary>
    struct Vertex
    {
        /// <summary>
        /// Коортината точки по осі абсцис
        /// </summary>
        int x;
        /// <summary>
        /// Координати точки по осі ординат
        /// </summary>
        int y;
        /// <summary>
        /// Номер вершини
        /// </summary>
        int number;
        /// <summary>
        /// Гетер для доступу до х
        /// </summary>
        public int X { get { return x; } }
        /// <summary>
        /// Гетер для доступу до y
        /// </summary>
        public int Y { get { return y; } }
        /// <summary>
        /// Гетер для доступу до номера вершини
        /// </summary>
        public int Number { get { return number; } }
        /// <summary>
        /// Конструктор ініціалізації
        /// </summary>
        /// <param name="x">Координата х точки</param>
        /// <param name="y">Координати у точки</param>
        /// <param name="number">Номер вершини</param>
        public Vertex(int x, int y, int number)
        {
            this.x = x;
            this.y = y;
            this.number = number;
        }
    }
    /// <summary>
    /// Структрура Реберо графу
    /// </summary>
    struct Node
    {
        /// <summary>
        /// Початок ребра
        /// </summary>
        Vertex first;
        /// <summary>
        /// Кінець ребра
        /// </summary>
        Vertex second;
        /// <summary>
        /// Вага ребра
        /// </summary>
        int weight;

        /// <summary>
        /// Гетер доступу до початку ребра
        /// </summary>
        public Vertex First { get { return first; } }

        /// <summary>
        /// Гетер доступу до кінця ребра
        /// </summary>
        public Vertex Second { get { return second; } }

        /// <summary>
        /// Гетер доступу до ваги ребра
        /// </summary>
        public int Weight { get { return weight; }  }

        /// <summary>
        /// Коструктор ініціалізації
        /// </summary>
        /// <param name="first">Початок ребра</param>
        /// <param name="second">Кінець ребра</param>
        /// <param name="weight">Вага ребра</param>
        public Node(Vertex first,Vertex second, int weight)
        {
            this.first = first;
            this.second = second;
            this.weight = weight;
        }
    }

    
    /// <summary>
    /// Клас для відображення елеменів графу
    /// </summary>
    class GraphDraw
    {
        /// <summary>
        /// Радіус кола вершини
        /// </summary>
        int Radius = 12;

        /// <summary>
        /// Номер потчної вершини
        /// </summary>
        int numberOfVertex = 0;
        /// <summary>
        /// Прямокутник для відображення вершини
        /// </summary>
        private Rectangle DotDrawed;
        /// <summary>
        /// Об'ект який містить інформацію про колір і тд намальованого елементу графу
        /// </summary>
        private Pen standartPen;
        /// <summary>
        /// Як відобразити лінію
        /// </summary>
        private Pen LinePen;
        /// <summary>
        /// Як відобразити червоний елемент
        /// </summary>
        private Pen RedPen;
        /// <summary>
        /// Зображення на якому відображається граф
        /// </summary>
        private Image image;
        /// <summary>
        /// Елемент класу який виконує всі побудови
        /// </summary>
        Graphics draw;

        /// <summary>
        /// Список всіх вершин
        /// </summary>
        List<Vertex> AllVertex;
        /// <summary>
        /// Список всіх ребер
        /// </summary>
        public List<Node> Nodes;

        /// <summary>
        /// Гетер для доступу до зображення
        /// </summary>
        public Image Image { get { return image; } }
        
        /// <summary>
        /// Похибка між курсором та точкою вершини
        /// </summary>
        int scatter = 4;

        /// <summary>
        /// Конструктор ініціалізації
        /// </summary>
        /// <param name="width">Ширина зображення</param>
        /// <param name="height">Висота зображення</param>
        public GraphDraw(int width, int height)
        {
            //Nodes = new List<Vertex>[2];
            //Nodes[0] = new List<Vertex>();
            //Nodes[1] = new List<Vertex>();
            //Nodes[2] = new List<int>();
            //Nodes = new Node();
            AllVertex = new List<Vertex>();
            Nodes=new List<Node>();

            //XVertexLocatedNearCursorXPosition = new List<Vertex>();
            //YVertexLocatedNearCursorYPosition = new List<Vertex>();

            //this.placeToDraw = box;
           
            standartPen = new Pen(Color.Black, 2);
            RedPen = new Pen(Color.Red, 3);
            LinePen = new Pen(Color.Black, 2);

            //image = new Bitmap(box.Width, box.Height);

            image = new Bitmap(width, height);
            draw = Graphics.FromImage(image);
            //this.placeToDraw.Image = image;
        }

        /// <summary>
        /// Побудова вершини
        /// </summary>
        /// <param name="x">Координата х вершнини</param>
        /// <param name="y">Координати у вершнии</param>
        /// <returns>Зображення з побудованою вершиною</returns>
        public Image DrawDot(int x, int y)
        {
            AllVertex.Add(new Vertex(x, y, ++numberOfVertex));
            return drawOnlyDot(LastVertex());
        }

        /// <summary>
        /// Кількість вершини графа
        /// </summary>
        /// <returns>Кількість вершин графа</returns>
        public int CountOfVertex()
        {
            return AllVertex.Count;
        }
        
        /// <summary>
        /// Кількість ребер графа
        /// </summary>
        /// <returns>Кількість ребер графа</returns>
        public int CountOfNodes()
        {
            return Nodes.Count;
        }

        /// <summary>
        /// Побудова вершнии, якщо будувати не по черзі
        /// </summary>
        /// <param name="x">Координата х вершини</param>
        /// <param name="y">Координата у вершини</param>
        /// <param name="number">Номер вершини</param>
        /// <returns>Зображення з побудованою вершиною</returns>
        public Image DrawDot(int x, int y, int number)
        {
            bool similar = false;
            Vertex current = FindVertex(number);
            if(current.Number==number)
            {
                similar = true;
            }
            if (!similar)
            {
                ++numberOfVertex;
                AllVertex.Add(new Vertex(x, y, number));
                return drawOnlyDot(LastVertex());
            }
            else
            {
                return image;
            }
        }

        /// <summary>
        /// Метод відображення вершини
        /// </summary>
        /// <param name="v">Вершина</param>
        /// <returns>Зображення з побудованю вершиною</returns>
        private Image drawOnlyDot(Vertex v)
        {
            DotDrawed.X = v.X - Radius;
            DotDrawed.Y = v.Y - Radius;

            DotDrawed.Width = Radius*2;
            DotDrawed.Height = Radius*2;

            draw.FillEllipse(Brushes.White, DotDrawed);
            draw.DrawEllipse(standartPen, DotDrawed);

            
            PointF point = new PointF(v.X - Radius / 2 - (Math.Sign(v.Number / 10)) * Radius / 4 - (Math.Sign(v.Number / 100)) * Radius / 4, v.Y - Radius / 2);
            draw.DrawString(v.Number.ToString(), new Font("Arial", 10), Brushes.Blue, point);


            return image;
        }

        /// <summary>
        /// Метод відображення вершини з червоним колом
        /// </summary>
        /// <param name="v">Вершина</param>
        /// <returns>Зображення з побудованою вершиною</returns>
        public Image drawRedDot(Vertex v)
        {
            DotDrawed.X = v.X - Radius;
            DotDrawed.Y = v.Y - Radius;

            DotDrawed.Width = Radius*2;
            DotDrawed.Height = Radius*2;

            draw.DrawEllipse(RedPen, DotDrawed);


            return image;
        }


        /// <summary>
        /// Метод визначення вершини по заданим координатам
        /// </summary>
        /// <param name="x">Координати х точки</param>
        /// <param name="y">Координата у точки</param>
        /// <returns>Повертає знайдену вершину, або вершину з координатами -10;-10 та номером -10, якщо не існує вершини в даній точці</returns>
        public Vertex FindVertex(int x, int y)
        {
            for (int i = 0; i < AllVertex.Count; i++)
            {
                if (AllVertex[i].Y >= y - Radius && AllVertex[i].Y <= y + Radius)
                {
                    if (AllVertex[i].X >= x - Radius && AllVertex[i].X <= x + Radius)
                    {
                        return AllVertex[i];
                    }
                }
            }
            return new Vertex(-10, -10, -10);

        }

        /// <summary>
        /// Знаходження вершини по її номеру
        /// </summary>
        /// <param name="number">Номер вершини</param>
        /// <returns>Повертає знайдену вершину, або вершину з координатами -10;-10 та номером -10, якщо не існує вершини з даним номером</returns>
        public Vertex FindVertex(int number)
        {
            for (int i = 0; i < AllVertex.Count; i++)
            {
                if (AllVertex[i].Number == number)
                {
                    return AllVertex[i];
                }
            }
            return new Vertex(-10, -10, -10);

        }

        /// <summary>
        /// Зміна координат точки
        /// </summary>
        /// <param name="x">Нова координата х</param>
        /// <param name="y">Нова координата у</param>
        /// <param name="number">Номер вршини</param>
        /// <param name="isOrtogonal">Чи ортогональний граф</param>
        /// <returns>Зображення з вершиною на новому місці</returns>
        public Image ChangeDotPosition(int x, int y, int number,bool isOrtogonal)
        {
            bool isConsist = false;
            int index = -1;
            for(int i=0;i<AllVertex.Count;i++)
            {
                if(AllVertex[i].Number==number)
                {
                    index = i;
                    isConsist = true;
                    break;
                }
            }
            if(isConsist)
            {
                AllVertex[index] = new Vertex(x, y, number);
                
                for(int i=0;i<Nodes.Count;i++)
                {
                    if(Nodes[i].First.Number==number)
                    {
                        Nodes[i] = new Node(AllVertex[index], Nodes[i].Second, Nodes[i].Weight);
                    }
                    else if(Nodes[i].Second.Number==number)
                    {
                        Nodes[i] = new Node(Nodes[i].First, AllVertex[index], Nodes[i].Weight);
                    }
                }
            }
            return DrawAllFromScrath(isOrtogonal, image.Width, image.Height);
        }

        /// <summary>
        /// Побудова ребра графу
        /// </summary>
        /// <param name="first">Початок ребра</param>
        /// <param name="second">Кінець ребра</param>
        /// <param name="weight">Вага ребра</param>
        /// <param name="isOrtagonal">Чи ортогональний граф</param>
        /// <returns>Зображення з побудованим ребром</returns>
        public Image LineDraw(Vertex first, Vertex second,int weight, bool isOrtagonal)
        {
            if(first.X>0 && second.X>0)
            {
                if(!first.Equals(second))
                {
                    bool foundSimilar = false;
                    for (int i = 0; i < Nodes.Count;i++ )
                    {
                        if(Nodes[i].First.Number==first.Number)
                        {
                            if(Nodes[i].Second.Number==second.Number)
                            {
                                foundSimilar = true;
                            }
                        }
                    }
                    if (!foundSimilar)
                    {
                        Nodes.Add(new Node(first,second,weight));
                        //Nodes.Add(second);

                        //return drawArc(first, second);
                        if(weight!=0)
                        {
                            drawWeight(Nodes[Nodes.Count - 1], isOrtagonal);
                        }
                        drawOnlyLine(first, second, isOrtagonal, Color.Black);
                        
                        return image;
                    }
                }
            }
            return image;
        }

        /// <summary>
        /// Відображення ваги ребра на зображені
        /// </summary>
        /// <param name="node">Ребра</param>
        /// <param name="isOrtogonal">Чи ортогональний граф</param>
        /// <returns>Зображення з вагою ребра</returns>
        public Image drawWeight(Node node, bool isOrtogonal)
        {
            Brush brushB = Brushes.Blue;
            Brush brushP = Brushes.Orange;

            Brush printBrush = brushB;
            int shiftValue = 5;

            Vertex first=node.First;
            Vertex second=node.Second;


            int indexCurrent = IsNodeConsist(first.Number, second.Number);
            int indexOrtogonal = IsNodeConsist(second.Number, first.Number);

            PointF point = new PointF((node.First.X + node.Second.X) / 2, (node.First.Y + node.Second.Y) / 2);


            if(first.X!=second.X)
            {
                
                double k = (second.Y - first.Y) / (second.X - first.X);

                if(k>0)
                {

                    point = new PointF(((node.First.X + node.Second.X) / 2) + shiftValue, ((node.First.Y + node.Second.Y) / 2) - shiftValue);
                    if (isOrtogonal)
                    {
                        if(indexOrtogonal!=-1 && indexCurrent>indexOrtogonal)
                        {
                            point = new PointF(((node.First.X + node.Second.X) / 2) - 3*shiftValue, ((node.First.Y + node.Second.Y) / 2) + 2*shiftValue);
                            printBrush = brushP;
                        }
                    }
                  

                }
                else if(k<0)
                {
                    point = new PointF(((node.First.X + node.Second.X) / 2), ((node.First.Y + node.Second.Y) / 2));
                    if (isOrtogonal)
                    {
                        if (indexOrtogonal!=-1 && indexCurrent > indexOrtogonal)
                        {
                            point = new PointF(((node.First.X + node.Second.X) / 2) -2*shiftValue, ((node.First.Y + node.Second.Y) / 2) - shiftValue*3);
                           printBrush = brushP;
                        }
                    }
                }
                else
                {

                    point = new PointF(((node.First.X + node.Second.X) / 2), ((node.First.Y + node.Second.Y) / 2) + shiftValue);
                    if (isOrtogonal)
                    {
                        if (indexOrtogonal!=-1 && indexCurrent > indexOrtogonal)
                        {
                            point = new PointF(((node.First.X + node.Second.X) / 2), ((node.First.Y + node.Second.Y) / 2) - shiftValue * 3);
                            printBrush = brushP;
                        }
                    }
                }

            }
            else
            {
                point = new PointF(((node.First.X + node.Second.X) / 2) + shiftValue, ((node.First.Y + node.Second.Y) / 2));
                if (isOrtogonal)
                {
                    if (indexOrtogonal != -1 && indexCurrent > indexOrtogonal)
                    {
                        point = new PointF(((node.First.X + node.Second.X) / 2) - shiftValue * 3, ((node.First.Y + node.Second.Y) / 2));
                        printBrush = brushP;
                    }
                }
            }

            if(node.Weight!=0)
            {
                draw.DrawString(node.Weight.ToString(), new Font("Arial", 10), printBrush, point);
            }
            return image;
        }

        /// <summary>
        /// Метод відображення лінії ребра
        /// </summary>
        /// <param name="first">Початок ребра</param>
        /// <param name="second">Кінець ребра</param>
        /// <param name="isOrtagonal">Чи ортагональний граф</param>
        /// <param name="color">Колір лінії</param>
        /// <returns>Зображення з лінією</returns>
        private Image drawOnlyLine(Vertex first, Vertex second, bool isOrtagonal, Color color)
        {
            LinePen.Color = color;
            if(color.Equals(Color.Red))
            {
                LinePen.Width = 3;
            }
            else
            {
                LinePen.Width = 2;
            }
            if(isOrtagonal)
            {
                System.Drawing.Drawing2D.CustomLineCap custom = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 6);
                LinePen.EndCap = System.Drawing.Drawing2D.LineCap.Custom;
                LinePen.CustomEndCap = custom;
            }
            else
            {
                LinePen.StartCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                LinePen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;

            }
            Vertex onCircule = CalculateDotPosition(second, first);
            
            
            draw.DrawLine(LinePen,first.X,first.Y, onCircule.X, onCircule.Y);
            drawOnlyDot(first);
            drawOnlyDot(second);
            return image;
        }
        
        /// <summary>
        /// Визначення точки до якох потрібно малювати лінію
        /// </summary>
        /// <param name="first">Початок лінії</param>
        /// <param name="second">Кінець лінії</param>
        /// <returns>Точка до якої потрібно малювати лінію</returns>
        private Vertex CalculateDotPosition(Vertex first, Vertex second)
        {
            double x1 = first.X;
            double x2 = second.X;
            double y1 = first.Y;
            double y2 = second.Y;

            double S=Math.Sqrt(Math.Pow(x1-x2,2)+Math.Pow(y1-y2,2));


            double a = (Radius * Radius - Math.Pow(S - Radius, 2) + x2 * x2 + y2 * y2 - x1 * x1 - y1 * y1) / (2 * x2 - 2 * x1);
            double b = (y2 - y1) / (x2 - x1);



            double D=Math.Pow(b*x1-y1-b*a,2)-(a*a-2*x1*a-Radius*Radius+x1*x1+y1*y1)*(b*b+1);
            double y31;
            double y32;
            if((int)D==0)
            {
                y31 = (-(b * x1 - y1 - b * a)) / (b * b + 1);
                y32 = (-(b * x1 - y1 - b * a)) / (b * b + 1);
            }
            else
            {
               y31  = (-(b * x1 - y1 - b * a) + Math.Sqrt(D)) / (b * b + 1);
               y32 = (-(b * x1 - y1 - b * a) - Math.Sqrt(D)) / (b * b + 1);
            }
            double x31 = a - b * y31;
            double x32 = a - b * y32;
            double S2;
            double S3;
            try
            {
                checked
                {
                    S2 = Math.Sqrt(Math.Pow(x31 - x2, 2) + Math.Pow(y31 - y2, 2));
                    S3 = Math.Sqrt(Math.Pow(x32 - x2, 2) + Math.Pow(y32 - y2, 2));
                    x31 = (int)x31;
                    x32 = (int)x32;
                    y31 = (int)y31;
                    y32 = (int)y32;
                }
            }
            catch(System.OverflowException e)
            {
                return second;
            }
            return S2 > S3 ? new Vertex((int)x32, (int)y32, -1) : new Vertex((int)x31, (int)y31, -1);

        }


        //private Image drawArc(Vertex first,Vertex second)
        //{
        //    //зміна, яка задає наскільки заокруглена буде дуга
        //    int rang=2;

        //    double distance=Math.Sqrt(Math.Pow(first.X-second.X,2)+Math.Pow(first.Y-second.Y,2));
        //    double h = distance / rang;
        //    //double radius = Math.Pow(((distance * distance / 2) + h * h),1.5) / (distance * h * h);

        //    double radius = h * 1.5;

        //    int deltaX = Math.Abs(first.X - second.X);
        //    int deltaY = Math.Abs(first.Y - second.Y);

        //    //int Width = 1.5 * (int)distance;
        //    //int Height = 2 * (int)distance;

        //    //DotDrawed.Width = Width;
        //    //DotDrawed.Height = Height;           


        //    double startAngle = 0, endAngle = 90;

        //    if(first.X>second.X)
        //    {
        //        if(first.Y>second.Y)
        //        {
        //            if(deltaX>deltaY)
        //            {
        //                Vertex center = FindRadius(first, second, 1, 0);
        //                DotDrawed.Height = DotDrawed.Width = 2*(int)DetermDistanceBetween2Dots(center, first);
        //                DotDrawed.X = first.X - DotDrawed.Width;
        //                DotDrawed.Y = first.Y - DotDrawed.Width/2;

        //                startAngle = 0;
        //                double delta = (double)deltaY / (double)deltaX;
        //                double atan = Math.Atan(delta);
        //                double angle = atan * (180.0 / Math.PI);

        //                endAngle = -180 + 2*angle;
        //                ////////////////////
        //                //draw.DrawEllipse(LinePen, DotDrawed);
        //                ////////////////
        //            }
        //            else
        //            {
        //                Vertex center = FindRadius(first, second, 0, -1);
        //                DotDrawed.Height = DotDrawed.Width = 2*(int)DetermDistanceBetween2Dots(center, first);

        //                //
        //                DotDrawed.X = second.X - DotDrawed.Width/2;
        //                DotDrawed.Y = second.Y;

        //                endAngle = -2 * Math.Atan(deltaY / deltaX) * (180 / Math.PI);
        //                startAngle = -endAngle - 90;
        //                ////////////////////
        //                //draw.DrawEllipse(LinePen, DotDrawed);
        //                ////////////////
        //            }

                    
        //        }
        //        else if (first.Y < second.Y)
        //        {
        //            DotDrawed.X = first.X - DotDrawed.Width;
        //            DotDrawed.Y = first.Y - DotDrawed.Height/2;
        //            startAngle = 0;

        //        }
        //        else
        //        {
        //            DotDrawed.X = first.X - DotDrawed.Width;
        //            DotDrawed.Y = first.Y - DotDrawed.Height / 2;
        //        }
        //    }
        //    else if(first.X<second.X)
        //    {
        //        if (first.Y > second.Y)
        //        {
        //            DotDrawed.X = first.X;
        //            DotDrawed.Y = first.Y - (int)radius;

        //        }
        //        else if (first.Y < second.Y) 
        //        {
        //            DotDrawed.X = second.X - 2 * (int)radius;
        //            DotDrawed.Y = second.Y - (int)radius;
        //        }
        //        else
        //        {
        //            DotDrawed.X = second.X - 2 * (int)radius;
        //            DotDrawed.Y = second.Y - (int)radius;
        //        }
        //    }
        //    else
        //    {
        //        if (first.Y > second.Y)
        //        {
        //            DotDrawed.X = first.X - (int)radius;
        //            DotDrawed.Y = first.Y;

        //        }
        //        else if (first.Y < second.Y) 
        //        {
        //            DotDrawed.X = first.X - (int)radius;
        //            DotDrawed.Y = second.Y;
        //        }
        //    }

        //    //draw.DrawRectangle(LinePen, DotDrawed);

        //    /////////////////////!!!!!!!!!!!!!!!!!!!!!!!!!
        //    System.Drawing.Drawing2D.CustomLineCap custom = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 5);
        //    LinePen.EndCap = System.Drawing.Drawing2D.LineCap.Custom;
        //    LinePen.CustomEndCap = custom;
        //    //LinePen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
        //    ////////////////////////////!!!!!!!!!!!!!!!!


        //    draw.DrawArc(LinePen, DotDrawed, (float)startAngle, (float)endAngle);
        //    return image;
        //}

        /// <summary>
        /// Визначення останьої доданої вершини
        /// </summary>
        /// <returns>Остання вершина</returns>
        public Vertex LastVertex()
        {
            return AllVertex[AllVertex.Count - 1];
        }

        /// <summary>
        /// Зміна зображення на якому малюється граф
        /// </summary>
        /// <param name="image">Нове зображення</param>
        public void SetDrawingImage(Image image)
        {
            this.image = image;
            draw = Graphics.FromImage(image);
        }

        
        //private Vertex FindRadius(Vertex first, Vertex second, int dX, int dY)
        //{
        //    double e = 1.0;
        //    int x = second.X;
        //    int y = first.Y;
        //    double radius, radius2;
        //    Vertex found;
        //    do
        //    {
        //        found = new Vertex(x, y, -10);
        //        radius = DetermDistanceBetween2Dots(second, found);
        //        radius2 = DetermDistanceBetween2Dots(found, first);
        //        x += dX;
        //        y += dY;
        //    } while (Math.Abs(radius2 - radius) > e);
        //    return found;
        //}

        //private double DetermDistanceBetween2Dots(Vertex first, Vertex second)
        //{
        //    return Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
        //}

        /// <summary>
        /// Побудова графу з чистого листа
        /// </summary>
        /// <param name="isOrtogonal">Чи ортогональний граф</param>
        /// <param name="width">Ширина зображення</param>
        /// <param name="height">Висота зображення</param>
        /// <returns>Зображення з побудованим графом</returns>
        public Image DrawAllFromScrath(bool isOrtogonal, int width, int height)
        {
            SetDrawingImage(new Bitmap(width, height));
            foreach(Vertex v in AllVertex)
            {
                drawOnlyDot(v);
            }
            
            for (int i = 0; i < Nodes.Count;i++ )
            {
                drawOnlyLine(Nodes[i].First, Nodes[i].Second, isOrtogonal, Color.Black);
                drawWeight(Nodes[i], isOrtogonal);
            }
                return image;
        }

        /// <summary>
        /// Визначення індексу ребра по заданим координатам
        /// </summary>
        /// <param name="x">Координата х</param>
        /// <param name="y">Координата у</param>
        /// <returns>Індекс ребра чи -1 якщо на заданих координатах ребро не існує</returns>
        public int FindNodes(int x, int y)
        {

            for (int i = 0; i < Nodes.Count;i++ )
            {
                Vertex first=Nodes[i].First;
                Vertex second=Nodes[i].Second;
                int minX = Math.Min(first.X, second.X);
                int maxX = Math.Max(first.X, second.X);
                int minY = Math.Min(first.Y, second.Y);
                int maxY = Math.Max(first.Y, second.Y);

                double temp = (x - first.X) * (second.Y - first.Y) / (second.X - first.X) + first.Y;
                if(Math.Abs(temp-y)<=scatter)
                {
                    if(x>=minX&&x<=maxX&&y>=minY&&y<=maxY)
                    {
                        return i;
                    }
                }


            }

            return -1;
        }

        /// <summary>
        /// Видалення ребра чи вершини
        /// </summary>
        /// <param name="x">Координата х точки видалення</param>
        /// <param name="y">Координата у точки видалення</param>
        /// <param name="isOrtogonal">Чи ортогональний граф</param>
        /// <returns>Зображення з видаленою вершиною чи ребром</returns>
        public Image DeleteVertexOrNode(int x,int y, bool isOrtogonal)
        {
            Vertex find=FindVertex(x,y);
            int numberOfFindNode=FindNodes(x,y);
            bool findSmth = false;
            if(find.X>0)
            {
                AllVertex.Remove(find);
                for(int i=0;i<Nodes.Count;i++)
                {
                    if(Nodes[i].First.Number==find.Number || Nodes[i].Second.Number==find.Number)
                    {
                        Nodes.RemoveAt(i);
                        i--;
                    }
                }
                if(find.Number==numberOfVertex)
                {
                    numberOfVertex--;
                }

                findSmth = true;
            }
            else if(numberOfFindNode>=0)
            {
                Nodes.RemoveAt(numberOfFindNode);
                findSmth = true;
            }

            if (findSmth)
            {
                return DrawAllFromScrath(isOrtogonal,this.image.Width,this.image.Height);
            }
            else
            {
                return image;
            }


        }

        /// <summary>
        /// Відображення червоної лінії
        /// </summary>
        /// <param name="numberOfNode">Номер ребра</param>
        /// <param name="isOrtogonal">чи ортогональний граф</param>
        /// <returns>Зображення з червоною лінією</returns>
        public Image DrawRedLine(int numberOfNode,bool isOrtogonal)
        {
            Vertex first = Nodes[numberOfNode].First;
            Vertex second = Nodes[numberOfNode].Second;
            return drawOnlyLine(first, second, isOrtogonal, Color.Red);
        }

        /// <summary>
        /// Видалення всіх вершин та ребер
        /// </summary>
        /// <returns>Зображення без вершин та ребер</returns>
        public Image ClearAll()
        {
            SetDrawingImage(new Bitmap(image.Width, image.Height));

            AllVertex.Clear();
            Nodes.Clear();
            numberOfVertex = 0;
            return image;
        }

        /// <summary>
        /// Визанчення чи ребро існує по номерм вершин кінців
        /// </summary>
        /// <param name="first">Номер виршини початку ребра</param>
        /// <param name="second">Номер вершини кінця ребра</param>
        /// <returns>Індек ребра чи -1 якщо ребро не існує</returns>
        public int IsNodeConsist(int first, int second)
        {
            for(int i=0;i<Nodes.Count;i++)
            {
                if(Nodes[i].First.Number==first)
                {
                    if (Nodes[i].Second.Number==second)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Визначення ортогонального ребра до ребра з вершинами номера яких задані 
        /// </summary>
        /// <param name="first">Номер вершини початку ребра</param>
        /// <param name="second">Номер вершини кінця ребра</param>
        /// <returns>Індекс ортогонального ребра або -1 якщо ортогональне ребро не існує</returns>
        public int FindOrtogonalNode(int first,int second)
        {
            if(IsNodeConsist(first,second)!=-1)
            {
                return IsNodeConsist(second, first);
            }
            return -1;
        }

        /// <summary>
        /// Визанчення вершини з максимальним номером
        /// </summary>
        /// <returns>Номер вершини</returns>
        public int MaxVertexValue()
        {
            int max = -1;
            for(int i=0;i<AllVertex.Count;i++)
            {
                if(AllVertex[i].Number>max)
                {
                    max = AllVertex[i].Number;
                }
            }
            return max;
        }
    }
}
