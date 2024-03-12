using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
    // Класс еда
    class Food
    {
        public int x, y;    // Координаты
        public float n;     // Кол-во еды

        // Конструктор
        public Food(int x, int y, float n)
        {
            this.x = x;
            this.y = y;
            this.n = n;
        }
    }
}
