using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using SchemaExplorer;
using CodeSmith.Engine;
using System.ComponentModel;
using System.Windows.Forms;

public class CodeTemplatTool:CodeTemplate
{
    #region 属性
    private string _fileDesc= string.Empty;
    
    [Category("Custom")]
	[Description("文件描述信息（可不填）")]
    public string FileDesc
    {
        get
        {
            if(_fileDesc.Length <= 0)
                return "单独生成的代码，请在代码生成后手动添加【描述】信息";
            else
                return _fileDesc;
        }
        set
        {
            _fileDesc = value;
        }
    }
    
    private string _companyName= string.Empty;
    
    [Category("Custom")]
	[Description("公司名称（可不填）")]
    public string CompanyName
    {
        get
        {
            if(_companyName.Length <= 0)
                return "zhumingming(Berton)";
            else
                return _companyName;
        }
        set
        {
            _companyName = value;
        }
    }
    
    /// <summary>
    /// 创建人
    /// </summary>
    public string CreatePerson = "zhumingming(Berton)";
    
    /// <summary>
    /// BusinessRepository类名称后缀
    /// </summary>
    public string BusinessRepositorySuffix = "Repository";
    
    #endregion
    
    /// <summary>
    /// 根据列对象获得列描述
    /// </summary>
    /// <param name="column">表列</param>
    /// <returns></returns>
    public string GetColumnDesc(ColumnSchema column)
    {
        string tempDesc=column.Name;
        if(column.Description.Length <= 0)
            return tempDesc;
        else
            return column.Description;   
    }
    
    /// <summary>
    /// 根据列对象获得列描述
    /// </summary>
    /// <param name="viewColumn">视图列</param>
    /// <returns></returns>
    public string GetColumnDesc(ViewColumnSchema viewColumn)
    {
        string tempDesc=viewColumn.Name;
        if(viewColumn.Description.Length <= 0)
            return tempDesc;
        else
            return viewColumn.Description;   
    }
    
    #region 获得名称
    /// <summary>
    /// 根据表对象获得名称
    /// </summary>
    /// <param name="table">表</param>
    /// <returns></returns>
    public string GetTableName(TableSchema table)
    {
        return MakePascal(table.Name);
    }
    
    /// <summary>
    /// 根据视图对象获得名称
    /// </summary>
    /// <param name="view">视图</param>
    /// <returns></returns>
    public string GetTableName(ViewSchema view)
    {
        return MakePascal(view.Name);
    }
    
    /// <summary>
    /// 根据表对象获得Repository类名称
    /// </summary>
    /// <param name="table">表</param>
    /// <returns></returns>
    public string GetRepositoryClassName(TableSchema table)
    {
        return MakePascal(table.Name + BusinessRepositorySuffix);
    }
    
    /// <summary>
    /// 根据视图对象获得Repository类名称
    /// </summary>
    /// <param name="view">视图</param>
    /// <returns></returns>
    public string GetRepositoryClassName(ViewSchema view)
    {
        return MakePascal(view.Name + BusinessRepositorySuffix);
    }
    
    #endregion
    
    #region 主键操作
    /// <summary>
    /// 根据表对象获得主键的类型(C#中)
    /// </summary>
    /// <param name="table">表</param>
    /// <returns></returns>
    public string GetPrimaryKeyType(TableSchema table)
    {
        if (table.PrimaryKey != null)
        {
            if (table.PrimaryKey.MemberColumns.Count == 1)
            {
                return GetCSharpTypeFromDBFieldType(table.PrimaryKey.MemberColumns[0]);
            }
            else
            {
                return string.Empty;
            }
        }
        else
        {
            throw new ApplicationException("This template will only work on MyTables with a primary key.");
        }
    }
    
    /// <summary>
    /// 根据表对象获得主键的类型(SQL中)
    /// </summary>
    /// <param name="table">表</param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public string GetPrimaryKeyType(TableSchema table,int flag)
    {
        if (table.PrimaryKey != null)
        {
            if (table.PrimaryKey.MemberColumns.Count == 1)
            {
                return GetSqlDbType(table.PrimaryKey.MemberColumns[0]);
            }
            else
                return string.Empty;
        }
        else
        {
            throw new ApplicationException("This template will only work on MyTables with a primary key.");
        }
    }
    
