using System;
using System.Data.SQLite;
using System.Reflection.PortableExecutable;

namespace ChuckleIt;
internal class SqlLiteChucks : IChucksKeeper
{

    const string ChucksTable = "Chuck";
    const string ChucksColumn = "Joke";
    const string SourceColumn = "Whereabout";

    void IChucksKeeper.KeepHim(string suspect, string source = "unknown") {
        AccessData($"insert into {ChucksTable}" + $"({ChucksColumn}, {SourceColumn})" + $"values('{suspect}','{source}')", operation => {
            operation.ExecuteNonQuery();
        });
    }

    bool IChucksKeeper.IsAlreadyThere(string suspect, DuplicatesReferee isDuplicate) {
        var isThere = false;
        AccessData($"select Joke from {ChucksTable}", operation => {
            using(SQLiteDataReader reader = operation.ExecuteReader()) {
                while(reader.Read() && false == isThere) {
                    var nextKnown = reader.GetString(0);
                    isThere |= isDuplicate(nextKnown, suspect);
                }
            }     
        });
        return isThere;
    }

    delegate void DataAccessor(SQLiteCommand operation);

    void AccessData(string command, DataAccessor toDo) {
        try {
            using(SQLiteConnection data = new(DataSourcePath())) {
                data.Open();
                //Debug(data);
                using(SQLiteCommand operation = new(command, data)) {
                    toDo(operation);
                }
            }
        } catch(Exception error) {
            Log.Error("I'm too stupid for that");
        }

        //void Debug(SQLiteConnection toBeChecked) {
        //    var schemaQuery = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table'", toBeChecked);
        //    var schemaReader = schemaQuery.ExecuteReader();
        //    var xxx = schemaReader.GetSchemaTable();
        //    while(schemaReader.Read()) {
        //        var table = schemaReader.GetString(0);
        //        Log.Info($"found table: {table}");
        //    }
        //}
    }

    string DataSourcePath() {
        var home = Environment.GetEnvironmentVariable("HOME") ?? "";
        if(false == string.IsNullOrEmpty(home)) {
            home = System.IO.Path.Combine(home, "site", "wwwroot");
        }
        var path = System.IO.Path.Combine(home, "chuck.db");
        return $"Data Source={path}";
    }
}
