// JavaScript Document
function tag(id){
      return typeof id=="string"?document.getElementById(id):id
}
	
function water(parent){
        var oparent=tag(parent);
             achild=oparent.getElementsByTagName("ul")[0].getElementsByTagName("li");
             awidth=achild[0].offsetWidth;
             cols=Math.floor((document.documentElement.clientWidth||document.body.clientWidth)/awidth);
             aHeight=[];
        for(var i=0;i<achild.length;i++){
            if(i<cols){
                achild[i].style.top=0;
                achild[i].style.left=i*awidth+"px";
                aHeight[i]=achild[i].offsetHeight;
            }
            else{
                var minHeight=Math.min.apply(null,aHeight);
                minlen=aHeight.indexOf(minHeight);
                achild[i].style.top=minHeight+"px";
                achild[i].style.left=minlen*awidth+"px";
                aHeight[minlen]+=achild[i].offsetHeight;
            }
        }
        var maxHeight=Math.max.apply(null,aHeight);
        oparent.style.height=maxHeight+"px";
        oparent.style.width=awidth*cols+"px"
}
//water("main");
//window.onresize=function(){water("main");}