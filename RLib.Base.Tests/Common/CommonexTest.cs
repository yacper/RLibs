/********************************************************************
    created:	2020-07-23 14:50:05
    author:		joshua
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using NUnit.Framework;

namespace RLib.Base.Tests
{
	public class CommonexTest
	{

		[Test]
		public void Test_Vardm()
        {
            VarDM l = new VarDM(new List<int>() {1, 2, 3});
            List<int> l2 = l.Value as List<int>;



		}
	
	}

}
