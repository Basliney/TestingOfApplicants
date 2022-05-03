using System;
using System.Collections.Generic;

namespace TestingOfApplicants.Models.ProgramStaticFiles.Map
{
    public class Map
    {
        private List<Pair> map = new List<Pair>();
        private int _countComplete = 0;

        #region Property

        public int Count { get { return map.Count; } }
        public int CountComplete { get { return _countComplete; } set { _countComplete = value; } }

        #endregion

        public bool Add(int key)
        {
            try
            {
                int index = InMap(key);
                if (index == -1)
                {
                    Pair pair = new Pair(key);
                    map.Add(pair);
                }
                else
                {
                    map[index].Value++;
                }
                CountComplete++;
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        private int InMap(int key)
        {
            for (int i = 0; i < map.Count; i++)
            {
                if (map[i].Key == key)
                {
                    return i;
                }
            }
            return -1;
        }

        public Pair Get(int index) { return map[index]; }

        public Pair Popular(bool flag)
        {
            Pair mostPopular = map[0];
            for (int i = 1; i < map.Count; i++)
            {
                if (mostPopular.Value < map[i].Value == flag)
                {
                    mostPopular = map[i];
                }
            }
            return mostPopular;
        }
    }
}