    /// <summary>
    /// 根据表对象获得主键的参数类型(最后不包含'?')
    /// </summary>
    /// <param name="table">表</param>
    /// <returns></returns>
    public string GetPKParameter(TableSchema table)
    {
        string tempType=GetPrimaryKeyType(table);
        if(tempType.EndsWith("?"))
            tempType=tempType.Substring(0,tempType.Length-1);
        
        return tempType;
    }
     
    /// <summary>
    /// 根据表对象获得主键的名称（使用Pascal命名方式）
    /// </summary>
    /// <param name="table">表</param>
    /// <param name="prefix">前缀</param>
    /// <returns></returns>
    public string GetPrimaryKeyName(TableSchema table,string prefix)
    {
        return prefix + MakePascal(GetPKName(table));
    }
    
    /// <summary>
    /// 根据表对象获得主键变量的名称（使用Camel方式命名)
    /// </summary>
    /// <param name="table">表</param>
    /// <returns></returns>
    public string GetPKVarName(TableSchema table)
    {
        return "_"+MakeCamel(GetPKName(table));
    }
    
    /// <summary>
    /// 根据表对象获得主见的默认值
    /// </summary>
    /// <param name="table">表</param>
    /// <returns></returns>
    public string GetPKDefaultValue(TableSchema table)
    {
        if (table.PrimaryKey != null)
        {
            if (table.PrimaryKey.MemberColumns.Count == 1)
            {
                return GetMemberVariableDefaultValue(table.PrimaryKey.MemberColumns[0]);
            }
            else
                return string.Empty;
        }
        else
        {
            throw new ApplicationException("This template will only work on MyTables with a primary key.");
        }
    }
    
    /// <summary>
    /// 根据表对象获得主键的名称(原始方法)
    /// </summary>
    /// <param name="table">表</param>
    /// <returns></returns>
    public string GetPKName(TableSchema table)
    {
        if (table.PrimaryKey != null)
        {
            if (table.PrimaryKey.MemberColumns.Count == 1)
            {
                return table.PrimaryKey.MemberColumns[0].Name;
            }
            else
            {
                throw new ApplicationException("This template will not work on primary keys with more than one member column.");
            }
        }
        else
        {
            throw new ApplicationException("This template will only work on tables with a primary key.");
        }
    }
    
    /// <summary>
    /// 根据表对象获得排序列
    /// </summary>
    /// <param name="table">表</param>
    /// <returns></returns>
    public string GetSortedKeyName(TableSchema table)
    {
        foreach(ColumnSchema one in table.Columns) 
        {
          if(one.Name.Equals("createdate",StringComparison.CurrentCultureIgnoreCase)||
              one.Name.Equals("createtime",StringComparison.CurrentCultureIgnoreCase))
          {
              return one.Name;
          }
        }
        ColumnSchema orderCol=table.Columns.Find(p=>p.Name.Contains("Date")||p.Name.Contains("Time")||p.Name.Contains("date")||p.Name.Contains("time"));
        if(orderCol!=null)
        {
            return orderCol.Name;
        }
        return GetPKName(table);
    }
    
    /// <summary>
    /// 根据视图对象获得排序列
    /// </summary>
    /// <param name="view">视图</param>
    /// <returns></returns>
    public string GetSortedKeyName(ViewSchema view)
    {
          foreach(ViewColumnSchema one in view.Columns) 
          {
              if(one.Name.Equals("createdate",StringComparison.CurrentCultureIgnoreCase)||
                  one.Name.Equals("creattime",StringComparison.CurrentCultureIgnoreCase))
              {
                  return one.Name;
              }
          }
          ViewColumnSchema orderCol=view.Columns.Find(p=>p.Name.EndsWith("Date")||p.Name.EndsWith("Time"));
          if(orderCol!=null)
          {
              return orderCol.Name;
          }
          return "in View: "+view.Name+" can't find sort colume";
    }
    #endregion

    #region 成员操作
    
