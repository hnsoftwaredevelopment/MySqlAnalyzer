using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlAnalyzer.Helpers;
public class DBNames
{
    #region Sql commands
    public static readonly string SqlAnd = " AND ";
    public static readonly string SqlAsc = " ASC ";
    public static readonly string SqlAs = " AS ";
    public static readonly string SqlBetween = " BETWEEN ";
    public static readonly string SqlCast = "CAST( ";
    public static readonly string SqlConcat = "CONCAT( ";
    public static readonly string SqlCount = " COUNT( ";
    public static readonly string SqlCountAll = " COUNT(*) ";
    public static readonly string SqlCountUnique = " COUNT(DISTINCT ";
    public static readonly string SqlDelete = "DELETE ";
    public static readonly string SqlDeleteFrom = "DELETE FROM ";
    public static readonly string SqlDesc = " DESC ";
    public static readonly string SqlFrom = " FROM ";
    public static readonly string SqlIn = "IN ";
    public static readonly string SqlInnerJoin = "INNER JOIN ";
    public static readonly string SqlInsert = "INSERT INTO ";
    public static readonly string SqlIsNull = " IS NULL ";
    public static readonly string SqlLimit = " LIMIT ";
    public static readonly string SqlLimit1 = " LIMIT 1 ";
    public static readonly string SqlLike = " LIKE ";
    public static readonly string SqlLower = " LOWER ( ";
    public static readonly string SqlMax = "MAX( ";
    public static readonly string SqlMin = "MIN( ";
    public static readonly string SqlOn = " ON ";
    public static readonly string SqlOr = " OR ";
    public static readonly string SqlOrder = " ORDER BY ";
    public static readonly string SqlOrderBy = " ORDER BY ";
    public static readonly string SqlSelect = "SELECT ";
    public static readonly string SqlSelectAll = "SELECT *";
    public static readonly string SqlSelectDistinct = "SELECT DISTINCT ";
    public static readonly string SqlSet = " SET ";
    public static readonly string SqlSum = " SUM( ";
    public static readonly string SqlSubString = " SUBSTRING( ";
    public static readonly string SqlUnsigned = " as UNSIGNED) ";
    public static readonly string SqlUnionAll = "UNION ALL";
    public static readonly string SqlUpdate = "UPDATE ";
    public static readonly string SqlValues = " VALUES ";
    public static readonly string SqlWhere = " WHERE ";
    public static readonly string SqlWithRecursive = "WITH RECURSIVE ";
    #endregion Sql commands

    #region Database
    public static readonly string Database = "modelbuilder";
    #endregion Database
}
