<?php
  function connect()
  {
    //Database
    define('db_host', 'YOUR_DB_HOST');
    define('db_name', 'YOUR_DB_NAME');
    define('db_username',  'YOUR_DB_USERNAME');
    define('db_password', 'YOUR_DB_PASSWORD');

	//Connect
    try
    {
      $dsn = 'mysql:host='.db_host.';dbname='.db_name.';charset=utf8';
      $dbh = new PDO($dsn, db_username, db_password);
      $dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    } 
	catch (PDOException $e)
    {
      exit('' . $e->GetMessage());
    }

    return $dbh;
  }
?>