    /// <summary>
    /// 根据表中列对象获得列的类型(C#中)
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetMemberKeyType(ColumnSchema column)
    {
        return GetCSharpTypeFromDBFieldType(column);
    }
    
    /// <summary>
    /// 根据视图中列对象获得列的类型(C#中)
    /// </summary>
    /// <param name="viewColumn"></param>
    /// <returns></returns>
    public string GetMemberKeyType(ViewColumnSchema viewColumn)
    {
        return GetCSharpTypeFromDBFieldType(viewColumn);
    }
    
    /// <summary>
    /// 根据表中列对象获得列的类型(SQL中)
    /// </summary>
    /// <param name="column"></param>
    /// <param name="flage"></param>
    /// <returns></returns>
    public string GetMemberKeyType(ColumnSchema column,int flage)
    {
        return GetSqlDbType(column);
    }
    
    /// <summary>
    /// 根据表中列对象获得列的参数类型(最后不包含'?')
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetMKParameter(ColumnSchema column)
    {
        string tempType=GetMemberKeyType(column);
        if(tempType.EndsWith("?"))
            tempType=tempType.Substring(0,tempType.Length-1);
        
        return tempType;
    }
    
    /// <summary>
    /// 根据视图中列对象获得列的参数类型(最后不包含'?')
    /// </summary>
    /// <param name="viewColumn"></param>
    /// <returns></returns>
    public string GetMKParameter(ViewColumnSchema viewColumn)
    {
        string tempType=GetMemberKeyType(viewColumn);
        if(tempType.EndsWith("?"))
            tempType=tempType.Substring(0,tempType.Length-1);
        
        return tempType;
    }
    
    /// <summary>
    /// 根据表中列对象获得列的名称（使用Pascal方式命名)
    /// </summary>
    /// <param name="column"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public string GetMemberKeyName(ColumnSchema column,string prefix)
    {
        return prefix + MakePascal(GetNameFromDBFieldName(column));
    }
    
    /// <summary>
    /// 根据视图中列对象获得列的名称（使用Pascal方式命名)
    /// </summary>
    /// <param name="viewColumn"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public string GetMemberKeyName(ViewColumnSchema viewColumn,string prefix)
    {
        return prefix + MakePascal(GetNameFromDBFieldName(viewColumn));
    }
    
    /// <summary>
    /// 根据表中列对象获得列的成员变量名称（使用Camel方式命名）
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetMKVarName(ColumnSchema column)
    {
        return "_" + MakeCamel(GetNameFromDBFieldName(column));
    }
    
    /// <summary>
    /// 根据视图中列对象获得列的成员变量名称（使用Camel方式命名）
    /// </summary>
    /// <param name="viewcolumn"></param>
    /// <returns></returns>
    public string GetMKVarName(ViewColumnSchema viewcolumn)
    {
        return "_" + MakeCamel(GetNameFromDBFieldName(viewcolumn));
    }
    
    /// <summary>
    /// 根据表中列对象获得成员的名称(原始方法)
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    private string GetNameFromDBFieldName(ColumnSchema column)
    {
        return column.Name;
    }
    
    /// <summary>
    /// 根据视图中列对象获得成员的名称(原始方法)
    /// </summary>
    /// <param name="viewColumn"></param>
    /// <returns></returns>
    private string GetNameFromDBFieldName(ViewColumnSchema viewColumn)
    {
        return viewColumn.Name;
    }
    
    /// <summary>
    /// 根据表中列对象获得列的C#数据类型
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetCSharpTypeFromDBFieldType(ColumnSchema column)
    {
        switch (column.DataType)
        {
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.StringFixedLength:
            case DbType.String:
            {
                return "string";
            }
            case DbType.VarNumeric:
            case DbType.Currency:
            case DbType.Decimal: 
            {
                return "decimal?";
            }
            case DbType.Date: 
            case DbType.DateTime:
            {
                return "DateTime?";
            }
            case DbType.Double: return "double?";
            case DbType.Guid: return "Guid?";
            case DbType.Binary: return "byte[]";
            case DbType.Boolean: return "bool?";
            case DbType.Byte: return "byte?";
            case DbType.Int16: return "short?";
            case DbType.Int32: return "int?";
            case DbType.Int64: return "long?";
            case DbType.Object: return "object";
            case DbType.SByte: return "sbyte?";
            case DbType.Single: return "float?";
            case DbType.Time: return "TimeSpan?";
            case DbType.UInt16: return "ushort?";
            case DbType.UInt32: return "uint?";
            case DbType.UInt64: return "ulong?";
            default:
            {
                return "__UNKNOWN__" + column.NativeType;
            }
        }
    }
    
