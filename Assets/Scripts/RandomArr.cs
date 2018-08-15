using System;
using UnityEngine;
using System.Collections.Generic;
sealed class RandomArr {
    static int _max = 100;
    static int[] _list = new int[_max];
    static int[] _ranIdList = new int[_max];//存储打乱ID的数组
    static System.Random _rd = new System.Random();

    public static void Instance() {
        /*
        16 17 18 19 20 21 22 23 24 25 
        32 33 34 35 36 37 38 39 40 41 
        48 49 50 51 52 53 54 55 56 57 
        64 65 66 67 68 69 70 71 72 73 
        80 81 82 83 84 85 86 87 88 89 
        96 97 98 99 100 101 102 103 104 105 
        112 113 114 115 116 117 118 119 120 121 
        128 129 130 131 132 133 134 135 136 137 
        144 145 146 147 148 149 150 151 152 153 
        */
        int id = System.Convert.ToInt32(0);
        int i; int j;
        for(i = 0; i < 10; i++) {
            for(j = 0; j < 10; j++) {
                _list[id] = (int)((i << 4) | j);
                id++;
            }
        }

        //随机化id数组 
        for(i = 0; i < _max; i++) _ranIdList[i] = i;//添加id 0~99
        Randomize(_ranIdList, _max);
    }

    public static void Randomize(int[] list, int maxLength) {
        int id0; int id1;
        for(int i = 0; i < maxLength; i++) {
            id0 = _rd.Next(0, maxLength);//取0~maxLength(不包含maxLength)间的随机数
            id1 = list[i];
            list[i] = list[id0];
            list[id0] = id1;
        }
    }

    //产生cnt个元素，各个元素的和为sum，元素的最小值为min，元素的最大值为max的32位整数数组。
    public static int[] RandElement(int sum, int cnt, int min, int max) {
        //计算初始数组
        int[] tmp = new int[cnt - 1 + 1];
        int mn = sum / cnt;
        int i;
        for(i = 0; i <= cnt - 2; i++) {
            tmp[i] = mn;
        }
        //最后一个元素用来容纳转整时产生的小数部分，保证和。
        tmp[cnt - 1] = sum - mn * (cnt - 1);
        //随机化等值加减
        int a1;
        int a2;
        int r1;
        int rn;
        for(i = 0; i <= cnt - 1; i++) {
            //取一个随机差
            r1 = _rd.Next(0, max - min + 1);
            //加减随机差之后两个数据
            a1 = System.Convert.ToInt32(tmp[i] + r1);
            do //防止自身加减，会导致仅记录减法结果。并且，导致数据变换次数减小。
            {
                rn = _rd.Next(0, cnt - 1);
            } while(!(rn != i));
            a2 = System.Convert.ToInt32(tmp[rn] - r1);
            //不超范围的进行加减
            if(a1 >= min && a1 <= max && a2 >= min && a2 <= max) {
                tmp[i] = a1;
                tmp[rn] = a2;
            }
        }
        return tmp;
    }

    static int _nextRandomId = -1;
    public static int GetNextRandomElement() {
        if(_nextRandomId == _max - 1) _nextRandomId = -1;
        _nextRandomId++;
        int id = _ranIdList[_nextRandomId];
        return _list[id];
    }
}