using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Thread.Sleep()
//Application.DoEvents();
namespace elevator2
{
    public enum flosta { up,down,both,none};
    public struct task
    {
        public int askfloor;
        public bool isup;
    }
    public partial class Form1 : Form
    {
        lift[] lifts = new lift[6];
        int current_inside_lift = new int();
        int current_floor = new int();
        public List<task> tasklist = new List<task>();

        public static Button[] systemButtons=new Button[21];
        public static Button[] upbutton = new Button[6];
        public static Button[] downbutton = new Button[6];
        public static Label[] liftlabel = new Label[6];
        public flosta[] floorrequest = new flosta[21];
        public int[] liftwaittime = new int[6];

        //文本框加载时附带的操作
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "1";

            systemButtons[0] = new Button();
            systemButtons[0].Text = "null";
            systemButtons[1] = ibutton1;
            systemButtons[2] = ibutton2;
            systemButtons[3] = ibutton3;
            systemButtons[4] = ibutton4;
            systemButtons[5] = ibutton5;
            systemButtons[6] = ibutton6;
            systemButtons[7] = ibutton7;
            systemButtons[8] = ibutton8;
            systemButtons[9] = ibutton9;
            systemButtons[10] = ibutton10;
            systemButtons[11] = ibutton11;
            systemButtons[12] = ibutton12;
            systemButtons[13] = ibutton13;
            systemButtons[14] = ibutton14;
            systemButtons[15] = ibutton15;
            systemButtons[16] = ibutton16;
            systemButtons[17] = ibutton17;
            systemButtons[18] = ibutton18;
            systemButtons[19] = ibutton19;
            systemButtons[20] = ibutton20;

            upbutton[0] = new Button();
            upbutton[0].Text = "null";
            upbutton[1] = upbutton1;
            upbutton[2] = upbutton2;
            upbutton[3] = upbutton3;
            upbutton[4] = upbutton4;
            upbutton[5] = upbutton5;

            downbutton[0] = new Button();
            downbutton[0].Text = "null";
            downbutton[1] = downbutton1;
            downbutton[2] = downbutton2;
            downbutton[3] = downbutton3;
            downbutton[4] = downbutton4;
            downbutton[5] = downbutton5;

