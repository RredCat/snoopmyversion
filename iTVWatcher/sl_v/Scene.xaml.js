if (!window.ClipSlides)
	window.ClipSlides = {};

ClipSlides.Scene = function(){
}

var isShowVisible=false;

ClipSlides.Scene.prototype =
{
	handleLoad: function(control, userContext, rootElement){
		this.control = control;
	}
}

function media_begin(sender, args) {
    sender.findName("mediaElement").play();
}
function media_pause(sender, args) {
    sender.findName("mediaElement").stop();
}
function media_volume_inc(sender, args) {
    sender.findName("mediaElement").volume += .1;
}
function media_volume_dec(sender, args) {
    sender.findName("mediaElement").volume -= .1;
}

function showMouseLeftButtonDown(sender, mouseEventArgs){   
	if(false==this.isShowVisible){
		sender.findName("showList").begin();
		sender.Opacity = 1;
	}
	else{
		sender.findName("hideList").begin();
		sender.Opacity = 0.5;
	}
	
	this.isShowVisible = !this.isShowVisible;
}

function textMouseLeftButtonDown(sender, mouseEventArgs){ 
	switch (sender.Name){
		case "textBox1" : 
			sender.findName("mediaElement").Source="mms://video.rfn.ru/rtr-planeta_64";
			break;
		case "textBox2" : 
			sender.findName("mediaElement").Source="mms://tv.gldn.net/rbc";
			break;
		case "textBox3" : 
			sender.findName("mediaElement").Source="http://www.utr.kiev.ua/UTR-US.asx";
			break;
		case "textBox4" : 
			sender.findName("mediaElement").Source="mms://rustavi.avstream.com/rustavilb";
			break;
		case "textBox5" : 
			sender.findName("mediaElement").Source="mms://70.87.8.31/miracle-sat";
			break;
		default : 
			alert("Source wasn't set for this item! "+sender.Text);
	}
}

function buttonMouseEnter(sender, mouseEventArgs){  
	sender.Opacity = 1;
}
function buttonMouseLeave(sender, mouseEventArgs){  
	sender.Opacity = .5;
}

function eventhandlerFunction(sender, mouseEventArgs){  
	sender.findName("mediaElement").stop();
	sender.findName("mediaElement").Source="";
	mouseEventArgs.Handler = true;
}