    /// <summary>
    /// 根据视图中列对象获得列的C#数据类型
    /// </summary>
    /// <param name="viewColumn"></param>
    /// <returns></returns>
    public string GetCSharpTypeFromDBFieldType(ViewColumnSchema viewColumn)
    {
        switch (viewColumn.DataType)
        {
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.StringFixedLength:
            case DbType.String:
            {
                return "string";
            }
            case DbType.VarNumeric:
            case DbType.Currency:
            case DbType.Decimal: 
            {
                return "decimal?";
            }
            case DbType.Date: 
            case DbType.DateTime:
            {
                return "DateTime?";
            }
            case DbType.Double: return "double?";
            case DbType.Guid: return "Guid?";
            case DbType.Binary: return "byte[]";
            case DbType.Boolean: return "bool?";
            case DbType.Byte: return "byte?";
            case DbType.Int16: return "short?";
            case DbType.Int32: return "int?";
            case DbType.Int64: return "long?";
            case DbType.Object: return "object";
            case DbType.SByte: return "sbyte?";
            case DbType.Single: return "float?";
            case DbType.Time: return "TimeSpan?";
            case DbType.UInt16: return "ushort?";
            case DbType.UInt32: return "uint?";
            case DbType.UInt64: return "ulong?";
            default:
            {
                return "__UNKNOWN__" + viewColumn.NativeType;
            }
        }
    }
    
    /// <summary>
    /// 根据表中列对象获得对应的默认值
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetMemberVariableDefaultValue(ColumnSchema column)
    {
        switch (column.DataType)
        {
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.String:
            case DbType.StringFixedLength:
            {
                return "string.Empty";
            }
            case DbType.Int16:
            case DbType.Int32:
            case DbType.Int64:
            {
                return "0";
            }
            case DbType.Date: 
            case DbType.DateTime:
            {
                return "DateTime.MinValue";
            }
            case DbType.VarNumeric:
            case DbType.Double:
            case DbType.Currency:
            case DbType.Decimal:
            {
                return "0.0M";
            }
            case DbType.Guid:return "Guid.Empty";
            case DbType.Boolean:return "false";
            default:return string.Empty;
        }
    }
    
    /// <summary>
    /// 根据视图中列对象获得对应的默认值
    /// </summary>
    /// <param name="viewColumn"></param>
    /// <returns></returns>
    public string GetMemberVariableDefaultValue(ViewColumnSchema viewColumn)
    {
        switch (viewColumn.DataType)
        {
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.String:
            case DbType.StringFixedLength:
            {
                return "string.Empty";
            }
            case DbType.Int16:
            case DbType.Int32:
            case DbType.Int64:
            {
                return "0";
            }
            case DbType.Date: 
            case DbType.DateTime:
            {
                return "DateTime.MinValue";
            }
            case DbType.VarNumeric:
            case DbType.Double:
            case DbType.Currency:
            case DbType.Decimal:
            {
                return "0.0M";
            }
            case DbType.Guid:return "Guid.Empty";
            case DbType.Boolean:return "false";
            default:return string.Empty;
        }
    }
    
