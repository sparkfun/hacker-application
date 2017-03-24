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
function identificaCol($colConClase,$numCol){
    if( $colConClase == $numCol )
        return "selecionado";
    else
        return "";
}

//Cambia el Tiempo
if ((isset($_POST["MM_update"])) && ($_POST["MM_update"] == "cambiart")) {
    $client=$_POST["Clientee"];
    $proce=$_POST["Procesoo"];
 $updateSQL = sprintf("UPDATE tiempos SET $client=%s WHERE Proceso=%s",
                       GetSQLValueString($_POST["tiemponew"], "text"),
                        GetSQLValueString($proce, "text"));

  mysql_select_db($database_Base, $Base);
  $Result1 = mysql_query($updateSQL, $Base) or die(mysql_error());

  $insertGoTo="tiempos2.php?C=".$client."&P=".$proce."&Tn=".$_POST["tiemponew"];
  header(sprintf("Location: %s", $insertGoTo));
}
//OBtiene info de Tiempo
$editFormAction = $_SERVER['PHP_SELF'];
if (isset($_SERVER['QUERY_STRING'])) {
  $editFormAction .= "?" . htmlentities($_SERVER['QUERY_STRING']);
}
if ((isset($_POST["MM_insert"])) && ($_POST["MM_insert"] == "form1")) {
$proc=$_POST['Proceso'];
mysql_select_db($database_Base, $Base);
$query_TiemposID = sprintf("SELECT * FROM tiempos WHERE Proceso = %s", GetSQLValueString($proc, "text"));
$TiemposID = mysql_query($query_TiemposID, $Base) or die(mysql_error());
$row_TiemposID = mysql_fetch_assoc($TiemposID);
$totalRows_TiemposID = mysql_num_rows($TiemposID);
}


