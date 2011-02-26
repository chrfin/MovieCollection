using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MovieDataSource.Properties;
using System.Collections.ObjectModel;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Threading;

namespace MovieDataSource
{
    internal enum Table
    {
        Movies,
        Persons,
        /// <summary>
        /// NOT A REAL TABLE, only for refrences...
        /// </summary>
        Directors,
        /// <summary>
        /// NOT A REAL TABLE, only for refrences...
        /// </summary>
        Cast,
        Genres,
        MediaFiles,
        VideoProperties,
        AudioProperties,
        UserProfiles,
        UserMovieSettings
    }

    /// <summary>
    /// Class to create a Sql CE movie data source.
    /// </summary>
    [DataSourcePlugin]
    public class SqlCeMovieDataSourceFactory : IMovieDataSourceFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeMovieDataSourceFactory"/> class.
        /// </summary>
        public SqlCeMovieDataSourceFactory()
        {
            Extension = Resources.Extension;
        }

        #region IMovieDataSourceFactory Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get { return "File-MovieCollection"; } }
        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public Image Icon { get { return Resources.harddrive; } }

        /// <summary>
        /// The type of this data source.
        /// </summary>
        /// <value></value>
        public DataSourceType Type { get { return DataSourceType.FileDataSource; } }

        /// <summary>
        /// Creates the data source with the specified filename (call this for file data sources).
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public IMovieDataSourcePlugin Create(string filename)
        {
            if (!File.Exists(filename))
                SqlCE.CreateNewDB(filename, true);

            return new SqlCeMovieDataSource(filename);
        }

        /// <summary>
        /// NOT VALID FOR FILE.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public IMovieDataSourcePlugin Create(ConnectionStringStruct connectionString) { throw new NotImplementedException(); }

        /// <summary>
        /// The extension of the file used by this data source (empty for database data sources).
        /// </summary>
        /// <value></value>
        public string Extension { get; private set; }

