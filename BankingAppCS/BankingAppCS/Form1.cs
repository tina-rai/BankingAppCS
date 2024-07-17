using System.Data.SqlClient;
    namespace BankingAppCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
    SqlConnection con = new SqlConnection("Data Source = TINA\\SQLEXPRESS; Initial Catalog = bankingapp; Integrated Security = True; Encrypt = False");
    con.Open();

    string depositQuery = "INSERT INTO account(name, address, balance, accountno," +
        "date, deposit) VALUES(@name, @add, @balance, @acc, @date, @deposit)";

    SqlCommand cmd = new SqlCommand(depositQuery, con);

    cmd.Parameters.AddWithValue("@name", txtName.Text);
    cmd.Parameters.AddWithValue("@add", txtAdd.Text);
    cmd.Parameters.AddWithValue("@balance", txtBalance.Text);
    cmd.Parameters.AddWithValue("@date", dtDate.Text);
    cmd.Parameters.AddWithValue("@deposit", txtBalance.Text);
    string accountno = AccountNo();
    cmd.Parameters.AddWithValue("@acc", accountno);

    int count = cmd.ExecuteNonQuery();
    con.Close();

    if (count > 0)
    {
        MessageBox.Show("deposited successfully", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}

private string AccountNo()
{
    int startWithAsInt = int.Parse("1280"); // Parses the string to an integer
    //string startWith "1280";
    Random random = new Random();
    string gen = random.Next(0, 999999).ToString("D6");
    string accountNo = startWithAsInt.ToString() + gen;

    //  string accountNo = startWith + gen;
    return accountNo;

}

  private void btnWithd_Click(object sender, EventArgs e)
  {
      int balance = 0;
      SqlConnection con = new SqlConnection("Data Source = TINA\\SQLEXPRESS; Initial Catalog = bankingapp; Integrated Security = True; Encrypt = False");
      con.Open();
      string readQuery = "SELECT* FROM account WHERE accountno= @acc";
      SqlCommand cmd = new SqlCommand(readQuery, con);
      cmd.Parameters.AddWithValue("@acc", txtAcc.Text);
      int count = Convert.ToInt32(cmd.ExecuteScalar());
      if (count > 0)
      {
          SqlDataReader reader = cmd.ExecuteReader();

          while (reader.Read())
          {
              balance = Convert.ToInt32(reader["balance"]);

          }
          reader.Close();

          if (balance > 0 && balance > Convert.ToInt32(txtBalance.Text))
          {
              int newBalance = balance - int.Parse(txtBalance.Text);
              string updateQuery = "UPDATE account SET balance= @balance, withdraw= @with" +
                                                             "WHERE accountno= @accno";
              SqlCommand cmdup = new SqlCommand(updateQuery, con);
              cmdup.Parameters.AddWithValue("@balance", newBalance);
              cmdup.Parameters.AddWithValue("@with", txtBalance.Text);
              cmdup.Parameters.AddWithValue("@accno", txtAcc.Text);
              cmdup.ExecuteNonQuery();
              con.Close();

              MessageBox.Show("Remaining Balance" + newBalance);


          }
          else
          {
              MessageBox.Show("insufficient amount", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);


          }

      }
      else
      {
          MessageBox.Show("account not found", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

      }

  }

  private void btnShow_Click(object sender, EventArgs e)
  {
      SqlConnection con = new SqlConnection("Data Source = TINA\\SQLEXPRESS; Initial Catalog = bankingapp; Integrated Security = True; Encrypt = False");
      con.Open();
      string selectQuery = "SELECT * FROM account WHERE accountno= @acc";
      SqlCommand cmd = new SqlCommand(selectQuery, con);
      cmd.Parameters.AddWithValue("@acc", txtAcc.Text);
      SqlDataReader reader = cmd.ExecuteReader();
       while(reader.Read())
      {
          txtName.Text= reader["name"].ToString();
          txtName.Text = reader["address"].ToString();
          txtName.Text = reader["balance"].ToString();
      }

      reader.Close();
      con.Close();
      dtDate.Enabled=false;
        }
    }
}
