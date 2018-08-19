// JavaScript Document

var index={
    init:function(){
		this.shadow();
		this.picimg();
	},
	//鼠标移动到图片上
	shadow:function(){
		 var timg=$(".com-cont .top,.com-cont .recimg")
	     timg.hover(function(){
			 var tip=$(this).find("p");
		     $(this).find("i").stop().animate({opacity:".4"},200);
			 if(tip.length!=0){
			      tip.fadeIn(200);
			 }
		 },function(){
			 var tip=$(this).find("p");
		     $(this).find("i").stop().animate({opacity:"0"},200);
			 if(tip.length!=0){
			      tip.fadeOut(200);
			 }
		 })
	},
	//Instart特色
	picimg:function(){
        var _ul=$(".insliu ul");
        var lis=$(".insliu li");
		if(lis.length==0){
		   return;
		}
		var PW=$(window).width()*0.8;
	    var here=0;
		lis.css("width",Math.floor(PW*0.2));
        var lisW=lis.width();
        var len=lis.length;
		var herelink=window.location.href;
        _ul.css("width",lisW*len+"px");
	    //下一页
        $(".next").click(function(){
	    	here+=1;
	    	if(here+4==len){here=0}
	    	showpic(here)
        });
	    //上一页
        $(".per").click(function(){
	    	here-=1;
	    	if(here==-1){here=len-5}
	    	showpic(here)
        });
	
	    function showpic(here){
	        var now=-here*lisW;
	    	_ul.stop(true,false).animate({"left":now},300)
	    }
		lis.on("click",function(){
		    $(this).find("a").addClass("active");
			$(this).siblings().find("a").removeClass("active");
		})
	 }
}
index.init();



//页面滚动
$(window).scroll(function(){
   var tophere=$(".toprel")
   var scrT=$(window).scrollTop();
   if(scrT>0){
       tophere.addClass("topfix")
   }
   else{
       tophere.removeClass("topfix")
   }
})

