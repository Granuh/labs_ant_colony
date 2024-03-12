using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AntColony
{
    class World
    {
        AntBase antbase;   // Указатель на базу
        Random rand;       // Для генерации рандомных чисел
        public List<Food> food;    // Список еды
        public List<Enemy> enemy; // Список врагов

        // Конструктор
        public World(AntBase antbase, Random rand)
        {
            this.antbase = antbase;
            this.rand = rand;

            food = new List<Food>();
            enemy = new List<Enemy>();

            // Добавляем еду
            food.Add(new Food(rand.Next(700), rand.Next(40), rand.Next(50)));
        }
        
        // Отрисовка пищи и врагов
        public void Draw(Graphics g)
        {
            for (int i = 0; i < food.Count; i++)
                g.FillEllipse(Brushes.YellowGreen, food[i].x - 2, food[i].y - 2, 8, 8);

            for (int i = 0; i < enemy.Count; i++)
                g.FillEllipse(Brushes.DarkSlateGray, enemy[i].x - 2, enemy[i].y - 2, 5, 5);
        }

        // Обновление мира
        public void Update()
        {
            // Генерация врагов
            if (rand.Next(100) == 0)
                enemy.Add(new Enemy(this, antbase, rand.Next(600), rand.Next(600), rand.Next(25), rand));

            // Обновляем врагов
            for (int i = 0; i < enemy.Count; i++)
            {
                enemy[i].Update();
            }

            // Генерация еды
            if (rand.Next(10) == 0)
                food.Add(new Food(rand.Next(700), rand.Next(840), rand.Next(100)));
        }
     }
  }