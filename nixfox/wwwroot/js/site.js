var urlInput = document.querySelector("#urlshort");
var submitBtn = document.querySelector("#submit"); 
var respArea = document.querySelector("#resp-area");

urlInput.addEventListener('input', function(ev){
    //TODO: add on change event logic
})

submitBtn.onclick = function(ev){
    if(!this.classList.contains("copy")){
        let url = urlInput.value;
        if(!validateURL(url)){
            // TODO: let the user know what was wrong
            RespArea("Does not appear to be a valid URL");
            return null;
        }
        fetch("/",{
            method:"POST",
            body: JSON.stringify(url), 
            headers:{
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
        .then(response => {
			if (response.status === "URL already exists") {
				urlInput.value = new URL(window.location) + response.token;
				submitBtn.innerHTML = "Copy";
				submitBtn.classList.add("copy");
			}
			else if (response.status === "already shortened") {
				urlInput.value = "";
				RespArea("That link has already been shortened by nixfox.de");
            }else{
                console.log(response);
                urlInput.value = new URL(window.location)+response;
                submitBtn.innerHTML = "Copy";
                submitBtn.classList.add("copy");
            }
            
        }).catch(error=> console.log(error));
    }else{
        urlInput.select();
        document.execCommand("copy");
    }
}

function validateURL(url){
    var pattern = new RegExp('^(https?:\\/\\/)?'+ 
    '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|'+ 
    '((\\d{1,3}\\.){3}\\d{1,3}))'+ 
    '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*'+
    '(\\?[;&a-z\\d%_.~+=-]*)?'+ 
    '(\\#[-a-z\\d_]*)?$','i'); 
  return !!pattern.test(url);
}

respArea.addEventListener("mouseout", function(ev){
    setTimeout(() => {
        respArea.classList.remove("active");
        respArea.classList.add("inactive");
    }, 1000);
    
})

respArea.onclick = function(ev){
    respArea.classList.remove("active");
    respArea.classList.add("inactive");
}

function RespArea(text){
    respArea.classList.remove("inactive");
    respArea.classList.add("active");
    respArea.innerHTML = text;
}