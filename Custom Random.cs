using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duck_Hunt
{
    class Custom_Random<T>
    {
        List<T> args = new List<T>();
        int numberOfArgs = 0;

        Random rand = new Random();

        public Custom_Random()
        {

        }

        public Custom_Random(params T[] arguments)
        {
            foreach(T argum in arguments)
            {
                args.Add(argum);
                numberOfArgs++;
            }
        }

        public T ReturnRandom()
        {
            int randNum = rand.Next(0, numberOfArgs);

            return args[randNum];
        }
    }
}

