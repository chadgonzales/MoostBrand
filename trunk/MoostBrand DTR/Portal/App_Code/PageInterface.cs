using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Inherit this class to easily enable web pages with Repeater control to perform paging and sorting functions.
/// This class inherits System.Web.UI.Page.
/// </summary>
public class PagingIterface : System.Web.UI.Page
{
    #region variables
    IPageable _myObject = null;
    Repeater rptrMyRepeater;
    TextBox txtSearch;
    Label lblShowing;
    DropDownList drpEntries;
    LinkButton lnkPage1, lnkPage2, lnkPage3, lnkPage4, lnkFirst, lnkPrevious, lnkNext, lnkLast;
    string[] custom;

    List<int> lstAvailableDropDownValues = new List<int> { 5, 10, 15, 20, 25, 30 };

    int _defaultItemPerPage = 5;
    #endregion


    #region properties
    public int DefaultItemPerPage
    {
        get { return _defaultItemPerPage; }
        set
        {
            if (lstAvailableDropDownValues.Contains(value))
                _defaultItemPerPage = value;
            else
                throw new ArgumentException("Invalid property value: DefaultItemPerPage");
        }
    }

    public int PageNo
    {
        get { return (int?)ViewState["pageNo"] ?? 1; }
        set { ViewState["pageNo"] = value; }
    }

    private int TotalPage
    {
        get { return (int?)ViewState["TotalPage"] ?? 0; }
        set { ViewState["TotalPage"] = value; }
    }

    private int TotalRows
    {
        get { return (int?)ViewState["totalRows"] ?? 0; }
        set { ViewState["totalRows"] = value; }
    }
    #endregion

    #region public method
    /// <summary>
    /// References the minimum controls needed to perform pagination and sorting.
    /// </summary>
    /// <param name="myObject">An object instance which class implements IPageable</param>
    /// <param name="rptrMyRepeater">The Repeater control</param>
    /// <param name="txtSearch">The search textbox control</param>
    /// <param name="lblShowing">The textbox that shows the number of rows being shown</param>
    /// <param name="drpEntries">The drop down list control that lists the numbers for showing rows</param>
    /// <param name="lnkPage1">Link for pagination</param>
    /// <param name="lnkPage2">Link for pagination</param>
    /// <param name="lnkPage3">Link for pagination</param>
    /// <param name="lnkPage4">Link for pagination</param>
    /// <param name="lnkFirst">Link for pagination</param>
    /// <param name="lnkPrevious">Link for pagination</param>
    /// <param name="lnkNext">Link for pagination</param>
    /// <param name="lnkLast">Link for pagination</param>
    /// <param name="custom">Custom data</param>
    public void ReferenceControls(IPageable myObject, Repeater rptrMyRepeater,
        TextBox txtSearch, Label lblShowing, DropDownList drpEntries,
        LinkButton lnkPage1, LinkButton lnkPage2, LinkButton lnkPage3, LinkButton lnkPage4,
        LinkButton lnkFirst, LinkButton lnkPrevious, LinkButton lnkNext, LinkButton lnkLast,
        params string[] custom)
    {
        _myObject = myObject;
        this.rptrMyRepeater = rptrMyRepeater;
        this.txtSearch = txtSearch;
        this.lblShowing = lblShowing;
        this.drpEntries = drpEntries;
        this.lnkPage1 = lnkPage1;
        this.lnkPage2 = lnkPage2;
        this.lnkPage3 = lnkPage3;
        this.lnkPage4 = lnkPage4;
        this.lnkFirst = lnkFirst;
        this.lnkPrevious = lnkPrevious;
        this.lnkNext = lnkNext;
        this.lnkLast = lnkLast;
        this.custom = custom;

        this.drpEntries.SelectedIndexChanged += drpEntries_SelectedIndexChanged;
        this.lnkPage1.Click += lnkPage1_Click;
        this.lnkPage2.Click += lnkPage2_Click;
        this.lnkPage3.Click += lnkPage3_Click;
        this.lnkPage4.Click += lnkPage4_Click;
        this.lnkFirst.Click += lnkFirst_Click;
        this.lnkPrevious.Click += lnkPrevious_Click;
        this.lnkNext.Click += lnkNext_Click;
        this.lnkLast.Click += lnkLast_Click;
    }

