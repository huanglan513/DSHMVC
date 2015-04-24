using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Objects;

namespace DSHOrder.Repository
{

    /// <summary>
    /// 数据容器访问接口
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">待添加的实体实例</param>
        /// <returns>返回添加成功的实例</returns>
        /// <remarks>
        /// 返回的实体和传入的实例是同一个引用
        /// </remarks>
        T Add<T>(T entity, bool isMultipleSave = false) where T : class;

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新的实例</param>
        T Update<T>(T entity, bool isMultipleSave = false) where T : class;

        /// <summary>
        /// 删除一个实例
        /// </summary>
        /// <param name="entity">待删除的实例</param>
        int Delete<T>(T entity, bool isMultipleSave = false) where T : class;

        /// <summary>
        /// 根据条件查询获取一个实例
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>符合条件的实例</returns>
        T GetBy<T>(Expression<Func<T, bool>> condition) where T : class;

        IList<T> GetAll<T>() where T : class;
        IList<T> GetAllBy<T>(Expression<Func<T, bool>> condition) where T : class;

        int SaveChanges();
        /// <summary>
        /// 提供数据源的查询进行计算的功能
        /// </summary>
        /// <returns>数据集的查询集</returns>
        IQueryable<T> CreateQuery<T>() where T : class;

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="where"></param>
        //int Delete<T>(Expression<Func<T, bool>> where) where T : class;

        ///// <summary>
        ///// 批量更新
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="set"></param>
        ///// <param name="where"></param>
        //int Update<T>(Expression<Func<T, T>> set,
        //            Expression<Func<T, bool>> where) where T : class;

        ///// <summary>
        ///// 根据ESql创建一个查询
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="query">ESql</param>
        ///// <param name="params"><seealso cref="ObjectParameter"/></param>
        ///// <returns><see cref="IQueryable"/></returns>
        //IQueryable<T> CreateQuery<T>(string query, params ObjectParameter[] @params) where T : class;

        /// <summary>
        /// 执行一个SP获取返回结果，SP必须在实体模型中注册
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="funName">SP名称</param>
        /// <param name="params">参数</param>
        /// <returns></returns>
        ObjectResult<T> ExecuteFunction<T>(string funName,
                                params ObjectParameter[] @params);

        ///// <summary>
        ///// 执行一个SP获取返回结果，SP必须在实体模型中注册
        ///// </summary>
        ///// <param name="funName">SP名称</param>
        ///// <param name="params">参数</param>
        //int ExecuteFunction(string funName,
        //                        params ObjectParameter[] @params);

        ///// <summary>
        ///// 直接执行Sql语句返回一个结果集
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="script">TSQL 脚本</param>
        ///// <param name="params">参数</param>
        ///// <returns></returns>
        //ObjectResult<T> ExecuteStoreQuery<T>(string script,
        //                       params ObjectParameter[] @params);

        void UpdateEntityWithDelegate<T>(Expression<Func<T, bool>> predicate, Action<T> action) where T : class;

    }


}

