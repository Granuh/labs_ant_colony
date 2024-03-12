using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
    class Ant
    {
        public float x, y;  // Координаты
        public float dirOfMovX, dirOfMovY;    // Направление движения
        float destPointX, destPointY; // Точка назначения
        bool isDest = false;    // Для проверки движения к точке назначения

        World world;    // Ук-ль на класс мир
        AntBase antbase;   // Ук-ль на класс базы муравьев

        float speed = 30;    // Скорость движения
        float isFood; // Для переноса еды

        public int antType;    // Тип муравья

        public static int warriors = 2;      // Типы муравьев
        public static int builders = 4;
        public static int scouts = 7;
        public static int fooders = 1;

        // Конструктор
        public Ant(World world, AntBase antbase, float x, float y, int type)
        {
            this.world = world;
            this.antbase = antbase;
            this.x = x;
            this.y = y;
            this.antType = type;
            this.isFood = 0;

            dirOfMovX = 0.0f;
            dirOfMovY = 0.0f;
        }

        // Обновление муравья
        public void Update(Random rand)
        {
            // Если муравей идет к точке назначения, и может перестать двигаться к точке назначения
            if (isDest && rand.Next(0, 850) != 0)
            {
                // Если муравей взял еду и несет ее на базу, и это муравей-собиратель
                if (isFood != 0 && antType == fooders)
                {
                    // Муравей движется к муравейнику
                    dirOfMovX = 200 - x;
                    dirOfMovY = 200 - y;

                    float p = (float)Math.Sqrt(dirOfMovX * dirOfMovX + dirOfMovY * dirOfMovY);
                    dirOfMovX /= p;
                    dirOfMovY /= p;

                    dirOfMovX /= speed;
                    dirOfMovY /= speed;
                }
                else    // Иначе движемся к точке назначения
                {
                    dirOfMovX = destPointX - x;
                    dirOfMovY = destPointY - y;

                    float p = (float)Math.Sqrt(dirOfMovX * dirOfMovX + dirOfMovY * dirOfMovY);
                    dirOfMovX /= p;
                    dirOfMovY /= p;

                    dirOfMovX /= speed;
                    dirOfMovY /= speed;
                }
            }
            else    // Иначе просто муравей исследует мир
            {
                // Изменяем направление
                if (rand.Next(0, 850) == 0)
                {
                    dirOfMovX = (rand.Next(0, 100) - 50) / (speed * 50);
                    dirOfMovY = (rand.Next(0, 100) - 50) / (speed * 50);
                }

                // Если муравей-собиратель не идет к точке назначения,
                if (antType == fooders)
                {
                    antType = scouts;       // то становиться муравьем-разведчиком
                    isDest = false;
                }

            }

            // Cдвигаем муравья
            x += dirOfMovX;
            y += dirOfMovY;

            // Обработка движения для наших типов муравьев
            if (antType == builders)
            {
                MoveBuilder();
            }
            if (antType == warriors)
            {
                MoveWarriors();
            }
            if (antType == scouts)
            {
                MoveScouts();
            }
            if (antType == fooders)
            {
                MoveFooders();
            }
        }

        // Движения муравьев-воинов
        private void MoveWarriors()
        {
            // Муравьи отходят на 60 квадратов от базы
            if (x - 200 > 60)
            {
                dirOfMovX = -Math.Abs(dirOfMovX);
            }
            if (x - 200 < -60)
            {
                dirOfMovX = Math.Abs(dirOfMovX);
            }
            if (y - 200 > 60)
            {
                dirOfMovY = -Math.Abs(dirOfMovY);
            }
            if (y - 200 < -60)
            {
                dirOfMovY = Math.Abs(dirOfMovY);
            }

            // Проверка если есть рядом враг
            for (int i = 0; i < world.enemy.Count; i++)
            {
                // Если рядом враг, то отнимае у него здоровье
                if (Math.Abs(world.enemy[i].x - x) < 2 && Math.Abs(world.enemy[i].y - y) < 2)
                {
                    world.enemy[i].health -= 1;
                    // Убиваем врага если у него не осталось здоровья
                    if (world.enemy[i].health <= 0)
                    {
                        world.enemy.RemoveAt(i);
                        i--;
                    }
                }
            }

            // Проверка к какому врагу идем
            for (int i = 0; i < world.enemy.Count; i++)
            {
                // Если есть рядом враг движемся к нему
                if (Math.Abs(world.enemy[i].x - x) < 30 && Math.Abs(world.enemy[i].y - y) < 30)
                {
                    MovOfDest(world.enemy[i].x, world.enemy[i].y);
                    return;
                }
            }
        }

        // Движение для муравья-строителя
        private void MoveBuilder()
        {
            // Муравьи отходят на 30 квадратов от базы
            if (x - 200 > 30)
                dirOfMovX = -Math.Abs(dirOfMovX);
            if (x - 200 < -30)
                dirOfMovX = Math.Abs(dirOfMovX);
            if (y - 200 > 30)
                dirOfMovY = -Math.Abs(dirOfMovY);
            if (y - 200 < -30)
                dirOfMovY = Math.Abs(dirOfMovY);
        }

        // Движение муравьев-разведчиков
        private void MoveScouts()
        {
            // Проверка, что рядом есть еда
            for (int i = 0; i < world.food.Count; i++)
            {
                // Если есть рядом еда,
                if (Math.Abs(world.food[i].x - x) < 30 && Math.Abs(world.food[i].y - y) < 30)
                {
                    // то муравей-разведчик становится муравьем-собирателем, и идет к еде
                    MovOfDest(world.food[i].x, world.food[i].y);
                    antType = fooders;
                }
            }
        }

        // Движение муравьев-собирателей
        private void MoveFooders()
        {
            bool sit1 = (Math.Abs(destPointX - x) < 0.2 && Math.Abs(destPointY - y) < 0.2 && isFood == 0);
            bool sit2 = (Math.Abs(200 - x) < 0.2 && Math.Abs(200 - y) < 0.2 && isFood != 0);

            if (!sit1)
            {
                return;
            }

            if (!sit1 && sit2)
            {
                antbase.food += isFood;
                isFood = 0.0f;
            }

             bool fnd = false;

            // Ищем к какой еде подошли
             for (int i = 0; i < world.food.Count; i++)
             {
                 if (!(Math.Abs(world.food[i].x - x) < 0.2f) || !(Math.Abs(world.food[i].y - y) < 0.2f)) continue;

                 fnd = true;     // Берем еду
                 isFood = 3.0f;

                 // Отнимаем запас еды у этой точки
                 world.food[i].n -= 3.0f;

                 // Проверяем есть ли еще еда
                 if (world.food[i].n < 0)
                 {
                     world.food.RemoveAt(i);
                 }
                 break;
             }
             // Если нету еды, ее уже перенесли,
             if (fnd == false)
             {
                 isDest = false; // муравей-собиратель становится муравьем-разведчиком
                 antType = scouts;
             }
        }

        // Установим направление движения
        private void MovOfDest(float x, float y)
        {
            destPointX = x;
            destPointY = y;
            isDest = true;
        }
    }
}
