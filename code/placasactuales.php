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
$editFormAction = $_SERVER['PHP_SELF'];

//Obtencion de datos de Placas
mysql_select_db($database_Base, $Base);
$query_PlacasID = "SELECT Cliente, Placa, Detalle, Ingreso FROM idPlacas ORDER BY Ingreso ASC";
$PlacasID = mysql_query($query_PlacasID, $Base) or die(mysql_error());
$row_PlacasID = mysql_fetch_assoc($PlacasID);
$totalRows_PlacasID = mysql_num_rows($PlacasID);

//Borrar una sola placa
if ((isset($_POST["MM_update"])) && ($_POST["MM_update"] == "submitform")) {
  $updateSQL = sprintf("DELETE FROM idPlacas WHERE Placa=%s and Cliente=%s ",
                       GetSQLValueString($_POST['Placa'], "text"),
                       GetSQLValueString($_POST['Cliente'], "text"));
  mysql_select_db($database_Base, $Base);
  $Result = mysql_query($updateSQL, $Base) or die(mysql_error());

  $updateSQL2 = sprintf("DELETE FROM Procesos WHERE Placa=%s ",
                       GetSQLValueString($_POST['Placa'], "text"));
  mysql_select_db($database_Base, $Base);
  $Result2 = mysql_query($updateSQL2, $Base) or die(mysql_error());

  header('Location: ' . $_SERVER['PHP_SELF']);
}
//Borrar todas
if ((isset($_POST["MM_update2"])) && ($_POST["MM_update2"] == "submitform2")) {
  $updateSQL3 = sprintf("DELETE FROM idPlacas");
  mysql_select_db($database_Base, $Base);
  $Result = mysql_query($updateSQL3, $Base) or die(mysql_error());

  $updateSQL4 = sprintf("DELETE FROM Procesos");
  
  $Result = mysql_query($updateSQL4, $Base) or die(mysql_error());
  header('Location: ' . $_SERVER['PHP_SELF']);
}
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Placas Actuales</title>
<LINK REL=StyleSheet HREF="css/divisiones.css" TITLE="Divisiones"/>
<link rel="shortcut icon" href="imagenes/kme.ico" type="image/x-icon"/>
<link rel="icon" href="imagenes/kme.ico" type="image/x-icon" />

<script language="JavaScript">
function confirmSubmit()
{
var agree=confirm("¿Seguro que quiere Borrar?");
if (agree)
	return true ;
else
	return false ;
}
</script>
</head>

<body>
<div id="navbar">
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td>&nbsp;</td>
    </tr>
    <tr>
    <td>
        <style type="text/css">
                 .mylink img{border:0}
        </style>
        <div align="center"><a class="mylink" href="index.php"><img src="imagenes/kme-1.jpg" width="98" height="42" /></a></div></td>
    </tr>
    <tr>
      <td>&nbsp;</td>
    </tr>
    <tr>
      <td><p><a href="ingreso.php">Ingreso</a> </p></td>
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

  <table border="0" cellspacing="0">
      <tr>
          <td colspan="10">
              &nbsp;
          </td>
      </tr>
      <tr>
      <td bordercolor="#000000"><span class="B16">Placa</span></td>
      <td width="1">&nbsp;</td>
      <td width="1">&nbsp;</td>
      <td bordercolor="#000000"><span class="B16">Especificaci&oacute;n</span></td>
      <td width="1">&nbsp;</td>
      <td width="1">&nbsp;</td>
       <td bordercolor="#000000"><span class="B16">Cliente</span></td>
      <td width="1">&nbsp;</td>
      <td width="1">&nbsp;</td>
      <td bordercolor="#000000"><span class="B16">Ingreso</span></td>
    </tr>
    <?php  do { ?>
      <tr>
      	<td ><span class="tituloPlacas"><a href="deatalles.php?Placa=<?php echo $row_PlacasID['Placa'];?>"><?php echo $row_PlacasID['Placa']; ?></a></span></td>
        <td width="1">&nbsp;</td>
        <td width="1">&nbsp;</td>
         <td><?php echo $row_PlacasID['Detalle']; ?></td>
       	<td width="1">&nbsp;</td>
        <td width="1">&nbsp;</td>
        <td><?php echo $row_PlacasID['Cliente']; ?></td>
       	<td width="1">&nbsp;</td>
        <td width="1">&nbsp;</td>
        <td>
        <?php
        if( $row_PlacasID['Placa']== NULL)
            {
            echo NULL;
            $boton="hidden";
            }
       else
           {
           echo $f_esp1=date('d/M/y  H:i', strtotime($row_PlacasID['Ingreso']));
           $boton="Submit";
           }
        ?>
        </td>
        <td width="1">&nbsp;</td>
        <td width="1">&nbsp;</td>
        <td>
            <form method="POST" action="<?php echo $editFormAction; ?>" id="submitform" name="submitform">
                <input type="<?php echo $boton; ?>" name="Delete" value="Delete" onClick="return confirmSubmit()" style="padding-left:17px;padding-right:17px;color:red;"/>
                <input type="hidden" name="MM_update" value="submitform" />
                <input type="hidden" name="Placa" value="<?php echo $row_PlacasID['Placa']; ?>" />
                <input type="hidden" name="Cliente" value="<?php echo $row_PlacasID['Cliente']; ?>" />
           </form>
        </td>
      </tr> 
      <?php } while ($row_PlacasID = mysql_fetch_assoc($PlacasID)); ?>

      <!--Delete ALL-->
      <tr>
      	<td ><span class="tituloPlacas"><a href="deatalles.php?Placa=<?php echo $row_PlacasID['Placa'];?>"><?php echo $row_PlacasID['Placa']; ?></a></span></td>
        <td width="1">&nbsp;</td>
        <td width="1">&nbsp;</td>
         <td><?php echo $row_PlacasID['Detalle']; ?></td>
       	<td width="1">&nbsp;</td>
        <td width="1">&nbsp;</td>
        <td><?php echo $row_PlacasID['Cliente']; ?></td>
       	<td width="1">&nbsp;</td>
        <td width="1">&nbsp;</td>
        <td></td>
        <td width="1">&nbsp;</td>
        <td width="1">&nbsp;</td>
        <td>
            <form method="POST" action="<?php echo $editFormAction; ?>" id="submitform2" name="submitform2">
                <input type="<?php echo $boton; ?>" name="DeleteAll" value="DeleteALL" onClick="return confirmSubmit()" STYLE="color:red"/>
                <input type="hidden" name="MM_update2" value="submitform2" />
           </form>
        </td>
      </tr>
  </table>
</div>


<?php
mysql_free_result($PlacasID);
?>
</body>
</html>
