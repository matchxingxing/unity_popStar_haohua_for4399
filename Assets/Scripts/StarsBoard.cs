using System.Collections.Generic;
using System;


/// <summary>
/// 棋盘类，实现读、写、添加、删除操作，不提供列移动操作（整个程序中没有移动列）；提供招法查询函数、招法块数查询函数。
/// 其中坐标表示为1int，高4位为X，低4位为Y。
/// </summary>
/// <remarks></remarks>
public class StarsBoard
{

    //局面
    private int[] m_Array = new int[10];
    //坐标排序器
    private static PointComparer pc = new PointComparer();

    public StarsBoard()
    {
    }

    public StarsBoard(StarsBoard pos)
    {
        Array.Copy(pos.m_Array, m_Array, 10);
    }

    //进行或运算
    public StarsBoard Or(StarsBoard value)
    {
        for (int i = 0; i <= 9; i++)
        {
            this.m_Array[i] = System.Convert.ToInt32(this.m_Array[i] | value.m_Array[i]);
        }
        return this;
    }

    //获取有多少个块
    public int GetTrueCount()
    {
        int i;
        int c = System.Convert.ToInt32(0);
        int v;
        // int c;
        // int v;
        for (i = 0; i <= 9; i++)
        {
            v = this.m_Array[i];
            while (v > 0)
            {
                v = v & (v - 1);
                c++;
            }
        }
        return c;
    }

    //获取某个位置的值
    public bool GetValue(int p)
    {
        return (this.m_Array[p >> 4] & (1 << (p & 0xF))) != 0;
    }

    //设置某个位置的值为True
    public void SetTrue(int p)
    {
        this.m_Array[p >> 4] = System.Convert.ToInt32(this.m_Array[p >> 4] | (1 << (p & 0xF)));
    }

    //设置某个位置的值为False
    public void SetFalse(int p)
    {
        this.m_Array[p >> 4] = System.Convert.ToInt32(this.m_Array[p >> 4] & ~(1 << (p & 0xF)));
    }

    //检测是否为空行
    public bool IsEmptyColumn(int index)
    {
        return m_Array[index] == 0;
    }

    //设置一列的值
    public void SetColumn(int index, int value)
    {
        m_Array[index] = value;
    }

    //获取一列的值
    public int GetColumn(int index)
    {
        return m_Array[index];
    }

    //设置全部值
    public void SetAll(bool value)
    {
        int num = System.Convert.ToInt32(value ? -1 : 0);
        int i;
        for (i = 0; i <= 9; i++)
        {
            this.m_Array[i] = num;
        }
    }

    //消除一个列上的块。会导致原来的位置消失上面的下移
    public void Pop(int p)
    {
        int num;
        int index = (int)(p >> 4);
        int num4 = p & 15;
        int num2 = this.m_Array[index] & (((int)(-1)) << (num4 + 1));
        if ((this.m_Array[index] & (((int)1) << num4)) != 0)
        {
            num = this.m_Array[index] & ~(((int)(-1)) << num4);
        }
        else
        {
            num = this.m_Array[index] & ~(((int)(-1)) << (num4 + 1));
        }
        this.m_Array[index] = num2 | (num << 1);
    }

    //向列插入块。会导致原来的位置的和上面的一同上移
    public void Push(int p, bool val)
    {
        int index = (int)(p >> 4);
        int num4 = p & 15;
        int num2 = this.m_Array[index] & (((int)(-1)) << (num4 + 1));
        if (val)
        {
            num2 |= ~(((int)(-1)) << (num4 + 1)) & (((int)(-1)) << num4);
        }
        int num = this.m_Array[index] & ~(((int)(-1)) << (num4 + 1));
        this.m_Array[index] = num2 | (num >> 1);
    }

    //获取连通性。局面，已处理表，点坐标，连通点集，连通点集包含的点数。
    public void FillPath(StarsBoard mTable, StarsBoard FullBoard, int p, ref int[] retval, ref int retcnt)
    {
        Stack<int> mArray = new Stack<int>(); //栈——将处理点表
        int mP = p; //正在处理点
        int mAP; //临时变量——可能被入栈的点
        int cnt = System.Convert.ToInt32(0);
        int x;
        int i;

        mArray.Push(mP); //入栈
        do
        {
            if (mTable.GetValue(mP) == false) //若未处理过
            {
                x = mP >> 4;
                if ((this.m_Array[x] & (1 << (mP & 0xF))) != 0) //若有子
                {
                    //临近点入栈
                    if ((mP & 0xF) > 0) //上
                    {
                        mAP = (int)(mP - 1);
                        if (!mTable.GetValue(mAP))
                        {
                            mArray.Push(mAP);
                        }
                    }
                    if ((mP & 0xF) < 10) //下
                    {
                        mAP = (int)(mP + 1);
                        if (!mTable.GetValue(mAP))
                        {
                            mArray.Push(mAP);
                        }
                    }
                    if (x > 0) //左
                    {
                        mAP = (int)(mP - 0x10);
                        if (!mTable.GetValue(mAP))
                        {
                            for (i = x - 1; i >= 0; i--)
                            {
                                if (FullBoard.m_Array[i] != 0)
                                {
                                    mAP = (int)((i << 4) | (mP & 0xF)); //向左查找第一个非空行取坐标。
                                    break;
                                }
                            }
                            mArray.Push(mAP);
                        }
                    }
                    if (x < 9) //右
                    {
                        mAP = (int)(mP + 0x10);
                        if (!mTable.GetValue(mAP))
                        {
                            for (i = x + 1; i <= 9; i++) //向右查找第一个非空行取坐标。
                            {
                                if (FullBoard.m_Array[i] != 0)
                                {
                                    mAP = (int)((i << 4) | (mP & 0xF));
                                    break;
                                }
                            }
                            mArray.Push(mAP);
                        }
                    }
                    //记录本点
                    retval[cnt] = mP;
                    cnt++;
                }
                mTable.SetTrue(mP); //修改为已处理
            }
            if (mArray.Count == 0) //还有点就出栈，没有就结束
            {
                break;
            }
            else
            {
                mP = mArray.Pop();
            }
        } while (true);
        //按Y进行排序，以便恢复局面时使用。
        Array.Sort(retval, 0, cnt, pc);
        //返回有效个数
        retcnt = cnt;
    }

