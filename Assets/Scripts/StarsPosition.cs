public class StarsPosition{
    //当前局面
    private StarsBoard[] _starsBoard=new StarsBoard[5];
    //消除导致的分值累计
    private int _sumPop;
    //距离根节点步数
    private int _nDistance;

    public StarsPosition(StarsBoard[] starsBoard){
        _starsBoard = starsBoard;
    }

    //按坐标获取可消除块数
    public int GetBlackByPoint(int p, ref int[] retval){
        int tmpcnt = 0;
        StarsBoard mTable = new StarsBoard();
        StarsBoard fullBoard = new StarsBoard(); //二值化完整棋盘，用于判定是否为空行
        for (int i = 0; i < 5; i++)  fullBoard = fullBoard.Or(_starsBoard[i]);
        for (int i = 0; i < 5; i++){
            if (_starsBoard[i].GetValue(p)){
                _starsBoard[i].FillPath(mTable, fullBoard, p, ref retval, ref tmpcnt);
                retval[100] = (int)(i);        //颜色
                retval[101] = (int)(tmpcnt);   //长度
                return tmpcnt;
            }
        }
        return 0;
    }

    //执行一个招法
    public int DoPop(int[] point){
        int cnt = point[101];
        int vl = cnt * cnt * 5;
        int i;
        int j;
        //累计分值
        _sumPop += vl;
        //更新步数
        _nDistance++;
        //消除块；消除时会导致当前位置消失更高位置下降
        for (i = 0; i < 5; i++){
            for (j = 0; j < cnt; j++) {
                _starsBoard[i].Pop(point[j]);
            }
        }
        return vl;
    }

    public void DoPopOne(int p){
        for(int i = 0; i < 5; i++) _starsBoard[i].Pop(p);
    }

    //左移填满空行
    public int[] DoMoveToLeft()
    {
        int[] ret = new int[11];
        int c;  int i;
        StarsBoard fullBoard = new StarsBoard(); //二值化完整棋盘，用于判定是否为空行
        StarsBoard[] tmpBoard = new StarsBoard[5]; //新的棋盘数组
        for (c = 0; c <= 4; c++)
        {
            fullBoard = fullBoard.Or(_starsBoard[c]);
            tmpBoard[c] = new StarsBoard();
        }

        //遇到非空行就复制到新的之中
        int curidx = 0;
        for (i = 0; i <10; i++)
        {
            if (!fullBoard.IsEmptyColumn(i))
            {
                for (c = 0; c <= 4; c++)
                {
                    tmpBoard[c].SetColumn(curidx, System.Convert.ToInt32(_starsBoard[c].GetColumn(i)));
                }
                ret[curidx] = i;    //记录当前位置存储的是原来的第几列。
                curidx++;
            }
        }
        //记录有效个数
        ret[10] = curidx;

        //替换现有引用
        _starsBoard = tmpBoard;
        return ret;
    }

    //获取合理招法
    public int NextGenerateMove(ref int[][] retval)
    {
        int cnt = 0;
        int tmpcnt = 0;
        StarsBoard mTable = new StarsBoard();
        StarsBoard fullBoard = new StarsBoard(); //二值化完整棋盘，用于判定是否为空行
        for (int c = 0; c <= 4; c++)
        {
            fullBoard = fullBoard.Or(_starsBoard[c]);
        }
        int p;
        for (int c = 0; c <= 4; c++) //遍历颜色
        {
            mTable = new StarsBoard();
            for (int y = 0; y <= 9; y++) //遍历坐标
            {
                for (int x = 0; x <= 9; x++)
                {
                    p = (int)((x << 4) | y);
                    int[] tmpps = new int[102];
                    if (mTable.GetValue(p) == false)
                    {
                        _starsBoard[c].FillPath(mTable, fullBoard, p, ref tmpps, ref tmpcnt);
                        if (tmpcnt > 1)
                        {
                            tmpps[100] = (int)(c); //颜色
                            tmpps[101] = (int)(tmpcnt); //长度
                            retval[cnt] = tmpps; //记录结果
                            cnt++;
                        }
                    }
                }
            }
        }
        return cnt;
    }

    public StarsBoard[] starsBoard{ get{return _starsBoard; }}
}
