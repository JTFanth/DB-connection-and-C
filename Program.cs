// See https://aka.ms/new-console-template for more information


using System;
using System.Data;
using System.Security.Authentication;
using Npgsql;

class Sample
{
    static void Main(string[] args)
    {
        // Connect to a PostgreSQL database
        NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1:5432;User Id=postgres; " +
           "Password=Fanth15407;Database=prods;");
        conn.Open();

        // Define a query returning a single row result set
        NpgsqlCommand prod = new NpgsqlCommand("SELECT * FROM product", conn);
        NpgsqlDataReader reader1 = prod.ExecuteReader();
        DataTable prod_table = new DataTable();
        prod_table.Load(reader1);

        NpgsqlCommand cust = new NpgsqlCommand("SELECT * FROM customer", conn);
        NpgsqlDataReader reader2 = cust.ExecuteReader();
        DataTable cust_table = new DataTable();
        cust_table.Load(reader2);

        print_7(prod_table);

        Console.WriteLine("");
        
        print_20(cust_table);
        
        conn.Close();
    }

    static void print_(DataTable data)
    {
        Console.WriteLine();
        Dictionary<string, int> colWidths = new Dictionary<string, int>();
        foreach (DataColumn col in data.Columns)
        {
            Console.Write(col.ColumnName);
            var maxLabelSize = data.Rows.OfType<DataRow>()
                    .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                    .OrderByDescending(m => m).FirstOrDefault();

            colWidths.Add(col.ColumnName, maxLabelSize);
            for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 14; i++) Console.Write(" ");
        }

        Console.WriteLine();
        
        foreach (DataRow dataRow in data.Rows)
        {
            for (int j = 0; j < dataRow.ItemArray.Length; j++)
            {
                Console.Write(dataRow.ItemArray[j]);
                for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 14; i++) Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
    static void print_7(DataTable data)
    {
        string filter = "prod_quantity > 12 AND prod_quantity < 30";

        DataRow[] filtered = data.Select(filter);

        DataTable filteredtable = data.Clone();

        Console.WriteLine("Problem #7");

        foreach (DataRow row in filtered)
        {
            filteredtable.ImportRow(row);
        }
        print_(filteredtable);
    }

    static void print_20(DataTable data)
    {
        Dictionary<string,int> colWidths = new Dictionary<string,int>();
        Console.WriteLine("Problem #20");
        foreach (DataColumn col in data.Columns)
        {
            if ((col.ColumnName).ToString() == "rep_id")
            {
                Console.Write(col.ColumnName);
                var maxSize = data.Rows.OfType<DataRow>()
                    .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                    .OrderByDescending(m => m).FirstOrDefault();

                colWidths.Add(col.ColumnName, maxSize);
                for (int i = 0; i < maxSize - col.ColumnName.Length + 14; i++)
                {
                    Console.Write(" ");
                }

                Console.Write("balance_sum");
                colWidths.Add("balance_sum", maxSize);
                for (int i = 0; i < maxSize - "balance_sum".Length + 14; i++)
                {
                    Console.Write("");
                }
            }

            Console.Write("");
        }
        Console.WriteLine();

        Dictionary<int, int> rep_balances = new Dictionary<int, int>();

        foreach (DataRow dataRow in data.Rows)
        {
            int rep_id = Convert.ToInt16(dataRow.ItemArray[8]);
            int currCustBalance = Convert.ToInt16(dataRow.ItemArray[6]);

            if (rep_balances != null && rep_balances.ContainsKey(rep_id))
            {
                int previous_value = rep_balances[rep_id];
                rep_balances[rep_id] = previous_value + currCustBalance;
            }
            else if (rep_balances != null && !rep_balances.ContainsKey(rep_id))
            {
                rep_balances.Add(rep_id, currCustBalance);
            }
        }

        foreach (KeyValuePair<int, int> pair in rep_balances)
        {
            if (pair.Value > 12000)
            {
                Console.WriteLine(pair.Key.ToString() + "              " + pair.Value);
                Console.WriteLine();
            }
        }
    }
}