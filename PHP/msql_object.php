<?php
  //Connect
  require_once('msql_connect.php');
  $dbh = connect();

  //Argument
  $method  = $_POST["method"];
  $table = $_POST["table"];
  $target  = $_POST["target"];
  $targetValue = $_POST["targetValue"];
  $field = json_decode($_POST["field"], true); 
  $value = json_decode($_POST["value"], true);
  $result = 0;

  //Update, Save, Delete
  try 
  {
    //1.Update
    if ($method == 'Update')
    {
      //Search
      $sql = "SELECT * FROM $table WHERE $target = :$target";
      $stmt = $dbh->prepare($sql);
      $stmt->bindParam(":$target", $targetValue);
      $stmt->execute();
      $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
      
      //NotExist
      if (count($result) == 0)
      {
        $method = 'Insert';
        array_unshift($field, $target);
        array_unshift($value, $targetValue);
      }
      //Exist
      else if(count($result) == 1)
      {        
        //UPDATE
        $sql = "UPDATE $table";
        
        //SET
        $sql .= " SET ";
        
        for ($i = 0;  $i < count($field); $i++)
        {
          if ($i > 0)
          {
            $sql .= ', ';
          }
          $sql .= "$field[$i] = :$field[$i]";
        }
        
        //WHERE
        $sql .= " WHERE $target = :$target";
        
        //SetParameter
        $stmt = $dbh->prepare($sql);
        
        for ($i = 0;  $i < count($field); $i++)
        {
          $stmt->bindParam(":$field[$i]", $value[$i]);
        }
        
        $stmt->bindParam(":$target", $targetValue);
        
        //Execute
        $stmt->execute();
      }
    }
    
    //2.Insert
    if ($method == 'Insert')
    {      
      //INSERT INTO      
      $sql = "INSERT INTO $table (";
      
      for ($i = 0;  $i < count($field); $i++)
      {
        if ($i > 0)
        {
          $sql .= ', ';
        }
        $sql .= $field[$i];
      }
      $sql .= ')';
      
      //VALUES
      $sql .= ' VALUES (';
      
      for ($i = 0;  $i < count($field); $i++)
      {
        if ($i > 0)
        {
          $sql .= ', ';
        }
        $sql .= ":$field[$i]";
      }
      $sql .= ')';
      
      //SetParameter
      $stmt = $dbh->prepare($sql);
      
      for ($i = 0;  $i < count($field); $i++)
      {
        $stmt->bindParam(":$field[$i]", $value[$i]);
      }
      
      //Execute
      $stmt->execute();
    }
    
    //3.Delete
    else if ($method == 'Delete')
    {
      $sql = "DELETE FROM $table WHERE $target = :$target";
      $stmt = $dbh->prepare($sql);
      $params = array(":$target"=>$targetValue);
      $stmt->execute($params);
    }
  } 
  catch (PDOException $e)
  {
    var_dump($e->getMessage());
  }
  
  //Disconnect
  $stmt = null;
  $dbh = null;
  
  echo $sql;  
?>