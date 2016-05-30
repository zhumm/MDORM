
///=========================================================================
/// 版    权： Coypright 2012 - 2016 @ 朱明明个人
/// 文件名称： GroupPower_AtdPersonalScheduling.cs
/// 模版作者： 朱明明 最后修改于 2014-3-20 16:10:10
/// 作者邮箱： zhumingming1040@163.com,937553351@qq.com
/// 描    述： 单独生成的代码，请在代码生成后手动添加【描述】信息
/// 创 建 人： 朱明明 (CodeSmith V5.2.2 自动生成的代码 模板V3.0.5)
/// 创建时间： 2016/5/23 15:03:02
///=========================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Common;
using DapperExtensions.Mapper;

namespace Entity
{
    /// <summary>
    /// AtdPersonalScheduling 实体类,包括:属性,构造函数等
    /// </summary>
    /// 创建人：朱明明
    /// 创建时间：2016/5/25 16:05:28
    [Serializable]
    public class AtdPersonalScheduling
    {
        #region 成员变量
        private Guid? _personalScheduID;
        private string _personID;
        private string _year;
        private DateTime? _schedulDate;
        private int? _dateType;
        private string _classID;
        private string _creator;
        private DateTime? _createDate;
        private string _modefier;
        private DateTime? _modifyDate;
        private string _coopCode;
        #endregion

        #region 属性
        /// <summary>
        /// PersonalScheduID
        /// </summary>
        public Guid? PersonalScheduID
        {
            get { return this._personalScheduID; }
            set { this._personalScheduID = value; }
        }

        /// <summary>
        /// PersonID
        /// </summary>
        public string PersonID
        {
            get { return this._personID; }
            set { this._personID = value; }
        }

        /// <summary>
        /// Year
        /// </summary>
        public string Year
        {
            get { return this._year; }
            set { this._year = value; }
        }

        /// <summary>
        /// SchedulDate
        /// </summary>
        public DateTime? SchedulDate
        {
            get { return this._schedulDate; }
            set { this._schedulDate = value; }
        }

        /// <summary>
        /// DateType
        /// </summary>
        public int? DateType
        {
            get { return this._dateType; }
            set { this._dateType = value; }
        }

        /// <summary>
        /// ClassID
        /// </summary>
        public string ClassID
        {
            get { return this._classID; }
            set { this._classID = value; }
        }

        /// <summary>
        /// Creator
        /// </summary>
        public string Creator
        {
            get { return this._creator; }
            set { this._creator = value; }
        }

        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime? CreateDate
        {
            get { return this._createDate; }
            set { this._createDate = value; }
        }

        /// <summary>
        /// Modefier
        /// </summary>
        public string Modefier
        {
            get { return this._modefier; }
            set { this._modefier = value; }
        }

        /// <summary>
        /// ModifyDate
        /// </summary>
        public DateTime? ModifyDate
        {
            get { return this._modifyDate; }
            set { this._modifyDate = value; }
        }

        /// <summary>
        /// CoopCode
        /// </summary>
        public string CoopCode
        {
            get { return this._coopCode; }
            set { this._coopCode = value; }
        }

        #endregion

        #region 扩展的变量、属性、方法
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder temp = new StringBuilder();
            temp.Append("[{ ");
            temp.AppendFormat("\"PersonalScheduID\":\"{0}\", ", this.PersonalScheduID);
            temp.AppendFormat("\"PersonID\":\"{0}\", ", this.PersonID);
            temp.AppendFormat("\"Year\":\"{0}\", ", this.Year);
            temp.AppendFormat("\"SchedulDate\":\"{0}\", ", this.SchedulDate);
            temp.AppendFormat("\"DateType\":\"{0}\", ", this.DateType);
            temp.AppendFormat("\"ClassID\":\"{0}\", ", this.ClassID);
            temp.AppendFormat("\"Creator\":\"{0}\", ", this.Creator);
            temp.AppendFormat("\"CreateDate\":\"{0}\", ", this.CreateDate);
            temp.AppendFormat("\"Modefier\":\"{0}\", ", this.Modefier);
            temp.AppendFormat("\"ModifyDate\":\"{0}\", ", this.ModifyDate);
            temp.AppendFormat("\"CoopCode\":\"{0}\", ", this.CoopCode);
            int lastPos = temp.ToString().LastIndexOf(',');
            if (lastPos != -1)
                temp = temp.Remove(lastPos, 1);
            temp.Append("}]");
            return temp.ToString();
        }
        #endregion
    }

    /// <summary>
    /// GroupPower_AtdPersonalScheduling 实体类,包括:属性,构造函数等
    /// </summary>
    /// 创建人：朱明明
    /// 创建时间：2016/5/23 15:03:02

    [Serializable]
    public class AtdPersonalSchedulingMapper : ClassMapper<AtdPersonalScheduling>
    {
        public AtdPersonalSchedulingMapper()
        {
            base.Table("AtdPersonalScheduling");
            //Map(f=>f.TableName).Ignore();//设置忽略
            //Map(f => f.PrimaryKey).Ignore();//设置忽略
            //Map(f => f.Name).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)           
            AutoMap();
        }
    }
}