    /// <summary>
    /// 根据表对象中的列获取数据转化方法
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetConvertMethod(ColumnSchema column)
    {
        switch (column.DataType)
        {
            case DbType.Byte:return "ConvertByte";
            case DbType.Int16:
            case DbType.Int32:
            {
                return "ConvertInt";
            }
            case DbType.Int64:
            {
                return "ConvertLong";
            }
            case DbType.AnsiStringFixedLength:
            case DbType.AnsiString:
            case DbType.String:
            case DbType.StringFixedLength:
            {
                return "ConvertStr";
            }
            case DbType.Boolean:return "ConvertBool";
            case DbType.Guid:return "ConvertGuid";
            case DbType.Currency:
            case DbType.Decimal:
            {
                return "ConvertDecimal";
            }
            case DbType.DateTime:
            case DbType.Date:
            {
                return "ConvertDate";
            }
            case DbType.Binary:
            {
                return "GetBytes";
            }
            default:
            {
                return "__SQL__" + column.DataType;
            }
        }
    }
    
    /// <summary>
    /// 根据视图中的列获取数据转化方法
    /// </summary>
    /// <param name="viewColumn"></param>
    /// <returns></returns>
    public string GetConvertMethod(ViewColumnSchema viewColumn)
    {
        switch (viewColumn.DataType)
        {
            case DbType.Byte:return "ConvertByte";
            case DbType.Int16:
            case DbType.Int32:
            {
                return "ConvertInt";
            }
            case DbType.Int64:
            {
                return "ConvertLong";
            }
            case DbType.AnsiStringFixedLength:
            case DbType.AnsiString:
            case DbType.String:
            case DbType.StringFixedLength:
            {
                return "ConvertStr";
            }
            case DbType.Boolean:return "ConvertBool";
            case DbType.Guid:return "ConvertGuid";
            case DbType.Currency:
            case DbType.Decimal:
            {
                return "ConvertDecimal";
            }
            case DbType.DateTime:
            case DbType.Date:
            {
                return "ConvertDate";
            }
            case DbType.Binary:
            {
                return "GetBytes";
            }
            default:
            {
                return "__SQL__" + viewColumn.DataType;
            }
        }
    }
    
    /// <summary>
    /// 根据表对象中的列获得列的列定义的数据类型（在C#中为SqlDbType）
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetSqlDbType(ColumnSchema column)
    {
        string prefix="SqlDbType.";
        switch (column.NativeType)
        {
            case "bigint": return prefix + "BigInt";
            case "binary": return prefix + "Binary";
            case "bit": return prefix + "Bit";
            case "char": return prefix + "Char";
            case "datetime": return prefix + "DateTime";
            case "decimal": return prefix + "Decimal";
            case "float": return prefix + "Float";
            case "image": return prefix + "Image";
            case "int": return prefix + "Int";
            case "money": return prefix + "Money";
            case "nchar": return prefix + "NChar";
            case "ntext": return prefix + "NText";
            case "numeric": return prefix + "Decimal";
            case "nvarchar": return prefix + "NVarChar";
            case "real": return prefix +"Real";
            case "smalldatetime": return prefix +"SmallDateTime";
            case "smallint": return prefix +"SmallInt";
            case "smallmoney": return prefix + "SmallMoney";
            case "sql_variant": return prefix + "Variant";
            case "sysname": return prefix + "NChar";
            case "text": return prefix + "Text";
            case "timestamp": return prefix + "Timestamp";
            case "tinyint": return prefix + "TinyInt";
            case "uniqueidentifier": return prefix + "UniqueIdentifier";
            case "varbinary": return prefix + "VarBinary";
            case "varchar": return prefix + "VarChar";
            default: return "__UNKNOWN__" + column.NativeType;
        }
    }
    
