In this file I will give a quick overview/walkthrough of the code samples I have provided.

These code files are part of previous web projects I developed, the full code base for these projects can be found in my Github at: 

[KMM Plates](https://github.com/adrifloresm/WebDev_KMM_Plates) and [Dear Architects](https://github.com/adrifloresm/WebDev_DearArchitects)

## KMM Plates Project

The KMM project is a web tool to auto-supervise the multi-month process of nickel coating of steel plates.
The system helped the company learn the real duration of each step, understand delays to perform delay management and cost recalculation of plates.

In this sample code I am including the following files: [index.php](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/index.php) ,
[ingreso.php](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/ingreso.php) , 
[placasactuales.php](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/placasactuales.php) and
[tiempos.php](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/tiempos.php)

(I apologize for the Spanish names and comments, this project was developed for a Mexican company.)

I first want to walk through the functionality of [ingreso.php](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/ingreso.php).
In this file the creation of a plate occurs. Here an HTML form allows the input of all the plate information required to calculate the processes the plate will go through and time calculation of each process.

In the form, there are entries for: steel plate name (Placa), client of the plate (Cliente) and start date (Fecha de Ingreso) this last one uses a JQuery date picker.
The key challenge in the entry of a plate is the calculation of process durations and storage of these in the MySQL database.

First the plate information (Name, Client, Start date) is input into the *idPlacas* database.

```PHP
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
```

Next, the process durations need to be calculated and stored in the *Procesos* database.

Here, we loop through each process taken by the plate, which is obtained by querying the pre-defined process durations database for the specific client (Note, each client plate undergoes different processes and times). 
After the duration for a given process is obtained, we calculate the start time and end time for this specific plate given the input start time. 
The trick here, is that we need to account for working hours (8am to 5pm), thus durations that do not fall within working hours must be transferred to the next day during working hours.
  All this is shown in the following code snippet.

```PHP
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
```

---

For In the rest of the files in this project I will provide a high level overview of their main goal.
  * In [index.php](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/index.php) 
we display a Gantt style chart of the stage of the existing plates.
  * In [placasactuales.php](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/placasactuales.php) 
we display the existing plates, with the option to delete one or all plates and clickable option to a detail view of a specific plate.
  * In [tiempos.php](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/tiempos.php) 
  the pre-defined times all clients can be edited. Here you can select to edit the specific duration of any process for any client.
  
---
  
Next to show an example CSS file, I uploaded the main [CSS](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/CSS.css)
file for the [Dear Architects](https://github.com/adrifloresm/WebDev_DearArchitects) project.
  
In this file you can find style settings for the dear architects website. In this project I created a dynamic website, with an admin portal for the architects to be able to add and remove projects, pages and photos. 
The css file is divided by the Menu styles, Index page styles, Project page styles and About us styles.
  
Please note that this CSS was made very statically, since the architects wanted a very specific look, thus not many dynamic/adaptive features where added. 
With new CSS features (This project was developed in 2011-2012) a much more adaptive CSS can be created. 
I have used more adaptive CSS features (per browser or mobile CSS) for other projects, but I do not have access to the code.
    
---
  
Last but not least, I added a sample [Python code](https://github.com/adrifloresm/hacker-application/blob/branch_adrianaflores/code/Assignment3_AppliedDS.ipynb) in Jupyter Notebook from my recent assignment from my Applied Data Science in Python certification course. 
In this submission I use Python Pandas, to clean data and respond to the different questions based on the 3 data sets.

---

Please note, the comments used in the previous web development projects were limited, since I was the only developer of the projects. 
However, when I develop code that will be shared or in a collaborative development, I try to comment as much as possible to make code easy to follow for other developers.

That concludes my submission, SparkFun! Thank you for the opportunity to share a little more about me and previous projects.    
