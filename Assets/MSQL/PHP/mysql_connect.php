<?php
  function connect()
  {
    // Database
    define('DatabaseHost', 'YOUR_DATABASE_HOST');
    define('DatabaseName', 'YOUR_DATABASE_NAME');
    define('UserName', 'YOUR_USERNAME');
    define('Password', 'YOUR_PASSWORD');

    // Connect
    try
    {
      $dsn = 'mysql:host=' . DatabaseHost . ';dbname=' . DatabaseName . ';charset=utf8';
      $dbh = new PDO($dsn, UserName, Password);
      $dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    }
    catch (PDOException $e)
    {
      exit('' . $e->GetMessage());
    }

    return $dbh;
  }
?>