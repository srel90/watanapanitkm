// JavaScript Document
function readAndPreviewImgfile(input,control) {
	if (input.files && input.files[0]) {
		var reader = new FileReader();

		reader.onload = function(e) {
			$('#'+control).show();
			$('#'+control+' img').attr('src', e.target.result);
		}

		reader.readAsDataURL(input.files[0]);
	}
}
function decodeEntities(input) {
  var y = document.createElement('textarea');
  y.innerHTML = input;
  return y.value;
}
function ajax(url,data){
	return $.ajax({
        url: url,
        type: 'POST',
        data: data,
	async: false
    }).responseText;
}
function getRDOValue(radioName){   
    var rdo = document.getElementsByName(radioName);
    for(i=0;i < rdo.length;i++)
    {   
        if(rdo[i].checked) {return rdo[i].value;}
    }
    return null;
}
function setRDOValue(radioName,radioValue){  
    var rdo = document.getElementsByName(radioName);
    for(i=0;i < rdo.length;i++)
    {   
        if(rdo[i].value==radioValue)
		{
		rdo[i].checked = true;
		break;
		}
    }
    return null;
}
function setCHKValue(chkid,chkValue){ 
var chk = document.getElementById(chkid);
if(chk.value==chkValue.toString()) { chk.checked = true;}else{chk.checked = false;}
return null;
}
function setRDOValue(radioName,radioValue){ 
  
    var rdo = document.getElementsByName(radioName);
    for(i=0;i < rdo.length;i++)
    {   
        if(rdo[i].value==radioValue.toString()) { rdo[i].checked = true;}
    }
    return null;
}
function chkIDCard(value)
{
if(value.length != 13) return false;
for(i=0, sum=0; i < 12; i++)
sum += parseFloat(value.charAt(i))*(13-i); if((11-sum%11)%10!=parseFloat(value.charAt(12)))
return false; return true;
}
function _Redirect (url) {

    // IE8 and lower fix
    if (navigator.userAgent.match(/MSIE\s(?!9.0)/)) {
        var referLink = document.createElement('a');
        referLink.href = url;
        document.body.appendChild(referLink);
        referLink.click();
    } 

    // All other browsers
    else { window.location.href = url; }
}

