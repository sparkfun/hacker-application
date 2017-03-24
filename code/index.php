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

mysql_select_db($database_Base, $Base);
$query_TiemposID = "SELECT * FROM tiempos ORDER BY id ASC";
$TiemposID = mysql_query($query_TiemposID, $Base) or die(mysql_error());
$row_TiemposID = mysql_fetch_assoc($TiemposID);
$totalRows_TiemposID = mysql_num_rows($TiemposID);
?>
<!--http://kmmreporte.hostzi.com/-->
<!--http://localhost/KMM_Placas/-->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>KME Control de Placas</title>
<link REL=StyleSheet HREF="css/divisiones.css" TITLE="Divisiones" />
<link rel="shortcut icon" href="imagenes/kme.ico" type="image/x-icon"/>
<link rel="icon" href="imagenes/kme.ico" type="image/x-icon" />
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
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="style4">Proceso</td>
    <td width="117" height="20" class="style4"><div align="center">Ahmsa CC-2</div></td>
    <td width="117" class="style4"><div align="center">Ahmsa CC-3</div></td>
    <td width="117" class="style4"><div align="center">Hylsa</div></td>
    <td width="117" class="style4"><div align="center">Mittal</div></td>
    <td width="117" class="style4"><div align="center">Angostas</div></td>
  </tr>
  <?php  do { 
      if ($row_TiemposID['Proceso']== "Total:")
          {
              $clase="style4";
              $al="right";
          }
          else
              {
                  $clase="style3";
                  $al="left";
              }
      ?>

  <tr>
    <td width="473" align="<?php echo $al;?>"><span class="<?php echo $clase; ?>"><?php echo $row_TiemposID['Proceso']; ?></span></td>
    <td width="117" height="20" class="<?php echo $clase; ?>"><div align="center"><?php printf("%10.2f", $row_TiemposID['AhmsaCC2']); ?></div></td>
    <td width="117" class="<?php echo $clase;?>"><div align="center"><?php printf("%10.2f", $row_TiemposID['AhmsaCC3']); ?></div></td>
    <td width="117" class="<?php echo $clase; ?>"><div align="center"><?php printf("%10.2f", $row_TiemposID['Hylsa']); ?></div></td>
    <td width="117" class="<?php echo $clase; ?>"><div align="center"><?php printf("%10.2f", $row_TiemposID['Mittal']); ?></div></td>
    <td width="117" class="<?php echo $clase; ?>"><div align="center"><?php printf("%10.2f", $row_TiemposID['Angostas']); ?></div></td>
  </tr>
 <?php } while ($row_TiemposID = mysql_fetch_assoc($TiemposID)); ?>
</table>
</div>

</body>
</html>

<?php
mysql_free_result($TiemposID);
?>