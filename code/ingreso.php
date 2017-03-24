<?php require_once('Connections/Base.php'); ?>
<?php
if (!function_exists("GetSQLValueString")) {
function GetSQLValueString($theValue, $theType, $theDefinedValue = "", $theNotDefinedValue = "")
{
  $theValue = get_magic_quotes_gpc() ? stripslashes($theValue) : $theValue;

  $theValue = function_exists("mysql_real_escape_string") ? mysql_real_escape_string($theValue) : mysql_escape_string($theValue);

  switch ($theType) {
    case "text":
      $theValue = ($theValue != "") ? "'" . $theValue . "'" : "NULL";
      break;
    case "long":
    case "int":
      $theValue = ($theValue != "") ? intval($theValue) : "NULL";
      break;
    case "double":
      $theValue = ($theValue != "") ? "'" . doubleval($theValue) . "'" : "NULL";
      break;
    case "date":
      $theValue = ($theValue != "") ? "'" . $theValue . "'" : "NULL";
      break;
    case "defined":
      $theValue = ($theValue != "") ? $theDefinedValue : $theNotDefinedValue;
      break;
  }
  return $theValue;
}
}

//TOdos los Tiempos
mysql_select_db($database_Base, $Base);
$query_TiemposID1 = "SELECT * FROM tiempos ORDER BY id ASC";
$TiemposID1 = mysql_query($query_TiemposID1, $Base) or die(mysql_error());
$row_TiemposID1 = mysql_fetch_assoc($TiemposID1);
$totalRows_TiemposID1 = mysql_num_rows($TiemposID1);

//Agregar a IDPLACAS
$editFormAction = $_SERVER['PHP_SELF'];
if (isset($_SERVER['QUERY_STRING'])) {
  $editFormAction .= "?" . htmlentities($_SERVER['QUERY_STRING']);
}

$cliente_act=$_POST['Cliente'];
if($cliente_act=="AhmsaCC2") {
    $clienteB1="Ahmsa-CC2";
}
else if($cliente_act=="AhmsaCC3") {
    $clienteB1="Ahmsa-CC3";
}
else {
    $clienteB1=$_POST['Cliente'];
}

$f_act=$_POST['Date']." ".$_POST['Hora'].":".$_POST['Min'];

if ((isset($_POST["MM_insert"])) && ($_POST["MM_insert"] == "form1")) {
  $insertSQL = sprintf("INSERT INTO idPlacas (Placa, Cliente, Detalle, Ingreso) VALUES (%s,%s,%s, %s)",
                       GetSQLValueString($_POST['Placa'], "text"),
                       GetSQLValueString($clienteB1, "text"),
                       GetSQLValueString($_POST['Detalle'], "text"),
                       GetSQLValueString($f_act , "date"));

  mysql_select_db($database_Base, $Base);
  $Result = mysql_query($insertSQL, $Base) or die(mysql_error());
//------------------------------------------------------------------------//
//Agregar  a Procesos
    $tant=$f_act;
 // strtotime("tomorrow ".$horaincio."am",$dia)

    do { //Por cada tiempos
        $tfin="17:00";
        $tini="8:00";
        $tproceso=$row_TiemposID1[$cliente_act];
        $proc=$row_TiemposID1['Proceso'];
        $suma=date('H:i', strtotime($tant)+3600*$tproceso);
   //Menor
        if($suma<$tfin){
            $menor=1;
            $tnuevo=date('Y-m-d H:i:s', strtotime($tant)+3600*$tproceso);

            $insertSQL1 = sprintf("INSERT INTO Procesos (Placa, Proceso, FEsperada_Ini, FEsperada_Fin) VALUES (%s, %s, %s, %s)",
                           GetSQLValueString($_POST['Placa'], "text"),
                           GetSQLValueString($proc,"text"),
                           GetSQLValueString($tant, "date"),
                            GetSQLValueString($tnuevo, "date"));

            mysql_select_db($database_Base, $Base);
            $Result1 = mysql_query($insertSQL1, $Base) or die(mysql_error());
            $tant=$tnuevo;
        }
   //Mayor
        else
        {
            //Sacar lo que queda de tiempo del proceso

           $insertSQL1 = sprintf("INSERT INTO Procesos (Placa, Proceso, FEsperada_Ini, FEsperada_Fin) VALUES (%s, %s, %s, %s)",
                           GetSQLValueString($_POST['Placa'], "text"),
                           GetSQLValueString($proc,"text"),
                           GetSQLValueString("2000-01-01 00:00", "date"),
                           GetSQLValueString("2000-01-01 00:00", "date"));
             mysql_select_db($database_Base, $Base);
            $Result1 = mysql_query($insertSQL1, $Base) or die(mysql_error());
        }

    } while ($row_TiemposID1 = mysql_fetch_assoc($TiemposID1));
//-------------------------------------
  $insertGoTo="placasactuales.php";
  if (isset($_SERVER['QUERY_STRING'])) {
    $insertGoTo .= (strpos($insertGoTo, '?')) ? "&" : "?";
    $insertGoTo .= $_SERVER['QUERY_STRING'];
  }
  header(sprintf("Location: %s", $insertGoTo));
}

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

