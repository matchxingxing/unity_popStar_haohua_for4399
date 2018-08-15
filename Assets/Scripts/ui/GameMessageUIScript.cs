using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class GameMessageUIScript : MonoBehaviour {
    private Text _txtCurLevel;
    private Text _txtCurScore;
    private Text _txtCurLevelMaxScore;
    private Text _txtCurLevelTargetScore;
    private Text _txtRewardScore;//奖励分数文本
    private Text _txtRemainStars;//剩下的星星文本
    private int _score = 0;
    private int _dynScore = 0;

    private int _rewardScore;//要显示的奖励分数
    private int _dynRewardScore;

    void Awake(){
        gameObject.name = "gameMessageUI";
        _txtCurLevel = GameObject.Find("gameMessageUI/txtCurLevel").GetComponent<Text>();
        _txtCurScore = GameObject.Find("gameMessageUI/txtCurScore").GetComponent<Text>();
        _txtCurLevelMaxScore = GameObject.Find("gameMessageUI/txtCurLevelMaxScore").GetComponent<Text>();
        _txtCurLevelTargetScore = GameObject.Find("gameMessageUI/txtCurLevelTargetScore").GetComponent<Text>();
        _txtRewardScore = GameObject.Find("gameMessageUI/txtRewardScore").GetComponent<Text>();
        _txtRemainStars = GameObject.Find("gameMessageUI/txtRemainStars").GetComponent<Text>();
    }

    public void Init(int level,int maxScore,int targetScore) {
        //当前关卡
        _txtCurLevel.text = level.ToString();
        //最高纪录
        if (maxScore >= 1e4) _txtCurLevelMaxScore.text = (maxScore * 0.0001f).ToString("0.00") + "万";
        else _txtCurLevelMaxScore.text = maxScore.ToString();
        //目标分数
       // _txtCurLevelTargetScore.text = targetScore.ToString();
        if (targetScore >= 1e4) _txtCurLevelTargetScore.text = (targetScore * 0.0001f).ToString("0.00") + "万";
        else _txtCurLevelTargetScore.text = targetScore.ToString();
        //隐藏奖励文本框
        ShowOrHideReward(true);
    }

    void OnGUI() {
        if (_dynScore != _score) {
            _dynScore += (int)Mathf.Sign(_score - _dynScore);
            if (_dynScore >= 1e4) _txtCurScore.text = (_dynScore * 0.0001f).ToString("0.00") + "万";
            else _txtCurScore.text = _dynScore.ToString();
        }
    }

    public void SetScore(int value,bool isRightNow) {
        _score = value;
        if (isRightNow) {
            _dynScore = _score;
            if(_dynScore>=1e4) _txtCurScore.text = (_dynScore*0.0001f).ToString("0.00")+"万";
            else _txtCurScore.text = _dynScore.ToString();
        }
    }

    public void ShowOrHideReward(bool hide = false) {
        if (hide) {
            _txtRewardScore.color = Color.clear;
            _txtRemainStars.color = Color.clear;
        } else {
            _txtRewardScore.color = Color.white;
            _txtRemainStars.color = Color.white;
        }
    }

    public void SetRewardScore(int value,int dynRewardScore = -1,bool isRightNow = false) {
        _rewardScore = value;
        if (dynRewardScore >= 0) _dynRewardScore = dynRewardScore;
        if (isRightNow) {
            _dynRewardScore = _rewardScore;
            _txtRewardScore.text = (GameData.version == "cn" ? "奖励分数 " : "reward score ") + _dynRewardScore.ToString();
        } else {
            
            float repeatRate = 0.1f;
            InvokeRepeating("RewardRepeat",0,repeatRate);
        }
    }

    private void RewardRepeat() {
        if (_dynRewardScore != _rewardScore) {
            float sign = Mathf.Sign(_rewardScore - _dynRewardScore);
            _dynRewardScore += (int)(sign * 20f);
            if (sign > 0) { if (_dynRewardScore > _rewardScore)_dynRewardScore = _rewardScore; } else { if (_dynRewardScore < _rewardScore)_dynRewardScore = _rewardScore; }
            _txtRewardScore.text = (GameData.version == "cn" ? "奖励分数 " : "reward score ") + _dynRewardScore.ToString();
        } else {
            CancelInvoke("RewardRepeat");
        }
    }

    public void SetRemainStars(int value) {
        _txtRemainStars.text = (GameData.version == "cn" ? "剩下的星星 " : "remain stars ") + value.ToString();
    }

    public Vector2 GetScoretextPos(){
        return _txtCurScore.rectTransform.anchoredPosition;
    }

    public int rewardScore { get { return _rewardScore; } }
}
