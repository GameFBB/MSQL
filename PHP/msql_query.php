<?php
  //Connect
  require_once('msql_connect.php');
  $dbh = connect();

  //Argument
  $method  = $_POST["method"];
  $table = $_POST["table"];
  $select = json_decode($_POST["select"], true);
  $where = $_POST["where"];
  $target = json_decode($_POST["target"], true);
  $operator = json_decode($_POST["operator"], true);
  $value = json_decode($_POST["value"], true);
  $field = $_POST["field"];
  $order = $_POST["order"];
  $limit = $_POST["limit"];

  //Query
  try
  {
    //SELECT
    $sql = 'SELECT ';

    if (count($select) == 0)
    {
      $sql .= '*';
    }
    else
    {
      for ($i = 0;  $i <= (count($select)-1); $i++)
      {
        if ($i > 0)
        {
          $sql .= ', ';
        }

        $sql .= $select[$i];
      }
    }

    //FROM
    $sql .= " FROM $table";

    //WHERE
    if ($where >= 1)
    {
      $sql .= ' WHERE ';

      for ($i = 0;  $i <= ($where-1); $i++)
      {
        if ($i > 0)
        {
          $sql .= ' AND ';
        }

        $sql .= $target[$i].$operator[$i].":value$i";
      }
    }

    //ORDER BY
    if ($field != '')
    {
      $sql .= " ORDER BY $field $order";
    }

    //LIMIT
    if ($limit != '')
    {
      $sql .= " LIMIT $limit";
    }

    //SetParameter
    $stmt = $dbh->prepare($sql);

    if ($where >= 1)
    {
      for ($i = 0;  $i <= ($where-1); $i++)
      {
        $stmt->bindParam(':value'.$i, $value[$i]);
      }
    }

    //Execute
    $stmt->execute();
    $result = $stmt->fetchAll(PDO::FETCH_ASSOC);  //AssociativeArray
  } 
  catch(Exception $e)
  {
    var_dump($e->getMessage());
  }

  //json → Unity
  if ($method == 'Find')
  {
    echo json_encode($result); 
  }
  else if($method == 'Count')
  {
    echo json_encode(count($result)); 
  }

  //Disconnect
  $stmt = null;
  $dbh = null;
?>