<title>Ingreso de Placa</title>
<link type="text/css" href="themes/blitzer/ui.all.css" rel="stylesheet" />
<link REL=StyleSheet HREF="css/divisiones.css" TITLE="Divisiones" />
<link rel="shortcut icon" href="imagenes/kme.ico" type="image/x-icon"/>
<link rel="icon" href="imagenes/kme.ico" type="image/x-icon" />

<script type="text/javascript" src="js/jquery-1.3.2.min.js"> </script>
<script type="text/javascript" src="js/jquery-ui-1.7.2.custom.min.js"> </script>
<script type="text/javascript" src="js/ui/ui.core.js"></script>
<script type="text/javascript" src="js/ui/ui.datepicker.js"></script>
<script type="text/javascript" src="js/ui.datepicker-es.js"></script>
</head>

<body>
<p> <script type="text/javascript">
$(function() {
 $("#datepicker").datepicker(
 {showOn: 'button', buttonImage: 'imagenes/iconCalendar.gif', buttonText: 'Fecha',
     buttonImageOnly: true, dateFormat: 'yy-mm-dd', showButtonPanel: true});});
</script></p>

<div id="navbar">
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td>&nbsp;</td>
    </tr>
    <tr>
        <style type="text/css">
                 .mylink img{border:0}
        </style>
    <td><div align="center"><a class="mylink" href="index.php"><img src="imagenes/kme-1.jpg" width="98" height="42" /></a></div></td>
    </tr>
    <tr>
      <td>&nbsp;</td>
    </tr>
    <tr>
      <td><p><a href="ingreso.php">Ingreso </a></p></td>
    </tr>
    <tr>
      <td><p><a href="placasactuales.php">Detalles</a></p></td>
    </tr>
    <tr>
      <td><p><a href="tiempos.php">Tiempos</a></p></td>
    </tr>
     <tr>
      <td><p><a href="index.php">Gant</a></p></td>
    </tr>
  </table>
</div>