        #endregion
    }


    /// <summary>
    /// A movie data source which uses a SQL Compact Edition to store the data.
    /// </summary>
    public class SqlCeMovieDataSource : IMovieDataSourcePlugin, IDisposable
    {
        /// <summary>
        /// Gets the db connection.
        /// </summary>
        /// <value>The db connection.</value>
        internal SqlCeConnection DbConnection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeMovieDataSource"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public SqlCeMovieDataSource(string filename)
        {
            Filename = filename;
            DbConnection = new SqlCeConnection(SqlCE.GetConnectionString(filename));
            DbConnection.Open();

            Movies = new ObservableCollection<IMovie>();
            LoadList();
            Movies.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Movies_CollectionChanged);

            Users = new ObservableCollection<IUserProfile>();
            LoadUsers();
            Users.CollectionChanged += new NotifyCollectionChangedEventHandler(Users_CollectionChanged);
        }

        /// <summary>
        /// Closes this data source.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Movies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Movies_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((new List<IMovie>(e.NewItems.Cast<IMovie>())).Find(m => !(m is SqlCeMovie)) != null)
                        throw new ArgumentException("Can only add SqlCeMovies!");
                    if ((new List<SqlCeMovie>(e.NewItems.Cast<SqlCeMovie>())).Find(m => m.Store != this) != null)
                        throw new ArgumentException("Can only add Movies from this source!");
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IMovie movie in e.OldItems)
                    {
                        while (movie.Cast.Count > 0) movie.Cast.RemoveAt(0);
                        while (movie.Directors.Count > 0) movie.Directors.RemoveAt(0);
                        while (movie.Genres.Count > 0) movie.Genres.RemoveAt(0);
                        while (movie.MediaFiles.Count > 0) movie.MediaFiles.RemoveAt(0);

                        foreach (IUserProfile user in Users)
                        {
                            IUserMovieSettings settings = user.MovieSettings.ToList().Find(s => s.Movie == movie);
                            if (settings != null)
                                user.MovieSettings.Remove(settings);
                        }

                        RemoveRow(Table.Movies, movie.Id);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException("Please don't change the whole list at all!");
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Users control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((new List<IUserProfile>(e.NewItems.Cast<IUserProfile>())).Find(m => !(m is SqlCeUserProfile)) != null)
                        throw new ArgumentException("Can only add SqlCeUserProfile!");
                    if ((new List<SqlCeUserProfile>(e.NewItems.Cast<SqlCeUserProfile>())).Find(m => m.Store != this) != null)
                        throw new ArgumentException("Can only add Users from this source!");
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IUserProfile user in e.OldItems)
                        RemoveRow(Table.UserProfiles, user.Id);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException("Please don't change the whole list at all!");
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Loads the list.
        /// </summary>
        private void LoadList()
        {
            SqlCeCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = "SELECT id FROM Movies";
            SqlCeDataReader reader = SqlCE.ExecuteReader(cmd);
            List<int> ids = new List<int>();
            while (reader.Read())
                ids.Add(Convert.ToInt32(reader["id"]));
            reader.Close();
            ids.ForEach(i => Movies.Add(new SqlCeMovie(i, this)));
        }

        /// <summary>
        /// Loads the users.
        /// </summary>
        private void LoadUsers()
        {
            SqlCeCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = "SELECT id FROM UserProfiles";
            SqlCeDataReader reader = SqlCE.ExecuteReader(cmd);
            List<int> ids = new List<int>();
            while (reader.Read())
                ids.Add(Convert.ToInt32(reader["id"]));
            reader.Close();
            ids.ForEach(i => Users.Add(new SqlCeUserProfile(i, this)));
        }

        #region DB methods

        private const bool CACHE_ENABLED = true;

        private Dictionary<Table, Dictionary<int, Dictionary<string, object>>> RowCache = new Dictionary<Table, Dictionary<int, Dictionary<string, object>>>();
        /// <summary>
        /// Adds the row to the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="values">The values (AT LEAST ONE, even if it is "NULL").</param>
        /// <returns></returns>
        internal int AddRow(Table table, Dictionary<string, object> values)
        {
            SqlCeCommand cmd = DbConnection.CreateCommand();

            string col = string.Empty, val = string.Empty;
            foreach (KeyValuePair<string, object> pair in values)
            {
                col += pair.Key + ", ";
                val += "@" + pair.Key + ", ";
                cmd.Parameters.Add(pair.Key, pair.Value);
            }
            col = col.Trim(new char[] { ',', ' ' });
            val = val.Trim(new char[] { ',', ' ' });

            cmd.CommandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2}); SELECT @@IDENTITY", table, col, val);

            while (commandsQueue.Count > 0) Thread.Sleep(10);

            int id = SqlCE.ExecuteScalar<int>(cmd).Value;

            //if (CACHE_ENABLED)
            //{
            //if (!RowCache.ContainsKey(table))
            //    RowCache[table] = new Dictionary<int, Dictionary<string, object>>();
            //RowCache[table][id] = values;
            //}

            return id;
        }
        /// <summary>
        /// Removes the row from the specified table (incl. foreign keys).
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <param name="table">The table.</param>
        internal void RemoveRow(Table table, int id)
        {
            SqlCeCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = string.Format("DELETE FROM {0} WHERE id=@id", table);
            cmd.Parameters.Add("id", id);
            ExecuteCommandAsync(cmd);

            if (CACHE_ENABLED && RowCache.ContainsKey(table) && RowCache[table].ContainsKey(id))
                RowCache[table].Remove(id);
        }
        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        internal Dictionary<string, object> GetRow(Table table, int id)
        {
            if (CACHE_ENABLED && RowCache.ContainsKey(table) && RowCache[table].ContainsKey(id))
                return RowCache[table][id];

            while (commandsQueue.Count > 0) Thread.Sleep(10);

            SqlCeDataReader reader;
            if (CACHE_ENABLED && !RowCache.ContainsKey(table))
            {
                SqlCeCommand cmd = DbConnection.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM {0}", table);
                reader = SqlCE.ExecuteReader(cmd);
            }
            else
            {
                SqlCeCommand cmd = DbConnection.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM {0} WHERE id=@id", table);
                cmd.Parameters.Add("id", id);
                reader = SqlCE.ExecuteReader(cmd);
            }

            while (reader.Read())
            {
                Dictionary<string, object> columns = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    columns.Add(reader.GetName(i), reader[i]);

                if (!CACHE_ENABLED)
                    return columns;

                if (!RowCache.ContainsKey(table))
                    RowCache[table] = new Dictionary<int, Dictionary<string, object>>();
                RowCache[table][Convert.ToInt32(columns["id"])] = columns;
            }
            reader.Close();

            return RowCache[table][id];
        }
        /// <summary>
        /// Updates the column.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="id">The id.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        internal void UpdateColumn(Table table, int id, string column, object value)
        {
            SqlCeCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = string.Format("UPDATE {0} SET {1}=@value WHERE id=@id", table, column);
            cmd.Parameters.Add("id", id);
            cmd.Parameters.Add("value", value ?? DBNull.Value);
            ExecuteCommandAsync(cmd);

            if (CACHE_ENABLED && RowCache.ContainsKey(table) && RowCache[table].ContainsKey(id))
                RowCache[table][id][column] = value;
        }

        private Dictionary<string, object> FieldCache = new Dictionary<string, object>();
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="id1Column">The id1 column.</param>
        /// <param name="id1">The id1.</param>
        /// <param name="id2Column">The id2 column.</param>
        /// <param name="id2">The id2.</param>
        /// <param name="fieldColumn">The field column.</param>
        /// <returns></returns>
        internal object GetField(string table, string id1Column, int id1, string id2Column, int id2, string fieldColumn)
        {
            if (CACHE_ENABLED)
            {
                string key = GetFieldKey(table, id1Column, id1, id2Column, id2, fieldColumn);
                if (FieldCache.ContainsKey(key))
                    return FieldCache[key];

                SqlCeCommand cmd = DbConnection.CreateCommand();
                cmd.CommandText = string.Format("SELECT {2}, {3}, {0} FROM {1}", fieldColumn, table, id1Column, id2Column);

                while (commandsQueue.Count > 0) Thread.Sleep(10);

                SqlCeDataReader reader = SqlCE.ExecuteReader(cmd);

                while (reader.Read())
                    FieldCache[GetFieldKey(table, id1Column, Convert.ToInt32(reader[id1Column]), id2Column, Convert.ToInt32(reader[id2Column]), fieldColumn)] = reader[fieldColumn];

                return FieldCache[key];
            }
            else
            {
                SqlCeCommand cmd = DbConnection.CreateCommand();
                cmd.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2}=@id1 AND {3}=@id2", fieldColumn, table, id1Column, id2Column);
                cmd.Parameters.Add("id1", id1);
                cmd.Parameters.Add("id2", id2);

                while (commandsQueue.Count > 0) Thread.Sleep(10);

                return SqlCE.ExecuteScalar(cmd);
            }
        }
        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="id1Column">The id1 column.</param>
        /// <param name="id1">The id1.</param>
        /// <param name="id2Column">The id2 column.</param>
        /// <param name="id2">The id2.</param>
        /// <param name="fieldColumn">The field column.</param>
        /// <param name="value">The value.</param>
        /// <remarks>Documented by CFI, 2009-04-16</remarks>
        internal void SetField(string table, string id1Column, int id1, string id2Column, int id2, string fieldColumn, object value)
        {
            SqlCeCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = string.Format("UPDATE {0} SET {1}=@value WHERE {2}=@id1 AND {3}=@id2", table, fieldColumn, id1Column, id2Column);
            cmd.Parameters.Add("id1", id1);
            cmd.Parameters.Add("id2", id2);
            cmd.Parameters.Add("value", value);
            ExecuteCommandAsync(cmd);

            if (CACHE_ENABLED)
                FieldCache[GetFieldKey(table, id1Column, id1, id2Column, id2, fieldColumn)] = value;
        }
        /// <summary>
        /// Gets the field key.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="id1Column">The id1 column.</param>
        /// <param name="id1">The id1.</param>
        /// <param name="id2Column">The id2 column.</param>
        /// <param name="id2">The id2.</param>
        /// <param name="fieldColumn">The field column.</param>
        /// <returns></returns>
        private string GetFieldKey(string table, string id1Column, int id1, string id2Column, int id2, string fieldColumn)
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}_{5}", table, id1Column, id1, id2Column, id2, fieldColumn);
        }

        private Dictionary<Table, Dictionary<Table, List<KeyValuePair<int, int>>>> ReferencesCache = new Dictionary<Table, Dictionary<Table, List<KeyValuePair<int, int>>>>();
        /// <summary>
        /// Gets the references of table 2 in the specified row of table 1.
        /// </summary>
        /// <param name="tabel1">The tabel1.</param>
        /// <param name="table2">The table2.</param>
        /// <param name="table1Id">The table1 id.</param>
        /// <returns></returns>
        internal List<int> GetReferences(Table tabel1, Table table2, int table1Id)
        {
            if (CACHE_ENABLED)
            {
                if (!ReferencesCache.ContainsKey(tabel1) || !ReferencesCache[tabel1].ContainsKey(table2))
                {
                    if (!ReferencesCache.ContainsKey(tabel1))
                        ReferencesCache[tabel1] = new Dictionary<Table, List<KeyValuePair<int, int>>>();
                    if (!ReferencesCache[tabel1].ContainsKey(table2))
                        ReferencesCache[tabel1][table2] = new List<KeyValuePair<int, int>>();

                    SqlCeCommand cmd = DbConnection.CreateCommand();
                    cmd.CommandText = string.Format("SELECT {0}_id, {1}_id FROM {2}",
                        tabel1.ToString().ToLower(), table2.ToString().ToLower(), tabel1 + "_" + table2);

                    while (commandsQueue.Count > 0) Thread.Sleep(10);

                    SqlCeDataReader reader = SqlCE.ExecuteReader(cmd);

                    while (reader.Read())
                        ReferencesCache[tabel1][table2].Add(new KeyValuePair<int, int>(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1])));
                    reader.Close();
                }

                var refs = from reference in ReferencesCache[tabel1][table2]
                           where reference.Key == table1Id
                           select reference.Value;

                return refs.ToList();
            }
            else
            {
                SqlCeCommand cmd = DbConnection.CreateCommand();
                cmd.CommandText = string.Format("SELECT {1}_id FROM {2} WHERE {0}_id=@id",
                    tabel1.ToString().ToLower(), table2.ToString().ToLower(), tabel1 + "_" + table2);
                cmd.Parameters.Add("id", table1Id);

                while (commandsQueue.Count > 0) Thread.Sleep(10);

                SqlCeDataReader reader = SqlCE.ExecuteReader(cmd);

                List<int> refs = new List<int>();
                while (reader.Read())
                    refs.Add(Convert.ToInt32(reader[0]));
                reader.Close();

                return refs;
            }
        }
        /// <summary>
        /// Adds the references.
        /// </summary>
        /// <param name="tabel1">The tabel1.</param>
        /// <param name="table2">The table2.</param>
        /// <param name="table1Id">The table1 id.</param>
        /// <param name="table2Id">The table2 id.</param>
        internal void AddReferences(Table tabel1, Table table2, int table1Id, int table2Id)
        {
            if (CACHE_ENABLED)
            {
                if (ReferencesCache.ContainsKey(tabel1) && ReferencesCache[tabel1].ContainsKey(table2) && ReferencesCache[tabel1][table2].FindIndex(p => p.Key == table1Id && p.Value == table2Id) >= 0)
                    return;
            }

            SqlCeCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = string.Format("INSERT INTO {1} ({2}_id, {0}_id) VALUES (@id1, @id2)",
                table2.ToString().ToLower(), tabel1 + "_" + table2, tabel1.ToString().ToLower());
            cmd.Parameters.Add("id1", table1Id);
            cmd.Parameters.Add("id2", table2Id);
            ExecuteCommandAsync(cmd);

            if (!CACHE_ENABLED)
                return;

            if (!ReferencesCache.ContainsKey(tabel1))
                ReferencesCache[tabel1] = new Dictionary<Table, List<KeyValuePair<int, int>>>();
            if (!ReferencesCache[tabel1].ContainsKey(table2))
                ReferencesCache[tabel1][table2] = new List<KeyValuePair<int, int>>();

            ReferencesCache[tabel1][table2].Add(new KeyValuePair<int, int>(table1Id, table2Id));
        }
        /// <summary>
        /// Removes the references.
        /// </summary>
        /// <param name="tabel1">The tabel1.</param>
        /// <param name="table2">The table2.</param>
        /// <param name="table1Id">The table1 id.</param>
        /// <param name="table2Id">The table2 id.</param>
        internal void RemoveReferences(Table tabel1, Table table2, int table1Id, int table2Id)
        {
            SqlCeCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = string.Format("DELETE FROM {1} WHERE {2}_id=@id1 AND {0}_id=@id2",
                table2.ToString().ToLower(), tabel1 + "_" + table2, tabel1.ToString().ToLower());
            cmd.Parameters.Add("id1", table1Id);
            cmd.Parameters.Add("id2", table2Id);
            ExecuteCommandAsync(cmd);

            if (CACHE_ENABLED && ReferencesCache.ContainsKey(tabel1) && ReferencesCache[tabel1].ContainsKey(table2))
                ReferencesCache[tabel1][table2].RemoveAll(p => p.Key == table1Id && p.Value == table2Id);
        }

        /// <summary>
        /// Searches the row.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal int? SearchRow(Table table, string column, object value)
        {
            if (CACHE_ENABLED && RowCache.ContainsKey(table))
            {
                var res = from r in RowCache[table]
                          where r.Value.ContainsKey(column) && r.Value[column] == value
                          select r.Key;

                if (res.ToList().Count > 0)
                    return res.ToList()[0];
            }

            SqlCeCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = string.Format("SELECT id FROM {0} WHERE {1}=@value", table, column);
            cmd.Parameters.Add("value", value);

            while (commandsQueue.Count > 0) Thread.Sleep(10);

            return SqlCE.ExecuteScalar<int>(cmd);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal T? GetValue<T>(object value) where T : struct
        {
            return (value == null || value is DBNull) ? (T?)null : (T?)Convert.ChangeType(value, typeof(T));
        }

        private Thread asyncThread = null;
        private Queue<SqlCeCommand> commandsQueue = new Queue<SqlCeCommand>();
        /// <summary>
        /// Executes the command async.
        /// </summary>
        /// <param name="command">The command.</param>
        private void ExecuteCommandAsync(SqlCeCommand command)
        {
            if (CACHE_ENABLED)
            {
                if (asyncThread == null)
                {
                    asyncThread = new Thread(new ThreadStart(CommandExecuter));
                    asyncThread.Priority = ThreadPriority.Lowest;
                    asyncThread.Name = "SqlCE Command Execution Thread";
                    asyncThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                    asyncThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                    asyncThread.Start();
                }

                commandsQueue.Enqueue(command);
                asyncThread.IsBackground = false;
            }
            else
                SqlCE.ExecuteNonQuery(command);
        }

        /// <summary>
        /// Commands the executer.
        /// </summary>
        private void CommandExecuter()
        {
            while (true)
            {
                if (commandsQueue.Count <= 0)
                    Thread.CurrentThread.IsBackground = true;
                while (commandsQueue.Count <= 0) Thread.Sleep(10);

                Thread.CurrentThread.IsBackground = false;
                SqlCE.ExecuteNonQuery(commandsQueue.Dequeue());
            }
        }

        #endregion

        #region IMovieDataSourcePlugin Members

        /// <summary>
        /// Gets the filename (empty for database data source).
        /// </summary>
        /// <value>The filename.</value>
        public string Filename { get; private set; }
        /// <summary>
        /// Gets the connection (null for file data source).
        /// </summary>
        /// <value>The connection.</value>
        public ConnectionStringStruct? Connection { get { return null; } }

        /// <summary>
        /// Gets the movies.
        /// </summary>
        /// <value>The movies.</value>
        public ObservableCollection<IMovie> Movies { get; private set; }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>The users.</value>
        public ObservableCollection<IUserProfile> Users { get; private set; }

        /// <summary>
        /// Creates the movie.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public IMovie CreateMovie(string title)
        {
            IMovie movie = new SqlCeMovie(AddRow(Table.Movies, new Dictionary<string, object>() { { "title", title } }), this);
            Movies.Add(movie);
            return movie;
        }

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IPerson CreatePerson(string name)
        {
            int? id = SearchRow(Table.Persons, "name", name);
            return new SqlCePerson(id.HasValue ? id.Value : AddRow(Table.Persons, new Dictionary<string, object>() { { "name", name } }), this, null);
        }

        /// <summary>
        /// Creates the genre.
        /// </summary>
        /// <param name="genre">The title.</param>
        /// <returns></returns>
        public IGenre CreateGenre(string title)
        {
            int? id = SearchRow(Table.Genres, "title", title);
            return new SqlCeGenre(id.HasValue ? id.Value : AddRow(Table.Genres, new Dictionary<string, object>() { { "title", title } }), this);
        }

        /// <summary>
        /// Creates the media file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public IMediaFile CreateMediaFile(string path)
        {
            return new SqlCeMediaFile(AddRow(Table.MediaFiles, new Dictionary<string, object>() { { "path", path } }), this);
        }

        /// <summary>
        /// Creates the video properties.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        public IVideoProperties CreateVideoProperties(long duration)
        {
            return new SqlCeVideoProperties(AddRow(Table.VideoProperties, new Dictionary<string, object>() { { "duration", duration } }), this);
        }

        /// <summary>
        /// Creates the audio properties.
        /// </summary>
        /// <param name="channels">The channels.</param>
        /// <returns></returns>
        public IAudioProperties CreateAudioProperties(int channels)
        {
            return new SqlCeAudioProperties(AddRow(Table.AudioProperties, new Dictionary<string, object>() { { "channels", channels } }), this);
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IUserProfile CreateUser(string name)
        {
            if (Users.ToList().Find(u => u.Name == name) != null)
                throw new ArgumentException("User already exists!");

            return new SqlCeUserProfile(AddRow(Table.UserProfiles, new Dictionary<string, object>() { { "name", name } }), this);
        }

        /// <summary>
        /// Creates the movie settings.
        /// </summary>
        /// <param name="movie">The movie.</param>
        /// <returns></returns>
        public IUserMovieSettings CreateMovieSettings(IMovie movie)
        {
            return new SqlCeUserMovieSettings(AddRow(Table.UserMovieSettings, new Dictionary<string, object>() { { "movies_id", movie.Id } }), this);
        }

        /// <summary>
        /// Forces the on title property changed event.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        public void ForceOnTitlePropertyChanged()
        {
            Movies.ToList().ForEach(m => m.ForceOnTitlePropertyChanged());
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                DbConnection.Close();
                DbConnection.Dispose();
            }
            catch (Exception e) { Trace.WriteLine(e.ToString()); }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the <see cref="E:PropetyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropetyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion
    }
}
