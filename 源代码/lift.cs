using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator2
{
    public enum Sta {up,down,stay,wait };
    public class lift
    {
        //PCB设计
        public int current_floor = new int();
        public List<int> target_floor = new List<int>();
        public List<int> call_floor = new List<int>();
        public Sta status = new Sta();
        public bool isuse = new bool();
        public int num=new int();
        public bool isgood = new bool();
        public int mxfloor = new int();

        public lift()
        {
             current_floor=1;
            isgood = true;
            isuse = false;
            mxfloor = -1;
            status = Sta.stay;
        }
        public bool isarrive()
        {
            if(target_floor.Count()==0&&mxfloor!=-1)
            {
                if (current_floor == mxfloor)
                { mxfloor = -1; return true; }
                else return false;
            }
            else {
                foreach (int a in target_floor)
                    if (current_floor == a)
                        return true;
                foreach (int a in call_floor)
                    if (current_floor == a)
                        return true;
                return false;
            }
        }
        public bool is_b_target(int b)
        {
            foreach (int a in target_floor)
                if (b == a)
                    return true;
            return false;
        }
    }
}