//TOdos los Tiempos
mysql_select_db($database_Base, $Base);
$query_TiemposID1 = "SELECT * FROM tiempos ORDER BY id ASC";
$TiemposID1 = mysql_query($query_TiemposID1, $Base) or die(mysql_error());
$row_TiemposID1 = mysql_fetch_assoc($TiemposID1);
$totalRows_TiemposID1 = mysql_num_rows($TiemposID1);
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
 <div id="col1">
    <div id="header" class="titulos">
    Administracion de Tiempos
    </div>
    <div id="parte1">
    <form action="<?php echo $editFormAction;?>" method="post" name="form1" id="form1">
             <table align="left">
                <tr valign="baseline">
                  <td nowrap="nowrap" align="right"><span class="style4">Cliente:</span></td>
                  <td><select name="Cliente">
                      <option value="-" <?php if (!(strcmp("-", ""))) {echo "SELECTED";} ?>>-</option>
                      <option value="AhmsaCC2" <?php if (!(strcmp("Ahmsa CC-2", ""))) {echo "SELECTED";} ?>>Ahmsa CC-2</option>
                      <option value="AhmsaCC3" <?php if (!(strcmp("Ahmsa CC-3", ""))) {echo "SELECTED";} ?>>Ahmsa CC-3</option>
                      <option value="Hylsa" <?php if (!(strcmp("Hylsa", ""))) {echo "SELECTED";} ?>>Hylsa</option>
                      <option value="Mittal" <?php if (!(strcmp("Mittal", ""))) {echo "SELECTED";} ?>>Mittal</option>
                      <option value="Angostas" <?php if (!(strcmp("Angostas", ""))) {echo "SELECTED";} ?>> Angostas</option>
                    </select></td>
                </tr>
                  <tr valign="baseline">
                      <td nowrap="nowrap" align="right"><span class="style4">Proceso:</span></td>
                      <td>
                        <select name="Proceso">
                          <option value="-" <?php if (!(strcmp("-", ""))) {echo "SELECTED";} ?>>-</option>
                          <option value="1. Recepcion-Medicion y Pesado" <?php if (!(strcmp("1. Recepcion-Medicion y Pesado", ""))) {echo "SELECTED";} ?>>1. Recepcion-Medicion y Pesado</option>
                          <option value="2. Desengrasado" <?php if (!(strcmp("2. Desengrasado", ""))) {echo "SELECTED";} ?>>2. Desengrasado</option>
                          <option value="3. Enjuague y Sopleteado" <?php if (!(strcmp("3. Enjuague y Sopleteado", ""))) {echo "SELECTED";} ?>>3. Enjuague y Sopleteado</option>
                          <option value="4. Prelimpieza y enderezado" <?php if (!(strcmp("4. Prelimpieza y enderezado", ""))) {echo "SELECTED";} ?>>4. Prelimpieza y enderezado</option>
                          <option value="5. Refrescar cuerdas montar en dommy torqueo e inspeccion" <?php if (!(strcmp("5. Refrescar cuerdas montar en dommy torqueo e inspeccion", ""))) {echo "SELECTED";} ?>>5. Refrescar cuerdas montar en dommy torqueo e inspeccion</option>
                          <option value="6. Maquinado de Preparacion (Caja)" <?php if (!(strcmp("6. Maquinado de Preparacion (Caja)", ""))) {echo "SELECTED";} ?>>6. Maquinado de Preparacion (Caja)</option>
                          <option value="7. Desengrasado p/encintado" <?php if (!(strcmp("7. Desengrasado p/encintado", ""))) {echo "SELECTED";} ?>>7. Desengrasado p/encintado</option>
                          <option value="8. Enjuague y Sopleteado p/encintado" <?php if (!(strcmp("8. Enjuague y Sopleteado p/encintado", ""))) {echo "SELECTED";} ?>>8. Enjuague y Sopleteado p/encintado</option>
                          <option value="9. Limpieza y preparacion p/encintado" <?php if (!(strcmp("9. Limpieza y preparacion p/encintado", ""))) {echo "SELECTED";} ?>>9. Limpieza y preparacion p/encintado</option>
                          <option value="10. Encintado de Placas" <?php if (!(strcmp("10. Encintado de Placas", ""))) {echo "SELECTED";} ?>>10. Encintado de Placas</option>
                          <option value="11. Fabricacion de marcos tomacorriente" <?php if (!(strcmp("11. Fabricacion de marcos tomacorriente", ""))) {echo "SELECTED";} ?>>11. Fabricacion de marcos tomacorriente</option>
                          <option value="12. Ensamblado de Placas pintado y silicon" <?php if (!(strcmp("12. Ensamblado de Placas pintado y silicon", ""))) {echo "SELECTED";} ?>>12. Ensamblado de Placas pintado y silicon</option>
                          <option value="13. Activacion de placas" <?php if (!(strcmp("13. Activacion de placas", ""))) {echo "SELECTED";} ?>>13. Activacion de placas</option>
                          <option value="14. Niquelado ( Electrodepositacion )" <?php if (!(strcmp("14. Niquelado ( Electrodepositacion )", ""))) {echo "SELECTED";} ?>>14. Niquelado ( Electrodepositacion )</option>
                          <option value="15. Enjuague y Sopleteado" <?php if (!(strcmp("15. Enjuague y Sopleteado", ""))) {echo "SELECTED";} ?>>15. Enjuague y Sopleteado</option>
                          <option value="16. Desensamblar" <?php if (!(strcmp("16. Desensamblar", ""))) {echo "SELECTED";} ?>>16. Desensamblar</option>
                          <option value="17. Desencintado" <?php if (!(strcmp("17. Desencintado", ""))) {echo "SELECTED";} ?>>17. Desencintado</option>
                        <option value="18. Pesar Placas y Marcos (catodos)" <?php if (!(strcmp("18. Pesar Placas y Marcos (catodos)", ""))) {echo "SELECTED";} ?>>18. Pesar Placas y Marcos (catodos)</option>
                        <option value="19. Medicion de espesores y dureza" <?php if (!(strcmp("19. Medicion de espesores y dureza", ""))) {echo "SELECTED";} ?>>19. Medicion de espesores y dureza</option>
                        <option value="20. Maquinado de cantos y excesos de Ni." <?php if (!(strcmp("20. Maquinado de cantos y excesos de Ni.", ""))) {echo "SELECTED";} ?>>20. Maquinado de cantos y excesos de Ni.</option>
                        <option value="21. Maquinado Final" <?php if (!(strcmp("21. Maquinado Final", ""))) {echo "SELECTED";} ?>>21. Maquinado Final</option>
                        <option value="22. Pulido y Medicion con Lainometro" <?php if (!(strcmp("22. Pulido y Medicion con Lainometro", ""))) {echo "SELECTED";} ?>>22. Pulido y Medicion con Lainometro</option>
                        <option value="23. Medicion final de espesores y dureza" <?php if (!(strcmp("23. Medicion final de espesores y dureza", ""))) {echo "SELECTED";} ?>>23. Medicion final de espesores y dureza</option>
                        <option value="24. Pesado final de las placas" <?php if (!(strcmp("24. Pesado final de las placas", ""))) {echo "SELECTED";} ?>>24. Pesado final de las placas</option>
                        </select>
                      </td>
                    </tr>
            <tr valign="baseline">
                          <td>&nbsp;</td>
                          <td><input type="submit" value="Checar Tiempo" /></td>

                        </tr>
                      </table>
              <input type="hidden" name="MM_insert" value="form1" />
          </form>
    </div>
    <div id="parte2">
      <table border="0" cellspacing="0" cellpadding="2" align="left">
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                       </tr>
                      <?php
                      if ((isset($_POST["MM_insert"])) && ($_POST["MM_insert"] == "form1")) {
                        do {
                          $cl=$_POST['Cliente'];
                          $pr=$_POST['Proceso'];
                       ?>
                       <tr>
                           <td class="NegDer">Cliente: </td>
                           <td class="style3"><?php echo $cl; ?></td>
                       </tr>
                       <tr>
                            <td class="NegDer">Proceso: </td>
                           <td class="style3"><?php echo $pr?></td>
                       </tr>
                       <tr>
                            <td class="NegDer">Tiempo Actual: </td>
                            <td class="style3"><?php  printf("%10.2f", $row_TiemposID[$_POST['Cliente']]); ?></td>
                       </tr>
                <form action="<?php echo $editFormAction;?>" method="post" name="cambiart" id="cambiart">
                       <tr>
                           <td class="NegDer">Nuevo Tiempo: </td>

                            <td>
                                   <input type="text" name="tiemponew" value="" size="6"/>
                           </td>
                       </tr>
                       <tr>
                           <td colspan="2" align="center">
                               <input type="submit" value="Cambiar Tiempo"/>
                           </td>
                       </tr>
                          <input type="hidden" name="MM_update" value="cambiart" />
                          <input type="hidden" name="Clientee" value="<?php echo $cl;?>" />
                          <input type="hidden" name="Procesoo" value="<?php echo $pr;?>" />
                 </form>
                     <?php } while ($row_TiemposID = mysql_fetch_assoc($TiemposID)); }?>
                    </table>
    </div>
