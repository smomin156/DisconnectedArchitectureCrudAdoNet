using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DBLibrary
{

    public class EmpDataStore
    {
        SqlConnection connection;
        SqlDataAdapter adapter;
        DataSet ds;

        public EmpDataStore(string connectionstring)
        {
            connection = new SqlConnection(connectionstring);
            string sql = "select empno,ename,hiredate,sal from emp";
            adapter = new SqlDataAdapter(sql, connection);
            ds = new DataSet();
            adapter.Fill(ds, "emp");
        }

        public List<Employee> GetAllEmps()
        {
            try
            {
                List<Employee> empList = new List<Employee>();
                foreach (DataRow row in ds.Tables["emp"].Rows)
                {
                    Employee emp = new Employee();
                    emp.EmpNo =(int)row["Empno"];
                    emp.EmpName = row["Ename"].ToString();
                    emp.HireDate = row["HireDate"] as DateTime?;
                    emp.Salary = row["sal"] as decimal?;
                    empList.Add(emp);
                }
                return empList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Employee GetEmpById(int eno)
        {
            try
            {
                DataRow[] dw = ds.Tables["emp"].Select($"empno='{eno}'");
                Employee emp = new Employee();
                emp.EmpNo = Convert.ToInt32(dw[0]["Empno"]);
                emp.EmpName = dw[0]["Ename"].ToString();
                emp.HireDate = dw[0]["HireDate"] as DateTime?;
                emp.Salary = dw[0]["sal"] as decimal?;
                return emp;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public void InsertEmp(Employee emp)
        {
            DataRow row = ds.Tables["emp"].NewRow();
            row["empno"] = emp.EmpNo;
            row["ename"] = emp.EmpName;
            row["hiredate"] = emp.HireDate;
            row["sal"] = emp.Salary;
            ds.Tables["emp"].Rows.Add(row);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Update(ds.Tables["emp"]);
        }
        public void UpdateEmp(Employee emp)
        {
            DataRow row = ds.Tables["emp"].Select($"empno='{emp.EmpNo}'").First();
            if (row != null)
            {
                row["ename"] = emp.EmpName;
                row["hiredate"] = emp.HireDate;
                row["sal"] = emp.Salary;
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(ds.Tables["emp"]);
            }
        }

        public void DeleteEmp(int empno)
        {
            DataRow row = ds.Tables["emp"].Select($"empno='{empno}'").First();
            if (row != null)
            {
                //ds.Tables["emp"].Rows.Remove(row);
                row.Delete();
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(ds.Tables["emp"]);
            }
        }

    }
}