            liftlabel[0] = new Label();
            liftlabel[0].Text = "null";
            liftlabel[1] = lift1label;
            liftlabel[2] = lift2label;
            liftlabel[3] = lift3label;
            liftlabel[4] = lift4label;
            liftlabel[5] = lift5label;
        }
        //文本框改变后的反应
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int temp = int.Parse(comboBox1.Text);
            if(temp>=1&&temp<=20)
            current_floor = temp;
            updownbuttonrenew();
        }
        //添加下拉选项
        private void Form1_Load(object sender, EventArgs e)
        {
            current_inside_lift = 1;
            current_floor = 1;
            for (int i = 1; i <= 20; i++)
                comboBox1.Items.Add(i); 
            for (int i = 0; i < 6; i++)
            {
                lifts[i] = new lift();
                lifts[i].num = i;
            }
            for (int i = 0; i < 21; i++)
                floorrequest[i] = flosta.none;
            for(int i=0;i<6;i++)
            {
                liftwaittime[i] = new int();
                liftwaittime[i] = 0;
            }
            //testlabel.Text = 0.ToString();
        }
        //内部电梯按钮切换
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            current_inside_lift = 1;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            current_inside_lift = 2;
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            current_inside_lift = 3;
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            current_inside_lift = 4;
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            current_inside_lift = 5;
        }
        //点击电梯内部按钮
        private void insidebutton_Click(object sender, EventArgs e)
        {
            Button buttons = sender as Button;
            if (lifts[current_inside_lift].isgood)
            {
                int target_f = new int();
                target_f = int.Parse(buttons.Text);
                buttons.Enabled = false;
                lifts[current_inside_lift].target_floor.Add(target_f);
                if (lifts[current_inside_lift].isuse == false)
                {
                    lifts[current_inside_lift].isuse = true;
                    if (target_f > lifts[current_inside_lift].current_floor)
                        lifts[current_inside_lift].status = Sta.up;
                    else if (target_f < lifts[current_inside_lift].current_floor)
                        lifts[current_inside_lift].status = Sta.down;
                    else lifts[current_inside_lift].status = Sta.stay;
                }
            }
        }
        //电梯的实时更新
        private void lifttimer_Tick(object sender, EventArgs e)
        {
            Timer timers = sender as Timer; 
            int currentlift = int.Parse(timers.Tag.ToString());
            if (lifts[currentlift].isuse == false && lifts[currentlift].call_floor.Count() != 0)
                lifts[currentlift].isuse = true;
            if (lifts[currentlift].isuse == true)
            {
                if (lifts[currentlift].status == Sta.up) 
                { 
                    lifts[currentlift].current_floor++;
                    if(current_inside_lift==currentlift)
                    inside_buttonrenew();
                    if(lifts[currentlift].isarrive())
                    {
                        floorrequest[lifts[currentlift].current_floor] =flosta.none;
                        lifts[currentlift].status = Sta.wait;
                    }
                }
                else if(lifts[currentlift].status==Sta.down)
                {
                    lifts[currentlift].current_floor--;
                    if (current_inside_lift == currentlift)
                        inside_buttonrenew();
                    if (lifts[currentlift].isarrive())
                    {
                        floorrequest[lifts[currentlift].current_floor] = flosta.none;
                        lifts[currentlift].status = Sta.wait;
                    }
                }
                else if(lifts[currentlift].status == Sta.stay)
                {
                    if(liftwaittime[currentlift] > 0)
                    {
                        liftwaittime[currentlift]--;
                    }
                    else {
                        lifts[currentlift].target_floor.Remove(lifts[currentlift].current_floor);
                        lifts[currentlift].call_floor.Remove(lifts[currentlift].current_floor);
                        if (lifts[currentlift].target_floor.Count() == 0)
                            lifts[currentlift].isuse = false;
                        floorrequest[lifts[currentlift].current_floor] = flosta.none;
                       
                        if (lifts[currentlift].target_floor.Count() != 0)
                            if (lifts[currentlift].target_floor.First() > lifts[currentlift].current_floor)
                                lifts[currentlift].status = Sta.up;
                            else lifts[currentlift].status = Sta.down;
                        else if (lifts[currentlift].call_floor.Count() != 0)
                        {
                            if (lifts[currentlift].call_floor.First() < lifts[currentlift].current_floor)
                                lifts[currentlift].status = Sta.down;
                            else lifts[currentlift].status = Sta.up;
                        }
                        if (current_inside_lift == currentlift)
                            inside_buttonrenew();
                    }
                }
                else if(lifts[currentlift].status == Sta.wait)
                {
                    lifts[currentlift].status = Sta.stay;
                    liftwaittime[currentlift] = 1;
                }
            }
            else
            { 
                if (current_inside_lift == currentlift)
                    inside_buttonrenew();
            }
        }
        //点击向上按钮
        private void upbutton_Click(object sender, EventArgs e)
        {
            foreach (Button b in upbutton)
                b.Enabled = false;
            task add_task = new task();
            add_task.askfloor = current_floor;
            add_task.isup = true;
            if (floorrequest[current_floor] == flosta.none)
            {
                floorrequest[current_floor] = flosta.up;
                dispatch(add_task);
            }
        }
        //点击向下按钮
        private void downbutton_Click(object sender,EventArgs e)
        {
            foreach (Button b in downbutton)
                b.Enabled = false;
            task add_task = new task();
            add_task.askfloor = current_floor;
            add_task.isup = false;
            if (floorrequest[current_floor] == flosta.none)
            {
                floorrequest[current_floor] = flosta.down;
                dispatch(add_task);
            }
        }
        //最后一个timer,进行等待队列调度和对于电梯状况的分析
        private void waitlistconduct_Tick(object sender, EventArgs e)
        {
                dispatch_wait();
        } 
        //对于每一个电梯内按钮的情况的更新
        public void inside_buttonrenew()
        {
            foreach (Button b in systemButtons)
            {
                b.Enabled = true;
            }
            int cil = current_inside_lift;
            if (lifts[current_inside_lift].isuse == true)
                if (lifts[current_inside_lift].status == Sta.up)
                    foreach (Button b in systemButtons)
                        if (b.Text != "null")
                        {
                            if (int.Parse(b.Text) < lifts[current_inside_lift].current_floor)
                                b.Enabled = false;
                            else
                                if (lifts[current_inside_lift].is_b_target(int.Parse(b.Text)))
                                b.Enabled = false;
                        }
                        else;
                else if (lifts[current_inside_lift].status == Sta.down)
                    foreach (Button b in systemButtons)
                        if (b.Text != "null")
                        {
                            if (int.Parse(b.Text) > lifts[current_inside_lift].current_floor)
                                b.Enabled = false;
                            else
                                if (lifts[current_inside_lift].is_b_target(int.Parse(b.Text)))
                                b.Enabled = false;
                        }
                        else;
                else if (lifts[current_inside_lift].status == Sta.stay)
                {
                    if (lifts[current_inside_lift].target_floor.Count() == 0)
                    {
                        foreach (Button c in systemButtons)
                            if (c.Text != "null")
                                c.Enabled = true;
                    }
                    else lifts[current_inside_lift].status = Sta.up;
                }
                else;
            else
            {
                foreach (Button b in systemButtons)
                {
                    b.Enabled = true;
                }
            }
        }
        //外部电梯调度
        public void dispatch(task t)
        {
            int mylift = new int();
            mylift = chooselift(t.isup, t.askfloor);
            if (mylift != -1)
            {
                lifts[mylift].call_floor.Add(t.askfloor);
            }
            else { 
                tasklist.Add(t);
                    //testlabel.BackColor = Color.Red; 
            }
        }
        //对于等待队列的调度
        public void dispatch_wait()
        {
            List<task> temptask = new List<task>();
            int mylift = new int();
            if (tasklist.Count() == 0) return;
            //进行剩余元素的合理性分析，若可以加入任务则加入
            foreach (lift l in lifts)//等待队列正常
                if (l.isuse == false && l.num != 0)
                {
                    task maxtask = tasklist.First(), mintask = tasklist.First();
                    //找到等待队列的最高楼层和最低楼层
                    foreach (task t in tasklist)
                    {
                        if (t.askfloor > maxtask.askfloor) maxtask = t;
                        if (t.askfloor < mintask.askfloor) mintask = t;
                    }
                    //取最高或最低楼层作为电梯的目的地
                    if (l.current_floor < 10 && l.current_floor < maxtask.askfloor)
                    {
                        l.call_floor.Add(maxtask.askfloor); l.mxfloor = maxtask.askfloor; tasklist.Remove(maxtask);
                        foreach (task t in tasklist)
                        {
                            if (t.askfloor > l.current_floor)
                                temptask.Add(t);

                        }
                    }
                    else
                    if (l.current_floor > 10 && l.current_floor > mintask.askfloor)
                    {
                        l.call_floor.Add(mintask.askfloor); l.mxfloor = mintask.askfloor; tasklist.Remove(mintask);
                        foreach (task t in tasklist)
                        {
                            if (t.askfloor < l.current_floor)
                                temptask.Add(t);

                        }
                    }
                    else
                    {
                        l.call_floor.Add(tasklist.First().askfloor); l.mxfloor = maxtask.askfloor; tasklist.Remove(tasklist.First());
                        foreach (task t in tasklist)
                        {
                            if (t.askfloor > l.current_floor && t.askfloor < maxtask.askfloor)
                                temptask.Add(t);

                        }
                    }
                    foreach (task t in temptask)
                    {
                        l.call_floor.Add(t.askfloor); tasklist.Remove(t);
                    }
                    temptask.Clear();
                    foreach (task t in tasklist)
                    {
                        mylift = chooselift(t.isup, t.askfloor);
                        if (mylift != -1)
                        {
                            lifts[mylift].call_floor.Add(t.askfloor);
                            tasklist.Remove(t); break;
                        }
                    }
                }
        }
        //获取可用的电梯
        public int chooselift(bool isup,int c_floor)
        {
            int bestlift = new int();
            int shortestdistance = new int();
            if (isup == true)
            {
                foreach (lift l in lifts)
                {
                    if (l.num == 0) continue;
                    int maxone = new int();
                    maxone = 0;
                    foreach (int t in l.target_floor)
                    {
                        if (t > maxone) maxone = t;
                    }
                    if (l.current_floor <= c_floor && maxone > c_floor)
                        return l.num;
                }
                foreach (lift l in lifts)
                {
                    if (l.isuse == false&&l.num!=0)
                    {
                        shortestdistance = 20;
                        bestlift = l.num;
                        foreach (lift li in lifts)
                        {
                            if (li.isuse == false&&li.isgood && li.num != 0 && abs(li.current_floor , c_floor) < shortestdistance)
                            {
                                bestlift = li.num;
                                shortestdistance = abs(li.current_floor , c_floor);
                            }
                        }
                        return bestlift;
                    }
                }
            }
            else
            {
                foreach (lift l in lifts)
                {
                    if (l.num == 0) continue;
                    int minone = new int();
                    minone = 20;
                    foreach (int t in l.target_floor)
                    {
                        if (t < minone) minone = t;
                    }
                    if (l.current_floor >= c_floor && minone < c_floor)
                        return l.num;
                }
                foreach (lift l in lifts)
                {
                    if (l.isuse == false)
                    {
                        shortestdistance = 20;
                        bestlift = l.num;
                        foreach (lift li in lifts)
                        {
                            if (li.isuse == false&&li.isgood&& li.num != 0 && abs(li.current_floor, c_floor) < shortestdistance)
                            {
                                bestlift = li.num;
                                shortestdistance = abs(li.current_floor, c_floor);
                            }
                        }
                        return bestlift;
                    }
                }
            }
            return -1;
        }
        //求绝对值
        public int abs(int a,int b)
        {
            return a > b ? (a-b) : (b-a);
        }
        //对于电梯的标签的更新
        public void outlabelrenew()
        {
            updownbuttonrenew();
            int liftnum;
            lift cl;
            cl = lifts[current_inside_lift];
            if (!cl.isgood) { pictureBox1.BackColor = Color.Red; }
            else if (cl.status == Sta.up)
                currentfloorlabel.Text = cl.current_floor.ToString() + "↑";
            else if (cl.status == Sta.down)
                currentfloorlabel.Text = cl.current_floor.ToString() + "↓";
            else currentfloorlabel.Text = cl.current_floor.ToString();
            foreach (Label label in liftlabel)
            {
                if (label.Text == "null") continue;
                liftnum = int.Parse(label.Tag.ToString());
                if (!lifts[liftnum].isuse)
                    label.Text = lifts[liftnum].current_floor.ToString();
                else if (lifts[liftnum].status == Sta.up)
                    label.Text = lifts[liftnum].current_floor.ToString() + "↑";
                else if (lifts[liftnum].status == Sta.down)
                    label.Text = lifts[liftnum].current_floor.ToString() + "↓";
                else if (lifts[liftnum].status == Sta.stay)
                    label.Text = lifts[liftnum].current_floor.ToString();
            }
        }
        //对五个标签的更新
        private void insidelabelrenew_Tick(object sender, EventArgs e)
        {
            outlabelrenew();
        }
        //对向上向下按钮的更新
        public void updownbuttonrenew()
        { 
            switch (floorrequest[current_floor])
            {
                case flosta.up:
                    {
                        foreach (Button b in upbutton)
                            b.Enabled = false;
                        foreach (Button b in downbutton)
                            b.Enabled = true;
                        break;
                    }
                case flosta.down:
                    {
                        foreach (Button b in upbutton)
                            b.Enabled = true ;
                        foreach (Button b in downbutton)
                            b.Enabled = false;
                        break;
                    }
                case flosta.both:
                    {
                        foreach (Button b in upbutton)
                            b.Enabled = false;
                        foreach (Button b in downbutton)
                            b.Enabled = false;
                        break;
                    }
                case flosta.none:
                    {
                        foreach (Button b in upbutton)
                            b.Enabled = true;
                        foreach (Button b in downbutton)
                            b.Enabled = true;
                        break;
                    }
            }
            if (current_floor == 1)
                foreach (Button b in downbutton)
                    b.Enabled = false;
            if (current_floor == 20)
                foreach (Button b in upbutton)
                    b.Enabled = false;
        }
        //点击开门键
        private void openbutton_Click(object sender, EventArgs e)
        {
            if (lifts[current_inside_lift].isgood&&lifts[current_inside_lift].status ==Sta.stay)
            {
                lifts[current_inside_lift].status = Sta.wait; 
            }
        }
        //点击关门键
        private void closebutton_Click(object sender, EventArgs e)
        {
            if(lifts[current_inside_lift].isgood&&lifts[current_inside_lift].status==Sta.wait)
            {
                lifts[current_inside_lift].status=Sta.stay;
                liftwaittime[current_inside_lift] = 0;
            }
        }
        //点击报警键
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            lifts[current_inside_lift].isgood = false;
            lifts[current_inside_lift].status = Sta.stay;
            lifts[current_inside_lift].isuse = false;
        }
    }
}