;WITH person AS --remove duplicate
(
  SELECT first_name,last_name, rn = ROW_NUMBER() OVER 
      (PARTITION BY first_name,last_name ORDER BY id)
  FROM dbo.person
)
DELETE person WHERE rn > 1;
GO