    /// <summary>
    /// 根据视图对象中的列获得列的列定义的数据类型（在C#中为SqlDbType）
    /// </summary>
    /// <param name="viewColumn"></param>
    /// <returns></returns>
    public string GetSqlDbType(ViewColumnSchema viewColumn)
    {
        string prefix="SqlDbType.";
        switch (viewColumn.NativeType)
        {
            case "bigint": return prefix + "BigInt";
            case "binary": return prefix + "Binary";
            case "bit": return prefix + "Bit";
            case "char": return prefix + "Char";
            case "datetime": return prefix + "DateTime";
            case "decimal": return prefix + "Decimal";
            case "float": return prefix + "Float";
            case "image": return prefix + "Image";
            case "int": return prefix + "Int";
            case "money": return prefix + "Money";
            case "nchar": return prefix + "NChar";
            case "ntext": return prefix + "NText";
            case "numeric": return prefix + "Decimal";
            case "nvarchar": return prefix + "NVarChar";
            case "real": return prefix +"Real";
            case "smalldatetime": return prefix +"SmallDateTime";
            case "smallint": return prefix +"SmallInt";
            case "smallmoney": return prefix + "SmallMoney";
            case "sql_variant": return prefix + "Variant";
            case "sysname": return prefix + "NChar";
            case "text": return prefix + "Text";
            case "timestamp": return prefix + "Timestamp";
            case "tinyint": return prefix + "TinyInt";
            case "uniqueidentifier": return prefix + "UniqueIdentifier";
            case "varbinary": return prefix + "VarBinary";
            case "varchar": return prefix + "VarChar";
            default: return "__UNKNOWN__" + viewColumn.NativeType;
        }
    }
   
    #endregion
    
    #region 命名方法
    /// <summary>
    /// Camel命名方法
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string MakeCamel(string value)
    {
        return value.Substring(0, 1).ToLower() + value.Substring(1);
    }
    
    /// <summary>
    /// Pascal命名方法
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string MakePascal(string value)
    {
        return value.Substring(0, 1).ToUpper() + value.Substring(1);
    }
    
    /// <summary>
    /// Plural命名方法
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string MakePlural(string name)
    {
        Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
        Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
        Regex plural3 = new Regex("(?<keep>[sxzh])$");
        Regex plural4 = new Regex("(?<keep>[^sxzhy])$");
        
        if(plural1.IsMatch(name))
            return plural1.Replace(name, "${keep}ies");
        else if(plural2.IsMatch(name))
            return plural2.Replace(name, "${keep}s");
        else if(plural3.IsMatch(name))
            return plural3.Replace(name, "${keep}es");
        else if(plural4.IsMatch(name))
            return plural4.Replace(name, "${keep}s");
        
        return name;
    }
    
    /// <summary>
    /// Single命名方法
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string MakeSingle(string name)
    {
        Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
        Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
        Regex plural3 = new Regex("(?<keep>[sxzh])es$");
        Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");
        
        if(plural1.IsMatch(name))
            return plural1.Replace(name, "${keep}y");
        else if(plural2.IsMatch(name))
            return plural2.Replace(name, "${keep}");
        else if(plural3.IsMatch(name))
            return plural3.Replace(name, "${keep}");
        else if(plural4.IsMatch(name))
            return plural4.Replace(name, "${keep}");
        
        return name;
    }
    #endregion
    
    /// <summary>
    /// 打印文件头部信息
    /// </summary>
    public void PrintHeader()
    {
        Response.WriteLine();
        Response.WriteLine("///=========================================================================");
        Response.WriteLine("/// 版    权： Coypright 2012 - {0} @ {1}",DateTime.Now.Year,_companyName);
        Response.WriteLine("/// 文件名称： {0}",GetFileName());
        Response.WriteLine("/// 模版作者： zhumingming(berton) 最后修改于 2016-5-27 16:10:10");
        Response.WriteLine("/// 作者邮箱： zhumingming1040@163.com,937553351@qq.com");
        Response.WriteLine("/// 描    述： {0}",FileDesc);
        Response.WriteLine("/// 创 建 人： {0} (CodeSmith V6.5.0 自动生成的代码 模板V4.0)",CreatePerson);
        Response.WriteLine("/// 创建时间： {0}", DateTime.Now);
        Response.WriteLine("///=========================================================================");
        Response.WriteLine();
    }
    
    #region 重写
    
    //修正导出代码到文件时乱码的问题
    //public override void Render(TextWriter writer)
    //{
         //在这里定义StreamWriter的Encoding为UTF8即可
    //     StreamWriter fw = new StreamWriter(GetFileName(), false,System.Text.Encoding.UTF8);
    //     this.Response.AddTextWriter(fw);
    //      base.Render(writer);
    //     fw.Close();
    //}
    #endregion
    
    #region 统一错误提示 2014-3-24日加入
    public void ShowErrorMsg(string message)
    {
        MessageBox.Show(message,"错误提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
    }
    #endregion
}