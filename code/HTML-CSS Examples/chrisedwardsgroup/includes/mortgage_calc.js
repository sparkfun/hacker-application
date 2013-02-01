
var WGdc=".";  
var WGgc=",";
var WGnc="-";
var WGcs="";

function WGformatMoney(A,W) 
{  
// Author   : Jonathan Weesner (http://cyberstation.net/~jweesner/)
// Copyright: Use freely. Keep Author and Copyright lines intact.
	var N=Math.abs(Math.round(A*100));
   var S=((N<10)?"00":((N<100)?"0":""))+N;

   S=WGcs+((A<0)?WGnc:"")+WGgroup(S.substring(0,(S.length-2)))+WGdc+
      S.substring((S.length-2),S.length)+((A<0&&WGnc=="(")?")":"");
   return (S.length>W)?"Over":S;
}

// WGgroup inspired by Bill Dortch's usenet post (www.hidaho.com)
function WGgroup(S) 
{
   return (S.length<4)?S:(WGgroup(S.substring(0,S.length-3))+
      WGgc+S.substring(S.length-3,S.length));
}

function amtround(num)
{
	numrnd = 0;
	numrnd = num * 100;
	numrnd = Math.round(numrnd);
	temp1 = numrnd.toString(10);
	temp1n = temp1.length;
	numrnd = temp1.substring(0,temp1n-2) + "." + temp1.substring(temp1n-2,temp1n);	
	numrnd = parseFloat(numrnd);
}

function calc1(form) {
a = form.a.value*1;
a = a+form.b.value*1;
a = a+form.c.value*1;
a = a+form.d.value*1;
a = a-form.e.value*1;
a = a-form.f.value*1;
a = a-form.g.value*1;
amtround(a);
a = numrnd;
numrnd = WGformatMoney(numrnd, 12);	
form.Answer.value = numrnd;	
}

function calc2(form) {
z = form.z.value*.01;
z = z*(100-form.y.value)*1;

amtround(z);
z = numrnd;
numrnd = WGformatMoney(numrnd, 12);	
form.tax.value = numrnd;	
}
	
	
function calc(amt, rate, time) {
	var exp = -time;
      var j;

	i = rate;
	i = i / 100;
	i /= 12;
	
		if (time != 0)
{
		var term = 1;
		var diff = 1.0;
		var sum = 1.0;
		for (j = 1 ; j < 10; j++) {
			diff = diff * exp / j;
                  exp = exp - 1;
                  term = term * i;
                  sum = sum + diff * term;
                }
 		result = (amt * (1.0 - sum) / i*100)/100; }
}


function getpayment2(form)
{
	borrow22 = form.borrow22.value;
	bval = borrow22.split(",");
		if (bval.length > 1) {temp_value = bval[0] + bval[1];} else {temp_value = bval[0];}
		if (bval.length > 2) {temp_value = bval[0] + bval[1] + bval[2];}
		bval_n = parseFloat(temp_value);

	if ((form.borrow22.value != "") || (form.months22.value != "") || (form.rate22.value != "")) {
	calc(bval_n, form.rate22.value, form.months22.value);} else
	 {result = 0}
	scenario22 = result;
	amtround(scenario22);
	payment22 = numrnd;

	form.payment22.value = WGformatMoney(payment22, 12);
	//	form.payment22.value = borrow22;
}	

function calculate(amt, rate, time)
{
	var paymts = 0;
	var p1 = 0;
	var p2 = 0;
	var exp = time;

	i = rate;
	paymts = time;

	i = i / 100;
	i /= 12;
	
		if (paymts != 0)
{
      var p1 = amt * i; 
	   var p2 = 1 + i; 
		base = p2;
		var cnt = 1;
		for (j = 0 ; j < exp; j++) {
			cnt = cnt * p2;
		}
		p2 = 1 / cnt;
		p2 = 1 - p2; 
		result = p1 / p2; }
}

function getpayment(form)
{

	borrow1 = form.borrow1.value;
	bval = borrow1.split(",");
		if (bval.length > 1) {temp_value = bval[0] + bval[1];} else {temp_value = bval[0];}
		if (bval.length > 2) {temp_value = bval[0] + bval[1] + bval[2];}
		bval_n = parseFloat(temp_value);

	if ((form.borrow1.value != "") || (form.months1.value != "") || (form.rate1.value != "")) {
	calculate(bval_n, form.rate1.value, form.months1.value);} else
	 {result = 0}
	scenario1 = result;
	amtround(scenario1);
	payment1 = numrnd;
	//	form.payment1.value = numrnd;

	form.payment1.value = WGformatMoney(payment1, 12);
}

function MM_callJS(jsStr) { //v2.0
  return eval(jsStr)
}
