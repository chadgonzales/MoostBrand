using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


/// <summary>
/// Summary description for Holidays
/// </summary>
public class Holidays : DBInterface
{
    #region properties
    public int ID { get; set; }
    public string LocationName { get; set; }

    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string HolidayType { get; set; }
    public int LocationID { get; set; }

    public string UserID { get; set; }


    #endregion

    #region public method
    public DataTable GetAllLocations()
    {
        DataTable dt = this.ExecuteRead("sp_locations_list");
        return dt;
    }

    public int AddUpdateHolidays()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@ID", ID);
            oparam.AddWithValue("@UserID", UserID);
            oparam.AddWithValue("@Date", Date);
            oparam.AddWithValue("@Description", Description);
            oparam.AddWithValue("@HolidayType", HolidayType);
            oparam.AddWithValue("@LocationID", LocationID);

            _rowsAffected = this.ExecuteCUD("sp_holidays_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }

    public DataTable CheckHolidaysIfExists()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@ID", ID);
        oparam.AddWithValue("@Date", Date);
        oparam.AddWithValue("@Description", Description);
        oparam.AddWithValue("@HolidayType", HolidayType);
        oparam.AddWithValue("@LocationID", LocationID);

        DataTable dt = this.ExecuteRead("sp_user_holidays_checkIfExists", oparam);

        return dt;
    }
    public DataTable GetAllHolidays()
    {
        DataTable dt = this.ExecuteRead("sp_user_holidays_list");
        return dt;
    }
    public DataTable GetHolidaysPerDate()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@Date", Date);
        DataTable dt = this.ExecuteRead("sp_user_holidays_getPerDate", oparam);

        return dt;
    }
    #endregion
}