    /// <summary>
    /// Perform the pagination and sorting.
    /// </summary>
    public void Paginate()
    {
        GetTotalPage();
        List();
        SetInitialPageDisplay();
        PagesButtonVisibility();
    }
    public void PaginateDTR()
    {
        GetTotalPageDTR();
        List();
        SetInitialPageDisplay();
        PagesButtonVisibility();
    }
    #endregion

    /// <summary>
    /// Initialize pagination default values such as column and sort ordering. This also start pagination.
    /// </summary>
    public void InitializePagination()
    {
        PopulateDropDownList();

        drpEntries.SelectedValue = _defaultItemPerPage.ToString();

        txtSearch.Text = string.Empty;
        Paginate();
    }
    public void InitializePaginationDTR()
    {
        PopulateDropDownList();

        drpEntries.SelectedValue = _defaultItemPerPage.ToString();

        txtSearch.Text = string.Empty;
        PaginateDTR();
    }
    #region private method
    private void List()
    {
        int _rowPerPage = Convert.ToInt32(drpEntries.SelectedValue);

        int _start = 1;
        int _end = _rowPerPage;

        int _tempStart = PageNo * _rowPerPage;

        if (_tempStart > _rowPerPage)
        {
            _start = (_tempStart - _rowPerPage) + 1;
            _end = _tempStart;
        }

        string _totalEntries = TotalRows.ToString();

        if (Convert.ToInt32(_totalEntries) > _end)
            lblShowing.Text = "Showing " + _start.ToString() + " to " + _end.ToString() + " of " + _totalEntries;
        else
            lblShowing.Text = "Showing " + _start.ToString() + " to " + _totalEntries + " of " + _totalEntries;

        rptrMyRepeater.DataSource = _myObject.ListPerPageDTR(txtSearch.Text, _start, _end, custom);
        rptrMyRepeater.DataBind();
    }

    private void GetTotalPage()
    {
        CheckControlReference();

        int _totalRows = _myObject.TotalRows(txtSearch.Text);
        TotalRows = _totalRows;

        int _totalRowPerPage = Convert.ToInt32(drpEntries.SelectedValue);
        int _totalPage = 0;

        try { _totalPage = _totalRows / _totalRowPerPage; }
        catch { }

        if (_totalPage == 0)
        {
            _totalPage = 1;
        }
        else
        {
            if ((_totalPage * _totalRowPerPage) < _totalRows)
                _totalPage = _totalPage + 1;
        }

        TotalPage = _totalPage;
    }

    private void SetInitialPageDisplay()
    {
        lnkPage1.Text = "1";
        lnkPage2.Text = "2";
        lnkPage3.Text = "3";
        lnkPage4.Text = "4";
        lnkPage1.CssClass = "paginate_active";
        lnkPage2.CssClass = "paginate_button";
        lnkPage3.CssClass = "paginate_button";
        lnkPage4.CssClass = "paginate_button";
    }

    private void PagesButtonVisibility()
    {
        int _totalPage = Convert.ToInt32(TotalPage.ToString());

        if (_totalPage >= Convert.ToInt32(lnkPage2.Text))
            lnkPage2.Visible = true;
        else
            lnkPage2.Visible = false;

        if (_totalPage >= Convert.ToInt32(lnkPage3.Text))
            lnkPage3.Visible = true;
        else
            lnkPage3.Visible = false;

        if (_totalPage >= Convert.ToInt32(lnkPage4.Text))
            lnkPage4.Visible = true;
        else
            lnkPage4.Visible = false;
    }

    private void PopulateDropDownList()
    {
        foreach (int item in lstAvailableDropDownValues)
        {
            if (!drpEntries.Items.Contains(new ListItem(item.ToString())))
            {
                drpEntries.Items.Add(item.ToString());
            }
        }
    }

