using MySqlAnalyzer.Models;

using System.Text;

namespace MySqlAnalyzer.Services;

public class ExportService
    {
    public string ExportToHtml ( DatabaseModel database )
        {
        var html = new StringBuilder();

        html.AppendLine ( "<!DOCTYPE html>" );
        html.AppendLine ( "<html>" );
        html.AppendLine ( "<head>" );
        html.AppendLine ( "    <title>Database Documentation - " + database.Name + "</title>" );
        html.AppendLine ( "    <style>" );
        html.AppendLine ( "        body { font-family: Arial, sans-serif; margin: 20px; }" );
        html.AppendLine ( "        h1 { color: #007ACC; }" );
        html.AppendLine ( "        h2 { border-bottom: 2px solid #007ACC; padding-bottom: 5px; }" );
        html.AppendLine ( "        table { border-collapse: collapse; width: 100%; margin-bottom: 20px; }" );
        html.AppendLine ( "        th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }" );
        html.AppendLine ( "        th { background-color: #f2f2f2; }" );
        html.AppendLine ( "        .sql { background-color: #f5f5f5; font-family: monospace; padding: 10px; border: 1px solid #ddd; }" );
        html.AppendLine ( "    </style>" );
        html.AppendLine ( "</head>" );
        html.AppendLine ( "<body>" );

        // Database Overview
        html.AppendLine ( "<h1>Database: " + database.Name + "</h1>" );
        html.AppendLine ( "<p><strong>Character Set:</strong> " + database.DefaultCharacterSet + "</p>" );
        html.AppendLine ( "<p><strong>Collation:</strong> " + database.DefaultCollation + "</p>" );

        // Tables
        html.AppendLine ( "<h2>Tables (" + database.Tables.Count + ")</h2>" );
        foreach ( var table in database.Tables )
            {
            html.AppendLine ( "<h3>Table: " + table.Name + "</h3>" );
            html.AppendLine ( "<p>" + table.Comment + "</p>" );

            // Columns
            html.AppendLine ( "<h4>Columns</h4>" );
            html.AppendLine ( "<table>" );
            html.AppendLine ( "<tr><th>Name</th><th>Type</th><th>Nullable</th><th>Default</th><th>Key</th><th>Comment</th></tr>" );

            foreach ( var column in table.Columns )
                {
                html.AppendLine ( "<tr>" );
                html.AppendLine ( "<td>" + column.Name + "</td>" );
                html.AppendLine ( "<td>" + column.ColumnType + "</td>" );
                html.AppendLine ( "<td>" + ( column.IsNullable ? "YES" : "NO" ) + "</td>" );
                html.AppendLine ( "<td>" + column.DefaultValue + "</td>" );
                html.AppendLine ( "<td>" + column.ColumnKey + "</td>" );
                html.AppendLine ( "<td>" + column.Comment + "</td>" );
                html.AppendLine ( "</tr>" );
                }

            html.AppendLine ( "</table>" );

            // SQL
            html.AppendLine ( "<h4>SQL CREATE Statement</h4>" );
            html.AppendLine ( "<div class='sql'>" );
            html.AppendLine ( "<pre>" + table.CreateScript + "</pre>" );
            html.AppendLine ( "</div>" );
            }

        html.AppendLine ( "</body>" );
        html.AppendLine ( "</html>" );

        return html.ToString ();
        }

    public void ExportToFile ( string content, string filePath )
        {
        File.WriteAllText ( filePath, content );
        }
    }
