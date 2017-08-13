using CessInvest.Models.DataBase;
using CessInvest.Models.Helpers;
using CessInvest.Models.Infrastructure;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CessInvest.Models.Domain.Users
{
    public class UserManager
    {
        private string _lastError = "";
        public string LastError { get { return _lastError; } }
        public static readonly ILog log = LogManager.GetLogger(typeof(UserManager));

        public VM_Users GetUsers(VM_UsersFilters filter, int page = 1)
        {
            VM_Users _users = new VM_Users();
            _users.Filters.Assign(filter);
            _users.PagingInfo.SetData(page, _getUsersTotalCount(filter));
            if (_users.PagingInfo.TotalItems == -1) return null;
            _users.PagingInfo.CurrentPage = page;
            _users.Items = _getUsers(filter, _users.PagingInfo.GetNumberFrom(), _users.PagingInfo.GetNumberTo());
            return _users;
        }
        public VM_User GetUser(int userId, bool allInfo = false)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserView.Params.Id, userId);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    VM_User user = new VM_User();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                for (int j = 0; j < reader.FieldCount; j++)
                                    user.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                            }
                        }
                    }
                    if (allInfo)
                    {
                        //user.ForecastInfo = GetUserProfiletInfo(userId);
                        //if (user.ForecastInfo == null)
                        //    return null;
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время загрузки пользователя!\n" + ex.ToString();
                    log.Error(_lastError);
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }            
        }
        
        #region РЕДАКТИРОВАНИЕ
        public bool UpdateUserProfile(VM_User user)
        {
            try
            {
                SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UpdateUserProfile.Name, GlobalParams.GetConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UpdateUserProfile.Params.UserId, user.Id);
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UpdateUserProfile.Params.Nic, user.Nic.Trim());
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UpdateUserProfile.Params.Name, user.Name.Trim());
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UpdateUserProfile.Params.LastName, 
                    String.IsNullOrEmpty(user.LastName) ? "" : user.LastName.Trim());
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UpdateUserProfile.Params.FatherName,
                    String.IsNullOrEmpty(user.FatherName) ? "" : user.FatherName.Trim());
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UpdateUserProfile.Params.Email, user.Email.Trim());
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UpdateUserProfile.Params.Sex, (int)user.Sex);
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UpdateUserProfile.Params.IsSubscribed, user.IsSubscribed);
                SqlParameter returnValue = new SqlParameter();
                returnValue.DbType = System.Data.DbType.Int32;
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
                returnValue.Value = 1;
                command.Parameters.Add(returnValue);
                command.CommandTimeout = 15;
                lock (GlobalParams._DBAccessLock)
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        if ((int)returnValue.Value == 1)
                        {
                            _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!",
                            DbStruct.PROCEDURES.UpdateUserProfile.Name);
                            log.Error(_lastError);
                            return false;
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}! {1}",
                            DbStruct.PROCEDURES.UpdateUserProfile.Name,
                            ex.ToString());
                        log.Error(_lastError);
                        return false;
                    }
                    finally
                    {
                        if (command != null)
                            command.Dispose();
                    }
                } 
            }
            catch(Exception ex)
            {
                _lastError = ex.ToString();
                log.Error(_lastError);
                return false;
            }
        }
        public bool DeleteUser(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserDelete.Name, GlobalParams.GetConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserDelete.Params.UserId, id);
                SqlParameter returnValue = new SqlParameter();
                returnValue.DbType = System.Data.DbType.Int32;
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
                returnValue.Value = 1;
                command.Parameters.Add(returnValue);
                command.CommandTimeout = 15;
                lock (GlobalParams._DBAccessLock)
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        if ((int)returnValue.Value == 1)
                        {
                            _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!",
                            DbStruct.PROCEDURES.UserDelete.Name);
                            log.Error(_lastError);
                            return false;
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}! {1}",
                            DbStruct.PROCEDURES.UserDelete.Name,
                            ex.ToString());
                        log.Error(_lastError);
                        return false;
                    }
                    finally
                    {
                        if (command != null)
                            command.Dispose();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _lastError = ex.ToString();
                log.Error(_lastError);
                return false;
            }
        }
        #endregion

        #region ЧАСТНЫЕ МЕТОДЫ
        private int _getUsersTotalCount(VM_UsersFilters filter)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UsersCount.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            if(filter.IsActive == EnumBoolType.None)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.IsActive, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.IsActive,
                    filter.IsActive == EnumBoolType.True ? 1 : 0);
            if(String.IsNullOrEmpty(filter.Nic))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Nic, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Nic, filter.Nic);
            if (String.IsNullOrEmpty(filter.Email))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Email, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Email, filter.Email);
            if (String.IsNullOrEmpty(filter.Name))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Name, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Name, filter.Name);
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    object objCount = command.ExecuteScalar();
                    if(objCount == null)
                        return -1;
                    else 
                        return Convert.ToInt32(objCount.ToString());
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время определения общего числа пользователей!\n" + ex.ToString();
                    log.Error(_lastError);
                    return -1;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        private List<VM_UserItem> _getUsers(VM_UsersFilters filter, int from, int to)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UsersView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            if (filter.IsActive == EnumBoolType.None)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.IsActive, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.IsActive, 
                    filter.IsActive == EnumBoolType.True ? 1 : 0);
            if (String.IsNullOrEmpty(filter.Nic))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Nic, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Nic, filter.Nic);
            if (String.IsNullOrEmpty(filter.Email))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Email, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Email, filter.Email);
            if (String.IsNullOrEmpty(filter.Name))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Name, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Name, filter.Name);

            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.From, from);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.To, to);

            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    List<VM_UserItem> items = new List<VM_UserItem>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            VM_UserItem item = null;
                            while (reader.Read())
                            {
                                item = new VM_UserItem();
                                for (int j = 0; j < reader.FieldCount; j++)
                                    item.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                                    items.Add(item);
                            }
                        }
                    }
                    return items;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время загрузки пользователей!\n" + ex.ToString();
                    log.Error(_lastError);
                    return null;
                }
                finally
                {                    
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        #endregion
    }
}