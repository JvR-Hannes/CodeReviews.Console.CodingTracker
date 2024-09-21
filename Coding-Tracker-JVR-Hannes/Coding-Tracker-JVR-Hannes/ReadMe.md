<h1>Coding Tracker</h1>

  <p>This C# program is a simple command-line coding tracker application that utilizes a SQLite database to store coding sessions.<br></p>

  <p>Below is a breakdown of the key components and how they work:</p>

<h2>Key Components:</h2>

<h3>Database Path Configuration:</h3>

<p>The database path is fetched from the configuration file (App.config) via ConfigurationManager.AppSettings["DatabasePath"].<br></p>

<p>The path is combined with the database filename to form the full path of the SQLite database file.<br></p>
<h3>SQLite Database:</h3>

<div>A SQLite connection is established using the SQLiteConnection class.<br></div>
<p>The program ensures that a table (coding_session) exists to store session records.</p>
<h4>The table coding_session includes columns for:</h4>

<div>Id (primary key, autoincrement),<br>
StartTime (start time of the coding session),<br>
EndTime (end time of the coding session),<br>
Duration (total time spent coding).<br></div>

<h3>User Interaction:</h3>

<p>The GetUserInput method handles user inputs in a loop until the user decides to exit.</p>
<div>The menu allows users to:
  <ul>
    <li>View all coding sessions: Displays a list of all recorded sessions.</li>
    <li>Insert a new session: Accepts user input for start and end times, computes the duration, and saves the session to the database.</li>
    <li>Delete a session: Allows the user to delete a session by specifying its ID.</li>
    <li>Clear the Database: Allows the user to clear all records in the database after confirmation prompts</li>

  </ul>
</div>
