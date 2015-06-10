
** command to import data from .txt file into mongodb
** note to export data to txt file from SQL server, use export utility and select "flat file" in destination path.
1. open command prompt and navigate to win folder where mongoshell (exe's) are installed.
2. then type mongoimport as shown below.
 
 first way:
    c:\Program Files\MongoDB\Server\3.0\bin>mongoimport --db test --collection north
    windb --type csv --file "c:\Roshit\MongoDB\CSVData\NorthwindDB\northwinddb.txt"
    -f id,compname,cname,ctitle,address
Note: Ensure to start the mongodb server (windows service) before running mongoimport




second way to do mongoimport (using headerline switch):
  mongoimport -d mydb -c northwindb --type csv --file "c:\.....locations.csv" --headerline

Note:  
**** headerline switch only to be used with .csv or .txt files
** to use headerline, first row in .csv/.txt file should be column names
** if any column has double-quotes in the data, mongoimport will fail with csv switch
*** thereby while exporting data to txt files, check "column names in first data row"
    in the sql server => tasks => export data utility in the destination tab

  
Third way is to use "tsv" switch (tab -separated instead of comma separated)
 mongoimport -d mydb -c northwindb --type tsv --file "c:\.....locations.csv" --headerline --ignoreBlanks
 This command works even if there are double-quotes in the data.
 
 
3. after all data is imported, view them from "Robomongo" tool.
