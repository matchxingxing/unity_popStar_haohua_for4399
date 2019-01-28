var apiObject={

	/**
	* 控制进度条播放进度
	* @param num  播放进度，值为0到100
	*/
	progress:function(num){
		window.h5api.progress(num);
	},
		
	/**
	* 提交积分
	* @param score    需要提交的积分
	*/
	submitScore:function(score){
		console.log(window);
		function callback(obj){
			console.log('代码:' + obj.code + ',消息:' + obj.message + ',数据:' + obj.data)
			if(obj.code === 10000){
				
				console.log('上传成功');
			} else {
				console.log('上传失败');
			}
			SendMessage('API4399', 'submitScoreCallback',obj.code);
		}
		window.h5api.submitScore(score,callback);
	},
		
		
	/**
	* 获得排行榜
	*/
	getRank:function(){
		function callback(obj){
			console.log('代码:' + obj.code + ',消息:' + obj.message + ',数据:' + obj.data)
			var datas = obj.code+"";
			if(obj.code === 10000){
				console.log('获取成功')
				for(var i = 0; i < obj.data.length; i++){
					datas+=","+obj.data[i].score+","+obj.data[i].rank;
					console.log(obj.data[i]);
					console.log('积分:' + obj.data[i].score + ',排名:' + obj.data[i].rank);
				}
			}else{
				console.log('获取失败')
			}
			SendMessage('API4399', 'getRankCallback',datas);
		}
		window.h5api.getRank(callback);
	},
		
		
	/**
	* 是否可以播放广告
	* @return boolean 是否可播放
	*/
	canPlayAd:function(){
		return window.h5api.canPlayAd();
	},
		
	/**
	* 播放全屏广告
	*/
	playAd:function(){
		function callback(obj){
			console.log('代码:' + obj.code + ',消息:' + obj.message)
			if(obj.code === 10000){
				console.log('开始播放')
			} else if(obj.code === 10001){
				console.log('播放结束')
			} else {
				console.log('广告异常')
			}
			SendMessage('API4399', 'playAdCallback',obj.code);
		}
		window.h5api.playAd(callback);
	}
};
mergeInto(LibraryManager.library, apiObject);