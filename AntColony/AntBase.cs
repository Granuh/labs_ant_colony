using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AntColony
{
    // Класс базы муравьев
    class AntBase
    {
        float startX = 200, startY = 200;   // Начальн. координаты базы
        float growthRate = 0.1f;  // Скорость появления муравьев
        float antsNums = 10;  // Кол-во муравьев в начале
        float capacity = 100;    // Вместимость базы
        int warriorsNum, buildersNum, scoutsNum, foodersNum;   // Кол-во разных типов муравьев
        public float food;  // Количество еды
        public List<Ant> ant;  // Список муравьев

        World world;    // Ук-ль на класс мир
        Random rand;    // Для генерации случайных чисел

        // Конструктор
        public AntBase()
        {
            rand = new Random();
            world = new World(this, rand);
            ant = new List<Ant>();

            // Создаем начальное количество муравьев
            for (int i = 0; i < antsNums; i++)
            {
                AddAnt();
            }
            food = 12;
        }

        // Функция добавления новых муравьев
        private void AddAnt()
        {
          // Определим тип муравья
            int antType;
            int k = rand.Next(0, 10);
            if (k < 5) {
                antType = Ant.scouts;   // Будет муравей-разведчик
            }
            else if (k == 5) {
                antType = Ant.builders;     // Будет муравей-строитель
            }
            else {
                antType = Ant.warriors;             // Будет муравей-воин
            }

            // Добавляем в список
            ant.Add(new Ant(world, this, startX + rand.Next(0, 10), startY + rand.Next(0, 10), antType));
        }

        // Функция обновления численности муравьев
        private void UpdateNums()
        {
            // Если пищи нету
            if (food == 0)
            {
                growthRate -= ant.Count / 10000000.0f;  // Уменьшаем рост численности муравьев
            }
            else {  // Иначе если есть еда находим рост численности 
                growthRate = food / 100.0f;
            }
            int antsBefore = (int)antsNums;     // Это сколько муравьев до

            int antsAfter = (int)(antsNums + growthRate);       // А это стало после
            
            // Если численность все таки увеличилась
            if (antsBefore < antsAfter)
            {
                // Если есть место,
                if (capacity > ant.Count - 1)
                    AddAnt();   // то добавляем муравья 
                else
                    antsNums -= 1;    // Иначе ничего не делаем
            }
            else if (antsBefore > antsAfter)    // Иначе если численность упала
            {
                // Просто убираем рандмного муравья
                if (ant.Count > 0)
                {
                    ant.RemoveAt(rand.Next(ant.Count));     // Удаляем из списка
                }
            }

            // Изменяем численность
            antsNums += growthRate;

            if (antsNums < 0) {
                antsNums = 0;
            }
        }

        // Обновляем базу муравьев
        public void Update()
        {
            // Обновляем численность
            UpdateNums();

            scoutsNum = 0;
            warriorsNum = 0;
            foodersNum = 0;
            buildersNum = 0;

            // Считаем кол-во разных типов муравьев
            for (int i = 0; i < ant.Count; i++)
            {
                ant[i].Update(rand);

                if (ant[i].antType == Ant.scouts)
                {
                    scoutsNum++;
                }
                if (ant[i].antType == Ant.builders)
                {
                    buildersNum++;
                }
                if (ant[i].antType == Ant.warriors)
                {
                    warriorsNum++;
                }
                if (ant[i].antType == Ant.fooders)
                {
                    foodersNum++;
                }
            }

            // Если еда есть, то вместимость базы муравьев повышается
            if (food > 0) {
                capacity += buildersNum / 1000.0f;
            }

            // Уменьшаем запас еды
            food -= (float)(antsNums / 10000.0);
            if (food < 0)
            {
                food = 0;
            }

            // Так же обновляем мир
            world.Update();
        }
        
        // Отрисовка
        public void Draw(Graphics g)
        {
            // Очистка
            g.Clear(Color.White);

            // Отрисовка базы
            g.DrawRectangle(Pens.Brown, startX, startY, 25, 25);

            // Отрисовка муравьев
            for (int i = 0; i < ant.Count; i++)
            {
                if (ant[i].antType == Ant.warriors)
                {
                    g.DrawRectangle(Pens.Red, ant[i].x, ant[i].y, 2, 2);
                }
                else if (ant[i].antType == Ant.builders)
                {
                    g.DrawRectangle(Pens.Orange, ant[i].x, ant[i].y, 2, 2);
                }
                else if (ant[i].antType == Ant.scouts)
                {
                    g.DrawRectangle(Pens.Green, ant[i].x, ant[i].y, 2, 2);
                }
                else if (ant[i].antType == Ant.fooders)
                {
                    g.DrawRectangle(Pens.Blue, ant[i].x, ant[i].y, 2, 2);
                }
            }

            // Отрисовка мира (внешних факторов: врагов и пищи)
            world.Draw(g);
        }

        // Убиваем муравья
        public void killedAnt(int l)
        {
            ant.RemoveAt(l);         //   Удаляет элемент списка с выбранным индексом
            antsNums -= 1;          // и уменьшаем кол-во муравьев
        }

        // Получить рост
        public float getGrowthRate()
        {
            return growthRate;
        }

        // Получить еду
        public float getFood()
        {
            if (food > 0) {
                return food;
            }
            else {
                return 0;
            }
        }

        // Получить вместимость
        public int getCapacity()
        {
            return (int)capacity;
        }

        // Получить кол-во муравьев
        public int getAntsNum()
        {
            return (int)antsNums;
        }

        // Получить кол-во муравьев-разведчиков
        public int getNumScouts()
        {
            return scoutsNum;
        }

        // Получить кол-во муравьев-собирателей
        public int getNumFooders()
        {
            return foodersNum;
        }
        
        // Получить кол-во муравьев-воинов 
        public int getNumWarriors()
        {
            return warriorsNum;
        }

        // Получить кол-во муравьев-строителей
        public int getNumBuilders()
        {
            return buildersNum;
        }
     }
  }