    private void CheckControlReference()
    {
        if (rptrMyRepeater == null || lblShowing == null || drpEntries == null ||
            lnkPage1 == null || lnkPage2 == null || lnkPage3 == null || lnkPage4 == null ||
            lnkFirst == null || lnkPrevious == null || lnkNext == null || lnkLast == null)
        {
            throw new ArgumentException("The controls are not correctly referenced to PagingInterface class. Have you invoked ReferenceControls() method?");
        }
    }



    private void GetTotalPageDTR()
    {
        CheckControlReference();

        int _totalRows = _myObject.TotalRowsDTR(txtSearch.Text);
        TotalRows = _totalRows;

        int _totalRowPerPage = Convert.ToInt32(drpEntries.SelectedValue);
        int _totalPage = 0;

        try { _totalPage = _totalRows / _totalRowPerPage; }
        catch { }

        if (_totalPage == 0)
        {
            _totalPage = 1;
        }
        else
        {
            if ((_totalPage * _totalRowPerPage) < _totalRows)
                _totalPage = _totalPage + 1;
        }

        TotalPage = _totalPage;
    }
    private void ListDTR()
    {
        int _rowPerPage = Convert.ToInt32(drpEntries.SelectedValue);

        int _start = 1;
        int _end = _rowPerPage;

        int _tempStart = PageNo * _rowPerPage;

        if (_tempStart > _rowPerPage)
        {
            _start = (_tempStart - _rowPerPage) + 1;
            _end = _tempStart;
        }

        string _totalEntries = TotalRows.ToString();

        if (Convert.ToInt32(_totalEntries) > _end)
            lblShowing.Text = "Showing " + _start.ToString() + " to " + _end.ToString() + " of " + _totalEntries;
        else
            lblShowing.Text = "Showing " + _start.ToString() + " to " + _totalEntries + " of " + _totalEntries;

        rptrMyRepeater.DataSource = _myObject.ListPerPageDTR(txtSearch.Text, _start, _end, custom);
        rptrMyRepeater.DataBind();
    }


    #endregion


    #region events

    protected void drpEntries_SelectedIndexChanged(object sender, EventArgs e)
    {
        Paginate();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        PageNo = 1;
        Paginate();
    }

