/********************************************************************
    created:	2020/1/10 15:26:42
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class ThreadEx
    {
        public static void SleepRandom(int minMill, int maxMill)
        {
            if(maxMill<minMill ||
               minMill<=0)
                throw new ArgumentException(string.Format("Bad Args [%0,%1]:%2", minMill, maxMill));

            int delay = MathEx.Random(minMill, maxMill);
            System.Threading.Thread.Sleep(delay);
        }

    }
}
