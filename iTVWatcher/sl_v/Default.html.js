function createSilverlight()
{
	var scene = new ClipSlides.Scene();
	Sys.Silverlight.createObjectEx({
		source: "Scene.xaml",
		parentElement: document.getElementById("SilverlightControlHost"),
		id: "SilverlightControl",
		properties: {
			width: "100%",
			height: "100%",
			version: "0.9"
		},
		events: {
			onLoad: Sys.Silverlight.createDelegate(scene, scene.handleLoad)
		}
	});
}

if (!window.Sys)
	window.Sys = {};
	
if (!window.Silverlight) 
	window.Silverlight = {};

Sys.Silverlight.createDelegate = function(instance, method) {
	return function() {
        return method.apply(instance, arguments);
    }
}