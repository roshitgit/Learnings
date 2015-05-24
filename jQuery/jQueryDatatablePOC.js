<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="PTSReplication.Forms.Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script language="javascript" type="text/javascript">
    /*
        THIS LATEST CODE CHANGES IS THE WORKING SAMPLE WITH THEMEROLLER STYLING SUPPORT
    */
    var oTable;
    function BindOrders(Orders) {
        oTable = $('#datatable_orders').dataTable();
        oTable.fnClearTable();          //Quickly and simply clear a table. works only if "bDestroy" is set to "true"
        BindDataTable(Orders);
        oTable.fnDraw();            // redrawing is very important.
      
    }

    function BindDataTable(Orders) {
        $('#datatable_orders').dataTable({
            "aaData": Orders,           // Bind directly to JSON Object Data source.
            "bServerSide": false,       //Configure DataTables to use server-side processing. Note that the sAjaxSource parameter must also be given in order to give DataTables a source to obtain the required data for each draw.
            "sPaginationType": "full_numbers",
            "bPaginate": true,
            "bProcessing": true,        //Enable or disable the display of a 'processing' indicator when the table is being processed (e.g. a sort).
            "bLengthChange": true,     //Allows the end user to select the size of a formatted page from a select menu (sizes are 10, 25, 50 and 100). Requires pagination (bPaginate).
            "bInfo": true,              //to display Showing {} to {} of {} entries
            "bDestroy": true,
            "iDisplayLength": 10,
            "oLanguage": {
                "oPaginate": {
                    "sFirst": "First page",
                    "sLast": "Last page",
                    "sNext": "Next page",
                    "sPrevious": "Previous page"
                },
                "sEmptyTable": "No data available in table",
                "sInfoEmpty": "No entries to show",
                "sInfoFiltered": " - filtering from _MAX_ records", //on search, displays ...filtering from {} records text at the bottom
                "sProcessing": "Please wait - loading...",
                "sZeroRecords": "No records to display"
            },
            "bSortClasses": false,
            "bStateSave": false,        //Enable or disable state saving. When enabled a cookie will be used to save table display information such as pagination information, display length, filtering and sorting
            "bSort": true,              //Enable or disable sorting of columns.
            "sScrollY": "200px",          //enables vertical scrolling and for setting grid height
            "aoColumnDefs": [
                      { "mDataProp": "OrderID", "aTargets": [0], "sWidth": "20%" },      //here OrderID is actually from JSON data. it is not the column heading. 
                      {"mDataProp": "CustomerID", "aTargets": [1], "sWidth": "20%" },   // specifying the "aTargets" is a must.
                      {"mDataProp": "ShipName", "aTargets": [2], "sWidth": "20%" },
                      { "mDataProp": "ShipCity", "aTargets": [3], "sWidth": "20%" },
                      { "mDataProp": "ShipAddress", "aTargets": [4], "sWidth": "20%" }
                ],
            "bScrollCollapse": true,    //When vertical (y) scrolling is enabled, DataTables will force the height of the table's viewport to the given height at all times (useful for layout). 
            "bScrollAutoCss": true,     //Indicate if DataTables should be allowed to set the padding / margin etc for the scrolling header elements or not. Typically you will want this.
            "bJQueryUI": true,          //Enable jQuery UI ThemeRoller support 
            "bFilter": true,            //enables user to input multiple words (space separated) and will match a row containing those words.
            "aLengthMenu": [[10, 25, 50, 75, 100], [10, 25, 50, 75, 100]], //allows you to readily specify the entries in the length drop down menu that DataTables shows when pagination is enabled
            "bAutoWidth": true,         //automatic column width calculation
            "bDeferRender": true       //provides huge speed boost when you are using an Ajax or JS data source for the table
        });
        $(window).bind('resize', function () {
            oTable.fnAdjustColumnSizing(); //This function will make DataTables recalculate the column sizes, based on the data contained in the table and the sizes applied to the columns (in the DOM, CSS or through the sWidth parameter). 
        });
    }

    $(document).ready(function () {
        $("#<%=btnOrder.ClientID %>").live('click', function (e) {
            e.preventDefault();     // preventing default behavior is very important or else there is unexpected behavior. data does not show up at all times.
            $.ajax({
                url: "../WebServices/JsonService.asmx/GetOrderList",
                type: "GET",
                data: {},
                cache: false,
                async: true,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    var orders = $.parseJSON(result.d); // convert JSON string to JSON Object.
                    BindOrders(orders);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status == 400) {
                        alert("NorthWindService/GetOrderDetails/ Exception" + errorThrown);
                    }
                    else {
                        alert("Request Error NorthWindService/GetOrderDetails/. \njqXHR: " + jqXHR + "\n textStatus: " + textStatus + "\n errorThrown: " + errorThrown);
                    }
                }
            });
        });
    });
    
    </script>
    <asp:UpdatePanel ID="upNorthWind" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnOrder" runat="server" Text="Get Orders" />
            <asp:Panel ID="pnlNorthWind" runat="server">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <legend>Order List from Northwind DB</legend>
                            <fieldset>
                                <table id="datatable_orders" width="100%">
                                    <thead>
                                        <tr>
                                            <th>
                                                Order ID
                                            </th>
                                            <th>
                                                CustomerID
                                            </th>
                                            <th>
                                                Ship Name
                                            </th>
                                            <th>
                                                Ship City
                                            </th>
                                            <th>
                                                Ship Address
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
