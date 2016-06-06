using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using SchemaExplorer;
using CodeSmith.Engine;
using System.ComponentModel;
using System.Diagnostics;

public class Main : CodeTemplate
{
    #region 属性
    private DatabaseSchema _sourceDataBase = null;

    [Description("选择目标数据库")]
    [Category("1.Choose Source DataBase")]
    public DatabaseSchema SourceDataBase
    {
        get
        {
            return _sourceDataBase;
        }
        set
        {
            _sourceDataBase = value;
        }
    }

    //private string _rootNamespace=string.Empty;

    //[Description("输入项目顶级名称空间（很重要）")]
    //[Category("3.Custom Root Namespace")]
    //public string RootNamespace 
    //{
    //    get
    //    {
    //        if(_rootNamespace.Length<=0)
    //        { 
    //             //ShowErrorMsg("必须输入顶级名称空间");
    //             return "DefaultNamespace";
    //         }
    //         return _rootNamespace;
    //     }
    //     set
    //     {
    //         _rootNamespace = value;
    //     } 
    // }

    private string _outputDirectory = string.Empty;

    [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [Description("选择生成的代码最后输出的目录")]
    [Category("2.Choose Output Directory")]
    [DefaultValue("\\")]
    public string OutputDirectory
    {
        get
        {
            return _outputDirectory;
        }
        set
        {
            if (value != null && !value.EndsWith("\\"))
            {
                value += "\\";
            }
            _outputDirectory = value;
        }
    }

    private string _createPerson = "zhumingming(Berton)";

    [Description("自定义属性-代码创建人（默认zhumingming(Berton))")]
    [Category("Custom")]
    public string CreatePerson
    {
        get
        {
            return _createPerson;
        }
        set
        {
            _createPerson = value;
        }
    }

    private string _entityNamescape= "Entity";
    
    [Description("自定义 Entity 层名称空间(格式项目简称.Entity层名称,如root.Entity)，默认为Entity")]
    [Category("3.Custom Layer Namespace")]
    public string EntityNamespace
    {
        get
        {
            return _entityNamescape;
        }
        set
        {
            _entityNamescape = value;
        }
    }
    
    private string _businessRepositoryNamespace = "BusinessRepository";

    [Description("自定义 BusinessRepository 层名称空间(格式项目简称.BusinessRepository层名称,如root.Repository)，默认为Repository")]
    [Category("3.Custom Layer Namespace")]
    public string BusinessRepositoryNamespace
    {
        get
        {
            return _businessRepositoryNamespace;
        }
        set
        {
            _businessRepositoryNamespace = value;
        }
    }
    
    private string _commonNamescape= "Common";
    
    [Description("自定义 Common 层名称空间(格式项目简称.Common层名称,如root.Common)，默认为Common")]
    [Category("3.Custom Layer Namespace")]
    public string CommonNamespace
    {
        get
        {
            return _commonNamescape;
        }
        set
        {
            _commonNamescape = value;
        }
    }
    
     private string _dBUtilityNamescape= "DBUtility";
    
    [Description("自定义 DBUtility 层名称空间(格式项目简称.DBUtility层名称,如root.DBUtility)，默认为DBUtility")]
    [Category("3.Custom Layer Namespace")]
    public string DBUtilityNamespace
    {
        get
        {
            return _dBUtilityNamescape;
        }
        set
        {
            _dBUtilityNamescape = value;
        }
    }

    private string _businessRepositorySuffix = "Repository";

    [Description("自定义 BusinessRepository 类(文件)名称后缀，默认为Repository")]
    [Category("4.Custom Layer Suffix")]
    public string BusinessRepositorySuffix
    {
        get
        {
            return _businessRepositorySuffix;
        }
        set
        {
            _businessRepositorySuffix = value;
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
    
    private string _solutionName=string.Empty;
    
    [Category("5.Custom Solution Name")]
	[Description("自定义解决方案名称")]
    public string SolutionName
    {
        get
        {
            if(_solutionName.Length<=0)
                return "solution";
            else
                return _solutionName;
        }
        set
        {
            _solutionName = value;
        }
    }
    //[Description(" Model 层文件路径，根据输入自动匹配.")]
    //public string ModelFilePath
    //{
    //    get
    //    {
    //        if (_rootNamespace.Length <= 0)
    //            return string.Empty;
    //        if (_modelNamespace.Length <= 0)
    //            return "Model";
    //        return string.Format("{0}.{1}", _rootNamespace, _modelNamespace);
    //    }
    //}

    //[Description(" DAL 层文件路径，根据输入自动匹配.")]
    //public string DALFilePath
    //{
    //    get
    //    {
    //        if (_rootNamespace.Length <= 0)
    //            return string.Empty;
    //        if (_dALNamespace.Length <= 0)
    //            return "DAL";
    //        return string.Format("{0}.{1}", _rootNamespace, _dALNamespace);
    //    }
    //}

    //[Description(" BLL 层文件路径，根据输入自动匹配.")]
    //public string BLLFilePath
    //{
    //    get
    //    {
    //        if (_rootNamespace.Length <= 0)
    //            return string.Empty;
    //        if (_bLLNamespace.Length <= 0)
    //            return "BLL";
    //        return string.Format("{0}.{1}", _rootNamespace, _bLLNamespace);
    //    }
    //}
    #endregion

    #region  生成代码
    /// <summary>
    /// 生成数据实体代码
    /// </summary>
    /// <param name="tables"></param>
    /// <returns></returns>
    public int GenerateEntityClasses(TableSchemaCollection tables)
    {
        if (tables == null || tables.Count <= 0)
            return 0;
        int tempIntTableNum = 0;
        CodeTemplate EntityTemplate = GetCodeTemplate("Entity.cst");
        foreach (TableSchema table in tables)
        {
            EntityTemplate.SetProperty("TargetTable", table);
            EntityTemplate.SetProperty("EntityNamespace", EntityNamespace);
            EntityTemplate.SetProperty("CreatePerson", CreatePerson);
            EntityTemplate.SetProperty("CompanyName", CompanyName);
            EntityTemplate.SetProperty("FileDesc", "表[" + table.Name + "]的 Entity 层代码");
            string tempFilePath = string.Format(@"{0}{1}\{2}", this.OutputDirectory, EntityNamespace, EntityTemplate.GetFileName());
            EntityTemplate.RenderToFile(tempFilePath, true);
            WriteInfo("成功在路径[" + this.OutputDirectory + EntityNamespace + "] 生成 Entity 层代码文件：" + EntityTemplate.GetFileName() + "");
            tempIntTableNum++;
        }
        WriteInfo("-----成功在路径[" + this.OutputDirectory + EntityNamespace + "] 生成 " + tempIntTableNum + " 个 Entity 层代码文件-----");
        return tempIntTableNum;
    }
    
    /// <summary>
    /// 生成数据实体代码
    /// </summary>
    /// <param name="tables"></param>
    /// <returns></returns>
    public int GenerateEntityClasses(ViewSchemaCollection views)
    {
        if (views == null || views.Count <= 0)
            return 0;
        int tempIntTableNum = 0;
        CodeTemplate EntityTemplate = GetCodeTemplate("ViewEntity.cst");
        foreach (ViewSchema view in views)
        {
            EntityTemplate.SetProperty("TargetView", view);
            EntityTemplate.SetProperty("EntityNamespace", EntityNamespace);
            EntityTemplate.SetProperty("CreatePerson", CreatePerson);
            EntityTemplate.SetProperty("CompanyName", CompanyName);
            EntityTemplate.SetProperty("FileDesc", "表[" + view.Name + "] 数据库实体代码");
            string tempFilePath = string.Format(@"{0}{1}\{2}", this.OutputDirectory, EntityNamespace, EntityTemplate.GetFileName());
            EntityTemplate.RenderToFile(tempFilePath, true);
            WriteInfo("成功在路径[" + this.OutputDirectory + EntityNamespace + "] 生成 Entity 层代码文件：" + EntityTemplate.GetFileName() + "");
            tempIntTableNum++;
        }
        WriteInfo("-----成功在路径[" + this.OutputDirectory + EntityNamespace + "] 生成 " + tempIntTableNum + " 个 Entity 层代码文件-----");
        return tempIntTableNum;
    }
        
    /// <summary>
    /// 生成数据仓库代码
    /// </summary>
    /// <param name="tables"></param>
    /// <returns></returns>
    public int GenerateBusinessRepositoryClasses(TableSchemaCollection tables)
    {
        if (tables == null || tables.Count <= 0)
            return 0;
        int tempIntTableNum = 0;
        CodeTemplate BusinessRepositoryTemplate = GetCodeTemplate("BusinessRepository.cst");
        foreach (TableSchema table in tables)
        {
            BusinessRepositoryTemplate.SetProperty("TargetTable", table);
            BusinessRepositoryTemplate.SetProperty("CommonNamespace", CommonNamespace);
            BusinessRepositoryTemplate.SetProperty("BusinessRepositoryNamespace", BusinessRepositoryNamespace);
            BusinessRepositoryTemplate.SetProperty("EntityNamespace", EntityNamespace);
            BusinessRepositoryTemplate.SetProperty("DBUtilityNamespace", DBUtilityNamespace);
            BusinessRepositoryTemplate.SetProperty("CreatePerson", CreatePerson);
            BusinessRepositoryTemplate.SetProperty("CompanyName", CompanyName);
            BusinessRepositoryTemplate.SetProperty("BusinessRepositorySuffix", BusinessRepositorySuffix);
            BusinessRepositoryTemplate.SetProperty("FileDesc", "表[" + table.Name + "]的 BusinessReposity 层代码");
            string tempFilePath = string.Format(@"{0}{1}\{2}", this.OutputDirectory, BusinessRepositoryNamespace, BusinessRepositoryTemplate.GetFileName());
            BusinessRepositoryTemplate.RenderToFile(tempFilePath, true);
            WriteInfo("成功在路径[" + this.OutputDirectory + BusinessRepositoryNamespace + "] 生成 BusinessReposity 层代码文件：" + BusinessRepositoryTemplate.GetFileName() + "");
            tempIntTableNum++;
        }
        WriteInfo("-----成功在路径[" + this.OutputDirectory + BusinessRepositoryNamespace + "] 生成：" + tempIntTableNum + " 个 BusinessReposity 层代码文件-----");
        return tempIntTableNum;
    }
    
     /// <summary>
    /// 生成数据仓库代码
    /// </summary>
    /// <param name="tables"></param>
    /// <returns></returns>
    public int GenerateBusinessRepositoryClasses(ViewSchemaCollection views)
    {
        if (views == null || views.Count <= 0)
            return 0;
        int tempIntTableNum = 0;
        CodeTemplate BusinessRepositoryTemplate = GetCodeTemplate("ViewBusinessRepository.cst");
        foreach (ViewSchema view in views)
        {
            BusinessRepositoryTemplate.SetProperty("TargetView", view);
            BusinessRepositoryTemplate.SetProperty("CommonNamespace", CommonNamespace);
            BusinessRepositoryTemplate.SetProperty("BusinessRepositoryNamespace", BusinessRepositoryNamespace);
            BusinessRepositoryTemplate.SetProperty("EntityNamespace", EntityNamespace);
            BusinessRepositoryTemplate.SetProperty("DBUtilityNamespace", DBUtilityNamespace);
            BusinessRepositoryTemplate.SetProperty("CreatePerson", CreatePerson);
            BusinessRepositoryTemplate.SetProperty("CompanyName", CompanyName);
            BusinessRepositoryTemplate.SetProperty("BusinessRepositorySuffix", BusinessRepositorySuffix);
            BusinessRepositoryTemplate.SetProperty("FileDesc", "表[" + view.Name + "]的 数据仓库代码");
            string tempFilePath = string.Format(@"{0}{1}\{2}", this.OutputDirectory, BusinessRepositoryNamespace, BusinessRepositoryTemplate.GetFileName());
            BusinessRepositoryTemplate.RenderToFile(tempFilePath, true);
            WriteInfo("成功在路径[" + this.OutputDirectory + BusinessRepositoryNamespace + "] 生成 BusinessReposity 层代码文件：" + BusinessRepositoryTemplate.GetFileName() + "");
            tempIntTableNum++;
        }
        WriteInfo("-----成功在路径[" + this.OutputDirectory + BusinessRepositoryNamespace + "] 生成：" + tempIntTableNum + " 个 BusinessReposity 层代码文件-----");
        return tempIntTableNum;
    }

    #endregion
    //复制文件夹
    public string CopyDir(string srcPath, string aimPath)
    {
        try
        {
            if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)// 检查目标目录是否以目录分割字符结束如果不是则添加之 
                aimPath += System.IO.Path.DirectorySeparatorChar;
            if (!System.IO.Directory.Exists(aimPath))// 判断目标目录是否存在如果不存在则新建之 
                System.IO.Directory.CreateDirectory(aimPath);
            if (!System.IO.Directory.Exists(srcPath))// 判断目标目录是否存在如果不存在则新建之 
                return "复制失败！目录：[" + srcPath + "]不存在";
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组 
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法 
            // string[] fileList = Directory.GetFiles(srcPath); 
            string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
            // 遍历所有的文件和目录 
            foreach (string file in fileList)
            {
                if (System.IO.Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                else// 否则直接Copy文件 
                    System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
            }
            return "复制目录[" + srcPath + "]到[" + aimPath + "]成功！";
        }
        catch (Exception ex)
        {
            return "复制过程出现异常，详情：" + ex.ToString();
        }
    }
    
    /// <summary>
    /// 复制单个文件
    /// </summary>
    /// <param name="sourceFilePath">源文件路径</param>
    /// <param name="destFilePath">目标文件路径</param>
    /// <returns></returns>
    /// 创建人：朱明明
    /// 创建时间：2014-4-14 15:16
    public string copyFile(string sourceFilePath, string destFilePath)
    {
        if (string.IsNullOrEmpty(sourceFilePath))
            return sourceFilePath + "源文件不存在";
        if (!File.Exists(sourceFilePath))
            return sourceFilePath + "源文件不存在";
        try
        {
            File.Copy(sourceFilePath, destFilePath, true);
            return "复制文件[" + sourceFilePath + "]到[" + destFilePath + "]成功！";
        }
        catch (Exception ex)
        {
            return "复制过程出现异常，详情：" + ex.ToString();
        }

        return string.Empty;
    }

    //获得模版实例
    private CodeTemplate GetCodeTemplate(string TemplateName)
    {
        //CodeTemplate template=null;
        CodeTemplateCompiler compiler = new CodeTemplateCompiler(this.CodeTemplateInfo.DirectoryName + TemplateName);
        compiler.CodeTemplateInfo.ToString();
        compiler.Compile();
        if (compiler.Errors.Count == 0)
        {
            return compiler.CreateInstance();
        }
        else
        {
            System.Text.StringBuilder errorMessage = new System.Text.StringBuilder();
            for (int i = 0; i < compiler.Errors.Count; i++)
            {
                errorMessage.Append(compiler.Errors[i].ToString()).Append("\n");
            }
            throw new ApplicationException(errorMessage.ToString());
        }
    }

    //输出信息
    public void WriteInfo(string paramInfo)
    {
        if (string.IsNullOrEmpty(paramInfo))
            return;
        Debug.WriteLine(paramInfo);
        Response.WriteLine(paramInfo);
    }
}
