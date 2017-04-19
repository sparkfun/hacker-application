function txtDescription_ClientValidate750(source, value){

    if(value.Value.length > 750) {

     value.IsValid = false;


    } else {

     value.IsValid = true;

    }

   }
			
			function txtDescription_ClientValidate500(source, value){

    if(value.Value.length > 500) {

     value.IsValid = false;


    } else {

     value.IsValid = true;

    }

   }
			
			function txtDescription_ClientValidate250(source, value){

    if(value.Value.length > 250) {

     value.IsValid = false;


    } else {

     value.IsValid = true;

    }

   }
			
			function txtDescription_ClientValidate150(source, value){

    if(value.Value.length > 150) {

     value.IsValid = false;


    } else {

     value.IsValid = true;

    }

   }
			
			function confirmDelete(source, value){
				if (confirm('Are you sure you want to delete this listing?')) {
					value.IsValid = true;
					}
				else {
					value.IsValid = false;
					} 
			}