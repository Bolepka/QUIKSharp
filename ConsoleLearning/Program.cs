using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuikSharp;
using QuikSharp.DataStructures;

namespace ConsoleLearning
{
    class Program
    {
        private static void OnNewCandleDo(Candle candle)
        {

            Console.WriteLine("Получена новая свечка: Open = {0}   Close = {1}  Low = {2}  High = {3}", candle.Open, candle.Close, candle.Low, candle.High);
        }

        static void Main(string[] args)
        {
            //Выводим в лог приветствие
            Console.WriteLine(" - Вы готовы дети?");
            Thread.Sleep(1500);
            Console.WriteLine(" - Дааа, капитаааан!");
            Thread.Sleep(1500);
            Console.WriteLine(" - Я не слышуу!");
            Thread.Sleep(1500);
            Console.WriteLine(" - ТАК ТОЧНО, КАПИТААААН!");
            Thread.Sleep(1500);


            for (int i = 1; i < 10; i++) {

                Thread.Sleep(100);
                Console.Write(" - У");
            }

            Console.WriteLine();


            //ИТАК, ПОЕХАЛИ 

            // Объявляем колоссальную переменую _quik. Она будет нашим главным связующим узлом с терминалом
            Quik _quik = null;

            bool isServerConnected = false;

            // Создаём экземпляр и записываем его в переменную _quik
            try
            {
                Console.WriteLine("Подключаемся к терминалу Quik..." + Environment.NewLine);

                 _quik = new Quik(Quik.DefaultPort, new InMemoryStorage());    // инициализируем объект Quik с использованием локального расположения терминала (по умолчанию)
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+ ". Ошибка инициализации объекта Quik..." + Environment.NewLine);
            }
            if (_quik != null)
            {
                Console.WriteLine("Экземпляр Quik создан." + Environment.NewLine);
                try
                {
                    Console.WriteLine("Получаем статус соединения с сервером...." + Environment.NewLine);
                    isServerConnected = _quik.Service.IsConnected().Result;

                    if (isServerConnected)
                    {
                        Console.WriteLine("Соединение с сервером установлено." + Environment.NewLine);
                    }
                    else
                    {
                        Console.WriteLine("Соединение с сервером НЕ установлено." + Environment.NewLine);
                    }
                }
                catch
                {
                    Console.WriteLine("Неудачная попытка получить статус соединения с сервером." + Environment.NewLine);
                }
            }



            Console.WriteLine("Подписываемся на получение исторических данных..." + Environment.NewLine);
            _quik.Candles.Subscribe("SPBFUT", "SiU0", CandleInterval.M1).Wait();


            var isSubscribedToolCandles = _quik.Candles.IsSubscribed("SPBFUT", "SiU0", CandleInterval.M1).Result;


            if (isSubscribedToolCandles)
            {
                var toolCandles = _quik.Candles.GetAllCandles("SPBFUT", "SiU0", CandleInterval.M1).Result;


                Console.WriteLine( "Подтянули торт и {0} свечек!", toolCandles.Count);
                Console.WriteLine("Последняя свечка:  Open = {0}   Close = {1}  Low = {2}  High = {3} ", toolCandles.Last().Open, toolCandles.Last().Close, toolCandles.Last().Low, toolCandles.Last().High);

                _quik.Candles.NewCandle += OnNewCandleDo;

            }

            Console.Read();
        }
    }
}
