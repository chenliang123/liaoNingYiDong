var vm = new Vue({
    //指向绑定的element(元素)，之后该实例的属性和方法可以在对应元素内使用
    el: "#app",
    // 绑定的数据(即调用的变量)，可以实时监听变化
    data: {
    	stuList:"",     //班级学生人名列表
    	stuListNum:0,   //班级学生总人数
    	answerStuLists:[], //答题学生人名列表
    	answerStuNum:0, //答题学生人数
    	dadui:"",       //答对学生人名列表
    	dacuo:"",       //答错学生人名列表
    	noAnswerList:[], //未答学生人名列表
    	stuAnswerCount:[], //答题学生分类
    	askTimer:null,   //答题请求timer
        isFold: false,
        isShowTip:false,
        tiNum:1,
        tiNumAll:0,
        appId:'vke8u7u7y3',
	  	accesstoken:'RmHQtUNz+D1xaC1YuQZV2h3xN7PKK3UHQ8wak3KoNLs=',
	  	httpStr:'http://api.kuaxue.com',
        isAll:false,    //是否开启全体作答
        allInfor:false,
        isAllCount:false,
        allCount:false,
        allParse:false,
        isGroup:false,
        enableStart:false,
        groupStart:false,
        isStopAnswer:false,
        groupStopCard:false,
        Rank:false,
        groupData:[],
        selectData:[],
        selectAnswered:[],
        selectNoAnswer:[],
        isShowAddTip:false,
       	isShowDecTip:false,
       	Random:false,
       	randomRusult:false,
       	randomStuName:false,
       	randomName:"",
       	randomAnswer:false,
       	starArr:[{"light":false},{"light":false},{"light":false},{"light":false},{"light":false}],
        isVie:false,
        inVie:false,
        isVied:false,
        viaStuName:false,
        viaResult:false,
        viaName:"",
        isMul:false,
        mulTopicData:[],
        isMulPanel:false,
        isMulAnswer:false,
        mulInfor:false,
        mulConfirm:true,    //套卷模式下第一次弹出，以后不弹
        isMulConfirm:false,
        total:6,
        current:2,
        isMulCount:false,
        time:00,
        timer:null,
        leftTime:"",
        isQuize:false,
        topicData:[],                     //开始题的数据管理
        topicCurrent:{},
    },
    watch:{
//  	groupData:function(value){
//  		var num = 0;
//  		var total = 0;
//  		for(var item in value){
//        		value[item].member.forEach(function(val,index,arr){
//        			if(arr[index].isSelect){
//        				num--;
//        			}else{
//        				num++;
//        			}
//        			total++;
//        		});             	
//  		}
//  		if(total == num){
//  			this.enableStart = false;
//  		}else{
//  			this.enableStart = true;
//  		}
//  		console.log(this.enableStart)
//  	}
    },
    // 定义过滤方法,局部的，也可以定义全局的
    filters: {
    },
    // Vue1.0的ready方法->迁移为mounted
    mounted: function () {
        // 根据官方文档添加$nextTick钩子，以保证该vm已被实例加载
        this.$nextTick(function () {
//      	  this.$http.post('http://api.skyeducation.cn/EduApi_Test/pcgroupnet?action=addCompetitive',
//				        	  {
//				        	  	lessonid:"17245",
//				        	  	courseid:"12",
//				        	  	classid:"1878",
//				        	  	schoolid:"93",
//				        	  	teacherid:"7940",
//				        	  	ctime:"201712141433512",
//				        	  	duration:"15",
//				        	  	rid:"93-1878-201712141433512",
//				        	  	result:"15974:A",
//				        	  	testid:"2"
//				        	  },
//      	  				  {emulateJSON:true}).then(function (res) {
//					            	console.log(res)
//					            }, function (res) {
//					                console.log(res);
//					           });
//      	  this.$http.get("http://api.skyeducation.cn/EduApi_Test/pcgroupnet?action=addCompetitive&lessonid=17245&coureid=12&classid=1878&schoolid=93&teacherid=7940&ctime=201712141433512&duration=15&rid=93-1878-201712141433512&result=15974:A&testid=2").then(function (res) {
//      	  	console.log(res)
//      	  });
        	  
        	  
        	  this.mulConfirm = true;
              this.getData();
              this.stuList = window.external.getList();
              this.stuList = this.stuList.split("|");
              this.stuList.pop();
              this.stuListNum = this.stuList.length;
              for(var i = 0;i < this.stuListNum;i++){
              	this.stuList[i] = JSON.parse(this.stuList[i]);
              }
              clearInterval(this.askTimer);
//            _this = this;
//            this.askTimer = setInterval(function(){
//            	_this.answerStuLists =window.external.getAnswerList();
//            	if(_this.answerStuLists.length > 0){
//            		_this.answerStuLists = _this.answerStuLists.split("|");
//            	}
//            	_this.answerStuNum = _this.answerStuLists.length;
//            	_this.CountRightAnswer(_this.answerStuLists);
//            },1000);
        })
    },
    // 实例内要用到的所有方法,本地调取json文件失败
    methods: {
        getData: function () {
            this.$http.get("http://api.skyeducation.cn/EduApi_Test/pcgroupnet?action=getGroupResult&teacherid=7940&classid=1878&callback").then(function (res) {
            	this.groupData = res.body.data;
            	for(var item in this.groupData){
              		this.groupData[item].member.forEach(function(val,index,arr){
              			Vue.set(arr[index],'isSelect', false);
              			Vue.set(arr[index],'answer', "");
              			Vue.set(arr[index],'score', 0);
              		});
             }
            }, function (res) {
                alert("获取数据失败");
            });
            this.$http.get("http://api.skyeducation.cn/EduApi_Test/pcgroupnet?action=getTeacherQuestionList").then(function (res) {
            	this.mulTopicData = res.body.data;
            }, function (res) {
                alert("获取数据失败");
            });
            this.$http.get("http://api.skyeducation.cn/EduApi_Test/pcgroupnet?action=getTeacherFavoriteList&teacherid=7940&classid=1878&callback").then(function (res) {
				var _this = this;
				this.topicData = JSON.parse(res.bodyText);
				this.topicData = this.topicData.data.testids;
				this.topicData = this.topicData.split(",");
				console.log(this.topicData)
				this.tiNumAll = this.topicData.length;
				if(this.tiNumAll > 0){
					this.getById(this.topicData[0],function(data){
						data = JSON.parse(data.bodyText);
						data = data.datalist[0];
						_this.topicCurrent = data;
					});
//					this.topicCurrent = this.topicData[0];
				};
            }, function (res) {
                alert("获取数据失败");
            });
        },
        getById:function(id,callback){
		  		var that = this;
	  			this.$http.post('http://1.dltest.applinzi.com/kuaxuewang.php',
	  							{'appId':this.appId},
	  							{emulateJSON:true}
	  						  ).then(function(res) {
	                				var tempUrl = that.httpStr + "/KuaxueResource/getOneQuestion?sign="+ res.data +"&appid=vke8u7u7y3&tid="+id;
	            					that.ajaxGet(tempUrl,callback);
	  						  },function(res) {
									console.log(res.data);
	           				  });
		},
	  	ajaxGet:function(url,callback){
	  		this.$http.post("http://1.dltest.applinzi.com/get.php",
	  						{'url':url},
	  					    {emulateJSON:true}
	  					).then(function(res) {
									callback && callback(res);
	            				 }, function(res) {
	           					 });
	  	},
        CountRightAnswer:function(list){
        	var match = [];
        	var newAnswerList = [];
        	var temp = null;
        	var t;
        	if(list.length > 0){
        		for(var i = 0;i < list.length;i++){
        			temp = list[i].split(":");
        			that = this;
        			this.stuList.filter(function (e) {
        				if(e.cardid == temp[0]){
        					e.tanswer = temp[1];
        					newAnswerList.push(e);
        				}
			        });	
			        if(newAnswerList[newAnswerList.length-1].tanswer != null){
			        	newAnswerList[newAnswerList.length-1].tanswer = temp[1];
			        }
        		}
        		this.noAnswerList = this.stuList.filter(function (e) {
			        return e.tanswer == "";
			    });
        		this.answerStuLists = newAnswerList;
        		_this = this;
        		match = newAnswerList.filter(function (e) {
		            return e.tanswer === _this.topicCurrent.answer;
		        });
		        this.dadui = match;
				match = newAnswerList.filter(function (e) {
		            return e.tanswer !== _this.topicCurrent.answer;
		        });
		        this.dacuo = match;
        	}else{
        		this.noAnswerList = this.stuList;
        	}
        },
        initFn:function(){       	
        	this.Fold = false;
	        this.isShowTip = false;
//	        this.tiNum = 1;
//	        this.tiNumAll = 12;
	        this.isAll = false;    //是否开启全体作答
	        this.allInfor = false;
	        this.isAllCount = false;
	        this.allCount = false;
	        this.allParse = false;
	        this.isGroup = false;
	        this.enableStart = false;
	        this.groupStart = false;
	        this.isStopAnswer = false;
	        this.Rank = false;
	        this.selectData.filter(function(e){
				e.tanswer = "";
			});
	        this.selectData = [];
	        this.selectAnswered = [];
	        this.selectNoAnswer = [];
	        this.isShowAddTip = false;
	       	this.isShowDecTip = false;
	       	for(var item in this.groupData){
              		this.groupData[item].member.forEach(function(val,index,arr){
              			arr[index].isSelect= false;
              		});
            };
        	this.Random = false;
        	this.randomRusult = false;
        	this.randomStuName = false;
        	this.randomAnswer = false;
        	this.isVie = false;
        	this.inVie = false;
        	this.isVied = false;
        	this.isMulPanel = false;
        	this.isMulAnswer = false;
        	this.mulConfirm = true;
        	this.mulInfor = false;
        	this.isMulCount = false;
        	this.groupStopCard = false;
        	this.answerStuLists = [];
        },
        closeFn: function () {
            window.external.hideWeb();
        },
        foldFn: function () {
            this.isFold = !this.isFold;
        },
        showTipsFn: function () {
            this.isShowTip = !this.isShowTip;
            this.isMul = false;
            this.isQuize = false;
        },
        select: function(num){
        	var _this = this;
        	if(num > 0){
        		this.tiNum++;
        		if(this.tiNum > this.tiNumAll){
        			this.tiNum = this.tiNumAll;
        		}else{
        			this.getById(this.topicData[this.tiNum - 1],function(data){
						data = JSON.parse(data.bodyText);
						data = data.datalist[0];
						_this.topicCurrent = data;
					});
        		}
        	}else{
        		this.tiNum--;
        		if(this.tiNum < 1){
        			this.tiNum = 1;
        		}else{
        			this.getById(this.topicData[this.tiNum - 1],function(data){
						data = JSON.parse(data.bodyText);
						data = data.datalist[0];
						_this.topicCurrent = data;
					});
        		}
        	};
//      	this.homeFn();
//      	window.external.recieveData(this.topicCurrent.answer,this.topicCurrent.topicType);
        },
        allAnswerFn:function(){
        	window.external.recieveData(this.topicCurrent.answer,this.topicCurrent.type);
        	this.isAll = true;
        	clearInterval(this.askTimer);
            _this = this;
            this.askTimer = setInterval(function(){
          	    _this.answerStuLists =window.external.getAnswerList();
          		if(_this.answerStuLists.length > 0){
          			_this.answerStuLists = _this.answerStuLists.split("|");
          		}
          		_this.answerStuNum = _this.answerStuLists.length;
          		_this.CountRightAnswer(_this.answerStuLists);
          	},1000);
        },
        groupAnswerFn:function(){
        	this.isGroup = true;
        },       
        trait:function(flag){
        	if(flag == 0){
        		this.isAll = false;
        		this.isShowTip = !this.isShowTip;
        	}
        },
        allInforFn:function(){
        	this.allInfor = true;
        	this.isAllCount = false;
        },
        isAllCountFn:function(){
        	this.isAllCount = !this.isAllCount;
        	this.allInfor = false;
        },
        allContinueFn:function(){
        	this.isAllCount  = !this.isAllCount;
        },
        allCloseFn:function(){
        	var myChart = echarts.init(document.getElementById('main'));
	        var	option = {
				    legend: {
				        orient: 'vertical',
				        left: 'left',
				        show:false,
				    },
				    series : [
				        {
				            name: '访问来源',
				            type: 'pie',
				            radius : '45%',
				            center: ['50%', '50%'],
				            data:[
				                {
				                	value:this.dadui.length,
				                	name:'正确:'+this.dadui.length+'人',
				                	itemStyle: {
								        normal: {
								            color: '#00C853'
								        }
								    }
				                },
				                {
				                	value:this.dacuo.length, 
				                	name:'错误:'+this.dacuo.length+'人',
				                	itemStyle: {
								        normal: {
								            color: '#FB6D74'
								        }
								    }
				                },
				            ],
				            itemStyle: {
				                emphasis: {
				                    shadowBlur: 10,
				                    shadowOffsetX: 0,
				                    shadowColor: 'rgba(0, 0, 0, 0.5)'
				                }
				            }
				        }
				    ]
				};
        	myChart.setOption(option);
        	this.isAllCount  = !this.isAllCount;
        	this.allCount = !this.allCount;
        	this.toCountFn();
        	window.external.extiAnswer();
        },
        toCountFn:function(){
        	var stuAnswerString = ',';
        	this.stuAnswerCount = [];
        	_this = this;
        	if(this.answerStuLists.length > 0){
	        	this.answerStuLists.forEach(function(v,i){
	        		var flag = true;
	        		if(_this.stuAnswerCount.length == 0) {
							//声明变量
							_this.stuAnswerCount[0] = [];
							_this.stuAnswerCount[0][0] = v;
							stuAnswerString += v.tanswer + ','
						} else {
							//遍历如果答案相同那么久放入同一个数组；如果答案不同那么新建数组
							for(var k = 0; k < _this.stuAnswerCount.length; k++) {
								if(_this.stuAnswerCount[k][0].tanswer == v.tanswer) {
									_this.stuAnswerCount[k][_this.stuAnswerCount[k].length] = v;
									flag = false
								}
							}
							if(flag) {
								_this.stuAnswerCount[_this.stuAnswerCount.length] = []
								_this.stuAnswerCount[_this.stuAnswerCount.length - 1][0] = v;
								stuAnswerString += v.tanswer + ','
							}
						}
	        	});
        	}
        },
        allParseFn:function(){
        	this.allParse = !this.allParse;
        	this.isStopAnswer = !this.isStopAnswer;        	
        },
        mulParseFn:function(){
        	this.allParse = !this.allParse;
        	this.isMulCount = false;
        },
        panelCloseFn:function(){
        	this.allInfor = false;
//      	this.allCount = false;
        	this.allParse = false;
        	this.isStopAnswer = false;
//      	this.Rank = false;
        	this.randomRusult = false;
        	this.randomStuName = false;
//      	this.inVie = false;
//      	this.isVied = false;
        	this.isMulCount = false;
        },
        homeFn:function(){
//      	var temp = window.external.extiAnswer();
//      	if(temp >= 0){
//      		window.external.getScreen();
				this.initFn();
				clearInterval(this.askTimer);
//			};
        },
        groupStartFn:function(){
        	if(this.groupStart){
        		console.log(1111111)   
        		this.selectCount();
        		this.isStopAnswer = true;
        		this.groupStopCard = true;
        	};
        	if(this.enableStart){
        		console.log(22222222)
	        	this.groupStart = true;
	        	this.Rank = false;
        	};
        	if(!this.groupStopCard){
        		console.log(3333333)
	        	 window.external.recieveData(this.topicCurrent.answer,this.topicCurrent.type);
	        	 clearInterval(this.askTimer);
	              _this = this;
	              this.askTimer = setInterval(function(){
	              	_this.answerStuLists =window.external.getAnswerList();
	              	if(_this.answerStuLists.length > 0){
	              		_this.answerStuLists = _this.answerStuLists.split("|");
	              		for(var i = 0;i < _this.answerStuLists.length;i++){
		        			temp = _this.answerStuLists[i].split(":");
		        			_this.selectData.filter(function(e){
		        				if(temp[0] == e.cardID){
		        					e.tanswer = temp[1];
		        				}
		        			});							
		        		}
	              		
	              		_this.selectCount();
	              	}
	              },1000);
        	}
        },
        selectCount:function(){
        	_this = this;
        	this.selectAnswered = [];
        	this.selectNoAnswer = [];
        	this.selectData.filter(function(e){
				if(e.tanswer){
					_this.selectAnswered.push(e);
				}else{
					_this.selectNoAnswer.push(e);
				}
			});	
        },
        addScoreFn:function(item){
        	item.score++;
        },
        isRankFn:function(){
        	clearInterval(this.askTimer);
        	this.panelCloseFn();
        	this.Rank = !this.Rank;
//      	this.groupStart = false;
			this.selectData.sort(function(a,b){
            					return b.score-a.score
							});
        },
        selectAnswerFn:function(item){
        	this.selectData = [];
        	item.isSelect = !item.isSelect;
        	_this = this;
        	this.groupData.filter(function(e){
        		e.member.filter(function(sub){
        			if(sub.isSelect){
        				sub.score = 0;
        				_this.selectData.push(sub);
        			}
        		});
        	});

//      	var num = 0;
//      	var len = this.selectData.length;
//      	for(var i = 0; i < len;i++){
//      		if(this.selectData[i].cardID == item.cardID){
//      			if(!item.isSelect){
//      				this.removeByValuePb(this.selectData,item);
//      				break;
//      			}else{}
//      		}else{
//      			num++;
//      			this.selectData.push(item);
//      		} 
//      	}
//      	if(num == len){
//      		if(item.isSelect){
//      			this.selectData.push(item);
//      		}
//      	}
//      	if(item.isSelect && !this.selectData.length){
//      		this.selectData.push(item);
//      	};
        	if(this.selectData.length){
        		this.enableStart = true;
        	}else{
        		this.enableStart = false;
        	}
        },
        showTipFn:function(item,flag){
        	if(flag > 0){
        		item.score++;
        		item.isShowAddTip = !item.isShowAddTip;
        		_this = this;
        		setTimeout(function(){
        			item.isShowAddTip = !item.isShowAddTip;
        		},1500);
        	}else{
        		if(item.score){
        			item.score--;
        		}else{
        			item.score = 0;
        		}        		
        		item.isShowDecTip = !item.isShowDecTip;
        		_this = this;
        		setTimeout(function(){
        			item.isShowDecTip = !item.isShowDecTip;
        		},1500);
        	}
        },       
        viaFn:function(){
        	this.isVie = true;
        	this.inVie = true;
        	this.isVied = false;
//      	this.viaName = {'name':'李明','tanswer':'AB'};
//      	this.isVie = true;
//      	this.inVie = true;
//      	this.isVied = false;
        	this.starArr.forEach(function(item){
        		item.light = false;
        	});
        	window.external.recieveData(this.topicCurrent.answer,this.topicCurrent.type);
        	clearInterval(this.askTimer);
            _this = this;
            this.askTimer = setInterval(function(){
              	_this.answerStuLists =window.external.getAnswerList();
              	if(_this.answerStuLists.length > 0){
              		_this.answerStuLists = _this.answerStuLists.split("|");
	        		temp = _this.answerStuLists[0].split(":");
					_this.stuList.filter(function (e) {
						if(e.cardid == temp[0]){
							_this.viaName = e;
						}
        			});
				    _this.viaName.tanswer = temp[1];
				    _this.inVie = false;
				    _this.isVied = true;
					clearInterval(_this.askTimer);
              	}
            },1000);
        },
        rewardFn:function(flag){
        	this.starArr.forEach(function(item){
        		item.light = false;
        	});
			this.starArr.forEach(function(item,index){
        		if(index <= flag){
        			item.light = true;
        		}
        	});
        },
        RandomFn:function(){
        	this.Random = true;
        	this.startRandom();
        },
        startRandom:function(){
        	this.randomRusult = true;
        	this.randomStuName = true;
        	this.randomAnswer = false;
        	this.allParse = false;
//      	this.randomName={'name':'李明','tanswer':'AB'};
        	
        	this.starArr.forEach(function(item){
        		item.light = false;
        	});       	
        	var num = Math.floor(Math.random()*this.stuList.length);
        	var num = Math.floor(Math.random()*3);
        	this.randomName = this.stuList[num];
//      	this.randomRusult = true;
//      	this.randomStuName = true;
//      	this.allParse = false;
        	window.external.recieveData(this.topicCurrent.answer,this.topicCurrent.type);
        	 clearInterval(this.askTimer);
              _this = this;
              this.askTimer = setInterval(function(){
              	_this.answerStuLists =window.external.getAnswerList();
              	if(_this.answerStuLists.length > 0){
              		_this.answerStuLists = _this.answerStuLists.split("|");
              		for(var i = 0;i < _this.answerStuLists.length;i++){
	        			temp = _this.answerStuLists[i].split(":");
						if(temp[0] == _this.randomName.cardid){
							_this.randomName.tanswer = temp[1];
							_this.randomStuName = false;
							_this.randomAnswer = true;
							clearInterval(_this.askTimer);
						}
	        		}
              	}
              },1000);
        },
        randomParseFn:function(){
        	 this.allParse = true;
//      	 this.randomRusult = false;
//      	 this.inVie = false;
//      	 this.isVied = false;
        },
        startVieFn:function(){
        	this.starArr.forEach(function(item){
        		item.light = false;
        	});
        	this.inVie = true;
        	this.isVied = false;
        	window.external.recieveData(this.topicCurrent.answer,this.topicCurrent.type);
        	clearInterval(this.askTimer);
            _this = this;
            this.askTimer = setInterval(function(){
              	_this.answerStuLists =window.external.getAnswerList();
              	if(_this.answerStuLists.length > 0){
              		_this.answerStuLists = _this.answerStuLists.split("|");
	        		temp = _this.answerStuLists[0].split(":");
					_this.stuList.filter(function (e) {
						if(e.cardid == temp[0]){
							_this.viaName = e;
						}
        			});
				    _this.viaName.tanswer = temp[1];
				    _this.inVie = false;
				    _this.isVied = true;				    
					clearInterval(_this.askTimer);
              	}
            },1000);
        },
        mulStartFn:function(){
//      	window.external.
        	var num = this.time*60;
			if(num > 0){
	        	this.leftTime = this.toTwoFn(parseInt(num / 60 % 60, 10)) + ":" + this.toTwoFn(parseInt(num % 60, 10));
	        	this.isMulPanel = false;
	        	this.isMulAnswer = true;
	        	clearInterval(this.timer);       	
	        	_this = this;
	        	this.timer = setInterval(function(){
	        		num--;
	        		var minutes = parseInt(num / 60 % 60, 10);//计算剩余的分钟 
	 			 	var seconds = parseInt(num % 60, 10);//计算剩余的秒数 
	 			 	minutes = _this.toTwoFn(minutes);
	 			 	seconds = _this.toTwoFn(seconds);
	 			 	_this.leftTime = minutes + ":" + seconds;
	 			 	if(num <= 0){
	 			 		_this.leftTime = "00:00";
	 			 		_this.mulInfor = true;
	 			 		_this.mulConfirm = false;
	 			 		clearInterval(_this.timer);
	 			 	}
	        	},1000);
			}        	
        },
        toTwoFn:function(n){
			return n < 10 ?  '0' + n : '' + n;
        },
        mulInforFn:function(){
        	 if(!this.mulInfor){
        	 	clearInterval(this.timer);
        	 };
        	 this.mulInfor = true;
        },
        mulPreFn:function(){
        	if(this.current > 1){
        		this.current--
        	}else{
        		this.current = 1;
        	}        	
        },
        mulNextFn:function(){
        	if(this.current >= this.total){
        		this.current = this.total;
        	}else{
        		this.current++;
        	} 
        },
        isMulCountFn:function(){
//      	this.isMulCount = true;
//      	this.mulInfor = false;
			if(this.mulConfirm){
				this.isMulConfirm = true;
			}else{
				this.isMulCount = true;
				this.mulInfor = false;
			}
        },
        mulContinueFn:function(){
        	this.isMulConfirm = false;
        },
        mulCloseFn:function(){
        	this.mulConfirm = false;
        	this.isMulCount = true;
			this.mulInfor = false;
			this.isMulConfirm = false;
			clearInterval(this.timer);
        },
        showMulFn:function(){
        	this.isMul = !this.isMul;
        	this.isShowTip = false;
        	this.isQuize = false;
        },
        mulBtnStartFn:function(item){
			var _this = this;
        	this.topicData = item.content;
        	this.topicData = this.topicData.split(",");
//      	this.topicCurrent = this.topicData[0];
        	this.tiNum = 1;
        	this.tiNumAll = this.topicData.length;
        	if(this.tiNumAll > 0){
					this.getById(this.topicData[0],function(data){
						data = JSON.parse(data.bodyText);
						data = data.datalist[0];
						_this.topicCurrent = data;
					});
			};
        	this.isMul = false;
        	this.isMulPanel = true;
        	this.isQuize = false;
        },
        mulHomeFn:function(){
//      	var temp = window.external.extiAnswer();
//      	if(temp >= 0){
//				this.initFn();
//				clearInterval(this.askTimer);
//			};
			this.$http.get("http://api.skyeducation.cn/EduApi_Test/pcgroupnet?action=getTeacherFavoriteList&teacherid=7940&classid=1878&callback").then(function (res) {
				var _this = this;
				this.topicData = JSON.parse(res.bodyText);
				this.topicData = this.topicData.data.testids;
				this.topicData = this.topicData.split(",");
				this.tiNumAll = this.topicData.length;
				this.tiNum = 1;
				if(this.tiNumAll > 0){
					this.getById(this.topicData[0],function(data){
						data = JSON.parse(data.bodyText);
						data = data.datalist[0];
						_this.topicCurrent = data;
					});
				};
				this.initFn();
				clearInterval(this.askTimer);
            }, function (res) {
                alert("获取数据失败");
            });
        },
        showQuizeFn:function(){
        	this.isQuize = !this.isQuize;
        	this.isShowTip = false;
        	this.isMul = false;
        }
    }
});

Vue.prototype.removeByValuePb = function (arr, val){
  for(var i=0; i<arr.length; i++) {
	    if(arr[i] == val) {
	      arr.splice(i, 1);
	      break;
	    }
    }
}