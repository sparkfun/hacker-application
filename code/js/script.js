/******************************
 ******************************
 ****** HireStarts, LLC. ******
 ******************************
 ******************************/

//ajax submit comments
$(document).ready(function(){
	var working = false;

	$('#commentForm').submit(function(e){
	
		if(working) return false;
		
		working = true;
		$('#submitComment').val('Working..');
		$('span.error').remove();

		$.post('/ajax/comment.process.php',$(this).serialize(),function(msg){

			working = false;
			$('#submitComment').val('Submit');
			console.log(msg);
			if(msg.status){
				
				var x = JSON.stringify(msg.html);
				$('#commentArea').html(x);
				$('#blogComment').val();
			
			} else {
				
				$.each(msg.errors,function(k,v){
					$('label[for='+k+']').append('<span class="error">'+v+'</span>');
				});
			}
		},'json');
	});
});


//ajax post job
$(document).ready(function(){
	var working = false;

	$('#addJobForm').submit(function(e){
	
		if(working) return false;
		
		working = true;
		$('#addJob').val('Working...');
		$('span.error').remove();

		$.post('/ajax/job.process.php',$(this).serialize(),function(msg){

			working = false;
			$('#addJob').val('Post Job');
			console.log(msg);
			if(msg.status){
				
				window.location('/opportunities');
			
			} else {
				
				$.each(msg.errors,function(k,v){
					$('label[for='+k+']').append('<span class="error">'+v+'</span>');
				});
			}
		},'json');
	});
});
 
 
/* Image Preview */
function PreviewImage() {
	oFReader = new FileReader();
	oFReader.readAsDataURL(document.getElementById("image_file").files[0]);

	oFReader.onload = function (oFREvent) {
		document.getElementById("uploadPreview").src = oFREvent.target.result;
	};
};

/* Client Side Form Validation */
function checkForm(form) {

	if(form.email.value == "") {
		alert("Error: Email cannot be blank!");
		form.email.focus();
		return false;
	}
	re = /[a-z0-9_.-]+)@([0-9a-z.-]+).([a-z.]{2,6}/;
	if(!re.test(form.email.value)) {
		alert("Error: Invalid email format!");
		form.email.focus();
		return false;
	}
	
    if(form.username.value == "") {
		alert("Error: Username cannot be blank!");
		form.username.focus();
		return false;
    }
    re = /^\w+$/;
    if(!re.test(form.username.value)) {
		alert("Error: Username must contain only letters, numbers and underscores!");
		form.username.focus();
		return false;
    }

    if(form.password1.value != "" && form.password1.value == form.pwd2.value) {
		if(form.password1.value.length < 6) {
			alert("Error: Password must contain at least six characters!");
			form.password1.focus();
			return false;
		}
		if(form.password1.value == form.username.value) {
			alert("Error: Password must be different from Username!");
			form.password1.focus();
			return false;
		}
		re = /[a-z]/;
		if(!re.test(form.password1.value)) {
			alert("Error: password must contain at least one lowercase letter (a-z)!");
			form.password1.focus();
			return false;
		}
		re = /[A-Z]/;
		if(!re.test(form.password1.value)) {
			alert("Error: password must contain at least one uppercase letter (A-Z)!");
			form.password1.focus();
			return false;
		}
    } else {
		alert("Error: Please check that you've entered and confirmed your password!");
		form.password1.focus();
		return false;
    };
    return true;
}