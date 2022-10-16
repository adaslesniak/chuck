using System;
using System.Data.SQLite;

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
                using(SQLiteCommand operation = new(command, data)) {
                    toDo(operation);
                }
            }
        } catch(Exception error) {
            Log.Error($"Issue with sequelite ({command})", error);
        }
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
