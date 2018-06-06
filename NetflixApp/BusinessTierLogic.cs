//
//
//
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
//
// BusinessTier:  business logic, acting as interface between UI and data store.
//
//  Jhon Nunez
//  jnunez34
//
//  U.  of  Illinois,  Chicago
//  CS341,  Spring  2018
//  Final Project
//
//--------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; //added
using System.Windows.Forms;//added
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace BusinessTier
{

  //
  // Business:
  //
  public class Business
  {
    //
    // Fields:
    //
    private string _DBFile;
    private DataAccessTier.Data dataTier;

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // Constructor:
    //
    public Business(string DatabaseFilename)
    {
      _DBFile = DatabaseFilename;

      dataTier = new DataAccessTier.Data(DatabaseFilename);
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // TestConnection:
    //
    // Returns true if we can establish a connection to the database, false if not.
    //
    public bool TestConnection()
    {
      return dataTier.TestConnection();
    }
 //--------------------------------------------------------------------------------------------------------------------------------------------------------------
 // GetNamedUser:
 //
 // Retrieves User object based on USER NAME; returns null if user is not
 // found.
 //
 // NOTE: there are "named" users from the Users table, and anonymous users
 // that only exist in the Reviews table.  This function only looks up "named"
 // users from the Users table.

   public User GetNamedUser(string UserName)
       {
            UserName = UserName.Replace("'", "''");//just incase there is a '
            string sql = string.Format(@" Select UserName, UserID, Occupation from Users Where UserName = '{0}' ;", UserName);//search for UserName 

            //Object result = cmd.ExecuteScalar();
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);

            if (result == null)
            {
                return null;
            }
            else
            {
                foreach (DataTable table in result.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        User userResult = new User(Convert.ToInt32(row["UserID"]), Convert.ToString(row["UserName"]), Convert.ToString(row["Occupation"]));
                        return userResult;
                    }
                }
            }
            return null;
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
     
    public IReadOnlyList<Movie> GetAllNamedMovies()
        {

            string sql = string.Format(@"SELECT MovieID, MovieName FROM Movies ORDER BY MovieName ASC;");
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            List<Movie> movies = new List<Movie>();

            foreach (DataTable table in result.Tables)
            {
                foreach (DataRow row in table.Rows)
                {

                    Movie userResult = new Movie(Convert.ToInt32(row["MovieID"]), Convert.ToString(row["MovieName"]));
                    movies.Add(userResult);

                }
            }

            return movies;
        }
            
//------------------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // GetAllNamedUsers:
        //
        // Returns a list of all the users in the Users table ("named" users), sorted 
        // by user name.
        //
        // NOTE: the database also contains lots of "anonymous" users, which this 
        // function does not return.
        //
   public IReadOnlyList<User> GetAllNamedUsers()
   { 
            string sql = string.Format(@" Select UserName,UserID,Occupation from Users Order By UserName ASC;");//SQL: Retrieve list of Users

            DataSet result = dataTier.ExecuteNonScalarQuery(sql); // for more than one items
            List<User> users = new List<User>();

            
            foreach (DataTable table in result.Tables)
            {
                foreach (DataRow row in table.Rows)
                { 
 
                    User userResult = new User(Convert.ToInt32(row["UserID"]), Convert.ToString(row["UserName"]), Convert.ToString(row["Occupation"]));
                    users.Add(userResult);

                }
            }
     
            return users;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------
    // GetMovie: Retrieves Movie object based on MOVIE ID; returns null if movie is not found.
    public Movie GetMovie(int MovieID)
    {

            string sql = string.Format(@"Select MovieName from Movies where MovieID = {0} ;", MovieID);//search for MovieName with MovieID
            Object result = dataTier.ExecuteScalarQuery(sql);

            if (result == null)
            {
                return null;
            }
            else
            {   
                string displayResult = Convert.ToString(result);      
                BusinessTier.Movie test = new BusinessTier.Movie(MovieID,displayResult);
                return test;
            }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------
    // GetMovie: Retrieves Movie object based on MOVIE NAME; returns null if movie is not found.
    public Movie GetMovie(string MovieName)
    {
            MovieName = MovieName.Replace("'", "''");//just incase there is a '
            string sql = string.Format(@" Select MovieID from Movies where MovieName = '{0}' ;", MovieName);
            object result = dataTier.ExecuteScalarQuery(sql);

            if (result == null)
            {
                return null;
            }
            else
            {
                int displayResult = Convert.ToInt32(result);
                Movie movie = new Movie(displayResult,MovieName);
                return movie;
            }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // AddReview: ***
    //
    // Adds review based on MOVIE ID, returning a Review object containing
    // the review, review's id, etc.  If the add failed, null is returned.
    //
    public Review AddReview(int MovieID, int UserID, int Rating)
    {

      string sql = string.Format(@"INSERT INTO Reviews(MovieID,UserID,Rating) VALUES({0},{1},{2}); SELECT ReviewID FROM Reviews WHERE ReviewID = SCOPE_IDENTITY();", MovieID,UserID,Rating);

      object result = dataTier.ExecuteScalarQuery(sql);

      if (result == null)
      {
                return null;
      }
      else
      {
                MessageBox.Show(Convert.ToString(result) );
                Review review = new Review(Convert.ToInt32(result),MovieID, UserID, Rating);
                return review;
      }
      

    }

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // GetMovieDetail: ***
    //
    // Given a MOVIE ID, returns detailed information about this movie --- all
    // the reviews, the total number of reviews, average rating, etc.  If the 
    // movie cannot be found, null is returned.
    //
    public MovieDetail GetMovieDetail(int MovieID)
    {    
            
            string sql2 = string.Format(@" Select ReviewID, MovieID, UserID, Rating from Reviews Where MovieID = {0} Order By Rating DESC;", MovieID);//SQL: Retrive Review List with Ratings and User ID
            DataSet result2 = dataTier.ExecuteNonScalarQuery(sql2);

            List<Review> reviews = new List<Review>();
            int totalReviews = 0;
            double sumReviews = 0;
            foreach (DataTable table2 in result2.Tables)
            {
                foreach (DataRow row in table2.Rows)
                {
                    Review reviewResult = new Review(Convert.ToInt32(row["ReviewID"]), Convert.ToInt32(row["MovieID"]), Convert.ToInt32(row["UserID"]),Convert.ToInt32(row["Rating"]));
                    reviews.Add(reviewResult);
                    sumReviews += Convert.ToInt32(row["Rating"]);
                    totalReviews++;
                }
            }

            double avgRating =  (sumReviews/totalReviews);
            double displayRating = Math.Round(avgRating, 1);

            MovieDetail m = new MovieDetail(GetMovie(MovieID), displayRating, totalReviews, reviews );

            if (reviews == null)
            {
                return null;
            }
            else
            {
                return m;
            }

    }

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------
    // GetUserDetail: ***
    // Given a USER ID, returns detailed information about this user --- all
    // the reviews submitted by this user, the total number of reviews, average 
    // rating given, etc.  If the user cannot be found, null is returned.
    public UserDetail GetUserDetail(int UserID)
    {

            string sql = string.Format(@" Select ReviewID, MovieID, UserID, Rating from Reviews Where UserID = {0};", UserID);
            string sql2 = string.Format(@" Select UserName from Users Where UserID = {0};", UserID);
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            object result2 = dataTier.ExecuteScalarQuery(sql2);

            List<Review> reviews = new List<Review>();
            int totalReviews = 0;
            double sumReviews = 0;

            foreach (DataTable table2 in result.Tables)
            {
                foreach (DataRow row in table2.Rows)
                {
                    Review reviewResult = new Review(Convert.ToInt32(row["ReviewID"]), Convert.ToInt32(row["MovieID"]), Convert.ToInt32(row["UserID"]), Convert.ToInt32(row["Rating"]));
                    reviews.Add(reviewResult);
                    sumReviews += Convert.ToInt32(row["Rating"]);
                    totalReviews++;
                }
            }

            double avgRating = (sumReviews / totalReviews);
            double displayRating = Math.Round(avgRating, 1);

            UserDetail m = new UserDetail(GetNamedUser(Convert.ToString(result2) ), displayRating, totalReviews, reviews);

            if (reviews == null)
            {
                return null;
            }
            else
            {
                return m;
            }

            
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------
    // GetTopMoviesByAvgRating:
    // Returns the top N movies in descending order by average rating.  If two
    // movies have the same rating, the movies are presented in ascending order
    // by name.  If N < 1, an EMPTY LIST is returned.
    public IReadOnlyList<Movie> GetTopMoviesByAvgRating(int N)
    {
      List<Movie> movies = new List<Movie>();

      string sql = string.Format(@" Select MovieID, Round(AVG(Cast( Rating as float) ),5)  as averageRating from Reviews Group by MovieID");//Table with MovieID and Average Rating
      string sql2 = string.Format(@" Select top {0} Movies.MovieID, Movies.MovieName from  Movies,({1}) as NewTable Where NewTable.MovieID = Movies.MovieID Order By NewTable.averageRating DESC;", N, sql);//Table with MovieID, MovieName based Top N movies from Average Rating Table

      DataSet result2 = dataTier.ExecuteNonScalarQuery(sql2);

            foreach (DataTable table in result2.Tables)
            {
                foreach (DataRow row in table.Rows)
                { 
                    Movie userResult = new Movie(Convert.ToInt32(row["MovieID"]), Convert.ToString(row["MovieName"]) );
                    movies.Add(userResult);
                }
            }

      return movies;
    }


  }//class
}//namespace