    //这个函数与上面的函数基本相同，但只统计连续个数不返回坐标。
    public int SumStar(StarsBoard mTable, StarsBoard FullBoard, int p)
    {
        Stack<int> mArray = new Stack<int>(); //栈——将处理点表
        int mP = p; //正在处理点
        int mAP; //临时变量——可能被入栈的点
        int cnt = System.Convert.ToInt32(0);
        int x;
        int i;

        mArray.Push(mP); //入栈
        do
        {
            if (mTable.GetValue(mP) == false) //若未处理过
            {
                x = mP >> 4;
                if ((this.m_Array[x] & (1 << (mP & 0xF))) != 0) //若有子
                {
                    //临近点入栈
                    if ((mP & 0xF) > 0) //上
                    {
                        mAP = (int)(mP - 1);
                        if (!mTable.GetValue(mAP))
                        {
                            mArray.Push(mAP);
                        }
                    }
                    if ((mP & 0xF) < 10) //下
                    {
                        mAP = (int)(mP + 1);
                        if (!mTable.GetValue(mAP))
                        {
                            mArray.Push(mAP);
                        }
                    }
                    if (x > 0) //左
                    {
                        mAP = (int)(mP - 0x10);
                        if (!mTable.GetValue(mAP))
                        {
                            for (i = x - 1; i >= 0; i--) //向右查找第一个非空行取坐标。
                            {
                                if (FullBoard.m_Array[i] != 0)
                                {
                                    mAP = (int)((i << 4) | (mP & 0xF));
                                    break;
                                }
                            }
                            mArray.Push(mAP);
                        }
                    }
                    if (x < 9) //右
                    {
                        mAP = (int)(mP + 0x10);
                        if (!mTable.GetValue(mAP))
                        {
                            for (i = x + 1; i <= 9; i++)
                            {
                                if (FullBoard.m_Array[i] != 0)
                                {
                                    mAP = (int)((i << 4) | (mP & 0xF));
                                    break;
                                }
                            }
                            mArray.Push(mAP);
                        }
                    }
                    cnt++; //记录个数
                }
                mTable.SetTrue(mP); //修改为已处理
            }
            if (mArray.Count == 0) //还有点就出栈，没有就结束
            {
                break;
            }
            else
            {
                mP = mArray.Pop();
            }
        } while (true);
        return cnt;
    }

    public override string ToString()
    {
        string tmp = string.Empty;
        for (int y = 0; y <= 9; y++)
        {
            for (int x = 0; x <= 9; x++)
            {
                tmp += System.Convert.ToString(((GetValue((int)((x << 4) | y))) ? 1 : 0));
            }
            tmp += "\r\n";
        }
        return tmp;
    }

    public static string Int2BitString(int p)
    {
        string tmp = string.Empty;
        for (int i = 31; i >= 0; i--)
        {
            tmp += System.Convert.ToString(((((p >> i) & 1) == 1) ? 1 : 0));
        }
        return tmp;
    }

    public static int BitString2Int(string s)
    {
        if (s.Length > 32)
        {
            throw (new Exception("二进制串长度超过32"));
        }
        int ret = System.Convert.ToInt32(0);
        char[] tmp = s.PadLeft(32, '0').ToCharArray();
        for (int i = 0; i <= 31; i++)
        {
            if (int.Parse(tmp[i].ToString()) == 1)
            {
                ret = ret | (1 << (31 - i));
            }
        }
        return ret;
    }
    private class PointComparer : IComparer<int>
    {

        public int Compare(int p1, int p2)
        {
            return (p1 & 0xF) - (p2 & 0xF);
        }

    }

    public string toString(){
        string str ="";
        for(int i=0;i<10;i++){
            str+=" "+m_Array[i];
        }
        return str;
    }
}

