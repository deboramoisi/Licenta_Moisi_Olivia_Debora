<?php
	header("Content-type:application/json");
	$Produs1 = array("ID"=>"P1","Pret"=>100,"Denumire"=>"Televizor");
	$Produs2 = array("ID"=>"P2","Pret"=>30,"Denumire"=>"Ipod");
	$Produse = array($Produs1,$Produs2);
	$Raspuns = array("Comanda"=>array("Client"=>"PopIon","Produse"=>$Produse));
	print json_encode($Raspuns);
?>