<div id="main">
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td><span class="titulos">INGRESO DE PLACA</span></td>
    </tr>
    <tr>
      <td>&nbsp;</td>
    </tr>
    <tr>
      <td>
          <form action="<?php echo $editFormAction; ?>" method="post" name="form1" id="form1">
        <table align="center">
            <tr valign="baseline">

              <td nowrap="nowrap" align="right"><span class="style4">Placa:</span></td>
              <td><input type="text" name="Placa" value="" size="32" />
              </td>
            </tr>

            <tr valign="baseline">
              <td nowrap="nowrap" align="right"><span class="style4">Cliente:</span></td>
              <td><select name="Cliente">
                  <option value="AhmsaCC2" <?php if (!(strcmp("AhmsaCC2", ""))) {echo "SELECTED";} ?>>Ahmsa CC-2</option>
                  <option value="AhmsaCC3" <?php if (!(strcmp("AhmsaCC3", ""))) {echo "SELECTED";} ?>>Ahmsa CC-3</option>
                  <option value="Hylsa" <?php if (!(strcmp("Hylsa", ""))) {echo "SELECTED";} ?>>Hylsa</option>
                  <option value="Mittal" <?php if (!(strcmp("Mittal", ""))) {echo "SELECTED";} ?>>Mittal</option>
                  <option value="Angostas" <?php if (!(strcmp("Angostas", ""))) {echo "SELECTED";} ?>> Angostas</option>
                </select></td>
            </tr>
            <tr valign="baseline">
              <td nowrap="nowrap" align="right"><span class="style4">Especificación:</span></td>
              <td><input type="text" name="Detalle" value="" size="32"/></td>
            </tr>
            <tr valign="baseline">
              <td nowrap="nowrap" align="right"><span class="style4">Fecha de Ingreso:</span></td>
              <td>
      <label>
      <input type="text" name="Date" id="datepicker" size="10"/>
      </label>
      <label class="style3"> H:
	  <select name="Hora" id="Hora">
	    <option value="00">00</option>
	    <option value="01">01</option>
	    <option value="02">02</option>
        <option value="03">03</option>
	    <option value="04">04</option>
	    <option value="05">05</option>
        <option value="06">06</option>
	    <option value="07">07</option>
	    <option value="08">08</option>
        <option value="09">09</option>
	    <option value="10">10</option>
	    <option value="11">11</option>
        <option value="12">12</option>
        <option value="13">13</option>
	    <option value="14">14</option>
	    <option value="15">15</option>
        <option value="16">16</option>
	    <option value="17">17</option>
	    <option value="18">18</option>
        <option value="19">19</option>
	    <option value="20">20</option>
        <option value="21">21</option>
        <option value="22">22</option>
        <option value="23">23</option>
	    <option value="24">24</option>
      </select>
	  </label>
      <label class="style3">M:
	  <select name="Min" id="Min">
	    <option value="00">00</option>
	    <option value="01">01</option>
	    <option value="02">02</option>
        <option value="03">03</option>
	    <option value="04">04</option>
	    <option value="05">05</option>
        <option value="06">06</option>
	    <option value="07">07</option>
	    <option value="08">08</option>
        <option value="09">09</option>
	    <option value="10">10</option>
	    <option value="11">11</option>
        <option value="12">12</option>
        <option value="13">13</option>
	    <option value="14">14</option>
	    <option value="15">15</option>
        <option value="16">16</option>
	    <option value="17">17</option>
	    <option value="18">18</option>
        <option value="19">19</option>
	    <option value="20">20</option>
        <option value="21">21</option>
        <option value="22">22</option>
        <option value="23">23</option>
	    <option value="24">24</option>
        <option value="25">25</option>
  	    <option value="26">26</option>
        <option value="27">27</option>
        <option value="28">28</option>
        <option value="29">29</option>
	    <option value="30">30</option>
	    <option value="31">31</option>
	    <option value="32">32</option>
        <option value="33">33</option>
	    <option value="34">34</option>
	    <option value="35">35</option>
        <option value="36">36</option>
	    <option value="37">37</option>
	    <option value="38">38</option>
        <option value="39">39</option>
	    <option value="40">40</option>
	    <option value="41">41</option>
        <option value="42">42</option>
        <option value="43">43</option>
	    <option value="44">44</option>
	    <option value="45">45</option>
        <option value="46">46</option>
	    <option value="47">47</option>
	    <option value="48">48</option>
        <option value="49">49</option>
	    <option value="50">50</option>
        <option value="51">51</option>
        <option value="52">52</option>
        <option value="53">53</option>
	    <option value="54">54</option>
        <option value="55">55</option>
        <option value="56">56</option>
        <option value="57">57</option>
        <option value="58">58</option>
	    <option value="59">59</option>
      </select>
	  </label></td>

            </tr>
            <tr valign="baseline">
              <td>&nbsp;</td>
              <td><input type="submit" value="Agregar Placa" /></td>

            </tr>
          </table>
          <input type="hidden" name="MM_insert" value="form1" />
      </form>
      </td>
    </tr>
  </table>
</div>
</body>
</html>

<?php
mysql_free_result($TiemposID1);
?>