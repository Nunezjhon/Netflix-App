using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

//-----------------------------------------------------------------------------------------------------------------------------------------
//
//  Netflix  Database  Application  using  N-Tier  Design.
//
//  Jhon Nunez
//  jnunez34
//
//  U.  of  Illinois,  Chicago
//  CS341,  Spring  2018
//  Project  08
//
//-----------------------------------------------------------------------------------------------------------------------------------------
namespace NetflixApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool fileExists(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                string msg = string.Format("Input file not found: '{0}'",
                  filename);

                MessageBox.Show(msg);
                return false;
            }

            // exists!
            return true;
        }

        private void clearForm()
        {
           // this.chart.Series.Clear();
           // this.chart.Titles.Clear();
           // this.chart.Legends.Clear();
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)//Movies
        {

            listBox1.Items.Clear();// clear display

            string dbfilename = this.textBox3.Text;    //  get  DB  name  from  text  box:
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            var Movies = biztier.GetAllNamedMovies();//  obtain  movie  object: //var == autol

            foreach (var x in Movies)
            {
                if(x.MovieName != null)
                {
                    listBox1.Items.Add(x.MovieName);
                }
               
            }



            /*
            string filename, version, connectionInfo;
            SqlConnection db;

            this.Cursor = Cursors.WaitCursor;

            version = "MSSQLLocalDB";
            filename = textBox3.Text;

            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            string sql = string.Format(@"SELECT MovieName FROM Movies  GROUP BY MovieName ORDER BY MovieName ASC;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);

            db.Close();
            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    listBox1.Items.Add(row["MovieName"]);

                }
            }
            */


            textBox1.Clear();//Clear boxes
            textBox2.Clear();

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ListBox1: User's selection
            //textBox1: ID
            //textBox2: Average rating

            string dbfilename = this.textBox3.Text;   //get DB name from text box: //where the database is located
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            string moviename = this.listBox1.Text;     //get user’s input from where they are clicking
            BusinessTier.Movie movie = biztier.GetMovie(moviename);      //  obtain movie object:

            BusinessTier.MovieDetail movie2 = biztier.GetMovieDetail(movie.MovieID); // from Object browser
            
            this.textBox1.Text = "" + movie.MovieID;

            this.textBox2.Text = "" + movie2.AvgRating; //from Object browser
           

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)// USERS
        {

            listBox2.Items.Clear();// clear Display

            string dbfilename = this.textBox3.Text;    //  get  DB  name  from  text  box:
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            var users = biztier.GetAllNamedUsers();//  obtain  movie  object: //var == autol

            foreach(var x in users)
            {
                listBox2.Items.Add(x.UserName);
            }

            textBox4.Clear(); //clear textboxes
            textBox5.Clear();

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ListBox2: User's selection
            //textBox4: User ID
            //textBox5: Occupation

            string dbfilename = this.textBox3.Text;   //get DB name from text box: //where the database is located
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            string username = this.listBox2.Text;     //get user’s input from where they are clicking
            BusinessTier.User user = biztier.GetNamedUser(username); //obtain user object

            this.textBox4.Text = "" + user.UserID;
            this.textBox5.Text = "" + user.Occupation;

            
            
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void textBox4_TextChanged(object sender, EventArgs e)
        {


        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {


        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void button5_Click(object sender, EventArgs e)// Top N Movies
        {
            //get top movies by N + ratings
            //textBox9 is N

            listBox2.Items.Clear();// clear display

            var test = this.textBox9.Text;
            if (test == null)//no number added check
            {
                MessageBox.Show("Please insert a number");
                return;
            }

            string dbfilename = this.textBox3.Text;   //get DB name from text box: //where the database is located
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            //string username = this.listBox2.Text;     //get user’s input from where they are clicking
            //BusinessTier.User user = biztier.GetNamedUser(username); //obtain user object

            int number = Convert.ToInt32(this.textBox9.Text);//invalid number check
            if (number < 0 )
            {
                MessageBox.Show("Invalid number!");
                return;
            }

            var topMovies = biztier.GetTopMoviesByAvgRating( Convert.ToInt32(this.textBox9.Text) );

            foreach (var x in topMovies)
            {
                BusinessTier.Movie movie = biztier.GetMovie(x.MovieName);
                BusinessTier.MovieDetail movie2 = biztier.GetMovieDetail(movie.MovieID);

                var review = movie2.AvgRating;

                listBox2.Items.Add(x.MovieName + ":" + review);
            }

            this.textBox9.Clear();

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)// Search User Reviews
        {
            listBox2.Items.Clear();// clear display

            string dbfilename = this.textBox3.Text;   //get DB name from text box: //where the database is located
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            string username = this.textBox10.Text;     //get user’s input from where they are clicking
    
            BusinessTier.User user = biztier.GetNamedUser(username);

            if (user == null)
            {
                MessageBox.Show("Invalid User");
                return;
            }

            BusinessTier.UserDetail ratings = biztier.GetUserDetail(user.UserID); // from Object browser

            listBox2.Items.Add(username);
            listBox2.Items.Add(" ");

            foreach (var x in ratings.Reviews)
            {
                var movie = biztier.GetMovie(x.MovieID);
                string movieName = movie.MovieName;

                listBox2.Items.Add(movieName + " -> " + x.Rating );

            }

            this.textBox10.Clear();// clear textBox

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void Reviews_Click(object sender, EventArgs e)//Movie Reviews
        {
            //textBox11: Movie Input
            //listBox1: Diplay reviews 
            listBox1.Items.Clear();// Clear display

            string dbfilename = this.textBox3.Text;   //get DB name from text box: //where the database is located
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            string movie = this.textBox11.Text;     //get user’s input from where they are clicking

            BusinessTier.Movie movieObj = biztier.GetMovie(movie);

            if (movieObj == null)
            {
                MessageBox.Show("Invalid Movie");
                return;
            }

            BusinessTier.MovieDetail details = biztier.GetMovieDetail(movieObj.MovieID); // from Object browser

            listBox1.Items.Add(movie);
            listBox1.Items.Add(" ");

            foreach(var x in details.Reviews)
            {

                listBox1.Items.Add(x.UserID + ": " + x.Rating);

            }

            textBox11.Clear(); //Clear textbox
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void eachRating_Click(object sender, EventArgs e)//each rating
        {
            //listBox1: Display results
            //textBox12: movie Input
            listBox1.Items.Clear();// clear the display

            string dbfilename = this.textBox3.Text;   //get DB name from text box: //where the database is located
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            string movie = this.textBox12.Text;     //get user’s input from where they are clicking

            BusinessTier.Movie movieObj = biztier.GetMovie(movie);


            if (movieObj == null)
            {
                MessageBox.Show("Invalid Movie");
                return;
            }

            BusinessTier.MovieDetail details = biztier.GetMovieDetail(movieObj.MovieID); // from Object browser


            listBox1.Items.Add(movie);
            listBox1.Items.Add(" ");

            int x5 = 0;
            int x4 = 0;
            int x3 = 0;
            int x2 = 0;
            int x1 = 0;
            int x0 = 0;
            
            foreach (var x in details.Reviews)
            {
                
                if (x.Rating == 5)
                { 
                    x5 = x5 + 1;
                }
                else if (x.Rating == 4)
                {
                    x4 = x4 + 1;
                }
                else if (x.Rating == 3)
                {
                    x3 = x3 + 1;
                }
                else if (x.Rating == 2)
                {
                    x2 = x2 + 1;
                }
                else 
                {
                    x1 = x1 + 1;
                }

                x0 = x0 + 1; //total 

            }

            listBox1.Items.Add("5: " + x5);
            listBox1.Items.Add("4: " + x4);
            listBox1.Items.Add("3: " + x3);
            listBox1.Items.Add("2: " + x2);
            listBox1.Items.Add("1: " + x1);
            listBox1.Items.Add(" ");
            listBox1.Items.Add("Total: " + x0);

            textBox12.Clear();//clear textbox

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)// insert Review
        {
            //textBox6: User
            //textBox7: Movie
            //textBox8: Rating
            //addReview(int,int,int);

            string dbfilename = this.textBox3.Text;   //get DB name from text box: //where the database is located
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);


            string username = this.textBox6.Text;       //get user input from user
            string movie = this.textBox7.Text;      //get movie input from user
            string rating = this.textBox8.Text;     //get rating input from user
            BusinessTier.Movie movieObj = biztier.GetMovie(movie);
            BusinessTier.User userObj = biztier.GetNamedUser(username);
            
            if (userObj == null)
            {
                MessageBox.Show("Invalid Username");
                return;
            }
            else if (movieObj == null)
            {
                MessageBox.Show("Invalid Movie");
                return;
            }
            else if (Convert.ToInt32(rating) < 1 || Convert.ToInt32(rating) > 5)
            {
                MessageBox.Show("Rating must be 1 <= 5");
                return;
            }
            else
            {
            var x = biztier.AddReview( movieObj.MovieID, userObj.UserID, Convert.ToInt32(rating) );
            
                if (x == null)
                {
                MessageBox.Show("fail");
                }

            }


            this.textBox6.Clear();
            this.textBox7.Clear();
            this.textBox8.Clear();


        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void Clear_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            this.textBox2.Clear();
            this.textBox4.Clear();
            this.textBox5.Clear();
            this.textBox5.Clear();
            this.textBox6.Clear();
            this.textBox7.Clear();
            this.textBox8.Clear();
            this.textBox9.Clear();
            this.textBox10.Clear();
            this.textBox11.Clear();
            this.textBox12.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();

        }
    }
}