    protected void lnkPage1_Click(object sender, EventArgs e)
    {
        PageNo = Convert.ToInt32(lnkPage1.Text);
        lnkPage1.CssClass = "paginate_active";
        lnkPage2.CssClass = "paginate_button";
        lnkPage3.CssClass = "paginate_button";
        lnkPage4.CssClass = "paginate_button";
        List();
        if ((PageNo - 2) > 1)
        {
            lnkPage1.Text = (PageNo - 2).ToString();
            lnkPage2.Text = (PageNo - 1).ToString();
            lnkPage3.Text = PageNo.ToString();
            lnkPage4.Text = (PageNo + 1).ToString();
            lnkPage1.CssClass = "paginate_button";
            lnkPage2.CssClass = "paginate_button";
            lnkPage3.CssClass = "paginate_active";
            lnkPage4.CssClass = "paginate_button";
        }
        else if ((PageNo - 2) == 1)
        {
            lnkPage1.Text = "1";
            lnkPage2.Text = "2";
            lnkPage3.Text = "3";
            lnkPage4.Text = "4";
            lnkPage1.CssClass = "paginate_button";
            lnkPage2.CssClass = "paginate_button";
            lnkPage3.CssClass = "paginate_active";
            lnkPage4.CssClass = "paginate_button";
        }
        else
        {
            lnkPage1.Text = "1";
            lnkPage2.Text = "2";
            lnkPage3.Text = "3";
            lnkPage4.Text = "4";
            lnkPage1.CssClass = "paginate_active";
            lnkPage2.CssClass = "paginate_button";
            lnkPage3.CssClass = "paginate_button";
            lnkPage4.CssClass = "paginate_button";
        }
        PagesButtonVisibility();
    }
    protected void lnkPage2_Click(object sender, EventArgs e)
    {
        PageNo = Convert.ToInt32(lnkPage2.Text);
        lnkPage1.CssClass = "paginate_button";
        lnkPage2.CssClass = "paginate_active";
        lnkPage3.CssClass = "paginate_button";
        lnkPage4.CssClass = "paginate_button";
        List();
        PagesButtonVisibility();
    }
    protected void lnkPage3_Click(object sender, EventArgs e)
    {
        PageNo = Convert.ToInt32(lnkPage3.Text);
        lnkPage1.CssClass = "paginate_button";
        lnkPage2.CssClass = "paginate_button";
        lnkPage3.CssClass = "paginate_active";
        lnkPage4.CssClass = "paginate_button";
        List();
        PagesButtonVisibility();
    }
    protected void lnkPage4_Click(object sender, EventArgs e)
    {
        PageNo = Convert.ToInt32(lnkPage4.Text);
        lnkPage1.CssClass = "paginate_button";
        lnkPage2.CssClass = "paginate_button";
        lnkPage3.CssClass = "paginate_button";
        lnkPage4.CssClass = "paginate_active";
        List();
        if (Convert.ToInt32(TotalPage.ToString()) > (PageNo + 1))
        {
            lnkPage1.Text = (PageNo - 1).ToString();
            lnkPage2.Text = PageNo.ToString();
            lnkPage3.Text = (PageNo + 1).ToString();
            lnkPage4.Text = (PageNo + 2).ToString();
            lnkPage1.CssClass = "paginate_button";
            lnkPage2.CssClass = "paginate_active";
            lnkPage3.CssClass = "paginate_button";
            lnkPage4.CssClass = "paginate_button";
        }
        else
        {
            if (Convert.ToInt32(TotalPage.ToString()) == (PageNo + 1))
            {
                lnkPage1.Text = (PageNo - 2).ToString();
                lnkPage2.Text = (PageNo - 1).ToString();
                lnkPage3.Text = PageNo.ToString();
                lnkPage4.Text = (PageNo + 1).ToString();
                lnkPage1.CssClass = "paginate_button";
                lnkPage2.CssClass = "paginate_button";
                lnkPage3.CssClass = "paginate_active";
                lnkPage4.CssClass = "paginate_button";
            }
        }
        PagesButtonVisibility();
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        PageNo = 1;
        List();
        GetTotalPage();
        SetInitialPageDisplay();
        PagesButtonVisibility();
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        if ((lnkPage4.CssClass == "paginate_active") && (lnkPage4.Visible))
        {
            PageNo = Convert.ToInt32(lnkPage3.Text);
            List();
            lnkPage1.CssClass = "paginate_button";
            lnkPage2.CssClass = "paginate_button";
            lnkPage3.CssClass = "paginate_active";
            lnkPage4.CssClass = "paginate_button";
        }
        else if ((lnkPage3.CssClass == "paginate_active") && (lnkPage3.Visible))
        {
            PageNo = Convert.ToInt32(lnkPage2.Text);
            List();
            lnkPage1.CssClass = "paginate_button";
            lnkPage2.CssClass = "paginate_active";
            lnkPage3.CssClass = "paginate_button";
            lnkPage4.CssClass = "paginate_button";
        }
        else if ((lnkPage2.CssClass == "paginate_active") && (lnkPage2.Visible))
        {
            lnkPage1_Click(sender, e);
        }
        else
        {
            lnkPage1_Click(sender, e);
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        if ((lnkPage4.CssClass == "paginate_active") && (lnkPage4.Visible))
        {
            lnkPage4_Click(sender, e);
        }
        else if ((lnkPage3.CssClass == "paginate_active") && (lnkPage3.Visible))
        {
            if (lnkPage4.Visible)
            {
                lnkPage4_Click(sender, e);
            }
        }
        else if ((lnkPage2.CssClass == "paginate_active") && (lnkPage2.Visible))
        {
            if (lnkPage3.Visible)
            {
                PageNo = Convert.ToInt32(lnkPage3.Text);
                List();
                lnkPage1.CssClass = "paginate_button";
                lnkPage2.CssClass = "paginate_button";
                lnkPage3.CssClass = "paginate_active";
                lnkPage4.CssClass = "paginate_button";
            }
            else
            {
                PageNo = Convert.ToInt32(lnkPage2.Text);
                List();
                lnkPage1.CssClass = "paginate_button";
                lnkPage2.CssClass = "paginate_active";
                lnkPage3.CssClass = "paginate_active";
                lnkPage4.CssClass = "paginate_button";
            }
        }
        else
        {
            if (lnkPage2.Visible)
            {
                PageNo = Convert.ToInt32(lnkPage2.Text);
                List();
                lnkPage1.CssClass = "paginate_button";
                lnkPage2.CssClass = "paginate_active";
                lnkPage3.CssClass = "paginate_button";
                lnkPage4.CssClass = "paginate_button";
            }
            else
            {
                PageNo = Convert.ToInt32(lnkPage1.Text);
                List();
                lnkPage1.CssClass = "paginate_active";
                lnkPage2.CssClass = "paginate_button";
                lnkPage3.CssClass = "paginate_button";
                lnkPage4.CssClass = "paginate_button";
            }
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        PageNo = Convert.ToInt32(TotalPage.ToString());
        List();
        if (lnkPage4.Visible)
        {
            lnkPage1.Text = (PageNo - 3).ToString();
            lnkPage2.Text = (PageNo - 2).ToString();
            lnkPage3.Text = (PageNo - 1).ToString();
            lnkPage4.Text = PageNo.ToString();
            lnkPage1.CssClass = "paginate_button";
            lnkPage2.CssClass = "paginate_button";
            lnkPage3.CssClass = "paginate_button";
            lnkPage4.CssClass = "paginate_active";
        }
        else if (lnkPage3.Visible)
        {
            lnkPage1.Text = (PageNo - 2).ToString();
            lnkPage2.Text = (PageNo - 1).ToString();
            lnkPage3.Text = PageNo.ToString();
            lnkPage1.CssClass = "paginate_button";
            lnkPage2.CssClass = "paginate_button";
            lnkPage3.CssClass = "paginate_active";
        }
        else if (lnkPage2.Visible)
        {
            lnkPage1.Text = (PageNo - 1).ToString();
            lnkPage2.Text = PageNo.ToString();
            lnkPage1.CssClass = "paginate_button";
            lnkPage2.CssClass = "paginate_active";
        }
        else
        {
            lnkPage1.Text = PageNo.ToString();
            lnkPage1.CssClass = "paginate_active";
        }

    }

    #endregion
}

/// <summary>
/// Implement this interface to allow PagingInterface class to perform paging and sorting.
/// </summary>
public interface IPageable
{
    /// <summary>
    /// A function that returns a datatable based on the pagination and sorting arguments.
    /// </summary>
    /// <param name="_search">The search string wild card pattern</param>
    /// <param name="_start">The starting row number to fetch from the data source</param>
    /// <param name="_end">The ending row number to fetch from the data source</param>
    /// <param name="_orderBy">The column name that is used for sorting</param>
    /// <param name="_sort">The row ordering. It can either be ASC or DESC</param>
    /// <param name="_custom">The custom data that can be used for additional query logic</param>
    /// <returns></returns>
    DataTable ListPerPage(string _search, int _start, int _end, params string[] _custom);

    /// <summary>
    /// A function that returns the number of total rows being returned by the data source based on the search string.
    /// </summary>
    /// <param name="_search">The search string wild card pattern</param>
    /// <param name="_custom">The custom data that can be used for additional query logic</param>
    /// <returns></returns>
    int TotalRows(string _search, params string[] _custom);


    DataTable ListPerPageDTR(string _search, int _start, int _end, params string[] _custom);
    int TotalRowsDTR(string _search, params string[] _custom);
}