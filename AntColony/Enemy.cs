using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
    class Enemy
    {
        public float x, y;      // Координаты
        public float dirOfMovX, dirOfMovY;    // Направление движения
        public float health;    // Здоровье
        float speed = 50;                     // Скорость движения

        // Ук-ли на классы, которые нужны (мира, базы муравьев и для генерации случайных чисел)
        World world;
        AntBase antbase;
        Random rand;

        // Конструктор
        public Enemy(World world, AntBase antbase, float x, float y, float health, Random rand)
        {
            this.world = world;
            this.antbase = antbase;
            this.x = x;
            this.y = y;
            this.health = health;
            this.rand = rand;

            // Создаем направление движения
            dirOfMovX = (rand.Next(20, 80) - 50) / (speed * 50);
            dirOfMovY = (rand.Next(20, 80) - 50) / (speed * 50);
        }

        // Обновлем состояние врага
        public void Update()
        {
            if (rand.Next(0, 800) == 0)
            {
                // Изменяем направление движения
                dirOfMovX = (rand.Next(20, 80) - 50) / (speed * 50);
                dirOfMovY = (rand.Next(20, 80) - 50) / (speed * 50);
            }
            // Двигаем
            x += dirOfMovX;
            y += dirOfMovY;
            
            // Поиск
            for (int i = 0; i < antbase.ant.Count; i++)
            {
                // Если рядом есть цель
                if (Math.Abs(antbase.ant[i].x - x) < 3 && Math.Abs(antbase.ant[i].y - y) < 3)
                {
                    // Убиваем ее
                    antbase.killedAnt(i);
                }
            }
        }
     }
  }