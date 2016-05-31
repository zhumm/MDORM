# VSProject
托管在GitHub上的VS项目

Mini Dapper ORM（迷你DapperORM）。一款基于Dapper ORM扩展的迷你型ORM，有助于提高开发效率。

Dapper是一个轻型的ORM类。代码就一个SqlMapper.cs文件。编译后就一个很小的Dll.

Dapper通过Emit反射IDataReader的序列队列来快速的得到和产生对象。性能很牛逼,速度很快。Dapper的速度接近与IDataReader，取列表的数据超过了DataTable。

Dapper支持Mysql,SqlLite,Mssql等一系列的数据库，当然如果你知道原理也可以让它支持Mongo db.

Dapper没侵入性，想用就用，不想用就不用。无XML无属性。代码以前怎么写现在还怎么写。

Dapper的语法是这样的。语法十分简单。并且无须迁就数据库的设计。

MDORM在Dapper的基础上封装了CRUD操作（(Get, Insert, Update, Delete)），对更高级的查询场景，该类库还提供了一套谓词系统。它的目标是保持POCOs的纯净，不需要额外的attributes和类的继承。使得一些简单的数据库操作可以不用自己写sql语句。使用起来更方面，提高开发效率

详细示例见方案中的MDORM.Test项目

作者：朱明明（Berton）
邮箱：zhumingming1040@163.com、zhumingming1040@hotmail.com
QQ：937553351
