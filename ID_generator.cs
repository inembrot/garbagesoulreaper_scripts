using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarbageSoulReaper.Sources
{
    class ID_generator
    {

        private static int ID = 0;

        public static int getId()
        {
            ID++;
            return ID;
        }
    }
}