</div>
<div id="col2">
        <div id="header" class="titulos">
        Todos los Tiempos
        </div>
        <div id="parte1">
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
              if ($row_TiemposID1['Proceso']== "Total:")
                  {
                      $clase="style4";
                      $al="right";
                  }
                  else
                      {
                          $clase="style3";
                          $al="left";
                      }

                  if ((isset($_POST["MM_insert"])) && ($_POST["MM_insert"] == "form1")) {
                          $cl=$_POST['Cliente'];
                          $pr=$_POST['Proceso'];
                          $tp=$row_TiemposID[$_POST['Cliente']];

                       if($row_TiemposID1['Proceso']==$pr)
                      {
                          $clase2="selecionado";
                      }
                      else
                      {
                              $clase2="";
                      }

                       switch ($cl) {
                        case "AhmsaCC2":
                            $colACambiar=1;
                            break;
                        case "AhmsaCC3":
                            $colACambiar=2;
                            break;
                        case "Hylsa":
                            $colACambiar=3;
                            break;
                        case "Mittal":
                            $colACambiar=4;
                            break;
                        case "Angostas":
                            $colACambiar=5;
                            break;
                       }
                  }
              ?>

          <tr class="<?php echo $clase2;?>" >
            <td width="473" align="<?php echo $al;?>"class="<?php echo $clase; ?>">
            <?php echo $row_TiemposID1['Proceso']; ?>
            </td>
            <td width="117" height="20" class="<?php echo $clase; ?>">
                <div align="center" class="<?php echo identificaCol($colACambiar,1);?>">
                <?php printf("%10.2f", $row_TiemposID1['AhmsaCC2']); ?>
                </div></td>
            <td width="117" class="<?php echo $clase;?>">
                <div align="center" class="<?php echo identificaCol($colACambiar,2); ?>">
                <?php printf("%10.2f", $row_TiemposID1['AhmsaCC3']); ?>
                </div></td>
            <td width="117" class="<?php echo $clase; ?>">
                <div align="center" class="<?php echo identificaCol($colACambiar,3); ?>">
                <?php printf("%10.2f", $row_TiemposID1['Hylsa']); ?>
                </div></td>
            <td width="117" class="<?php echo $clase; ?>">
                <div align="center" class="<?php echo identificaCol($colACambiar,4); ?>">
                <?php printf("%10.2f", $row_TiemposID1['Mittal']); ?>
                </div></td>
            <td width="117" class="<?php echo $clase; ?>">
                <div align="center" class="<?php echo identificaCol($colACambiar,5); ?>">
                <?php printf("%10.2f", $row_TiemposID1['Angostas']); ?>
                </div></td>
          </tr>
         <?php } while ($row_TiemposID1 = mysql_fetch_assoc($TiemposID1)); ?>
        </table>
        </div>
    </div>
</div>
</body>
</html>
