/********************************************************************
    created:	2021/6/25 14:53:11
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlib.Fin
{
    public partial class Gics
    {
        public static IEnumerable<Gics> GetByLevel(int l)
        {
            return _dic.Values.Where(p => p.Level == l);
        }

        public static Gics GetByCode(string code)
        {
            Gics ret = null;
            _dic.TryGetValue(code, out ret);
            return ret;
        }
        public static Gics GetByName(string name)
        {
            return _dic.Values.LastOrDefault(p => p.Name == name); // 从后往前找
        }


        public string Code { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public override string ToString()
        {
            return $"{Code, -10}:[{Level}] {Name} {Description}";
        }


        // 1-4级
        public int          Level
        {
            get
            {
                return Code.Length / 2;
            }
        }

        public Gics         GetLevel(int i)
        {
            int l = Level;

            if (l < i)
                return null;

            string code = Code.Substring(0, i * 2); 

            Gics ret = null;
            _dic.TryGetValue(code, out ret);
            return ret;
        }


        public Gics         Sector
        {
            get
            {
                return GetLevel(1);
            }
        }
        public Gics         IndustryGroup
        {
            get
            {
                return GetLevel(2);
            }
        }
        public Gics         Industry
        {
            get
            {
                return GetLevel(3);
            }
        }
        public Gics         SubIndustry
        {
            get
            {
                return GetLevel(4);
            }
        }

        public Gics         Parent
        {
            get
            {
                return GetLevel(Level -1);
            }
        }

        public IEnumerable<Gics> GetChildren(int depth)
        {
            int len = Code.Length;
            return _dic.Values.Where(p => p.Code.StartsWith(Code) && p != this && p.Code.Length <= len + depth * 2);
        }


        public IEnumerable<Gics> Children
        {
            get
            {
                return GetChildren(1);
            }
        }



        public bool IsWithin(Gics g)
        {
            return this.Code != g.Code && this.Code.StartsWith(g.Code);
        }
        public bool IsImmediateWithin(Gics g)
        {
            return Code.Length - 2 == g.Code.Length && IsWithin(g);
        }

        public bool Contains(Gics g)
        {
            return g.IsWithin(this);
        }
        public bool IsImmediateContains(Gics g)
        {
            return g.IsImmediateWithin(this);
        }



        private Gics(string code, string name, string desc = null)
        {
            Code = code;
            Name = name;
            Description = desc;
        }

    }

}
