using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace MySqlAnalyzer.Models;

public class ParameterModel
{
    public string? Name { get; set; }
    public string? RoutineName { get; set; }
    public string? RoutineType { get; set; } // PROCEDURE or FUNCTION
    public string? DataType { get; set; }
    public ParameterMode ParameterMode { get; set; }
    public long CharacterMaximumLength { get; set; }
    public int OrdinalPosition { get; set; }
    public int NumericPrecision { get; set; }
    public int DateTimePrecision { get; set; }
    public int NumericScale { get; set; }
    public string? CharacterSet { get; set; }
    public string? Collation { get; set; }
    public string? DtdIdentifier { get; set; }



}

public enum ParameterMode
{
    IN,
    OUT,
    INOUT
}
